using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Exceptions.DataTier;

using NHibernate.Criterion;

using CategoryEntity = BetterCms.Module.Root.Models.Category;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultWidgetService : IWidgetService
    {
        private readonly IRepository repository;

        private readonly IUnitOfWork unitOfWork;

        private readonly IOptionService optionService;

        private readonly IContentService contentService;

        private readonly IChildContentService childContentService;

        private readonly ICategoryService categoryService;

        private readonly ICmsConfiguration cmsConfiguration;

        public DefaultWidgetService(IRepository repository, IUnitOfWork unitOfWork, IOptionService optionService, IContentService contentService,
            IChildContentService childContentService, ICategoryService categoryService, ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.optionService = optionService;
            this.contentService = contentService;
            this.childContentService = childContentService;
            this.categoryService = categoryService;
            this.cmsConfiguration = cmsConfiguration;
        }

        public void SaveHtmlContentWidget(EditHtmlContentWidgetViewModel model, IList<ContentOptionValuesViewModel> childContentOptionValues,
            out HtmlContentWidget widget, out HtmlContentWidget originalWidget,
            bool treatNullsAsLists = true, bool createIfNotExists = false)
        {
            if (model.Options != null)
            {
                optionService.ValidateOptionKeysUniqueness(model.Options);
            }

            unitOfWork.BeginTransaction();

            bool isCreatingNew;

            var widgetContent = GetHtmlContentWidgetFromRequest(model, treatNullsAsLists, !model.Id.HasDefaultValue());
            widget = GetWidgetForSave(widgetContent, model, createIfNotExists, out isCreatingNew);

            optionService.SaveChildContentOptions(widget, childContentOptionValues, model.DesirableStatus);

            repository.Save(widget);
            unitOfWork.Commit();

            // Notify
            if (widget.Status != ContentStatus.Preview)
            {
                if (isCreatingNew)
                {
                    Events.PageEvents.Instance.OnWidgetCreated(widget);
                }
                else
                {
                    Events.PageEvents.Instance.OnWidgetUpdated(widget);
                }
            }

            originalWidget = widget;
            if (model.DesirableStatus == ContentStatus.Draft && widget.History != null)
            {
                var draft = widget.History.FirstOrDefault(h => h is HtmlContentWidget && !h.IsDeleted && h.Status == ContentStatus.Draft) as HtmlContentWidget;
                if (draft != null)
                {
                    widget = draft;
                }
            }
        }

        public ServerControlWidget SaveServerControlWidget(EditServerControlWidgetViewModel model, bool treatNullsAsLists = true, bool createIfNotExists = false)
        {
            if (model.DesirableStatus == ContentStatus.Draft)
            {
                throw new CmsException(string.Format("Server widget does not support Draft state."));
            }

            if (model.Options != null)
            {
                optionService.ValidateOptionKeysUniqueness(model.Options);
            }

            unitOfWork.BeginTransaction();

            bool isCreatingNew;

            var requestWidget = GetServerControlWidgetFromRequest(model, treatNullsAsLists, !model.Id.HasDefaultValue());
            var widget = GetWidgetForSave(requestWidget, model, createIfNotExists, out isCreatingNew);

            repository.Save(widget);
            unitOfWork.Commit();

            // Notify.
            if (widget.Status != ContentStatus.Preview)
            {
                if (isCreatingNew)
                {
                    Events.PageEvents.Instance.OnWidgetCreated(widget);
                }
                else
                {
                    Events.PageEvents.Instance.OnWidgetUpdated(widget);
                }
            }

            return widget;
        }

        private TEntity GetWidgetForSave<TEntity>(TEntity widgetContent, EditWidgetViewModel model, bool createIfNotExists, out bool isCreatingNew)
            where TEntity : Widget
        {
            TEntity widget, originalWidget = null;
            var createNewWithId = false;

            isCreatingNew = model.Id.HasDefaultValue();
            if (createIfNotExists || !isCreatingNew)
            {
                originalWidget = repository.FirstOrDefault<TEntity>(model.Id);
                isCreatingNew = originalWidget == null;
                createNewWithId = isCreatingNew && !model.Id.HasDefaultValue();
            }

            var dynamicContentContainer = widgetContent as IDynamicContentContainer;
            if (dynamicContentContainer != null && !isCreatingNew && !model.IsUserConfirmed)
            {
                contentService.CheckIfContentHasDeletingChildrenWithException(null, widgetContent.Id, dynamicContentContainer.Html);
                CheckIfContentHasDeletingWidgetsWithDynamicRegions(originalWidget, dynamicContentContainer.Html);
            }

            if (model.DesirableStatus == ContentStatus.Published)
            {
                if (isCreatingNew)
                {
                    if (model.PublishedOn.HasValue)
                    {
                        widgetContent.PublishedOn = model.PublishedOn;
                    }
                    if (!string.IsNullOrEmpty(model.PublishedByUser))
                    {
                        widgetContent.PublishedByUser = model.PublishedByUser;
                    }
                }
                else
                {
                    widgetContent.PublishedOn = originalWidget.PublishedOn;
                    widgetContent.PublishedByUser = originalWidget.PublishedByUser;                    
                }
            }

            

            if (createNewWithId)
            {
                widget = widgetContent;
                contentService.UpdateDynamicContainer(widget);

                widget.Status = model.DesirableStatus;
                widget.Id = model.Id;
            }
            else
            {
                widget = (TEntity)contentService.SaveContentWithStatusUpdate(widgetContent, model.DesirableStatus);
            }

            return widget;
        }

        private HtmlContentWidget GetHtmlContentWidgetFromRequest(EditHtmlContentWidgetViewModel request, bool treatNullsAsLists, bool isNew)
        {
            HtmlContentWidget content = new HtmlContentWidget();
            content.Id = request.Id;

            SetWidgetCategories(request, content, treatNullsAsLists, isNew);
            SetWidgetOptions(request, content, treatNullsAsLists, isNew);

            content.Name = request.Name;
            content.Html = request.PageContent ?? string.Empty;
            content.UseHtml = request.EnableCustomHtml;
            content.UseCustomCss = request.EnableCustomCSS;
            content.CustomCss = request.CustomCSS;
            content.UseCustomJs = request.EnableCustomJS;
            content.CustomJs = request.CustomJS;
            content.Version = request.Version;
            content.EditInSourceMode = request.EditInSourceMode;

            return content;
        }

        private void SetWidgetCategories(EditWidgetViewModel request, Widget content, bool treatNullsAsLists, bool isNew)
        {
            if (request.Categories != null)
            {
                content.Categories = new List<WidgetCategory>();

                var categories =
                    repository.AsQueryable<Category>()
                        .Where(c => !c.CategoryTree.IsDeleted && c.CategoryTree.AvailableFor.Any(e => e.CategorizableItem.Name == content.GetCategorizableItemKey()))
                        .ToList();

                foreach (var categoryItem in request.Categories)
                {
                    var category = categories.FirstOrDefault(c => c.Id == categoryItem.Key.ToGuidOrDefault());
                    if (category == null)
                    {
                        var message = string.Format(RootGlobalization.Validation_Category_Unavailable_Message, categoryItem.Value);
                        throw new ValidationException(() => message, message);
                    }
                    var widgetCategory = new WidgetCategory
                                             {
                                                 Widget = content,
                                                 Category = repository.AsProxy<Category>(category.Id)
                                             };
                    content.Categories.Add(widgetCategory);
                }
            }
            else if (!treatNullsAsLists)
            {
                // When calling from API with null list, categories should be loaded before process.
                // Null from API means, that list should be kept unchanged.
                content.Categories = repository
                    .AsQueryable<WidgetCategory>(pco => pco.Widget.Id == request.Id)
                    .Fetch(pco => pco.Category)
                    .ToList();
            }
        }

        private ServerControlWidget GetServerControlWidgetFromRequest(EditServerControlWidgetViewModel request, bool treatNullsAsLists, bool isNew)
        {
            ServerControlWidget widget = new ServerControlWidget();
            widget.Id = request.Id;

            widget.Name = request.Name;
            widget.Url = request.Url;
            widget.Version = request.Version;
            widget.PreviewUrl = request.PreviewImageUrl;

            SetWidgetCategories(request, widget, treatNullsAsLists, isNew);
            SetWidgetOptions(request, widget, treatNullsAsLists, isNew);

            return widget;
        }

        private void SetWidgetOptions(EditWidgetViewModel model, Widget content, bool treatNullsAsLists, bool isNew)
        {
            if (model.Options != null)
            {
                content.ContentOptions = new List<ContentOption>();

                // NOTE: Loading custom options before saving.
                // In other case, when loading custom options from option service, nHibernate updates version number (nHibernate bug)
                var customOptionsIdentifiers = model.Options.Where(o => o.Type == OptionType.Custom).Select(o => o.CustomOption.Identifier).Distinct().ToArray();
                var customOptions = optionService.GetCustomOptionsById(customOptionsIdentifiers);

                foreach (var requestContentOption in model.Options)
                {
                    var contentOption = new ContentOption
                                        {
                                            Content = content,
                                            Key = requestContentOption.OptionKey,
                                            DefaultValue =
                                                optionService.ClearFixValueForSave(
                                                    requestContentOption.OptionKey,
                                                    requestContentOption.Type,
                                                    requestContentOption.OptionDefaultValue),
                                            Type = requestContentOption.Type,
                                            CustomOption =
                                                requestContentOption.Type == OptionType.Custom
                                                    ? repository.AsProxy<CustomOption>(customOptions.First(o => o.Identifier == requestContentOption.CustomOption.Identifier).Id)
                                                    : null
                                        };

                    optionService.ValidateOptionValue(contentOption);

                    if (cmsConfiguration.EnableMultilanguage && requestContentOption.Translations != null)
                    {
                        var translations = requestContentOption.Translations.Select(x => new ContentOptionTranslation
                        {
                            ContentOption = contentOption,
                            Language = repository.AsProxy<Language>(x.LanguageId.ToGuidOrDefault()),
                            Value = optionService.ClearFixValueForSave(requestContentOption.OptionKey, requestContentOption.Type, x.OptionValue)
                        }).ToList();
                        foreach (var translation in translations)
                        {
                            optionService.ValidateOptionValue(contentOption.Key, translation.Value, contentOption.Type, contentOption.CustomOption);
                        }
                        contentOption.Translations = translations;
                    }

                    content.ContentOptions.Add(contentOption);
                }
            }
            else if (!treatNullsAsLists)
            {
                // When calling from API with null list, options should be loaded before process
                // Null from API means, that list should be kept unchanged
                content.ContentOptions = repository
                    .AsQueryable<ContentOption>(pco => pco.Content.Id == model.Id)
                    .Fetch(pco => pco.CustomOption)
                    .ToList();
            }
        }

        public bool DeleteWidget(Guid widgetId, int widgetVersion)
        {
            unitOfWork.BeginTransaction();

            var widget = repository
                .AsQueryable<Widget>(w => w.Id == widgetId)
                .FetchMany(w => w.ContentOptions)
                .Fetch(w => w.Original)
                .ThenFetchMany(ow => ow.ContentOptions)
                .ToList()
                .FirstOne();

            if (widget.Original != null)
            {
                if (widget.Version != widgetVersion)
                {
                    throw new ConcurrentDataException(widget);
                }
                repository.Delete<Widget>(widget.Id, widget.Version);
                widget = (Widget)widget.Original;
            }
            else if (widgetVersion > 0)
            {
                widget.Version = widgetVersion;
            }

            var isWidgetInUse = repository
                .AsQueryable<PageContent>()
                .Any(f => f.Content.Id == widgetId && !f.IsDeleted && !f.Page.IsDeleted);

            if (!isWidgetInUse)
            {
                isWidgetInUse = repository
                    .AsQueryable<ChildContent>()
                    .Any(f => f.Child.Id == widgetId && !f.IsDeleted && !f.Parent.IsDeleted && f.Parent.Original == null);
            }

            if (isWidgetInUse)
            {
                var message = string.Format(PagesGlobalization.Widgets_CanNotDeleteWidgetIsInUse_Message, widget.Name);
                var logMessage = string.Format("A widget {0}(id={1}) can't be deleted because it is in use.", widget.Name, widgetId);
                throw new ValidationException(() => message, logMessage);
            }

            repository.Delete(widget);

            if (widget.ContentOptions != null)
            {
                foreach (var option in widget.ContentOptions)
                {
                    repository.Delete(option);
                }
            }

            unitOfWork.Commit();

            // Notify.
            Events.PageEvents.Instance.OnWidgetDeleted(widget);

            return true;
        }

        public SiteSettingWidgetListViewModel GetFilteredWidgetsList(WidgetsFilter filter)
        {
            filter = filter ?? new WidgetsFilter();
            filter.SetDefaultSortingOptions("WidgetName");

            var query = repository.AsQueryable<Widget>()
                        .Where(f => !f.IsDeleted
                                && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft)
                                && (f.Original == null || !f.Original.IsDeleted));

            if (filter.ChildContentId.HasValue && !filter.ChildContentId.Value.HasDefaultValue())
            {
                query = query.Where(f => f.ChildContents.Any(cc => cc.Child.Id == filter.ChildContentId.Value));
            }

            

            var modelQuery = query.Select(f => new SiteSettingWidgetItemViewModel
                {
                    Id = f.Id,
                    OriginalId = f.Status == ContentStatus.Draft && f.Original != null && f.Original.Status == ContentStatus.Published ? f.Original.Id : f.Id,
                    Version = f.Version,
                    OriginalVersion = f.Status == ContentStatus.Draft && f.Original != null && f.Original.Status == ContentStatus.Published ? f.Original.Version : f.Version,
                    WidgetName = f.Name,                    
                    WidgetEntityType = f.GetType(),
                    IsPublished = f.Status == ContentStatus.Published || (f.Original != null && f.Original.Status == ContentStatus.Published),
                    HasDraft = f.Status == ContentStatus.Draft
                });

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
            {
                var searchQuery = filter.SearchQuery.ToLowerInvariant();
                modelQuery = modelQuery.Where(f => f.WidgetName.ToLower().Contains(searchQuery)
                                                || repository.AsQueryable<WidgetCategory>().Where(c => c.Category.Name.ToLower().Contains(searchQuery)).Select(c => c.Id).Contains(f.Id));
            }

            modelQuery = modelQuery.ToList()
                .GroupBy(g => g.OriginalId)
                .Select(grp => grp.OrderByDescending(p => p.HasDraft).First())
                .AsQueryable();

            var count = modelQuery.ToRowCountFutureValue();
            var widgets = modelQuery.AddSortingAndPaging(filter).ToList();

            widgets.ForEach(
                item =>
                {
                    if (typeof(ServerControlWidget).IsAssignableFrom(item.WidgetEntityType))
                    {
                        item.WidgetType = WidgetType.ServerControl;
                    }
                    else if (typeof(HtmlContentWidget).IsAssignableFrom(item.WidgetEntityType))
                    {
                        item.WidgetType = WidgetType.HtmlContent;
                    }
                    else
                    {
                        item.WidgetType = null;
                    }
                });

            return new SiteSettingWidgetListViewModel(widgets, filter, count.Value);
        }

        private void CheckIfContentHasDeletingWidgetsWithDynamicRegions(Widget widget, string targetHtml)
        {
            var sourceHtml = ((IDynamicContentContainer)widget).Html;
            var sourceWidgets = PageContentRenderHelper.ParseWidgetsFromHtml(sourceHtml);
            var targetWidgets = PageContentRenderHelper.ParseWidgetsFromHtml(targetHtml);

            // Get ids of child widgets, which are being removed from the content
            var removingWidgetIds = sourceWidgets
                .Where(sw => targetWidgets.All(tw => tw.WidgetId != sw.WidgetId))
                .Select(sw => sw.WidgetId)
                .Distinct()
                .ToArray();
            if (removingWidgetIds.Any())
            {
                var childContents = childContentService.RetrieveChildrenContentsRecursively(true, removingWidgetIds);
                var childContentIds = childContents.Select(cc => cc.Child.Id).Distinct().ToList();

                PageContent pcAlias = null;
                ContentRegion contentRegionAlias = null;

                var subQuery = QueryOver.Of(() => contentRegionAlias)
                    .Where(() => !contentRegionAlias.IsDeleted)
                    .And(Restrictions.In(Projections.Property(() => contentRegionAlias.Content.Id), childContentIds))
                    .And(() => contentRegionAlias.Region == pcAlias.Region)
                    .Select(cr => cr.Id);

                var areAnyContentUsages = repository.AsQueryOver(() => pcAlias)
                    .WithSubquery.WhereExists(subQuery)
                    .And(() => !pcAlias.IsDeleted)
                    .RowCount() > 0;

                if (areAnyContentUsages)
                {
                    var message = PagesGlobalization.SaveWidget_ContentHasChildrenWidgetWithDynamicContents_ConfirmationMessage;
                    var logMessage = string.Format("User is trying to remove child widget which has children with dynamic regions and assigned contents. Confirmation is required.");
                    throw new ConfirmationRequestException(() => message, logMessage);
                }
            }
        }

        /// <summary>
        /// Gets the list of widget child regions view models.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The list of widget child regions view models</returns>
        public List<PageContentChildRegionViewModel> GetWidgetChildRegionViewModels(Root.Models.Content content)
        {
            var models = new List<PageContentChildRegionViewModel>();

            if (content.ContentRegions != null)
            {
                foreach (var contentRegion in content.ContentRegions.Where(cr => !cr.IsDeleted).Distinct())
                {
                    models.Add(new PageContentChildRegionViewModel(contentRegion));
                }
            }

            var childContents = childContentService.RetrieveChildrenContentsRecursively(true, new[] { content.Id });
            if (childContents != null)
            {
                foreach (var childContent in childContents)
                {
                    var contentToAdd = contentService.GetDraftOrPublishedContent(childContent.Child);
                    if (contentToAdd.ContentRegions != null)
                    {
                        foreach (var contentRegion in contentToAdd.ContentRegions.Where(cr => !cr.IsDeleted).Distinct())
                        {
                            models.Add(new PageContentChildRegionViewModel(contentRegion));
                        }
                    }
                }
            }

            return models.GroupBy(r => r.RegionIdentifier).Select(r => r.First()).ToList();
        }
    }
}