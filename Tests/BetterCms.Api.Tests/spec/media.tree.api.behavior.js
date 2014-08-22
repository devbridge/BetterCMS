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
            includeFiles: false,
            includeAccessRules: false
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
            includeFiles: false,
            includeAccessRules: false
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
            includeFiles: false,
            includeAccessRules: false
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
            includeFiles: true,
            includeAccessRules: true
        },
            results = {
                filesTreeCount: 4,
                filesFoldersCount: 2,
                firstFilesFolderChildrenCount: 3,
                includeAccessRules: true
            };
        
        runTreeTests(data, results);
    });

    it('03004: Should get a media tree: including everything (except archived)', function () {
        var data = {
            includeArchived: false,
            includeImagesTree: true,
            includeFilesTree: true,
            includeImages: true,
            includeFiles: true,
            includeAccessRules: true
        },
            results = {
                imagesTreeCount: 4,
                imagesFoldersCount: 2,
                filesTreeCount: 4,
                filesFoldersCount: 2,
                firstImagesFolderChildrenCount: 3,
                firstFilesFolderChildrenCount: 3,
                includeAccessRules: true
            };
        
        runTreeTests(data, results);
    });
    
    it('03005: Should get a media tree: including everything (folders / files / images / archived items)', function () {
        var data = {
            includeArchived: true,
            includeImagesTree: true,
            includeFilesTree: true,
            includeImages: true,
            includeFiles: true,
            includeAccessRules: true
        },
            results = {
                imagesTreeCount: 6,
                imagesFoldersCount: 3,
                filesTreeCount: 6,
                filesFoldersCount: 3,
                firstImagesFolderChildrenCount: 5,
                firstFilesFolderChildrenCount: 5,
                includeAccessRules: true
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');

            var tree = result.data;
            expect(tree).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(tree.imagesTree).toBeDefinedAndNotNull('JSON data.imagesTree object should be retrieved.');
            expect(tree.filesTree).toBeDefinedAndNotNull('JSON data.filesTree object should be retrieved.');

            testParentIds(tree.imagesTree, null);
            testParentIds(tree.filesTree, null);
        });
    });

    it('03007.1: Should get a media tree: including everything (folders / files / images / archived items)', function () {
        var url = '/bcms-api/media-tree/',
            result,
            ready = false,
            data = {
                includeArchived: true,
                includeImagesTree: false,
                includeFilesTree: true,
                includeImages: false,
                includeFiles: true,
                includeAccessRules: false
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
            expect(result.data.filesTree).not.toBeNull('JSON data.items object should be retrieved.');

            var folder = null,
                i;

            for (i = 0; i < result.data.filesTree.length; i++) {
                if (result.data.filesTree[i].id == '22c03f8220b446edbb15a2fc00fe2f2f') {
                    folder = result.data.filesTree[i];
                    break;
                }
            }

            expect(folder).not.toBeNull('JSON data.items[id="22c03f8220b446edbb15a2fc00fe2f2f"] object should be retrieved.');
            expect(folder.children).not.toBeNull('JSON data.items[id="22c03f8220b446edbb15a2fc00fe2f2f"].children object should be retrieved.');

            expect(folder.children.length).toBe(3, 'Returned files array length should be 3.');

            expect(folder.children[0].title).toBe('03112: for all', 'Correctly filtered folder.children[0].title should be retrieved.');
            expect(folder.children[1].title).toBe('03112: only for role role1', 'Correctly filtered folder.children[1].title should be retrieved.');
            expect(folder.children[2].title).toBe('03112: only for role role2', 'Correctly filtered folder.children[2].title should be retrieved.');
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
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');

            var tree = result.data;
            expect(tree).toBeDefinedAndNotNull('JSON data object should be retrieved.');

            var testResult = runListTest(tree.filesTree, data.includeFilesTree, data.includeFiles, expectingResults.filesTreeCount, expectingResults.filesFoldersCount);
            if (testResult.folder) {
                testFileFolder(testResult.folder);
                
                expect(testResult.folder.children.length).toBe(expectingResults.firstFilesFolderChildrenCount, 'Correct first files folder children count should be retrieved.');
            }
            if (testResult.file) {
                testFile(testResult.file, expectingResults.includeAccessRules);
            }

            testResult = runListTest(tree.imagesTree, data.includeImagesTree, data.includeImages, expectingResults.imagesTreeCount, expectingResults.imagesFoldersCount);
            if (testResult.folder) {
                testImageFolder(testResult.folder);
                
                expect(testResult.folder.children.length).toBe(expectingResults.firstImagesFolderChildrenCount, 'Correct first images folder children count should be retrieved.');
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
            expect(allItems).toBeDefinedAndNotNull('allItems object should be set');

            items = collectItems(allItems);
            expect(items.length).toBe(expectingCount, 'Correct array items count should be retrieved.');
            
            folders = findByContentType('Folder', items);
            expect(folders.length).toBe(expectingFolders, 'Correct count of folders should be retrieved.');

            result.folder = folders[0];

            if (includeFiles) {
                files = findByContentType('File', items);
                expect(files.length).toBe(expectingCount - expectingFolders, 'Correct count of files should be retrieved.');

                result.file = files[0];
            }
        } else {
            expect(allItems).toBeNull('tree should be null.');
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
        
        expect(image.title).toBe('_0000_Image_1', 'Correctly filtered title should be retrieved.');
        expect(image.description).toBe('_0000_Image_1 description', 'Correctly filtered description should be retrieved.');
        expect(image.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
    }

    function testImageFolder(folder) {
        testFolderBase(folder);
        
        expect(folder.title).toBe('_0000_Images_Folder_1', 'Correctly filtered title should be retrieved.');
        expect(folder.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
    }
    
    function testFile(file, includeAccessRules) {
        testFileBase(file);
        
        expect(file.title).toBe('_0000_File_1', 'Correctly filtered title should be retrieved.');
        expect(file.description).toBe('_0000_File_1 description', 'Correctly filtered description should be retrieved.');
        expect(file.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');

        if (includeAccessRules) {
            var rules = file.accessRules;
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
        }
    }

    function testFileFolder(folder) {
        testFolderBase(folder);
        
        expect(folder.title).toBe('_0000_Files_Folder_1', 'Correctly filtered title should be retrieved.');
        expect(folder.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
    }
    
    function testBase(item) {
        api.expectBasePropertiesAreNotNull(item);

        expect(item.parentFolderId).toBeNull('parentFolderId should be null.');
        expect(item.isArchived).toBe(false, 'Correctly filtered isArchived should be retrieved.');
    }

    function testFileBase(file) {
        testBase(file);
        
        expect(file.mediaContentType).toBe('File', 'Correctly filtered mediaContentType should be retrieved.');
        expect(file.url).toBeDefinedAndNotNull('url should be retrieved.');
        expect(file.children.length).toBe(0, 'Returned children nodes array length should be 0.');
    }

    function testFolderBase(folder) {
        testBase(folder);
        
        expect(folder.mediaContentType).toBe('Folder', 'Correctly filtered mediaContentType should be retrieved.');
        expect(folder.url).toBeNull('url should be null.');
    }
    
    function testParentIds(items, parentFolderId) {
        for (var i = 0; i < items.length; i++) {
            if (parentFolderId) {
                expect(items[i].parentFolderId).toBe(parentFolderId, 'Correctly filtered parentFolderId should be retrieved.');
            } else {
                expect(items[i].parentFolderId).toBeNull('parentFolderId should be null.');
            }
            
            if (items[i].children) {
                testParentIds(items[i].children, items[i].id);
            }
        }
    }
});