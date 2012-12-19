namespace BetterCms
{
    public interface ICmsSecurityConfiguration
    {
        string ContentManagementRoles { get; set; }

        string ContentPublishingRoles { get; set; }        

        string PagePublishingRoles { get; set; }        
    }
}