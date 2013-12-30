using System.Collections.Generic;

using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public class UntranslatedPagesFilter : PagesFilter
    {
        public UntranslatedPagesFilter()
        {
            IncludeMasterPages = false;
            OnlyMasterPages = false;
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
        public System.Guid? ExcludedLanguageId { get; set; }

        /// <summary>
        /// Gets the excplicitly included pages array.
        /// </summary>
        /// <value>
        /// The excplicitly included pages array.
        /// </value>
        public System.Guid[] ExcplicitlyIncludedPagesArray
        {
            get
            {
                var result = new List<System.Guid>();
                if (!string.IsNullOrWhiteSpace(ExcplicitlyIncludedPages))
                {
                    foreach (var id in ExcplicitlyIncludedPages.Split('|'))
                    {
                        var guid = id.ToGuidOrDefault();
                        if (!guid.HasDefaultValue())
                        {
                            result.Add(guid);
                        }
                    }
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the existing items.
        /// </summary>
        /// <value>
        /// The existing items.
        /// </value>
        public string ExistingItems { get; set; }

        /// <summary>
        /// Gets the existing items array.
        /// </summary>
        /// <value>
        /// The existing items array.
        /// </value>
        public System.Guid[] ExistingItemsArray
        {
            get
            {
                var result = new List<System.Guid>();
                if (!string.IsNullOrWhiteSpace(ExistingItems))
                {
                    foreach (var id in ExistingItems.Split('|'))
                    {
                        var guid = id.ToGuidOrDefault();
                        if (!guid.HasDefaultValue())
                        {
                            result.Add(guid);
                        }
                    }
                }

                return result.ToArray();
            }
        }
    }
}