using System;
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Content.SavePageHtmlContent
{
    public class SavePageHtmlContentCommand : CommandBase, ICommand<SavePageHtmlContentCommandRequest, ChangedContentResultViewModel>
    {
        private readonly IContentService contentService;
        
        private readonly ICmsConfiguration configuration;
        
        private readonly IOptionService optionsService;
        
        private readonly IWidgetService widgetService;

        public SavePageHtmlContentCommand(IContentService contentService, ICmsConfiguration configuration, 
            IOptionService optionsService, IWidgetService widgetService)
        {
            this.contentService = contentService;
            this.configuration = configuration;
            this.optionsService = optionsService;
            this.widgetService = widgetService;
        }

        public ChangedContentResultViewModel Execute(SavePageHtmlContentCommandRequest request)
        {
            var model = request.Content;
            if (model.DesirableStatus == ContentStatus.Published)
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.PublishContent);
            }

            if (model.Id == default(Guid) || model.DesirableStatus != ContentStatus.Published)
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }

            PageContent pageContent;
            var isNew = model.Id.HasDefaultValue();
            if (!isNew)
            {
                var query = Repository
                    .AsQueryable<PageContent>()
                    .Where(f => f.Id == model.Id && !f.IsDeleted)
                    .AsQueryable();

                if (!model.IsUserConfirmed)
                {
                    query = query.Fetch(f => f.Page);
                }

                if (configuration.Security.AccessControlEnabled)
                {
                    query = query.Fetch(f => f.Page).ThenFetchMany(f => f.AccessRules);
                }

                pageContent = query.ToList().FirstOne();

                // Check if user has confirmed the deletion of content
                if (!model.IsUserConfirmed && pageContent.Page.IsMasterPage)
                {
                    contentService.CheckIfContentHasDeletingChildrenWithException(model.PageId, model.ContentId, model.PageContent);
                }
            }
            else
            {              
                pageContent = new PageContent();
                pageContent.Order = contentService.GetPageContentNextOrderNumber(model.PageId, model.ParentPageContentId);

                if (configuration.Security.AccessControlEnabled)
                {
                    pageContent.Page = Repository
                        .AsQueryable<Root.Models.Page>(p => p.Id == model.PageId)
                        .FetchMany(p => p.AccessRules)
                        .ToList()
                        .FirstOne();
                }
            }

            // Demand access
            if (configuration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(pageContent.Page, Context.Principal, AccessLevel.ReadWrite);
            }

            // Get page as proxy, if page is not retrieved yet
            if (!configuration.Security.AccessControlEnabled)
            {
                pageContent.Page = Repository.AsProxy<Root.Models.Page>(model.PageId);
            }
            pageContent.Region = Repository.AsProxy<Region>(model.RegionId);
            if (!model.ParentPageContentId.HasDefaultValue())
            {
                pageContent.Parent = Repository.AsProxy<PageContent>(model.ParentPageContentId);
            }
            else
            {
                pageContent.Parent = null;
            }

            var contentToSave = new HtmlContent
                {
                    Id = model.ContentId,
                    ActivationDate = model.LiveFrom,
                    ExpirationDate = TimeHelper.FormatEndDate(model.LiveTo),
                    Name = model.ContentName,
                    Html = model.PageContent ?? string.Empty,
                    UseCustomCss = model.EnabledCustomCss,
                    CustomCss = model.CustomCss,
                    UseCustomJs = model.EnabledCustomJs,
                    CustomJs = model.CustomJs,
                    EditInSourceMode = model.EditInSourceMode,
                    ContentTextMode = ContentTextMode.Html
                };

            if (model.ContentTextMode == ContentTextMode.Markdown)
            {
                if (!string.IsNullOrWhiteSpace(model.PageContent))
                {
                    contentToSave.Html = MarkdownConverter.ToHtml(model.PageContent);
                }
                else
                {
                    contentToSave.Html = string.Empty;
                }
                contentToSave.ContentTextMode = ContentTextMode.Markdown;
                contentToSave.OriginalText = model.PageContent;
            }
            
            if (model.ContentTextMode == ContentTextMode.SimpleText)
            {
                if (!string.IsNullOrWhiteSpace(model.PageContent))
                {
                    contentToSave.Html = HttpUtility.HtmlEncode(model.PageContent);
                }
                else
                {
                    contentToSave.Html = string.Empty;
                }
                contentToSave.ContentTextMode = ContentTextMode.SimpleText;
                contentToSave.OriginalText = model.PageContent;
            }

            // Preserve content if user is not authorized to change it.
            if (!SecurityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent) && model.Id != default(Guid))
            {
                var originalContent = Repository.First<HtmlContent>(model.ContentId);
                var contentToPublish = (HtmlContent)(originalContent.History != null
                    ? originalContent.History.FirstOrDefault(c => c.Status == ContentStatus.Draft) ?? originalContent
                    : originalContent);

                contentToSave.Name = contentToPublish.Name;
                contentToSave.Html = contentToPublish.Html;
                contentToSave.UseCustomCss = contentToPublish.UseCustomCss;
                contentToSave.CustomCss = contentToPublish.CustomCss;
                contentToSave.UseCustomJs = contentToPublish.UseCustomJs;
                contentToSave.CustomJs = contentToPublish.CustomJs;
                contentToSave.EditInSourceMode = contentToPublish.EditInSourceMode;
            }

            UnitOfWork.BeginTransaction();
            pageContent.Content = contentService.SaveContentWithStatusUpdate(
                contentToSave,
                model.DesirableStatus);
            optionsService.SaveChildContentOptions(pageContent.Content, request.ChildContentOptionValues, model.DesirableStatus);

            if (pageContent.Content.ContentRegions != null 
                && pageContent.Content.ContentRegions.Count > 0)
            {
                if (!pageContent.Page.IsMasterPage)
                {
                    var logMessage = string.Format("Dynamic regions are not allowed. Page: {0}, Content: {1}", pageContent.Page, pageContent.Id);
                    throw new ValidationException(() => PagesGlobalization.SaveContent_DynamicRegionsAreNotAllowed_Message, logMessage);
                }
            }

            Repository.Save(pageContent);
            UnitOfWork.Commit();

            // Notify.
            if (model.DesirableStatus != ContentStatus.Preview)
            {
                if (isNew)
                {
                    Events.PageEvents.Instance.OnHtmlContentCreated((HtmlContent)pageContent.Content);
                    Events.PageEvents.Instance.OnPageContentInserted(pageContent);
                }
                else
                {
                    Events.PageEvents.Instance.OnHtmlContentUpdated((HtmlContent)pageContent.Content);
                }
            }

            var contentData = (pageContent.Content.History != null
                    ? pageContent.Content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft) ?? pageContent.Content
                    : pageContent.Content);

            var response = new ChangedContentResultViewModel
                {
                    PageContentId = pageContent.Id,
                    ContentId = contentData.Id,
                    RegionId = pageContent.Region.Id,
                    PageId = pageContent.Page.Id,
                    DesirableStatus = request.Content.DesirableStatus,
                    Title = contentData.Name,
                    ContentVersion = contentData.Version,
                    PageContentVersion = pageContent.Version,
                    ContentType = HtmlContentAccessor.ContentWrapperType
                };

            if (request.Content.IncludeChildRegions)
            {
                response.Regions = widgetService.GetWidgetChildRegionViewModels(contentData);
            }

            return response;
        }
    }
}