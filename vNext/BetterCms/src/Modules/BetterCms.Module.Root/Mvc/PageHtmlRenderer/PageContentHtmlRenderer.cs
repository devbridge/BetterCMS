using System.Collections.Generic;
using System.Text;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    /// <summary>
    /// Helper class, helps to render page content HTML
    /// </summary>
    public static class PageContentHtmlRenderer
    {
        /// <summary>
        /// The rendering page properties
        /// </summary>
        private readonly static IDictionary<string, IRenderingProperty> properties;

        /// <summary>
        /// Initializes the <see cref="PageHtmlRenderer" /> class.
        /// </summary>
        static PageContentHtmlRenderer()
        {
            properties = new Dictionary<string, IRenderingProperty>();

            Register(new RenderingContentOptionProperty());
        }

        /// <summary>
        /// Registers the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        public static void Register(IRenderingProperty property)
        {
            if (!properties.ContainsKey(property.Identifier))
            {
                properties.Add(property.Identifier, property);
            }
        }

        /// <summary>
        /// Replaces HTML with data from page view model.
        /// </summary>
        /// <returns>Replaced HTML</returns>
        public static StringBuilder GetReplacedHtml(StringBuilder stringBuilder, IList<IOptionValue> values)
        {
            foreach (var property in properties)
            {
                var renderingOption = property.Value as IRenderingOption;
                if (renderingOption != null)
                {
                    stringBuilder = renderingOption.GetReplacedHtml(stringBuilder, values);
                }
            }

            return stringBuilder;
        }
    }
}