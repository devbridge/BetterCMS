/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('Root: Layouts', function() {
    'use strict';

    it('0000: Should get a filtered, sorted and paged layout list', function () {
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

            // Layout 1
            expect(result.data.items[1].name).toBe('_0001_Layout4');
            
            // Layout 0
            api.expectBasePropertiesAreNotNull(result.data.items[0]);
            expect(result.data.items[0].name).toBe('_0001_Layout3 for _0000_Page_For_Tests');
            expect(result.data.items[0].layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml');
            expect(result.data.items[0].previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png');
        });
    });

    it('0001: Should get a layout by id', function () {
        var url = '/bcms-api/layouts/d2f39fbd2c28401a8625a1fe0114e1eb',
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

            // Layout
            api.expectBasePropertiesAreNotNull(result.data);
            expect(result.data.name).toBe('_0001_Layout3 for _0000_Page_For_Tests');
            expect(result.data.layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml');
            expect(result.data.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png');
        });
    });
    
    it('0003: Should get layout regions by layout id', function () {
        var url = '/bcms-api/layouts/d2f39fbd2c28401a8625a1fe0114e1eb/regions',
            result,
            ready = false;

        var data = {
            order: {
                by: [
                    { field: 'RegionIdentifier', direction: 'desc' }
                ]
            },
            // TODO: remove Hack after tests and solution:
            layoutId: 'd2f39fbd2c28401a8625a1fe0114e1eb'
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
            expect(result.data.items).toBeDefined();
            expect(result.data.items.length).toBe(3);

            api.expectBasePropertiesAreNotNull(result.data.items[0]);
            expect(result.data.items[0].regionIdentifier).toBe('CMSMainContent');
            expect(result.data.items[1].regionIdentifier).toBe('CMSHeader');
            expect(result.data.items[2].regionIdentifier).toBe('CMSFooter');
        });
    });
});