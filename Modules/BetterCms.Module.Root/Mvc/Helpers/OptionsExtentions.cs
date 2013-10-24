using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class OptionsExtentions
    {
        /// <summary>
        /// Gets the option value from the render widget view model options list.
        /// </summary>
        /// <typeparam name="TType">The type of expected value type.</typeparam>
        /// <param name="model">The render widger view model.</param>
        /// <param name="optionKey">The option key.</param>
        /// <returns>Option value converted to expected value type</returns>
        public static TType GetOptionValue<TType>(this RenderWidgetViewModel model, string optionKey)
        {
            return GetOptionValue<TType>(model.Options, optionKey);
        }

        /// <summary>
        /// Gets the option value from the render page view model options list.
        /// </summary>
        /// <typeparam name="TType">The type of expected value type.</typeparam>
        /// <param name="model">The render page view model.</param>
        /// <param name="optionKey">The option key.</param>
        /// <returns>Option value converted to expected value type</returns>
        public static TType GetOptionValue<TType>(this RenderPageViewModel model, string optionKey)
        {
            return GetOptionValue<TType>(model.Options, optionKey);
        }

        /// <summary>
        /// Gets the option value from the list of options values.
        /// </summary>
        /// <param name="optionValues">The list of option values.</param>
        /// <param name="optionKey">The option key.</param>
        /// <returns>
        /// Option value converted to expected value type
        /// </returns>
        private static TType GetOptionValue<TType>(IEnumerable<IOptionValue> optionValues, string optionKey)
        {
            var optionValue = optionValues.FirstOrDefault(o => o.Key == optionKey);

            if (optionValue != null && optionValue.Value != null)
            {
                try
                {
                    return (TType)optionValue.Value;
                }
                catch
                {
                    // Do Nothing - convertion failed
                }
            }

            return default(TType);
        }

        /// <summary>
        /// Converts a list of option values to the array of java script resources.
        /// </summary>
        /// <param name="optionValues">The list of options values.</param>
        /// <returns>An array of java script resources</returns>
        public static string[] ToJavaScriptResources(this IEnumerable<IOptionValue> optionValues)
        {
            return optionValues
                .Where(o => o.Type == OptionType.JavaScriptUrl && o.Value != null)
                .Select(o => o.Value.ToString())
                .ToArray();
        }
        
        /// <summary>
        /// Converts a list of option values to the array of CSS resources.
        /// </summary>
        /// <param name="optionValues">The list of option values.</param>
        /// <returns>An array of CSS resources</returns>
        public static string[] ToStyleResources(this IEnumerable<IOptionValue> optionValues)
        {
            return optionValues
                .Where(o => o.Type == OptionType.CssUrl && o.Value != null)
                .Select(o => o.Value.ToString())
                .ToArray();
        }
    }
}