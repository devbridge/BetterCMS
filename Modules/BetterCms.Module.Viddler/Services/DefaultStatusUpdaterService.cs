using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Viddler.Models;

using Common.Logging;

using NHibernate;

namespace BetterCms.Module.Viddler.Services
{
    internal class DefaultStatusUpdaterService : IStatusUpdaterService
    {
        private static readonly ILog logger = LogManager.GetCurrentClassLogger();

        private readonly ISessionFactoryProvider sessionFactoryProvider;

        private readonly IViddlerService viddlerService;

        private readonly ICacheService cacheService;

        public DefaultStatusUpdaterService(ISessionFactoryProvider sessionFactoryProvider, IViddlerService viddlerService, ICacheService cacheService)
        {
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.viddlerService = viddlerService;
            this.cacheService = cacheService;
        }

        public void UpdateStatus(IList<Guid> mediasIds)
        {
            try
            {
                if (mediasIds == null || mediasIds.Count < 1)
                {
                    return;
                }

                var tasks = new List<Task>();
                foreach (var mediaId in mediasIds)
                {
                    var id = mediaId;
                    tasks.Add(
                        new Task(
                            () =>
                                {
                                    using (var session = sessionFactoryProvider.OpenSession(false))
                                    {
                                        try
                                        {
                                            UpdateStatus(id, session);
                                        }
                                        finally
                                        {
                                            session.Close();
                                        }
                                    }
                                }));
                }

                tasks.ForEach(task => task.Start());
            }
            catch (Exception ex)
            {
                logger.Error("Video status requesting failed.", ex);
            }
        }

        private void UpdateStatus(Guid mediaId, ISession session)
        {
            try
            {
                var cacheToken = GetCacheToken(mediaId);
                var lastCheck = cacheService.Get<bool?>(cacheToken);
                if (lastCheck.HasValue && lastCheck.Value)
                {
                    return;
                }

                cacheService.Set(cacheToken, true, new TimeSpan(0, 2, 0));

                var video = session.Get<Video>(mediaId);
                if (video.IsDeleted || video.Original != null || video.IsUploaded != null)
                {
                    return;
                }

                var details = viddlerService.GetVideoDetails(viddlerService.GetSessionId(), video.VideoId);
                if (!details.IsReady)
                {
                    return;
                }

                video.IsUploaded = true;
                session.Save(video);
                session.Flush();
                cacheService.Remove(cacheToken);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Video status requesting for media {0} failed.", mediaId), ex);
            }
        }

        private string GetCacheToken(Guid mediaId)
        {
            return string.Format("viddler_video_status_updated_{0}", mediaId);
        }
    }
}