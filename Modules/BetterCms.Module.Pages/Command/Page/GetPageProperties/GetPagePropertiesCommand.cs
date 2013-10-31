using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using ITagService = BetterCms.Module.Pages.Services.ITagService;

namespace BetterCms.Module.Pages.Command.Page.GetPageProperties
{
    /// <summary>
    /// Command for getting view model with page properties
    /// </summary>
    public class GetPagePropertiesCommand : CommandBase, ICommand<Guid, EditPagePropertiesViewModel>
    {
        /// <summary>
        /// The category service
        /// </summary>
        private ICategoryService categoryService;

        /// <summary>
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;
        
        /// <summary>
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;
        
        /// <summary>
        /// The layout service
        /// </summary>
        private readonly ILayoutService layoutService;

        /// <summary>
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagePropertiesCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="categoryService">The category service.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="layoutService">The layout service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        public GetPagePropertiesCommand(ITagService tagService, ICategoryService categoryService, IOptionService optionService,
            ICmsConfiguration cmsConfiguration, ILayoutService layoutService, IMediaFileUrlResolver fileUrlResolver)
        {
            this.tagService = tagService;
            this.categoryService = categoryService;
            this.optionService = optionService;
            this.cmsConfiguration = cmsConfiguration;
            this.layoutService = layoutService;
            this.fileUrlResolver = fileUrlResolver;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="id">The page id.</param>
        /// <returns></returns>
        public EditPagePropertiesViewModel Execute(Guid id)
        {            
            var model = Repository
                .AsQueryable<PageProperties>()
                .Where(p => p.Id == id)
                .Select(page =>
                    new
                    {
                        Model = new EditPagePropertiesViewModel
                          {
                              Id = page.Id,
                              Version = page.Version,
                              PageName = page.Title,
                              PageUrl = page.PageUrl,
                              PageCSS = page.CustomCss,
                              PageJavascript = page.CustomJS,
                              UseNoFollow = page.UseNoFollow,
                              UseNoIndex = page.UseNoIndex,
                              UseCanonicalUrl = page.UseCanonicalUrl,
                              IsPagePublished = page.Status == PageStatus.Published,
                              IsInSitemap = page.NodeCountInSitemap > 0,
                              IsArchived = page.IsArchived,
                              TemplateId = page.Layout.Id,
                              MasterPageId = page.MasterPage.Id,
                              CategoryId = page.Category.Id,
                              AccessControlEnabled = cmsConfiguration.Security.AccessControlEnabled,
                              Image = page.Image == null || page.Image.IsDeleted ? null :
                                  new ImageSelectorViewModel
                                          {
                                              ImageId = page.Image.Id,
                                              ImageVersion = page.Image.Version,
                                              ImageUrl = fileUrlResolver.EnsureFullPathUrl(page.Image.PublicUrl),
                                              ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(page.Image.PublicThumbnailUrl),
                                              ImageTooltip = page.Image.Caption,
                                              FolderId = page.Image.Folder != null ? page.Image.Folder.Id : (Guid?)null
                                          },
                              SecondaryImage = page.SecondaryImage == null || page.SecondaryImage.IsDeleted ? null :
                                  new ImageSelectorViewModel
                                          {
                                              ImageId = page.SecondaryImage.Id,
                                              ImageVersion = page.SecondaryImage.Version,
                                              ImageUrl = fileUrlResolver.EnsureFullPathUrl(page.SecondaryImage.PublicUrl),
                                              ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(page.SecondaryImage.PublicThumbnailUrl),
                                              ImageTooltip = page.SecondaryImage.Caption,
                                              FolderId = page.SecondaryImage.Folder != null ? page.SecondaryImage.Folder.Id : (Guid?)null
                                          },
                              FeaturedImage = page.FeaturedImage == null || page.FeaturedImage.IsDeleted ? null :
                                  new ImageSelectorViewModel
                                          {
                                              ImageId = page.FeaturedImage.Id,
                                              ImageVersion = page.FeaturedImage.Version,
                                              ImageUrl = fileUrlResolver.EnsureFullPathUrl(page.FeaturedImage.PublicUrl),
                                              ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(page.FeaturedImage.PublicThumbnailUrl),
                                              ImageTooltip = page.FeaturedImage.Caption,
                                              FolderId = page.FeaturedImage.Folder != null ? page.FeaturedImage.Folder.Id : (System.Guid?)null
                                          }
                          }
                    })
                .FirstOne();

            if (model != null && model.Model != null)
            {
                model.Model.Tags = tagService.GetPageTagNames(id);
                model.Model.RedirectFromOldUrl = true;
                model.Model.Categories = categoryService.GetCategories();
                model.Model.UpdateSitemap = true;

                // Get layout options, page options and merge them
                var layoutOptions = Repository.AsQueryable<LayoutOption>(lo => lo.Layout.Id == model.Model.TemplateId).Fetch(o => o.CustomOption).ToList();
                var pageOptions = Repository.AsQueryable<PageOption>(p => p.Page.Id == id).Fetch(o => o.CustomOption).ToList();

                model.Model.OptionValues = optionService.GetMergedOptionValuesForEdit(layoutOptions, pageOptions);
                model.Model.CustomOptions = optionService.GetCustomOptions();

                if (cmsConfiguration.Security.AccessControlEnabled)
                {                    
                    model.Model.UserAccessList = Repository.AsQueryable<Root.Models.Page>()
                                                .Where(x => x.Id == id && !x.IsDeleted)                    
                                                .SelectMany(x => x.AccessRules)
                                                .OrderBy(x => x.Identity)
                                                .ToList()
                                                .Select(x => new UserAccessViewModel(x)).ToList();

                    var rules = model.Model.UserAccessList.Cast<IAccessRule>().ToList();

                    SetIsReadOnly(model.Model, rules);
                }

                model.Model.CanPublishPage = SecurityService.IsAuthorized(Context.Principal, RootModuleConstants.UserRoles.PublishContent);

                // Get templates
                model.Model.Templates = layoutService.GetLayouts();
                model.Model.Templates
                    .Where(x => x.TemplateId == model.Model.TemplateId || x.TemplateId == model.Model.MasterPageId)
                    .Take(1).ToList()
                    .ForEach(x => x.IsActive = true);               
            }

            return model != null ? model.Model : null;
        }
    }
}