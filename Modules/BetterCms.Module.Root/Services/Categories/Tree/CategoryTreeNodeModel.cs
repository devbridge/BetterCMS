using System;

namespace BetterCms.Module.Root.Services.Categories.Tree
{
    public class CategoryTreeNodeModel
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public int Version { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public string Macro { get; set; }

        public Guid? parentId { get; set; }
    }
}