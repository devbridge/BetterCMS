// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using System.Threading;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Web.Modules;
using BetterModules.Core.Web.Mvc.Extensions;

namespace BetterCms.Module.Root.Registration
{
    public class DatePickerJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public DatePickerJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.datepicker")
        {

            Links = new IActionProjection[]
                { 
                    new JavaScriptModuleLink(this, "calendarImageUrl", VirtualPath.Combine(module.CssBasePath, "images", "calendar.svg"))
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "dateFormat", () =>
                        {
                            // C# date format map to jQuery date picked.
                            // References:
                            //  * http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
                            //  * http://www.phpeveryday.com/articles/jQuery-UI-Changing-the-date-format-for-Datepicker-P1023.html

                            var datePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;

                            // Year:
                            if (datePattern.Contains("yyyy"))
                            {
                                // year (four digits).
                                datePattern = datePattern.Replace("yyyy", "yy");
                            }
                            else if(datePattern.Contains("yy"))
                            {
                                // year (two digits).
                                datePattern = datePattern.Replace("yy", "y");
                            }

                            // Month:
                            if (datePattern.Contains("MMMM"))
                            {
                                // long month name.
                                datePattern = datePattern.Replace("MMMM", "MM");
                            }
                            else if (datePattern.Contains("MMM"))
                            {
                                // short month name.
                                datePattern = datePattern.Replace("MMM", "M");
                            }
                            else if (datePattern.Contains("MM"))
                            {
                                // month of year (two digits).
                                datePattern = datePattern.Replace("MM", "mm");
                            }
                            else if (datePattern.Contains("M"))
                            {
                                // month of year (single digit where applicable).
                                datePattern = datePattern.Replace("M", "m");
                            }

                            // Day:
                            if (datePattern.Contains("dddd"))
                            {
                                // full day name.
                                datePattern = datePattern.Replace("dddd", "DD");
                            }
                            else if (datePattern.Contains("ddd"))
                            {
                                // short day name.
                                datePattern = datePattern.Replace("ddd", "D");
                            }

                            return datePattern;
                        }),
                    new JavaScriptModuleGlobalization(this, "currentCulture", () => Thread.CurrentThread.CurrentCulture.Name),
                };
        }
    }
}