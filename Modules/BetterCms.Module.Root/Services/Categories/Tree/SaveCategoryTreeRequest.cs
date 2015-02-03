using System;

namespace BetterCms.Module.Root.Services.Categories.Tree
{
    public class SaveCategoryTreeRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int Version { get; set; }


    }
}