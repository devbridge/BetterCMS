using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Pages.ViewModels.Templates;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public interface ILayoutService
    {
        /// <summary>
        /// Gets the future query for the list of layout view models.
        /// </summary>
        /// <param name="currentPageId">The current page id.</param>
        /// <param name="currentPageMasterPageId">The current page master page identifier.</param>
        /// <returns>
        /// The future query for the list of layout view models
        /// </returns>
        IList<TemplateViewModel> GetAvailableLayouts(System.Guid? currentPageId = null, System.Guid? currentPageMasterPageId = null);

        /// <summary>
        /// Gets the list of layout option view models.
        /// </summary>
        /// <param name="id">The layout id.</param>
        /// <returns>
        /// The list of layout option view models
        /// </returns>
        IList<OptionViewModel> GetLayoutOptions(System.Guid id);

        /// <summary>
        /// Gets the list of layout option values.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The list of layout option values.
        /// </returns>
        IList<OptionValueEditViewModel> GetLayoutOptionValues(System.Guid id);

        /// <summary>
        /// Saves the layout.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="treatNullsAsLists">if set to <c>true</c> treat null lists as empty lists.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> create if not exists.</param>
        /// <returns>
        /// Saved layout entity
        /// </returns>
        Layout SaveLayout(TemplateEditViewModel model, bool treatNullsAsLists = true, bool createIfNotExists = false);

        /// <summary>
        /// Deletes the layout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <returns><c>true</c>, if delete was successful, otherwise <c>false</c></returns>
        bool DeleteLayout(Guid id, int version);
    }
}