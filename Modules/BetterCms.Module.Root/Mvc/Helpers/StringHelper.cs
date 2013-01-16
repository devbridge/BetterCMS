using System.Text.RegularExpressions;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class StringHelper
    {
        private const string OneMinus = "-";
        private const string TwoMinus = "--";

        public static string Transliterate(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var rgx = new Regex("[ .,_/\\\\+:;?!@]");
            text = rgx.Replace(text, "-").Trim();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != '-' && !char.IsLetterOrDigit(text[i]))
                {
                    text = text.Remove(i, 1);
                    i--;
                }
            }

            while (text.Contains(TwoMinus))
            {
                text = text.Replace(TwoMinus, OneMinus);
            }

            text = text.Trim(new char[] { ' ', '-' }).ToLower();
            return text;
        }
    }
}