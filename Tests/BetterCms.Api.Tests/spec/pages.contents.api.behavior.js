/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('pages.contents.api.behavior', function () {
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
            expect(content.name).toBe('MainContent1 - HTML', 'Correctly filtered name should be retrieved.');
            expect(new Date(content.activationDate).getTime()).toBe(new Date('2013-07-23T00:00:00.0000000').getTime(), 'Correctly filtered activationDate should be retrieved.');
            
            expect(new Date(content.expirationDate).getTime()).toBe(new Date('2032-07-23T23:59:59.0000000').getTime(), 'Correctly filtered expirationDate should be retrieved.');
            
            expect(content.html).toBe('<p>MainContent1 - HTML</p>', 'Correctly filtered html should be retrieved.');
            expect(content.customCss).toBe('custom css', 'Correctly filtered customCss should be retrieved.');
            expect(content.useCustomCss).toBe(true, 'Correctly filtered useCustomCss should be retrieved.');
            expect(content.customJavaScript).toBe("console.log('test')", 'Correctly filtered customJavaScript should be retrieved.');
            expect(content.useCustomJavaScript).toBe(true, 'Correctly filtered useCustomJavaScipt should be retrieved.');
            expect(content.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(new Date(content.publishedOn).getTime()).toBe(new Date('2013-07-24T08:06:45.0000000').getTime(), 'Correctly filtered publishedOn should be retrieved.');
            expect(content.publishedByUser).toBe('Better CMS test user', 'Correctly filtered publishedByUser should be retrieved.');
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
            expect(published.contentType).toBe('html-content', 'Correctly filtered contentType should be retrieved.');

            // Status
            expect(published.status).toBe('Published', 'Correctly filtered published status should be retrieved.');
            expect(archived.status).toBe('Archived', 'Correctly filtered archived should be retrieved.');
            expect(draft.status).toBe('Draft', 'Correctly filtered draft should be retrieved.');
            
            // Original id
            expect(published.originalContentId).toBeNull('originalContentId for published content should be null.');
            expect(archived.originalContentId).toBeDefinedAndNotNull('archived originalContentId should be retrieved.');
            expect(draft.originalContentId).toBeDefinedAndNotNull('draft originalContentId should be retrieved.');
            
            // Publish info
            expect(published.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(published.publishedByUser).toBeDefinedAndNotNull('publishedByUser should be retrieved.');
            expect(archived.publishedOn).toBeNull('publishedOn for archived content should be null.');
            expect(archived.publishedByUser).toBeNull('publishedByUser for archived content should be null.');
            expect(draft.publishedOn).toBeNull('publishedOn for draft content should be null.');
            expect(draft.publishedByUser).toBeNull('publishedByUser for draft content should be null.');
            
            // Archivation info
            expect(archived.archivedOn).toBeDefinedAndNotNull('archivedOn should be retrieved.');
            expect(archived.archivedByUser).toBeDefinedAndNotNull('archivedByUser should be retrieved.');
            expect(published.archivedOn).toBeNull('archivedOn for published content should be null.');
            expect(published.archivedByUser).toBeNull('archivedByUser for published content should be null.');
            expect(draft.archivedOn).toBeNull('archivedOn for draft content should be null.');
            expect(draft.archivedByUser).toBeNull('archivedByUser for draft content should be null.');
        });
    });
    
    it('01103: Should get page content options by content id', function () {
        var url = '/bcms-api/pages/contents/D0E1D935-C15D-4984-9220-A21800B84EE3/options',
            result,
            ready = false,
            data = {
                order: {
                    by: [
                        { field: 'Key', direction: 'desc' }
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
            expect(result.data.items).not.toBeNull('JSON data.items object should be retrieved.');
            expect(result.data.items.length).toBe(3, 'Returned array length should be 3.');
            
            expect(result.data.items[0].key).toBe('Option 3', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].key).toBe('Option 2', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].key).toBe('Option 1', 'Correctly filtered items[2].key should be retrieved.');

            expect(result.data.items[0].value).toBe('12.5', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].value).toBe('A', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].value).toBe('200', 'Correctly filtered items[2].key should be retrieved.');

            expect(result.data.items[0].defaultValue).toBeNull('Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].defaultValue).toBeNull('Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].defaultValue).toBe('15', 'Correctly filtered items[2].key should be retrieved.');

            expect(result.data.items[0].type).toBe('Float', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].type).toBe('Text', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].type).toBe('Integer', 'Correctly filtered items[2].key should be retrieved.');
        });
    });
});