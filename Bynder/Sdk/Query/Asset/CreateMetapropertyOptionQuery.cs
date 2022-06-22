using Bynder.Sdk.Api.Converters;
using Bynder.Sdk.Query.Decoder;

namespace Bynder.Sdk.Query.Asset
{
    internal class CreateMetapropertyOptionsQueryData
    {
        public CreateMetapropertyOptionsQueryData(CreateMetapropertyOptionsQuery query)
        {
            Query = query;
        }

        [ApiField("data", Converter = typeof(JsonConverter))]
        public CreateMetapropertyOptionsQuery Query { get; private set; }
    }

    public class CreateMetapropertyOptionsQuery
    {
        /// <summary>
        /// Initializes the class with required information
        /// </summary>
        public CreateMetapropertyOptionsQuery(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of the (metaproperty) option, should be alphanumeric only. Cannot be modified after the metaproperty has been created.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Human-readable label. If no label is set, the name will be used.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Sorting order number.
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// Indicating whether or not the (metaproperty) option can be selected when saving/editing assets.
        /// </summary>
        public bool IsSelectable { get; set; }

        /// <summary>
        /// Id of the parent option this option should be a child of.
        /// </summary>
        public string ParentId { get; set; }
    }
}
