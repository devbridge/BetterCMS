using System.Globalization;

namespace BetterCms.Module.Root.Models.Extensions
{
    public static class CultureInfoExtensions
    {
        public static string GetFullName(this CultureInfo culture)
        {
            return string.Format("{0}, {1}, {2}", culture.EnglishName, culture.NativeName, culture.Name);
        }
    }
}