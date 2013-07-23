/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Pages Web API', function () {
    'use strict';

    var constants = {
        testPageId: 'f0464c233b67406babe8a20400b4d8b8',
        testPageTitle: '_0000_Page_For_Tests',
        testPageUrl: '/0000-page-for-tests/'
    };

    it('0000: Should get pages list', function() {
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.totalCount).toBe(4);
            expect(result.data.items.length).toBe(2);

            expect(result.data.items[0].title.substr(0, 6)).toBe('_0000_');
            expect(result.data.items[1].title.substr(0, 6)).toBe('_0000_');
        });
    });
    
    it('0001: Should get one page in list', function () {
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);

            expectPageListItemPropertiesAreNotNull(result.data.items[0]);
        });
    });

    it('0002: Should get page by id', function() {
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();

            expectPagePropertiesAreNotNull(result.data);
        });
    });

    it('0003: Should get page by url', function () {
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();

            expectPagePropertiesAreNotNull(result.data);
        });
    });

    it('0004: Should get page properties by id', function () {
        var url = '/bcms-api/page-properties/' + constants.testPageId,
             result,
             ready = false;

        var data = {
            // TODO: remove Hack after tests and solution:
            pageId: constants.testPageId,
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();

            expectPagePropertiesPropertiesAreNotNull(result);
        });
    });

    it('0005: Should get page properties by url', function () {
        expect(true).toBe(false);
    });
    
    it('0006: Page should exists', function () {
        expect(true).toBe(false);
    });

    it('0007: Page should not exists', function () {
        expect(true).toBe(false);
    });
    
    function expectPageListItemPropertiesAreNotNull(page) {
        api.expectBasePropertiesAreNotNull(page);

        expect(page.title).toBe(constants.testPageTitle);
        expect(page.pageUrl).toBe(constants.testPageUrl);
        expect(page.description).toBe('Test page');
        expect(page.isPublished).toBe(true);
        expect(page.publishedOn).toBeDefined();
        expect(page.layoutId).toBeDefined();
        expect(page.categoryId).toBeDefined();
        expect(page.categoryName).toBe('Category for _0000_Page_For_Tests');
        expect(page.mainImageId).toBeDefined();
        expect(page.mainImageThumbnauilUrl).toBeDefined();
        expect(page.mainImageCaption).toBe("Image for _0000_Page_For_Tests");
        expect(page.isArchived).toBe(true);
    }

    function expectPagePropertiesAreNotNull(page) {
        api.expectBasePropertiesAreNotNull(page);

        expect(page.title).toBe(constants.testPageTitle);
        expect(page.pageUrl).toBe(constants.testPageUrl);
        expect(page.description).toBe('Test page');
        expect(page.isPublished).toBe(true);
        expect(page.publishedOn).toBeDefined();
        expect(page.layoutId).toBeDefined();
        expect(page.categoryId).toBeDefined();
        expect(page.categoryName).toBe('Category for _0000_Page_For_Tests');
        expect(page.mainImageId).toBeDefined();
        expect(page.mainImageThumbnauilUrl).toBeDefined();
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
        expect(page.publishedOn).toBeDefined();
        expect(page.layoutId).toBeDefined();
        expect(page.categoryId).toBeDefined();
        expect(page.mainImageId).toBeDefined();
        expect(page.featuredImageId).toBeDefined();
        expect(page.secondaryImageId).toBeDefined();
        expect(page.canonicalUrl).toBe('canonical-url');
        expect(page.customCss).toBe('test page custom css');
        expect(page.customJavaScript).toBe('console.log("test");');
        expect(page.useCanonicalUrl).toBe(true);
        expect(page.useNoFollow).toBe(true);
        expect(page.useNoIndex).toBe(true);
        expect(page.isArchived).toBe(true);
        
        // layout
//        expect(response.layout).toBeDefined();
//        
//        // category
//        expect(response.category).toBeDefined();
//        
//        // tags
//        expect(response.tags).toBeDefined();
//        
//        // main image
//        expect(response.mainImage).toBeDefined();
//        
//        // featured image
//        expect(response.featuredImage).toBeDefined();
//        
//        // secondary image
//        expect(response.secondaryImage).toBeDefined();
//        
//        // meta data
//        expect(response.metaData).toBeDefined();
//        
//        // page contents
//        expect(response.pageContents).toBeDefined();
    }
});