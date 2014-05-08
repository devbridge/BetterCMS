using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using CategoryEntity = BetterCms.Module.Root.Models.Category;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultWidgetService : IWidgetService
    {
        private readonly IRepository repository;
        
        private readonly IUnitOfWork unitOfWork;
        
        private readonly IOptionService optionService;
        
        private readonly IContentService contentService;
        
        private readonly ISecurityService securityService;

        public DefaultWidgetService(IRepository repository, IUnitOfWork unitOfWork, IOptionService optionService, IContentService contentService,
            ISecurityService securityService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.optionService = optionService;
            this.contentService = contentService;
            this.securityService = securityService;
        }

        public void SaveHtmlContentWidget(EditHtmlContentWidgetViewModel model, out HtmlContentWidget widget, out HtmlContentWidget originalWidget)
        {
            if (model.Options != null)
            {
                optionService.ValidateOptionKeysUniqueness(model.Options);
            }

            unitOfWork.BeginTransaction();

            var widgetContent = GetHtmlContentWidgetFromRequest(model);

            widget = GetWidgetForSave(widgetContent, model);

            repository.Save(widget);

            unitOfWork.Commit();

            // Notify.
            if (widget.Status != ContentStatus.Preview)
            {
                if (model.Id == default(Guid))
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
                    originalWidget = draft;
                }
            }
        }

        private HtmlContentWidget GetWidgetForSave(HtmlContentWidget widgetContent, EditHtmlContentWidgetViewModel model)
        {
            HtmlContentWidget widget;
            var createNewWithId = false;

            // Check if entity is created
            if (model.CreateIfNotExists && !model.Id.HasDefaultValue())
            {
                createNewWithId = !repository.AsQueryable<HtmlContentWidget>().Any(w => w.Id == model.Id);
            }

            if (createNewWithId)
            {
                widget = widgetContent;
                if (model.DesirableStatus == ContentStatus.Published)
                {
                    widget.PublishedOn = model.PublishedOn ?? DateTime.Now;
                    widget.PublishedByUser = !string.IsNullOrEmpty(model.PublishedByUser) ? model.PublishedByUser : securityService.CurrentPrincipalName;
                }

                widget.Status = model.DesirableStatus;
                widget.Id = model.Id;
            }
            else
            {
                widget = (HtmlContentWidget)contentService.SaveContentWithStatusUpdate(widgetContent, model.DesirableStatus);
            }

            return widget;
        }

        private HtmlContentWidget GetHtmlContentWidgetFromRequest(EditHtmlContentWidgetViewModel request)
        {
            HtmlContentWidget content = new HtmlContentWidget();
            content.Id = request.Id;

            if (request.CategoryId.HasValue && !request.CategoryId.Value.HasDefaultValue())
            {
                content.Category = repository.AsProxy<CategoryEntity>(request.CategoryId.Value);
            }
            else
            {
                content.Category = null;
            }

            if (request.Options != null)
            {
                content.ContentOptions = new List<ContentOption>();

                // NOTE: Loading custom options before saving.
                // In other case, when loading custom options from option service, nHibernate updates version number (nHibernate bug)
                var customOptionsIdentifiers = request.Options
                    .Where(o => o.Type == OptionType.Custom)
                    .Select(o => o.CustomOption.Identifier)
                    .Distinct()
                    .ToArray();
                var customOptions = optionService.GetCustomOptionsById(customOptionsIdentifiers);

                foreach (var requestContentOption in request.Options)
                {
                    var contentOption = new ContentOption
                    {
                        Content = content,
                        Key = requestContentOption.OptionKey,
                        DefaultValue = optionService.ClearFixValueForSave(requestContentOption.OptionKey, requestContentOption.Type, requestContentOption.OptionDefaultValue),
                        Type = requestContentOption.Type,
                        CustomOption = requestContentOption.Type == OptionType.Custom
                          ? customOptions.First(o => o.Identifier == requestContentOption.CustomOption.Identifier)
                          : null
                    };

                    optionService.ValidateOptionValue(contentOption);

                    content.ContentOptions.Add(contentOption);
                }
            }

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
    }
}