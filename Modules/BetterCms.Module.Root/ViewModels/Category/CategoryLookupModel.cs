using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Root.ViewModels.Category
{
    public class CategoryLookupModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public IList<CategoryLookupModel> children { get; set; }

        public CategoryLookupModel()
        {
            children = new List<CategoryLookupModel>();
        }
    }
}