using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Services;

using BetterCms.Module.Pages.Content.Resources;
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

        public ServerControlWidget SaveServerControlWidget(EditServerControlWidgetViewModel model)
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

            var requestWidget = GetServerControlWidgetFromRequest(model);
            var widget = GetWidgetForSave(requestWidget, model);
            
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

            return widget;
        }

        private TEntity GetWidgetForSave<TEntity>(TEntity widgetContent, EditWidgetViewModel model)
            where TEntity : Widget
        {
            TEntity widget;
            var createNewWithId = false;

            // Check if entity is created
            if (model.CreateIfNotExists && !model.Id.HasDefaultValue())
            {
                createNewWithId = !repository.AsQueryable<TEntity>().Any(w => w.Id == model.Id);
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
                widget = (TEntity)contentService.SaveContentWithStatusUpdate(widgetContent, model.DesirableStatus);
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

            SetWidgetOptions(request, content);

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

        private ServerControlWidget GetServerControlWidgetFromRequest(EditServerControlWidgetViewModel request)
        {
            ServerControlWidget widget = new ServerControlWidget();
            widget.Id = request.Id;

            if (request.CategoryId.HasValue && !request.CategoryId.Value.HasDefaultValue())
            {
                widget.Category = repository.AsProxy<CategoryEntity>(request.CategoryId.Value);
            }
            else
            {
                widget.Category = null;
            }

            widget.Name = request.Name;
            widget.Url = request.Url;
            widget.Version = request.Version;
            widget.PreviewUrl = request.PreviewImageUrl;

            SetWidgetOptions(request, widget);

            return widget;
        }

        private void SetWidgetOptions(EditWidgetViewModel model, Widget content)
        {
            if (model.Options != null)
            {
                content.ContentOptions = new List<ContentOption>();

                // NOTE: Loading custom options before saving.
                // In other case, when loading custom options from option service, nHibernate updates version number (nHibernate bug)
                var customOptionsIdentifiers = model.Options
                    .Where(o => o.Type == OptionType.Custom)
                    .Select(o => o.CustomOption.Identifier)
                    .Distinct()
                    .ToArray();
                var customOptions = optionService.GetCustomOptionsById(customOptionsIdentifiers);

                foreach (var requestContentOption in model.Options)
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
        }

        public bool DeleteWidget(Guid widgetId, int widgetVersion)
        {
            unitOfWork.BeginTransaction();

            var widget = repository.First<Widget>(widgetId);
            if (widgetVersion > 0)
            {
                widget.Version = widgetVersion;
            }

            var isWidgetInUse = repository
                .AsQueryable<PageContent>()
                .Any(f => f.Content.Id == widgetId && !f.IsDeleted && !f.Page.IsDeleted);

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
    }
}