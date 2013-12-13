using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.UnassignMainCulturePage
{
    public class UnassignMainCulturePageCommand : CommandBase, ICommand<System.Guid, bool>
    {
        public bool Execute(System.Guid request)
        {
            var page = Repository.AsQueryable<Root.Models.Page>(p => p.Id == request).FirstOne();
            page.MainCulturePage = null;

            Repository.Save(page);
            UnitOfWork.Commit();

            return true;
        }
    }
}