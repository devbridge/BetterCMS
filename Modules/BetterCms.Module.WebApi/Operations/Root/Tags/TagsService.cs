using System.Linq;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    public class TagsService : Service
    {
        private readonly IRepository repository;

        public GetTagsResponse Get(GetTagsRequest request)
        {            
            var request2 = new BetterCms.Module.Pages.Api.DataContracts.GetTagsRequest();
            request.ApplyTo(request2);

            using (var api = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                var tags = api.GetTags(request2);

                return new GetTagsResponse
                           {
                               Status = "ok",
                               Data = new DataListResponse<TagModel>(
                                   tags.Items.Select(
                                       f => new TagModel
                                                {
                                                    Id = f.Id,
                                                    Name = f.Name,
                                                    IsDeleted = f.IsDeleted,
                                                    Version = f.Version
                                                }).ToList(),
                                   tags.TotalCount)
                           };
            }            
        }
    }
}