using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Services;
using BetterCms.Module.Viddler.ViewModels.UploadVideos;

namespace BetterCms.Module.Viddler.Command.ViddlerData
{
    internal class GetViddlerDataForUploadCommand : CommandBase, ICommandOut<ViddlerUploadViewModel>
    {
        private readonly IViddlerService viddlerService;

        public GetViddlerDataForUploadCommand(IViddlerService viddlerService)
        {
            this.viddlerService = viddlerService;
        }

        public ViddlerUploadViewModel Execute()
        {
            var sessionId = viddlerService.GetSessionId();
            var uploadData = viddlerService.GetUploadData(sessionId);

            return new ViddlerUploadViewModel
                {
                    SessionId = uploadData.SessionId,
                    Token = uploadData.Token,
                    Endpoint = uploadData.Endpoint,
                };
        }
    }
}