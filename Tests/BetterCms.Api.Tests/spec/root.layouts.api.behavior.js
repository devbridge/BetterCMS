/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('root.layouts.api.behavior', function () {
    'use strict';

    it('00100: Should get a filtered, sorted and paged layout list', function () {
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(4, 'Total count should be 4.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');

            // Layout 1
            expect(result.data.items[1].name).toBe('_0001_Layout4', 'Correctly filtered items[1].name should be retrieved.');
            
            // Layout 0
            api.expectBasePropertiesAreNotNull(result.data.items[0]);
            expect(result.data.items[0].name).toBe('_0001_Layout3 for _0000_Page_For_Tests', 'Correctly filtered items[0].name should be retrieved.');
            expect(result.data.items[0].layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml', 'Correctly filtered layoutPath should be retrieved.');
            expect(result.data.items[0].previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png', 'Correctly filtered previewUrl should be retrieved.');
        });
    });

    it('00101: Should get a layout by id', function () {
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');

            // Layout
            api.expectBasePropertiesAreNotNull(result.data);
            expect(result.data.name).toBe('_0001_Layout3 for _0000_Page_For_Tests', 'Correctly filtered name should be retrieved.');
            expect(result.data.layoutPath).toBe('~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml', 'Correctly filtered layoutPath should be retrieved.');
            expect(result.data.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png', 'Correctly filtered previewUrl should be retrieved.');
        });
    });
    
    it('00102: Should get layout regions by layout id', function () {
        var url = '/bcms-api/layouts/d2f39fbd2c28401a8625a1fe0114e1eb/regions',
            result,
            ready = false;

        var data = {
            order: {
                by: [
                    { field: 'RegionIdentifier', direction: 'desc' }
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
            expect(result.data.items).not.toBeNull('JSON data.items object should be retrieved.');
            expect(result.data.items.length).toBe(3, 'Returned array length should be 3.');

            api.expectBasePropertiesAreNotNull(result.data.items[0]);
            expect(result.data.items[0].regionIdentifier).toBe('CMSMainContent', 'Correctly filtered items[0].regionIdentifier should be retrieved.');
            expect(result.data.items[1].regionIdentifier).toBe('CMSHeader', 'Correctly filtered items[1].regionIdentifier should be retrieved.');
            expect(result.data.items[2].regionIdentifier).toBe('CMSFooter', 'Correctly filtered items[2].regionIdentifier should be retrieved.');
        });
    });
    
    it('00103: Should get a list with one layout, filtered by all available columns', function () {
        var url = '/bcms-api/layouts/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: '33d04bd9e37f40ecaad5a20700adbe11' },
                    { field: 'CreatedOn', value: '2013-07-26 10:32:34.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 10:32:34.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '1' },

                    { field: 'Name', value: '00103' },
                    { field: 'LayoutPath', value: '~/Areas/bcms-installation/Views/Shared/DefaultLayout.cshtml' },
                    { field: 'PreviewUrl', value: 'http://www.devbridge.com/Content/styles/images/responsive/logo.png' }
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

            expect(result.data.items[0].id).toBe('33d04bd9e37f40ecaad5a20700adbe11', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
    
    it('00104: Should get a list with one layout region, filtered by all available columns', function () {
        var url = '/bcms-api/layouts/1030da610baa40ccb484a20700b21ec2/regions/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: 'c0830f40833043c3b108a20700b21ec2' },
                    { field: 'CreatedOn', value: '2013-07-26 10:48:30.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 10:48:30.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '1' },

                    { field: 'RegionIdentifier', value: '00104' },
                    { field: 'Description', value: 'Region description' }
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

            expect(result.data.items[0].id).toBe('c0830f40833043c3b108a20700b21ec2', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
    
    it('00105: Should get layout options by layout id', function () {
        var url = '/bcms-api/layouts/34789cf9d3e942e3b866a218009e756c/options',
            result,
            ready = false;

        var data = {
            order: {
                by: [
                    { field: 'Key', direction: 'desc' }
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
            expect(result.data.items).not.toBeNull('JSON data.items object should be retrieved.');
            expect(result.data.items.length).toBe(3, 'Returned array length should be 3.');

            expect(result.data.items[0].key).toBe('Option 3', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].key).toBe('Option 2', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].key).toBe('Option 1', 'Correctly filtered items[2].key should be retrieved.');

            expect(result.data.items[0].defaultValue).toBe('A', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].defaultValue).toBe('18', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].defaultValue).toBe('12.5', 'Correctly filtered items[2].key should be retrieved.');

            expect(result.data.items[0].type).toBe('Text', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].type).toBe('Integer', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].type).toBe('Float', 'Correctly filtered items[2].key should be retrieved.');
        });
    });
    
    it('00106: Should get a list with one layout option, filtered by all available columns', function () {
        var url = '/bcms-api/layouts/3a7995d408c542cbbf5fa21800a0f066/options/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Key', value: 'Option 3' },
                    { field: 'DefaultValue', value: '18' },
                    { field: 'Type', value: 'Integer' },
                    { field: 'CustomTypeIdentifier', value: null }
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

            expect(result.data.items[0].key).toBe('Option 3', 'Correctly filtered key should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });

    it('00107: Should test CRUD for layouts.', function () {
        api.testCrud(runs, waitsFor, expect, "d2f39fbd2c28401a8625a1fe0114e1eb", "/bcms-api/layouts/", {
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });
});