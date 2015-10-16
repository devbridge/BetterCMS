using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.ViewModels.Security
{
    public class UserAccessTemplateViewModel
    {
        public UserAccessTemplateViewModel()
        {
            Title = RootGlobalization.AccessControl_UserAccess_Title;
            Tooltip = RootGlobalization.AccessControl_UserAccess_Tooltip_Description;
        }

        public string Title { get; set; }

        public string Tooltip { get; set; }

        public override string ToString()
        {
            return string.Format("Title: {0}, Tooltip: {1}", Title, Tooltip);
        }
    }
}