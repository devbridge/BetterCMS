using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate;
using NHibernate.Criterion;

using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.ModelTests.MapTests
{
    [TestFixture]
    public class MediaMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Media_Successfully()
        {
            var entity = TestDataProvider.CreateNewMedia();
            RunEntityMapTestsInTransaction(entity);
        }
        
        [Test]
        public void Should_Return_Media_Dependencies_Filtering_By_Tags_Media_ChildFolder()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var folderId = new Guid("90732339-B7D0-402C-ACD7-A20D0093CB08");

                var query = repository
                    .AsQueryable<Media>()
                    .Where(media => media.Folder.Id == folderId && media.Original == null)
                    .Where(
                        (m => 
                            // Select medias containing tags
                            (m.MediaTags.Any(mt => mt.Tag.Name == "panda1")
                                && m.MediaTags.Any(mt => mt.Tag.Name == "panda2") 
                                && m.MediaTags.Any(mt => mt.Tag.Name == "panda3")) 
                            
                            ||

                            // Select folders containing tags
                            (m is MediaFolder 
                                && ((MediaFolder) m).ChildFolders.Any(
                                    cf => cf.Child.Medias.Any(
                                        cm => cm.Folder.Id == cf.Child.Id
                                            && cm.MediaTags.Any(
                                                mt1 => mt1.Tag.Name == "panda1"
                                            )
                                            && cm.MediaTags.Any(
                                                mt1 => mt1.Tag.Name == "panda2"
                                            )
                                            && cm.MediaTags.Any(
                                                mt1 => mt1.Tag.Name == "panda3"
                                            )
                                    )
                                )
                            )
                        )
                    );

                var results = query.ToList();

                var tttt = results;
                Assert.AreEqual(tttt.Count, 1);
                Assert.AreEqual(tttt[0].Title, "3.3");
            });
        }

        [Test]
        public void Should_Return_Media_Dependencies_Filtering_By_Tags_Media_RootFolder()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var query = repository
                    .AsQueryable<Media>()
                    .Where(media => media.Folder == null && media.Original == null)
                    .Where(
                        (m =>
                            // Select medias containing tags
                            (m.MediaTags.Any(mt => mt.Tag.Name == "panda1")
                                && m.MediaTags.Any(mt => mt.Tag.Name == "panda2")
                                && m.MediaTags.Any(mt => mt.Tag.Name == "panda3"))

                            ||

                            // Select folders containing tags
                            (m is MediaFolder
                                && ((MediaFolder)m).ChildFolders.Any(
                                    cf => cf.Child.Medias.Any(
                                        cm => cm.Folder.Id == cf.Child.Id
                                            && cm.MediaTags.Any(
                                                mt1 => mt1.Tag.Name == "panda1"
                                            )
                                            && cm.MediaTags.Any(
                                                mt1 => mt1.Tag.Name == "panda2"
                                            )
                                            && cm.MediaTags.Any(
                                                mt1 => mt1.Tag.Name == "panda3"
                                            )
                                    )
                                )
                            )
                        )
                    );

                var results = query.ToList();

                var tttt = results;

                Assert.AreEqual(tttt.Count, 2);
                Assert.AreEqual(tttt.Any(t => t.Title == "3"), true);
                Assert.AreEqual(tttt.Any(t => t.Title == "root panda 1 2 3"), true);
            });
        }

        [Test]
        public void Should_Return_Media_Dependencies_Filtering_By_Tags_Media_WithSearchQuery()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var folderId = new Guid("091df677-515a-407b-a001-a20d0093d2b1");

                var query = repository
                    .AsQueryable<Media>()
                    .Where(media => media.Original == null)
                    .Where(
                        (m =>
                            // Select medias containing tags
                            ((m.Title.Contains("panda") // <-- Move to header
                                && m.MediaTags.Any(mt => mt.Tag.Name == "panda1")
                                && m.MediaTags.Any(mt => mt.Tag.Name == "panda2"))

                            ||

                            // Select folders containing tags
                            (m is MediaFolder
                                && m.Title.Contains("2") // <-- Move to header
                                && ((MediaFolder)m).ChildFolders.Any(
                                    cf => //cf.Parent.Id == folderId &&
                                        cf.Child.Medias.Any(
                                        cm => 
                                            cm.Folder.Id == cf.Child.Id
                                            && cm.MediaTags.Any(
                                                mt1 => mt1.Tag.Name == "panda1"
                                            )
                                            && cm.MediaTags.Any(
                                                mt1 => mt1.Tag.Name == "panda2"
                                            )
                                    )
                                )
                                // Filter out only current folder children
                                && ((MediaFolder)m).ParentFolders.Any(
                                    pf => pf.Parent.Id == folderId
                                )
                            ))
                        )
                    );

                var results = query.ToList();

                var tttt = results;
                Assert.AreEqual(tttt.Count, 1);
                Assert.AreEqual(tttt[0].Title, "3.3");
            });
        }

        

        private IList<Media> GetResult(DefaultRepository repository, string searchQuery, string[] tags, Guid? folderId = null)
        {
            var query = repository
                .AsQueryable<Media>()
                .Where(media => media.Original == null);

            // Filter by search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(m => m.Title.Contains(searchQuery) 
                        // TODO: uncomment after test: ||
                        // TODO: uncomment after test: m.Description.Contains(searchQuery) ||
                        // TODO: uncomment after test: m.MediaTags.Any(mt => mt.Tag.Name.Contains(searchQuery))
                        );
            }

            // Filter out only current folder children
            if (folderId != null)
            {
                query = query.Where(
                    m => m.Folder.ParentFolders.Any(
                        pf => pf.Parent.Id == folderId));
            }

            var subFilterMedias = PredicateBuilder.True<Media>();
            if (tags != null && tags.Length > 0)
            {
                foreach (var tag in tags)
                {
                    subFilterMedias = subFilterMedias.And(m => m.MediaTags.Any(mt => mt.Tag.Name == tag));
                }
            }
            
            var subFilterFolders = PredicateBuilder.True<Media>();
            if (tags != null && tags.Length > 0)
            {
                var tagsSubFilter = PredicateBuilder.True<Media>();
                foreach (var tag in tags)
                {
                    tagsSubFilter = tagsSubFilter.And(m => m.MediaTags.Any(mt => mt.Tag.Name == tag));
                }

                subFilterFolders = subFilterFolders.And(m => m is MediaFolder
                        && ((MediaFolder)m).ChildFolders.Any(
                            cf => cf.Child.Medias.Any(
                                cm => cm.Folder.Id == cf.Child.Id
                                    && tagsSubFilter.Compile().Invoke(m)
                            )
                        ));

            }

            // Combine filtering of items OR filtering of folders
            var filter = PredicateBuilder.True<Media>();
            filter.Or(subFilterFolders);
            filter.Or(subFilterMedias);
            query = query.Where(filter);

            var results = query.ToList();

            return results;
        }

        [Test]
        public void Invoke_Test()
        {
            RunActionInTransaction(
                session =>
                    {
                        var unitOfWork = new DefaultUnitOfWork(session);
                        var repository = new DefaultRepository(unitOfWork);

                        var filter = PredicateBuilder.True<Media>();
                        filter = filter.And(m => m.Title.Contains("."));

                        var results = repository
                            .AsQueryable<Media>()
                            .Where(m => m.Title.Contains("3") && filter.Compile().Invoke(m))
                            .ToList();

                        var tttt = 1;
                    });
        }

        [Test]
        public void Should_Return_Media_Dependencies_Filtering_By_Tags_Media_WithSearchQuery_FolderNull()
        {
            RunActionInTransaction(
                session =>
                {
                    var unitOfWork = new DefaultUnitOfWork(session);
                    var repository = new DefaultRepository(unitOfWork);

                    //var results1 = GetResult(repository, "panda", null);
                    //var results2 = GetResult(repository, "panda", new string[] { "panda1", "panda2" });
                    var results3 = GetResultCriteria(session, "panda", new string[] { "panda1", "panda2" }, new Guid("091df677-515a-407b-a001-a20d0093d2b1"));
                    //var results4 = GetResult(repository, "panda", new string[] { "panda1", "panda2" }, new Guid("90732339-B7D0-402C-ACD7-A20D0093CB08"));
                    //var results5 = GetResult(repository, null, new string[] { "panda1", "panda2" }, new Guid("091df677-515a-407b-a001-a20d0093d2b1"));
                });
        }

        public IList<Media> GetResultCriteria(ISession session, string searchQuery, string[] tags, Guid? folderId = null)
        {
            Media alias = null;
            var filter = session.CreateCriteria<Media>();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                filter.Add(Restrictions.Like(Projections.Property(() => alias.Title), searchQuery, MatchMode.Anywhere));
            }

            var results = filter.List<Media>();

            return results;

            //            if (!string.IsNullOrWhiteSpace(searchQuery))
            //            {
            //                query = query.Where(m => m.Title.Contains(searchQuery)
            //                    // TODO: uncomment after test: ||
            //                    // TODO: uncomment after test: m.Description.Contains(searchQuery) ||
            //                    // TODO: uncomment after test: m.MediaTags.Any(mt => mt.Tag.Name.Contains(searchQuery))
            //                        );
            //            }
            //
            //            // Filter out only current folder children
            //            if (folderId != null)
            //            {
            //                query = query.Where(
            //                    m => m.Folder.ParentFolders.Any(
            //                        pf => pf.Parent.Id == folderId));
            //            }
            //
            //            var subFilterMedias = PredicateBuilder.True<Media>();
            //            if (tags != null && tags.Length > 0)
            //            {
            //                foreach (var tag in tags)
            //                {
            //                    subFilterMedias = subFilterMedias.And(m => m.MediaTags.Any(mt => mt.Tag.Name == tag));
            //                }
            //            }
            //
            //            var subFilterFolders = PredicateBuilder.True<Media>();
            //            if (tags != null && tags.Length > 0)
            //            {
            //                var tagsSubFilter = PredicateBuilder.True<Media>();
            //                foreach (var tag in tags)
            //                {
            //                    tagsSubFilter = tagsSubFilter.And(m => m.MediaTags.Any(mt => mt.Tag.Name == tag));
            //                }
            //
            //                subFilterFolders = subFilterFolders.And(m => m is MediaFolder
            //                        && ((MediaFolder)m).ChildFolders.Any(
            //                            cf => cf.Child.Medias.Any(
            //                                cm => cm.Folder.Id == cf.Child.Id
            //                                    && tagsSubFilter.Compile().Invoke(m)
            //                            )
            //                        ));
            //
            //            }
            //
            //            // Combine filtering of items OR filtering of folders
            //            var filter = PredicateBuilder.True<Media>();
            //            filter.Or(subFilterFolders);
            //            filter.Or(subFilterMedias);
            //            query = query.Where(filter);
        }
    }
}