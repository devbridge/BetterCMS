/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('root.languages.api.behavior', function () {
    'use strict';

    it('00400: Should get languages list', function () {
        var url = '/bcms-api/languages/',
            result,
            ready = false;
        
        var data = {
                filter: {
                    where: [
                        { field: 'Name', operation: 'StartsWith', value: '00400' }
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
            expect(result.data.totalCount).toBe(6, 'Total count should be 5.');
            expect(result.data.items.length).toBe(3, 'Returned array length should be 3.');

            expect(result.data.items[0].name).toBe('00400 - 3', 'Correctly filtered items[0].name should be retrieved.');
            expect(result.data.items[0].code).toBe('am', 'Correctly filtered items[0].code should be retrieved.');
            expect(result.data.items[1].name).toBe('00400 - 4', 'Correctly filtered items[1].name should be retrieved.');
            expect(result.data.items[1].code).toBe('am-ET', 'Correctly filtered items[1].code should be retrieved.');
            expect(result.data.items[2].name).toBe('00400 - 5', 'Correctly filtered items[2].name should be retrieved.');
            expect(result.data.items[2].code).toBe('ar', 'Correctly filtered items[2].code should be retrieved.');
        });
    });

    it('00401: Should get a language by id', function () {
        var url = '/bcms-api/languages/03424e8f32cc4c89b1fba2a500ad595f',
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
            expect(result.data.name).toBe('00401', 'Correctly filtered name should be retrieved.');
            expect(result.data.code).toBe('ar-BH', 'Correctly filtered code should be retrieved.');
        });
    });
    
    it('00402: Should get a list with one language, filtered by all available columns', function () {
        var url = '/bcms-api/languages/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [
                        { field: 'Id', value: '9a90677b953d47278b09a2a500ae05f9' },
                        { field: 'CreatedOn', value: '2013-12-31 10:33:35.000' },
                        { field: 'CreatedBy', value: 'admin' },
                        { field: 'LastModifiedOn', value: '2013-12-31 10:33:35.000' },
                        { field: 'LastModifiedBy', value: 'admin' },
                        { field: 'Version', value: '1' },
                        { field: 'Name', value: '00402' },
                        { field: 'Code', value: 'ar-DZ' }
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

            expect(result.data.items[0].id).toBe('9a90677b953d47278b09a2a500ae05f9', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
    
    it('00403: Should throw validation exception for LanguageCode/LanguageId, when getting language.', function () {
        var url = '/bcms-api/languages/' + api.emptyGuid + '/?languageCode=test',
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
            api.expectValidationExceptionIsThrown(result, 'LanguageId');
        });
    });
    
    it('00404: Should get a language by code', function () {
        var url = '/bcms-api/languages/by-code/ar-EG',
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
            expect(result.data.name).toBe('00404', 'Correctly filtered name should be retrieved.');
            expect(result.data.code).toBe('ar-EG', 'Correctly filtered code should be retrieved.');
        });
    });

    it('00405: Should test CRUD for languages.', function () {
        api.testCrud(runs, waitsFor, expect, "03424e8f32cc4c89b1fba2a500ad595f", "/bcms-api/languages/", {
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.code = 'lt-LT';
                json.data.version = 0;
                return json.data;
            }
        });
    });
});