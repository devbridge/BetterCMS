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
    public class DefaultAuthorApiService : ApiServiceBase, IAuthorApiService
    {
        private IRepository repository { get; set; }

        public DefaultAuthorApiService(IRepository repository)
        {
            this.repository = repository;
        }

       
    }
}