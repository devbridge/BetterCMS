using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace BetterCms.Test.Module.Root.Attributes
{
    [TestFixture]
    public class DisallowHtmlAttributeTests
    {
        private TestViewModel model;

        [SetUp]
        public void Init()
        {
            model = new TestViewModel();
        }

        [TestCaseSource(typeof(AttributeTestDataFactory), "NormalTextCases")]
        public bool Should_Not_Show_Error_For_Normal_Text(string html)
        {
            // Arrange
            model.DisallowHtmlProperty = html;

            // Act
            var results = ValidateModel(model);

            // Assert
            return (results.Count == 0);
        }

        [TestCaseSource(typeof(AttributeTestDataFactory), "UnpairedTagTestCases")]
        [TestCaseSource(typeof(AttributeTestDataFactory), "PairedTagTestCases")]
        [TestCaseSource(typeof(AttributeTestDataFactory), "XssVectorTestCases")]
        public bool Should_Detect_Html(string html)
        {
            // Arrange
            model.DisallowHtmlProperty = html;

            // Act
            var results = ValidateModel(model);

            // Assert
            return (results.Count == 1 && results[0].ErrorMessage == "Field must not contain HTML.");
        }

        private class AttributeTestDataFactory
        {
            public static IEnumerable UnpairedTagTestCases 
            {
                get
                {
                    yield return new TestCaseData("<b>").Returns(true).SetName("Unpaired_Tag");
                    yield return new TestCaseData("<b id=\"test\">").Returns(true).SetName("Unpaired_Tag_With_One_Attribute");
                    yield return new TestCaseData("<b id=\"test\" class=\"test\">").Returns(true).SetName("Unpaired_Tag_With_Multiple_Attributes");
                }
            }

            public static IEnumerable PairedTagTestCases
            {
                get
                {
                    yield return new TestCaseData("<script>alert('title');</script>").Returns(true).SetName("Paired_Tag");
                    yield return new TestCaseData("<script id=\"test\"></script>").Returns(true).SetName("Paired_Tag_With_One_Attribute");
                    yield return new TestCaseData("<script id=\"test\" class=\"test\"></script>").Returns(true).SetName("Paired_Tag_With_Multiple_Attributes");
                }
            }

            public static IEnumerable XssVectorTestCases
            {
                get
                {
                    yield return new TestCaseData("<img src='#' onerror=alert(1) />").Returns(true).SetName("Image_Tag_OnError");
                    yield return new TestCaseData("<<SCRIPT>alert(\"XSS\");//<</SCRIPT>").Returns(true).SetName("Extraneous_Open_Brackets");
                    //yield return new TestCaseData("<<scr\0ipt/src=http://xss.com/xss.js></script").Returns(true).SetName("Invalid_Html"); // possible IE7 XSS
                }
            }

            public static IEnumerable NormalTextCases
            {
                get
                {
                    yield return new TestCaseData("TEST test TeSt").Returns(true).SetName("Normal_Upper_And_Lower_Case_Text");
                    yield return new TestCaseData("Test \"double\" and 'single' quotes").Returns(true).SetName("Text_With_Quotes");
                }
            }
        }

        private static IList<ValidationResult> ValidateModel(object viewModel)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(viewModel, null, null);
            Validator.TryValidateObject(viewModel, ctx, validationResults, true);
            return validationResults;
        }
    }
}