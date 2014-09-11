using System;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.GetInsertHtmlContent
{
    public class GetInsertHtmlContentCommand : CommandBase, ICommand<InsertHtmlContentRequest, PageContentViewModel>
    {
        private readonly IRepository repository;

        private readonly IMasterPageService masterPageService;

        public GetInsertHtmlContentCommand(IRepository repository, IMasterPageService masterPageService)
        {
            this.masterPageService = masterPageService;
            this.repository = repository;
        }

        public PageContentViewModel Execute(InsertHtmlContentRequest request)
        {
            var isMasterPage = repository.First<Root.Models.Page>(request.PageId.ToGuidOrDefault()).IsMasterPage;
            var parentPageContentId = request.ParentPageContentId != null ? request.ParentPageContentId.ToGuidOrDefault() : Guid.Empty;

            var model = new PageContentViewModel
                {
                    PageId = Guid.Parse(request.PageId),
                    RegionId = Guid.Parse(request.RegionId),
                    ParentPageContentId = parentPageContentId,
                    LiveFrom = DateTime.Today,
                    EnableInsertDynamicRegion = isMasterPage,
                    EditInSourceMode = isMasterPage,
                    CanEditContent = true
                };

            if (model.EnableInsertDynamicRegion)
            {
                model.LastDynamicRegionNumber = masterPageService.GetLastDynamicRegionNumber(model.PageId);
            }

            return model;
        }
    }
}