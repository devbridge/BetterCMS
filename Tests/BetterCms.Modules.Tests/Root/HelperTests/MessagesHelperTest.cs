// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessagesHelperTest.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            var messages = new UserMessages();
            messages.AddSuccess("Success test!");
            messages.AddError("Error test 1!");
            messages.AddError("Error test 2!");

            var controller = new Mock<CmsControllerBase>();
            controller
                .Setup(f => f.Messages)
                .Returns(messages);
            var context = new ViewContext();
            context.Controller = controller.Object;

            IHtmlString box = new HtmlHelper(context, new ViewPage()).MessagesBox("bcms-test-id");
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
                        new HtmlHelper(new ViewContext(), new ViewPage()).MessagesBox("bcms-test-id");
                    });

            Assert.IsInstanceOf<NotSupportedException>(ex.InnerException);
        }
    }
}
