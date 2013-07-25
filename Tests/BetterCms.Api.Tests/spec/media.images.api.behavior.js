/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Media Manager: Images', function () {
    'use strict';

    it('0000: Should get a list of image folders', function () {
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
                totalCount: 2,
                title: '_0000_Images_Folder_1',
                isArchived: false
            };

        runImagesListTests(data, results);
    });
    
    it('0001: Should get a list of images without folders', function () {
        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: false,
            includeImages: true,
            includeArchived: false,
            take: 1
        },
            results =
            {
                totalCount: 2,
                title: '_0000_Image_1',
                isArchived: false
            };
        
        runImagesListTests(data, results);
    });
    
    it('0002: Should get a list of not archived images and folders', function () {
        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: true,
            includeImages: true,
            includeArchived: false,
            take: 1
        },
            results =
            {
                totalCount: 4,
                title: '_0000_Image_1',
                isArchived: false
            };
        
        runImagesListTests(data, results);
    });
    
    it('0003: Should get a list of images and folders (including archived)', function () {
        var data = {
            filter: {
                where: [{ field: 'Title', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Title' }]
            },
            includeFolders: true,
            includeImages: true,
            includeArchived: true,
            take: 1
        },
            results =
            {
                totalCount: 6,
                title: '_0000_Image_1',
                isArchived: false
            };
        
        runImagesListTests(data, results);
    });

    it('0004: Should get a list of images and folders (only archived)', function () {
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
            includeImages: true,
            includeArchived: true,
            take: 1
        },
            results =
            {
                totalCount: 2,
                title: '_0000_Image_3_Archived',
                isArchived: true
            };
        
        runImagesListTests(data, results);
    });
    
    it('0005: Should get a list from subfolder with specified image', function () {
        var url = '/bcms-api/images/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Title', value: '_0001_Image_For_Tests' }]
                },
                folderId: 'a9ed8f5c4241427bbe5ca205012b215f'
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

            var image = result.data.items[0];
            api.expectBasePropertiesAreNotNull(image);
            expect(image.title).toBe('_0001_Image_For_Tests');
            expect(image.caption).toBe('Image Caption');
            expect(image.mediaContentType).toBe('File');
            expect(image.fileExtension).toBe('.png');
            expect(image.fileSize).toBe(26354);
            expect(image.imageUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/15f824b13a9e428fa013dc1940741295/__Tapir_9.png');
            expect(image.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/15f824b13a9e428fa013dc1940741295/t___Tapir_9.png');
            expect(image.isArchived).toBe(false);
        });
    });
    
    it('0006: Should get a list with specified folder', function () {
        var url = '/bcms-api/images/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'Title', value: '_0001_Images_Folder_For_Tests' }]
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
            expect(folder.title).toBe('_0001_Images_Folder_For_Tests');
            expect(folder.caption).toBeUndefined();
            expect(folder.mediaContentType).toBe('Folder');
            expect(folder.fileExtension).toBeUndefined();
            expect(folder.fileSize).toBeUndefined();
            expect(folder.imageUrl).toBeUndefined();
            expect(folder.thumbnailUrl).toBeUndefined();
            expect(folder.isArchived).toBe(false);
        });
    });

    it('0007: Should get image properties by image id', function () {
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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();

            // Image
            var image = result.data;
            expect(image).toBeDefined();
            api.expectBasePropertiesAreNotNull(image);
            expect(image.title).toBe('_0001_Image_For_Tests');
            expect(image.caption).toBe('Image Caption');
            expect(image.fileExtension).toBe('.png');
            expect(image.fileSize).toBe(26354);
            expect(image.imageUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/15f824b13a9e428fa013dc1940741295/__Tapir_9.png');
            expect(image.width).toBe(480);
            expect(image.height).toBe(100);
            expect(image.thumbnailUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/15f824b13a9e428fa013dc1940741295/t___Tapir_9.png');
            expect(image.thumbnailWidth).toBe(150);
            expect(image.thumbnailHeight).toBe(150);
            expect(image.thumbnailSize).toBe(15590);
            expect(image.isArchived).toBe(false);
            expect(image.folderId).toBeDefined();
            expect(image.folderName).toBe('_0001_Images_Folder_For_Tests');
            expect(image.publishedOn).toBeDefined();
            expect(image.originalFileName).toBe('__Tapir.png');
            expect(image.originalFileExtension).toBe('.png');
            expect(image.originalWidth).toBe(480);
            expect(image.originalHeight).toBe(480);
            expect(image.originalSize).toBe(92217);
            expect(image.originalUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/15f824b13a9e428fa013dc1940741295/o___Tapir.png');

            // Tags
            var tags = result.tags;
            expect(tags).toBeDefined();
            expect(tags.length).toBe(2);

            api.expectBasePropertiesAreNotNull(tags[0]);
            expect(tags[0].name).toBe('tag1_0001_Image_For_Tests');
            expect(tags[1].name).toBe('tag2_0001_Image_For_Tests');
        });
    });

    it('0008: Should get images list, filtered by tags, using AND connector', function () {
        filterByTags('and', 1, ['IFilterByTags Image 1']);
    });

    it('0009: Should get images list, filtered by tags, using OR connector', function () {
        filterByTags('or', 2, ['IFilterByTags Image 1', 'IFilterByTags Image 3']);
    });

    function runImagesListTests(data, expectingResults) {
        var url = '/bcms-api/images/',
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
        var url = '/bcms-api/images/',
            result,
            ready = false;

        var data = {
            order: {
                by: [{ field: 'Title' }]
            },
            folderId: 'e16c7e649d564e19946fa206008a198d',
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