using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Blog.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Blog.DataServices
{
    public class DefaultBlogApiService : ApiServiceBase, IBlogApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBlogApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultBlogApiService(IRepository repository)
        {
            this.repository = repository;
        }

        
    }
}