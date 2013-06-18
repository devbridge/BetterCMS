using System.Linq;
using System.Web.Http.OData.Query;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;

using NHibernate;

namespace BetterCms.Module.WebApi.Extensions
{
    public static class IQueryableExtensions
    {
        public static DataListResponse<TModel> ToDataListResponse<TModel>(this IQueryable<TModel> models, ODataQueryOptions<TModel> options)
        {
            IFutureValue<int> totalCount;
            if (options.Top != null && options.Top.Value > 0)
            {
                if (options.Filter != null)
                {
                    totalCount = options.Filter.ApplyToModels(models).ToRowCountFutureValue(options.Top.Value);
                }
                else
                {
                    totalCount = models.ToRowCountFutureValue(options.Top.Value);
                }
            }
            else
            {
                totalCount = null;
            }

            models = options.ApplyToModels(models);

            return models.ToDataListResponse(totalCount);
        }
    }
}