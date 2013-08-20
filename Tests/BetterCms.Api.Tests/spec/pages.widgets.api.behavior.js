/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('pages.widgets.api.behavior', function () {
    'use strict';

    it('01200: Should get a list of widgets', function () {
        var url = '/bcms-api/widgets/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [{ field: 'Name', operation: 'StartsWith', value: '_0004_' }]
            },
            order: {
                by: [{field: 'Name'}]
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

            var htmlWidget = result.data.items[0];
            api.expectBasePropertiesAreNotNull(htmlWidget);
            expect(htmlWidget.widgetType).toBe('html-widget', 'Correctly filtered widgetType should be retrieved.');
            expect(htmlWidget.name).toBe('_0004_Html_Widget_2', 'Correctly filtered name should be retrieved.');
            expect(htmlWidget.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(htmlWidget.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(htmlWidget.publishedByUser).toBe('Better CMS test user', 'Correctly filtered publishedByUser should be retrieved.');
            
            var serverWidget = result.data.items[1];
            api.expectBasePropertiesAreNotNull(serverWidget);
            expect(serverWidget.widgetType).toBe('server-widget', 'Correctly filtered widgetType should be retrieved.');
            expect(serverWidget.name).toBe('_0004_Server_Widget_1', 'Correctly filtered name should be retrieved.');
            expect(serverWidget.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(serverWidget.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(serverWidget.publishedByUser).toBe('Better CMS test user', 'Correctly filtered publishedByUser should be retrieved.');
        });
    });
    
    it('01201: Should get an html content widget by id', function () {
        var url = '/bcms-api/widgets/html-content/fa0cbcfb96454fcfa576a205009119c8',
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

            var widget = result.data;
            expect(widget).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(widget);
            expect(widget.name).toBe('_0004_Html_Widget_2', 'Correctly filtered name should be retrieved.');
            expect(widget.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(widget.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(widget.publishedByUser).toBe('Better CMS test user', 'Correctly filtered publishedByUser should be retrieved.');
            expect(widget.categoryId).toBeDefinedAndNotNull('categoryId should be retrieved.');
            expect(widget.categoryName).toBe('Category for _0004_Html_Widget_2', 'Correctly filtered categoryName should be retrieved.');
            expect(widget.customCss).toBe('custom css', 'Correctly filtered customCss should be retrieved.');
            expect(widget.useCustomCss).toBe(true, 'Correctly filtered useCustomCss should be retrieved.');
            expect(widget.html).toBe('_0004_Html_Widget_2 HTML', 'Correctly filtered html should be retrieved.');
            expect(widget.useHtml).toBe(true, 'Correctly filtered useHtml should be retrieved.');
            expect(widget.customJavaScript).toBe("console.log('test')", 'Correctly filtered customJavaScript should be retrieved.');
            expect(widget.useCustomJavaScript).toBe(true, 'Correctly filtered useCustomJavaScript should be retrieved.');
        });
    });
    
    it('01202: Should get a server control widget by id', function () {
        var url = '/bcms-api/widgets/server-control/3ac115dfc5f34f148141a205009162cd',
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

            var widget = result.data;
            expect(widget).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(widget);
            expect(widget.name).toBe('_0004_Server_Widget_1', 'Correctly filtered name should be retrieved.');
            expect(widget.widgetUrl).toBe('~/Views/Widgets/TestWidget.cshtml', 'Correctly filtered widgetUrl should be retrieved.');
            expect(widget.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png', 'Correctly filtered previewUrl should be retrieved.');
            expect(widget.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(widget.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(widget.publishedByUser).toBe('Better CMS test user', 'Correctly filtered publishedByUser should be retrieved.');
            expect(widget.categoryId).toBeDefinedAndNotNull('categoryId should be retrieved.');
            expect(widget.categoryName).toBe('Category for _0004_Server_Widget_1', 'Correctly filtered categoryName should be retrieved.');
        });
    });
    
    it('01203: Should get a list with one widget, filtered by all available columns', function () {
        var url = '/bcms-api/widgets/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: 'd674977e193f4d858b83a20700ac13b6' },
                    { field: 'CreatedOn', value: '2013-07-26 10:26:30.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 10:26:50.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '2' },

                    { field: 'Name', value: '01203' },
                    { field: 'IsPublished', value: true },
                    { field: 'PublishedOn', value: '2013-07-26 10:26:50.000' },
                    { field: 'PublishedByUser', value: 'Better CMS test user' },
                    { field: 'CategoryId', value: '1d8dbfbce4bf46c2acb7a20700ac186a' },
                    { field: 'CategoryName', value: '01203' }
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

            expect(result.data.items[0].id).toBe('d674977e193f4d858b83a20700ac13b6', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            // data.filter.where.length + 1 <-- Because field WidgetType cannnot be filtered by
            expect(data.filter.where.length + 1).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
    
    it('01204: Should throw validation exception for filterting by WidgetType, when getting widgets.', function () {
        var url = '/bcms-api/widgets/',
            result,
            ready = false,
            data = {
                filter: {
                    where: [{ field: 'WidgetType', value: 'test' }]
                }
            };

        runs(function () {
            api.get(url, data, null, function (response) {
                result = response.responseJSON;
                ready = true;
            });
        });

        waitsFor(function () {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            api.expectValidationExceptionIsThrown(result, 'Data');
        });
    });
    
    it('01205: Should get widget options by widget id', function () {
        var url = '/bcms-api/widgets/server-control/f34c1135dc364f49a674a21800aadbff/options',
            result,
            ready = false;

        var data = {
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

            expect(result.data.items[0].defaultValue).toBe('A', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].defaultValue).toBe('18', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].defaultValue).toBe('12.5', 'Correctly filtered items[2].key should be retrieved.');

            expect(result.data.items[0].type).toBe('Text', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].type).toBe('Integer', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].type).toBe('Float', 'Correctly filtered items[2].key should be retrieved.');
        });
    });
    
    it('01206: Should get a list with one layout option, filtered by all available columns', function () {
        var url = '/bcms-api/widgets/server-control/f34c1135dc364f49a674a21800aadbff/options/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Key', value: 'Option 2' },
                    { field: 'DefaultValue', value: '18' },
                    { field: 'Type', value: 'Integer' }
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

            expect(result.data.items[0].key).toBe('Option 2', 'Correctly filtered key should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
        });
    });
});