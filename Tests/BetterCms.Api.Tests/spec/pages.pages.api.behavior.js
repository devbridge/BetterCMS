/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('pages.pages.api.behavior', function () {
    'use strict';

    var constants = {
        testPageId: 'f0464c233b67406babe8a20400b4d8b8',
        testPageTitle: '_0000_Page_For_Tests',
        testPageUrl: '/0000-page-for-tests/',
        testPage2Id: '0a9a85852e4944e4a5d98bcee6d88793'
    };

    it('01000: Should get list of pages', function() {
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(4, 'Total count should be 4.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');

            expect(result.data.items[0].title.substr(0, 6)).toBe('_0000_', 'Correctly filtered items[0].title should be retrieved.');
            expect(result.data.items[1].title.substr(0, 6)).toBe('_0000_', 'Correctly filtered items[1].title should be retrieved.');
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
            includeArchived: true,
            includeCategories: true,
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');

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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');

            expectPagePropertiesAreNotNull(result.data);
        });
    });

    it('01004.1: Should get page properties by id', function () {
        var url = '/bcms-api/page-properties/' + constants.testPageId,
             result,
             ready = false;

        var data = {
            includeTags: true,
            includeLayout: true,
            includeCategories: true,
            includeImages: true,
            includeMetaData: true,
            includePageContents: true,
            includePageOptions: true,
            includeLanguage: true,
            includePageTranslations: true,
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

            expectPagePropertiesPropertiesAreNotNull(result);
        });
    });

    it('01004.2: Should test CRUD for pages.', function () {
        api.testCrud(runs, waitsFor, expect, constants.testPageId, "/bcms-api/page-properties/", {
            getPostData: function (json) {
                json.data.title = api.createGuid();
                json.data.pageUrl = null;
                json.data.version = 0;
                return json.data;
            }
        });
    });

    it('01005: Should get page properties by url', function () {
        var url = '/bcms-api/page-properties/by-url/' + constants.testPageUrl,
              result,
              ready = false;

        var data = {
            includeTags: true,
            includeLayout: true,
            includeCategories: true,
            includeImages: true,
            includeMetaData: true,
            includePageContents: true,
            includePageOptions: true,
            includeLanguage: true,
            includePageTranslations: true,
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.exists).toBe(true, 'Correctly filtered exists should be retrieved.');
            expect(result.data.pageId).toBe(constants.testPageId, 'Correctly filtered pageId should be retrieved.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.exists).toBe(false, 'Correctly filtered exists should be retrieved.');
            expect(result.data.pageId).toBeNull('pageId should be null.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.items).not.toBeNull('JSON data.items object should be retrieved.');
            
            var contents = result.data.items;
            expect(contents.length).toBe(5, 'Returned array length should be 5.');
            
            var contentsFound = 0;
            for (var i = 0; i < contents.length; i++) {
                if (contents[i].name == 'HeaderContent1') {
                    expect(contents[i].contentType).toBe('html-content', 'Correctly filtered contentType should be retrieved.');
                    expect(contents[i].regionIdentifier).toBe('CMSHeader', 'Correctly filtered regionIdentifier should be retrieved.');

                    contentsFound++;
                }
                if (contents[i].name == 'MainContent1 - HTML') {
                    var content = contents[i];
                    api.expectBasePropertiesAreNotNull(content);
                    expect(content.contentType).toBe('html-content', 'Correctly filtered contentType should be retrieved.');
                    expect(content.regionIdentifier).toBe('CMSMainContent', 'Correctly filtered regionIdentifier should be retrieved.');
                    expect(content.isPublished).toBe(true, 'Correctly filtered content.isPublished should be retrieved.');
                    expect(content.regionId).toBeDefinedAndNotNull('regionId should be retrieved.');

                    contentsFound++;
                }
                if (contents[i].name == 'Html widget for _0000_Page_For_Tests') {
                    expect(contents[i].contentType).toBe('html-widget', 'Correctly filtered contentType should be retrieved.');
                    expect(contents[i].regionIdentifier).toBe('CMSMainContent', 'Correctly filtered regionIdentifier should be retrieved.');

                    contentsFound++;
                }
                if (contents[i].name == 'Server Widget for for _0000_Page_For_Tests') {
                    expect(contents[i].contentType).toBe('server-widget', 'Correctly filtered contentType should be retrieved.');
                    expect(contents[i].regionIdentifier).toBe('CMSMainContent', 'Correctly filtered regionIdentifier should be retrieved.');

                    contentsFound++;
                }
            }

            expect(contentsFound).toBe(4, 'Correct count of found contents should be retrieved.');
        });
    });

    it('01008.1: Should get page content by id', function() {
        var url = '/bcms-api/pages/' + constants.testPageId + '/contents/' + 'd88b0aa8172149bb86c8a20400fe687e',
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
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.items).not.toBeNull('JSON data.items object should be retrieved.');
        });
    });

    it('01008.2: Should test CRUD for page contents.', function () {
        api.testCrud(runs, waitsFor, expect, "d88b0aa8172149bb86c8a20400fe687e", '/bcms-api/pages/' + constants.testPageId + '/contents/', {
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });

    it('01008.3: Should destroy content draft.', function () {
        // TODO: imepelement.
        // DELETE: /bcms-api/contents/{Id}/draft
    });

    it('01009: Should get pages list, filtered by tags, using AND connector', function () {
        filterByTags('and', 1, ['IFilterByTags Page 1']);
    });
    
    it('01010: Should get pages list, filtered by tags, using OR connector', function () {
        filterByTags('or', 2, ['IFilterByTags Page 1', 'IFilterByTags Page 3']);
    });

    it('01010.1: Should get pages list, filtered by categories, using AND connector', function () {
        filterByCategories('and', 1, ['IFilterByCategories Page 1'], 'id');
    });

    it('01010.2: Should get pages list, filtered by categories, using OR connector', function () {
        filterByCategories('or', 2, ['IFilterByCategories Page 1', 'IFilterByCategories Page 2'], 'id');
    });

    it('01010.3: Should get pages list, filtered by categories names, using AND connector', function () {
        filterByCategories('and', 1, ['IFilterByCategories Page 1'], 'name');
    });

    it('01010.4: Should get pages list, filtered by categories names, using OR connector', function () {
        filterByCategories('or', 2, ['IFilterByCategories Page 1', 'IFilterByCategories Page 2'], 'name');
    });

    it('01011: Should get a list with one page (with layout), filtered by all available columns', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: 'e81a87022bf4419688b4a2070081a57e' },
                    { field: 'CreatedOn', value: '2013-07-26 07:52:01.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 07:58:15.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '4' },

                    { field: 'PageUrl', value: '/01103/01011/' },
                    { field: 'Title', value: '01011' },
                    { field: 'Description' },
                    { field: 'IsPublished', value: true },
                    { field: 'PublishedOn', value: '2013-07-26 07:58:15.000' },
                    { field: 'LayoutId', value: '9ab0bbe1e02a4a3bb842a2070082af10' },
                    { field: 'MasterPageId' },
                    { field: 'IsArchived', value: false },
                    { field: 'IsMasterPage', value: false },
                    { field: 'LanguageId', value: '67432fcff2c349c09678a2a70091cf48' },
                    { field: 'LanguageCode', value: 'ar-YE' },
                    { field: 'LanguageGroupIdentifier', value: 'b2aa47dc114b47dd8f24d489bf8cdf71' },

                    { field: 'MainImageId', value: '389e059bcc6c4336a863a2070083436c' },
                    { field: 'MainImageUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/6173795ceadc4b619d68005ef57c9ca8/1_1.jpg' },
                    { field: 'MainImageThumbnauilUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/6173795ceadc4b619d68005ef57c9ca8/t_1_1.png' },
                    { field: 'MainImageThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/6173795ceadc4b619d68005ef57c9ca8/t_1_1.png' },
                    { field: 'MainImageCaption', value: '01011 caption' },

                    { field: 'SecondaryImageId', value: '5ae75858d0da418c9ed8a3b600d89294' },
                    { field: 'SecondaryImageUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/2c2b58710274429fbb99488d8194907f/__sam.png' },
                    { field: 'SecondaryImageThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/2c2b58710274429fbb99488d8194907f/t___sam.png' },
                    { field: 'SecondaryImageCaption', value: '01011-secondary-caption' },

                    { field: 'FeaturedImageId', value: '059f65eef4bc43c1be9da3b600d89294' },
                    { field: 'FeaturedImageUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/0f1037a441ee4b3aa2216ecff1183eb9/__logo-square.png' },
                    { field: 'FeaturedImageThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/0f1037a441ee4b3aa2216ecff1183eb9/t___logo-square.png' },
                    { field: 'FeaturedImageCaption', value: '01011-featured-caption' }
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

            expect(result.data.items[0].id).toBe('e81a87022bf4419688b4a2070081a57e', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            // data.filter.where.length + 4 <-- Because field: {options, tags, metadata, accessRules, categories} cannnot be filtered by
            expect(data.filter.where.length + 5).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties count should be equal to filtering parameters count.');
        });
    });
    
    it('01012: Should get a list with one page content, filtered by all available columns', function () {
        var url = '/bcms-api/pages/1b13fcadc8714ba29a20a20700846ab2/contents/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: '457c26ad2d654dc381e8a20700848027' },
                    { field: 'CreatedOn', value: '2013-07-26 08:02:25.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 08:02:25.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '1' },

                    { field: 'ContentId', value: '60b49f2c856649c5819ea20700848027' },
                    { field: 'IsPublished', value: 'true' },
                    { field: 'Name', value: '01012' },
                    { field: 'RegionId', value: 'e3e2e7fe62df4ba683216fdcc1691d8a' },
                    { field: 'RegionIdentifier', value: 'CMSMainContent' },
                    { field: 'Order', value: '0' },
                    { field: 'ParentPageContentId' }
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

            expect(result.data.items[0].id).toBe('457c26ad2d654dc381e8a20700848027', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            // data.filter.where.length + 1 <-- Because field ContentType cannnot be filtered by            
            expect(data.filter.where.length + 1).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties count should be equal to filterting parameters count.');
        });
    });

    it('01013: Should throw validation exception for RegionId/RegionIdentifier, when getting page contents.', function () {
        var url = '/bcms-api/pages/' + api.emptyGuid + '/contents/',
            result,
            ready = false,
            data = {
                regionId: api.emptyGuid,
                regionIdentifier: 'test'
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
            api.expectValidationExceptionIsThrown(result, 'Data.RegionIdentifier');
        });
    });
    
    it('01014: Should throw validation exception for filtering by ContentType, when getting page contents.', function () {
        var url = '/bcms-api/pages/' + api.emptyGuid + '/contents/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'ContentType', value: 'test' }]
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
    
    it('01015: Should get filtered pages list with included option values and with access rules', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '01015' }]
            },
            order: {
                by: [{field: 'Title', direction: 'asc'}]  
            },
            includeUnpublished: true,
            includeArchived: true,
            includePageOptions: true,
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
            expect(result.data.totalCount).toBe(2, 'Total count should be 2.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');

            expect(result.data.items[0].title).toBe('01015 - With Options And Rules', 'Correctly filtered items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('01015 - Without Options', 'Correctly filtered items[1].title should be retrieved.');
            
            expect(result.data.items[1].options.length).toBe(0, 'Correctly filtered items[1].options should be null.');
            expect(result.data.items[0].options).toBeDefinedAndNotNull('Correctly filtered items[0].options should be defined and not null.');
            expect(result.data.items[0].options.length).toBe(3, 'Correctly filtered items[0].options.length should be 3.');
            
            expect(result.data.items[0].options[0].key).toBe('Option 1 With Default Value', 'Correctly filtered items[0].options[0].key should be retrieved.');
            expect(result.data.items[0].options[0].value).toBe('Default Value', 'Correctly filtered items[0].options[0].value should be retrieved.');
            expect(result.data.items[0].options[0].defaultValue).toBe('Default Value', 'Correctly filtered items[0].options[0].defaultValue should be retrieved.');
            expect(result.data.items[0].options[0].type).toBe('Text', 'Correctly filtered items[0].options[0].type should be retrieved.');
            
            expect(result.data.items[0].options[1].key).toBe('Option 2 Without Default Value', 'Correctly filtered items[0].options[1].key should be retrieved.');
            expect(result.data.items[0].options[1].value).toBe('Value 2', 'Correctly filtered items[0].options[1].value should be retrieved.');
            expect(result.data.items[0].options[1].defaultValue).toBeNull('Correctly filtered items[0].options[1].defaultValue should be retrieved.');
            expect(result.data.items[0].options[1].type).toBe('Text', 'Correctly filtered items[0].options[1].type should be retrieved.');
            
            expect(result.data.items[0].options[2].key).toBe('Option 3 with Custom Value', 'Correctly filtered items[0].options[2].key should be retrieved.');
            expect(result.data.items[0].options[2].value).toBe('Custom Value', 'Correctly filtered items[0].options[2].value should be retrieved.');
            expect(result.data.items[0].options[2].defaultValue).toBeNull('Correctly filtered items[0].options[2].defaultValue should be retrieved.');
            expect(result.data.items[0].options[2].type).toBe('Text', 'Correctly filtered items[0].options[2].type should be retrieved.');

            expect(result.data.items[1].accessRules.length).toBe(0, 'Correctly filtered items[0].accessRules.length should be 0.');
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
    
    it('01016: Should throw validation exception for filtering by Options, when getting pages.', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Options', value: 'test' }]
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

    it('01017: Should get list of pages with tags', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '01017:' }]
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

            expect(result.data.items[0].title).toBe('01017:1', 'Correctly filtered items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('01017:2', 'Correctly filtered items[1].title should be retrieved.');
            
            expect(result.data.items[0].tags).toBeDefinedAndNotNull('Correctly filtered items[0].tags should be retrieved.');
            expect(result.data.items[0].tags.length).toBe(2, 'items[0].tags should contain 2 items.');
            expect(result.data.items[0].tags[0]).toBe('01017_1', 'Correctly filtered result.data.items[0].tags[0] should be retrieved.');
            expect(result.data.items[0].tags[1]).toBe('01017_2', 'Correctly filtered result.data.items[0].tags[1] should be retrieved.');
            
            expect(result.data.items[1].tags).toBeDefinedAndNotNull('Correctly filtered items[1].tags should be retrieved.');
            expect(result.data.items[1].tags.length).toBe(3, 'items[1].tags should contain 3 items.');
            expect(result.data.items[1].tags[0]).toBe('01017_1', 'Correctly filtered result.data.items[1].tags[0] should be retrieved.');
            expect(result.data.items[1].tags[1]).toBe('01017_2', 'Correctly filtered result.data.items[1].tags[1] should be retrieved.');
            expect(result.data.items[1].tags[2]).toBe('01017_3', 'Correctly filtered result.data.items[1].tags[2] should be retrieved.');
        });
    });
    
    it('01018: Should throw validation exception for filtering by Tags, when getting pages.', function () {
        var url = '/bcms-api/pages/',
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

    it('01019: Should get a list of pages with included metadata', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '01019' }]
            },
            order: {
                by:[{ field: 'Title' }]
            },
            take: 1,
            includeUnpublished: true,
            includeArchived: true,
            includeMetadata: true
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
            expect(result.data.totalCount).toBe(2, 'Total count should be 2.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');

            expect(result.data.items[0].title).toBe('01019: Page 1', 'Correctly filtered items[0].title should be retrieved.');
            expect(result.data.items[0].metadata).toBeDefinedAndNotNull('Correctly filtered items[0].metadata should be retrieved');
            expect(result.data.items[0].metadata.metaTitle).toBe('01019-meta-title', 'Correctly filtered items[0].metadata.metaTitle should be retrieved');
            expect(result.data.items[0].metadata.metaKeywords).toBe('01019-meta-keywords', 'Correctly filtered items[0].metadata.metaKeywords should be retrieved');
            expect(result.data.items[0].metadata.metaDescription).toBe('01019-meta-description', 'Correctly filtered items[0].metadata.metaDescription should be retrieved');
            expect(result.data.items[0].metadata.useNoFollow).toBe(true, 'Correctly filtered items[0].metadata.useNoFollow should be retrieved');
            expect(result.data.items[0].metadata.useNoIndex).toBe(true, 'Correctly filtered items[0].metadata.useNoIndex should be retrieved');
            expect(result.data.items[0].metadata.useCanonicalUrl).toBe(true, 'Correctly filtered items[0].metadata.useCanonicalUrl should be retrieved');
        });
    });

    it('01020: Should get page translations by page id', function () {
        var url = '/bcms-api/pages/' + constants.testPageId + '/translations',
            result,
            ready = false,
            data = {
                skip: 2,
                take: 2
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
            expect(result.data.items).toBeDefinedAndNotNull('JSON data.items object should be retrieved.');
            expect(result.data.totalCount).toBe(4, 'Total count should be 4.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');
            
            expectPageTranslationsPropertiesAreNotNull(result.data.items, true);
        });
    });

    it('01021: Should get page translations by page url', function () {
        var url = '/bcms-api/pages/translations/by-url' + constants.testPageUrl,
            result,
            ready = false,
            data = {
                skip: 2,
                take: 2
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
            expect(result.data.items).toBeDefinedAndNotNull('JSON data.items object should be retrieved.');
            expect(result.data.totalCount).toBe(4, 'Total count should be 4.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');

            expectPageTranslationsPropertiesAreNotNull(result.data.items, true);
        });
    });

    it('01022: Should get a list with one page translation, filtered by all available columns', function () {
        var url = '/bcms-api/pages/' + constants.testPageId + '/translations',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: constants.testPageId },
                    { field: 'PageUrl', value: constants.testPageUrl },
                    { field: 'Title', value: constants.testPageTitle },
                    { field: 'LanguageId', value: '5fea841ef108430da6eca2a7009366ec' },
                    { field: 'LanguageCode', value: 'ar-KW' },
                    { field: 'IsPublished', value: true },
                    { field: 'PublishedOn', value: '2013-08-12 11:01:00' }
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

            expect(result.data.items[0].id).toBe(constants.testPageId, 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties count should be equal to filtering parameters count.');
        });
    });

    it('01023: Should throw validation exception for PageId/PageCode, when getting page translation.', function () {
        var url = '/bcms-api/pages/' + api.emptyGuid + '/translations/?pageUrl=test',
            result,
            ready = false;

        runs(function () {
            api.get(url, null, null, function (response) {
                result = response.responseJSON;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            api.expectValidationExceptionIsThrown(result, 'PageId');
        });
    });

    it('01024: Should get a list with one page (with master page), filtered by all available columns', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: 'd8f382816bed45d0847ea2f100db58e7' },
                    { field: 'CreatedOn', value: '2014-03-17 13:18:37.000' },
                    { field: 'CreatedBy', value: 'admin' },
                    { field: 'LastModifiedOn', value: '2014-03-17 13:19:30.000' },
                    { field: 'LastModifiedBy', value: 'admin' },
                    { field: 'Version', value: '2' },

                    { field: 'PageUrl', value: '/01024/' },
                    { field: 'Title', value: '01024' },
                    { field: 'Description' },
                    { field: 'IsPublished', value: true },
                    { field: 'PublishedOn', value: '2014-03-17 13:19:30.000' },
                    { field: 'LayoutId' },
                    { field: 'MasterPageId', value: '5d5eb2f6dd16420d81fca2f100db32d4' },

                    { field: 'IsArchived', value: false },
                    { field: 'IsMasterPage', value: false },
                    { field: 'LanguageId' },
                    { field: 'LanguageCode' },
                    { field: 'LanguageGroupIdentifier' },

                    { field: 'MainImageId' },
                    { field: 'MainImageUrl' },
                    { field: 'MainImageThumbnauilUrl' },
                    { field: 'MainImageThumbnailUrl' },
                    { field: 'MainImageCaption' },

                    { field: 'SecondaryImageId' },
                    { field: 'SecondaryImageUrl' },
                    { field: 'SecondaryImageThumbnailUrl' },
                    { field: 'SecondaryImageCaption' },

                    { field: 'FeaturedImageId' },
                    { field: 'FeaturedImageUrl' },
                    { field: 'FeaturedImageThumbnailUrl' },
                    { field: 'FeaturedImageCaption' }
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

            expect(result.data.items[0].id).toBe('d8f382816bed45d0847ea2f100db58e7', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            // data.filter.where.length + 4 <-- Because field: {options, tags, metadata, accessRules, categories} cannnot be filtered by
            expect(data.filter.where.length + 5).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties count should be equal to filtering parameters count.');
        });
    });

    it('01025: Should get filtered page with master page with included option values', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '01025' }]
            },
            order: {
                by: [{ field: 'Title', direction: 'asc' }]
            },
            includeUnpublished: true,
            includeArchived: true,
            includePageOptions: true
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

            expect(result.data.items[0].title).toBe('01025', 'Correctly filtered items[0].title should be retrieved.');
            expect(result.data.items[0].id).toBe('90968e7134494a85bff2a2f100f0e675', 'Correctly filtered items[0].title should be retrieved.');

            expect(result.data.items[0].options).toBeDefinedAndNotNull('Correctly filtered items[0].options should be defined and not null.');
            expect(result.data.items[0].options.length).toBe(3, 'Correctly filtered items[0].options.length should be 3.');

            expect(result.data.items[0].options[0].key).toBe('O1', 'Correctly filtered items[0].options[0].key should be retrieved.');
            expect(result.data.items[0].options[0].value).toBe('Layout', 'Correctly filtered items[0].options[0].value should be retrieved.');
            expect(result.data.items[0].options[0].defaultValue).toBe('Layout', 'Correctly filtered items[0].options[0].defaultValue should be retrieved.');
            expect(result.data.items[0].options[0].type).toBe('Text', 'Correctly filtered items[0].options[0].type should be retrieved.');

            expect(result.data.items[0].options[1].key).toBe('O2', 'Correctly filtered items[0].options[1].key should be retrieved.');
            expect(result.data.items[0].options[1].value).toBe('Master', 'Correctly filtered items[0].options[1].value should be retrieved.');
            expect(result.data.items[0].options[1].defaultValue).toBe('Master', 'Correctly filtered items[0].options[1].defaultValue should be retrieved.');
            expect(result.data.items[0].options[1].type).toBe('Text', 'Correctly filtered items[0].options[1].type should be retrieved.');

            expect(result.data.items[0].options[2].key).toBe('O3', 'Correctly filtered items[0].options[2].key should be retrieved.');
            expect(result.data.items[0].options[2].value).toBe('Page', 'Correctly filtered items[0].options[2].value should be retrieved.');
            expect(result.data.items[0].options[2].defaultValue).toBe('Master', 'Correctly filtered items[0].options[2].defaultValue should be retrieved.');
            expect(result.data.items[0].options[2].type).toBe('Text', 'Correctly filtered items[0].options[2].type should be retrieved.');
        });
    });

    it('01026: Should get filtered page properties with master page with included option values', function () {
        var url = '/bcms-api/page-properties/90968e7134494a85bff2a2f100f0e675/',
            result,
            ready = false;

        var data = {
            includePageOptions: true
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

            expect(result.data.title).toBe('01025', 'Correctly filtered title should be retrieved.');
            expect(result.data.id).toBe('90968e7134494a85bff2a2f100f0e675', 'Correctly filtered title should be retrieved.');

            expect(result.pageOptions).toBeDefinedAndNotNull('Correctly filtered pageOptions should be defined and not null.');
            expect(result.pageOptions.length).toBe(3, 'Correctly filtered pageOptions.length should be 3.');

            expect(result.pageOptions[0].key).toBe('O1', 'Correctly filtered pageOptions[0].key should be retrieved.');
            expect(result.pageOptions[0].value).toBe('Layout', 'Correctly filtered pageOptions[0].value should be retrieved.');
            expect(result.pageOptions[0].defaultValue).toBe('Layout', 'Correctly filtered pageOptions[0].defaultValue should be retrieved.');
            expect(result.pageOptions[0].type).toBe('Text', 'Correctly filtered pageOptions[0].type should be retrieved.');

            expect(result.pageOptions[1].key).toBe('O2', 'Correctly filtered pageOptions[1].key should be retrieved.');
            expect(result.pageOptions[1].value).toBe('Master', 'Correctly filtered pageOptions[1].value should be retrieved.');
            expect(result.pageOptions[1].defaultValue).toBe('Master', 'Correctly filtered pageOptions[1].defaultValue should be retrieved.');
            expect(result.pageOptions[1].type).toBe('Text', 'Correctly filtered pageOptions[1].type should be retrieved.');

            expect(result.pageOptions[2].key).toBe('O3', 'Correctly filtered pageOptions[2].key should be retrieved.');
            expect(result.pageOptions[2].value).toBe('Page', 'Correctly filtered pageOptions[2].value should be retrieved.');
            expect(result.pageOptions[2].defaultValue).toBe('Master', 'Correctly filtered pageOptions[2].defaultValue should be retrieved.');
            expect(result.pageOptions[2].type).toBe('Text', 'Correctly filtered pageOptions[2].type should be retrieved.');
        });
    });

    it('01027.1: Should get list of pages for user admin with roles: [role1, role2]', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
                filter: {
                    where: [{ field: 'Title', operation: 'StartsWith', value: '01027' }]
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

            expect(result.data.items[0].title).toBe('01027: for all', 'Correctly filtered data.items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('01027: only for role1', 'Correctly filtered data.items[1].title should be retrieved.');
            expect(result.data.items[2].title).toBe('01027: only for role2', 'Correctly filtered data.items[2].title should be retrieved.');
        });
    });

    it('01027.2: Should get list of pages for user admin without roles', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
                filter: {
                    where: [{ field: 'Title', operation: 'StartsWith', value: '01027' }]
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

            expect(result.data.items[0].title).toBe('01027: for all', 'Correctly filtered data.items[0].title should be retrieved.');
        });
    });

    it('01027.3: Should get list of pages for user admin2 with [role1]', function () {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
                filter: {
                    where: [{ field: 'Title', operation: 'StartsWith', value: '01027' }]
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

            expect(result.data.items[0].title).toBe('01027: dienied for admin', 'Correctly filtered data.items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('01027: for all', 'Correctly filtered data.items[1].title should be retrieved.');
            expect(result.data.items[2].title).toBe('01027: only for admin2', 'Correctly filtered data.items[2].title should be retrieved.');
            expect(result.data.items[3].title).toBe('01027: only for role1', 'Correctly filtered data.items[3].title should be retrieved.');
        });
    });


    function expectPageListItemPropertiesAreNotNull(page) {
        api.expectBasePropertiesAreNotNull(page);

        expect(page.title).toBe(constants.testPageTitle, 'Correctly filtered title should be retrieved.');
        expect(page.pageUrl).toBe(constants.testPageUrl, 'Correctly filtered pageUrl should be retrieved.');
        expect(page.description).toBe('Test page', 'Correctly filtered description should be retrieved.');
        expect(page.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
        expect(page.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
        expect(page.layoutId).toBeDefinedAndNotNull('layoutId should be retrieved.');
        expect(page.categories.length).toBe(1, 'Categories count should be ' + 1 + '.');        
        expect(page.categories[0].name).toBe('Category for _0000_Page_For_Tests', 'Correctly filtered categoryName should be retrieved.');
        expect(page.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
        expect(page.mainImageThumbnauilUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
        expect(page.mainImageThumbnailUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
        expect(page.mainImageCaption).toBe("Image for _0000_Page_For_Tests", 'Correctly filtered mainImageCaption should be retrieved.');
        expect(page.isArchived).toBe(true, 'Correctly filtered isArchived should be retrieved.');
        expect(page.isMasterPage).toBe(false, 'Correctly filtered isMasterPage should be retrieved.');
        expect(page.languageId).toBe('5fea841ef108430da6eca2a7009366ec', 'Correctly filtered languageId should be retrieved.');
        expect(page.languageCode).toBe('ar-KW', 'Correctly filtered languageCode should be retrieved.');
        expect(page.languageGroupIdentifier).toBe('10e54c92e03643f2b5df656825726ad6', 'Correctly filtered languageGroupIdentifier should be retrieved.');
    }

    function expectPagePropertiesAreNotNull(page) {
        api.expectBasePropertiesAreNotNull(page);

        expect(page.title).toBe(constants.testPageTitle, 'Correctly filtered title should be retrieved.');
        expect(page.pageUrl).toBe(constants.testPageUrl, 'Correctly filtered pageUrl should be retrieved.');
        expect(page.description).toBe('Test page', 'Correctly filtered description should be retrieved.');
        expect(page.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
        expect(page.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
        expect(page.layoutId).toBeDefinedAndNotNull('layoutId should be retrieved.');
        expect(page.categories.length).toBe(1, 'Categories count should be ' + 1 + '.');
        expect(page.categories[0].name).toBe('Category for _0000_Page_For_Tests', 'Correctly filtered categoryName should be retrieved.');
        expect(page.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
        expect(page.mainImageThumbnauilUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
        expect(page.mainImageThumbnailUrl).toBeDefinedAndNotNull('mainImageThumbnailUrl should be retrieved.');
        expect(page.mainImageCaption).toBe("Image for _0000_Page_For_Tests", 'Correctly filtered mainImageCaption should be retrieved.');
        expect(page.isArchived).toBe(true, 'Correctly filtered isArchived should be retrieved.');
        expect(page.isMasterPage).toBe(false, 'Correctly filtered isMasterPage should be retrieved.');
        expect(page.languageId).toBe('5fea841ef108430da6eca2a7009366ec', 'Correctly filtered languageId should be retrieved.');
        expect(page.languageCode).toBe('ar-KW', 'Correctly filtered languageCode should be retrieved.');
        expect(page.languageGroupIdentifier).toBe('10e54c92e03643f2b5df656825726ad6', 'Correctly filtered languageGroupIdentifier should be retrieved.');
    }

    function expectPagePropertiesPropertiesAreNotNull(response) {
        // Page
        var page = response.data;
        api.expectBasePropertiesAreNotNull(page);
        expect(page.title).toBe(constants.testPageTitle, 'Correctly filtered title should be retrieved.');
        expect(page.pageUrl).toBe(constants.testPageUrl, 'Correctly filtered pageUrl should be retrieved.');
        expect(page.description).toBe('Test page', 'Correctly filtered description should be retrieved.');
        expect(page.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
        expect(page.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
        expect(page.layoutId).toBeDefinedAndNotNull('layoutId should be retrieved.');
        expect(page.categories.length).toBe(1, 'Categories count should be ' + 1 + '.');
        expect(page.mainImageId).toBeDefinedAndNotNull('mainImageId should be retrieved.');
        expect(page.featuredImageId).toBeDefinedAndNotNull('featuredImageId should be retrieved.');
        expect(page.secondaryImageId).toBeDefinedAndNotNull('secondaryImageId should be retrieved.');
        expect(page.customCss).toBe('test page custom css', 'Correctly filtered customCss should be retrieved.');
        expect(page.customJavaScript).toBe('console.log("test");', 'Correctly filtered customJavaScript should be retrieved.');
        expect(page.useCanonicalUrl).toBe(true, 'Correctly filtered useCanonicalUrl should be retrieved.');
        expect(page.useNoFollow).toBe(true, 'Correctly filtered useNoFollow should be retrieved.');
        expect(page.useNoIndex).toBe(true, 'Correctly filtered useNoIndex should be retrieved.');
        expect(page.isArchived).toBe(true, 'Correctly filtered isArchived should be retrieved.');
        expect(page.isMasterPage).toBe(false, 'Correctly filtered isMasterPage should be retrieved.');
        expect(page.languageId).toBe('5fea841ef108430da6eca2a7009366ec', 'Correctly filtered languageId should be retrieved.');
        expect(page.languageGroupIdentifier).toBe('10e54c92e03643f2b5df656825726ad6', 'Correctly filtered languageGroupIdentifier should be retrieved.');
        expect(page.forceAccessProtocol).toBe('None', 'Correctly filtered forceAccessProtocol should be retrieved.');

        // layout
        var layout = response.layout;
        expect(layout).toBeDefinedAndNotNull('JSON layout object should be retrieved.');
        api.expectBasePropertiesAreNotNull(layout);
        expect(layout.name).toBe('_0001_Layout3 for _0000_Page_For_Tests', 'Correctly filtered layout.name should be retrieved.');
        expect(layout.layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml', 'Correctly filtered layoutPath should be retrieved.');
        expect(layout.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png', 'Correctly filtered layout.previewUrl should be retrieved.');
        
        // category
        var category = response.categories[0];
        expect(category).toBeDefinedAndNotNull('JSON category object should be retrieved.');
        api.expectBasePropertiesAreNotNull(category);
        expect(category.name).toBe('Category for _0000_Page_For_Tests', 'Correctly filtered category.name should be retrieved.');
        
        // tags
        var tags = response.tags;
        expect(tags).toBeDefinedAndNotNull('JSON tags object should be retrieved.');
        expect(tags.length).toBe(2, 'Returned tags array length should be 2.');
        expect(tags[0].name).toBe('tag1', 'Correctly filtered tags[0].name should be retrieved.');

        api.expectBasePropertiesAreNotNull(tags[1]);
        expect(tags[1].name).toBe('tag2', 'Correctly filtered tags[1].name should be retrieved.');

        // access rules
        var accessRules = response.accessRules;
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

        // images
        expectImagePropertiesAreNotNull(response.mainImage, 'mainImage');
        expectImagePropertiesAreNotNull(response.featuredImage, 'featuredImage');
        expectImagePropertiesAreNotNull(response.secondaryImage, 'secondaryImage');
        
        // meta data
        var metadata = response.metaData;
        expect(metadata).toBeDefinedAndNotNull('JSON metadata object should be retrieved.');
        expect(metadata.metaTitle).toBe('Test meta title', 'Correctly filtered metaTitle should be retrieved.');
        expect(metadata.metaKeywords).toBe('Test meta keywords', 'Correctly filtered metaKeywords should be retrieved.');
        expect(metadata.metaDescription).toBe('Test meta description', 'Correctly filtered metaDescription should be retrieved.');
        
        // language
        var language = response.language;
        expect(language).toBeDefinedAndNotNull('JSON language object should be retrieved.');
        api.expectBasePropertiesAreNotNull(language);
        expect(language.name).toBe('language for 0000-page-for-tests', 'Correctly filtered language.name should be retrieved.');
        expect(language.code).toBe('ar-KW', 'Correctly filtered language.name should be retrieved.');

        // options
        var options = response.pageOptions;
        expect(options).toBeDefinedAndNotNull('JSON pageOptions object should be retrieved.');
        expect(options.length).toBe(2, 'Returned pageOptions array length should be 2.');
        expect(options[0].key).toBe('Option 1', 'Correctly filtered pageOptions[0].key should be retrieved.');
        expect(options[1].key).toBe('Option 2', 'Correctly filtered pageOptions[1].key should be retrieved.');
        expect(options[0].type).toBe('Text', 'Correctly filtered pageOptions[0].type should be retrieved.');
        expect(options[1].type).toBe('Integer', 'Correctly filtered pageOptions[1].type should be retrieved.');
        expect(options[0].value).toBe('Value 1', 'Correctly filtered pageOptions[0].value should be retrieved.');
        expect(options[1].value).toBe('60', 'Correctly filtered pageOptions[1].value should be retrieved.');
        expect(options[0].defaultValue).toBe('Default 1', 'Correctly filtered pageOptions[0].defaultValue should be retrieved.');
        expect(options[1].defaultValue).toBe('50', 'Correctly filtered pageOptions[1].defaultValue should be retrieved.');
        
        // translations
        var translations = response.pageTranslations;
        expect(translations).toBeDefinedAndNotNull('JSON pageTranslations object should be retrieved.');
        expect(translations.length).toBe(4, 'Returned pageTranslations array length should be 2.');
        expect(translations[0].id).toBe('f0464c233b67406babe8a20400b4d8b8', 'Correctly filtered pageTranslations[0].id should be retrieved.');
        expect(translations[1].id).toBe('c8ced8bd4a3643a48759a2a7009b482a', 'Correctly filtered pageTranslations[1].id should be retrieved.');
        expect(translations[2].id).toBe('5ad9f5f2c3fa401a9b31a2a7009b6b1d', 'Correctly filtered pageTranslations[2].id should be retrieved.');
        expect(translations[3].id).toBe('4266673e191c47c38be8a2a7009cd9b9', 'Correctly filtered pageTranslations[3].id should be retrieved.');
        expect(translations[0].title).toBe('_0000_Page_For_Tests', 'Correctly filtered pageTranslations[0].title should be retrieved.');
        expect(translations[1].title).toBe('translation 1 for 0000-page-for-tests', 'Correctly filtered pageTranslations[1].title should be retrieved.');
        expect(translations[2].title).toBe('translation 2 for 0000-page-for-tests', 'Correctly filtered pageTranslations[2].title should be retrieved.');
        expect(translations[3].title).toBe('translation 3 for 0000-page-for-tests', 'Correctly filtered pageTranslations[3].title should be retrieved.');
        expect(translations[0].pageUrl).toBe('/0000-page-for-tests/', 'Correctly filtered pageTranslations[0].pageUrl should be retrieved.');
        expect(translations[1].pageUrl).toBe('/translation-1-for-0000-page-for-tests/', 'Correctly filtered pageTranslations[1].pageUrl should be retrieved.');
        expect(translations[2].pageUrl).toBe('/translation-2-for-0000-page-for-tests/', 'Correctly filtered pageTranslations[2].pageUrl should be retrieved.');
        expect(translations[3].pageUrl).toBe('/translation-3-for-0000-page-for-tests/', 'Correctly filtered pageTranslations[3].pageUrl should be retrieved.');
        expect(translations[0].languageId).toBe('5fea841ef108430da6eca2a7009366ec', 'Correctly filtered pageTranslations[0].languageId should be retrieved.');
        expect(translations[1].languageId).toBe('c8b29d008d224d99be24a2a7009b16dc', 'Correctly filtered pageTranslations[1].languageId should be retrieved.');
        expect(translations[2].languageId).toBe('c7205b78f63243d5a7e0a2a7009b309e', 'Correctly filtered pageTranslations[2].languageId should be retrieved.');
        expect(translations[3].languageId).toBeNull('Correctly filtered pageTranslations[3].languageId should be retrieved.');
        expect(translations[0].languageCode).toBe('ar-KW', 'Correctly filtered pageTranslations[0].languageCode should be retrieved.');
        expect(translations[1].languageCode).toBe('ar-IQ', 'Correctly filtered pageTranslations[1].languageCode should be retrieved.');
        expect(translations[2].languageCode).toBe('ar-JO', 'Correctly filtered pageTranslations[2].languageCode should be retrieved.');
        expect(translations[3].languageCode).toBeNull('Correctly filtered pageTranslations[3].languageCode should be retrieved.');

        // page contents
        var contents = response.pageContents;
        expect(contents).toBeDefinedAndNotNull('JSON contents object should be retrieved.');
        expect(contents.length).toBe(5, 'Returned contents array length should be 5.');

        var contentsFound = 0;
        for (var i = 0; i < contents.length; i ++)
        {
            if (contents[i].name == 'HeaderContent1') {
                expect(contents[i].contentType).toBe('html-content', 'Correctly filtered contentType should be retrieved.');
                expect(contents[i].regionIdentifier).toBe('CMSHeader', 'Correctly filtered regionIdentifier should be retrieved.');

                contentsFound++;
            }
            if (contents[i].name == 'MainContent1 - HTML') {
                var content = contents[i];
                api.expectBasePropertiesAreNotNull(content);
                expect(content.contentType).toBe('html-content', 'Correctly filtered contentType should be retrieved.');
                expect(content.regionIdentifier).toBe('CMSMainContent', 'Correctly filtered regionIdentifier should be retrieved.');
                expect(content.isPublished).toBe(true, 'Correctly filtered content.isPublished should be retrieved.');
                expect(content.regionId).toBeDefinedAndNotNull('regionId should be retrieved.');
                
                contentsFound++;
            }
            if (contents[i].name == 'Html widget for _0000_Page_For_Tests') {
                expect(contents[i].contentType).toBe('html-widget', 'Correctly filtered contentType should be retrieved.');
                expect(contents[i].regionIdentifier).toBe('CMSMainContent', 'Correctly filtered regionIdentifier should be retrieved.');

                contentsFound++;
            }
            if (contents[i].name == 'Server Widget for for _0000_Page_For_Tests') {
                expect(contents[i].contentType).toBe('server-widget', 'Correctly filtered conentType should be retrieved.');
                expect(contents[i].regionIdentifier).toBe('CMSMainContent', 'Correctly filtered regionIdentifier should be retrieved.');

                contentsFound++;
            }
        }

        expect(contentsFound).toBe(4, 'Correct count of content should be retrieved.');
    }
    
    function expectImagePropertiesAreNotNull(image, name) {
        expect(image).toBeDefinedAndNotNull('JSON ' + name + ' object should be retrieved.');
        api.expectBasePropertiesAreNotNull(image);
        
        expect(image.title).toBe('Image for _0000_Page_For_Tests', 'Correctly filtered ' + name + '.title should be retrieved.');
        expect(image.caption).toBe('Image for _0000_Page_For_Tests', 'Correctly filtered ' + name + '.caption should be retrieved.');
        expect(image.url).toBeDefinedAndNotNull('url should be retrieved.');
        expect(image.thumbnailUrl).toBeDefinedAndNotNull('thumbnailUrl should be retrieved.');
    }
    
    function expectPageTranslationsPropertiesAreNotNull(translations, full) {
        expect(translations[0].id).toBe('5ad9f5f2c3fa401a9b31a2a7009b6b1d', 'Correctly filtered pageTranslations[0].id should be retrieved.');
        expect(translations[0].title).toBe('translation 2 for 0000-page-for-tests', 'Correctly filtered pageTranslations[0].title should be retrieved.');
        expect(translations[0].pageUrl).toBe('/translation-2-for-0000-page-for-tests/', 'Correctly filtered pageTranslations[0].pageUrl should be retrieved.');
        expect(translations[0].languageId).toBe('c7205b78f63243d5a7e0a2a7009b309e', 'Correctly filtered pageTranslations[0].languageId should be retrieved.');
        expect(translations[0].languageCode).toBe('ar-JO', 'Correctly filtered pageTranslations[0].languageCode should be retrieved.');
        expect(translations[0].isPublished).toBe(true, 'Correctly filtered pageTranslations[0].isPublished should be retrieved.');
        expect(translations[0].publishedOn).toBe('2014-07-09T09:24:20.0000000', 'Correctly filtered pageTranslations[0].publishedOn should be retrieved.');
        
        if (full) {
            expect(translations[1].id).toBe('4266673e191c47c38be8a2a7009cd9b9', 'Correctly filtered pageTranslations[1].id should be retrieved.');
            expect(translations[1].title).toBe('translation 3 for 0000-page-for-tests', 'Correctly filtered pageTranslations[1].title should be retrieved.');
            expect(translations[1].pageUrl).toBe('/translation-3-for-0000-page-for-tests/', 'Correctly filtered pageTranslations[1].pageUrl should be retrieved.');
            expect(translations[1].languageId).toBeNull('Correctly filtered pageTranslations[1].languageId should be retrieved.');
            expect(translations[1].languageCode).toBeNull('Correctly filtered pageTranslations[1].languageCode should be retrieved.');
            expect(translations[1].isPublished).toBe(false, 'Correctly filtered pageTranslations[1].isPublished should be retrieved.');
            expect(translations[1].publishedOn).toBeNull('Correctly filtered pageTranslations[1].publishedOn should be retrieved.');
        }
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(expectedCount, 'Total count should be ' + expectedCount + '.');
            expect(result.data.items.length).toBe(expectedCount, 'Returned array length should be ' + expectedCount + '.');
            
            for (var i = 0; i < result.data.items.length; i++) {
                expect(result.data.items[i].title).toBe(expectedTitles[i], 'Correctly filtered title should be retrieved.');
            }
        });
    };

    function filterByCategories(connector, expectedCount, expectedTitles, filterBy) {
        var url = '/bcms-api/pages/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: 'IFilterByCategories' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            filterByCategoriesConnector: connector,
            includeUnpublished: true,
            includeArchived: true
        };
        
        if (filterBy === 'id') {
            data.filterByCategories = ['15A86920-78E5-4DDC-A259-A43500A2B573', 'FD36A148-DD10-44E0-A5C1-A43500B8A450'];
        } else if (filterBy === 'name') {
            data.filterByCategoriesNames = ['IFilterByCategories Category 1', 'IFilterByCategories Category 2'];
        }

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
    };
});