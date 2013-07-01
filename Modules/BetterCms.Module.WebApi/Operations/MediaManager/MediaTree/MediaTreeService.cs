using System.Collections.Generic;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    public class MediaTreeService : Service, IMediaTreeService
    {
        public GetMediaTreeResponse Get(GetMediaTreeRequest request)
        {
            return new GetMediaTreeResponse
                       {
                           Data = new MediaTreeModel
                                      {
                                          FilesTree = new List<MediaItemModel>
                                                          {
                                                              new MediaItemModel(),
                                                              new MediaItemModel(),
                                                              new MediaItemModel(),
                                                          },
                                          ImagesTree = new List<MediaItemModel>
                                                          {
                                                              new MediaItemModel(),
                                                              new MediaItemModel(),
                                                              new MediaItemModel(),
                                                          },
                                          VideosTree = new List<MediaItemModel>
                                                          {
                                                              new MediaItemModel(),
                                                              new MediaItemModel(),
                                                              new MediaItemModel(),
                                                          },
                                      }
                       };
        }
    }
}