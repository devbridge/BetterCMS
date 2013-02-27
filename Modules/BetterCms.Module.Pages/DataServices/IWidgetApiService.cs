using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public interface IWidgetApiService
    {
        HtmlContentWidget GetHtmlContentWidget(Guid id);

        ServerControlWidget GetServerControlWidget(Guid id);

        IList<Widget> GetWidgets(Expression<Func<Widget, bool>> filter = null, Expression<Func<Widget, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null);

        IList<Widget> GetPageWidgets(Guid pageId, Expression<Func<Widget, bool>> filter = null);
    }
}
