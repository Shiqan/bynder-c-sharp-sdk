// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using Bynder.Sdk.Api.Requests;
using Bynder.Sdk.Api.RequestSender;
using Bynder.Sdk.Model.Upload;
using Bynder.Sdk.Query.Upload;
using Bynder.Sdk.Utils;

namespace Bynder.Sdk.Service.Upload
{
    /// <summary>
    /// Class used to upload files to Bynder
    /// </summary>
    internal class FileUploader
    {
        /// <summary>
        /// Max chunk size
        /// </summary>
        private readonly int _chunkSize;

        /// <summary>
        /// Request sender used to call Bynder API.
        /// </summary>
        private readonly IApiRequestSender _requestSender;

        readonly IFileSystem _fileSystem;

        // <summary>Create FileUploader with values injected for testing.</summary>
        internal FileUploader(IApiRequestSender requestSender, IFileSystem fileSystem, int chunkSize)
        {
            _requestSender = requestSender;
            _fileSystem = fileSystem;
            _chunkSize = chunkSize;
        }

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="requestSender">Request sender to communicate with Bynder API</param>
        internal FileUploader(IApiRequestSender requestSender) : this(
            requestSender,
            fileSystem: new FileSystem(),
            chunkSize: 1024 * 1024 * 5
        )
        { }

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="path">path to the file to be uploaded</param>
        /// <param name="brandId">Brand ID to save the asset to.</param>
        /// <returns>Information about the created asset</returns>
        [Obsolete]
        internal async Task<SaveMediaResponse> UploadFileToNewAssetAsync(string path, string brandId, IList<string> tags)
        {
            var fileId = await UploadFileAsync(path);
            return await SaveMediaAsync(fileId, new SaveMediaQuery
            {
                BrandId = brandId,
                FileName = Path.GetFileName(path),
                Tags = tags
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="fileName">name of the file to be uploaded</param>
        /// <param name="fileStream">stream of the file to be uploaded</param>
        /// <param name="brandId">Brand ID to save the asset to.</param>
        /// <returns>Information about the created asset</returns>
        [Obsolete]
        internal async Task<SaveMediaResponse> UploadFileToNewAssetAsync(string fileName, Stream fileStream, string brandId, IList<string> tags)
        {
            var fileId = await UploadFileAsync(fileName, fileStream); 
            return await SaveMediaAsync(fileId, new SaveMediaQuery
            {
                BrandId = brandId,
                FileName = fileName,
                Tags = tags
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="path">path to the file to be uploaded</param>
        /// <param name="query">Information about tag which will be set to media files</param>
        /// <returns>Information about the created asset</returns>
        internal async Task<SaveMediaResponse> UploadFileToNewAssetAsync(string path, SaveMediaQuery query)
        {
            var fileId = await UploadFileAsync(path);
            if (string.IsNullOrEmpty(query.FileName))
            {
                query.FileName = Path.GetFileName(path);
            }
            return await SaveMediaAsync(fileId, query).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="fileStream">stream of the file to be uploaded</param>
        /// <param name="query">stream of the file to be uploaded</param>
        /// <returns>Information about the created asset</returns>
        internal async Task<SaveMediaResponse> UploadFileToNewAssetAsync(Stream fileStream, SaveMediaQuery query)
        {
            var fileId = await UploadFileAsync(query.FileName, fileStream);
            return await SaveMediaAsync(fileId, query).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new version of an existing asset.
        /// </summary>
        /// <param name="path">path to the file to be uploaded</param>
        /// <param name="mediaId">Asset ID for which to save the new version.</param>
        /// <returns>Information about the created asset</returns>
        internal async Task<SaveMediaResponse> UploadFileToExistingAssetAsync(string path, string mediaId)
        {
            var fileId = await UploadFileAsync(path);
            return await SaveMediaAsync(mediaId, fileId).ConfigureAwait(false);
        }

        internal async Task<SaveMediaResponse> UploadFileToExistingAssetAsync(string fileName, Stream stream, string mediaId)
        {
            var fileId = await UploadFileAsync(fileName, stream);
            return await SaveMediaAsync(mediaId, fileId).ConfigureAwait(false);
        }

        private async Task<string> UploadFileAsync(string fileName, Stream fileStream)
        {
            // Prepare the upload to retrieve the file ID.
            var fileId = await PrepareAsync().ConfigureAwait(false);

            await UploadAsync(fileId, fileName, fileStream);

            return fileId;
        }

        private async Task<string> UploadFileAsync(string path)
        {
            using (var fileStream = _fileSystem.File.OpenRead(path))
            {
                return await UploadFileAsync(Path.GetFileName(path), fileStream);
            }
        }

        private async Task UploadAsync(string fileId, string fileName, Stream fileStream)
        {
            using (var reader = new BinaryReader(fileStream))
            {
                int chunksUploaded = 0;

                // Upload the file divided in chunks.
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    await UploadChunkAsync(
                        fileId,
                        chunksUploaded++,
                        reader.ReadBytes(_chunkSize)
                    ).ConfigureAwait(false);
                }

                // Finalize the upload.
                await FinalizeAsync(
                    fileId,
                    chunksUploaded,
                    fileName,
                    fileStream
                ).ConfigureAwait(false);
            }
        }

        #region API calls

        private async Task<string> PrepareAsync()
        {
            var response = await _requestSender.SendRequestAsync(
                new ApiRequest<PrepareUploadResponse>
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = "/v7/file_cmds/upload/prepare",
                }
            ).ConfigureAwait(false);
            return response.FileId;
        }

        private async Task UploadChunkAsync(string fileId, int chunkNumber, byte[] chunk)
        {
            await _requestSender.SendRequestAsync(
                new ApiRequest
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = $"/v7/file_cmds/upload/{fileId}/chunk/{chunkNumber}",
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-SHA256", SHA256Utils.SHA256hex(chunk) }
                    },
                    BinaryContent = chunk,
                }
            ).ConfigureAwait(false);
        }

        private async Task FinalizeAsync(string fileId, int chunksUploaded, string filename, Stream fileStream)
        {
            await _requestSender.SendRequestAsync(
                new ApiRequest
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = $"/v7/file_cmds/upload/{fileId}/finalise_api",
                    Query = new FinalizeUploadQuery
                    {
                        ChunksCount = chunksUploaded,
                        Filename = filename,
                        FileSize = fileStream.Length,
                        SHA256 = SHA256Utils.SHA256hex(fileStream),
                    },
                }
            ).ConfigureAwait(false);
        }

        private async Task<SaveMediaResponse> SaveMediaAsync(string fileId, SaveMediaQuery query)
        {
            return await _requestSender.SendRequestAsync(
                new ApiRequest<SaveMediaResponse>
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = $"/api/v4/media/save/{fileId}",
                    Query = query
                }
            ).ConfigureAwait(false);
        }

        private async Task<SaveMediaResponse> SaveMediaAsync(string mediaId, string fileId)
        {
            return await _requestSender.SendRequestAsync(
                new ApiRequest<SaveMediaResponse>
                {
                    HTTPMethod = HttpMethod.Post,
                    Path = $"/api/v4/media/{mediaId}/save/{fileId}",
                }
             ).ConfigureAwait(false);
        }

        #endregion

    }
}
