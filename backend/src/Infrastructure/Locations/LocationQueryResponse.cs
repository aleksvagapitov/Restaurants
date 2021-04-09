using System.Collections.Generic;

namespace Infrastructure.Locations
{
    public class LocationQueryResponse
    {
        public GeoObjectCollectionType Response { get; set; }
    }

    public class GeoObjectCollectionType
    {
        public GeoObjectEnvelopeType GeoObjectCollection { get; set; }
    }

    public class GeoObjectEnvelopeType
    {
        public MetaDataPropertyType MetaDataProperty { get; set; }
        public ICollection<FeatureMemberType> FeatureMember { get; set; }
    }

    public class MetaDataPropertyType
    {
        public GeocoderResponseMetaDataType GeocoderResponseMetaData { get; set; }
    }

    public class GeocoderResponseMetaDataType
    {
        public string Request { get; set; }
        public string Results { get; set; }
        public string Found { get; set; }
    }

    public class FeatureMemberType
    {
        public GeoObjectType GeoObject { get; set; }
    }

    public class GeoObjectType
    {
        public string Name { get; set; }
        public BoundedByType BoundedBy { get; set; }
        public PointType Point { get; set; }
    }

    public class PointType
    {
        public string Pos { get; set; }
    }

    public class BoundedByType
    {
        public EnvelopeType Envelope { get; set; }
    }

    public class EnvelopeType
    {
        public string LowerCorner { get; set; }
        public string UpperCorner { get; set; }
    }
}