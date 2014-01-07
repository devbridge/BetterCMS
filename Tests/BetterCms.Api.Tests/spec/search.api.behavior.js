/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('search.api.behavior', function () {
    'use strict';
    
//    it('09100: Should search within pages', function () {
//        var url = '/bcms-api/pages/search/09100',
//            result,
//            ready = false;
//
//        runs(function () {
//            api.get(url, null, function (json) {
//                result = json;
//                ready = true;
//            });
//        });
//
//        waitsFor(function () {
//            return ready;
//        }, 'The ' + url + ' timeout.');
//
//        runs(function () {
//            var found = 0,
//                item, i;
//
//            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
//            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
//            expect(result.data.totalCount).toBe(2, 'Total count should be 2.');
//            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');
//
//            
//            for (i = 0; i < result.data.items.length; i++) {
//                item = result.data.items[i];
//                
//                if (item.title == '09100 - 1') {
//                    found++;
//                    
//                    expect(item.snippet).toBe(' 09100 - 1 ', 'Correctly filtered items[0].snippet should be retrieved.');
//                    expect(item.link).toBe('/09100-1/', 'Correctly filtered item[0].link should be retrieved.');
//                    expect(item.formattedUrl).toBe('/09100-1/', 'Correctly filtered item[0].formattedUrl should be retrieved.');
//                }
//                if (item.title == '09100 - 2') {
//                    found++;
//                    
//                    expect(item.snippet).toBe(' 09100 - 2 ', 'Correctly filtered items[1].snippet should be retrieved.');
//                    expect(item.link).toBe('/09100-2/', 'Correctly filtered item[1].link should be retrieved.');
//                    expect(item.formattedUrl).toBe('/09100-2/', 'Correctly filtered item[1].formattedUrl should be retrieved.');
//                }
//            }
//            
//            expect(found).toBe(2, 'Correctly filtered item count should be retrieved.');
//        });
//    });

});