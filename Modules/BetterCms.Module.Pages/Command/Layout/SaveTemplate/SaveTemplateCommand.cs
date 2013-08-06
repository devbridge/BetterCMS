using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Templates;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Option;

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
            UnitOfWork.BeginTransaction();

            var template = !request.Id.HasDefaultValue()
                               ? Repository.AsQueryable<Root.Models.Layout>()
                                           .Where(f => f.Id == request.Id)
                                           .FetchMany(f => f.LayoutRegions)
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
                    var requestRegion = request.Regions != null
                                                   ? request.Regions.FirstOrDefault(f => f.Identifier == region.Region.RegionIdentifier)
                                                   : null;

                    if (requestRegion != null && region.Region.RegionIdentifier == requestRegion.Identifier)
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
            if (request.Regions != null)
            {
                if (template.LayoutRegions == null)
                {
                    template.LayoutRegions = new List<LayoutRegion>();
                }

                var regions = GetRegions(request.Regions);

                foreach (var requestRegionOption in request.Regions)
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
                            Repository.Save(regionOption);
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
                            Repository.Save(layoutRegion);
                        }
                    }
                }
            }

            SetOptions(template, request.Options);

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

        private void SetOptions(Root.Models.Layout template, IList<OptionViewModel> options)
        {
            // Delete old ones
            foreach (var option in template.LayoutOptions.Distinct())
            {
                if (options == null || options.All(o => o.OptionKey != option.Key))
                {
                    Repository.Delete(option);
                }
            }

            // Add new ones
            if (options != null)
            {
                foreach (var requestLayoutOption in options)
                {
                    var option = template.LayoutOptions.FirstOrDefault(o => o.Key == requestLayoutOption.OptionKey);
                    if (option == null)
                    {
                        option = new LayoutOption();
                        template.LayoutOptions.Add(option);
                    }

                    option.Key = requestLayoutOption.OptionKey;
                    option.DefaultValue = requestLayoutOption.OptionDefaultValue;
                    option.Type = requestLayoutOption.Type;
                    option.Layout = template;
                }
            }
        }
    }
}