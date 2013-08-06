using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Command.Content.GetPageContentOptions;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{
    [TestFixture]
    public class GetPageContentOptionsTest : DatabaseTestBase
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
                command.OptionService = new DefaultOptionService(repository);

                // Execute command
                var result = command.Execute(pageContent.Id);

                // Should return 4 options: 2 with assigned values, 1 without parent option and 1 unassigned
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.OptionValues);
                Assert.AreEqual(result.OptionValues.Count, 4);
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == content.ContentOptions[0].Key 
                    && o.OptionValue == pageContent.Options[0].Value
                    && o.OptionDefaultValue == content.ContentOptions[0].DefaultValue));
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == content.ContentOptions[1].Key
                    && o.OptionValue == pageContent.Options[1].Value
                    && o.OptionDefaultValue == content.ContentOptions[1].DefaultValue));
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == content.ContentOptions[2].Key
                    && o.OptionValue == null
                    && o.OptionDefaultValue == content.ContentOptions[2].DefaultValue));
                Assert.IsNotNull(result.OptionValues.FirstOrDefault(o => o.OptionKey == pageContent.Options[2].Key
                    && o.OptionValue == pageContent.Options[2].Value
                    && o.OptionDefaultValue == null));
            });
        }

        protected void FillContentWithOptions(Content content)
        {
            var option1 = TestDataProvider.CreateNewContentOption(content);
            var option2 = TestDataProvider.CreateNewContentOption(content);
            var option3 = TestDataProvider.CreateNewContentOption(content);

            content.ContentOptions = new List<ContentOption>();
            content.ContentOptions.Add(option1);
            content.ContentOptions.Add(option2);
            content.ContentOptions.Add(option3);
        }

        protected void FillPageContentWithOptions(Content content, PageContent pageContent)
        {
            var po1 = TestDataProvider.CreateNewPageContentOption(pageContent);
            po1.Key = content.ContentOptions[0].Key;
            var po2 = TestDataProvider.CreateNewPageContentOption(pageContent);
            po2.Key = content.ContentOptions[1].Key;
            var po3 = TestDataProvider.CreateNewPageContentOption(pageContent);
            po3.Key = Guid.NewGuid().ToString();

            pageContent.Options = new List<PageContentOption>();
            pageContent.Options.Add(po1);
            pageContent.Options.Add(po2);
            pageContent.Options.Add(po3);
        }
    }
}
