using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    public class TagService : Service, ITagService
    {
        private readonly IRepository repository;

        public TagService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetTagResponse Get(GetTagRequest request)
        {
            var query = repository.AsQueryable<Module.Root.Models.Tag>();

            if (request.TagId.HasValue)
            {
                query = query.Where(tag => tag.Id == request.TagId);
            }
            else
            {
                query = query.Where(tag => tag.Name == request.TagName);
            }

            var model = query
                .Select(tag => new TagModel
                    {
                        Id = tag.Id,
                        Version = tag.Version,
                        CreatedBy = tag.CreatedByUser,
                        CreatedOn = tag.CreatedOn,
                        LastModifiedBy = tag.ModifiedByUser,
                        LastModifiedOn = tag.ModifiedOn,

                        Name = tag.Name
                    })
                .FirstOne();

            return new GetTagResponse
                       {
                           Data = model
                       };
        }

        public PutTagResponse Put(PutTagRequest request)
        {
            // TODO: implement PUT
            return new PutTagResponse
            {
                Data = request.Id,
            };
        }

        PutTagResponse ITagService.Update(PutTagRequest request)
        {
            return Put(request);
        }
    }
}