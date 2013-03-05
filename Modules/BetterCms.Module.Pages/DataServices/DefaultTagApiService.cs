using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultTagApiService : ApiServiceBase, ITagApiService
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTagApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultTagApiService(IRepository repository)
        {
            this.repository = repository;
        }

        
    }
}