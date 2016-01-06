using System.IO;
using System.Reflection;
using System.Text;

using BetterCms.Module.Pages.Helpers;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.HelperTests
{
    public class MarkdownConverterTests
    {
        [Test]
        public void Should_Convert_Markdown_To_Html_Correctly()
        {
            string markdown = GetStream("BetterCms.Test.Module.Contents.Markdown.markdown.txt");
            string html = GetStream("BetterCms.Test.Module.Contents.Markdown.html.txt");

            var result = MarkdownConverter.ToHtml(markdown);

            result = ClearSpaces(result);
            html = ClearSpaces(html);

            Assert.AreEqual(html, result);
        }

        [Test]
        public void Should_Strip_Widget_Paragraphs_Correctly()
        {
            string before = @"<p><widget data-id=""AFA0AFEF-6D71-4962-9EF4-324BB9344F92"" data-assign-id=""FCBD3A46-4A77-4FC9-B9CE-9515C9D6AF77"">Header Logo</widget>    </p>
# {{CmsPageTitle}}
<p>      <widget data-id=""AFA0AFEF-6D71-4962-9EF4-324BB9344F92"" data-assign-id=""FCBD3A46-4A77-4FC9-B9CE-9515C9D6AF77"">Header Logo</widget>   test    </p>
<h1>{{CmsPageTitle}}</h1>
<p>      <widget data-id=""AFA0AFEF-6D71-4962-9EF4-324BB9344F92"" data-assign-id=""FCBD3A46-4A77-4FC9-B9CE-9515C9D6AF77"">Header Logo</widget>           </p>";

            string shouldBe = @"<widget data-id=""AFA0AFEF-6D71-4962-9EF4-324BB9344F92"" data-assign-id=""FCBD3A46-4A77-4FC9-B9CE-9515C9D6AF77"">Header Logo</widget>
<h1>{{CmsPageTitle}}</h1>
<p>      <widget data-id=""AFA0AFEF-6D71-4962-9EF4-324BB9344F92"" data-assign-id=""FCBD3A46-4A77-4FC9-B9CE-9515C9D6AF77"">Header Logo</widget>   test    </p>
<h1>{{CmsPageTitle}}</h1>
<widget data-id=""AFA0AFEF-6D71-4962-9EF4-324BB9344F92"" data-assign-id=""FCBD3A46-4A77-4FC9-B9CE-9515C9D6AF77"">Header Logo</widget>";

            var after = MarkdownConverter.ToHtml(before);

            after = ClearSpaces(after);
            shouldBe = ClearSpaces(shouldBe);

            Assert.AreEqual(after, shouldBe);
        }

        private string ClearSpaces(string text)
        {
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");

            return text;
        }

        private string GetStream(string path)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
