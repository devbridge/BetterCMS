/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('root.tags.api.behavior', function () {
    'use strict';

    it('00200: Should get tags list', function () {
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.totalCount).toBe(5, 'Total count should be 5.');
            expect(result.data.items.length).toBe(3, 'Returned array length should be 3.');

            expect(result.data.items[0].name).toBe('_0001_ - 3', 'Correctly filtered items[0].name should be retrieved.');
            expect(result.data.items[1].name).toBe('_0001_ - 4', 'Correctly filtered items[1].name should be retrieved.');
            expect(result.data.items[2].name).toBe('_0001_ - 5', 'Correctly filtered items[2].name should be retrieved.');
        });
    });

    it('00201: Should get a tag by id', function () {
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');

            api.expectBasePropertiesAreNotNull(result.data);
            expect(result.data.name).toBe('_0001_ - 3', 'Correctly filtered name should be retrieved.');
        });
    });
    
    it('00202: Should get a list with one tag, filtered by all available columns', function () {
        var url = '/bcms-api/tags/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [
                        { field: 'Id', value: 'b0dc1aa59fe54d4f9ad9a2060116b0f5' },
                        { field: 'CreatedOn', value: '2013-07-25 16:54:41.000' },
                        { field: 'CreatedBy', value: 'Better CMS test user' },
                        { field: 'LastModifiedOn', value: '2013-07-25 16:54:41.000' },
                        { field: 'LastModifiedBy', value: 'Better CMS test user' },
                        { field: 'Version', value: '1' },
                        { field: 'Name', value: '00202' }
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

            expect(result.data.items[0].id).toBe('b0dc1aa59fe54d4f9ad9a2060116b0f5', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
    
    it('00203: Should throw validation exception for TagName/TagId, when getting tag.', function () {
        var url = '/bcms-api/tags/' + api.emptyGuid + '/?tagName=test',
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
            api.expectValidationExceptionIsThrown(result, 'TagId');
        });
    });

    it('00204: Should test CRUD for tags.', function () {
        api.testCrud(runs, waitsFor, expect, "b0dc1aa59fe54d4f9ad9a2060116b0f5", "/bcms-api/tags/", {
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });
});