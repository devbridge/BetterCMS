/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Pages: Contents', function () {
    'use strict';

    it('01100: Should get html content by content id', function () {
        var url = '/bcms-api/contents/html/61263510-2810-4c6f-b7c5-a20400fe6877',
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

            var content = result.data;
            expect(content).toBeDefinedAndNotNull('JSON data object should be retrieved.');

            api.expectBasePropertiesAreNotNull(content);
            expect(content.name).toBe('MainContent1 - HTML');
            expect(content.activationDate).toBe('/Date(1374526800000-0000)/');
            expect(content.expirationDate).toBe('/Date(1974229199000-0000)/');
            expect(content.html).toBe('<p>MainContent1 - HTML</p>');
            expect(content.customCss).toBe('custom css');
            expect(content.useCustomCss).toBe(true);
            expect(content.customJavaScript).toBe("console.log('test')");
            expect(content.useCustomJavaScript).toBe(true);
            expect(content.isPublished).toBe(true);
            expect(content.publishedOn).toBe('/Date(1374642405000-0000)/');
            expect(content.publishedByUser).toBe('Better CMS test user');
        });
    });
    
    it('01101: Should get blog post content by id', function () {
        var url = '/bcms-api/contents/blog-post/06182C92-E61E-4023-B3C9-A2050086598C',
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

            var content = result.data;
            expect(content).toBeDefinedAndNotNull('JSON data object should be retrieved.');

            api.expectBasePropertiesAreNotNull(content);
            expect(content.name).toBe('_0002_Blog_Post_For_Tests');
            expect(content.html).toBe('<p>_0002_Blog_Post_For_Tests Test HTML</p>');
            expect(content.isPublished).toBe(true);
            expect(content.publishedOn).toBe('/Date(1374643031000-0000)/');
            expect(content.publishedByUser).toBe('Better CMS test user');
        });
    });
    
    it('01102: Should get content history by content id', function () {
        var url = '/bcms-api/contents/97A1C2CF-DC8A-4D3F-9DD1-A205008C3F36/history',
            result,
            ready = false,
            data = {
                order: {
                    by: [
                        {field: 'CreatedOn'}
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

            var history = result.data;
            expect(history).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(history.length).toBe(3, 'Returned array length should be 3.');

            var published = history[0];
            var archived = history[1];
            var draft = history[2];

            api.expectBasePropertiesAreNotNull(published);
            expect(published.contentType).toBe('html-content');

            // Status
            expect(published.status).toBe('Published');
            expect(archived.status).toBe('Archived');
            expect(draft.status).toBe('Draft');
            
            // Original id
            expect(published.originalContentId).toBeNull();
            expect(archived.originalContentId).toBeDefinedAndNotNull('archived originalContentId should be retrieved.');
            expect(draft.originalContentId).toBeDefinedAndNotNull('draft originalContentId should be retrieved.');
            
            // Publish info
            expect(published.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(published.publishedByUser).toBeDefinedAndNotNull('publishedByUser should be retrieved.');
            expect(archived.publishedOn).toBeNull();
            expect(archived.publishedByUser).toBeNull();
            expect(draft.publishedOn).toBeNull();
            expect(draft.publishedByUser).toBeNull();
            
            // Archivation info
            expect(archived.archivedOn).toBeDefinedAndNotNull('archivedOn should be retrieved.');
            expect(archived.archivedByUser).toBeDefinedAndNotNull('archivedByUser should be retrieved.');
            expect(published.archivedOn).toBeNull();
            expect(published.archivedByUser).toBeNull();
            expect(draft.archivedOn).toBeNull();
            expect(draft.archivedByUser).toBeNull();
        });
    });
});