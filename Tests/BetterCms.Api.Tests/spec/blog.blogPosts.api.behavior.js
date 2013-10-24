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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(1, 'Total count should be 1.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(2, 'Total count should be 2.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(2, 'Total count should be 4.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(4, 'Total count should be 4.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');

            var blogPost = result.data.items[1];
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle, 'Correctly filtered title should be retrieved.');
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl, 'Correctly filtered blogPostUrl should be retrieved.');
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text', 'Correctly filtered introText should be retrieved.');
            expect(blogPost.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(blogPost.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(blogPost.layoutId).toBeDefinedAndNotNull('layoutId should be retrieved.');
            expect(blogPost.categoryId).toBeDefinedAndNotNull('categoryId should be retrieved.');
            expect(blogPost.categoryName).toBe('Category for _0003_Blog_For_Tests_', 'Correctly filtered categoryName should be retrieved.');
            expect(blogPost.authorId).toBeDefinedAndNotNull('authorId should be retrieved.');
            expect(blogPost.authorName).toBe('Author for _0003_Blog_For_Tests_', 'Correctly filtered authorName should be retrieved.');
            expect(blogPost.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
            expect(blogPost.mainImageUrl).toBeDefinedAndNotNull('mainImageUrl should be retrieved.');
            expect(blogPost.mainImageThumbnauilUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
            expect(blogPost.mainImageCaption).toBe('Image caption for _0003_Blog_For_Tests_', 'Correctly filtered mainImageCaption should be retrieved.');
            expect(new Date(blogPost.activationDate).getTime()).toBe(new Date('2013-07-24T00:00:00').getTime(), 'Correctly filtered activationDate should be retrieved.');
            expect(new Date(blogPost.expirationDate).getTime()).toBe(new Date('2032-07-24T23:59:59').getTime(), 'Correctly filtered expirationDate should be retrieved.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');

            var blogPost = result.data;
            expect(blogPost).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle, 'Correctly filtered title should be retrieved.');
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl, 'Correctly filtered blogPostUrl should be retrieved.');
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text', 'Correctly filtered introText should be retrieved.');
            expect(blogPost.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(blogPost.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(blogPost.layoutId).toBeDefinedAndNotNull('layoutId should be retrieved.');
            expect(blogPost.categoryId).toBeDefinedAndNotNull('categoryId should be retrieved.');
            expect(blogPost.categoryName).toBe('Category for _0003_Blog_For_Tests_', 'Correctly filtered categoryName should be retrieved.');
            expect(blogPost.authorId).toBeDefinedAndNotNull('authorId should be retrieved.');
            expect(blogPost.authorName).toBe('Author for _0003_Blog_For_Tests_', 'Correctly filtered authorName should be retrieved.');
            expect(blogPost.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
            expect(blogPost.mainImageUrl).toBeDefinedAndNotNull('mainImageUrl should be retrieved.');
            expect(blogPost.mainImageThumbnauilUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
            expect(blogPost.mainImageCaption).toBe('Image caption for _0003_Blog_For_Tests_', 'Correctly filtered mainImageCaption should be retrieved.');
            expect(new Date(blogPost.activationDate).getTime()).toBe(new Date('2013-07-24T00:00:00').getTime(), 'Correctly filtered activationDate should be retrieved.');
            expect(new Date(blogPost.expirationDate).getTime()).toBe(new Date('2032-07-24T23:59:59').getTime(), 'Correctly filtered expirationDate should be retrieved.');            
            expect(blogPost.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');

            // Page
            var blogPost = result.data;
            api.expectBasePropertiesAreNotNull(blogPost);
            expect(blogPost.title).toBe(constants.testPageTitle, 'Correctly filtered title should be retrieved.');
            expect(blogPost.blogPostUrl).toBe(constants.testPageUrl, 'Correctly filtered blogPostUrl should be retrieved.');
            expect(blogPost.introText).toBe('_0003_Blog_For_Tests_ intro text', 'Correctly filtered introText should be retrieved.');
            expect(blogPost.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(blogPost.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(blogPost.layoutId).toBeDefinedAndNotNull('layoutId should be retrieved.');
            expect(blogPost.categoryId).toBeDefinedAndNotNull('categoryId should be retrieved.');
            expect(blogPost.authorId).toBeDefinedAndNotNull('authorId should be retrieved.');
            expect(blogPost.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
            expect(blogPost.secondaryImageId).toBeDefinedAndNotNull('secondaryImageId should be retrieved.');
            expect(blogPost.featuredImageId).toBeDefinedAndNotNull('featuredImageId should be retrieved.');
            expect(new Date(blogPost.activationDate).getTime()).toBe(new Date('2013-07-24T00:00:00').getTime(), 'Correctly filtered activationDate should be retrieved.');
            expect(new Date(blogPost.expirationDate).getTime()).toBe(new Date('2032-07-24T23:59:59').getTime(), 'Correctly filtered expirationDate should be retrieved.');
            
            expect(blogPost.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');

            // html
            expect(result.htmlContent).toBe('<p>_0003_Blog_For_Tests_ HTML</p>', 'Correctly filtered htmlContent should be retrieved.');

            // layout
            var layout = result.layout;
            expect(layout).toBeDefinedAndNotNull('JSON layout object should be retrieved.');
            api.expectBasePropertiesAreNotNull(layout);
            expect(layout.name).toBe('_0003_Layout for _0003_Blog_For_Tests_', 'Correctly filtered layout.name should be retrieved.');
            expect(layout.layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml', 'Correctly filtered layoutPath should be retrieved.');
            expect(layout.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png', 'Correctly filtered layout.previewUrl should be retrieved.');

            // category
            var category = result.category;
            expect(category).toBeDefinedAndNotNull('JSON category object should be retrieved.');
            api.expectBasePropertiesAreNotNull(category);
            expect(category.name).toBe('Category for _0003_Blog_For_Tests_', 'Correctly filtered category.name should be retrieved.');
            
            // author
            var author = result.author;
            expect(author).toBeDefinedAndNotNull('JSON author object should be retrieved.');
            api.expectBasePropertiesAreNotNull(author);
            expect(author.name).toBe('Author for _0003_Blog_For_Tests_', 'Correctly filtered author.name should be retrieved.');

            // tags
            var tags = result.tags;
            expect(tags).toBeDefinedAndNotNull('JSON tags object should be retrieved.');
            expect(tags.length).toBe(2, 'Returned array length should be 2.');
            expect(tags[0].name).toBe('tag1_0003_Blog_For_Tests_', 'Correctly filtered tags[0].name should be retrieved.');

            api.expectBasePropertiesAreNotNull(tags[1]);
            expect(tags[1].name).toBe('tag2_0003_Blog_For_Tests_', 'Correctly filtered tags[1].name should be retrieved.');

            // images
            expectImagePropertiesAreNotNull(result.mainImage, 'mainImage');
            expectImagePropertiesAreNotNull(result.featuredImage, 'featuredImage');
            expectImagePropertiesAreNotNull(result.secondaryImage, 'secondaryImage');

            // meta data
            var metadata = result.metaData;
            expect(metadata).toBeDefinedAndNotNull('JSON metadata object should be retrieved.');
            expect(metadata.metaTitle).toBe('Test meta title', 'Correctly filtered metaTitle should be retrieved.');
            expect(metadata.metaKeywords).toBe('Test meta keywords', 'Correctly filtered metaKeywords should be retrieved.');
            expect(metadata.metaDescription).toBe('Test meta description', 'Correctly filtered metaDescription should be retrieved.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(1, 'Total count should be 1.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');

            expect(result.data.items[0].id).toBe('c1efcb1107ed4901abb3a206012b0b87', 'Correctly filtered ____ should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            // data.filter.where.length + 1 <-- Because field Tags cannnot be filtered by
            expect(data.filter.where.length + 1).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });

    it('02109: Should get blog post content by id', function () {
        var url = '/bcms-api/blog-posts/content/06182C92-E61E-4023-B3C9-A2050086598C',
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');

            var content = result.data;
            expect(content).toBeDefinedAndNotNull('JSON data object should be retrieved.');

            api.expectBasePropertiesAreNotNull(content);
            expect(content.name).toBe('_0002_Blog_Post_For_Tests', 'Correctly filtered name should be retrieved.');
            expect(content.html).toBe('<p>_0002_Blog_Post_For_Tests Test HTML</p>', 'Correctly filtered html should be retrieved.');
            expect(content.isPublished).toBe(true, 'Correctly filtered isPubslihed should be retrieved.');
            expect(new Date(content.publishedOn).getTime()).toBe(new Date('2013-07-24T08:17:11').getTime(), 'Correctly filtered publishedOn should be retrieved.');
            expect(content.publishedByUser).toContain('Better CMS test user', 'Correctly filtered publishedByUser should be retrieved.');
        });
    });

    it('02110: Should get list of blog pots with tags', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '02110:' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeUnpublished: true,
            includeArchived: true,
            includeTags: true
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(2, 'Total count should be 3.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 3.');

            expect(result.data.items[0].title).toBe('02110:1', 'Correctly filtered items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('02110:2', 'Correctly filtered items[1].title should be retrieved.');

            expect(result.data.items[0].tags).toBeDefinedAndNotNull('Correctly filtered items[0].tags should be retrieved.');
            expect(result.data.items[0].tags.length).toBe(2, 'items[0].tags should contain 2 items.');
            expect(result.data.items[0].tags[0]).toBe('02110_1', 'Correctly filtered result.data.items[0].tags[0] should be retrieved.');
            expect(result.data.items[0].tags[1]).toBe('02110_2', 'Correctly filtered result.data.items[0].tags[1] should be retrieved.');

            expect(result.data.items[1].tags).toBeDefinedAndNotNull('Correctly filtered items[1].tags should be retrieved.');
            expect(result.data.items[1].tags.length).toBe(3, 'items[1].tags should contain 3 items.');
            expect(result.data.items[1].tags[0]).toBe('02110_1', 'Correctly filtered result.data.items[1].tags[0] should be retrieved.');
            expect(result.data.items[1].tags[1]).toBe('02110_2', 'Correctly filtered result.data.items[1].tags[1] should be retrieved.');
            expect(result.data.items[1].tags[2]).toBe('02110_3', 'Correctly filtered result.data.items[1].tags[2] should be retrieved.');
        });
    });

    it('02111: Should throw validation exception for filtering by Tags, when getting blog posts.', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Tags', value: 'test' }]
                }
            };

        runs(function () {
            api.get(url, data, null, function (response) {
                result = response.responseJSON;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            api.expectValidationExceptionIsThrown(result, 'Data');
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
    
    function expectImagePropertiesAreNotNull(image, name) {
        expect(image).toBeDefinedAndNotNull('JSON ' + name + ' object should be retrieved.');
        api.expectBasePropertiesAreNotNull(image);

        expect(image.title).toBe('Image for _0003_Blog_For_Tests_', 'Correctly filtered ' + name + '.title should be retrieved.');
        expect(image.caption).toBe('Image caption for _0003_Blog_For_Tests_', 'Correctly filtered ' + name + '.caption should be retrieved.');
        expect(image.url).toBeDefinedAndNotNull('url should be retrieved.');
        expect(image.thumbnailUrl).toBeDefinedAndNotNull('thumbnailUrl should be retrieved.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(expectedCount, 'Total count should be ' + expectedCount + '.');
            expect(result.data.items.length).toBe(expectedCount, 'Returned array length should be ' + expectedCount + '.');

            for (var i = 0; i < result.data.items.length; i++) {
                expect(result.data.items[i].title).toBe(expectedTitles[i], 'Correctly filtered title should be retrieved.');
            }
        });
    }
});