using System;

using CoreContentStatus = BetterCms.Core.DataContracts.Enums.ContentStatus;
using ApiContentStatus = BetterCms.Module.Api.Operations.Pages.Contents.Content.History.ContentStatus;

using MediaManagerMediaContentType = BetterCms.Module.MediaManager.Models.MediaContentType;
using ApiMediaContentType = BetterCms.Module.Api.Operations.MediaManager.MediaContentType;

using CoreOptionType = BetterCms.Core.DataContracts.Enums.OptionType;
using ApiOptionType = BetterCms.Module.Api.Operations.Root.OptionType;

using NUnit.Framework;

namespace BetterCms.Api.Tests.UnitTests
{
    [TestFixture]
    public class EnumMappingTests
    {
        [Test]
        public void ShouldMapContentStatusValues()
        {
            Assert.AreEqual((int)CoreContentStatus.Archived, (int)ApiContentStatus.Archived);
            Assert.AreEqual((int)CoreContentStatus.Draft, (int)ApiContentStatus.Draft);
            Assert.AreEqual((int)CoreContentStatus.Preview, (int)ApiContentStatus.Preview);
            Assert.AreEqual((int)CoreContentStatus.Published, (int)ApiContentStatus.Published);

            Assert.AreEqual(Enum.GetValues(typeof(CoreContentStatus)).Length, 4, "Not all Core.ContentStatus values are mapped.");
            Assert.AreEqual(Enum.GetValues(typeof(ApiContentStatus)).Length, 4, "Not all Api.ContentStatus values are mapped.");
        }
        
        [Test]
        public void ShouldMapMediaContentTypeValues()
        {
            Assert.AreEqual((int)MediaManagerMediaContentType.File, (int)ApiMediaContentType.File);
            Assert.AreEqual((int)MediaManagerMediaContentType.Folder, (int)ApiMediaContentType.Folder);

            Assert.AreEqual(Enum.GetValues(typeof(MediaManagerMediaContentType)).Length, 2, "Not all MediaManager.MediaContentType values are mapped.");
            Assert.AreEqual(Enum.GetValues(typeof(ApiMediaContentType)).Length, 2, "Not all Api.MediaContentTypevalues are mapped.");
        }
        
        [Test]
        public void ShouldMapOptionTypeValues()
        {
            Assert.AreEqual((int)CoreOptionType.Text, (int)ApiOptionType.Text);
            Assert.AreEqual((int)CoreOptionType.Float, (int)ApiOptionType.Float);
            Assert.AreEqual((int)CoreOptionType.Integer, (int)ApiOptionType.Integer);
            Assert.AreEqual((int)CoreOptionType.DateTime, (int)ApiOptionType.DateTime);
            Assert.AreEqual((int)CoreOptionType.Boolean, (int)ApiOptionType.Boolean);
            
            Assert.AreEqual((int)CoreOptionType.Custom, (int)ApiOptionType.Custom);
            Assert.AreEqual((int)CoreOptionType.CssUrl, (int)ApiOptionType.CssUrl);
            Assert.AreEqual((int)CoreOptionType.JavaScriptUrl, (int)ApiOptionType.JavaScriptUrl);

            Assert.AreEqual(Enum.GetValues(typeof(MediaManagerMediaContentType)).Length, 2, "Not all MediaManager.MediaContentType values are mapped.");
            Assert.AreEqual(Enum.GetValues(typeof(ApiMediaContentType)).Length, 2, "Not all Api.MediaContentTypevalues are mapped.");
        }
    }
}