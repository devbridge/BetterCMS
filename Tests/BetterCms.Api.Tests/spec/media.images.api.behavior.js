/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Media Manager: Images', function () {
    'use strict';

    it('03200: Should get a list of image folders', function () {
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
    
    it('03201: Should get a list of images without folders', function () {
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
    
    it('03202: Should get a list of not archived images and folders', function () {
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
    
    it('03203: Should get a list of images and folders (including archived)', function () {
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

    it('03204: Should get a list of images and folders (only archived)', function () {
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
    
    it('03205: Should get a list from subfolder with specified image', function () {
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.items).not.toBeNull();
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
    
    it('03206: Should get a list with specified folder', function () {
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.items).not.toBeNull();
            expect(result.data.totalCount).toBe(1);
            expect(result.data.items.length).toBe(1);

            var folder = result.data.items[0];
            api.expectBasePropertiesAreNotNull(folder);
            expect(folder.title).toBe('_0001_Images_Folder_For_Tests');
            expect(folder.caption).toBeNull();
            expect(folder.mediaContentType).toBe('Folder');
            expect(folder.fileExtension).toBeNull();
            expect(folder.fileSize).toBeNull();
            expect(folder.imageUrl).toBeNull();
            expect(folder.thumbnailUrl).toBeNull();
            expect(folder.isArchived).toBe(false);
        });
    });

    it('03207: Should get image properties by image id', function () {
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();

            // Image
            var image = result.data;
            expect(image).not.toBeNull();
            api.expectBasePropertiesAreNotNull(image);
            expect(image.title).toBe('_0001_Image_For_Tests');
            expect(image.caption).toBe('Image Caption');
            expect(image.description).toBe('Image Description');
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
            expect(image.folderId).not.toBeNull();
            expect(image.folderName).toBe('_0001_Images_Folder_For_Tests');
            expect(image.publishedOn).not.toBeNull();
            expect(image.originalFileName).toBe('__Tapir.png');
            expect(image.originalFileExtension).toBe('.png');
            expect(image.originalWidth).toBe(480);
            expect(image.originalHeight).toBe(480);
            expect(image.originalSize).toBe(92217);
            expect(image.originalUrl).toBe('http://bettercms.sandbox.mvc4.local/uploads/image/15f824b13a9e428fa013dc1940741295/o___Tapir.png');

            // Tags
            var tags = result.tags;
            expect(tags).not.toBeNull();
            expect(tags.length).toBe(2);

            api.expectBasePropertiesAreNotNull(tags[0]);
            expect(tags[0].name).toBe('tag1_0001_Image_For_Tests');
            expect(tags[1].name).toBe('tag2_0001_Image_For_Tests');
        });
    });

    it('03208: Should get images list, filtered by tags, using AND connector', function () {
        filterByTags('and', 1, ['IFilterByTags Image 1']);
    });

    it('03209: Should get images list, filtered by tags, using OR connector', function () {
        filterByTags('or', 2, ['IFilterByTags Image 1', 'IFilterByTags Image 3']);
    });

    it('03210: Should get a list with one image, filtered by all available columns', function () {
        var url = '/bcms-api/images/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: 'b53f6544cd6242a29a70a207007c75f8' },
                    { field: 'CreatedOn', value: '2013-07-26 07:33:08.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 07:33:27.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '6' },

                    { field: 'Title', value: '03210' },
                    { field: 'Caption', value: '03210 caption' },
                    { field: 'MediaContentType', value: 'File' },
                    { field: 'FileExtension', value: '.jpg' },
                    { field: 'FileSize', value: 9901 },
                    { field: 'ImageUrl', value: 'http://bettercms.sandbox.mvc4.local/uploads/image/c7371305555341ba9be1958aaca110e7/1_1.jpg' },
                    { field: 'ThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local/uploads/image/c7371305555341ba9be1958aaca110e7/t_1_1.png' },
                    { field: 'IsArchived', value: false }
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

            expect(result.data.items[0].id).toBe('b53f6544cd6242a29a70a207007c75f8');

            // Check if model properties count didn't changed. If so - update filter current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]));
        });
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