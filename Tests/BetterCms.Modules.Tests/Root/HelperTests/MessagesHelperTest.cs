using System;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Exceptions;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using BetterModules.Core.Web.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.HelperTests
{
    [TestFixture]
    public class MessagesHelperTest
    {
        [Test]
        public void Should_Render_Succes_Info_Warning_And_Error_Message_Blocks()
        {
            UserMessages messages = new UserMessages();
            messages.AddSuccess("Success test!");
            messages.AddError("Error test 1!");
            messages.AddError("Error test 2!");

            Mock<CmsControllerBase> controller = new Mock<CmsControllerBase>();
            controller
                .Setup(f => f.Messages)
                .Returns(messages);
            var context = new ViewContext();
            context.Controller = controller.Object;

            IHtmlString box = new HtmlHelper(context, new ViewPage()).SiteSettingsMessagesBox("bcms-test-id");
            string html = box.ToHtmlString().Trim();
            
            Assert.IsTrue(html.Contains("id=\"bcms-test-id\""));
            Assert.IsTrue(html.Contains("Success test!"));
            Assert.IsTrue(html.Contains("Error test 1!"));
            Assert.IsTrue(html.Contains("Error test 2!"));
        }

        [Test]        
        public void Throws_CmsException_If_Controller_Not_Inherits_CmsControllerBase()
        {
            CmsException ex = Assert.Throws<CmsException>(
                () =>
                    {
                        new HtmlHelper(new ViewContext(), new ViewPage()).SiteSettingsMessagesBox("bcms-test-id");
                    });

            Assert.IsInstanceOf<NotSupportedException>(ex.InnerException);
        }
    }
}
