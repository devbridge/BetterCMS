using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BetterCms.Configuration.Dynamic
{
    public class DynamicCmsConfiguration : ICmsConfiguration
    {
        public DynamicCmsConfiguration(CmsConfigurationSection cmsConfigurationSection)
        {
            Version = cmsConfigurationSection.Version;
            UseMinifiedResources = cmsConfigurationSection.UseMinifiedResources;
            ResourcesBasePath = cmsConfigurationSection.ResourcesBasePath;
            LoginUrl = cmsConfigurationSection.LoginUrl;
            WebSiteUrl = cmsConfigurationSection.WebSiteUrl;
            WorkingDirectoryRootPath = cmsConfigurationSection.WorkingDirectoryRootPath;
            PageNotFoundUrl = cmsConfigurationSection.PageNotFoundUrl;
            ArticleUrlPattern = cmsConfigurationSection.ArticleUrlPattern;
            RenderContentEndingDiv = cmsConfigurationSection.RenderContentEndingDiv;
            ContentEndingDivCssClassName = cmsConfigurationSection.ContentEndingDivCssClassName;
            EnableMultilanguage = cmsConfigurationSection.EnableMultilanguage;
            EnableMacros = cmsConfigurationSection.EnableMacros;

            Modules = new List<ICmsModuleConfiguration>();
            foreach (var module in cmsConfigurationSection.Modules)
            {
                var newModule = new DynamicModule
                {
                    Name = module.Name,
                    KeyValues = module.GetKeyValues().ToList().Select(x => new DynamicKeyValue { Key = x.Key, Value = x.Value }).ToList()
                };
                
                Modules.Add(newModule);
            }

            Storage = cmsConfigurationSection.Storage;
            Database = cmsConfigurationSection.Database;
            Security = cmsConfigurationSection.Security;
            Users = cmsConfigurationSection.Users;
            Search = cmsConfigurationSection.Search;
            ModuleGallery = cmsConfigurationSection.ModuleGallery;
            Installation = cmsConfigurationSection.Installation;
            Cache = cmsConfigurationSection.Cache;
            UrlPatterns = cmsConfigurationSection.UrlPatterns;
            UrlMode = cmsConfigurationSection.UrlMode;
        }

        public string Version { get; private set; }

        public bool UseMinifiedResources { get; private set; }

        public string ResourcesBasePath { get; private set; }

        public string LoginUrl { get; set; }

        public string WebSiteUrl { get; set; }

        public string WorkingDirectoryRootPath { get; private set; }

        public string PageNotFoundUrl { get; set; }

        public string ArticleUrlPattern { get; set; }

        public bool RenderContentEndingDiv { get; private set; }

        public string ContentEndingDivCssClassName { get; private set; }

        public bool EnableMultilanguage { get; private set; }

        public bool EnableMacros { get; private set; }

        public List<ICmsModuleConfiguration> Modules { get; set; }


        public ICmsStorageConfiguration Storage { get; private set; }

        public ICmsDatabaseConfiguration Database { get; private set; }

        public ICmsSecurityConfiguration Security { get; private set; }

        public ICmsUsersConfiguration Users { get; private set; }

        public ICmsSearchConfiguration Search { get; private set; }

        public ICmsModuleGalleryConfiguration ModuleGallery { get; private set; }

        public ICmsInstallationConfiguration Installation { get; private set; }

        public ICmsCacheConfiguration Cache { get; private set; }

        
        

        public UrlPatternsCollection UrlPatterns { get; set; }

        public TrailingSlashBehaviorType UrlMode { get; private set; }
    }
}
