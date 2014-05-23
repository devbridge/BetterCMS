/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('pages.redirects.api.behavior', function () {
    'use strict';

    it('01300: Should get a list of redirects', function () {
        var url = '/bcms-api/redirects/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'PageUrl', operation: 'StartsWith', value: '/_0000_' }]
            },
            order: {
                by: [{ field: 'PageUrl' }]
            },
            take: 2,
            skip: 2,
            includeUnpublished: true
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

            api.expectBasePropertiesAreNotNull(result.data.items[0]);
            api.expectBasePropertiesAreNotNull(result.data.items[1]);
            expect(result.data.items[0].pageUrl).toBe('/_0000_Redirect_From_3/', 'Correctly filtered items[0].pageUrl should be retrieved.');
            expect(result.data.items[0].redirectUrl).toBe('/_0000_Redirect_To_3/', 'Correctly filtered items[0].redirectUrl should be retrieved.');
            expect(result.data.items[1].pageUrl).toBe('/_0000_Redirect_From_4/', 'Correctly filtered items[1].pageUrl should be retrieved.');
            expect(result.data.items[1].redirectUrl).toBe('/_0000_Redirect_To_4/', 'Correctly filtered items[1].redirectUrl should be retrieved.');
        });
    });
    
    it('01301: Should get a redirect by id', function () {
        var url = '/bcms-api/redirects/72EC32B9-D5A4-4642-9D7A-A205009FE9B6',
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

            var redirect = result.data;
            expect(redirect).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(redirect);
            expect(redirect.pageUrl).toBe('/_0000_Redirect_From_3/', 'Correctly filtered pageUrl should be retrieved.');
            expect(redirect.redirectUrl).toBe('/_0000_Redirect_To_3/', 'Correctly filtered redirectUrl should be retrieved.');
        });
    });

    it('01302: Should get a list with one redirect, filtered by all available columns', function () {
        var url = '/bcms-api/redirects/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: '23574260f1984c9e98aba207008d08fe' },
                    { field: 'CreatedOn', value: '2013-07-26 08:33:29.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 08:36:24.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '2' },
                    
                    { field: 'PageUrl', value: '/01302-from/' },
                    { field: 'RedirectUrl', value: '/01302-to/' }
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

            expect(result.data.items[0].id).toBe('23574260f1984c9e98aba207008d08fe', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });

    it('01303: Should test CRUD for redirects.', function () {
        api.testCrud(runs, waitsFor, expect, "72EC32B9-D5A4-4642-9D7A-A205009FE9B6", "/bcms-api/redirects/", {
            getPostData: function (json) {
                json.data.pageUrl = "/" + api.createGuid() + "/";
                json.data.version = 0;
                return json.data;
            }
        });
    });
});