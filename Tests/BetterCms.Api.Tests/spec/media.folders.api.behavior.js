/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('media.folders.api.behavior', function () {
    'use strict';

    it('03300: Should get a list of folders', function () {
        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: true,
            includeImages: false,
            includeArchived: false,
            take: 1
        },
            results =
            {
                totalCount: 4,
                title: '_0000_Files_Folder_1',
                isArchived: false
            };

        runFoldersListTests(data, results);
    });
    
    it('03301: Should get folder by id', function () {
        var url = '/bcms-api/images/5606d5be1b6347d88621a2050129ed3f',
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

            // Image
            var image = result.data;
            expect(image).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(image);
            expect(image.title).toBe('_0001_Image_For_Tests', 'Correctly filtered title should be retrieved.');
            expect(image.caption).toBe('Image Caption', 'Correctly filtered caption should be retrieved.');
            expect(image.description).toBe('Image Description', 'Correctly filtered description should be retrieved.');
            expect(image.fileExtension).toBe('.png', 'Correctly filtered fileExtension should be retrieved.');
            expect(image.fileSize).toBe(26354, 'Correctly filtered fileSize should be retrieved.');
            expect(image.imageUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/15f824b13a9e428fa013dc1940741295/__Tapir_9.png', 'Correctly filtered imageUrl should be retrieved.');
            expect(image.width).toBe(480, 'Correctly filtered width should be retrieved.');
            expect(image.height).toBe(100, 'Correctly filtered height should be retrieved.');
            expect(image.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/15f824b13a9e428fa013dc1940741295/t___Tapir_9.png', 'Correctly filtered thumbnailUrl should be retrieved.');
            expect(image.thumbnailWidth).toBe(150, 'Correctly filtered thumbnailWidth should be retrieved.');
            expect(image.thumbnailHeight).toBe(150, 'Correctly filtered thumbnailHeight should be retrieved.');
            expect(image.thumbnailSize).toBe(15590, 'Correctly filtered thumbnailSize should be retrieved.');
            expect(image.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
            expect(image.folderId).toBeDefinedAndNotNull('folderId should be retrieved.', 'Correctly filtered folderId should be retrieved.');
            expect(image.folderName).toBe('_0001_Images_Folder_For_Tests', 'Correctly filtered folderName should be retrieved.');
            expect(image.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(image.originalFileName).toBe('__Tapir.png', 'Correctly filtered originalFileName should be retrieved.');
            expect(image.originalFileExtension).toBe('.png', 'Correctly filtered originalFileExtension should be retrieved.');
            expect(image.originalWidth).toBe(480, 'Correctly filtered originalWidth should be retrieved.');
            expect(image.originalHeight).toBe(480, 'Correctly filtered originalHeight should be retrieved.');
            expect(image.originalSize).toBe(92217, 'Correctly filtered originalSize should be retrieved.');
            expect(image.originalUrl).toBe('http://bettercms.sandbox.mvc4.local.net/uploads/image/15f824b13a9e428fa013dc1940741295/o___Tapir.png', 'Correctly filtered originalUrl should be retrieved.');

            // Tags
            var tags = result.tags;
            expect(tags).toBeDefinedAndNotNull('JSON tags object should be retrieved.');
            expect(tags.length).toBe(2, 'Returned array length should be 2.');

            api.expectBasePropertiesAreNotNull(tags[0]);
            expect(tags[0].name).toBe('tag1_0001_Image_For_Tests', 'Correctly filtered tags[0].name should be retrieved.');
            expect(tags[1].name).toBe('tag2_0001_Image_For_Tests', 'Correctly filtered tags[1].name should be retrieved.');
        });
    });

    it('03302: Should test CRUD for folders.', function () {
        api.testCrud(runs, waitsFor, expect, "5606d5be1b6347d88621a2050129ed3f", "/bcms-api/images/", {
            getPostData: function (json) {
                json.data.title = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });

    function runFoldersListTests(data, expectingResults) {
        var url = '/bcms-api/folders/',
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
});