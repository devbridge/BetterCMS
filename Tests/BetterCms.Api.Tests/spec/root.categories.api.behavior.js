/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('Root: Categories', function() {
    'use strict';

    it('00300: Should get categories list', function () {
        var url = '/bcms-api/categories/',
            result,
            ready = false;
        
        var data = {
                filter: {
                    where: [
                        { field: 'Name', operation: 'StartsWith', value: '_0001_' }
                    ]
                },
                order: {
                    by: [
                        { field: 'Name' }
                    ]
                },
                skip: 2,
                take: 3
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
            expect(result.data.totalCount).toBe(5, 'Total count should be 5.');
            expect(result.data.items.length).toBe(3, 'Returned array length should be 3.');

            expect(result.data.items[0].name).toBe('_0001_ - 3');
            expect(result.data.items[1].name).toBe('_0001_ - 4');
            expect(result.data.items[2].name).toBe('_0001_ - 5');
        });
    });

    it('00301: Should get a category by id', function () {
        var url = '/bcms-api/categories/e87bfb18cdf74fd3a5dfa2040115ed1d',
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

            api.expectBasePropertiesAreNotNull(result.data);
            expect(result.data.name).toBe('_0001_ - 3');
        });
    });
    
    it('00302: Should get a list with one category, filtered by all available columns', function () {
        var url = '/bcms-api/categories/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [
                        { field: 'Id', value: 'A60E08C1-9150-4DBE-BD32-A20601185796' },
                        { field: 'CreatedOn', value: '2013-07-25 17:00:41.000' },
                        { field: 'CreatedBy', value: 'Better CMS test user' },
                        { field: 'LastModifiedOn', value: '2013-07-25 17:00:41.000' },
                        { field: 'LastModifiedBy', value: 'Better CMS test user' },
                        { field: 'Version', value: '1' },
                        { field: 'Name', value: '00302' }
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

            expect(result.data.items[0].id).toBe('a60e08c191504dbebd32a20601185796');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]));
        });
    });
});