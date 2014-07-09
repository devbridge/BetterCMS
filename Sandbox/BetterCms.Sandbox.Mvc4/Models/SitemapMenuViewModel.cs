using System.Collections.Generic;

namespace BetterCms.Sandbox.Mvc4.Models
{
    public class SitemapMenuViewModel
    {
        public string LanguageCode { get; set; }

        public bool RenderIFrame { get; set; }

        public List<MenuItemViewModel> ObsoleteMenuItems { get; set; }
        
        public List<MenuItemViewModel> MenuItems { get; set; }
        
        public List<string> LanguageCodes { get; set; }
    }
}