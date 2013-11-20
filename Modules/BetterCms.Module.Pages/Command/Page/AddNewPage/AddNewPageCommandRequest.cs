namespace BetterCms.Module.Pages.Command.Page.AddNewPage
{
    public class AddNewPageCommandRequest
    {
        public string ParentPageUrl { get; set; }

        public bool CreateMasterPage { get; set; }
    }
}