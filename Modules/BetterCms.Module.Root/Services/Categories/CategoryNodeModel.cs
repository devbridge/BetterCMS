using System;
using System.Collections.Generic;

namespace BetterCms.Module.Root.Services.Categories
{
    public class CategoryNodeModel
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public int Version { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public string Macro { get; set; }

        public Guid ParentId { get; set; }

        public IEnumerable<CategoryNodeModel> ChildNodes { get; set; }
    }
}