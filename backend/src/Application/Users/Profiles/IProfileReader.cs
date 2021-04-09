using System.Threading.Tasks;

namespace Application.Users.Profiles
{
    public interface IProfileReader
    {
         Task<Profile> ReadProfile(string username);
    }
}