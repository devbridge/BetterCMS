/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('blog.authors.api.behavior', function () {
    'use strict';

    it('02000: Should get a list of authors', function () {
        var url = '/bcms-api/authors/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Name', operation: 'StartsWith', value: '_0000_' }]
            },
            order: {
                by: [{ field: 'Name' }]
            },
            take: 2,
            skip: 1,
            includeUnpublished: true
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
            expect(result.data.totalCount).toBe(4, 'Total count should be 4.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');

            api.expectBasePropertiesAreNotNull(result.data.items[0]);
            api.expectBasePropertiesAreNotNull(result.data.items[1]);

            expect(result.data.items[0].name).toBe('_0000_Author_2', 'Items[0] name should be _0000_Author_2');
            expect(result.data.items[0].description).toBe('_0000_Author_2 description', 'Items[0] description should be _0000_Author_2 description');
            expect(result.data.items[0].imageId).toBeDefinedAndNotNull('Items[0] image id should be retrieved.');
            expect(result.data.items[0].imageUrl).toBeDefinedAndNotNull('Items[0] image URL should be retrieved.');
            expect(result.data.items[0].imageThumbnailUrl).toBeDefinedAndNotNull('Items[0] image thumbnail URL should be retrieved.');
            expect(result.data.items[0].imageCaption).toBe('Image caption for _0000_Author_2', 'Items[0] image caption should be \"Image caption for _0000_Author_2\"');
            
            expect(result.data.items[1].name).toBe('_0000_Author_3', 'Items[1] name should be _0000_Author_3');
            expect(result.data.items[1].description).toBeNull();
            expect(result.data.items[1].imageId).toBeNull('Items[1] image id should be null');
            expect(result.data.items[1].imageUrl).toBeNull('Items[1] image URL should be null');
            expect(result.data.items[1].imageThumbnailUrl).toBeNull('Items[1] image thumbnail URL should be null');
            expect(result.data.items[1].imageCaption).toBeNull('Items[1] image caption should be null');
        });
    });
    
    it('02001: Should get an author by id', function () {
        var url = '/bcms-api/authors/b82a9428b40047c498a9a20500b7a276',
            result,
            ready = false;

        runs(function () {
            api.get(url, null, function (json) {
                result = json;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');

            var author = result.data;
            expect(author).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(author);
            
            expect(author.name).toBe('_0000_Author_2', 'Correctly filtered result.data.name should be retrieved.');
            expect(author.imageId).toBeDefinedAndNotNull('imageId should be retrieved.');
            expect(author.imageUrl).toBeDefinedAndNotNull('imageUrl should be retrieved.');
            expect(author.imageThumbnailUrl).toBeDefinedAndNotNull('imageThumbnailUrl should be retrieved.');
            expect(author.imageCaption).toBe('Image caption for _0000_Author_2', 'Correctly filtered result.data.imageCaption should be retrieved.');
        });
    });
    
    it('02002: Should get a list with one author, filtered by all available columns', function () {
        var url = '/bcms-api/authors/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: '200f5ee252af47abb5bea20601210dd3' },
                    { field: 'CreatedOn', value: '2013-07-25 17:32:24.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-25 17:32:59.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '2' },
                    { field: 'Name', value: '02002' },
                    { field: 'Description', value: '02002DSCR' },
                    { field: 'ImageId', value: 'a19a6e5d7e4948a5b5e0a206012117bd' },
                    { field: 'ImageUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/b244cadb494d4121b896f21ac93483ef/1_1.jpg' },
                    { field: 'ImageThumbnailUrl', value: 'http://bettercms.sandbox.mvc4.local.net/uploads/image/b244cadb494d4121b896f21ac93483ef/t_1_1.png' },
                    { field: 'ImageCaption', value: 'Image Caption' }
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

            expect(result.data.items[0].id).toBe('200f5ee252af47abb5bea20601210dd3', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });

    it('02003: Should test CRUD for authors.', function () {
        api.testCrud(runs, waitsFor, expect, "b82a9428b40047c498a9a20500b7a276", "/bcms-api/authors/", {
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });
});