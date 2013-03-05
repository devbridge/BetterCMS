using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Root.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultLayoutApiService : ApiServiceBase, ILayoutApiService
    {
        private IRepository repository { get; set; }
        private IUnitOfWork unitOfWork { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLayoutApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultLayoutApiService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

    }
}