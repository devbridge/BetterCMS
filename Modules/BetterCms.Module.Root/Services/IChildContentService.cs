using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.Services
{
    public interface IChildContentService
    {
        string CollectChildContents(string html, Models.Content content);

        void CopyChildContents(Models.Content destination, Models.Content source);

        void ValidateChildContentsCircularReferences(Models.Content destination, Models.Content source);

        void RetrieveChildrenContentsRecursively(IEnumerable<Models.Content> contents);

        IEnumerable<ChildContentProjection> CreateListOfChildProjectionsRecursively(PageContent pageContent, IEnumerable<ChildContent> children);
    }
}
