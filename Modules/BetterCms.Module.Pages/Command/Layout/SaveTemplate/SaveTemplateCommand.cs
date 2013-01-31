using System;
using System.Collections.Generic;
using System.Linq;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.ViewModels.Templates;
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
                var message = string.Format(PagesGlobalization.SaveTemplate_VirtualPathNotExists_Message, request.Url);
                var logMessage = string.Format("Template doesn't exists. Url: {0}, Id: {1}", request.Url, request.Id);
                throw new ValidationException(() => message, logMessage);
            }

            UnitOfWork.BeginTransaction();

            var template = !request.Id.HasDefaultValue()
                               ? Repository.AsQueryable<Root.Models.Layout>()
                                           .FetchMany(f => f.LayoutRegions)
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

            // Edits or removes regions.
            if (template.LayoutRegions != null && template.LayoutRegions.Any())
            {
                foreach (var region in template.LayoutRegions)
                {
                    var requestRegion = request.RegionOptions != null
                                                   ? request.RegionOptions.FirstOrDefault(f => f.Identifier == region.Region.RegionIdentifier)
                                                   : null;

                    if (requestRegion != null && region.Description != requestRegion.Description)
                    {
                        region.Description = requestRegion.Description;                        
                        Repository.Save(region);
                    }
                    else
                    {
                        Repository.Delete(region);
                    }
                }
            }
            
            // Adds new region.
            if (request.RegionOptions != null)
            {
                var regions = GetRegions(request.RegionOptions);

                foreach (var requestRegionOption in request.RegionOptions)
                {
                    if (template.LayoutRegions == null)
                    {
                        template.LayoutRegions = new List<LayoutRegion>();
                    }

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
                            Repository.Save(regionOption);
                        }
                        else
                        {
                            var layoutRegion = new LayoutRegion
                                                   {
                                                       Region = region, 
                                                       Layout = template
                                                   };
                            template.LayoutRegions.Add(layoutRegion);
                            Repository.Save(layoutRegion);
                        }
                    }
                }
            }

            Repository.Save(template);
            UnitOfWork.Commit();

            return new SaveTemplateResponse
            {
                Id = template.Id,
                TemplateName = template.Name,
                Version = template.Version
            };
        }

        private List<Region> GetRegions(IList<TemplateRegionItemViewModel> regionOptions)
        {            
            var identifiers = regionOptions.Select(r => r.Identifier).ToArray();

            var regions = UnitOfWork.Session.Query<Region>()
                                    .Where(r => !r.IsDeleted && identifiers.Contains(r.RegionIdentifier))
                                    .ToList(); 

            return regions;
        }
    }
}