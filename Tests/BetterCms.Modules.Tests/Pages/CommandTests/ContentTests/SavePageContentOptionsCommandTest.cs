using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Command.Content.SavePageContentOptions;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{
    [TestFixture]
    public class SavePageContentOptionsCommandTest : GetPageContentOptionsTest
    {
        [Test]
        public void Should_Save_Page_Content_Options_Successfully()
        {
            RunActionInTransaction(session =>
            {
                // Create page content with options
                var content = TestDataProvider.CreateNewContent();
                var pageContent = TestDataProvider.CreateNewPageContent(content);
                FillContentWithOptions(content);
                FillPageContentWithOptions(content, pageContent);

                session.SaveOrUpdate(pageContent);
                session.Flush();
                session.Clear();

                // Random option value
                var randomOptionValue = TestDataProvider.CreateNewPageContentOption();

                // Create request
                var request = new PageContentOptionsViewModel
                    {
                        WidgetOptions = new List<PageContentOptionViewModel>
                            {
                                  new PageContentOptionViewModel
                                      {
                                          // Will be deleted because of default value
                                          OptionValue = content.ContentOptions[0].DefaultValue,
                                          OptionKey = pageContent.Options[0].Key,
                                          OptionDefaultValue = content.ContentOptions[0].DefaultValue
                                      },
                                  new PageContentOptionViewModel
                                      {
                                          // Will be deleted because of null value
                                          OptionValue = null,
                                          OptionKey = pageContent.Options[1].Key
                                      },
                                  new PageContentOptionViewModel
                                      {
                                          OptionValue = pageContent.Options[2].Value,
                                          OptionKey = pageContent.Options[2].Key
                                      },
                                  new PageContentOptionViewModel
                                      {
                                          // Random value
                                          OptionValue = randomOptionValue.Value,
                                          OptionKey = randomOptionValue.Key
                                      }

                            },
                        PageContentId = pageContent.Id
                    };

                // Create command
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);
                var command = new SavePageContentOptionsCommand();
                command.UnitOfWork = unitOfWork;
                command.Repository = repository;
                var result = command.Execute(request);

                Assert.IsTrue(result);

                // Check results: one of page content values must be deleted after save
                var results = repository
                    .AsQueryable<PageContentOption>(pco => pco.PageContent.Id == pageContent.Id
                        && !pco.IsDeleted
                        && !pco.PageContent.IsDeleted)
                    .ToList();
                Assert.AreEqual(results.Count, 2);
                Assert.IsNotNull(results.FirstOrDefault(pco => pco.Key == pageContent.Options[2].Key
                    && pco.Value == pageContent.Options[2].Value));
                Assert.IsNotNull(results.FirstOrDefault(pco => pco.Key == randomOptionValue.Key
                    && pco.Value == randomOptionValue.Value));
            });
        }
    }
}
