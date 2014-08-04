using System.Collections.Generic;

namespace BetterCms.Module.Root.Services
{
    public interface IChildContentService
    {
        void CollectChildContents(string html, Models.Content content);

        void CopyChildContents(Models.Content destination, Models.Content source);

        void ValidateChildContentsCircularReferences(Models.Content destination, Models.Content source);

        void RetrieveChildrenContentsRecursively(IEnumerable<Models.Content> contents);
    }
}
