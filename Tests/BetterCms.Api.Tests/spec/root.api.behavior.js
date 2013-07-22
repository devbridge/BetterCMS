/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('Root', function() {
    'use strict';

    it('0000: Should get a Better CMS current version', function () {
        var url = '/bcms-api/current-version/',
            result,
            ready = false;

        runs(function () {
            get(url, null, function(json) {
                result = json;
                ready = true;
            });
        });
        
        waitsFor(function() {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.version).toBe("1.0.0-dev");
        });               
    });

    it('0001: Should get a filtered, sorted and paged layout list', function () {
        var url = '/bcms-api/layouts/',
            result,
            ready = false;
        
        var data = {
                filter: {
                    connector: 'and',
                    where: [
                        { field: 'Name', operation: 'NotEqual', value: 'NOT_FOUND' },
                        { field: 'Name', operation: 'StartsWith', value: '_0001_' }
                    ],
                    inner: [
                        {
                            connector: 'or',
                            where: [
                                { field: 'Name', operation: 'Contains', value: 'Layout1' },
                                { field: 'Name', operation: 'NotContains', value: 'NOT_FOUND' }
                            ]
                        }
                    ]
                },
                order: {
                    by: [
                        { field: 'Name' },
                        { field: 'LastModifiedOn', Direction: 'desc' }
                    ]
                },
                skip: 2,
                take: 2
            };

        runs(function () {
            get(url, data, function (json) {
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
            expect(result.data.items[0].name).toBe('_0001_Layout3');
            expect(result.data.items[1].name).toBe('_0001_Layout4');
        });
    });

    it('Root_0002: Should get a layout by id', function () {

    });
});