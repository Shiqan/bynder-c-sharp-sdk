// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bynder.Sdk.Model;
using Bynder.Sdk.Model.Upload;
using Bynder.Sdk.Query.Asset;
using Bynder.Sdk.Query.Upload;

namespace Bynder.Sdk.Service.Asset
{
    /// <summary>
    /// Interface to represent operations that can be done to the Bynder Asset Bank
    /// </summary>
    public interface IAssetService
    {
        /// <summary>
        /// Gets Brands Async
        /// </summary>
        /// <returns>Task with list of brands</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<Brand>> GetBrandsAsync();

        /// <summary>
        /// Gets the download file Url for specific fileItemId. If mediaItemId was not specified, 
        /// it will return the download Url of the media specified by mediaId
        /// </summary>
        /// <param name="query">information with the media we want to get the Url from</param>
        /// <returns>Task that contains the Uri of the media Item</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Uri> GetDownloadFileUrlAsync(DownloadMediaQuery query);

        /// <summary>
        /// Gets a dictionary of the metaproperties async. The key of the dictionary
        /// returned is the name of the metaproperty.
        /// </summary>
        /// <returns>Task with dictionary of metaproperties</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IDictionary<string, Metaproperty>> GetMetapropertiesAsync();

        /// <summary>
        /// Retrieve specific Metaproperty
        /// </summary>
        /// <param name="query">query containing the metaproperty ID</param>
        /// <returns>Structure representing the metaproperty</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Metaproperty> GetMetapropertyAsync(MetapropertyQuery query);

        /// <summary>
        /// Retrieve metaproperty dependencies
        /// </summary>
        /// <param name="query">iquery containing the metaproperty ID</param>
        /// <returns>List of the metaproperty's dependencies</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<string>> GetMetapropertyDependenciesAsync(MetapropertyQuery query);

        /// <summary>
        /// Gets all the information for a specific mediaId. This is needed 
        /// to get the media items of a media.
        /// </summary>
        /// <param name="query">Information about the media we want to get the information of.</param>
        /// <returns>Task with the Media information</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Media> GetMediaInfoAsync(MediaInformationQuery query);

        /// <summary>
        /// Gets a list of media using query information. The media information is not complete, for example
        /// media items for media returned are not present. For that client needs to call <see cref="RequestMediaInfoAsync(string)"/>
        /// </summary>
        /// <param name="query">information to correctly filter/paginate media</param>
        /// <returns>Task with List of media.</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<Media>> GetMediaListAsync(MediaQuery query);

        /// <summary>
        /// Modifies a media
        /// </summary>
        /// <param name="query">Information needed to modify a media</param>
        /// <returns>Task</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> ModifyMediaAsync(ModifyMediaQuery query);

        /// <summary>
        /// Retrieve tags
        /// </summary>
        /// <param name="query">Filters for searching tags</param>
        /// <returns>Task with list of tags</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<IList<Tag>> GetTagsAsync(GetTagsQuery query);

        /// <summary>
        /// Add tag to assets
        /// </summary>
        /// <param name="query">Information about tag which will be set to media files</param>
        /// <returns>Task representing the upload</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> AddTagToMediaAsync(AddTagToMediaQuery query);

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="path">path to the file to be uploaded</param>
        /// <param name="brandId">brand ID to save the asset to</param>
        /// <param name="tags">tags that will be added on the asset</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        [Obsolete("Use UploadFileToNewAssetAsync(Stream fileStream, SaveMediaQuery query) instead")]
        Task<SaveMediaResponse> UploadFileToNewAssetAsync(string path, string brandId, IList<string> tags = default);

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="fileName">file name of the file to be uploaded</param>
        /// <param name="fileStream">stream of the file to be uploaded</param>
        /// <param name="brandId">brand ID to save the asset to</param>
        /// <param name="tags">tags that will be added on the asset</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        [Obsolete("Use UploadFileToNewAssetAsync(Stream fileStream, SaveMediaQuery query) instead")]
        Task<SaveMediaResponse> UploadFileToNewAssetAsync(string fileName, Stream fileStream, string brandId, IList<string> tags = default);

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="path">path to the file to be uploaded</param>
        /// <param name="query">Information about tag which will be set to media files</param>
        /// <returns><see cref="SaveMediaResponse"/></returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<SaveMediaResponse> UploadFileToNewAssetAsync(string path, SaveMediaQuery query);

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new asset.
        /// </summary>
        /// <param name="fileStream">stream of the file to be uploaded</param>
        /// <param name="query">Information about tag which will be set to media files</param>
        /// <returns><see cref="SaveMediaResponse"/></returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<SaveMediaResponse> UploadFileToNewAssetAsync(Stream fileStream, SaveMediaQuery query);

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new version of an existing asset.
        /// </summary>
        /// <param name="path">path to the file to be uploaded</param>
        /// <param name="mediaId">Asset ID for which to save the new version.</param>
        /// <returns>Information about the created asset</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<SaveMediaResponse> UploadFileToExistingAssetAsync(string path, string mediaId);

        /// <summary>
        /// Uploads a file to Bynder to be stored as a new version of an existing asset.
        /// </summary>
        /// <param name="fileName">file name of the file to be uploaded</param>
        /// <param name="fileStream">stream of the file to be uploaded</param>
        /// <param name="mediaId">Asset ID for which to save the new version.</param>
        /// <returns>Information about the created asset</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<SaveMediaResponse> UploadFileToExistingAssetAsync(string fileName, Stream fileStream, string mediaId);

        /// <summary>
        /// Create an asset usage operation to track usage of Bynder assets in third party applications.
        /// </summary>
        /// <param name="query">Information about the asset usage</param>
        /// <returns>Task representing the operation</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> CreateAssetUsage(AssetUsageQuery query);

        /// <summary>
        /// Delete an asset usage operation to track usage of Bynder assets in third party applications.
        /// </summary>
        /// <param name="query">Information about the asset usage</param>
        /// <returns>Task representing the operation</returns>
        /// <exception cref="HttpRequestException">Can be thrown when requests to server can't be completed or HTTP code returned by server is an error</exception>
        Task<Status> DeleteAssetUsage(AssetUsageQuery query);

    }
}
