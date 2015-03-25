using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    [Serializable]
    public class UntranslatedPagesGridViewModel<TModel> : PagesGridViewModel<TModel> where TModel : IEditableGridItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UntranslatedPagesGridViewModel{TModel}" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="categories">The categories.</param>
        public UntranslatedPagesGridViewModel(IEnumerable<TModel> items, UntranslatedPagesFilter filter, int totalCount)
            : base(items, filter, totalCount)
        {
            ExcplicitlyIncludedPages = filter.ExcplicitlyIncludedPages;
            ExcludedLanguageId = filter.ExcludedLanguageId;
            ExistingItems = filter.ExistingItems;
        }

        /// <summary>
        /// Gets or sets the excplicitly included pages.
        /// </summary>
        /// <value>
        /// The excplicitly included pages.
        /// </value>
        public string ExcplicitlyIncludedPages { get; set; }

        /// <summary>
        /// Gets or sets the excluded language id.
        /// </summary>
        /// <value>
        /// The excluded language id.
        /// </value>
        public Guid? ExcludedLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the existing items.
        /// </summary>
        /// <value>
        /// The existing items.
        /// </value>
        public string ExistingItems { get; set; }
    }
}