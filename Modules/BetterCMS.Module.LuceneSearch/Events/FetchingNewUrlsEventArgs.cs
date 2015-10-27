using BetterCMS.Module.LuceneSearch.Models;
using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class FetchingNewUrlsEventArgs: EventArgs
    {
        public IList<IndexSource> IndexSources { get; set; }
    }
}
