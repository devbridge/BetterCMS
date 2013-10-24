using System;
using System.Globalization;
using System.Text;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Root.Mvc.Grids
{
    public class PagerHelper
    {
        private const int TotalPagingLinks = 5;
        private const int ActivePagePosition = 2;

        private const string PagerDivClassName = "bcms-pager";
        private const string ActivePageClassName = "bcms-active-pager";
        private const string PreviousPageClassName = "bcms-pager-prev";
        private const string NextPageClassName = "bcms-pager-next";
        private const string PagerTextClassName = "bcms-pager-text";
        private const string PageNumberClassName = "bcms-pager-no";

        public static string RenderPager<TEntity>(SearchableGridViewModel<TEntity> model)
            where TEntity : IEditableGridItem
        {
            var pagination = model.Items;
            var builder = new StringBuilder();

            builder.AppendFormat("<div class='{0} bcms-clearfix'>", PagerDivClassName);

            // total pages
            var totalPages = pagination.TotalPages > 0 ? pagination.TotalPages : 1;
            var pageNumber = pagination.PageNumber;

            // lower bound
            var pagingLowerBound = pageNumber - ActivePagePosition;
            if (pagingLowerBound < 1)
            {
                pagingLowerBound = 1;
            }

            // upper bound
            var pagingUpperBound = pagingLowerBound + TotalPagingLinks;
            if (pagingUpperBound > totalPages)
            {
                pagingUpperBound = totalPages;
            }

            // lower bound correction
            if (pagingUpperBound - pagingLowerBound < TotalPagingLinks)
            {
                pagingLowerBound = pagingUpperBound - TotalPagingLinks;
                if (pagingLowerBound < 1)
                {
                    pagingLowerBound = 1;
                }
            }

            builder.AppendFormat("<span class=\"{0}\">{1}: </span> ", PagerTextClassName, RootGlobalization.Paging_Page_Title);

            // Create first link according to lower bound
            if (pagingLowerBound > 1)
            {
                builder.Append(CreatePageLink(1, "1 "));
                builder.AppendFormat(pagingLowerBound != 2 ? "<span class=\"{0}\">...</span>" : " ", PagerTextClassName);
            }

            // Pages numbers:
            for (var i = pagingLowerBound; i <= pagingUpperBound; i++)
            {
                if (i == pageNumber)
                {
                    // Current page:
                    builder.Append(CreatePageLink(i, i.ToString(CultureInfo.InvariantCulture) + " ", string.Format("{0} {1}", PageNumberClassName, ActivePageClassName)));
                }
                else
                {
                    builder.Append(CreatePageLink(i, i.ToString(CultureInfo.InvariantCulture) + " "));
                    if (i == pagingUpperBound && i <= totalPages - 1)
                    {
                        builder.AppendFormat("<span class=\"{0}\">...</span>", PagerTextClassName);
                    }
                }
            }

            // last page according to upper bound
            if (pagingUpperBound < totalPages)
            {
                builder.Append(CreatePageLink(totalPages, string.Format("{0} ", totalPages)));
            }

            // previuos button
            builder.Append(pageNumber == 1
               ? string.Format("<a class=\"{0}\">{1}</a> ", PreviousPageClassName, RootGlobalization.Button_Paging_Previous)
               : string.Format(@"<a data-page-number=""{0}"" class=""{1}"">{2}</a> ", pageNumber - 1, PreviousPageClassName, RootGlobalization.Button_Paging_Previous));

            // next button
            builder.Append(pageNumber == totalPages || pagination.FirstItem > pagination.TotalItems
               ? string.Format("<a class=\"{0}\">{1}</a> ", NextPageClassName, RootGlobalization.Button_Paging_Next)
               : string.Format(@"<a data-page-number=""{0}"" class=""{1}"">{2}</a> ", pageNumber + 1, NextPageClassName, RootGlobalization.Button_Paging_Next));

            builder.Append("</div>");

            return builder.ToString();
        }

        /// <summary>
        /// Creates the page link.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="text">The text.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns>Generated page link HTML</returns>
        private static string CreatePageLink(int pageNumber, string text, string className = null)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                className = PageNumberClassName;
            }

            return string.Format(@"<a data-page-number=""{0}"" class=""{1}"">{2}</a>", pageNumber, className, text);
        }
    }
}