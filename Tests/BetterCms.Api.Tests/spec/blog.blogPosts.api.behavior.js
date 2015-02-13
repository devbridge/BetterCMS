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
            expect(blogPost.categories.length).toBe(1, 'Categories count should be ' + 1 + '.');
            expect(blogPost.categories[0].name).toBe('Category for _0003_Blog_For_Tests_', 'Correctly filtered categoryName should be retrieved.');
            expect(blogPost.authorId).toBeDefinedAndNotNull('authorId should be retrieved.');
            expect(blogPost.authorName).toBe('Author for _0003_Blog_For_Tests_', 'Correctly filtered authorName should be retrieved.');
            expect(blogPost.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
            expect(blogPost.mainImageUrl).toBeDefinedAndNotNull('mainImageUrl should be retrieved.');
            expect(blogPost.mainImageThumbnauilUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
            expect(blogPost.mainImageThumbnailUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
            expect(blogPost.mainImageCaption).toBe('Image caption for _0003_Blog_For_Tests_', 'Correctly filtered mainImageCaption should be retrieved.');
            expect(blogPost.languageCode).toBe('ar-MA', 'Correctly filtered languageCode should be retrieved.');
            expect(blogPost.languageId).toBe('a3f605164ab549a7afb1a38e00f7ec89', 'Correctly filtered languageId should be retrieved.');
            expect(blogPost.languageGroupIdentifier).toBe('15830a7f9c1044138bc1fb6cab6199a3', 'Correctly filtered languageGroupIdentifier should be retrieved.');
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
            expect(blogPost.categories.length).toBe(1, 'Categories count should be ' + 1 + '.');
            expect(blogPost.categories[0].name).toBe('Category for _0003_Blog_For_Tests_', 'Correctly filtered categoryName should be retrieved.');
            expect(blogPost.authorId).toBeDefinedAndNotNull('authorId should be retrieved.');
            expect(blogPost.authorName).toBe('Author for _0003_Blog_For_Tests_', 'Correctly filtered authorName should be retrieved.');
            expect(blogPost.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
            expect(blogPost.mainImageUrl).toBeDefinedAndNotNull('mainImageUrl should be retrieved.');
            expect(blogPost.mainImageThumbnauilUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
            expect(blogPost.mainImageThumbnailUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
            expect(blogPost.mainImageCaption).toBe('Image caption for _0003_Blog_For_Tests_', 'Correctly filtered mainImageCaption should be retrieved.');
            expect(blogPost.languageId).toBe('a3f605164ab549a7afb1a38e00f7ec89', 'Correctly filtered languageId should be retrieved.');
            expect(blogPost.languageCode).toBe('ar-MA', 'Correctly filtered languageCode should be retrieved.');
            expect(blogPost.languageGroupIdentifier).toBe('15830a7f9c1044138bc1fb6cab6199a3', 'Correctly filtered languageGroupIdentifier should be retrieved.');
            expect(new Date(blogPost.activationDate).getTime()).toBe(new Date('2013-07-24T00:00:00').getTime(), 'Correctly filtered activationDate should be retrieved.');
            expect(new Date(blogPost.expirationDate).getTime()).toBe(new Date('2032-07-24T23:59:59').getTime(), 'Correctly filtered expirationDate should be retrieved.');            
            expect(blogPost.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
        });
    });
    
    it('02105.1: Should get blog post properties by id.', function () {
        var url = '/bcms-api/blog-post-properties/' + constants.testPageId,
             result,
             ready = false;

        var data = {
            includeTags: true,
            includeLayout: true,
            includeAuthor: true,
            includeImages: true,
            includeMetaData: true,
            includeHtmlContent: true,
            includeAccessRules: true,
            includeTechnicalInfo: true,
            includeChildContentsOptions: true,
            includeLanguage: true,
            includeCategories: true
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
            expect(blogPost.categories.length).toBe(1, 'Categories count should be ' + 1 + '.');
            expect(blogPost.authorId).toBeDefinedAndNotNull('authorId should be retrieved.');
            expect(blogPost.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
            expect(blogPost.secondaryImageId).toBeDefinedAndNotNull('secondaryImageId should be retrieved.');
            expect(blogPost.featuredImageId).toBeDefinedAndNotNull('featuredImageId should be retrieved.');
            expect(new Date(blogPost.activationDate).getTime()).toBe(new Date('2013-07-24T00:00:00').getTime(), 'Correctly filtered activationDate should be retrieved.');
            expect(new Date(blogPost.expirationDate).getTime()).toBe(new Date('2032-07-24T23:59:59').getTime(), 'Correctly filtered expirationDate should be retrieved.');
            
            expect(blogPost.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');

            // html
            var substring = '<p>_0003_Blog_For_Tests_ HTML</p>';
            expect(result.htmlContent.substr(0, substring.length)).toBe(substring, 'Correctly filtered htmlContent should be retrieved.');

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

            // language
            var language = result.language;
            expect(language).toBeDefinedAndNotNull('JSON language object should be retrieved.');
            api.expectBasePropertiesAreNotNull(language);
            expect(language.name).toBe('Language for _0003_Blog_For_Tests_', 'Correctly filtered language.name should be retrieved.');
            expect(language.code).toBe('ar-MA', 'Correctly filtered language.code should be retrieved.');
            expect(language.languageGroupIdentifier).toBe('15830a7f9c1044138bc1fb6cab6199a3', 'Correctly filtered language.languageGroupIdentifier should be retrieved.');
            
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

            // technical info
            var techInfo = result.technicalInfo;
            expect(techInfo).toBeDefinedAndNotNull('JSON techInfo object should be retrieved.');
            expect(techInfo.blogPostContentId).toBe('335a518ba9e841eaab84a20500bbe74c', 'Correctly filtered techInfo.blogPostContentId should be retrieved.');
            expect(techInfo.pageContentId).toBe('2815b2be74314dff96aaa20500bbe74e', 'Correctly filtered techInfo.pageContentId should be retrieved.');
            expect(techInfo.regionId).toBe('e3e2e7fe62df4ba683216fdcc1691d8a', 'Correctly filtered techInfo.regionId should be retrieved.');

            // access rules
            var accessRules = result.accessRules;
            expect(accessRules).toBeDefinedAndNotNull('JSON accessRules object should be retrieved.');
            expect(accessRules.length).toBe(6, 'Returned accessRules array length should be 6.');

            var rule1 = accessRules[0],
                rule2 = accessRules[1],
                rule3 = accessRules[2],
                rule4 = accessRules[3],
                rule5 = accessRules[4],
                rule6 = accessRules[5];

            expect(rule1.isForRole).toBe(false, 'Correctly filtered accessRules[0].isForRole should be false.');
            expect(rule2.isForRole).toBe(false, 'Correctly filtered accessRules[1].isForRole should be false.');
            expect(rule3.isForRole).toBe(false, 'Correctly filtered accessRules[2].isForRole should be false.');
            expect(rule4.isForRole).toBe(true, 'Correctly filtered accessRules[3].isForRole should be true.');
            expect(rule5.isForRole).toBe(true, 'Correctly filtered accessRules[4].isForRole should be true.');
            expect(rule6.isForRole).toBe(true, 'Correctly filtered accessRules[5].isForRole should be true.');

            expect(rule1.accessLevel).toBe('ReadWrite', 'Correctly filtered accessRules[0].accessLevel should be ReadWrite.');
            expect(rule2.accessLevel).toBe('Deny', 'Correctly filtered accessRules[1].accessLevel should be Deny.');
            expect(rule3.accessLevel).toBe('Read', 'Correctly filtered accessRules[2].accessLevel should be Read.');
            expect(rule4.accessLevel).toBe('ReadWrite', 'Correctly filtered accessRules[3].accessLevel should be ReadWrite.');
            expect(rule5.accessLevel).toBe('Read', 'Correctly filtered accessRules[4].accessLevel should be Read.');
            expect(rule6.accessLevel).toBe('Deny', 'Correctly filtered accessRules[5].accessLevel should be Deny.');

            expect(rule1.identity).toBe('user1', 'Correctly filtered accessRules[0].identity should be user1.');
            expect(rule2.identity).toBe('user2', 'Correctly filtered accessRules[1].identity should be user2.');
            expect(rule3.identity).toBe('user3', 'Correctly filtered accessRules[2].identity should be user3.');
            expect(rule4.identity).toBe('Authenticated Users', 'Correctly filtered accessRules[3].identity should be Authenticated Users.');
            expect(rule5.identity).toBe('Everyone', 'Correctly filtered accessRules[4].identity should be Everyone.');
            expect(rule6.identity).toBe('role1', 'Correctly filtered accessRules[5].identity should be role1.');

            // Child content options
            expect(result.childContentsOptionValues).toBeDefinedAndNotNull('Correct childContentsOptionValues should be retrieved.');
            expect(result.childContentsOptionValues.length).toBe(2, 'Correct childContentsOptionValues.length should be retrieved.');

            var child = result.childContentsOptionValues[1];
            expect(child.assignmentIdentifier).toBe('94837f8d922d4ef2859cd99a1c66dbea', 'Correctly filtered childContentsOptionValues[1].assignmentIdentifier should be retrieved');
            expect(child.optionValues).toBeDefinedAndNotNull('Correctly filtered childContentsOptionValues[1].optionValues should be retrieved');
            expect(child.optionValues.length).toBe(2, 'The length of childContentsOptionValues[1].optionValues array should be 2');
            expect(child.optionValues[1].key).toBe('O3', 'Correctly filtered childContentsOptionValues[1].optionValues[1].key should be retrieved');
            expect(child.optionValues[1].value).toBe('902c287b-9eef-4de1-8975-a20601052b9a', 'Correctly filtered childContentsOptionValues[1].optionValues[1].value should be retrieved');
            expect(child.optionValues[1].defaultValue).toBe('0dbf035e-a1b8-4fe1-ba61-a20500fb8491', 'Correctly filtered childContentsOptionValues[1].optionValues[1].defaultValue should be retrieved');
            expect(child.optionValues[1].type).toBe('Custom', 'Correctly filtered childContentsOptionValues[1].optionValues[1].type should be retrieved');
            expect(child.optionValues[1].useDefaultValue).toBe(false, 'Correctly filtered childContentsOptionValues[1].optionValues[1].useDefaultValue should be retrieved');
            expect(child.optionValues[1].customTypeIdentifier).toBe('media-images-folder', 'Correctly filtered childContentsOptionValues[1].optionValues[1].customTypeIdentifier should be retrieved');

            child = result.childContentsOptionValues[0];
            expect(child.assignmentIdentifier).toBe('ae2efe36ec0c48dab9900babddd46e9f', 'Correctly filtered childContentsOptionValues[0].assignmentIdentifier should be retrieved');
            expect(child.optionValues).toBeDefinedAndNotNull('Correctly filtered childContentsOptionValues[0].optionValues should be retrieved');
            expect(child.optionValues.length).toBe(1, 'The length of childContentsOptionValues[0].optionValues array should be 1');
            expect(child.optionValues[0].key).toBe('O1', 'Correctly filtered childContentsOptionValues[0].optionValues[0].key should be retrieved');
            expect(child.optionValues[0].value).toBe('V1', 'Correctly filtered childContentsOptionValues[0].optionValues[0].value should be retrieved');
            expect(child.optionValues[0].defaultValue).toBeNull('Correctly filtered childContentsOptionValues[0].optionValues[0].defaultValue should be retrieved');
            expect(child.optionValues[0].type).toBe('Text', 'Correctly filtered childContentsOptionValues[0].optionValues[0].type should be retrieved');
            expect(child.optionValues[0].useDefaultValue).toBe(false, 'Correctly filtered childContentsOptionValues[0].optionValues[0].useDefaultValue should be retrieved');
            expect(child.optionValues[0].customTypeIdentifier).toBeNull('Correctly filtered childContentsOptionValues[0].optionValues[0].customTypeIdentifier should be retrieved');
        });
    });

    it('02105.2: Should test CRUD for blog post properties.', function () {
        api.testCrud(runs, waitsFor, expect, constants.testPageId, "/bcms-api/blog-post-properties/", {
            getPostData: function (json) {
                json.data.title = "Test 02105.2: " + api.createGuid();
                json.data.blogPostUrl = null;
                json.data.version = 0;
                return json.data;
            }
        });
    });

    it('02106: Should get blog post list, filtered by tags, using AND connector', function () {
        filterByTags('and', 1, ['IFilterByTags Page 1']);
    });

    it('02107: Should get blog post list, filtered by tags, using OR connector', function () {
        filterByTags('or', 2, ['IFilterByTags Page 1', 'IFilterByTags Page 3']);
    });
    
    it('02108: Should get a list with one blog post (with layout), filtered by all available columns', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: 'c1efcb1107ed4901abb3a206012b0b87' },
                    { field: 'CreatedOn', value: '2013-07-25 18:08:47.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2014-08-21 15:36:51.000' },
                    { field: 'LastModifiedBy', value: 'admin' },
                    { field: 'Version', value: '4' },

                    { field: 'Title', value: '02108' },
                    { field: 'BlogPostUrl', value: '/articles/02108/' },
                    { field: 'IntroText', value: '02108 description' },
                    { field: 'IsPublished', value: true },
                    { field: 'PublishedOn', value: '2013-07-25 18:13:56.000' },
                    { field: 'LayoutId', value: '0e991684003a43deb40f0ffeccddc6eb' },
                    { field: 'MasterPageId' },
                    { field: 'CategoryId', value: '4e2095dbbfa4405e808aa206012c2486' },
                    { field: 'CategoryName', value: '02108' },
                    { field: 'AuthorId', value: '58ee1671c9714f05b45ca206012c210b' },
                    { field: 'AuthorName', value: '02108' },
                    { field: 'MainImageId', value: 'ca4c9fb204554aecadbea206012c58a0' },
                    { field: 'MainImageUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/b2af6ffd204941a3ae8b59855911293a/1_1.jpg' },
                    { field: 'MainImageThumbnauilUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/b2af6ffd204941a3ae8b59855911293a/t_1_1.png' },
                    { field: 'MainImageThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/b2af6ffd204941a3ae8b59855911293a/t_1_1.png' },
                    { field: 'MainImageCaption', value: '02108 caption' },
                    { field: 'ActivationDate', value: '2013-07-25 00:00:00.000' },
                    { field: 'ExpirationDate', value: '2032-07-25 23:59:59.000' },
                    { field: 'IsArchived', value: false },
                    { field: 'LanguageId', value: '2000fc5567104616a34fa38e0100f060' },
                    { field: 'LanguageGroupIdentifier', value: '2fc90bc7af3341eb84cec8d29baaf6a4' },
                    { field: 'LanguageCode', value: 'arn' },
                    { field: 'ContentId', value: 'a3867237aa3345619051a206012b0b86' }
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

            expect(result.data.items[0].id).toBe('c1efcb1107ed4901abb3a206012b0b87', 'Correctly filtered items[0].id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            // data.filter.where.length + 2 <-- Because fields [Tags, AccessRules] cannnot be filtered by
            expect(data.filter.where.length + 2).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
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

    it('02112: Should get a list with one blog post (with master page), filtered by all available columns', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: '98a7ba1fbadd419db331a2f100b48f93' },
                    { field: 'CreatedOn', value: '2014-03-17 10:57:24.000' },
                    { field: 'CreatedBy', value: 'admin' },
                    { field: 'LastModifiedOn', value: '2014-08-21 15:36:51' },
                    { field: 'LastModifiedBy', value: 'admin' },
                    { field: 'Version', value: '4' },

                    { field: 'Title', value: '02112' },
                    { field: 'BlogPostUrl', value: '/articles/02112/' },
                    { field: 'IntroText', value: '02112' },
                    { field: 'IsPublished', value: true },
                    { field: 'PublishedOn', value: '2014-03-17 10:57:24.000' },
                    { field: 'LayoutId' },
                    { field: 'MasterPageId', value: '0eb7fcebf60e4227ab76a2f100b4c011' },
                    { field: 'CategoryId', value: '210e1d69c13442329a85a2f100b40868' },
                    { field: 'CategoryName', value: '02112' },
                    { field: 'AuthorId', value: '9c6d8902d39c4fc984b0a2f100b413df' },
                    { field: 'AuthorName', value: '02112' },
                    { field: 'MainImageId', value: 'b2e5f87a23834f438e50a2f100b46d79' },
                    { field: 'MainImageUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/cb58399225854a3285a2c8c081f7ca56/1_1.jpg' },
                    { field: 'MainImageThumbnauilUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/cb58399225854a3285a2c8c081f7ca56/t_1_1.png' },
                    { field: 'MainImageThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/cb58399225854a3285a2c8c081f7ca56/t_1_1.png' },
                    { field: 'MainImageCaption', value: '02112 caption' },
                    { field: 'ActivationDate', value: '2014-03-17 00:00:00.000' },
                    { field: 'ExpirationDate', value: '2014-03-19 23:59:59.000' },
                    { field: 'IsArchived', value: false },
                    { field: 'LanguageId', value: 'c0c6e81497e94ced8b8da38e01010139' },
                    { field: 'LanguageCode', value: 'arn-CL' },
                    { field: 'LanguageGroupIdentifier', value: '2fc90bc7af3341eb84cec8d29baaf6a4' },
                    { field: 'ContentId', value: 'bd05f17eecd54bbb85b3a2f100b48f92' }
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

            expect(result.data.items[0].id).toBe('98a7ba1fbadd419db331a2f100b48f93', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            // data.filter.where.length + 2 <-- Because fields [Tags, AccessRules] cannnot be filtered by
            expect(data.filter.where.length + 2).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });

    it('02113.1: Should get list of blog posts for user admin with roles: [role1, role2]', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '02113' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeUnpublished: true,
            includeArchived: true
        },
            user = {
                name: 'admin',
                roles: ['role1', 'role2']
            };

        runs(function () {
            api.getSecured(url, data, user, function (json) {
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
            expect(result.data.totalCount).toBe(3, 'Total count should be 3.');
            expect(result.data.items.length).toBe(3, 'Returned array length should be 3.');

            expect(result.data.items[0].title).toBe('02113: for all', 'Correctly filtered data.items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('02113: only for role role1', 'Correctly filtered data.items[1].title should be retrieved.');
            expect(result.data.items[2].title).toBe('02113: only for role role2', 'Correctly filtered data.items[2].title should be retrieved.');
        });
    });

    it('02113.2: Should get list of blog posts for user admin without roles', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '02113' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeUnpublished: true,
            includeArchived: true
        },
            user = {
                name: 'admin'
            };

        runs(function () {
            api.getSecured(url, data, user, function (json) {
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

            expect(result.data.items[0].title).toBe('02113: for all', 'Correctly filtered data.items[0].title should be retrieved.');
        });
    });

    it('02113.3: Should get list of blog posts for user admin2 with [role1]', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '02113' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeUnpublished: true,
            includeArchived: true
        },
            user = {
                name: 'admin2',
                roles: ['role1']
            };

        runs(function () {
            api.getSecured(url, data, user, function (json) {
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
            expect(result.data.items.length).toBe(4, 'Returned array length should be 4.');

            expect(result.data.items[0].title).toBe('02113: denied for admin', 'Correctly filtered data.items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('02113: for all', 'Correctly filtered data.items[1].title should be retrieved.');
            expect(result.data.items[2].title).toBe('02113: only for admin 2', 'Correctly filtered data.items[2].title should be retrieved.');
            expect(result.data.items[3].title).toBe('02113: only for role role1', 'Correctly filtered data.items[3].title should be retrieved.');
        });
    });

    it('02114: Should get list of blog pots with access rules', function () {
        var url = '/bcms-api/blog-posts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', value: '02114' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeUnpublished: true,
            includeArchived: true,
            includeAccessRules: true
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

            expect(result.data.items[0].accessRules.length).toBe(6, 'Correctly filtered items[0].accessRules.length should be 6.');

            var rule1 = result.data.items[0].accessRules[0],
                rule2 = result.data.items[0].accessRules[1],
                rule3 = result.data.items[0].accessRules[2],
                rule4 = result.data.items[0].accessRules[3],
                rule5 = result.data.items[0].accessRules[4],
                rule6 = result.data.items[0].accessRules[5];

            expect(rule1.isForRole).toBe(false, 'Correctly filtered accessRules[0].isForRole should be false.');
            expect(rule2.isForRole).toBe(false, 'Correctly filtered accessRules[1].isForRole should be false.');
            expect(rule3.isForRole).toBe(false, 'Correctly filtered accessRules[2].isForRole should be false.');
            expect(rule4.isForRole).toBe(true, 'Correctly filtered accessRules[3].isForRole should be true.');
            expect(rule5.isForRole).toBe(true, 'Correctly filtered accessRules[4].isForRole should be true.');
            expect(rule6.isForRole).toBe(true, 'Correctly filtered accessRules[5].isForRole should be true.');

            expect(rule1.accessLevel).toBe('ReadWrite', 'Correctly filtered accessRules[0].accessLevel should be ReadWrite.');
            expect(rule2.accessLevel).toBe('Deny', 'Correctly filtered accessRules[1].accessLevel should be Deny.');
            expect(rule3.accessLevel).toBe('Read', 'Correctly filtered accessRules[2].accessLevel should be Read.');
            expect(rule4.accessLevel).toBe('ReadWrite', 'Correctly filtered accessRules[3].accessLevel should be ReadWrite.');
            expect(rule5.accessLevel).toBe('Read', 'Correctly filtered accessRules[4].accessLevel should be Read.');
            expect(rule6.accessLevel).toBe('Deny', 'Correctly filtered accessRules[5].accessLevel should be Deny.');

            expect(rule1.identity).toBe('user1', 'Correctly filtered accessRules[0].identity should be user1.');
            expect(rule2.identity).toBe('user2', 'Correctly filtered accessRules[1].identity should be user2.');
            expect(rule3.identity).toBe('user3', 'Correctly filtered accessRules[2].identity should be user3.');
            expect(rule4.identity).toBe('Authenticated Users', 'Correctly filtered accessRules[3].identity should be Authenticated Users.');
            expect(rule5.identity).toBe('Everyone', 'Correctly filtered accessRules[4].identity should be Everyone.');
            expect(rule6.identity).toBe('role1', 'Correctly filtered accessRules[5].identity should be role1.');
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
            includeArchived: includeArchived,
            includeCategories: true
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
            },Should get blog post properties by id..
Expected 0 to be 1, 'Categories count should be 1.'.
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