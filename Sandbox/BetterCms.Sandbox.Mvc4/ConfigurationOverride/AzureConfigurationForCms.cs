using BetterCms.Configuration;


namespace BetterCms.Sandbox.Mvc4.ConfigurationOverride
{
    //public class AzureConfigurationForCms : CmsConfigurationSection, ICmsConfiguration
    //{
    //    private MyAzureStorageConfiguration cachedStorageConfiguration;
    //    ICmsStorageConfiguration ICmsConfiguration.Storage
    //    {
    //        get
    //        {
    //            if (cachedStorageConfiguration == null)
    //            {
    //                cachedStorageConfiguration = new MyAzureStorageConfiguration();
    //            }
    //            return cachedStorageConfiguration;
    //        }
    //    }
    //}

    public class ProxyConfigurationForCms : ICmsConfiguration
    {
        private readonly ICmsConfiguration realConfiguration;

        public ProxyConfigurationForCms(ICmsConfiguration realConfiguration)
        {
            this.realConfiguration = realConfiguration;
        }



        public string Version
        {
            get { return realConfiguration.Version; }
        }

        public bool UseMinifiedResources
        {
            get { return realConfiguration.UseMinifiedResources; }
        }

        public string ResourcesBasePath
        {
            get { return realConfiguration.ResourcesBasePath; }
        }

        public string LoginUrl
        {
            get { return realConfiguration.LoginUrl; }
            set { realConfiguration.LoginUrl = value; }
        }

        public string WebSiteUrl
        {
            get { return realConfiguration.WebSiteUrl; }
            set { realConfiguration.WebSiteUrl = value; }
        }

        public string WorkingDirectoryRootPath { get; private set; }


        private MyAzureStorageConfiguration cachedStorageConfiguration;
        public ICmsStorageConfiguration Storage
        {
            get
            {
                if (cachedStorageConfiguration == null)
                {
                    cachedStorageConfiguration = new MyAzureStorageConfiguration();
                }
                return cachedStorageConfiguration;
            }
        }

        public ICmsDatabaseConfiguration Database
        {
            get { return realConfiguration.Database; }
        }

        public ICmsSecurityConfiguration Security
        {
            get { return realConfiguration.Security; }
        }

        public ICmsUsersConfiguration Users
        {
            get { return realConfiguration.Users; }
        }

        public ICmsSearchConfiguration Search
        {
            get { return realConfiguration.Search; }
        }

        public string PageNotFoundUrl
        {
            get { return realConfiguration.PageNotFoundUrl; }
            set { realConfiguration.PageNotFoundUrl = value; }
        }

        public string ArticleUrlPattern
        {
            get { return realConfiguration.ArticleUrlPattern; }
            set { realConfiguration.ArticleUrlPattern = value; }
        }

        public UrlPatternsCollection UrlPatterns
        {
            get { return realConfiguration.UrlPatterns; }
            set { realConfiguration.UrlPatterns = value; }
        }

        public ICmsModuleGalleryConfiguration ModuleGallery
        {
            get { return realConfiguration.ModuleGallery; }
        }

        public ICmsInstallationConfiguration Installation
        {
            get { return realConfiguration.Installation; }
        }

        public ICmsCacheConfiguration Cache
        {
            get { return realConfiguration.Cache; }
        }

        public TrailingSlashBehaviorType UrlMode
        {
            get { return realConfiguration.UrlMode; }
        }

        public bool RenderContentEndingDiv
        {
            get { return realConfiguration.RenderContentEndingDiv; }
        }

        public string ContentEndingDivCssClassName
        {
            get { return realConfiguration.ContentEndingDivCssClassName; }
        }

        public bool EnableMultilanguage
        {
            get { return realConfiguration.EnableMultilanguage; }
        }

        public bool EnableMacros
        {
            get { return realConfiguration.EnableMacros; }
        }
    }    
}