/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Media Manager: Files', function () {
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.items).not.toBeNull();
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);

            var file = result.data.items[0];
            api.expectBasePropertiesAreNotNull(file);
            expect(file.title).toBe('_0001_File_For_Tests');
            expect(file.mediaContentType).toBe('File');
            expect(file.fileExtension).toBe('.png');
            expect(file.fileSize).toBe(92217);
            expect(file.fileUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/file/8f46bc6b4fd74c10aba01bf1f7269b92/__Tapir.png');
            expect(file.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/88247a8341184fc2be8c1761c7b86c02/t_1_1.png');
            expect(file.thumbnailId).not.toBeNull();
            expect(file.thumbnailCaption).toBe('Image caption for _0001_File_For_Tests');
            expect(file.isArchived).toBe(false);
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.items).not.toBeNull();
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);

            var folder = result.data.items[0];
            api.expectBasePropertiesAreNotNull(folder);
            expect(folder.title).toBe('_0001_Files_Folder_For_Tests');
            expect(folder.mediaContentType).toBe('Folder');
            expect(folder.fileExtension).toBeNull();
            expect(folder.fileSize).toBeNull();
            expect(folder.fileUrl).toBeNull();
            expect(folder.thumbnailUrl).toBeNull();
            expect(folder.thumbnailId).toBeNull();
            expect(folder.thumbnailCaption).toBeNull();
            expect(folder.isArchived).toBe(false);
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();

            // File
            var file = result.data;
            expect(file).not.toBeNull();
            api.expectBasePropertiesAreNotNull(file);
            expect(file.title).toBe('_0001_File_For_Tests');
            expect(file.description).toBe('File Description');
            expect(file.fileExtension).toBe('.png');
            expect(file.fileSize).toBe(92217);
            expect(file.fileUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/file/8f46bc6b4fd74c10aba01bf1f7269b92/__Tapir.png');
            expect(file.isArchived).toBe(false);
            expect(file.folderId).not.toBeNull();
            expect(file.folderName).toBe('_0001_Files_Folder_For_Tests');
            expect(file.publishedOn).not.toBeNull();
            expect(file.originalFileName).toBe('__Tapir.png');
            expect(file.originalFileExtension).toBe('.png');
            expect(file.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/88247a8341184fc2be8c1761c7b86c02/t_1_1.png');
            expect(file.thumbnailId).not.toBeNull();
            expect(file.thumbnailCaption).toBe('Image caption for _0001_File_For_Tests');
            
            // Tags
            var tags = result.tags;
            expect(tags).not.toBeNull();
            expect(tags.length).toBe(2);

            api.expectBasePropertiesAreNotNull(tags[0]);
            expect(tags[0].name).toBe('tag1_0001_File_For_Tests');
            expect(tags[1].name).toBe('tag2_0001_File_For_Tests');
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);

            expect(result.data.items[0].id).toBe('7f753def7d2647aaa36ca2070078465e');

            // Check if model properties count didn't changed. If so - update filter current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]));
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.items).not.toBeNull();
            expect(result.data.totalCount).toBe(expectingResults.totalCount);
            expect(result.data.items.length).toBe(1);
            expect(result.data.items[0].title).toBe(expectingResults.title);
            expect(result.data.items[0].isArchived).toBe(expectingResults.isArchived);
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.totalCount).toBe(expectedCount);
            expect(result.data.items.length).toBe(expectedCount);

            for (var i = 0; i < result.data.items.length; i++) {
                expect(result.data.items[i].title).toBe(expectedTitles[i]);
            }
        });
    }
});