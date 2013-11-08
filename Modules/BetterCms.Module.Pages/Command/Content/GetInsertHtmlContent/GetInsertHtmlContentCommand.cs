using System;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.GetInsertHtmlContent
{
    public class GetInsertHtmlContentCommand : CommandBase, ICommand<InsertHtmlContentRequest, PageContentViewModel>
    {
        public virtual IRepository Repository { get; set; }

        public PageContentViewModel Execute(InsertHtmlContentRequest request)
        {
            return new PageContentViewModel
            {
                PageId = Guid.Parse(request.PageId),
                RegionId = Guid.Parse(request.RegionId),
                LiveFrom = DateTime.Today,
                EnableInsetDynamicRegion = Repository.First<Root.Models.Page>(request.PageId.ToGuidOrDefault()).IsMasterPage
            };
        }
    }
}