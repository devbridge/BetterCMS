using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.History.RestoreMediaVersion
{
    public class RestoreMediaVersionCommand : CommandBase, ICommand<Guid, bool>
    {
        public bool Execute(Guid mediaHistoryItemId)
        {
            var versionToRevert = Repository
                .AsQueryable<Media>(p => p.Id == mediaHistoryItemId)
                .Fetch(f => f.Original)
                .First();

            var original = versionToRevert.Original;

            if (original != null)
            {
                UnitOfWork.BeginTransaction();
                Repository.Save(original.CreateHistoryItem());
                versionToRevert.CopyDataTo(original);
                original.Original = null;
                original.PublishedOn = DateTime.Now;
                Repository.Save(original);
                UnitOfWork.Commit();
                return true;
            }

            return false;
        }
    }
}