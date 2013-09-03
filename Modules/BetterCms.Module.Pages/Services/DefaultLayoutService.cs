using System.Linq;
using System.Collections.Generic;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultLayoutService : ILayoutService
    {
        private IRepository repository;

        public DefaultLayoutService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of layout view models.
        /// </summary>
        /// <returns>
        /// The list of layout view models
        /// </returns>
        public IList<TemplateViewModel> GetLayouts()
        {
            var templates = repository
                .AsQueryable<Layout>()
                .OrderBy(t => t.Name)
                .Select(t => new TemplateViewModel
                    {
                        Title = t.Name,
                        TemplateId = t.Id,
                        PreviewUrl = t.PreviewUrl
                    })
                .ToList();

            return templates;
        }

        /// <summary>
        /// Gets the list of layout option view models.
        /// </summary>
        /// <param name="id">The layout id.</param>
        /// <returns>
        /// The list of layout option view models
        /// </returns>
        public IList<OptionViewModel> GetLayoutOptions(System.Guid id)
        {
            var options = repository
                .AsQueryable<LayoutOption>(lo => lo.Layout.Id == id)
                .OrderBy(o => o.Key)
                .Select(o => new OptionViewModel
                    {
                        OptionKey = o.Key,
                        Type = o.Type,
                        OptionDefaultValue = o.DefaultValue
                    })
                .ToList();

            return options;
        }

        /// <summary>
        /// Gets the list of layout option values.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The list of layout option values.
        /// </returns>
        public IList<OptionValueEditViewModel> GetLayoutOptionValues(System.Guid id)
        {
            var options = repository
                .AsQueryable<LayoutOption>(lo => lo.Layout.Id == id)
                .OrderBy(o => o.Key)
                .Select(o => new OptionValueEditViewModel
                {
                    OptionKey = o.Key,
                    Type = o.Type,
                    OptionDefaultValue = o.DefaultValue,
                    UseDefaultValue = true
                })
                .ToList();

            return options;
        }
    }
}