// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionTypeExtensions.cs" company="Devbridge Group LLC">
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

                case OptionType.MultilineText:
                    return RootGlobalization.OptionTypes_MultilineText_Title;

                default:
                    throw new NotSupportedException(string.Format("Not supported option type: {0}", type));
            }
        }
    }
}