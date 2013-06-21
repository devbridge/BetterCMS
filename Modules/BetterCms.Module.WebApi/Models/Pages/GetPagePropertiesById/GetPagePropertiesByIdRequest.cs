namespace BetterCms.Module.WebApi.Models.Pages.GetPagePropertiesById
{
    public class GetPagePropertiesByIdRequest : RequestBase
    {
        public string PageId { get; set; }

        public bool FetchTags { get; set; }

        public bool FetchCategory { get; set; }

        public bool FetchMetadata { get; set; }

        public bool FetchLayout { get; set; }
    }
}