using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.ViewModelTests
{
    [TestFixture]
    public class CmsRequestViewModelTest : SerializationTestBase
    {
        [Test]
        public void Should_By_Xml_And_Binary_Serializable()
        {
            CmsRequestViewModel original = new CmsRequestViewModel
                                               {
                                                   Redirect = new RedirectViewModel(TestDataProvider.ProvideRandomString(100)),
                                                   RenderPage = new RenderPageViewModel(
                                                                    new Page
                                                                            {
                                                                                Id = Guid.NewGuid()
                                                                            })
                                               };

            RunSerializationAndDeserialization(original, 
                model =>
                    {
                        Assert.AreEqual(original.Redirect.RedirectUrl, model.Redirect.RedirectUrl);
                        Assert.AreEqual(original.RenderPage.Id, model.RenderPage.Id);
                    });
        }
    }
}
