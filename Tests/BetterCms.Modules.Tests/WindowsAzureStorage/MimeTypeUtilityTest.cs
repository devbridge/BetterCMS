using System;

using BetterCms.Module.WindowsAzureStorage;

using NUnit.Framework;

namespace BetterCms.Test.Module.WindowsAzureStorage
{
    [TestFixture]
    public class MimeTypeUtilityTest
    {
        [Test]
        public void Should_Determine_ContentType()
        {
            var uri = new Uri("http://www.bettercms.com/test/logo.png");
            var contentType = MimeTypeUtility.DetermineContentType(uri);

            Assert.AreEqual("image/png", contentType);
        }

        [Test]
        public void Should_Determine_ContentType_Uppercase()
        {
            var uri = new Uri("http://www.bettercms.com/test/LOGO.PNG");
            var contentType = MimeTypeUtility.DetermineContentType(uri);

            Assert.AreEqual("image/png", contentType);
        }

        [Test]
        public void Should_Return_Default_ContentType()
        {
            var uri = new Uri("http://www.bettercms.com/test/logo");
            var contentType = MimeTypeUtility.DetermineContentType(uri);

            Assert.AreEqual("application/octet-stream", contentType);
        }
    }
}