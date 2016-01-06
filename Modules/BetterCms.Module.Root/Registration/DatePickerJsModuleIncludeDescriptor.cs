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
                    new JavaScriptModuleLink(this, "calendarImageUrl", VirtualPath.Combine(module.CssBasePath, "images", "icn-calendar.png"))
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