using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class CmsModuleGalleryConfigurationElement : ConfigurationElement, ICmsModuleGalleryConfiguration
    {
        private const string FeedUrlAttribute = "feedUrl";

        /// <summary>
        /// Gets or sets the url of nuget feed for BetterCms modules.
        /// </summary>
        [ConfigurationProperty(FeedUrlAttribute, IsRequired = true)]
        public string FeedUrl
        {
            get { return Convert.ToString(this[FeedUrlAttribute]); }
            set { this[FeedUrlAttribute] = value; }
        }
    }
}
