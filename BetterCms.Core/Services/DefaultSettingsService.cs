using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Models;

using NHibernate.Linq;

namespace BetterCms.Core.Services
{
    public class DefaultSettingsService : ISettingsService
    {
        private IRepository repository;

        private IUnitOfWork unitOfWork;

        public DefaultSettingsService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public List<Setting> GetModuleSettings(Guid moduleId)
        {
            return repository.AsQueryable<Setting>().Where(s => s.ModuleId == moduleId).ToFuture().ToList();
        }

        public void SaveModuleSettings(Guid moduleId, IEnumerable<Setting> settings)
        {
            unitOfWork.BeginTransaction();

            foreach (var setting in settings)
            {
                repository.Save(setting);
            }

            unitOfWork.Commit();
        }
    }
}
