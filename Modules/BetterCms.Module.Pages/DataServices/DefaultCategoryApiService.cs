using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultCategoryApiService : ApiServiceBase, ICategoryApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCategoryApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultCategoryApiService(IRepository repository)
        {
            this.repository = repository;
        }

        
    }
}