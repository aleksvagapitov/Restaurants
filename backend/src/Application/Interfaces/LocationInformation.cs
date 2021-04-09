using NetTopologySuite.Geometries;

namespace Application.Interfaces
{
    public class LocationInformation
    {
        public Point Location {get; set;}
        public string StreetAddress { get; set; }
    }
}