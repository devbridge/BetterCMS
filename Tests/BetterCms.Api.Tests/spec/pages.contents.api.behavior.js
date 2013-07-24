/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Pages: Contents', function () {
    'use strict';

    it('0000: Should get html content by content id', function () {
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
            expect(result).toBeDefined();

            var content = result.data;
            expect(content).toBeDefined();

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
    
    it('0001: Should get blog post content by id', function () {
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
            expect(result).toBeDefined();

            var content = result.data;
            expect(content).toBeDefined();

            api.expectBasePropertiesAreNotNull(content);
            expect(content.name).toBe('_0002_Blog_Post_For_Tests');
            expect(content.html).toBe('<p>_0002_Blog_Post_For_Tests Test HTML</p>');
            expect(content.isPublished).toBe(true);
            expect(content.publishedOn).toBe('/Date(1374643031000-0000)/');
            expect(content.publishedByUser).toBe('Better CMS test user');
        });
    });
    
    it('0002: Should get content history by content id', function () {
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
            expect(result).toBeDefined();

            var history = result.data;
            expect(history).toBeDefined();
            expect(history.length).toBe(3);

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
            expect(published.originalContentId).toBeUndefined();
            expect(archived.originalContentId).toBeDefined();
            expect(draft.originalContentId).toBeDefined();
            
            // Publish info
            expect(published.publishedOn).toBeDefined();
            expect(published.publishedByUser).toBeDefined();
            expect(archived.publishedOn).toBeUndefined();
            expect(archived.publishedByUser).toBeUndefined();
            expect(draft.publishedOn).toBeUndefined();
            expect(draft.publishedByUser).toBeUndefined();
            
            // Archivation info
            expect(archived.archivedOn).toBeDefined();
            expect(archived.archivedByUser).toBeDefined();
            expect(published.archivedOn).toBeUndefined();
            expect(published.archivedByUser).toBeUndefined();
            expect(draft.archivedOn).toBeUndefined();
            expect(draft.archivedByUser).toBeUndefined();
        });
    });
});