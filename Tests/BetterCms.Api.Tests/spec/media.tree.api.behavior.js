/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('media.tree.api.behavior', function () {
    'use strict';

    it('03000: Should get a media tree: only image folders', function () {
        var data = {
            includeArchived: false,
            includeImagesTree: true,
            includeFilesTree: false,
            includeImages: false,
            includeFiles: false
        },
            results = {
                imagesTreeCount: 2,
                imagesFoldersCount: 2,
                firstImagesFolderChildrenCount: 2
            };

        runTreeTests(data, results);
    });

    it('03001: Should get a media tree: only image folders and images', function () {
        var data = {
            includeArchived: false,
            includeImagesTree: true,
            includeFilesTree: false,
            includeImages: true,
            includeFiles: false
        },
            results = {
                imagesTreeCount: 4,
                imagesFoldersCount: 2,
                firstImagesFolderChildrenCount: 3
            };
        
        runTreeTests(data, results);
    });
    
    it('03002: Should get a media tree: only files folders', function () {
        var data = {
            includeArchived: false,
            includeImagesTree: false,
            includeFilesTree: true,
            includeImages: false,
            includeFiles: false
        },
            results = {
                filesTreeCount: 2,
                filesFoldersCount: 2,
                firstFilesFolderChildrenCount: 2
            };
        
        runTreeTests(data, results);
    });

    it('03003: Should get a media tree: only files folders and files', function () {
        var data = {
            includeArchived: false,
            includeImagesTree: false,
            includeFilesTree: true,
            includeImages: false,
            includeFiles: true
        },
            results = {
                filesTreeCount: 4,
                filesFoldersCount: 2,
                firstFilesFolderChildrenCount: 3
            };
        
        runTreeTests(data, results);
    });

    it('03004: Should get a media tree: including everything (except archived)', function () {
        var data = {
            includeArchived: false,
            includeImagesTree: true,
            includeFilesTree: true,
            includeImages: true,
            includeFiles: true
        },
            results = {
                imagesTreeCount: 4,
                imagesFoldersCount: 2,
                filesTreeCount: 4,
                filesFoldersCount: 2,
                firstImagesFolderChildrenCount: 3,
                firstFilesFolderChildrenCount: 3
            };
        
        runTreeTests(data, results);
    });
    
    it('03005: Should get a media tree: including everything (folders / files / images / archived items)', function () {
        var data = {
            includeArchived: true,
            includeImagesTree: true,
            includeFilesTree: true,
            includeImages: true,
            includeFiles: true
        },
            results = {
                imagesTreeCount: 6,
                imagesFoldersCount: 3,
                filesTreeCount: 6,
                filesFoldersCount: 3,
                firstImagesFolderChildrenCount: 5,
                firstFilesFolderChildrenCount: 5
            };
        
        runTreeTests(data, results);
    });

    it('03006: Should get a media tree with corrent parent ids set', function() {
        var url = '/bcms-api/media-tree/',
            result,
            ready = false,
            data = {
                includeImages: true,
                includeArchived: true,
                includeFiles: true
            };

        runs(function() {
            api.get(url, data, function(json) {
                result = json;
                ready = true;
            });
        });
        
        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).not.toBeNull();

            var tree = result.data;
            expect(tree).not.toBeNull();
            expect(tree.imagesTree).not.toBeNull();
            expect(tree.filesTree).not.toBeNull();

            testParentIds(tree.imagesTree, null);
            testParentIds(tree.filesTree, null);
        });
    });

    function runTreeTests(data, expectingResults) {
        var url = '/bcms-api/media-tree/',
            result,
            ready = false;

        runs(function() {
            api.get(url, data, function(json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function() {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function() {
            expect(result).not.toBeNull();

            var tree = result.data;
            expect(tree).not.toBeNull();

            var testResult = runListTest(tree.filesTree, data.includeFilesTree, data.includeFiles, expectingResults.filesTreeCount, expectingResults.filesFoldersCount);
            if (testResult.folder) {
                testFileFolder(testResult.folder);
                
                expect(testResult.folder.children.length).toBe(expectingResults.firstFilesFolderChildrenCount);
            }
            if (testResult.file) {
                testFile(testResult.file);
            }

            testResult = runListTest(tree.imagesTree, data.includeImagesTree, data.includeImages, expectingResults.imagesTreeCount, expectingResults.imagesFoldersCount);
            if (testResult.folder) {
                testImageFolder(testResult.folder);
                
                expect(testResult.folder.children.length).toBe(expectingResults.firstImagesFolderChildrenCount);
            }
            if (testResult.file) {
                testImage(testResult.file);
            }
        });
    }

    function runListTest(allItems, includeTree, includeFiles, expectingCount, expectingFolders) {
        var items,
            folders,
            files,
            result = {};

        if (includeTree) {
            expect(allItems).not.toBeNull();

            items = collectItems(allItems);
            expect(items.length).toBe(expectingCount);
            
            folders = findByContentType('Folder', items);
            expect(folders.length).toBe(expectingFolders);

            result.folder = folders[0];

            if (includeFiles) {
                files = findByContentType('File', items);
                expect(files.length).toBe(expectingCount - expectingFolders);

                result.file = files[0];
            }
        } else {
            expect(allItems).toBeNull();
        }

        return result;
    }

    function findByContentType(contentType, allItems) {
        var items = [];

        for (var i = 0; i < allItems.length; i++) {
            if (allItems[i].mediaContentType == contentType) {
                items.push(allItems[i]);
            }
        }

        return items;
    }

    function collectItems(allItems) {
        var items = [];

        for (var i = 0; i < allItems.length; i++) {
            if (allItems[i].title.indexOf('_0000_') === 0) {
                items.push(allItems[i]);
            }
        }

        return items;
    }

    function testImage(image) {
        testFileBase(image);
        
        expect(image.title).toBe('_0000_Image_1');
        expect(image.isArchived).toBe(false);
    }

    function testImageFolder(folder) {
        testFolderBase(folder);
        
        expect(folder.title).toBe('_0000_Images_Folder_1');
        expect(folder.isArchived).toBe(false);
    }
    
    function testFile(file) {
        testFileBase(file);
        
        expect(file.title).toBe('_0000_File_1');
        expect(file.isArchived).toBe(false);
    }

    function testFileFolder(folder) {
        testFolderBase(folder);
        
        expect(folder.title).toBe('_0000_Files_Folder_1');
        expect(folder.isArchived).toBe(false);
    }
    
    function testBase(item) {
        api.expectBasePropertiesAreNotNull(item);

        expect(item.parentFolderId).toBeNull();
        expect(item.isArchived).toBe(false);
    }

    function testFileBase(file) {
        testBase(file);
        
        expect(file.mediaContentType).toBe('File');
        expect(file.url).not.toBeNull();
        expect(file.children.length).toBe(0);
    }

    function testFolderBase(folder) {
        testBase(folder);
        
        expect(folder.mediaContentType).toBe('Folder');
        expect(folder.url).toBeNull();
    }
    
    function testParentIds(items, parentFolderId) {
        for (var i = 0; i < items.length; i++) {
            if (parentFolderId) {
                expect(items[i].parentFolderId).toBe(parentFolderId);
            } else {
                expect(items[i].parentFolderId).toBeNull();
            }
            
            if (items[i].children) {
                testParentIds(items[i].children, items[i].id);
            }
        }
    }
});