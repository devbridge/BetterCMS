using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    /// <summary>
    /// Widgets API service implementation.
    /// </summary>
    public class DefaultWidgetApiService : ApiServiceBase, IWidgetApiService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWidgetApiService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultWidgetApiService(IRepository repository)
        {
            this.repository = repository;
        }

        
    }
}