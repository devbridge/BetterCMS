namespace BetterCms
{
    public interface ICmsSecurityConfiguration
    {
        string FullAccessRoles { get; set; }

        bool UseCustomRoles { get; set; }

        string Translate(string accessRole);


        // TODO: remove below - obsolete.

        string ContentManagementRoles { get; set; }

        string ContentPublishingRoles { get; set; }        

        string PagePublishingRoles { get; set; }        
    }
}