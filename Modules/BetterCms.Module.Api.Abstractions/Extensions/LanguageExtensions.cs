using BetterCms.Module.Api.Operations.Root.Languages.Language;

namespace BetterCms.Module.Api.Extensions
{
    public static class LanguageExtensions
    {
        public static PostLanguageRequest ToPostRequest(this GetLanguageResponse response)
        {
            var model = MapModel(response);

            return new PostLanguageRequest { Data = model };
        }

        public static PutLanguageRequest ToPutRequest(this GetLanguageResponse response)
        {
            var model = MapModel(response);

            return new PutLanguageRequest { Data = model, Id = response.Data.Id };
        }

        private static SaveLanguageModel MapModel(GetLanguageResponse response)
        {
            var model = new SaveLanguageModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            Code = response.Data.Code
                        };

            return model;
        }
    }
}
