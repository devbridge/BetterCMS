/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Pages: Pages', function () {
    'use strict';

    var constants = {
        testPageId: 'f0464c233b67406babe8a20400b4d8b8',
        testPageTitle: '_0000_Page_For_Tests',
        testPageUrl: '/0000-page-for-tests/'
    };

    it('01000: Should get pages list', function() {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: { 
                where: [ { field: 'Title', operation: 'StartsWith', value: '_0000_' } ]
            },
            take: 2,
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
            expect(result.data.totalCount).toBe(4);
            expect(result.data.items.length).toBe(2);

            expect(result.data.items[0].title.substr(0, 6)).toBe('_0000_');
            expect(result.data.items[1].title.substr(0, 6)).toBe('_0000_');
        });
    });
    
    it('01001: Should get one page in list', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', value: constants.testPageTitle }]
            },
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
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);

            expectPageListItemPropertiesAreNotNull(result.data.items[0]);
        });
    });

    it('01002: Should get page by id', function() {
        var url = '/bcms-api/pages/' + constants.testPageId,
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
            expect(result.data).not.toBeNull();

            expectPagePropertiesAreNotNull(result.data);
        });
    });

    it('01003: Should get page by url', function () {
        var url = '/bcms-api/pages/by-url/' + constants.testPageUrl,
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
            expect(result.data).not.toBeNull();

            expectPagePropertiesAreNotNull(result.data);
        });
    });

    it('01004: Should get page properties by id', function () {
        var url = '/bcms-api/page-properties/' + constants.testPageId,
             result,
             ready = false;

        var data = {
            includeTags: true,
            includeLayout: true,
            includeCategory: true,
            includeImages: true,
            includeMetaData: true,
            includePageContents: true,
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

            expectPagePropertiesPropertiesAreNotNull(result);
        });
    });

    it('01005: Should get page properties by url', function () {
        var url = '/bcms-api/page-properties/by-url/' + constants.testPageUrl,
              result,
              ready = false;

        var data = {
            includeTags: true,
            includeLayout: true,
            includeCategory: true,
            includeImages: true,
            includeMetaData: true,
            includePageContents: true,
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

            expectPagePropertiesPropertiesAreNotNull(result);
        });
    });
    
    it('01006: Page should exist', function () {
        var url = '/bcms-api/page-exists/' + constants.testPageUrl,
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
            expect(result.data).not.toBeNull();
            expect(result.data.exists).toBe(true);
            expect(result.data.pageId).toBe(constants.testPageId);
        });
    });

    it('01007: Page should not exist', function () {
        var url = '/bcms-api/page-exists/7E78D59E-6747-46D1-B71D-852F44F99E71/',
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
            expect(result.data).not.toBeNull();
            expect(result.data.exists).toBe(false);
            expect(result.data.pageId).toBeNull();
        });
    });

    it('01008: Should get page contents', function () {
        var url = '/bcms-api/pages/' + constants.testPageId + '/contents/',
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
            expect(result.data).not.toBeNull();
            expect(result.data.items).not.toBeNull();
            
            var contents = result.data.items;
            expect(contents.length).toBe(5);
            
            var contentsFound = 0;
            for (var i = 0; i < contents.length; i++) {
                if (contents[i].name == 'HeaderContent1') {
                    expect(contents[i].contentType).toBe('html-content');
                    expect(contents[i].regionIdentifier).toBe('CMSHeader');

                    contentsFound++;
                }
                if (contents[i].name == 'MainContent1 - HTML') {
                    var content = contents[i];
                    api.expectBasePropertiesAreNotNull(content);
                    expect(content.contentType).toBe('html-content');
                    expect(content.regionIdentifier).toBe('CMSMainContent');
                    expect(content.isPublished).toBe(true);
                    expect(content.regionId).not.toBeNull();

                    contentsFound++;
                }
                if (contents[i].name == 'Html widget for _0000_Page_For_Tests') {
                    expect(contents[i].contentType).toBe('html-widget');
                    expect(contents[i].regionIdentifier).toBe('CMSMainContent');

                    contentsFound++;
                }
                if (contents[i].name == 'Server Widget for for _0000_Page_For_Tests') {
                    expect(contents[i].contentType).toBe('server-widget');
                    expect(contents[i].regionIdentifier).toBe('CMSMainContent');

                    contentsFound++;
                }
            }

            expect(contentsFound).toBe(4);
        });
    });

    it('01009: Should get pages list, filtered by tags, using AND connector', function () {
        filterByTags('and', 1, ['IFilterByTags Page 1']);
    });
    
    it('01010: Should get pages list, filtered by tags, using OR connector', function () {
        filterByTags('or', 2, ['IFilterByTags Page 1', 'IFilterByTags Page 3']);
    });

    function expectPageListItemPropertiesAreNotNull(page) {
        api.expectBasePropertiesAreNotNull(page);

        expect(page.title).toBe(constants.testPageTitle);
        expect(page.pageUrl).toBe(constants.testPageUrl);
        expect(page.description).toBe('Test page');
        expect(page.isPublished).toBe(true);
        expect(page.publishedOn).not.toBeNull();
        expect(page.layoutId).not.toBeNull();
        expect(page.categoryId).not.toBeNull();
        expect(page.categoryName).toBe('Category for _0000_Page_For_Tests');
        expect(page.mainImageId).not.toBeNull();
        expect(page.mainImageThumbnauilUrl).not.toBeNull();
        expect(page.mainImageCaption).toBe("Image for _0000_Page_For_Tests");
        expect(page.isArchived).toBe(true);
    }

    function expectPagePropertiesAreNotNull(page) {
        api.expectBasePropertiesAreNotNull(page);

        expect(page.title).toBe(constants.testPageTitle);
        expect(page.pageUrl).toBe(constants.testPageUrl);
        expect(page.description).toBe('Test page');
        expect(page.isPublished).toBe(true);
        expect(page.publishedOn).not.toBeNull();
        expect(page.layoutId).not.toBeNull();
        expect(page.categoryId).not.toBeNull();
        expect(page.categoryName).toBe('Category for _0000_Page_For_Tests');
        expect(page.mainImageId).not.toBeNull();
        expect(page.mainImageThumbnauilUrl).not.toBeNull();
        expect(page.mainImageCaption).toBe("Image for _0000_Page_For_Tests");
        expect(page.isArchived).toBe(true);
    }

    function expectPagePropertiesPropertiesAreNotNull(response) {
        // Page
        var page = response.data;
        api.expectBasePropertiesAreNotNull(page);
        expect(page.title).toBe(constants.testPageTitle);
        expect(page.pageUrl).toBe(constants.testPageUrl);
        expect(page.description).toBe('Test page');
        expect(page.isPublished).toBe(true);
        expect(page.publishedOn).not.toBeNull();
        expect(page.layoutId).not.toBeNull();
        expect(page.categoryId).not.toBeNull();
        expect(page.mainImageId).not.toBeNull();
        expect(page.featuredImageId).not.toBeNull();
        expect(page.secondaryImageId).not.toBeNull();
        expect(page.canonicalUrl).toBe('canonical-url');
        expect(page.customCss).toBe('test page custom css');
        expect(page.customJavaScript).toBe('console.log("test");');
        expect(page.useCanonicalUrl).toBe(true);
        expect(page.useNoFollow).toBe(true);
        expect(page.useNoIndex).toBe(true);
        expect(page.isArchived).toBe(true);
        
        // layout
        var layout = response.layout;
        expect(layout).not.toBeNull();
        api.expectBasePropertiesAreNotNull(layout);
        expect(layout.name).toBe('_0001_Layout3 for _0000_Page_For_Tests');
        expect(layout.layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml');
        expect(layout.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png');
        
        // category
        var category = response.category;
        expect(category).not.toBeNull();
        api.expectBasePropertiesAreNotNull(category);
        expect(category.name).toBe('Category for _0000_Page_For_Tests');
        
        // tags
        var tags = response.tags;
        expect(tags).not.toBeNull();
        expect(tags.length).toBe(2);
        expect(tags[0].name).toBe('tag1');

        api.expectBasePropertiesAreNotNull(tags[1]);
        expect(tags[1].name).toBe('tag2');
        
        // images
        expectImagePropertiesAreNotNull(response.mainImage);
        expectImagePropertiesAreNotNull(response.featuredImage);
        expectImagePropertiesAreNotNull(response.secondaryImage);
        
        // meta data
        var metadata = response.metaData;
        expect(metadata).not.toBeNull();
        expect(metadata.metaTitle).toBe('Test meta title');
        expect(metadata.metaKeywords).toBe('Test meta keywords');
        expect(metadata.metaDescription).toBe('Test meta description');
        
        // page contents
        var contents = response.pageContents;
        expect(contents).not.toBeNull();
        expect(contents.length).toBe(5);

        var contentsFound = 0;
        for (var i = 0; i < contents.length; i ++)
        {
            if (contents[i].name == 'HeaderContent1') {
                expect(contents[i].contentType).toBe('html-content');
                expect(contents[i].regionIdentifier).toBe('CMSHeader');

                contentsFound++;
            }
            if (contents[i].name == 'MainContent1 - HTML') {
                var content = contents[i];
                api.expectBasePropertiesAreNotNull(content);
                expect(content.contentType).toBe('html-content');
                expect(content.regionIdentifier).toBe('CMSMainContent');
                expect(content.isPublished).toBe(true);
                expect(content.regionId).not.toBeNull();
                
                contentsFound++;
            }
            if (contents[i].name == 'Html widget for _0000_Page_For_Tests') {
                expect(contents[i].contentType).toBe('html-widget');
                expect(contents[i].regionIdentifier).toBe('CMSMainContent');

                contentsFound++;
            }
            if (contents[i].name == 'Server Widget for for _0000_Page_For_Tests') {
                expect(contents[i].contentType).toBe('server-widget');
                expect(contents[i].regionIdentifier).toBe('CMSMainContent');

                contentsFound++;
            }
        }

        expect(contentsFound).toBe(4);
    }
    
    function expectImagePropertiesAreNotNull(image) {
        expect(image).not.toBeNull();
        api.expectBasePropertiesAreNotNull(image);
        
        expect(image.title).toBe('Image for _0000_Page_For_Tests');
        expect(image.caption).toBe('Image for _0000_Page_For_Tests');
        expect(image.url).not.toBeNull();
        expect(image.thumbnailUrl).not.toBeNull();
    }
    
    function filterByTags(connector, expectedCount, expectedTitles) {
        var url = '/bcms-api/pages/',
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