// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkdownConverterTests.cs" company="Devbridge Group LLC">
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
