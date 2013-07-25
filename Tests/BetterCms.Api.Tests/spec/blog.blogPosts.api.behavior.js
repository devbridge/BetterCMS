/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Blog: Blog Posts', function () {
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.totalCount).toBe(4);
            expect(result.data.items.length).toBe(2);

            var blogPost = result.data.items[1];
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle);
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl);
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text');
            expect(blogPost.isPublished).toBe(true);
            expect(blogPost.publishedOn).toBeDefined();
            expect(blogPost.layoutId).toBeDefined();
            expect(blogPost.categoryId).toBeDefined();
            expect(blogPost.categoryName).toBe('Category for _0003_Blog_For_Tests_');
            expect(blogPost.authorId).toBeDefined();
            expect(blogPost.authorName).toBe('Author for _0003_Blog_For_Tests_');
            expect(blogPost.mainImageId).toBeDefined();
            expect(blogPost.mainImageUrl).toBeDefined();
            expect(blogPost.mainImageThumbnauilUrl).toBeDefined();
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
            expect(result).toBeDefined();

            var blogPost = result.data;
            expect(blogPost).toBeDefined();
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle);
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl);
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text');
            expect(blogPost.isPublished).toBe(true);
            expect(blogPost.publishedOn).toBeDefined();
            expect(blogPost.layoutId).toBeDefined();
            expect(blogPost.categoryId).toBeDefined();
            expect(blogPost.categoryName).toBe('Category for _0003_Blog_For_Tests_');
            expect(blogPost.authorId).toBeDefined();
            expect(blogPost.authorName).toBe('Author for _0003_Blog_For_Tests_');
            expect(blogPost.mainImageId).toBeDefined();
            expect(blogPost.mainImageUrl).toBeDefined();
            expect(blogPost.mainImageThumbnauilUrl).toBeDefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();

            // Page
            var blogPost = result.data;
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle);
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl);
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text');
            expect(blogPost.isPublished).toBe(true);
            expect(blogPost.publishedOn).toBeDefined();
            expect(blogPost.layoutId).toBeDefined();
            expect(blogPost.categoryId).toBeDefined();
            expect(blogPost.authorId).toBeDefined();
            expect(blogPost.mainImageId).toBeDefined();
            expect(blogPost.secondaryImageId).toBeDefined();
            expect(blogPost.featuredImageId).toBeDefined();
            expect(blogPost.activationDate).toBe('/Date(1374613200000-0000)/');
            expect(blogPost.expirationDate).toBe('/Date(1974315599000-0000)/');
            expect(blogPost.isArchived).toBe(false);

            // html
            expect(result.htmlContent).toBe('<p>_0003_Blog_For_Tests_ HTML</p>');

            // layout
            var layout = result.layout;
            expect(layout).toBeDefined();
            api.expectBasePropertiesAreNotNull(layout);
            expect(layout.name).toBe('_0003_Layout for _0003_Blog_For_Tests_');
            expect(layout.layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml');
            expect(layout.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png');

            // category
            var category = result.category;
            expect(category).toBeDefined();
            api.expectBasePropertiesAreNotNull(category);
            expect(category.name).toBe('Category for _0003_Blog_For_Tests_');
            
            // author
            var author = result.author;
            expect(author).toBeDefined();
            api.expectBasePropertiesAreNotNull(author);
            expect(author.name).toBe('Author for _0003_Blog_For_Tests_');

            // tags
            var tags = result.tags;
            expect(tags).toBeDefined();
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
            expect(metadata).toBeDefined();
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
        expect(image).toBeDefined();
        api.expectBasePropertiesAreNotNull(image);

        expect(image.title).toBe('Image for _0003_Blog_For_Tests_');
        expect(image.caption).toBe('Image caption for _0003_Blog_For_Tests_');
        expect(image.url).toBeDefined();
        expect(image.thumbnailUrl).toBeDefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.totalCount).toBe(expectedCount);
            expect(result.data.items.length).toBe(expectedCount);

            for (var i = 0; i < result.data.items.length; i++) {
                expect(result.data.items[i].title).toBe(expectedTitles[i]);
            }
        });
    }
});