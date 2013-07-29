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
            take: 1
        },
            results =
            {
                totalCount: 2,
                title: '_0000_Files_Folder_1',
                isArchived: false
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
            take: 1
        },
            results =
            {
                totalCount: 2,
                title: '_0000_File_1',
                isArchived: false
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
            take: 1
        },
            results =
            {
                totalCount: 4,
                title: '_0000_File_1',
                isArchived: false
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
            take: 1
        },
            results =
            {
                totalCount: 6,
                title: '_0000_File_1',
                isArchived: false
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
            take: 1
        },
            results =
            {
                totalCount: 2,
                title: '_0000_File_3_Archived',
                isArchived: true
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
            expect(file.mediaContentType).toBe('File', 'Correctly filtered mediaContentType should be retrieved.');
            expect(file.fileExtension).toBe('.png', 'Correctly filtered fileExtension should be retrieved.');
            expect(file.fileSize).toBe(92217, 'Correctly filtered fileSize should be retrieved.');
            expect(file.fileUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/file/8f46bc6b4fd74c10aba01bf1f7269b92/__Tapir.png', 'Correctly filtered fileUrl should be retrieved.');
            expect(file.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/88247a8341184fc2be8c1761c7b86c02/t_1_1.png', 'Correctly filtered thumbnailUrl should be retrieved.');
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
                includeTags: true
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
            expect(file.fileUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/file/8f46bc6b4fd74c10aba01bf1f7269b92/__Tapir.png', 'Correctly filtered fileUrl should be retrieved.');
            expect(file.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
            expect(file.folderId).toBeDefinedAndNotNull('folderId should be retrieved.', 'Correctly filtered folderId should be retrieved.');
            expect(file.folderName).toBe('_0001_Files_Folder_For_Tests', 'Correctly filtered folderName should be retrieved.');
            expect(file.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.', 'Correctly filtered publishedOn should be retrieved.');
            expect(file.originalFileName).toBe('__Tapir.png', 'Correctly filtered originalFileName should be retrieved.');
            expect(file.originalFileExtension).toBe('.png', 'Correctly filtered fileExtension should be retrieved.');
            expect(file.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/88247a8341184fc2be8c1761c7b86c02/t_1_1.png', 'Correctly filtered thumbnailUrl should be retrieved.');
            expect(file.thumbnailId).toBeDefinedAndNotNull('thumbnailId should be retrieved.');
            expect(file.thumbnailCaption).toBe('Image caption for _0001_File_For_Tests', 'Correctly filtered thumbnailCaption should be retrieved.');
            
            // Tags
            var tags = result.tags;
            expect(tags).toBeDefinedAndNotNull('JSON tags object should be retrieved.');
            expect(tags.length).toBe(2, 'Returned array length should be 2.');

            api.expectBasePropertiesAreNotNull(tags[0]);
            expect(tags[0].name).toBe('tag1_0001_File_For_Tests', 'Correctly filtered tags[0].name should be retrieved.');
            expect(tags[1].name).toBe('tag2_0001_File_For_Tests', 'Correctly filtered tags[1].name should be retrieved.');
        });
    });

    it('03108: Should get files list, filtered by tags, using AND connector', function () {
        filterByTags('and', 1, ['IFilterByTags File 1']);
    });

    it('03109: Should get files list, filtered by tags, using OR connector', function () {
        filterByTags('or', 2, ['IFilterByTags File 1', 'IFilterByTags File 3']);
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
                    { field: 'MediaContentType', value: 'File' },
                    { field: 'FileExtension', value: '.jpg' },
                    { field: 'FileSize', value: 9901 },
                    { field: 'FileUrl', value: 'http://bettercms.sandbox.mvc4.local/uploads/file/737256d2822d4c16b246d59fbfaa7f9b/1.jpg' },
                    { field: 'ThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local/uploads/image/ea8be69ccb97474eae513454f5dec93e/t_1_1.png' },
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
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
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
});