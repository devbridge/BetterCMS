using System.Linq;
using System.Web.Http.OData.Query;

namespace BetterCms.Module.WebApi.Extensions
{
    public static class ODataOptionsExtensions
    {
        public static IQueryable<TModel> ApplyToModels<TModel>(this ODataQueryOptions<TModel> options, IQueryable<TModel> models)
        {
            return options.ApplyTo(models).Cast<TModel>();
        }
    }
}