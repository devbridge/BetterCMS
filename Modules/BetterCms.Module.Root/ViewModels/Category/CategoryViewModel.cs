using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Root.ViewModels.Category
{
    public class CategoryViewModel
    {
        public Guid CategoryTreeId { get; set; }

        public Guid ?ParentCategoryId { get; set; }

        public Guid Id { get; set; }

        public int Version { get; set; }  
               
        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public override string ToString()
        {
            return string.Format("CategoryTreeId: {0}, ParentCategoryId: {1}, Id: {2}, Version: {3}, Title: {4}, DisplayOrder: {5}", CategoryTreeId, ParentCategoryId, Id, Version, Title, DisplayOrder);
        }
    }
}