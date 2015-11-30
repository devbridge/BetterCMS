// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeHelper.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Pages.Helpers
{
    /// <summary>
    /// Helper class for working with date and time.
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// Formats the end date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime? FormatEndDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            if (date.Value != new DateTime(date.Value.Year, date.Value.Month, date.Value.Day))
            {
                return date;
            }

            return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59);
        }

        public static bool IsTheSameDay(DateTime? date1, DateTime? date2)
        {
            return (!date1.HasValue && !date2.HasValue)
                   || (date1.HasValue && date2.HasValue
                       && date1.Value.Year == date2.Value.Year
                       && date1.Value.Month == date2.Value.Month
                       && date1.Value.Day == date2.Value.Day);
        }

        public static DateTime GetFirstIfTheSameDay(DateTime date1, DateTime date2)
        {
            return IsTheSameDay(date1, date2) ? date1 : date2;
        }

        public static DateTime? GetFirstIfTheSameDay(DateTime? date1, DateTime? date2)
        {
            return IsTheSameDay(date1, date2) ? date1 : date2;
        }
    }
}