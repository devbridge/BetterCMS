using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveServerControlWidgetCommand : SaveWidgetCommandBase<EditServerControlWidgetViewModel>
    {
        public virtual IContentService ContentService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override SaveWidgetResponse Execute(EditServerControlWidgetViewModel request)
        {
            // Validate.
            if (!HttpHelper.VirtualPathExists(request.Url))
            {
                var message = string.Format(PagesGlobalization.SaveWidget_VirtualPathNotExists_Message, request.Url);
                var logMessage = string.Format("Widget view doesn't exists. Url: {0}, Id: {1}", request.Url, request.Id);

                throw new ValidationException(() => message, logMessage);
            }

            if (request.DesirableStatus == ContentStatus.Draft)
            {
                throw new CmsException(string.Format("Server widget does not support Draft state."));
            }

            UnitOfWork.BeginTransaction();

            var widget = (ServerControlWidget)ContentService.SaveContentWithStatusUpdate(GetServerControlWidgetFromRequest(request), request.DesirableStatus);
            Repository.Save(widget);

            UnitOfWork.Commit();

            // Notify.
            if (widget.Status != ContentStatus.Preview)
            {
                if (request.Id == default(Guid))
                {
                    PagesApiContext.Events.OnWidgetCreated(widget);
                }
                else
                {
                    PagesApiContext.Events.OnWidgetUpdated(widget);
                }
            }

            return new SaveWidgetResponse
                       {
                           Id = widget.Id,
                           CategoryName = widget.Category != null ? widget.Category.Name : null,
                           WidgetName = widget.Name,
                           Version = widget.Version,
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
                widget.Category = Repository.AsProxy<Category>(request.CategoryId.Value);
            }
            else
            {
                widget.Category = null;
            }

            widget.Name = request.Name;
            widget.Url = request.Url;
            widget.Version = request.Version;
            widget.PreviewUrl = request.PreviewImageUrl;            

            if (request.ContentOptions != null)
            {
                widget.ContentOptions = new List<ContentOption>();

                foreach (var requestContentOption in request.ContentOptions)
                {
                    var contentOption = new ContentOption {
                                                              Content = widget,
                                                              Key = requestContentOption.OptionKey,
                                                              DefaultValue = requestContentOption.OptionDefaultValue,
                                                              Type = requestContentOption.Type
                                                          };

                    widget.ContentOptions.Add(contentOption);
                }
            }

            return widget;
        }
    }
}