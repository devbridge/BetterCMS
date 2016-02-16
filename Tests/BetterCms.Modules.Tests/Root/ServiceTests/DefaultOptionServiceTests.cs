// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultOptionServiceTests.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Services.Caching;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ServiceTests
{
    [TestFixture]
    public class DefaultOptionServiceTests : TestBase
    {
        private DefaultOptionService CreateOptionService()
        {
            var cmsConfiguration = new Mock<ICmsConfiguration>();
            cmsConfiguration.Setup(x => x.EnableMultilanguage).Returns(true);
            return new DefaultOptionService(null, new HttpRuntimeCacheService(), cmsConfiguration.Object);
        }
        [Test]
        public void Should_Return_MergedEmptyOptionsSuccessfully()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity>();
            var options = new List<IOptionEntity>();

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Should_Return_MergedOptionsSuccessfully()
        {
            var optionValue1 = TestDataProvider.CreateNewPageOption();
            var optionValue2 = TestDataProvider.CreateNewPageOption();
            var optionValue3 = TestDataProvider.CreateNewPageOption();

            optionValue1.Type = optionValue2.Type = optionValue3.Type = OptionType.Text;
            
            optionValue3.Value = null;
            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity> { optionValue1, optionValue2, optionValue3 };
            var options = new List<IOptionEntity>();

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(2, result.Count(o => o.Value != null));
        }

        [Test]
        public void Should_Return_MergedOptionValuesSuccessfully()
        {
            var option1 = TestDataProvider.CreateNewLayoutOption();
            var option2 = TestDataProvider.CreateNewLayoutOption();
            var option3 = TestDataProvider.CreateNewLayoutOption();

            option1.Type = option2.Type = option3.Type = OptionType.Text;
            
            option3.DefaultValue = null;

            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity>();
            var options = new List<IOptionEntity> { option1, option2, option3 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(2, result.Count(o => o.Value != null));
        }

        [Test]
        public void Should_Return_MergedOptionsAndValuesSuccessfully()
        {
            var option1 = TestDataProvider.CreateNewLayoutOption();
            var option2 = TestDataProvider.CreateNewLayoutOption();
            var option3 = TestDataProvider.CreateNewLayoutOption();
            var option4 = TestDataProvider.CreateNewLayoutOption();
            var option5 = TestDataProvider.CreateNewLayoutOption();

            var optionValue1 = TestDataProvider.CreateNewPageOption();
            var optionValue2 = TestDataProvider.CreateNewPageOption();
            var optionValue3 = TestDataProvider.CreateNewPageOption();
            var optionValue4 = TestDataProvider.CreateNewPageOption();
            var optionValue5 = TestDataProvider.CreateNewPageOption();

            optionValue1.Key = option1.Key;
            optionValue2.Key = option2.Key;
            option1.Type = option2.Type = option3.Type = option4.Type = option5.Type = OptionType.Text;
            optionValue1.Type = optionValue2.Type = optionValue3.Type = optionValue4.Type = optionValue5.Type = OptionType.Text;

            option3.DefaultValue = null;
            optionValue4.Value = null;
            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity> { optionValue1, optionValue2, optionValue3, optionValue4, optionValue5 };
            var options = new List<IOptionEntity> { option1, option2, option3, option4, option5 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 8);
            Assert.AreEqual(6, result.Count(o => o.Value != null));
        }

        [Test]
        public void Should_Return_Null_Values_Not_ConvertedToInteger()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity>();
            var options = new List<IOptionEntity>();

            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "not-integer";
            option.Type = OptionType.Integer;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.IsNull(result[0].Value);
        }
        
        [Test]
        public void Should_Return_Null_Values_Not_ConvertedToDateTime()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity>();
            var options = new List<IOptionEntity>();

            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "not-datetime";
            option.Type = OptionType.DateTime;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.IsNull(result[0].Value);
        }
        
        [Test]
        public void Should_Return_Null_Values_Not_ConvertedToBoolean()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity>();
            var options = new List<IOptionEntity>();

            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "not-boolean";
            option.Type = OptionType.Boolean;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.IsNull(result[0].Value);
        }
        
        [Test]
        public void Should_Return_Null_Values_Not_ConvertedToFloat()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity>();
            var options = new List<IOptionEntity>();

            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "not-float";
            option.Type = OptionType.Float;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.IsNull(result[0].Value);
        }

        [Test]
        public void Should_Return_MergedEmptyOptions_ForEdit_Successfully()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity>();
            var options = new List<IOptionEntity>();

            var result = service.GetMergedOptionValuesForEdit(options, optionValues);
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Should_Return_MergedOptions_ForEdit_Successfully()
        {
            var optionValue1 = TestDataProvider.CreateNewPageOption();
            var optionValue2 = TestDataProvider.CreateNewPageOption();
            var optionValue3 = TestDataProvider.CreateNewPageOption();

            optionValue1.Type = optionValue2.Type = optionValue3.Type = OptionType.Text;
            optionValue3.Value = null;

            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity> { optionValue1, optionValue2, optionValue3 };
            var options = new List<IOptionEntity>();

            var result = service.GetMergedOptionValuesForEdit(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void Should_Return_MergedOptionValues_ForEdit_Successfully()
        {
            var option1 = TestDataProvider.CreateNewLayoutOption();
            var option2 = TestDataProvider.CreateNewLayoutOption();
            var option3 = TestDataProvider.CreateNewLayoutOption();

            option1.Type = option2.Type = option3.Type = OptionType.Text;
            option3.DefaultValue = null;

            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity>();
            var options = new List<IOptionEntity> { option1, option2, option3 };

            var result = service.GetMergedOptionValuesForEdit(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void Should_Return_MergedOptionsAndValues_ForEdit_Successfully()
        {
            var option1 = TestDataProvider.CreateNewLayoutOption();
            var option2 = TestDataProvider.CreateNewLayoutOption();
            var option3 = TestDataProvider.CreateNewLayoutOption();
            var option4 = TestDataProvider.CreateNewLayoutOption();
            var option5 = TestDataProvider.CreateNewLayoutOption();

            var optionValue1 = TestDataProvider.CreateNewPageOption();
            var optionValue2 = TestDataProvider.CreateNewPageOption();
            var optionValue3 = TestDataProvider.CreateNewPageOption();
            var optionValue4 = TestDataProvider.CreateNewPageOption();
            var optionValue5 = TestDataProvider.CreateNewPageOption();

            optionValue1.Key = option1.Key;
            optionValue2.Key = option2.Key;
            option1.Type = option2.Type = option3.Type = option4.Type = option5.Type = OptionType.Text;
            optionValue1.Type = optionValue2.Type = optionValue3.Type = optionValue4.Type = optionValue5.Type = OptionType.Text;

            option3.DefaultValue = null;
            optionValue4.Value = null;

            var service = CreateOptionService();
            var optionValues = new List<IOptionValueEntity> { optionValue1, optionValue2, optionValue3, optionValue4, optionValue5 };
            var options = new List<IOptionEntity> { option1, option2, option3, option4, option5 };

            var result = service.GetMergedOptionValuesForEdit(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 8);
            // 3 of 5 values has no equivalent option, so they can be edited
            Assert.AreEqual(result.Count(o => o.CanEditOption), 3);
            // 4 of 5 options has default values
            Assert.AreEqual(result.Count(o => o.OptionDefaultValue == null), 4);
            // 4 of 5 option values has default values
            Assert.AreEqual(result.Count(o => o.OptionValue == o.OptionDefaultValue), 4);
            // 1 option and option value are equal
            Assert.AreEqual(result.Count(o => o.OptionKey == option1.Key 
                && o.OptionDefaultValue == option1.DefaultValue
                && o.OptionValue == optionValue1.Value), 1);
            // 2 option and option value are equal
            Assert.AreEqual(result.Count(o => o.OptionKey == option2.Key 
                && o.OptionDefaultValue == option2.DefaultValue
                && o.OptionValue == optionValue2.Value), 1);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForDefaultLanguage()
        {
            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var result = service.GetMergedOptionValues(options, new List<IOptionValueEntity>());
            Assert.NotNull(result);
            Assert.AreEqual(option1.DefaultValue, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForDefaultLanguage2()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var translation = new ContentOptionTranslation();
            translation.ContentOption = option1;
            translation.Value = TestDataProvider.ProvideRandomString(100);
            translation.Language = lang;
            option1.Translations.Add(translation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var result = service.GetMergedOptionValues(options, new List<IOptionValueEntity>());
            Assert.NotNull(result);
            Assert.AreEqual(option1.DefaultValue, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForDefaultLanguage3()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;

            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(optionValue1.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForDefaultLanguage4()
        {
            var lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            var translation = new ContentOptionTranslation();
            translation.ContentOption = option1;
            translation.Value = TestDataProvider.ProvideRandomString(100);
            translation.Language = lang;
            option1.Translations.Add(translation);
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;

            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(optionValue1.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForDefaultLanguage5()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.UseDefaultValue = true;
            optionValue1.Value = null;

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;
            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.Language = lang;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.ChildContentOption = optionValue1;
            optionValue1.Translations.Add(valueTranslation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(option1.DefaultValue, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForDefaultLanguage6()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            var translation = new ContentOptionTranslation();
            translation.ContentOption = option1;
            translation.Value = TestDataProvider.ProvideRandomString(100);
            translation.Language = lang;
            option1.Translations.Add(translation);
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.UseDefaultValue = true;
            optionValue1.Value = null;

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;
            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.Language = lang;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.ChildContentOption = optionValue1;
            optionValue1.Translations.Add(valueTranslation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(option1.DefaultValue, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForDefaultLanguage7()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();

            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;
            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.Language = lang;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.ChildContentOption = optionValue1;
            optionValue1.Translations.Add(valueTranslation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(optionValue1.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForDefaultLanguage8()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            var translation = new ContentOptionTranslation();
            translation.ContentOption = option1;
            translation.Value = TestDataProvider.ProvideRandomString(100);
            translation.Language = lang;
            option1.Translations.Add(translation);
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;
            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.Language = lang;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.ChildContentOption = optionValue1;
            optionValue1.Translations.Add(valueTranslation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(optionValue1.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForSpecificLanguage()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var result = service.GetMergedOptionValues(options, new List<IOptionValueEntity>(), lang.Id);
            Assert.NotNull(result);
            Assert.AreEqual(option1.DefaultValue, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForSpecificLanguage2()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var translation = new ContentOptionTranslation();
            translation.ContentOption = option1;
            translation.Value = TestDataProvider.ProvideRandomString(100);
            translation.Language = lang;
            option1.Translations.Add(translation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var result = service.GetMergedOptionValues(options, new List<IOptionValueEntity>(), lang.Id);
            Assert.NotNull(result);
            Assert.AreEqual(translation.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForSpecificLanguage3()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;

            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues, lang.Id);
            Assert.NotNull(result);
            Assert.AreEqual(optionValue1.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForSpecificLanguage4()
        {
            var lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            var translation = new ContentOptionTranslation();
            translation.ContentOption = option1;
            translation.Value = TestDataProvider.ProvideRandomString(100);
            translation.Language = lang;
            option1.Translations.Add(translation);
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;

            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues, lang.Id);
            Assert.NotNull(result);
            Assert.AreEqual(optionValue1.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForSpecificLanguage5()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.UseDefaultValue = true;
            optionValue1.Value = null;

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;
            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.Language = lang;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.ChildContentOption = optionValue1;
            optionValue1.Translations.Add(valueTranslation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues, lang.Id);
            Assert.NotNull(result);
            Assert.AreEqual(valueTranslation.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForSpecificLanguage6()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            var translation = new ContentOptionTranslation();
            translation.ContentOption = option1;
            translation.Value = TestDataProvider.ProvideRandomString(100);
            translation.Language = lang;
            option1.Translations.Add(translation);
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.UseDefaultValue = true;
            optionValue1.Value = null;

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;
            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.Language = lang;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.ChildContentOption = optionValue1;
            optionValue1.Translations.Add(valueTranslation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues, lang.Id);
            Assert.NotNull(result);
            Assert.AreEqual(valueTranslation.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForSpecificLanguage7()
        {
            var lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();

            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;
            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.Language = lang;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.ChildContentOption = optionValue1;
            optionValue1.Translations.Add(valueTranslation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues, lang.Id);
            Assert.NotNull(result);
            Assert.AreEqual(valueTranslation.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForSpecificLanguage8()
        {
            Language lang = new Language();
            lang.Id = Guid.NewGuid();
            var option1 = TestDataProvider.CreateNewContentOption();
            var translation = new ContentOptionTranslation();
            translation.ContentOption = option1;
            translation.Value = TestDataProvider.ProvideRandomString(100);
            translation.Language = lang;
            option1.Translations.Add(translation);
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();

            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;
            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.Language = lang;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.ChildContentOption = optionValue1;
            optionValue1.Translations.Add(valueTranslation);
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };

            var result = service.GetMergedOptionValues(options, optionValues, lang.Id);
            Assert.NotNull(result);
            Assert.AreEqual(valueTranslation.Value, result[0].Value);
        }

        [Test]
        public void ShouldReturnCorrectOptionValueForAnotherLanguage()
        {
            var lang1 = new Language();
            lang1.Id = Guid.NewGuid();

            var lang2 = new Language();
            lang2.Id = Guid.NewGuid();

            var option1 = TestDataProvider.CreateNewContentOption();
            option1.Type = OptionType.Text;
            var optionValue1 = TestDataProvider.CreateNewChildContentOption();
            optionValue1.Key = option1.Key;
            optionValue1.Type = option1.Type;

            var valueTranslation = new ChildContentOptionTranslation();
            valueTranslation.ChildContentOption = optionValue1;
            valueTranslation.Value = TestDataProvider.ProvideRandomString(100);
            valueTranslation.Language = lang1;
            optionValue1.Translations.Add(valueTranslation);
            optionValue1.Value = null;
            optionValue1.UseDefaultValue = true;
            var service = CreateOptionService();
            var options = new List<IOptionEntity> { option1 };
            var optionValues = new List<IOptionValueEntity> { optionValue1 };
            var result = service.GetMergedOptionValues(options, optionValues, lang2.Id);
            Assert.NotNull(result);
            Assert.AreEqual(option1.DefaultValue, result[0].Value);
        }
    }
}
