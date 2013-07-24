/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('Root: Tags', function() {
    'use strict';

    it('0000: Should get tags list', function () {
        var url = '/bcms-api/tags/',
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

    it('0001: Should get a tag by id', function () {
        var url = '/bcms-api/tags/7f6da39e75ae4718ad4ca2040113d40b',
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