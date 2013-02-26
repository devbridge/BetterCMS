using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataServices;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultTagApiService : ITagApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTagApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultTagApiService(IRepository repository)
        {
            this.repository = repository;
        }

//        public IList<ITag> GetTags()
//        {
//            return GetTagsQueryable().ToList();
//        }
//
//        public IQueryable<ITag> GetTagsQueryable()
//        {
//            return GetTagsQueryable<Tag>();
//        }
//
//        public IQueryable<TEntity> GetTagsQueryable<TEntity>() 
//            where TEntity : Core.Models.Entity, ITag
//        {
//            return repository.AsQueryable<TEntity>();
//        }
    }
}