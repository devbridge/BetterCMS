/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('users.users.api.behavior', function () {
    'use strict';

    it('04000: Should get users list', function () {
        var url = '/bcms-api/users/',
            result,
            ready = false;
        
        var data = {
                filter: {
                    where: [
                        { field: 'UserName', operation: 'StartsWith', value: '04000' }
                    ]
                },
                order: {
                    by: [
                        { field: 'UserName' }
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
            expect(result.data.items[0].userName).toBe('04000_2', 'Correctly filtered items[0].userName should be retrieved.');
            expect(result.data.items[0].firstName).toBe('04000_2 First Name', 'Correctly filtered items[0].firstName should be retrieved.');
            expect(result.data.items[0].lastName).toBe('04000_2 Last Name', 'Correctly filtered items[0].lastName should be retrieved.');
            expect(result.data.items[0].email).toBe('04000_2@email.com', 'Correctly filtered items[0].email should be retrieved.');
            expect(result.data.items[0].imageId).toBeNull('Correctly filtered items[0].imageId should be retrieved.');
            expect(result.data.items[0].imageUrl).toBeNull('Correctly filtered items[0].imageUrl should be retrieved.');
            expect(result.data.items[0].imageThumbnailUrl).toBeNull('Correctly filtered items[0].imageThumbnauilUrl should be retrieved.');
            expect(result.data.items[0].imageCaption).toBeNull('Correctly filtered items[0].imageCaption should be retrieved.');
            
            api.expectBasePropertiesAreNotNull(result.data.items[1]);
            expect(result.data.items[1].userName).toBe('04000_3', 'Correctly filtered items[1].userName should be retrieved.');
            expect(result.data.items[1].firstName).toBe('04000_3 First Name', 'Correctly filtered items[1].firstName should be retrieved.');
            expect(result.data.items[1].lastName).toBe('04000_3 Last Name', 'Correctly filtered items[1].lastName should be retrieved.');
            expect(result.data.items[1].email).toBe('04000_3@email.com', 'Correctly filtered items[1].email should be retrieved.');
            expect(result.data.items[1].imageId).toBe('650ad17e5aaa4809a9d2a2220103d3c7', 'Correctly filtered items[1].imageId should be retrieved.');
            expect(result.data.items[1].imageUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/c5fab583d8f94773ae86aba0ee81d6ac/1_1.jpg', 'Correctly filtered items[1].imageUrl should be retrieved.');
            expect(result.data.items[1].imageThumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/c5fab583d8f94773ae86aba0ee81d6ac/t_1_1.png', 'Correctly filtered items[1].imageThumbnailUrl should be retrieved.');
            expect(result.data.items[1].imageCaption).toBe('04000_3 Caption', 'Correctly filtered items[1].imageCaption should be retrieved.');
        });
    });

    it('04001: Should get a user by id', function () {
        var url = '/bcms-api/users/f7076866be7847269a5fa22201072aaf',
            result,
            ready = false,
            data = {
                includeRoles: true
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

            api.expectBasePropertiesAreNotNull(result.data);
            expect(result.data.id).toBe('f7076866be7847269a5fa22201072aaf', 'Correctly filtered id should be retrieved.');
            expect(result.data.userName).toBe('04001', 'Correctly filtered userName should be retrieved.');
            expect(result.data.firstName).toBe('04001 First Name', 'Correctly filtered firstName should be retrieved.');
            expect(result.data.lastName).toBe('04001 Last Name', 'Correctly filtered lastName should be retrieved.');
            expect(result.data.email).toBe('04001@email.com', 'Correctly filtered email should be retrieved.');
            expect(result.data.imageId).toBe('c71f7d4ea17044f085d5a22201071ac7', 'Correctly filtered imageId should be retrieved.');
            expect(result.data.imageUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/a98cc774306c42e5b8550dc77491ec26/1_1.jpg', 'Correctly filtered imageUrl should be retrieved.');
            expect(result.data.imageThumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/a98cc774306c42e5b8550dc77491ec26/t_1_1.png', 'Correctly filtered imageThumbnailUrl should be retrieved.');
            expect(result.data.imageCaption).toBe('04001 Caption', 'Correctly filtered imageCaption should be retrieved.');

            expect(result.roles).toBeDefinedAndNotNull('Correctly filtered roles should be retrieved.');
            expect(result.roles.length).toBe(2, 'Correctly filtered roles should be retrieved.');
            
            api.expectBasePropertiesAreNotNull(result.roles[0]);
            expect(result.roles[0].name).toBe('04001 - 1', 'Correctly filtered roles[0].name should be retrieved.');
            expect(result.roles[1].name).toBe('04001 - 2', 'Correctly filtered roles[1].name should be retrieved.');
            expect(result.roles[0].description).toBe('04001 - 1 Description', 'Correctly filtered roles[0].description should be retrieved.');
            expect(result.roles[1].description).toBe('04001 - 2 Description', 'Correctly filtered roles[1].description should be retrieved.');
            expect(result.roles[0].isSystematic).toBe(false, 'Correctly filtered roles[0].isSystematic should be retrieved.');
            expect(result.roles[1].isSystematic).toBe(true, 'Correctly filtered roles[1].isSystematic should be retrieved.');
        });
    });

    it('04002: Should get a list with one user, filtered by all available columns', function () {
        var url = '/bcms-api/users/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [
                        { field: 'Id', value: '01a0918d2c604b208ecca2220105e582' },
                        { field: 'CreatedOn', value: '2013-08-22 15:53:32.000' },
                        { field: 'CreatedBy', value: 'Better CMS test user' },
                        { field: 'LastModifiedOn', value: '2013-08-22 15:53:32.000' },
                        { field: 'LastModifiedBy', value: 'Better CMS test user' },
                        { field: 'Version', value: '1' },
                        
                        { field: 'UserName', value: '04002' },
                        { field: 'FirstName', value: '04002 First Name' },
                        { field: 'LastName', value: '04002 Last Name' },
                        { field: 'Email', value: '04002@email.com' },
                        { field: 'ImageId', value: 'f93ba968a3d842838ea9a222010584a8' },
                        { field: 'ImageUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/cb06965eda0b4ad2ba0f7b22000b0b80/1_1.jpg' },
                        { field: 'ImageThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/cb06965eda0b4ad2ba0f7b22000b0b80/t_1_1.png' },
                        { field: 'ImageCaption', value: '04002 Caption' }
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

            expect(result.data.items[0].id).toBe('01a0918d2c604b208ecca2220105e582', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
    
    it('04003: Should throw validation exception for UserName/UserId, when getting user.', function () {
        var url = '/bcms-api/users/' + api.emptyGuid + '/?userName=test',
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
            api.expectValidationExceptionIsThrown(result, 'UserId');
        });
    });
    
    it('04004: Should get a user by username', function () {
        var url = '/bcms-api/users/by-username/04004',
            result,
            ready = false,
            data = {
                includeRoles: true
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

            api.expectBasePropertiesAreNotNull(result.data);
            expect(result.data.id).toBe('48ca6f7c7cd44aa5bf8ea222010efd98', 'Correctly filtered id should be retrieved.');
            expect(result.data.userName).toBe('04004', 'Correctly filtered userName should be retrieved.');
            expect(result.data.firstName).toBe('04004 First Name', 'Correctly filtered firstName should be retrieved.');
            expect(result.data.lastName).toBe('04004 Last Name', 'Correctly filtered lastName should be retrieved.');
            expect(result.data.email).toBe('04004@email.com', 'Correctly filtered email should be retrieved.');
            expect(result.data.imageId).toBe('cd40891f9db7456798eca222010eed10', 'Correctly filtered imageId should be retrieved.');
            expect(result.data.imageUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/aecea7d38583450eb8bca3f15b2abf6e/1_1.jpg', 'Correctly filtered imageUrl should be retrieved.');
            expect(result.data.imageThumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/aecea7d38583450eb8bca3f15b2abf6e/t_1_1.png', 'Correctly filtered imageThumbnailUrl should be retrieved.');
            expect(result.data.imageCaption).toBe('04004 Caption', 'Correctly filtered imageCaption should be retrieved.');

            expect(result.roles).toBeDefinedAndNotNull('Correctly filtered roles should be retrieved.');
            expect(result.roles.length).toBe(2, 'Correctly filtered roles should be retrieved.');

            api.expectBasePropertiesAreNotNull(result.roles[0]);
            expect(result.roles[0].name).toBe('04004 - 1', 'Correctly filtered roles[0].name should be retrieved.');
            expect(result.roles[1].name).toBe('04004 - 2', 'Correctly filtered roles[1].name should be retrieved.');
            expect(result.roles[0].description).toBe('04004 - 1 Description', 'Correctly filtered roles[0].description should be retrieved.');
            expect(result.roles[1].description).toBe('04004 - 2 Description', 'Correctly filtered roles[1].description should be retrieved.');
            expect(result.roles[0].isSystematic).toBe(false, 'Correctly filtered roles[0].isSystematic should be retrieved.');
            expect(result.roles[1].isSystematic).toBe(true, 'Correctly filtered roles[1].isSystematic should be retrieved.');
        });
    });
    
    it('04005: Should get users list, filtered by roles using AND connector', function () {
        var url = '/bcms-api/users/',
            result,
            ready = false;

        var data = {
            filterByRolesConnector: 'and',
            filterByRoles: ['04005_1', '04005_2'],
            order: {
                by: [
                    { field: 'UserName' }
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
            expect(result.data.totalCount).toBe(2, 'Total count should be 3.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');

            expect(result.data.items[0].userName).toBe('04005_1', 'Correctly filtered items[0].userName should be retrieved.');
            expect(result.data.items[1].userName).toBe('04005_2', 'Correctly filtered items[1].userName should be retrieved.');
        });
    });
    
    it('04006: Should get users list, filtered by roles using OR connector', function () {
        var url = '/bcms-api/users/',
            result,
            ready = false;

        var data = {
            filterByRolesConnector: 'or',
            filterByRoles: ['04006_1', '04006_2'],
            order: {
                by: [
                    { field: 'UserName' }
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

            expect(result.data.items[0].userName).toBe('04006_2', 'Correctly filtered items[0].userName should be retrieved.');
            expect(result.data.items[1].userName).toBe('04006_3', 'Correctly filtered items[1].userName should be retrieved.');
        });
    });
    
    it('04007: Should validate username  and password', function () {
        var url = '/bcms-api/users/validate/',
            result,
            ready = false;

        var data = {
            userName: '04007',
            password: '04007'
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
            expect(result.data.valid).toBeTruthy('JSON data should be boolean true.');
            expect(result.data.userId).toBe('d0e5aa68dad146429db1a22e0101eb42', 'Correct user id should be retrieved.');
        });
    });

    it('04008: Should invalidate user name and password', function () {
        var url = '/bcms-api/users/validate/',
            result,
            ready = false;

        var data = {
            userName: '04008',
            password: '04008_wrong_password'
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
            expect(result.data.valid).toBeFalsy('JSON data should be boolean false.');
        });
    });

    it('04009: Should test CRUD for users.', function () {
        api.testCrud(runs, waitsFor, expect, "f7076866be7847269a5fa22201072aaf", "/bcms-api/users/", {
            getPostData: function (json) {
                json.data.userName = api.createGuid();
                json.data.password = api.createGuid();
                json.data.email = api.createGuid() + '@' + api.createGuid() + '.com';
                json.data.version = 0;
                return json.data;
            }
        });
    });
});