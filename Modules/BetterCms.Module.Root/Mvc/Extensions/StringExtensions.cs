// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Root.Mvc.Extensions
{
    public static class StringExtensions
    {
        public static string GetFileName(this string filePath)
        {
            var fileName = filePath;
            if (!string.IsNullOrEmpty(filePath))
            {
                var parts = filePath.Split('/');

                if (parts.Length > 1)
                {
                    fileName = parts[parts.Length - 1];
                }
            }

            return fileName;
        }

        public static List<string> SeparateConvertToList(this string value, char separator)
        {
            var result = new List<string>();

            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            value = value.ToLower();

            var arr = value.Split(separator);

            if (arr.Any())
            {
                result = arr.ToList();
            }
            else
            {
                result.Add(value);
            }

            return result;
        }

        public static string CamelToDash(this string value)
        {
            return CamelToSymbol(value, "-", true);
        }

        public static string CamelToSpaces(this string value, bool makeLowerCase)
        {
            return CamelToSymbol(value, " ", makeLowerCase);
        }

        private static string CamelToSymbol(this string value, string symbol, bool makeLowerCase)
        {
            value = value.Replace(" ", string.Empty);
            var result = System.Text.RegularExpressions.Regex.Replace(value, "([A-Z])", string.Format("{0}$1", symbol), System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            if (makeLowerCase)
            {
                result = result.ToLower();
            }
            if (result.StartsWith(symbol))
            {
                result = result.Substring(symbol.Length, result.Length - symbol.Length);
            }

            return result;
        }

        public static string DashToCamel(this string value, string separator = " ")
        {
            var result = System.Text.RegularExpressions.Regex.Replace(value, @"-\w", s => separator + s.Value.ToUpper().Substring(1, s.Value.Length - 1), System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            return result.Trim().Substring(0, 1).ToUpper() + result.Substring(1, result.Length - 1);
        }
    }
}