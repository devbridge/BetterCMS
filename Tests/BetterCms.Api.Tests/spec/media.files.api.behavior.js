/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('media.files.api.behavior', function () {
    'use strict';
    
    it('03100: Should get a list of file folders', function () {
        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: true,
            includeFiles: false,
            includeArchived: false,
            includeAccessRules: true,
            take: 1
        },
            results =
            {
                totalCount: 2,
                title: '_0000_Files_Folder_1',
                isArchived: false,
                accessRules: false
            };

        runFilesListTests(data, results);
    });

    it('03101: Should get a list of files without folders', function () {
        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: false,
            includeFiles: true,
            includeArchived: false,
            includeAccessRules: true,
            take: 1
        },
            results =
            {
                totalCount: 2,
                title: '_0000_File_1',
                isArchived: false,
                accessRules: true
            };

        runFilesListTests(data, results);
    });

    it('03102: Should get a list of not archived files and folders', function () {
        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: true,
            includeFiles: true,
            includeArchived: false,
            includeAccessRules: true,
            take: 1
        },
            results =
            {
                totalCount: 4,
                title: '_0000_File_1',
                isArchived: false,
                accessRules: true
            };

        runFilesListTests(data, results);
    });

    it('03103: Should get a list of files and folders (including archived)', function () {
        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: true,
            includeFiles: true,
            includeArchived: true,
            includeAccessRules: true,
            take: 1
        },
            results =
            {
                totalCount: 6,
                title: '_0000_File_1',
                isArchived: false,
                accessRules: true
            };

        runFilesListTests(data, results);
    });

    it('03104: Should get a list of files and folders (only archived)', function () {
        var data = {
            filter: {
                where: [
                    { field: 'Title', operation: 'StartsWith', value: '_0000_' },
                    { field: 'IsArchived', value: 'true' }
                ]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: true,
            includeFiles: true,
            includeArchived: true,
            includeAccessRules: true,
            take: 1
        },
            results =
            {
                totalCount: 2,
                title: '_0000_File_3_Archived',
                isArchived: true,
                accessRules: false
            };

        runFilesListTests(data, results);
    });

    it('03105: Should get a list from subfolder with specified file', function () {
        var url = '/bcms-api/files/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Title', value: '_0001_File_For_Tests' }]
                },
                folderId: '1531bbc30fc7471da0daa2060080e15b'
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
            expect(result.data.totalCount).toBe(1, 'Total count should be 1.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');

            var file = result.data.items[0];
            api.expectBasePropertiesAreNotNull(file);
            expect(file.title).toBe('_0001_File_For_Tests', 'Correctly filtered title should be retrieved.');
            expect(file.description).toBe('File Description', 'Correctly filtered description should be retrieved.');
            expect(file.mediaContentType).toBe('File', 'Correctly filtered mediaContentType should be retrieved.');
            expect(file.fileExtension).toBe('.png', 'Correctly filtered fileExtension should be retrieved.');
            expect(file.fileSize).toBe(92217, 'Correctly filtered fileSize should be retrieved.');
            expect(file.fileUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/file/8f46bc6b4fd74c10aba01bf1f7269b92/__Tapir.png', 'Correctly filtered fileUrl should be retrieved.');
            expect(file.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/88247a8341184fc2be8c1761c7b86c02/t_1_1.png', 'Correctly filtered thumbnailUrl should be retrieved.');
            expect(file.thumbnailId).toBeDefinedAndNotNull('thumbnailId should be retrieved.');
            expect(file.thumbnailCaption).toBe('Image caption for _0001_File_For_Tests', 'Correctly filtered thumbnailCaption should be retrieved.');
            expect(file.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
        });
    });

    it('03106: Should get a list with specified folder', function () {
        var url = '/bcms-api/files/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Title', value: '_0001_Files_Folder_For_Tests' }]
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
            expect(result.data.totalCount).toBe(1, 'Total count should be 1.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');

            var folder = result.data.items[0];
            api.expectBasePropertiesAreNotNull(folder);
            expect(folder.title).toBe('_0001_Files_Folder_For_Tests', 'Correctly filtered title should be retrieved.');
            expect(folder.mediaContentType).toBe('Folder', 'Correctly filtered mediaContentType should be retrieved.');
            expect(folder.fileExtension).toBeNull('fileExtension should be null.');
            expect(folder.fileSize).toBeNull('fileSize should be null.');
            expect(folder.fileUrl).toBeNull('fileUrl should be null.');;
            expect(folder.thumbnailUrl).toBeNull('thumbnailUrl should be null.');;
            expect(folder.thumbnailId).toBeNull('thumbnailId should be null.');;
            expect(folder.thumbnailCaption).toBeNull('thumbnailCaption should be null.');;
            expect(folder.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
        });
    });

    it('03107: Should get file properties by file id', function () {
        var url = '/bcms-api/files/4ef65c378a9f4a2f90b5a20600816711',
            result,
            ready = false,
            data = {
                includeTags: true,
                includeAccessRules: true
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

            // File
            var file = result.data;
            expect(file).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(file);
            expect(file.title).toBe('_0001_File_For_Tests', 'Correctly filtered title should be retrieved.');
            expect(file.description).toBe('File Description', 'Correctly filtered description should be retrieved.');
            expect(file.fileExtension).toBe('.png', 'Correctly filtered fileExtension should be retrieved.');
            expect(file.fileSize).toBe(92217, 'Correctly filtered fileSize should be retrieved.');
            expect(file.fileUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/file/8f46bc6b4fd74c10aba01bf1f7269b92/__Tapir.png', 'Correctly filtered fileUrl should be retrieved.');
            expect(file.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
            expect(file.folderId).toBeDefinedAndNotNull('folderId should be retrieved.', 'Correctly filtered folderId should be retrieved.');
            expect(file.folderName).toBe('_0001_Files_Folder_For_Tests', 'Correctly filtered folderName should be retrieved.');
            expect(file.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.', 'Correctly filtered publishedOn should be retrieved.');
            expect(file.originalFileName).toBe('__Tapir.png', 'Correctly filtered originalFileName should be retrieved.');
            expect(file.originalFileExtension).toBe('.png', 'Correctly filtered fileExtension should be retrieved.');
            expect(file.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/88247a8341184fc2be8c1761c7b86c02/t_1_1.png', 'Correctly filtered thumbnailUrl should be retrieved.');
            expect(file.thumbnailId).toBeDefinedAndNotNull('thumbnailId should be retrieved.');
            expect(file.thumbnailCaption).toBe('Image caption for _0001_File_For_Tests', 'Correctly filtered thumbnailCaption should be retrieved.');

            expect(file.fileUri).toBe('file:///D:/Projects/BCMS/Code/MAIN/Tests/BetterCms.Api.Tests/uploads/file/8f46bc6b4fd74c10aba01bf1f7269b92/__Tapir.png', 'Correctly filtered fileUri should be retrieved.');
            expect(file.isUploaded).toBe(true, 'Correctly filtered isUploaded should be retrieved.');
            expect(file.isTemporary).toBe(false, 'Correctly filtered isTemporary should be retrieved.');
            expect(file.isCanceled).toBe(false, 'Correctly filtered IsCanceled should be retrieved.');
            
            // Tags
            var tags = result.tags;
            expect(tags).toBeDefinedAndNotNull('JSON tags object should be retrieved.');
            expect(tags.length).toBe(2, 'Returned array length should be 2.');

            api.expectBasePropertiesAreNotNull(tags[0]);
            expect(tags[0].name).toBe('tag1_0001_File_For_Tests', 'Correctly filtered tags[0].name should be retrieved.');
            expect(tags[1].name).toBe('tag2_0001_File_For_Tests', 'Correctly filtered tags[1].name should be retrieved.');

            // Access rules
            var rules = result.accessRules;
            expect(rules).toBeDefinedAndNotNull('JSON AccessRules object should be retrieved.');
            expect(rules.length).toBe(6, 'Returned array length should be 6.');

            var rule1 = rules[0],
                rule2 = rules[1],
                rule3 = rules[2],
                rule4 = rules[3],
                rule5 = rules[4],
                rule6 = rules[5];

            expect(rule1.isForRole).toBe(false, 'Correctly filtered accessRules[0].isForRole should be false.');
            expect(rule2.isForRole).toBe(false, 'Correctly filtered accessRules[1].isForRole should be false.');
            expect(rule3.isForRole).toBe(false, 'Correctly filtered accessRules[2].isForRole should be false.');
            expect(rule4.isForRole).toBe(true, 'Correctly filtered accessRules[3].isForRole should be true.');
            expect(rule5.isForRole).toBe(true, 'Correctly filtered accessRules[4].isForRole should be true.');
            expect(rule6.isForRole).toBe(true, 'Correctly filtered accessRules[5].isForRole should be true.');

            expect(rule1.accessLevel).toBe('ReadWrite', 'Correctly filtered accessRules[0].accessLevel should be ReadWrite.');
            expect(rule2.accessLevel).toBe('Deny', 'Correctly filtered accessRules[1].accessLevel should be Deny.');
            expect(rule3.accessLevel).toBe('Read', 'Correctly filtered accessRules[2].accessLevel should be Read.');
            expect(rule4.accessLevel).toBe('ReadWrite', 'Correctly filtered accessRules[3].accessLevel should be ReadWrite.');
            expect(rule5.accessLevel).toBe('Read', 'Correctly filtered accessRules[4].accessLevel should be Read.');
            expect(rule6.accessLevel).toBe('Deny', 'Correctly filtered accessRules[5].accessLevel should be Deny.');

            expect(rule1.identity).toBe('user1', 'Correctly filtered accessRules[0].identity should be user1.');
            expect(rule2.identity).toBe('user2', 'Correctly filtered accessRules[1].identity should be user2.');
            expect(rule3.identity).toBe('user3', 'Correctly filtered accessRules[2].identity should be user3.');
            expect(rule4.identity).toBe('Authenticated Users', 'Correctly filtered accessRules[3].identity should be Authenticated Users.');
            expect(rule5.identity).toBe('Everyone', 'Correctly filtered accessRules[4].identity should be Everyone.');
            expect(rule6.identity).toBe('role1', 'Correctly filtered accessRules[5].identity should be role1.');
        });
    });

    it('03108: Should get files list, filtered by tags, using AND connector', function () {
        filterByTags('and', 1, ['IFilterByTags File 1']);
    });

    it('03109: Should get files list, filtered by tags, using OR connector', function () {
        filterByTags('or', 2, ['IFilterByTags File 1', 'IFilterByTags File 3']);
    });

    it('03109.1: Should get files list, filtered by categories, using AND connector', function () {
        filterByCategories('and', 1, ['IFilterByCategories File 2'], 'id');
    });

    it('03109.2: Should get files list, filtered by categories, using OR connector', function () {
        filterByCategories('or', 2, ['IFilterByCategories File 1', 'IFilterByCategories File 2'], 'id');
    });

    it('03109.3: Should get files list, filtered by categories names, using AND connector', function () {
        filterByCategories('and', 1, ['IFilterByCategories File 2'], 'name');
    });

    it('03109.4: Should get files list, filtered by categories names, using OR connector', function () {
        filterByCategories('or', 2, ['IFilterByCategories File 1', 'IFilterByCategories File 2'], 'name');
    });

    it('03110: Should get a list with one file, filtered by all available columns', function () {
        var url = '/bcms-api/files/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: '7f753def7d2647aaa36ca2070078465e' },
                    { field: 'CreatedOn', value: '2013-07-26 07:17:54.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 07:27:44.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '6' },
                    
                    { field: 'Title', value: '03110' },
                    { field: 'Description', value: 'description' },
                    { field: 'MediaContentType', value: 'File' },
                    { field: 'FileExtension', value: '.jpg' },
                    { field: 'FileSize', value: 9901 },
                    { field: 'FileUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/file/737256d2822d4c16b246d59fbfaa7f9b/1.jpg' },
                    { field: 'ThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/ea8be69ccb97474eae513454f5dec93e/t_1_1.png' },
                    { field: 'ThumbnailCaption', value: '03110 caption' },
                    { field: 'IsArchived', value: false },
                    { field: 'ThumbnailId', value: '8a653450def8433c8298a20700786dad' }
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

            expect(result.data.items[0].id).toBe('7f753def7d2647aaa36ca2070078465e', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            // data.filter.where.length + 1 <-- Because field: {accessRules, Categories} cannnot be filtered by
            expect(data.filter.where.length + 2).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
    
    it('03111: Should throw validation exception for IncludeFolders/IncludeFiles, when getting files.', function () {
        var url = '/bcms-api/files/',
            result,
            ready = false,
            data = {
                includeFiles: false,
                includeFolders: false
            };

        runs(function () {
            api.get(url, data, null, function (response) {
                result = response.responseJSON;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            api.expectValidationExceptionIsThrown(result, 'Data.IncludeFolders');
        });
    });

    it('03112.1: Should get list of files for user admin with roles: [role1, role2]', function () {
        var url = '/bcms-api/files/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Title', value: '03112', operation: 'StartsWith' }]
                },
                folderId: '22c03f8220b446edbb15a2fc00fe2f2f'
            },
            user = {
                name: 'admin',
                roles: ['role1', 'role2']
            };

        runs(function () {
            api.getSecured(url, data, user, function (json) {
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
            expect(result.data.totalCount).toBe(3, 'Total count should be 3.');
            expect(result.data.items.length).toBe(3, 'Returned array length should be 3.');

            expect(result.data.items[0].title).toBe('03112: for all', 'Correctly filtered data.items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('03112: only for role role1', 'Correctly filtered data.items[1].title should be retrieved.');
            expect(result.data.items[2].title).toBe('03112: only for role role2', 'Correctly filtered data.items[2].title should be retrieved.');
        });
    });

    it('03112.2: Should get list of pages for user admin without roles', function () {
        var url = '/bcms-api/files/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Title', value: '03112', operation: 'StartsWith' }]
                },
                folderId: '22c03f8220b446edbb15a2fc00fe2f2f'
            },
            user = {
                name: 'admin'
            };

        runs(function () {
            api.getSecured(url, data, user, function (json) {
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
            expect(result.data.totalCount).toBe(1, 'Total count should be 1.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');

            expect(result.data.items[0].title).toBe('03112: for all', 'Correctly filtered data.items[0].title should be retrieved.');
        });
    });

    it('03112.3: Should get list of pages for user admin2 with [role1]', function () {
        var url = '/bcms-api/files/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Title', value: '03112', operation: 'StartsWith' }]
                },
                folderId: '22c03f8220b446edbb15a2fc00fe2f2f'
            },
            user = {
                name: 'admin2',
                roles: ['role1']
            };

        runs(function () {
            api.getSecured(url, data, user, function (json) {
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
            expect(result.data.totalCount).toBe(4, 'Total count should be 4.');
            expect(result.data.items.length).toBe(4, 'Returned array length should be 4.');

            expect(result.data.items[0].title).toBe('03112: denied for admin', 'Correctly filtered data.items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('03112: for all', 'Correctly filtered data.items[1].title should be retrieved.');
            expect(result.data.items[2].title).toBe('03112: only for admin 2', 'Correctly filtered data.items[2].title should be retrieved.');
            expect(result.data.items[3].title).toBe('03112: only for role role1', 'Correctly filtered data.items[3].title should be retrieved.');
        });
    });

    it('03113: Should test CRUD for files.', function () {
        api.testCrud(runs, waitsFor, expect, "4ef65c378a9f4a2f90b5a20600816711", "/bcms-api/files/", {
            getPostData: function (json) {
                json.data.title = api.createGuid();
                json.data.publicUrl = "/" + api.createGuid();
                json.data.version = 0;
                return json.data;
            },
            getPutData: function (json) {
                json.data.title = api.createGuid();
                json.data.publicUrl = "/" + api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });

    function runFilesListTests(data, expectingResults) {
        var url = '/bcms-api/files/',
            result,
            ready = false;

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
            expect(result.data.totalCount).toBe(expectingResults.totalCount, 'Total count should be ' + expectingResults.totalCount + '.');
            expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');
            expect(result.data.items[0].title).toBe(expectingResults.title, 'Correctly filtered title should be retrieved.');
            expect(result.data.items[0].isArchived).toBe(expectingResults.isArchived, 'Correctly filtered isArchived should be retrieved.');

            if (expectingResults.accessRules) {
                expect(result.data.items[0].accessRules).toBeDefinedAndNotNull('JSON object accessRules should be retrieved.');
                expect(result.data.items[0].accessRules.length).toBe(6, 'Returned accessRules array length should be 6.');

                var rules = result.data.items[0].accessRules,
                    rule1 = rules[0],
                    rule2 = rules[1],
                    rule3 = rules[2],
                    rule4 = rules[3],
                    rule5 = rules[4],
                    rule6 = rules[5];

                expect(rule1.isForRole).toBe(false, 'Correctly filtered accessRules[0].isForRole should be false.');
                expect(rule2.isForRole).toBe(false, 'Correctly filtered accessRules[1].isForRole should be false.');
                expect(rule3.isForRole).toBe(false, 'Correctly filtered accessRules[2].isForRole should be false.');
                expect(rule4.isForRole).toBe(true, 'Correctly filtered accessRules[3].isForRole should be true.');
                expect(rule5.isForRole).toBe(true, 'Correctly filtered accessRules[4].isForRole should be true.');
                expect(rule6.isForRole).toBe(true, 'Correctly filtered accessRules[5].isForRole should be true.');

                expect(rule1.accessLevel).toBe('ReadWrite', 'Correctly filtered accessRules[0].accessLevel should be ReadWrite.');
                expect(rule2.accessLevel).toBe('Deny', 'Correctly filtered accessRules[1].accessLevel should be Deny.');
                expect(rule3.accessLevel).toBe('Read', 'Correctly filtered accessRules[2].accessLevel should be Read.');
                expect(rule4.accessLevel).toBe('ReadWrite', 'Correctly filtered accessRules[3].accessLevel should be ReadWrite.');
                expect(rule5.accessLevel).toBe('Read', 'Correctly filtered accessRules[4].accessLevel should be Read.');
                expect(rule6.accessLevel).toBe('Deny', 'Correctly filtered accessRules[5].accessLevel should be Deny.');

                expect(rule1.identity).toBe('user1', 'Correctly filtered accessRules[0].identity should be user1.');
                expect(rule2.identity).toBe('user2', 'Correctly filtered accessRules[1].identity should be user2.');
                expect(rule3.identity).toBe('user3', 'Correctly filtered accessRules[2].identity should be user3.');
                expect(rule4.identity).toBe('Authenticated Users', 'Correctly filtered accessRules[3].identity should be Authenticated Users.');
                expect(rule5.identity).toBe('Everyone', 'Correctly filtered accessRules[4].identity should be Everyone.');
                expect(rule6.identity).toBe('role1', 'Correctly filtered accessRules[5].identity should be role1.');
            }
        });
    }
    
    function filterByTags(connector, expectedCount, expectedTitles) {
        var url = '/bcms-api/files/',
            result,
            ready = false;

        var data = {
            order: {
                by: [{ field: 'Title' }]
            },
            folderId: 'cf304a17091244c69740a206008a9e7e',
            filterByTagsConnector: connector,
            filterByTags: ['IFilterByTags Tag 1', 'IFilterByTags Tag 2'],
            includeArchived: true
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
            expect(result.data.totalCount).toBe(expectedCount, 'Total count should be ' + expectedCount + '.');
            expect(result.data.items.length).toBe(expectedCount, 'Returned array length should be ' + expectedCount + '.');

            for (var i = 0; i < result.data.items.length; i++) {
                expect(result.data.items[i].title).toBe(expectedTitles[i]);
            }
        });
    }

    function filterByCategories(connector, expectedCount, expectedTitles, filterBy) {
        var url = '/bcms-api/files/',
            result,
            ready = false;

        var data = {
            order: {
                by: [{ field: 'Title' }]
            },
            folderId: 'DEE811B5-5F39-4846-862B-A43500F478BE',
            filterByCategoriesConnector: connector,
            includeArchived: true
        };

        if (filterBy === 'id') {
            data.filterByCategories = ['15A86920-78E5-4DDC-A259-A43500A2B573', 'FD36A148-DD10-44E0-A5C1-A43500B8A450'];
        } else if(filterBy === 'name') {
            data.filterByCategoriesNames = ['IFilterByCategories Category 1', 'IFilterByCategories Category 2'];
        }

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
            expect(result.data.totalCount).toBe(expectedCount, 'Total count should be ' + expectedCount + '.');
            expect(result.data.items.length).toBe(expectedCount, 'Returned array length should be ' + expectedCount + '.');

            for (var i = 0; i < result.data.items.length; i++) {
                expect(result.data.items[i].title).toBe(expectedTitles[i]);
            }
        });
    }
});