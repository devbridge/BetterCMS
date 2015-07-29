using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public interface IMasterPageService
    {
        IList<Guid> GetPageMasterPageIds(Guid masterPageId);

        void SetPageMasterPages(Page page, Guid masterPageId);

        void SetPageMasterPages(Page page, IList<Guid> masterPageIds);

        IList<OptionValueEditViewModel> GetMasterPageOptionValues(System.Guid id);

        /// <summary>
        /// Prepares for master page update.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="masterPageId">The master page identifier.</param>
        /// <param name="newMasterIds">The new master ids.</param>
        /// <param name="oldMasterIds">The old master ids.</param>
        /// <param name="childrenPageIds">The children page ids.</param>
        /// <param name="existingChildrenMasterPages">The existing children master pages.</param>
        void PrepareForUpdateChildrenMasterPages(
            PageProperties page,
            Guid? masterPageId,
            out IList<Guid> newMasterIds,
            out IList<Guid> oldMasterIds,
            out IList<Guid> childrenPageIds,
            out IList<MasterPage> existingChildrenMasterPages);

        void SetMasterOrLayout(PageProperties page, Guid? masterPageId, Guid? layoutId);

        /// <summary>
        /// Updates the children master pages.
        /// </summary>
        /// <param name="existingChildrenMasterPages">The existing children master pages.</param>
        /// <param name="oldMasterIds">The old master ids.</param>
        /// <param name="newMasterIds">The new master ids.</param>
        /// <param name="childrenPageIds">The children page ids.</param>
        void UpdateChildrenMasterPages(
            IList<MasterPage> existingChildrenMasterPages,
            IList<Guid> oldMasterIds,
            IList<Guid> newMasterIds,
            IEnumerable<Guid> childrenPageIds);

        int GetLastDynamicRegionNumber();
    }
}