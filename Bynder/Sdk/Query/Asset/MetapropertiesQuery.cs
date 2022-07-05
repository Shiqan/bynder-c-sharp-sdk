namespace Bynder.Sdk.Query.Asset
{
    /// <summary>
    /// Query to retrieve Metaproperties
    /// </summary>
    public class MetapropertiesQuery
    {
        /// <summary>
        /// Indicates whether or not the response should include asset count results for metaproperty-options.
        /// </summary>
        public bool Count { get; set; } = false;

        /// <summary>
        /// ndicates whether or not the response should include the metaproperty options of each metaproperty
        /// </summary>
        public bool Options { get; set; } = true;
    }
}
