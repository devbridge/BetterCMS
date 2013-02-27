using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Navigation.Models;

namespace BetterCms.Module.Navigation.DataServices
{
    public interface ISitemapApiService
    {
        IList<SitemapNode> GetSitemapTree();

        SitemapNode GetNode(Guid id);

        IList<SitemapNode> GetNodes(Expression<Func<SitemapNode, bool>> filter = null, Expression<Func<SitemapNode, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null);
    }
}