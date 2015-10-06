using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
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
            return new DefaultOptionService(null, new HttpRuntimeCacheService(), new Mock<ICmsConfiguration>().Object);
        }
        [Test]
        public void Should_Return_MergedEmptyOptionsSuccessfully()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionEntity>();
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
            var optionValues = new List<IOptionEntity> { optionValue1, optionValue2, optionValue3 };
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
            var optionValues = new List<IOptionEntity>();
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
            var optionValues = new List<IOptionEntity> { optionValue1, optionValue2, optionValue3, optionValue4, optionValue5 };
            var options = new List<IOptionEntity> { option1, option2, option3, option4, option5 };

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 8);
            Assert.AreEqual(6, result.Count(o => o.Value != null));
        }

        [Test]
        public void Should_Return_ValuesConvertedToInteger()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionEntity>();
            var options = new List<IOptionEntity>();
            
            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "580";
            option.Type = OptionType.Integer;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Value is int, true);
            Assert.AreEqual(result[0].Value, 580);
        }
        
        [Test]
        public void Should_Return_ValuesConvertedToLongInteger()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionEntity>();
            var options = new List<IOptionEntity>();
            
            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "4294967296";
            option.Type = OptionType.Integer;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Value is long, true);
            Assert.AreEqual(result[0].Value, 4294967296);
        }
        
        [Test]
        public void Should_Return_ValuesConvertedToDateTime()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionEntity>();
            var options = new List<IOptionEntity>();

            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "2010-10-10";
            option.Type = OptionType.DateTime;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Value is DateTime, true);
            Assert.AreEqual(result[0].Value, new DateTime(2010, 10, 10));
        }
        
        [Test]
        public void Should_Return_ValuesConvertedToBoolean()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionEntity>();
            var options = new List<IOptionEntity>();

            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "true";
            option.Type = OptionType.Boolean;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Value is bool, true);
            Assert.AreEqual(result[0].Value, true);
        }
        
        [Test]
        public void Should_Return_ValuesConvertedToFloat()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionEntity>();
            var options = new List<IOptionEntity>();

            var option = TestDataProvider.CreateNewLayoutOption();
            option.DefaultValue = "10.5";
            option.Type = OptionType.Float;
            options.Add(option);

            var result = service.GetMergedOptionValues(options, optionValues);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Value is decimal, true);
            Assert.AreEqual(result[0].Value, 10.5M);
        }
        
        [Test]
        public void Should_Return_Null_Values_Not_ConvertedToInteger()
        {
            var service = CreateOptionService();
            var optionValues = new List<IOptionEntity>();
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
            var optionValues = new List<IOptionEntity>();
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
            var optionValues = new List<IOptionEntity>();
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
            var optionValues = new List<IOptionEntity>();
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
            var optionValues = new List<IOptionEntity>();
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
            var optionValues = new List<IOptionEntity> { optionValue1, optionValue2, optionValue3 };
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
            var optionValues = new List<IOptionEntity>();
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
            var optionValues = new List<IOptionEntity> { optionValue1, optionValue2, optionValue3, optionValue4, optionValue5 };
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
    }
}
