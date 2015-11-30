// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageContentOptionsTest.cs" company="Devbridge Group LLC">
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
using System.Linq;

using Autofac;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Command.Content.GetPageContentOptions;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Services.Caching;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{
    [TestFixture]
    public class GetPageContentOptionsTest : IntegrationTestBase
    {
        [Test]
        public void Should_Return_Page_Content_Options_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var content = TestDataProvider.CreateNewContent();
                var pageContent = TestDataProvider.CreateNewPageContent(content);

                FillContentWithOptions(content);
                FillPageContentWithOptions(content, pageContent);

                session.SaveOrUpdate(pageContent);
                session.Flush();
                session.Clear();

                // Create command
                var unitOfWork = new DefaultUnitOfWork(session);
                var command = new GetPageContentOptionsCommand();
                var repository = new DefaultRepository(unitOfWork);
                command.UnitOfWork = unitOfWork;
                command.Repository = repository;
                command.CmsConfiguration = Container.Resolve<ICmsConfiguration>();
                command.OptionService = new DefaultOptionService(repository, new HttpRuntimeCacheService(), command.CmsConfiguration);

                // Execute command
                var result = command.Execute(pageContent.Id);

                // Should return 4 options: 2 with assigned values, 1 without parent option and 1 unassigned
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.OptionValues);
                Assert.AreEqual(result.OptionValues.Count, 5);
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == content.ContentOptions[0].Key 
                    && o.OptionValue == pageContent.Options[0].Value
                    && o.OptionDefaultValue == content.ContentOptions[0].DefaultValue
                    && !o.UseDefaultValue
                    && !o.CanEditOption));
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == content.ContentOptions[1].Key
                    && o.OptionValue == pageContent.Options[1].Value
                    && o.OptionDefaultValue == content.ContentOptions[1].DefaultValue
                    && !o.UseDefaultValue
                    && !o.CanEditOption));
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == content.ContentOptions[2].Key
                    && o.OptionValue == content.ContentOptions[2].DefaultValue
                    && o.OptionDefaultValue == content.ContentOptions[2].DefaultValue
                    && o.UseDefaultValue
                    && !o.CanEditOption));
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == pageContent.Options[2].Key
                    && o.OptionValue == pageContent.Options[2].Value
                    && o.OptionDefaultValue == null
                    && o.CanEditOption));
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == pageContent.Options[3].Key
                    && o.OptionValue == pageContent.Options[3].Value
                    && o.OptionDefaultValue == null
                    && o.CanEditOption));
            });
        }

        protected void FillContentWithOptions(Content content)
        {
            var option1 = TestDataProvider.CreateNewContentOption(content);
            var option2 = TestDataProvider.CreateNewContentOption(content);
            var option3 = TestDataProvider.CreateNewContentOption(content);

            option1.Type = OptionType.Text;
            option2.Type = OptionType.Text;
            option3.Type = OptionType.Text;

            content.ContentOptions = new List<ContentOption>();
            content.ContentOptions.Add(option1);
            content.ContentOptions.Add(option2);
            content.ContentOptions.Add(option3);
        }

        protected void FillPageContentWithOptions(Content content, PageContent pageContent)
        {
            var po1 = TestDataProvider.CreateNewPageContentOption(pageContent);
            po1.Key = content.ContentOptions[0].Key;
            po1.Type = OptionType.Text;
            var po2 = TestDataProvider.CreateNewPageContentOption(pageContent);
            po2.Key = content.ContentOptions[1].Key;
            po2.Type = OptionType.Text;
            var po3 = TestDataProvider.CreateNewPageContentOption(pageContent);
            po3.Key = Guid.NewGuid().ToString();
            po3.Type = OptionType.Text;
            var po4 = TestDataProvider.CreateNewPageContentOption(pageContent);
            po4.Key = Guid.NewGuid().ToString();
            po4.Type = OptionType.Text;

            pageContent.Options = new List<PageContentOption>();
            pageContent.Options.Add(po1);
            pageContent.Options.Add(po2);
            pageContent.Options.Add(po3);
            pageContent.Options.Add(po4);
        }
    }
}
