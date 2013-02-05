using System.Web;
using System.Web.Mvc;

using BetterCms.Module.Root.Mvc.Helpers;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.HelperTests
{
    [TestFixture]
    public class FormHelperTest
    {
        [Test]
        public void Should_Render_Hidden_Submit_Button()
        {
            IHtmlString button = new HtmlHelper(new ViewContext(), new ViewPage()).HiddenSubmit();
            string html = button.ToHtmlString().Trim();
            Assert.IsTrue(html.StartsWith("<input "));
            Assert.IsTrue(html.EndsWith("/>"));
            Assert.IsTrue(html.Contains("position:absolute; left:-999em; top:-999em;") || html.Contains("position: absolute; left: -999em; top: -999em;"));
        }
    }
}
