using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILocationConverter
    {
         Task<LocationInformation> GetLocationForAddress(string address);
    }
}