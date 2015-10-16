using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services
{
    public interface IChildContentService
    {
        void CollectChildContents(string html, Models.Content content);

        void CopyChildContents(Models.Content destination, Models.Content source);

        void ValidateChildContentsCircularReferences(Models.Content destination, Models.Content source);

        IList<ChildContent> RetrieveChildrenContentsRecursively(bool canManageContent, IEnumerable<System.Guid> contentIds);
        
        void RetrieveChildrenContentsRecursively(bool canManageContent, IEnumerable<Models.Content> contents);
    }
}
