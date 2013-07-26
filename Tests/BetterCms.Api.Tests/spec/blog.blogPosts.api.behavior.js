/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('blog.blogPosts.api.behavior', function () {
    'use strict';

    var constants = {
        testPageId: '2f2594ce6f8c484d8833a20500bbe74d',
        testPageTitle: '_0003_Blog_For_Tests_',
        testPageUrl: '/articles/0003-blog-for-tests/'
    };

    it('02100: Should get a list blog posts, excluding archived and unpublished.', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false,
            data = createDataForBlogPostsList(false, false, 1, 0);

        runs(function () {
            api.get(url, data, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);
        });
    });
    
    it('02101: Should get a list blog posts, excluding archived, including unpublished.', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false,
            data = createDataForBlogPostsList(false, true, 1, 0);

        runs(function () {
            api.get(url, data, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.totalCount).toBe(2);
            expect(result.data.items.length).toBe(1);
        });
    });
    
    it('02102: Should get a list blog posts, excluding unpublished, including archived.', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false,
            data = createDataForBlogPostsList(true, false, 1, 0);

        runs(function () {
            api.get(url, data, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.totalCount).toBe(2);
            expect(result.data.items.length).toBe(1);
        });
    });

    it('02103: Should get a list of all blog posts, including archived and unpublished.', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false,
            data = createDataForBlogPostsList(true, true, 2, 2);

        runs(function () {
            api.get(url, data, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.totalCount).toBe(4);
            expect(result.data.items.length).toBe(2);

            var blogPost = result.data.items[1];
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle);
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl);
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text');
            expect(blogPost.isPublished).toBe(true);
            expect(blogPost.publishedOn).not.toBeNull();
            expect(blogPost.layoutId).not.toBeNull();
            expect(blogPost.categoryId).not.toBeNull();
            expect(blogPost.categoryName).toBe('Category for _0003_Blog_For_Tests_');
            expect(blogPost.authorId).not.toBeNull();
            expect(blogPost.authorName).toBe('Author for _0003_Blog_For_Tests_');
            expect(blogPost.mainImageId).not.toBeNull();
            expect(blogPost.mainImageUrl).not.toBeNull();
            expect(blogPost.mainImageThumbnauilUrl).not.toBeNull();
            expect(blogPost.mainImageCaption).toBe('Image caption for _0003_Blog_For_Tests_');
            expect(blogPost.activationDate).toBe('/Date(1374613200000-0000)/');
            expect(blogPost.expirationDate).toBe('/Date(1974315599000-0000)/');
            expect(blogPost.isArchived).toBe(false);
        });
    });

    it('02104: Should get a blog post by id.', function () {
        var url = '/bcms-api/blog-posts/' + constants.testPageId,
            result,
            ready = false;

        runs(function () {
            api.get(url, null, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();

            var blogPost = result.data;
            expect(blogPost).not.toBeNull();
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle);
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl);
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text');
            expect(blogPost.isPublished).toBe(true);
            expect(blogPost.publishedOn).not.toBeNull();
            expect(blogPost.layoutId).not.toBeNull();
            expect(blogPost.categoryId).not.toBeNull();
            expect(blogPost.categoryName).toBe('Category for _0003_Blog_For_Tests_');
            expect(blogPost.authorId).not.toBeNull();
            expect(blogPost.authorName).toBe('Author for _0003_Blog_For_Tests_');
            expect(blogPost.mainImageId).not.toBeNull();
            expect(blogPost.mainImageUrl).not.toBeNull();
            expect(blogPost.mainImageThumbnauilUrl).not.toBeNull();
            expect(blogPost.mainImageCaption).toBe('Image caption for _0003_Blog_For_Tests_');
            expect(blogPost.activationDate).toBe('/Date(1374613200000-0000)/');
            expect(blogPost.expirationDate).toBe('/Date(1974315599000-0000)/');
            expect(blogPost.isArchived).toBe(false);
        });
    });
    
    it('02105: Should get blog post properties by id.', function () {
        var url = '/bcms-api/blog-post-properties/' + constants.testPageId,
             result,
             ready = false;

        var data = {
            includeTags: true,
            includeLayout: true,
            includeCategory: true,
            includeAuthor: true,
            includeImages: true,
            includeMetaData: true,
            includeHtmlContent: true
        };

        runs(function () {
            api.get(url, data, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();

            // Page
            var blogPost = result.data;
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle);
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl);
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text');
            expect(blogPost.isPublished).toBe(true);
            expect(blogPost.publishedOn).not.toBeNull();
            expect(blogPost.layoutId).not.toBeNull();
            expect(blogPost.categoryId).not.toBeNull();
            expect(blogPost.authorId).not.toBeNull();
            expect(blogPost.mainImageId).not.toBeNull();
            expect(blogPost.secondaryImageId).not.toBeNull();
            expect(blogPost.featuredImageId).not.toBeNull();
            expect(blogPost.activationDate).toBe('/Date(1374613200000-0000)/');
            expect(blogPost.expirationDate).toBe('/Date(1974315599000-0000)/');
            expect(blogPost.isArchived).toBe(false);

            // html
            expect(result.htmlContent).toBe('<p>_0003_Blog_For_Tests_ HTML</p>');

            // layout
            var layout = result.layout;
            expect(layout).not.toBeNull();
            api.expectBasePropertiesAreNotNull(layout);
            expect(layout.name).toBe('_0003_Layout for _0003_Blog_For_Tests_');
            expect(layout.layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml');
            expect(layout.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png');

            // category
            var category = result.category;
            expect(category).not.toBeNull();
            api.expectBasePropertiesAreNotNull(category);
            expect(category.name).toBe('Category for _0003_Blog_For_Tests_');
            
            // author
            var author = result.author;
            expect(author).not.toBeNull();
            api.expectBasePropertiesAreNotNull(author);
            expect(author.name).toBe('Author for _0003_Blog_For_Tests_');

            // tags
            var tags = result.tags;
            expect(tags).not.toBeNull();
            expect(tags.length).toBe(2);
            expect(tags[0].name).toBe('tag1_0003_Blog_For_Tests_');

            api.expectBasePropertiesAreNotNull(tags[1]);
            expect(tags[1].name).toBe('tag2_0003_Blog_For_Tests_');

            // images
            expectImagePropertiesAreNotNull(result.mainImage);
            expectImagePropertiesAreNotNull(result.featuredImage);
            expectImagePropertiesAreNotNull(result.secondaryImage);

            // meta data
            var metadata = result.metaData;
            expect(metadata).not.toBeNull();
            expect(metadata.metaTitle).toBe('Test meta title');
            expect(metadata.metaKeywords).toBe('Test meta keywords');
            expect(metadata.metaDescription).toBe('Test meta description');
        });
    });
    
    it('02106: Should get blog post list, filtered by tags, using AND connector', function () {
        filterByTags('and', 1, ['IFilterByTags Page 1']);
    });

    it('02107: Should get blog post list, filtered by tags, using OR connector', function () {
        filterByTags('or', 2, ['IFilterByTags Page 1', 'IFilterByTags Page 3']);
    });
    
    it('02108: Should get a list with one blog post, filtered by all available columns', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: 'c1efcb1107ed4901abb3a206012b0b87' },
                    { field: 'CreatedOn', value: '2013-07-25 18:08:47.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-25 18:13:56.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '2' },

                    { field: 'Title', value: '02108' },
                    { field: 'BlogPostUrl', value: '/articles/02108/' },
                    { field: 'IntroText', value: '02108 description' },
                    { field: 'IsPublished', value: true },
                    { field: 'PublishedOn', value: '2013-07-25 18:13:56.000' },
                    { field: 'LayoutId', value: '0e991684003a43deb40f0ffeccddc6eb' },
                    { field: 'CategoryId', value: '4e2095dbbfa4405e808aa206012c2486' },
                    { field: 'CategoryName', value: '02108' },
                    { field: 'AuthorId', value: '58ee1671c9714f05b45ca206012c210b' },
                    { field: 'AuthorName', value: '02108' },
                    { field: 'MainImageId', value: 'ca4c9fb204554aecadbea206012c58a0' },
                    { field: 'MainImageUrl', value: 'http://bettercms.sandbox.mvc4.local/uploads/image/b2af6ffd204941a3ae8b59855911293a/1_1.jpg' },
                    { field: 'MainImageThumbnauilUrl', value: 'http://bettercms.sandbox.mvc4.local/uploads/image/b2af6ffd204941a3ae8b59855911293a/t_1_1.png' },
                    { field: 'MainImageCaption', value: '02108 caption' },
                    { field: 'ActivationDate', value: '2013-07-25 00:00:00.000' },
                    { field: 'ExpirationDate', value: '2032-07-25 23:59:59.000' },
                    { field: 'IsArchived', value: false }
                ]
            }
        };

        runs(function () {
            api.get(url, data, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);

            expect(result.data.items[0].id).toBe('c1efcb1107ed4901abb3a206012b0b87');

            // Check if model properties count didn't changed. If so - update filter current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]));
        });
    });

    function createDataForBlogPostsList(includeArchived, includeUnpublished, take, skip) {
        return {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0003_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            take: take,
            skip: skip,
            includeUnpublished: includeUnpublished,
            includeArchived: includeArchived
        };
    }
    
    function expectImagePropertiesAreNotNull(image) {
        expect(image).not.toBeNull();
        api.expectBasePropertiesAreNotNull(image);

        expect(image.title).toBe('Image for _0003_Blog_For_Tests_');
        expect(image.caption).toBe('Image caption for _0003_Blog_For_Tests_');
        expect(image.url).not.toBeNull();
        expect(image.thumbnailUrl).not.toBeNull();
    }
    
    function filterByTags(connector, expectedCount, expectedTitles) {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: 'IFilterByTags' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            filterByTagsConnector: connector,
            filterByTags: ['IFilterByTags Tag 1', 'IFilterByTags Tag 2'],
            includeUnpublished: true,
            includeArchived: true
        };

        runs(function () {
            api.get(url, data, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.totalCount).toBe(expectedCount);
            expect(result.data.items.length).toBe(expectedCount);

            for (var i = 0; i < result.data.items.length; i++) {
                expect(result.data.items[i].title).toBe(expectedTitles[i]);
            }
        });
    }
});