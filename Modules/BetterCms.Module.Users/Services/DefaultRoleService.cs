using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Users.Models;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

namespace BetterCms.Module.Users.Services
{
    public class DefaultRoleService : IRoleService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRoleService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultRoleService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the list of roles lookup values.
        /// </summary>
        /// <returns>
        /// List of roles lookup values.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<LookupKeyValue> GetUserRoles()
        {
            Role alias = null;
            LookupKeyValue lookupAlias = null;

            return unitOfWork.Session
                .QueryOver(() => alias)
                .SelectList(select => select
                    .Select(Projections.Cast(NHibernateUtil.String, Projections.Property<Role>(c => c.Id))).WithAlias(() => lookupAlias.Key)
                    .Select(() => alias.Name).WithAlias(() => lookupAlias.Value)).Where(c => !c.IsDeleted)
                .OrderBy(o => o.Name).Asc()
                .TransformUsing(Transformers.AliasToBean<LookupKeyValue>())
                .List<LookupKeyValue>();
        }

        public Role GetRole(Guid? id)
        {
            return unitOfWork.Session.Query<Role>().FirstOrDefault(r => r.Id == id);
        }
    }
}