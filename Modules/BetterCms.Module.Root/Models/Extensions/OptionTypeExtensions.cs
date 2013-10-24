using System;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Models.Extensions
{
    public static class OptionTypeExtensions
    {
        public static string ToGlobalizationString(this OptionType type)
        {
            switch (type)
            {
                case OptionType.Text:
                    return RootGlobalization.OptionTypes_Text_Title;

                case OptionType.Integer:
                    return RootGlobalization.OptionTypes_Integer_Title;

                case OptionType.Float:
                    return RootGlobalization.OptionTypes_Float_Title;

                case OptionType.DateTime:
                    return RootGlobalization.OptionTypes_DateTime_Title;

                case OptionType.Boolean:
                    return RootGlobalization.OptionTypes_Boolean_Title;

                default:
                    throw new NotSupportedException(string.Format("Not supported option type: {0}", type));
            }
        }
    }
}