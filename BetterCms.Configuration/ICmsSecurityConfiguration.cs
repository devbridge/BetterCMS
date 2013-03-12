namespace BetterCms
{
    public interface ICmsSecurityConfiguration
    {
        string FullAccessRoles { get; set; }



        // TODO: remove below - obsolete.

        string ContentManagementRoles { get; set; }

        string ContentPublishingRoles { get; set; }        

        string PagePublishingRoles { get; set; }        
    }
}