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
                            : null
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
                                .Select(ov => new OptionValueModel
                                        {
                                            Key = ov.OptionKey,
                                            Value = ov.OptionValue,
                                            DefaultValue = ov.OptionDefaultValue,
                                            Type = ((Operations.Root.OptionType)(int)ov.Type),
                                            UseDefaultValue = ov.UseDefaultValue,
                                            CustomTypeIdentifier = ov.CustomOption != null ? ov.CustomOption.Identifier : null
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
    }
}