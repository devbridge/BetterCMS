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
