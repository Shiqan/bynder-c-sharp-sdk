// Copyright (c) Bynder. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Upload
{
    public class SaveMediaQuery
    {
        /// <summary>
        /// Brand id we want to save media to
        /// </summary>
        [ApiField("brandid")]
        public string BrandId { get; set; }

        /// <summary>
        /// Name of the asset.
        /// </summary>
        [ApiField("name")]
        public string FileName { get; set; }

        /// <summary>
        /// Copyright information of the asset.
        /// </summary>
        [ApiField("copyright")]
        public string Copyright { get; set; }

        /// <summary>
        /// Description of the asset.
        /// </summary>
        [ApiField("description")]
        public string Description { get; set; }

        /// <summary>
        /// Flags if the asset should be Public.
        /// </summary>
        [ApiField("isPublic")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Date/time of publication in ISO08601-format.
        /// </summary>
        [ApiField("ISOPublicationDate", Converter = typeof(DateTimeOffsetConverter))]
        public DateTimeOffset? PublicationDate { get; set; }

        /// <summary>
        /// Media id. If specified it will add the asset as new version
        /// of the specified media. Otherwise a new media will be added to 
        /// the asset bank
        /// </summary>
        public string MediaId { get; set; }


        [ApiField("tags", Converter = typeof(ListConverter))]
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Metaproperty options to set on the asset.
        /// </summary>
        [ApiField("metaproperty.", Converter = typeof(MetapropertyOptionsConverter))]
        public IDictionary<string, IList<string>> MetapropertyOptions { get; set; } = new Dictionary<string, IList<string>>();

        /// <summary>
        /// Add a set of options to a metaproperty
        /// </summary>
        /// <param name="metapropertyId">metaproperty ID</param>
        /// <param name="optionIds">set of options</param>
        public void AddMetapropertyOptions(string metapropertyId, IList<string> optionIds)
        {
            MetapropertyOptions.Add(metapropertyId, optionIds);
        }

    }
}
