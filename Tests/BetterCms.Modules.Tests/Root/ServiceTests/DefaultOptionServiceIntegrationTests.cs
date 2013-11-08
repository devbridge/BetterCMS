using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ServiceTests
{
    [TestFixture]
    public class DefaultOptionServiceIntegrationTests : IntegrationTestBase
    {
        [Test]
        public void Should_Save_Options_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var optionService = new DefaultOptionService(repository, new HttpRuntimeCacheService());

                // Create layout with options
                var layout = TestDataProvider.CreateNewLayout();
                layout.LayoutOptions = new List<LayoutOption>();

                var option1 = TestDataProvider.CreateNewLayoutOption(layout);
                option1.Type = OptionType.Text;
                layout.LayoutOptions.Add(option1);

                var option2 = TestDataProvider.CreateNewLayoutOption(layout);
                option2.Type = OptionType.Text;
                layout.LayoutOptions.Add(option2);

                var option3 = TestDataProvider.CreateNewLayoutOption(layout);
                option3.Type = OptionType.Text;
                layout.LayoutOptions.Add(option3);
                
                session.SaveOrUpdate(layout);
                session.Flush();
                session.Clear();
                
                // Create fake options:
                // 1 should be kept
                // 2 should be updated
                // 3 should be inserted
                // option2 should be deleted
                var newOption1 = new LayoutOption { Key = option1.Key, Type = option1.Type, DefaultValue = option1.DefaultValue };
                var newOption2 = new LayoutOption { Key = option2.Key, Type = option2.Type, DefaultValue = TestDataProvider.ProvideRandomString(100) };
                var newOption3 = new LayoutOption { Key = TestDataProvider.ProvideRandomString(100), Type = OptionType.Text, DefaultValue = TestDataProvider.ProvideRandomString(100) };
                var newOptions = new List<IOption> { newOption1, newOption2, newOption3 };

                optionService.SetOptions<LayoutOption, Layout>(layout, newOptions);
                unitOfWork.Commit();

                // Load all options
                var options = repository.AsQueryable<LayoutOption>(lo => lo.Layout == layout).ToList();

                Assert.AreEqual(options.Count, 3);
                Assert.IsTrue(options.Any(o => o.Key == option1.Key && o.DefaultValue == option1.DefaultValue && o.Type == option1.Type));
                Assert.IsTrue(options.Any(o => o.Key == option2.Key && o.DefaultValue == newOption2.DefaultValue && o.Type == option2.Type));
                Assert.IsTrue(options.Any(o => o.Key == newOption3.Key && o.DefaultValue == newOption3.DefaultValue && o.Type == newOption3.Type));
            });
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Should_ThrowValidationException_AboutNonDeletableOption()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var optionService = new DefaultOptionService(repository, new HttpRuntimeCacheService());

                // Create layout with options
                var layout = TestDataProvider.CreateNewLayout();
                layout.LayoutOptions = new List<LayoutOption>();

                var option1 = TestDataProvider.CreateNewLayoutOption(layout);
                option1.Type = OptionType.Text;
                option1.IsDeletable = false;
                layout.LayoutOptions.Add(option1);

                session.SaveOrUpdate(layout);
                session.Flush();
                session.Clear();

                optionService.SetOptions<LayoutOption, Layout>(layout, new List<IOption>());
                unitOfWork.Commit();
            });
        }

        [Test]
        public void ShouldRetrieveMasterPageOptionsSuccessfully()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var pages = CreateNestedOptions(session, 1);

                var optionService = new DefaultOptionService(repository, new HttpRuntimeCacheService());
                var optionValues = optionService.GetMergedMasterPagesOptionValues(pages[0].Id, null, pages[0].Layout.Id);

                Assert.AreEqual(optionValues.Count, 4);
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l1" && o.OptionValue == "l1"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l2" && o.OptionValue == "l2"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l3" && o.OptionValue == "l3p1"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "p1" && o.OptionValue == "p1"));
            });
        }
        
        [Test]
        public void ShouldRetrieveFirstChildPageOptionsSuccessfully()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var pages = CreateNestedOptions(session, 2);

                var optionService = new DefaultOptionService(repository, new HttpRuntimeCacheService());
                var optionValues = optionService.GetMergedMasterPagesOptionValues(pages[1].Id, pages[1].MasterPage.Id, null);

                Assert.AreEqual(optionValues.Count, 5);
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l1" && o.OptionValue == "l1"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l2" && o.OptionValue == "l2p2"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l3" && o.OptionValue == "l3p2"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "p1" && o.OptionValue == "p1"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "p2" && o.OptionValue == "p2"));
            });
        }

        [Test]
        public void ShouldRetrieveSecondChildPageOptionsSuccessfully()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var pages = CreateNestedOptions(session, 3);

                var optionService = new DefaultOptionService(repository, new HttpRuntimeCacheService());
                var optionValues = optionService.GetMergedMasterPagesOptionValues(pages[2].Id, pages[2].MasterPage.Id, null);

                Assert.AreEqual(optionValues.Count, 5);
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l1" && o.OptionValue == "l1"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l2" && o.OptionValue == "l2p2"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "l3" && o.OptionValue == "l3p3"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "p1" && o.OptionValue == "p1"));
                Assert.IsTrue(optionValues.Any(o => o.OptionKey == "p2" && o.OptionValue == "p2"));
            });
        }

        private PageProperties[] CreateNestedOptions(ISession session, int level)
        {
            var pages = new PageProperties[3];

            // Create layout and options
            var layout = TestDataProvider.CreateNewLayout();

            var lo1 = TestDataProvider.CreateNewLayoutOption(layout);
            lo1.Type = OptionType.Text;
            lo1.Key = "l1";
            lo1.DefaultValue = "l1";

            var lo2 = TestDataProvider.CreateNewLayoutOption(layout);
            lo2.Type = OptionType.Text;
            lo2.Key = "l2";
            lo2.DefaultValue = "l2";

            var lo3 = TestDataProvider.CreateNewLayoutOption(layout);
            lo3.Type = OptionType.Text;
            lo3.Key = "l3";
            lo3.DefaultValue = "l3";

            layout.LayoutOptions = new List<LayoutOption> { lo1, lo2, lo3 };
            session.SaveOrUpdate(layout);

            // Create Master page
            var page1 = TestDataProvider.CreateNewPageProperties(layout);

            var o11 = TestDataProvider.CreateNewPageOption(page1);
            o11.Type = OptionType.Text;
            o11.Key = "p1";
            o11.Value = "p1";

            var o12 = TestDataProvider.CreateNewPageOption(page1);
            o12.Type = OptionType.Text;
            o12.Key = "l3";
            o12.Value = "l3p1";

            page1.Options = new List<PageOption> { o11, o12 };
            session.SaveOrUpdate(page1);
            pages[0] = page1;

            if (level > 1)
            {
                // Create First Child With Options
                var page2 = TestDataProvider.CreateNewPageProperties();
                page2.Layout = null;
                page2.MasterPage = page1;

                var o21 = TestDataProvider.CreateNewPageOption(page2);
                o21.Type = OptionType.Text;
                o21.Key = "p2";
                o21.Value = "p2";

                var o22 = TestDataProvider.CreateNewPageOption(page2);
                o22.Type = OptionType.Text;
                o22.Key = "l3";
                o22.Value = "l3p2";

                var o23 = TestDataProvider.CreateNewPageOption(page2);
                o23.Type = OptionType.Text;
                o23.Key = "l2";
                o23.Value = "l2p2";

                page2.Options = new List<PageOption> { o21, o22, o23 };
                session.SaveOrUpdate(page2);
                pages[1] = page2;

                var master21 = new MasterPage { Page = page2, Master = page1 };
                session.SaveOrUpdate(master21);

                if (level > 2)
                {
                    // Create Second Child With Options
                    var page3 = TestDataProvider.CreateNewPageProperties();
                    page3.Layout = null;
                    page3.MasterPage = page2;

                    var o31 = TestDataProvider.CreateNewPageOption(page3);
                    o31.Type = OptionType.Text;
                    o31.Key = "l3";
                    o31.Value = "l3p3";

                    page3.Options = new List<PageOption> { o31 };
                    session.SaveOrUpdate(page3);
                    pages[2] = page3;

                    var master31 = new MasterPage { Page = page3, Master = page1 };
                    session.SaveOrUpdate(master31);
                    var master32 = new MasterPage { Page = page3, Master = page2 };
                    session.SaveOrUpdate(master32);
                }
            }

            session.Flush();
            session.Clear();

            return pages;
        }
    }
}
