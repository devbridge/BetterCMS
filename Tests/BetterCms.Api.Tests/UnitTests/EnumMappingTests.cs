using System;

using CoreContentStatus = BetterCms.Core.DataContracts.Enums.ContentStatus;
using ApiContentStatus = BetterCms.Module.Api.Operations.Pages.Contents.Content.History.ContentStatus;

using MediaManagerMediaContentType = BetterCms.Module.MediaManager.Models.MediaContentType;
using ApiMediaContentType = BetterCms.Module.Api.Operations.MediaManager.MediaContentType;

using CoreOptionType = BetterCms.Core.DataContracts.Enums.OptionType;
using ApiOptionType = BetterCms.Module.Api.Operations.Root.OptionType;

using CoreAccessLevel = BetterCms.Core.Security.AccessLevel;
using ApiAccessLevel = BetterCms.Module.Api.Operations.Root.AccessLevel;

using CoreForceProtocolType = BetterCms.Core.DataContracts.Enums.ForceProtocolType;
using ApiForceProtocolType = BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties.ForceProtocolType;

using PageContentTextMode = BetterCms.Module.Pages.Models.Enums.ContentTextMode;
using ApiContentTextMode = BetterCms.Module.Api.Operations.Pages.ContentTextMode;

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
            Assert.AreEqual((int)CoreOptionType.MultilineText, (int)ApiOptionType.MultilineText);
            
            Assert.AreEqual((int)CoreOptionType.Custom, (int)ApiOptionType.Custom);
            Assert.AreEqual((int)CoreOptionType.CssUrl, (int)ApiOptionType.CssUrl);
            Assert.AreEqual((int)CoreOptionType.JavaScriptUrl, (int)ApiOptionType.JavaScriptUrl);

            Assert.AreEqual(Enum.GetValues(typeof(CoreOptionType)).Length, 9, "Not all CoreOptionType values are mapped.");
            Assert.AreEqual(Enum.GetValues(typeof(ApiOptionType)).Length, 9, "Not all ApiOptionType are mapped.");
        }
        
        [Test]
        public void ShouldMapAccessLevelValues()
        {
            Assert.AreEqual((int)CoreAccessLevel.Deny, (int)ApiAccessLevel.Deny);
            Assert.AreEqual((int)CoreAccessLevel.Read, (int)ApiAccessLevel.Read);
            Assert.AreEqual((int)CoreAccessLevel.ReadWrite, (int)ApiAccessLevel.ReadWrite);

            Assert.AreEqual(Enum.GetValues(typeof(CoreAccessLevel)).Length, 3, "Not all CoreAccessLevel values are mapped.");
            Assert.AreEqual(Enum.GetValues(typeof(ApiAccessLevel)).Length, 3, "Not all ApiAccessLevel are mapped.");
        }
        
        [Test]
        public void ShouldMapForceProtocolType()
        {
            Assert.AreEqual((int)CoreForceProtocolType.None, (int)ApiForceProtocolType.None);
            Assert.AreEqual((int)CoreForceProtocolType.ForceHttp, (int)ApiForceProtocolType.ForceHttp);
            Assert.AreEqual((int)CoreForceProtocolType.ForceHttps, (int)ApiForceProtocolType.ForceHttps);

            Assert.AreEqual(Enum.GetValues(typeof(CoreForceProtocolType)).Length, 3, "Not all CoreForceProtocolType values are mapped.");
            Assert.AreEqual(Enum.GetValues(typeof(ApiForceProtocolType)).Length, 3, "Not all ApiForceProtocolType are mapped.");
        }
        
        [Test]
        public void ShouldMapContentTextModeValuese()
        {
            Assert.AreEqual((int)PageContentTextMode.Html, (int)ApiContentTextMode.Html);
            Assert.AreEqual((int)PageContentTextMode.Markdown, (int)ApiContentTextMode.Markdown);
            Assert.AreEqual((int)PageContentTextMode.SimpleText, (int)ApiContentTextMode.SimpleText);

            Assert.AreEqual(Enum.GetValues(typeof(PageContentTextMode)).Length, 3, "Not all PageContentTextMode values are mapped.");
            Assert.AreEqual(Enum.GetValues(typeof(ApiContentTextMode)).Length, 3, "Not all ApiContentTextMode are mapped.");
        }
    }
}