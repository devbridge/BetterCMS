using System.Configuration;

using BetterCms.Test.Module.Configuration;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module
{
    public abstract class StorageTestBase : TestBase
    {
        protected const string TestImagePath = "BetterCms.Test.Module.Contents.Images.logo.png";
        protected const string TestImageFileName = "logo.png";
        protected const string TestImageCopyFileName = "logo.copy.png";

        /// <summary>
        /// Mocks the configuration.
        /// </summary>
        /// <returns>Mocked CMS configuration</returns>
        protected ICmsConfiguration MockConfiguration()
        {
            var serviceSection = (CmsTestConfigurationSection)ConfigurationManager.GetSection(CmsTestConfigurationSection.StorageSectionName);

            ICmsStorageConfiguration storageConfiguration = GetStorageConfiguration(serviceSection);
            if (storageConfiguration == null)
            {
                Assert.Ignore("Set up Config/storage.config values or Environment values for storage tests.");
            }

            Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();
            cmsConfigurationMock.Setup(f => f.Storage).Returns(storageConfiguration);

            return cmsConfigurationMock.Object;
        }

        /// <summary>
        /// Gets the storage configuration.
        /// </summary>
        /// <param name="serviceSection">The service section.</param>
        /// <returns>Storage Configuration</returns>
        protected abstract ICmsStorageConfiguration GetStorageConfiguration(CmsTestConfigurationSection serviceSection);
    }
}
