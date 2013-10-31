using System.Linq;
using System.Collections.Generic;

using BetterCms.Core.DataAccess;

using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultLayoutService : ILayoutService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The option service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLayoutService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="optionService">The option service.</param>
        public DefaultLayoutService(IRepository repository, IOptionService optionService)
        {
            this.repository = repository;
            this.optionService = optionService;
        }

        /// <summary>
        /// Gets the list of layout view models.
        /// </summary>
        /// <returns>
        /// The list of layout view models
        /// </returns>
        public IList<TemplateViewModel> GetLayouts()
        {
            // Load layouts
            var templatesFuture = repository
                .AsQueryable<Layout>()
                .OrderBy(t => t.Name)
                .Select(t => new TemplateViewModel
                    {
                        Title = t.Name,
                        TemplateId = t.Id,
                        PreviewUrl = t.PreviewUrl
                    })
                .ToFuture();

            // Load master pages
            var masterPagesFuture = repository
                .AsQueryable<Page>()
                .Where(p => p.IsMasterPage)
                .OrderBy(t => t.Title)
                .Select(t => new TemplateViewModel
                    {
                        Title = t.PageUrl,
                        TemplateId = t.Id,
                        PreviewUrl = null,
                        IsMasterPage = true
                    })
                .ToFuture();

            var templates = templatesFuture.ToList().Concat(masterPagesFuture.ToList()).ToList();
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
                .Select(o => new OptionViewModel
                    {
                        OptionKey = o.Key,
                        Type = o.Type,
                        OptionDefaultValue = optionService.ClearFixValueForEdit(o.Type, o.DefaultValue),
                        CanDeleteOption = o.IsDeletable,
                        CustomOption = new CustomOptionViewModel { Identifier = o.CustomOption.Identifier, Title = o.CustomOption.Title }
                    })
                .OrderBy(o => o.OptionKey)
                .ToList();

            optionService.SetCustomOptionValueTitles(options);

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
                    OptionDefaultValue = optionService.ClearFixValueForEdit(o.Type, o.DefaultValue),
                    UseDefaultValue = true,
                    CustomOption = new CustomOptionViewModel { Identifier = o.CustomOption.Identifier, Title = o.CustomOption.Title }
                })
                .ToList();

            optionService.SetCustomOptionValueTitles(options, options);

            return options;
        }
    }
}