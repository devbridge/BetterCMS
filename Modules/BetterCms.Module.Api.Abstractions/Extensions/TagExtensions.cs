using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

namespace BetterCms.Module.Api.Extensions
{
    public static class TagExtensions
    {
        public static PostTagRequest ToPostRequest(this GetTagResponse response)
        {
            var model = MapModel(response);

            return new PostTagRequest { Data = model };
        }

        public static PutTagRequest ToPutRequest(this GetTagResponse response)
        {
            var model = MapModel(response);

            return new PutTagRequest { Data = model, Id = response.Data.Id };
        }

        private static SaveTagModel MapModel(GetTagResponse response)
        {
            var model = new SaveTagModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                        };

            return model;
        }
    }
}
