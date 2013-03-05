using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Pages.DataContracts.Enums;
using BetterCms.Module.Pages.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultPageApiService : ApiServiceBase, IPageApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPageApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultPageApiService(IRepository repository)
        {
            this.repository = repository;
        }

       
    }
}