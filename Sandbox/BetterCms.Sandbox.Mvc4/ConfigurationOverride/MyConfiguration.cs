namespace BetterCms.Sandbox.Mvc4.ConfigurationOverride
{
    public class MyConfiguration : IMyConfiguration
    {
        public string GetStorageConnectionString()
        {
            // blah-blah, some clever way to get storage connection string

            return "DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=blahblahblahblahblahblahblahblahblahblahblahblahblahblahblahblah==";
        }


        public string GetCmsStorageContainerName()
        {
            return "cmsfilestest";
        }
    }
}