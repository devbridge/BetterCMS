namespace BetterCms.Module.WebApi.Models.Pages.GetPagePropertiesById
{
    public class GetPagePropertiesByIdRequest : RequestBase
    {
        public string PageId { get; set; }

        public bool IncludeTags { get; set; }

        public bool IncludeCategory { get; set; }

        public bool IncludeMetadata { get; set; }

        public bool IncludeLayout { get; set; }
    }
}