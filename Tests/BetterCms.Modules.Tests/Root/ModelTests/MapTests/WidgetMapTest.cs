using System.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class WidgetMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Widget_Successfully()
        {
            var entity = TestDataProvider.CreateNewWidget();
            RunEntityMapTestsInTransaction(entity);
        }

        [Test]
        public void Should_Insert_And_Retrieve_Widget_ContentOptions_Successfully()
        {
            var widget = TestDataProvider.CreateNewWidget();

            var contentOptions = new[]
                {
                    TestDataProvider.CreateNewContentOption(widget),
                    TestDataProvider.CreateNewContentOption(widget)
                };
            widget.ContentOptions = contentOptions;

            SaveEntityAndRunAssertionsInTransaction(
                widget,
                result =>
                {
                    Assert.AreEqual(widget, result);
                    Assert.AreEqual(contentOptions.OrderBy(f => f.Id), result.ContentOptions.OrderBy(f => f.Id));
                });
        }
    }
}
