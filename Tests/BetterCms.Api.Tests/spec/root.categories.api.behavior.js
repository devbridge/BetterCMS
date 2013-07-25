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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.totalCount).toBe(5);
            expect(result.data.items.length).toBe(3);

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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();

            api.expectBasePropertiesAreNotNull(result.data);
            expect(result.data.name).toBe('_0001_ - 3');
        });
    });
});