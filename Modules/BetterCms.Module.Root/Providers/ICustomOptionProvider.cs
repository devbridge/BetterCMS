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
    }
}