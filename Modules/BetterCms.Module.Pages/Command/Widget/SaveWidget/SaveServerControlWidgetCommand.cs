using System;
using System.Collections.Generic;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using System.Linq;
using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveServerControlWidgetCommand : SaveWidgetCommandBase<ServerControlWidgetViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override SaveWidgetResponse Execute(ServerControlWidgetViewModel request)
        {
            // Validate
            if (!HttpHelper.VirtualPathExists(request.Url))
            {
                var message = string.Format(PagesGlobalization.SaveWidget_VirtualPathNotExists_Message, request.Url);
                var logMessage = string.Format("Widget view doesn't exists. Url: {0}, Id: {1}", request.Url, request.Id);
                throw new ValidationException(m => message, logMessage);
            }

            UnitOfWork.BeginTransaction();

            var widget = !request.Id.HasDefaultValue()
                ? Repository.AsQueryable<ServerControlWidget>().Fetch(f => f.Category).FetchMany(f => f.ContentOptions).Where(f => f.Id == request.Id).ToList().FirstOrDefault()
                : new ServerControlWidget();

            if (widget == null)
            {
                widget = new ServerControlWidget();
            }

            if (request.CategoryId.HasValue && !request.CategoryId.Value.HasDefaultValue())
            {
                widget.Category = Repository.FirstOrDefault<Category>(request.CategoryId.Value);
            }
            else
            {
                widget.Category = null;
            }

            widget.Name = request.Name;
            widget.Url = request.Url;
            widget.Version = request.Version;
            widget.PreviewUrl = request.PreviewImageUrl;

            // Edits or removes options.
            if (widget.ContentOptions != null && widget.ContentOptions.Any())
            {                
                foreach (var contentOption in widget.ContentOptions)
                {
                    var requestContentOption = request.ContentOptions != null 
                                                ? request.ContentOptions.FirstOrDefault(f => f.OptionKey.Equals(contentOption.Key, StringComparison.InvariantCultureIgnoreCase))
                                                : null;

                    if (requestContentOption != null)
                    {
                        contentOption.DefaultValue = requestContentOption.OptionDefaultValue;
                        contentOption.Type = requestContentOption.Type;
                        Repository.Save(contentOption);
                    }
                    else
                    {
                        Repository.Delete(contentOption);
                    }
                }                
            }

            // Adds new options.
            if (request.ContentOptions != null)
            {
                foreach (var requestContentOption in request.ContentOptions)
                {                    
                    if (widget.ContentOptions == null)
                    {
                        widget.ContentOptions = new List<ContentOption>();
                    }

                    if (!widget.ContentOptions.Any(f => f.Key.Equals(requestContentOption.OptionKey, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        var contentOption = new ContentOption
                                                {
                                                    Content = widget,
                                                    Key = requestContentOption.OptionKey,
                                                    DefaultValue = requestContentOption.OptionDefaultValue,
                                                    Type = requestContentOption.Type
                                                };
                        widget.ContentOptions.Add(contentOption);
                        Repository.Save(contentOption);
                    }
                }                
            }
            
            Repository.Save(widget);
            UnitOfWork.Commit();

            return new SaveWidgetResponse
            {
                Id = widget.Id,
                CategoryName = widget.Category != null ? widget.Category.Name : null,
                WidgetName = widget.Name,
                Version = widget.Version ,
                WidgetType = WidgetType.ServerControl.ToString()
            };
        }
    }
}