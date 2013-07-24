/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Media Manager: Images', function () {
    'use strict';

//    it('0000: Should get a full media tree', function() {
//    });

    //it('0000: Should get a list of redirects', function () {
    //    var url = '/bcms-api/redirects/',
    //        result,
    //        ready = false;

    //    var data = {
    //        filter: {
    //            where: [{ field: 'PageUrl', operation: 'StartsWith', value: '/_0000_' }]
    //        },
    //        order: {
    //            by: [{ field: 'PageUrl' }]
    //        },
    //        take: 2,
    //        skip: 2,
    //        includeUnpublished: true
    //    };

    //    runs(function () {
    //        api.get(url, data, function (json) {
    //            result = json;
    //            ready = true;
    //        });
    //    });

    //    waitsFor(function () {
    //        return ready;
    //    }, 'The ' + url + ' timeout.');

    //    runs(function () {
    //        expect(result).toBeDefined();
    //        expect(result.data).toBeDefined();
    //        expect(result.data.totalCount).toBe(4);
    //        expect(result.data.items.length).toBe(2);

    //        api.expectBasePropertiesAreNotNull(result.data.items[0]);
    //        api.expectBasePropertiesAreNotNull(result.data.items[1]);
    //        expect(result.data.items[0].pageUrl).toBe('/_0000_Redirect_From_3/');
    //        expect(result.data.items[0].redirectUrl).toBe('/_0000_Redirect_To_3/');
    //        expect(result.data.items[1].pageUrl).toBe('/_0000_Redirect_From_4/');
    //        expect(result.data.items[1].redirectUrl).toBe('/_0000_Redirect_To_4/');
    //    });
    //});

    //it('0001: Should get a redirect by id', function () {
    //    var url = '/bcms-api/redirects/72EC32B9-D5A4-4642-9D7A-A205009FE9B6',
    //        result,
    //        ready = false;

    //    runs(function () {
    //        api.get(url, null, function (json) {
    //            result = json;
    //            ready = true;
    //        });
    //    });

    //    waitsFor(function () {
    //        return ready;
    //    }, 'The ' + url + ' timeout.');

    //    runs(function () {
    //        expect(result).toBeDefined();

    //        var redirect = result.data;
    //        expect(redirect).toBeDefined();
    //        api.expectBasePropertiesAreNotNull(redirect);
    //        expect(redirect.pageUrl).toBe('/_0000_Redirect_From_3/');
    //        expect(redirect.redirectUrl).toBe('/_0000_Redirect_To_3/');
    //    });
    //});
});