namespace Bynder.Sdk.Query.Asset
{
    public class MetapropertyOptionsQuery
    {
        /// <summary>
        /// Initializes the class with required information
        /// </summary>
        /// <param name="metapropertyId">The metadata to be modified</param>
        public MetapropertyOptionsQuery(string metapropertyId)
        {
            MetapropertyId = metapropertyId;
        }

        /// <summary>
        /// Id of the media to modify
        /// </summary>
        public string MetapropertyId { get; private set; }

        /// <summary>
        /// Metaproperty option nam
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Maximum number of results.
        /// Default is 50;
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Offset page for results: return the N-th set of limit-results.
        /// Default is 1;
        /// </summary>
        public int Page { get; set; }
    }
}
