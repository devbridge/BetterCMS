using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
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

using NHibernate.Linq;

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
                              IsMasterPage = page.IsMasterPage,
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
                                              FolderId = page.FeaturedImage.Folder != null ? page.FeaturedImage.Folder.Id : (Guid?)null
                                          }
                          }
                    })
                .ToList()
                .FirstOrDefault();

            if (model != null && model.Model != null)
            {
                model.Model.Tags = tagService.GetPageTagNames(id);
                model.Model.RedirectFromOldUrl = true;
                model.Model.Categories = categoryService.GetCategories();
                model.Model.UpdateSitemap = true;

                // Get layout options, page options and merge them
                LoadOptionValues(model.Model);
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

                // Child page can not be selected as master.
                model.Model.Templates = model.Model.Templates
                    .Where(x => !x.IsMasterPage || !Repository.AsQueryable<MasterPage>().Where(m => m.Page.Id == x.TemplateId).Any(m => m.Master.Id == model.Model.Id))
                    .ToList();

                // Page can not be master for it self.
                model.Model.Templates = model.Model.Templates
                    .Where(x => x.TemplateId != model.Model.Id)
                    .ToList();
            }

            return model != null ? model.Model : null;
        }

        /// <summary>
        /// Loads the option values and sets up the list of option value view models.
        /// </summary>
        /// <param name="model">The model.</param>
        private void LoadOptionValues(EditPagePropertiesViewModel model)
        {
            Guid layoutId;
            var allPages = new List<PageMasterPage>
                            {
                                new PageMasterPage
                                    {
                                        Id = model.Id,
                                        MasterPageId = model.MasterPageId,
                                        LayoutId = model.TemplateId
                                    }
                            };
            if (model.TemplateId.HasValue)
            {
                layoutId = model.TemplateId.Value;
            }
            else
            {
                // Load ids of all the master pages
                var masterPages = Repository
                    .AsQueryable<MasterPage>()
                    .Where(mp => mp.Page.Id == model.Id)
                    .Select(mp => new PageMasterPage
                    {
                        Id = mp.Master.Id,
                        MasterPageId = mp.Master.MasterPage.Id,
                        LayoutId = mp.Master.Layout.Id
                    })
                    .ToList();
                allPages.AddRange(masterPages);

                layoutId = masterPages.Where(m => m.LayoutId.HasValue).Select(m => m.LayoutId.Value).FirstOrDefault();
            }

            var layoutOptionsFutureQuery = Repository
                .AsQueryable<LayoutOption>()
                .Where(lo => lo.Layout.Id == layoutId)
                .ToFuture();

            var pageIds = allPages.Select(p => p.Id).ToArray();
            var pageOptionsFutureQuery = Repository
                .AsQueryable<PageOption>()
                .Where(po => pageIds.Contains(po.Page.Id))
                .ToFuture();

            var layoutOptions = layoutOptionsFutureQuery.ToList();
            var allPagesOptions = pageOptionsFutureQuery.ToList();
            var pageOptions = allPagesOptions.Where(po => po.Page.Id == model.Id);

            // Get lowest level options, when going up from master pages to layout
            var masterOptions = GetMasterOptionValues(model.Id, allPages, allPagesOptions, layoutOptions, new List<IOption>());
            model.OptionValues = optionService.GetMergedOptionValuesForEdit(masterOptions, pageOptions);
        }

        private List<IOption> GetMasterOptionValues(Guid id, List<PageMasterPage> allPages, List<PageOption> allPagesOptions,
            List<LayoutOption> layoutOptions, List<IOption> allMasterOptions)
        {
            var page = allPages.FirstOrDefault(p => p.Id == id);
            if (page != null)
            {
                if (page.MasterPageId.HasValue)
                {
                    allMasterOptions = GetMasterOptionValues(page.MasterPageId.Value, allPages, allPagesOptions, layoutOptions, allMasterOptions);

                    foreach (var option in allPagesOptions.Where(o => o.Page.Id == page.MasterPageId.Value))
                    {
                        var masterOption = allMasterOptions.FirstOrDefault(o => o.Key == option.Key
                            && o.Type == option.Type
                            && ((o.CustomOption == null && option.CustomOption == null)
                                || (o.CustomOption != null
                                    && option.CustomOption != null
                                    && o.CustomOption.Identifier == option.CustomOption.Identifier)));

                        if (masterOption != null)
                        {
                            allMasterOptions.Remove(masterOption);
                            allMasterOptions.Add(option);
                        }
                        else
                        {
                            allMasterOptions.Add(option);
                        }
                    }
                }
                else if (page.LayoutId.HasValue)
                {
                    // Returning layout options as master option values
                    return layoutOptions.Cast<IOption>().ToList();
                }
            }

            return allMasterOptions;
        }

        private class PageMasterPage
        {
            public Guid Id { get; set; }

            public Guid? MasterPageId { get; set; }

            public Guid? LayoutId { get; set; }
        }
    }
}