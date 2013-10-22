using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using CategoryEntity = BetterCms.Module.Root.Models.Category;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveServerControlWidgetCommand : SaveWidgetCommandBase<EditServerControlWidgetViewModel>
    {
        public virtual IContentService ContentService { get; set; }

        public virtual IOptionService OptionService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override SaveWidgetResponse Execute(EditServerControlWidgetViewModel request)
        {
            if (request.DesirableStatus == ContentStatus.Draft)
            {
                throw new CmsException(string.Format("Server widget does not support Draft state."));
            }

            if (request.Options != null)
            {
                OptionService.ValidateOptionKeysUniqueness(request.Options);
            }

            UnitOfWork.BeginTransaction();

            var requestWidget = GetServerControlWidgetFromRequest(request);
            var widget = (ServerControlWidget)ContentService.SaveContentWithStatusUpdate(requestWidget, request.DesirableStatus);
            Repository.Save(widget);

            UnitOfWork.Commit();

            // Notify.
            if (widget.Status != ContentStatus.Preview)
            {
                if (request.Id == default(Guid))
                {
                    Events.PageEvents.Instance.OnWidgetCreated(widget);
                }
                else
                {
                    Events.PageEvents.Instance.OnWidgetUpdated(widget);
                }
            }

            return new SaveWidgetResponse
                       {
                           Id = widget.Id,
                           OriginalId = widget.Id,
                           CategoryName = widget.Category != null ? widget.Category.Name : null,
                           WidgetName = widget.Name,
                           Version = widget.Version,
                           OriginalVersion = widget.Version,
                           WidgetType = WidgetType.ServerControl.ToString(),
                           IsPublished = widget.Status == ContentStatus.Published,
                           HasDraft = widget.Status == ContentStatus.Draft || widget.History != null && widget.History.Any(f => f.Status == ContentStatus.Draft),
                           DesirableStatus = request.DesirableStatus,
                           PreviewOnPageContentId = request.PreviewOnPageContentId
                       };
        }

        private ServerControlWidget GetServerControlWidgetFromRequest(EditServerControlWidgetViewModel request)
        {
            ServerControlWidget widget = new ServerControlWidget();
            widget.Id = request.Id;

            if (request.CategoryId.HasValue && !request.CategoryId.Value.HasDefaultValue())
            {
                widget.Category = Repository.AsProxy<CategoryEntity>(request.CategoryId.Value);
            }
            else
            {
                widget.Category = null;
            }

            widget.Name = request.Name;
            widget.Url = request.Url;
            widget.Version = request.Version;
            widget.PreviewUrl = request.PreviewImageUrl;            

            if (request.Options != null)
            {
                widget.ContentOptions = new List<ContentOption>();

                // NOTE: Loading custom options before saving.
                // In other case, when loading custom options from option service, nHibernate updates version number (nHibernate bug)
                var customOptionsIdentifiers = request.Options
                    .Where(o => o.Type == OptionType.Custom)
                    .Select(o => o.CustomOption.Identifier)
                    .Distinct()
                    .ToArray();
                var customOptions = OptionService.GetCustomOptionsById(customOptionsIdentifiers);

                foreach (var requestContentOption in request.Options)
                {
                    var contentOption = new ContentOption {
                                                              Content = widget,
                                                              Key = requestContentOption.OptionKey,
                                                              DefaultValue = OptionService.ClearFixValueForSave(requestContentOption.OptionKey, requestContentOption.Type, requestContentOption.OptionDefaultValue),
                                                              Type = requestContentOption.Type,
                                                              CustomOption = requestContentOption.Type == OptionType.Custom 
                                                                ? customOptions.First(o => o.Identifier == requestContentOption.CustomOption.Identifier)
                                                                : null
                                                          };

                    OptionService.ValidateOptionValue(contentOption);

                    widget.ContentOptions.Add(contentOption);
                }
            }

            return widget;
        }
    }
}