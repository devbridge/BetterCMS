// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmsRequestViewModelTest.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.ViewModelTests
{
    [TestFixture]
    public class CmsRequestViewModelTest : SerializationTestBase
    {
        [Test]
        public void Should_By_Xml_And_Binary_Serializable()
        {
            CmsRequestViewModel original = new CmsRequestViewModel
                                               {
                                                   Redirect = new RedirectViewModel(TestDataProvider.ProvideRandomString(100)),
                                                   RenderPage = new RenderPageViewModel(
                                                                    new Page
                                                                            {
                                                                                Id = Guid.NewGuid()
                                                                            })
                                               };

            RunSerializationAndDeserialization(original, 
                model =>
                    {
                        Assert.AreEqual(original.Redirect.RedirectUrl, model.Redirect.RedirectUrl);
                        Assert.AreEqual(original.RenderPage.Id, model.RenderPage.Id);
                    });
        }
    }
}
