/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('users.roles.api.behavior', function () {
    'use strict';

    it('04100: Should get roles list', function () {
        var url = '/bcms-api/roles/',
            result,
            ready = false;
        
        var data = {
                filter: {
                    where: [
                        { field: 'Name', operation: 'StartsWith', value: '04100' }
                    ]
                },
                order: {
                    by: [
                        { field: 'Name' }
                    ]
                },
                skip: 1,
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
            expect(result.data.totalCount).toBe(3, 'Total count should be 3.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');

            api.expectBasePropertiesAreNotNull(result.data.items[0]);
            expect(result.data.items[0].name).toBe('04100_2', 'Correctly filtered items[0].name should be retrieved.');
            expect(result.data.items[0].isSystematic).toBe(false, 'Correctly filtered items[0].isSystematic should be retrieved.');
            
            api.expectBasePropertiesAreNotNull(result.data.items[1]);
            expect(result.data.items[1].name).toBe('04100_3', 'Correctly filtered items[1].name should be retrieved.');
            expect(result.data.items[1].isSystematic).toBe(true, 'Correctly filtered items[1].isSystematic should be retrieved.');
        });
    });

    it('04101: Should get a role by id', function () {
        var url = '/bcms-api/roles/5e82afa37695479397f3a222011157ba',
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
            expect(result.data.id).toBe('5e82afa37695479397f3a222011157ba', 'Correctly filtered id should be retrieved.');
            expect(result.data.name).toBe('04101', 'Correctly filtered name should be retrieved.');
            expect(result.data.isSystematic).toBe(true, 'Correctly filtered isSystematic should be retrieved.');
        });
    });

    it('04102: Should get a list with one role, filtered by all available columns', function () {
        var url = '/bcms-api/roles/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [
                        { field: 'Id', value: 'dfcf842f56dd46afa16ea22201115f76' },
                        { field: 'CreatedOn', value: '2013-08-22 16:35:19.000' },
                        { field: 'CreatedBy', value: 'Better CMS test user' },
                        { field: 'LastModifiedOn', value: '2013-08-22 16:35:19.000' },
                        { field: 'LastModifiedBy', value: 'Better CMS test user' },
                        { field: 'Version', value: '1' },
                        
                        { field: 'Name', value: '04102' },
                        { field: 'IsSystematic', value: 'true' },
                        { field: 'Description', value: null }
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

            expect(result.data.items[0].id).toBe('dfcf842f56dd46afa16ea22201115f76', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
    
    it('04103: Should throw validation exception for RoleName/RoleId, when getting role.', function () {
        var url = '/bcms-api/roles/' + api.emptyGuid + '/?roleName=test',
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
            api.expectValidationExceptionIsThrown(result, 'RoleId');
        });
    });
    
    it('04104: Should get a role by name', function () {
        var url = '/bcms-api/roles/by-name/04104',
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
            expect(result.data.id).toBe('5fb47003153f4e258b70a22201116a31', 'Correctly filtered id should be retrieved.');
            expect(result.data.name).toBe('04104', 'Correctly filtered name should be retrieved.');
            expect(result.data.isSystematic).toBe(true, 'Correctly filtered isSystematic should be retrieved.');
        });
    });

    it('04105: Should test CRUD for roles.', function () {
        api.testCrud(runs, waitsFor, expect, "5e82afa37695479397f3a222011157ba", "/bcms-api/roles/", {
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });
});