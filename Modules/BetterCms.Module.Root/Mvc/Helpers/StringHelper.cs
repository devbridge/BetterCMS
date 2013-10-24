using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class StringHelper
    {
        private const string OneMinus = "-";
        private const string TwoMinus = "--";
        private static IDictionary<char, char> symbolsMap = null;
        private static object lockObject = new object();

        static StringHelper()
        {
            EnsureLatinSybmols();
        }

        public static string UrlHash(this string url)
        {
            url = url.Trim();

            if (url.EndsWith("/") && url != "/")
            {
                url = url.TrimEnd('/');
                url = url.Trim();
            }

            url = url.ToLowerInvariant();

            var md5 = MD5.Create();
            var inputBytes = Encoding.Unicode.GetBytes(url);
            var hash = md5.ComputeHash(inputBytes);

            var result = new StringBuilder();
            foreach (var b in hash)
            {
                result.Append(b.ToString("x2").ToLower());
            }

            return result.ToString();
        }

        public static string Transliterate(this string text, bool allowunicodeCharacters = false)
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

            if (!allowunicodeCharacters)
            {
                text = ReplaceWithLatinSymbols(text);
            }

            while (text.Contains(TwoMinus))
            {
                text = text.Replace(TwoMinus, OneMinus);
            }

            text = text.Trim(new char[] { ' ', '-' }).ToLower();

            return text;
        }

        private static string ReplaceWithLatinSymbols(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (symbolsMap.ContainsKey(text[i]))
                {
                    text = text.Replace(text[i], symbolsMap[text[i]]);
                }
            }

            text = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(text)).Replace("?", string.Empty); 

            return text;
        }

        private static void EnsureLatinSybmols()
        {
            lock (lockObject)
            {
                if (symbolsMap != null)
                {
                    return;
                }

                symbolsMap = new Dictionary<char, char>();

                symbolsMap.Add('ą', 'a');
                symbolsMap.Add('č', 'c');
                symbolsMap.Add('ę', 'e');
                symbolsMap.Add('ė', 'e');
                symbolsMap.Add('į', 'i');
                symbolsMap.Add('š', 's');
                symbolsMap.Add('ų', 'u');
                symbolsMap.Add('ū', 'u');
                symbolsMap.Add('ž', 'z');

                symbolsMap.Add('Ą', 'A');
                symbolsMap.Add('Č', 'C');
                symbolsMap.Add('Ę', 'E');
                symbolsMap.Add('Ė', 'E');
                symbolsMap.Add('Į', 'I');
                symbolsMap.Add('Š', 'S');
                symbolsMap.Add('Ų', 'U');
                symbolsMap.Add('Ū', 'U');
                symbolsMap.Add('Ž', 'Z');

                symbolsMap.Add('б', 'b');
                symbolsMap.Add('Б', 'B');

                symbolsMap.Add('в', 'v');
                symbolsMap.Add('В', 'V');

                symbolsMap.Add('г', 'h');
                symbolsMap.Add('Г', 'H');

                symbolsMap.Add('ґ', 'g');
                symbolsMap.Add('Ґ', 'G');

                symbolsMap.Add('д', 'd');
                symbolsMap.Add('Д', 'D');

                symbolsMap.Add('є', 'e');
                symbolsMap.Add('Э', 'E');

                symbolsMap.Add('ж', 'z');
                symbolsMap.Add('Ж', 'Z');

                symbolsMap.Add('з', 'z');
                symbolsMap.Add('З', 'Z');

                symbolsMap.Add('и', 'y');
                symbolsMap.Add('И', 'Y');

                symbolsMap.Add('ї', 'i');
                symbolsMap.Add('Ї', 'I');

                symbolsMap.Add('й', 'j');
                symbolsMap.Add('Й', 'J');

                symbolsMap.Add('к', 'k');
                symbolsMap.Add('К', 'K');

                symbolsMap.Add('л', 'l');
                symbolsMap.Add('Л', 'L');

                symbolsMap.Add('м', 'm');
                symbolsMap.Add('М', 'M');

                symbolsMap.Add('н', 'n');
                symbolsMap.Add('Н', 'N');

                symbolsMap.Add('п', 'p');
                symbolsMap.Add('П', 'P');

                symbolsMap.Add('р', 'r');
                symbolsMap.Add('Р', 'R');

                symbolsMap.Add('с', 's');
                symbolsMap.Add('С', 'S');

                symbolsMap.Add('ч', 'C');
                symbolsMap.Add('Ч', 'C');

                symbolsMap.Add('ш', 's');
                symbolsMap.Add('Щ', 'S');

                symbolsMap.Add('ю', 'y');
                symbolsMap.Add('Ю', 'Y');

                symbolsMap.Add('Я', 'A');
                symbolsMap.Add('я', 'a');

                symbolsMap.Add('ь', 'j');
                symbolsMap.Add('Ь', 'J');

                symbolsMap.Add('т', 't');
                symbolsMap.Add('Т', 'T');

                symbolsMap.Add('ц', 'c');
                symbolsMap.Add('Ц', 'C');

                symbolsMap.Add('о', 'o');
                symbolsMap.Add('О', 'O');

                symbolsMap.Add('е', 'e');
                symbolsMap.Add('Е', 'E');

                symbolsMap.Add('а', 'a');
                symbolsMap.Add('А', 'A');

                symbolsMap.Add('ф', 'f');
                symbolsMap.Add('Ф', 'F');

                symbolsMap.Add('і', 'i');
                symbolsMap.Add('І', 'I');

                symbolsMap.Add('У', 'U');
                symbolsMap.Add('у', 'u');

                symbolsMap.Add('х', 'x');
                symbolsMap.Add('Х', 'X');
            }
        }

    }
}