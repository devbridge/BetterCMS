using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Registration;
using BetterCms.Module.Root.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Services
{
    public class DefaultSettingsService : ISettingsService
    {
        private IModulesRegistration modulesRegistrationService;

        private IRepository repository;

        private IUnitOfWork unitOfWork;

        public DefaultSettingsService(IModulesRegistration modulesRegistrationService, IRepository repository, IUnitOfWork unitOfWork)
        {
            this.modulesRegistrationService = modulesRegistrationService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public List<ModuleDescriptor> GetActiveModules()
        {
            return modulesRegistrationService.GetModules().Select(m => m.ModuleDescriptor).ToList();
        }

        public List<Setting> GetActiveModulesSettings()
        {
            var activeModules = GetActiveModules().Select(m => m.Id);
            return repository.AsQueryable<Setting>().Where(x => activeModules.Contains(x.ModuleId)).ToFuture().ToList();
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
