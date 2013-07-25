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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.items).toBeDefined();
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
            expect(file.thumbnailId).toBeDefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.items).toBeDefined();
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);

            var folder = result.data.items[0];
            api.expectBasePropertiesAreNotNull(folder);
            expect(folder.title).toBe('_0001_Files_Folder_For_Tests');
            expect(folder.mediaContentType).toBe('Folder');
            expect(folder.fileExtension).toBeUndefined();
            expect(folder.fileSize).toBeUndefined();
            expect(folder.fileUrl).toBeUndefined();
            expect(folder.thumbnailUrl).toBeUndefined();
            expect(folder.thumbnailId).toBeUndefined();
            expect(folder.thumbnailCaption).toBeUndefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();

            // File
            var file = result.data;
            expect(file).toBeDefined();
            api.expectBasePropertiesAreNotNull(file);
            expect(file.title).toBe('_0001_File_For_Tests');
            expect(file.description).toBe('File Description');
            expect(file.fileExtension).toBe('.png');
            expect(file.fileSize).toBe(92217);
            expect(file.fileUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/file/8f46bc6b4fd74c10aba01bf1f7269b92/__Tapir.png');
            expect(file.isArchived).toBe(false);
            expect(file.folderId).toBeDefined();
            expect(file.folderName).toBe('_0001_Files_Folder_For_Tests');
            expect(file.publishedOn).toBeDefined();
            expect(file.originalFileName).toBe('__Tapir.png');
            expect(file.originalFileExtension).toBe('.png');
            expect(file.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/88247a8341184fc2be8c1761c7b86c02/t_1_1.png');
            expect(file.thumbnailId).toBeDefined();
            expect(file.thumbnailCaption).toBe('Image caption for _0001_File_For_Tests');
            
            // Tags
            var tags = result.tags;
            expect(tags).toBeDefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.items).toBeDefined();
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.totalCount).toBe(expectedCount);
            expect(result.data.items.length).toBe(expectedCount);

            for (var i = 0; i < result.data.items.length; i++) {
                expect(result.data.items[i].title).toBe(expectedTitles[i]);
            }
        });
    }
});