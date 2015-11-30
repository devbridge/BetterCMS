// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchModuleConstants.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Search
{
    public static class SearchModuleConstants
    {
        public static class WidgetOptionNames
        {
            public const string FormMethod = "Form.Method";
            public const string QueryParameterName = "Query.Parameter.Name";
            public const string LabelTitle = "Label.Title";
            public const string SubmitTitle = "Submit.Title";
            public const string InputPlaceholder = "Input.Placeholder";
            public const string SearchResultsUrl = "SearchResults.Url";
            public const string ShowTotalResults = "SearchResults.Show.Total.Count";
            public const string TotalCountMessage = "SearchResults.Total.Count.Message";
            public const string ResultsCount = "SearchResults.Count";
        }

        public static class WidgetOptionDefaultValues
        {
            public const string FormMethod = "GET";
            public const string QueryParameterName = "Query";
        }

        public const int DefaultSearchResultsCount = 10;

        public const int SearchQueryMaximumLength = 1000;
    }
}