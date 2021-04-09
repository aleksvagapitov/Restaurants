using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IPhotoAccessor
    {
         PhotoUploadResult AddPhoto(IFormFile file);
         bool DeletePhoto(string photoId);
    }
}