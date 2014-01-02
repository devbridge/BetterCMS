using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class PageLookupKeyValue : LookupKeyValue
    {
        public System.Guid? LanguageId { get; set; }
        
        public string PageUrl { get; set; }
    }
}