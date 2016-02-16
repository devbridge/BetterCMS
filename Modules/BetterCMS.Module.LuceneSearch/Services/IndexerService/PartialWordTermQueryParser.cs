// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartialWordTermQueryParser.cs" company="Devbridge Group LLC">
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
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public class PartialWordTermQueryParser : QueryParser
    {
        public PartialWordTermQueryParser(Version matchVersion, string f, Analyzer a)
            : base(matchVersion, f, a)
        {
            AllowLeadingWildcard = true;
        }

        protected override Query NewTermQuery(Term term)
        {
            var query = CreateQuery(term);
            if (query != null)
            {
                return query;
            }

            return base.NewTermQuery(term);
        }

        private Query CreateQuery(Term term)
        {
            var text = FixQueryWord(term.Text);
            if (text != term.Text)
            {
                return new WildcardQuery(new Term(term.Field, text));
            }

            return null;
        }

        private string FixQueryWord(string queryText)
        {
            if (!queryText.StartsWith("*", System.StringComparison.Ordinal))
            {
                queryText = string.Concat("*", queryText);
            }
            if (!queryText.EndsWith("*", System.StringComparison.Ordinal))
            {
                queryText = string.Concat(queryText, "*");
            }

            return queryText;
        }
    }
}
