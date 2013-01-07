using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Command.Widget.SaveWidget;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Templates;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Layout.SaveTemplate
{
    public class SaveTemplateCommand : CommandBase, ICommand<TemplateEditViewModel, SaveTemplateResponse>
    {
    /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public SaveTemplateResponse Execute(TemplateEditViewModel request)
        {
            // Validate
            if (!HttpHelper.VirtualPathExists(request.Url))
            {
                var message = string.Format(PagesGlobalization.SaveWidget_VirtualPathNotExists_Message, request.Url);
                var logMessage = string.Format("Template doesn't exists. Url: {0}, Id: {1}", request.Url, request.Id);
                throw new ValidationException(m => message, logMessage);
            }

            UnitOfWork.BeginTransaction();

            var template = !request.Id.HasDefaultValue()
                               ? Repository.AsQueryable<Root.Models.Layout>()
                                           .Where(f => f.Id == request.Id)
                                           .ToList()
                                           .FirstOrDefault()
                               : new Root.Models.Layout();

            if (template == null)
            {
                template = new Root.Models.Layout();
            }

            

            template.Name = request.Name;
            template.LayoutPath = request.Url;
            template.Version = request.Version;
            template.PreviewUrl = request.PreviewImageUrl;
            //template.

            // Edits or removes options.
            /*if (template.ContentOptions != null && template.ContentOptions.Any())
            {
               /* foreach (var contentOption in template.ContentOptions)
                {
                    var requestContentOption = request.ContentOptions != null
                                                   ? request.ContentOptions.FirstOrDefault(
                                                       f => f.OptionKey.Equals(contentOption.Key, StringComparison.InvariantCultureIgnoreCase))
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
                }*/
            //}
            /*
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
              */
            Repository.Save(template);
            UnitOfWork.Commit();

            return new SaveTemplateResponse
                       {
                           Id = template.Id,
                           TemplateName = template.Name,
                           Version = template.Version,
 
                       };
        }
    }
}