using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRazorViewToStringRenderer
    {
         Task<string> RenderViewToStringAsync<TModel> (EmailTemplate template, TModel model);
    }
}