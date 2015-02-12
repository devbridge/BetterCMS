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
            ready = false,
            data = {
                includeChildContentsOptions: true,
                includeCategories: true
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

            var widget = result.data;
            expect(widget).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(widget);

            var substring = '_0004_Html_Widget_2 HTML';
            expect(widget.name).toBe('_0004_Html_Widget_2', 'Correctly filtered name should be retrieved.');
            expect(widget.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(widget.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(widget.publishedByUser).toBe('Better CMS test user', 'Correctly filtered publishedByUser should be retrieved.');            
            expect(widget.customCss).toBe('custom css', 'Correctly filtered customCss should be retrieved.');
            expect(widget.useCustomCss).toBe(true, 'Correctly filtered useCustomCss should be retrieved.');
            expect(widget.html.substring(0, substring.length)).toBe(substring, 'Correctly filtered html should be retrieved.');
            expect(widget.useHtml).toBe(true, 'Correctly filtered useHtml should be retrieved.');
            expect(widget.customJavaScript).toBe("console.log('test')", 'Correctly filtered customJavaScript should be retrieved.');
            expect(widget.useCustomJavaScript).toBe(true, 'Correctly filtered useCustomJavaScript should be retrieved.');

            expect(result.categories).toBeDefinedAndNotNull('Correct categories should be retrieved.');
            expect(result.categories.length).toBe(1, 'Correct categories.length should be retrieved.');

            var category = result.categories[0];

            expect(category.id).toBeDefinedAndNotNull('categoryId should be retrieved.');
            expect(category.name).toBe('Category for _0004_Html_Widget_2', 'Correctly filtered categoryName should be retrieved.');

            expect(result.childContentsOptionValues).toBeDefinedAndNotNull('Correct childContentsOptionValues should be retrieved.');
            expect(result.childContentsOptionValues.length).toBe(2, 'Correct childContentsOptionValues.length should be retrieved.');

            var child = result.childContentsOptionValues[1];
            expect(child.assignmentIdentifier).toBe('fde11eb4f67741e089ff37de5ad73fab', 'Correctly filtered childContentsOptionValues[1].assignmentIdentifier should be retrieved');
            expect(child.optionValues).toBeDefinedAndNotNull('Correctly filtered childContentsOptionValues[1].optionValues should be retrieved');
            expect(child.optionValues.length).toBe(2, 'The length of childContentsOptionValues[1].optionValues array should be 2');
            expect(child.optionValues[1].key).toBe('O3', 'Correctly filtered childContentsOptionValues[1].optionValues[1].key should be retrieved');
            expect(child.optionValues[1].value).toBe('902c287b-9eef-4de1-8975-a20601052b9a', 'Correctly filtered childContentsOptionValues[1].optionValues[1].value should be retrieved');
            expect(child.optionValues[1].defaultValue).toBe('0dbf035e-a1b8-4fe1-ba61-a20500fb8491', 'Correctly filtered childContentsOptionValues[1].optionValues[1].defaultValue should be retrieved');
            expect(child.optionValues[1].type).toBe('Custom', 'Correctly filtered childContentsOptionValues[1].optionValues[1].type should be retrieved');
            expect(child.optionValues[1].useDefaultValue).toBe(false, 'Correctly filtered childContentsOptionValues[1].optionValues[1].useDefaultValue should be retrieved');
            expect(child.optionValues[1].customTypeIdentifier).toBe('media-images-folder', 'Correctly filtered childContentsOptionValues[1].optionValues[1].customTypeIdentifier should be retrieved');

            child = result.childContentsOptionValues[0];
            expect(child.assignmentIdentifier).toBe('f156028886d645d18ba92bb6dea3a96f', 'Correctly filtered childContentsOptionValues[0].assignmentIdentifier should be retrieved');
            expect(child.optionValues).toBeDefinedAndNotNull('Correctly filtered childContentsOptionValues[0].optionValues should be retrieved');
            expect(child.optionValues.length).toBe(1, 'The length of childContentsOptionValues[0].optionValues array should be 1');
            expect(child.optionValues[0].key).toBe('O1', 'Correctly filtered childContentsOptionValues[0].optionValues[0].key should be retrieved');
            expect(child.optionValues[0].value).toBe('V1', 'Correctly filtered childContentsOptionValues[0].optionValues[0].value should be retrieved');
            expect(child.optionValues[0].defaultValue).toBeNull('Correctly filtered childContentsOptionValues[0].optionValues[0].defaultValue should be retrieved');
            expect(child.optionValues[0].type).toBe('Text', 'Correctly filtered childContentsOptionValues[0].optionValues[0].type should be retrieved');
            expect(child.optionValues[0].useDefaultValue).toBe(false, 'Correctly filtered childContentsOptionValues[0].optionValues[0].useDefaultValue should be retrieved');
            expect(child.optionValues[0].customTypeIdentifier).toBeNull('Correctly filtered childContentsOptionValues[0].optionValues[0].customTypeIdentifier should be retrieved');
        });
    });
    
    it('01202: Should get a server control widget by id', function () {
        var url = '/bcms-api/widgets/server-control/3ac115dfc5f34f148141a205009162cd',
            result,
            ready = false,
            data = {
                includeCategories: true
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

            var widget = result.data;
            expect(widget).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(widget);
            expect(widget.name).toBe('_0004_Server_Widget_1', 'Correctly filtered name should be retrieved.');
            expect(widget.widgetUrl).toBe('~/Views/Widgets/TestWidget.cshtml', 'Correctly filtered widgetUrl should be retrieved.');
            expect(widget.previewUrl).toBe('http://www.devbridge.com/Content/styles/images/responsive/logo.png', 'Correctly filtered previewUrl should be retrieved.');
            expect(widget.isPublished).toBe(true, 'Correctly filtered isPublished should be retrieved.');
            expect(widget.publishedOn).toBeDefinedAndNotNull('publishedOn should be retrieved.');
            expect(widget.publishedByUser).toBe('Better CMS test user', 'Correctly filtered publishedByUser should be retrieved.');

            expect(result.categories).toBeDefinedAndNotNull('Correct categories should be retrieved.');
            expect(result.categories.length).toBe(1, 'Correct categories.length should be retrieved.');

            var category = result.categories[0];

            expect(category.id).toBeDefinedAndNotNull('categoryId should be retrieved.');
            expect(category.name).toBe('Category for _0004_Server_Widget_1', 'Correctly filtered categoryName should be retrieved.');
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
                ]
            },
            filterByCategoriesConnector: 'AND',
            filterByCategories: ['1d8dbfbc-e4bf-46c2-acb7-a20700ac186a'],
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
            // data.filter.where.length + 1 <-- Because field WidgetType and Categories cannnot be filtered by
            expect(data.filter.where.length + 2).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties cound should be equal to filterting parameters count.');
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
    
    it('01205: Should get server control widget options by widget id', function () {
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
    
    it('01206: Should get a list with one server control widget option, filtered by all available columns', function () {
        var url = '/bcms-api/widgets/server-control/f34c1135dc364f49a674a21800aadbff/options/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Key', value: 'Option 2' },
                    { field: 'DefaultValue', value: '18' },
                    { field: 'Type', value: 'Integer' },
                    { field: 'CustomTypeIdentifier', value: null }
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
    
    it('01207: Should get html content widget options by widget id', function () {
        var url = '/bcms-api/widgets/html-content/da359cd105b94da0b62ba2ee0108f4b0/options',
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

            expect(result.data.items[0].defaultValue).toBe('01207', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].defaultValue).toBe('1207', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].defaultValue).toBe('1207.5', 'Correctly filtered items[2].key should be retrieved.');

            expect(result.data.items[0].type).toBe('Text', 'Correctly filtered items[0].key should be retrieved.');
            expect(result.data.items[1].type).toBe('Integer', 'Correctly filtered items[1].key should be retrieved.');
            expect(result.data.items[2].type).toBe('Float', 'Correctly filtered items[2].key should be retrieved.');
        });
    });

    it('01208: Should get a list with one html content widget option, filtered by all available columns', function () {
        var url = '/bcms-api/widgets/html-content/3b365b64298546a7988da2ee0109ae73/options/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Key', value: 'Option 2' },
                    { field: 'DefaultValue', value: '1208' },
                    { field: 'Type', value: 'Integer' },
                    { field: 'CustomTypeIdentifier', value: null }
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

    it('01209: Should test CRUD for html content widgets.', function () {
        api.testCrud(runs, waitsFor, expect, "fa0cbcfb96454fcfa576a205009119c8", "/bcms-api/widgets/html-content/", {
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });

    it('01210: Should test CRUD for server control widgets.', function () {
        api.testCrud(runs, waitsFor, expect, "3ac115dfc5f34f148141a205009162cd", "/bcms-api/widgets/server-control/", {
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });
});