using System;
using System.Security.Principal;

namespace BetterCms.Module.Root.Services.Categories.Tree
{
    public class DeleteCategoryTreeRequest
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public IPrincipal CurrentUser { get; set; }
    }
}