using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultRedirectApiService : ApiServiceBase, IRedirectApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRedirectApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultRedirectApiService(IRepository repository)
        {
            this.repository = repository;
        }

        
    }
}