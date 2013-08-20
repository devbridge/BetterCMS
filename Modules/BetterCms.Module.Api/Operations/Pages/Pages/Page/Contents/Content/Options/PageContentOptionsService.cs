using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext.Fetching;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options
{
    public class PageContentOptionsService : Service, IPageContentOptionsService
    {
        private readonly IRepository repository;
        
        private readonly IOptionService optionService;

        public PageContentOptionsService(IRepository repository, IOptionService optionService)
        {
            this.repository = repository;
            this.optionService = optionService;
        }

        public GetPageContentOptionsResponse Get(GetPageContentOptionsRequest request)
        {
            var pageContent = repository
                .AsQueryable<PageContent>()
                .Where(f => f.Id == request.PageContentId && !f.IsDeleted && !f.Content.IsDeleted)
                .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                .FetchMany(f => f.Options)
                .ToList()
                .FirstOrDefault();

            var results = optionService
                .GetMergedOptionValuesForEdit(pageContent.Content.ContentOptions, pageContent.Options)
                .Select(o => new OptionModel
                    {
                        Key = o.OptionKey,
                        Value = o.OptionValue,
                        DefaultValue = o.OptionDefaultValue,
                        Type = ((Root.OptionType)(int)o.Type)
                    })
                .AsQueryable()
                .ToDataListResponse(request);

            return new GetPageContentOptionsResponse { Data = results };
        }
    }
}