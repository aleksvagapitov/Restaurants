using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;

namespace Infrastructure.Locations
{
    public class LocationConverter : ILocationConverter
    {
        private readonly LocationApiParameters _locationApiParameters;

        public LocationConverter(IOptions<LocationApiParameters> locationApiParameters)
        {
            _locationApiParameters = locationApiParameters.Value;
        }
        public async Task<LocationInformation> GetLocationForAddress(string address)
        {
            var myJsonObject = await _locationApiParameters.BaseUrl
                .AppendPathSegment(_locationApiParameters.ApiVersion)
                .SetQueryParams(new
                {
                    lang = _locationApiParameters.Lang,
                    format = _locationApiParameters.Format,
                    apikey = _locationApiParameters.ApiKey,
                    geocode = address,
                })
                .GetJsonAsync<LocationQueryResponse>();

            var geoObjects = myJsonObject.Response.GeoObjectCollection.FeatureMember;

            var positions = geoObjects.Select(x => x.GeoObject.Point.Pos).FirstOrDefault()
                                .Split(' ').Where(x => double.TryParse(x, out _)).Select(double.Parse)
                                .ToList();
        
            return new LocationInformation {
                Location = new Point(positions[0], positions[1]){
                    SRID = 4326
                },
                StreetAddress = geoObjects.Select(x => x.GeoObject.Name).ToString()
            };
        }
    }
}