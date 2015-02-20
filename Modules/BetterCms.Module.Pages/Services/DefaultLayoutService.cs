using System;
using System.Linq;
using System.Collections.Generic;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Pages.ViewModels.Templates;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultLayoutService : ILayoutService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The option service.
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The access control service
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLayoutService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultLayoutService(IRepository repository, IOptionService optionService, ICmsConfiguration configuration,
            IAccessControlService accessControlService, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.optionService = optionService;
            this.configuration = configuration;
            this.accessControlService = accessControlService;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the future query for the list of layout view models.
        /// </summary>
        /// <param name="currentPageId">The current page identifier.</param>
        /// <param name="currentPageMasterPageId">The current page master page identifier.</param>
        /// <returns>
        /// The future query for the list of layout view models
        /// </returns>
        public IList<TemplateViewModel> GetAvailableLayouts(Guid? currentPageId = null, Guid? currentPageMasterPageId = null)
        {
            // Load layouts
            var templatesFuture = repository
                .AsQueryable<Layout>()
                .OrderBy(t => t.Name)
                .Select(t => new TemplateViewModel
                    {
                        Title = t.Name,
                        TemplateId = t.Id,
                        PreviewUrl = t.PreviewUrl
                    }).ToFuture();

            // Load master pages
            var masterPagesQuery = repository
                .AsQueryable<PageProperties>()
                .Where(p => p.IsMasterPage);

            if (configuration.Security.AccessControlEnabled)
            {
                var deniedPages = accessControlService.GetDeniedObjects<PageProperties>();
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    if (id == currentPageMasterPageId)
                    {
                        continue;
                    }
                    masterPagesQuery = masterPagesQuery.Where(f => f.Id != id);
                }
            }

            var masterPagesFuture = masterPagesQuery
                .OrderBy(t => t.Title)
                .Select(t => new TemplateViewModel
                    {
                        Title = t.Title,
                        TemplateId = t.Id,
                        PreviewUrl = t.Image != null
                            ? t.Image.PublicUrl
                            : t.FeaturedImage != null
                                ? t.FeaturedImage.PublicUrl
                                : t.SecondaryImage != null
                                    ? t.SecondaryImage.PublicUrl
                                    : null,
                        PreviewThumbnailUrl = t.Image != null
                            ? t.Image.PublicUrl
                            : t.FeaturedImage != null
                                ? t.FeaturedImage.PublicUrl
                                : t.SecondaryImage != null
                                    ? t.SecondaryImage.PublicUrl
                                    : null,
                        IsMasterPage = true,
                        MasterUrlHash = t.PageUrlHash,
                        IsCircularToCurrent = currentPageId.HasValue && !currentPageId.Value.HasDefaultValue()
                            && (t.Id == currentPageId || t.MasterPages.Any(cp => cp.Master.Id == currentPageId))
                    }).ToFuture();

            var templates = templatesFuture.ToList().Concat(masterPagesFuture.ToList()).ToList();
            return templates;
        }

        /// <summary>
        /// Gets the list of layout option view models.
        /// </summary>
        /// <param name="id">The layout id.</param>
        /// <returns>
        /// The list of layout option view models
        /// </returns>
        public IList<OptionViewModel> GetLayoutOptions(Guid id)
        {
            var options = repository
                .AsQueryable<LayoutOption>(lo => lo.Layout.Id == id)
                .Select(o => new OptionViewModel
                    {
                        OptionKey = o.Key,
                        Type = o.Type,
                        OptionDefaultValue = optionService.ClearFixValueForEdit(o.Type, o.DefaultValue),
                        CanDeleteOption = o.IsDeletable,
                        CustomOption = o.CustomOption != null ? new CustomOptionViewModel
                                       {
                                           Identifier = o.CustomOption.Identifier,
                                           Title = o.CustomOption.Title,
                                           Id = o.CustomOption.Id
                                       } : null
                    })
                .OrderBy(o => o.OptionKey)
                .ToList();

            optionService.SetCustomOptionValueTitles(options);

            return options;
        }

        /// <summary>
        /// Gets the list of layout option values.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The list of layout option values.
        /// </returns>
        public IList<OptionValueEditViewModel> GetLayoutOptionValues(Guid id)
        {
            var options = repository
                .AsQueryable<LayoutOption>(lo => lo.Layout.Id == id)
                .OrderBy(o => o.Key)
                .Select(o => new OptionValueEditViewModel
                    {
                        OptionKey = o.Key,
                        Type = o.Type,
                        OptionDefaultValue = optionService.ClearFixValueForEdit(o.Type, o.DefaultValue),
                        UseDefaultValue = true,
                        CustomOption = o.CustomOption != null ? new CustomOptionViewModel
                                       {
                                           Identifier = o.CustomOption.Identifier,
                                           Title = o.CustomOption.Title,
                                           Id = o.CustomOption.Id
                                       } : null
                    })
                .ToList();

            optionService.SetCustomOptionValueTitles(options, options);

            return options;
        }

        /// <summary>
        /// Saves the layout.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="treatNullsAsLists">if set to <c>true</c> treat null lists as empty lists.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> create if not exists.</param>
        /// <returns>
        /// Saved layout entity
        /// </returns>
        public Layout SaveLayout(TemplateEditViewModel model, bool treatNullsAsLists = true, bool createIfNotExists = false)
        {
            if (model.Options != null)
            {
                optionService.ValidateOptionKeysUniqueness(model.Options);
            }

            unitOfWork.BeginTransaction();

            var isNew = model.Id.HasDefaultValue();
            Layout template = null;
            if (!isNew)
            {
                template = repository.AsQueryable<Layout>()
                    .Where(f => f.Id == model.Id)
                    .FetchMany(f => f.LayoutRegions)
                    .ToList()
                    .FirstOrDefault();
                isNew = template == null;

                if (isNew && !createIfNotExists)
                {
                    throw new EntityNotFoundException(typeof(Layout), model.Id);
                }
            }

            if (template == null)
            {
                template = new Layout { Id = model.Id };
            }
            else if (model.Version > 0)
            {
                template.Version = model.Version;
            }

            template.Name = model.Name;
            template.LayoutPath = model.Url;
            template.PreviewUrl = model.PreviewImageUrl;

            // Set null list as empty
            if (treatNullsAsLists)
            {
                model.Options = model.Options ?? new List<OptionViewModel>();
                model.Regions = model.Regions ?? new List<TemplateRegionItemViewModel>();
            }

            // Edits or removes regions.
            if (model.Regions != null)
            {
                if (template.LayoutRegions != null && template.LayoutRegions.Any())
                {
                    foreach (var region in template.LayoutRegions)
                    {
                        var requestRegion = model.Regions != null
                            ? model.Regions.FirstOrDefault(f => f.Identifier.ToLowerInvariant() == region.Region.RegionIdentifier.ToLowerInvariant())
                            : null;

                        if (requestRegion != null && region.Region.RegionIdentifier.ToLowerInvariant() == requestRegion.Identifier.ToLowerInvariant())
                        {
                            region.Description = requestRegion.Description;
                            repository.Save(region);
                        }
                        else
                        {
                            repository.Delete(region);
                        }
                    }
                }

                if (template.LayoutRegions == null)
                {
                    template.LayoutRegions = new List<LayoutRegion>();
                }

                var regions = GetRegions(model.Regions);

                foreach (var requestRegionOption in model.Regions)
                {
                    if (!template.LayoutRegions.Any(f => f.Region.RegionIdentifier.Equals(requestRegionOption.Identifier, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        var region = regions.Find(f => f.RegionIdentifier.Equals(requestRegionOption.Identifier, StringComparison.InvariantCultureIgnoreCase));

                        if (region == null)
                        {
                            if (requestRegionOption.Description == null)
                            {
                                requestRegionOption.Description = string.Empty;
                            }

                            var regionOption = new Region
                            {
                                RegionIdentifier = requestRegionOption.Identifier
                            };

                            template.LayoutRegions.Add(new LayoutRegion
                            {
                                Description = requestRegionOption.Description,
                                Region = regionOption,
                                Layout = template
                            });
                            repository.Save(regionOption);
                        }
                        else
                        {
                            var layoutRegion = new LayoutRegion
                            {
                                Description = requestRegionOption.Description,
                                Region = region,
                                Layout = template
                            };
                            template.LayoutRegions.Add(layoutRegion);
                            repository.Save(layoutRegion);
                        }
                    }
                }
            }

            if (model.Options != null)
            {
                optionService.SetOptions<LayoutOption, Layout>(template, model.Options);
            }

            repository.Save(template);
            unitOfWork.Commit();

            // Notify
            if (isNew)
            {
                Events.PageEvents.Instance.OnLayoutCreated(template);
            }
            else
            {
                Events.PageEvents.Instance.OnLayoutUpdated(template);
            }

            return template;
        }

        private List<Region> GetRegions(IList<TemplateRegionItemViewModel> regionOptions)
        {
            var identifiers = regionOptions.Select(r => r.Identifier).ToArray();

            var regions = unitOfWork.Session.Query<Region>()
                .Where(r => !r.IsDeleted && identifiers.Contains(r.RegionIdentifier))
                .ToList();

            return regions;
        }

        /// <summary>
        /// Deletes the layout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        ///   <c>true</c>, if delete was successful, otherwise <c>false</c>
        /// </returns>
        public bool DeleteLayout(Guid id, int version)
        {
            var pagesInUsage = repository
                .AsQueryable<Page>()
                .Any(p => p.Layout.Id == id);

            if (pagesInUsage)
            {
                throw new ValidationException(() => PagesGlobalization.DeleteTemplate_TemplateIsInUse_Message,
                    string.Format("Failed to delete template {0}. Template is in use.", id));
            }

            var layout = repository.First<Layout>(id);
            if (version > 0)
            {
                layout.Version = version;
            }
            repository.Delete(layout);

            if (layout.LayoutOptions != null)
            {
                foreach (var option in layout.LayoutOptions)
                {
                    repository.Delete(option);
                }
            }

            if (layout.LayoutRegions != null)
            {
                foreach (var region in layout.LayoutRegions)
                {
                    repository.Delete(region);
                }
            }

            unitOfWork.Commit();

            // Notify
            Events.PageEvents.Instance.OnLayoutDeleted(layout);

            return true;
        }
    }
}