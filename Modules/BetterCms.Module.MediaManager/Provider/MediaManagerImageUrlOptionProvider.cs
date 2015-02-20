using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Providers;

namespace BetterCms.Module.MediaManager.Provider
{
    public class MediaManagerImageUrlOptionProvider : ICustomOptionProvider
    {
        public const string Identifier = "media-images-url";

        /// <summary>
        /// Converts the value to correct type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Value, converted to correct type
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertValueToCorrectType(string value)
        {
            return value;
        }

        /// <summary>
        /// Gets the default value for type.
        /// </summary>
        /// <returns>
        /// Default value for provider type
        /// </returns>
        public object GetDefaultValueForType()
        {
            return null;
        }

        /// <summary>
        /// Gets the titles for values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>
        /// The dictionary with value - title pairs
        /// </returns>
        public Dictionary<string, string> GetTitlesForValues(string[] values, IRepository repository)
        {

            Dictionary<string, string> result;
            result = new Dictionary<string, string>();
            if (values.Length > 0)
            {
                values.ToList().ForEach(
                    x =>
                    {
                        if (!string.IsNullOrEmpty(x))
                        {
                            result.Add(x, x);
                        }
                    });


            }
            return result;
        }
    }
}