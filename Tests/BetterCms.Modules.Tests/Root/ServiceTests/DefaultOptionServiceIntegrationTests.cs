using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

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

                var optionService = new DefaultOptionService(repository);

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

                var optionService = new DefaultOptionService(repository);

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
    }
}
