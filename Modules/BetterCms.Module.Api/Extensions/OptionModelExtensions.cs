using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Api.Extensions
{
    public static class OptionModelExtensions
    {
        public static IList<OptionViewModel> ToServiceModel(this IList<OptionModel> model)
        {
            return model
                .Select(o => new OptionViewModel
                    {
                        OptionDefaultValue = o.DefaultValue,
                        OptionKey = o.Key,
                        Type = (OptionType)(short)o.Type,
                        CustomOption = o.Type == Operations.Root.OptionType.Custom 
                            ? new CustomOptionViewModel {Identifier = o.CustomTypeIdentifier} 
                            : null,
                        Translations = o.Translations != null ? o.Translations.Select(ot => new OptionTranslationViewModel
                        {
                            LanguageId = ot.LanguageId,
                            OptionValue = ot.Value
                        }).ToList() : null
                    })
                .ToList();
        }
        
        public static IList<OptionValueEditViewModel> ToServiceModel(this IList<OptionValueModel> model)
        {
            return model
                .Select(o => new OptionValueEditViewModel
                    {
                        OptionDefaultValue = o.DefaultValue,
                        OptionKey = o.Key,
                        Type = (OptionType)(short)o.Type,
                        UseDefaultValue = o.UseDefaultValue,
                        OptionValue = o.Value,
                        CustomOption = o.Type == Operations.Root.OptionType.Custom 
                            ? new CustomOptionViewModel {Identifier = o.CustomTypeIdentifier} 
                            : null
                    })
                .ToList();
        }

        public static IList<ChildContentOptionValuesModel> ToServiceModel(this IList<ContentOptionValuesViewModel> model)
        {
            return model
                .Select(o => new ChildContentOptionValuesModel
                        {
                            AssignmentIdentifier = o.OptionValuesContainerId,
                            OptionValues = o.OptionValues != null ? o.OptionValues
                                .Select(ov => new OptionValueModel()
                                        {
                                            Key = ov.OptionKey,
                                            Value = ov.OptionValue,
                                            DefaultValue = ov.OptionDefaultValue,
                                            Type = ((Operations.Root.OptionType)(int)ov.Type),
                                            UseDefaultValue = ov.UseDefaultValue,
                                            CustomTypeIdentifier = ov.CustomOption != null ? ov.CustomOption.Identifier : null
                                        }).ToList() : null
                        })
                .ToList();
        }
        
        public static IList<MultilingualChildContentOptionValuesModel> ToMultilingualServiceModel(this IList<ContentOptionValuesViewModel> model)
        {
            return model
                .Select(o => new MultilingualChildContentOptionValuesModel
                        {
                            AssignmentIdentifier = o.OptionValuesContainerId,
                            MultilingualOptionValues = o.OptionValues != null ? o.OptionValues
                                .Select(ov => new MultilingualOptionValueModel()
                                        {
                                            Key = ov.OptionKey,
                                            Value = ov.OptionValue,
                                            DefaultValue = ov.OptionDefaultValue,
                                            Type = ((Operations.Root.OptionType)(int)ov.Type),
                                            UseDefaultValue = ov.UseDefaultValue,
                                            CustomTypeIdentifier = ov.CustomOption != null ? ov.CustomOption.Identifier : null,
                                            Translations = ov.ValueTranslations != null ? ov.ValueTranslations.Select(ovt => new OptionTranslationModel
                                            {
                                                LanguageId = ovt.LanguageId,
                                                Value = ovt.OptionValue
                                            }).ToList() : null
                                        })
                                .ToList() : null
                        })
                .ToList();
        }

        public static IList<ContentOptionValuesViewModel> ToViewModel(this IList<ChildContentOptionValuesModel> model)
        {
            return model
                .Select(o => new ContentOptionValuesViewModel
                {
                    OptionValuesContainerId = o.AssignmentIdentifier,
                    OptionValues = o.OptionValues != null ? o.OptionValues
                        .Select(ov => new OptionValueEditViewModel
                        {
                            OptionKey = ov.Key,
                            OptionValue = ov.Value,
                            OptionDefaultValue = ov.DefaultValue,
                            Type = (OptionType)(int)ov.Type,
                            UseDefaultValue = ov.UseDefaultValue,
                            CustomOption = ov.CustomTypeIdentifier != null ? new CustomOptionViewModel { Identifier = ov.CustomTypeIdentifier } : null
                        })
                        .ToList() : null
                })
                .ToList();
        }

        public static IList<ContentOptionValuesViewModel> ToMultilingualViewModel(this IList<MultilingualChildContentOptionValuesModel> model)
        {
            return model
                .Select(o => new ContentOptionValuesViewModel
                {
                    OptionValuesContainerId = o.AssignmentIdentifier,
                    OptionValues = o.MultilingualOptionValues != null ? o.MultilingualOptionValues
                        .Select(ov => new OptionValueEditViewModel
                        {
                            OptionKey = ov.Key,
                            OptionValue = ov.Value,
                            OptionDefaultValue = ov.DefaultValue,
                            Type = (OptionType)(int)ov.Type,
                            UseDefaultValue = ov.UseDefaultValue,
                            CustomOption = ov.CustomTypeIdentifier != null ? new CustomOptionViewModel { Identifier = ov.CustomTypeIdentifier } : null,
                            ValueTranslations = ov.Translations != null ? ov.Translations.Select(ovt => new OptionTranslationViewModel
                            {
                                OptionValue = ovt.Value,
                                LanguageId = ovt.LanguageId
                            }).ToList() : null
                        })
                        .ToList() : null
                })
                .ToList();
        }
 
    }
}