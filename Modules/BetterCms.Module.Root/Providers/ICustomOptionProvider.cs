using System.Collections.Generic;
using BetterModules.Core.DataAccess;

namespace BetterCms.Module.Root.Providers
{
    public interface ICustomOptionProvider
    {
        /// <summary>
        /// Converts the value to correct type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Value, converted to correct type</returns>
        object ConvertValueToCorrectType(string value);

        /// <summary>
        /// Gets the default value for type.
        /// </summary>
        /// <returns>Default value for provider type</returns>
        object GetDefaultValueForType();

        /// <summary>
        /// Gets the titles for values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>The dictionary with value - title pairs</returns>
        Dictionary<string, string> GetTitlesForValues(string[] values, IRepository repository);
    }
}