// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MimeTypeUtilityTest.cs" company="Devbridge Group LLC">
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