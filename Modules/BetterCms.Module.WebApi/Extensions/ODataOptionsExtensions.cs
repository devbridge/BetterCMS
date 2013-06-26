using System.Linq;
using System.Web.Http.OData.Query;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;

using NHibernate;

namespace BetterCms.Module.WebApi.Extensions
{
    public static class ODataOptionsExtensions
    {
        private static ODataQuerySettings emptySettings = new ODataQuerySettings();

        public static IQueryable<TModel> ApplyToModels<TModel>(this ODataQueryOptions<TModel> options, IQueryable<TModel> models)
        {
            return options.ApplyTo(models).Cast<TModel>();
        }
        
        public static IQueryable<TModel> ApplyToModels<TModel>(this FilterQueryOption options, IQueryable<TModel> models)
        {
            return options.ApplyTo(models, emptySettings).Cast<TModel>();
        }

        public static DataListResponse<TModel> ToDataListResponse<TModel>(this ODataQueryOptions<TModel> options, IQueryable<TModel> models)
        {
            IFutureValue<int> totalCount;
            if (options.Filter != null)
            {
                totalCount = options.Filter.ApplyToModels(models).ToRowCountFutureValue(options.Top.Value);
            }
            else
            {
                totalCount = models.ToRowCountFutureValue(options.Top.Value);
            }

            models = options.ApplyToModels(models);

            return models.ToDataListResponse(totalCount);
        }
    }
}