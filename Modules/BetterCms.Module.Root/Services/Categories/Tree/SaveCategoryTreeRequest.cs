using System;
using System.Collections;
using System.Collections.Generic;

namespace BetterCms.Module.Root.Services.Categories.Tree
{
    public class SaveCategoryTreeRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int Version { get; set; }

        public IEnumerable<CategoryNodeModel> RootNodes { get; set; }
    }
}