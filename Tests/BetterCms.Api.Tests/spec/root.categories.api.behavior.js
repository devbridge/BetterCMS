/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('root.categories.api.behavior', function () {
    'use strict';

    var constants = {
        defaultCategoryTreeId: '98fd87b4a25c4dde933c83826b6a94d7',

        categoryNodeId: 'e87bfb18cdf74fd3a5dfa2040115ed1d',
        categoryNodeName: '_0001_ - 3',
    };
    
    it('00300: Should get a list of category trees.', function () {
        var url = '/bcms-api/categorytrees/',
            result,
            ready = false,
            data = {
                filter: {
                    connector: 'or',
                    where: []
                },
                order: {
                    by: [{ field: 'Name', direction: 'desc' }]
                },
                take: 2
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

            expect(result.data.totalCount).toBe(2, 'Total count should be 2.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');
        });
    });

    it('00301: Should get a category tree.', function () {
        var url = '/bcms-api/categorytrees/' + constants.defaultCategoryTreeId,
            result,
            ready = false,
            data = {
                includeAccessRules: true,
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
            expect(result.data.title).not.toBeNull('JSON data.title object should be retrieved.');
            expect(result.data.nodes).not.toBeNull('JSON data.tags object should be retrieved.');
            expect(result.data.availableFor).toBeDefined('JSON data.availableFor should be retrieved');
            expect(result.data.macro).toBeDefined('JSON data.macro should be retrieved');

        });
    });

    it('00302: Should get the full category tree.', function () {
        var url = '/bcms-api/categorytrees/' + constants.defaultCategoryTreeId + '/tree',
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
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.length).toBeGreaterThan(0);

            var rootFound = false;
            for (var i = 0; i < result.data.length; i++) {
                if (result.data[i].name == constants.categoryNodeName) {
                    var rootNode = result.data[i];
                    rootFound = true;

                    api.expectBasePropertiesAreNotNull(rootNode);
                    expect(rootNode.parentId).toBeNull('parentId should be null.');
                    expect(rootNode.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
                    expect(rootNode.childrenNodes).toBeDefinedAndNotNull('childrenNodes should be retrieved.');
                    expect(rootNode.childrenNodes.length).toBe(0, 'Returned childrenNodes array length should be 0.');
                }
            }

            expect(rootFound).toBe(true, 'Root node should be retrieved.');
        });
    });

    it('00303: Should get category tree, filtered by node.', function () {
        var url = '/bcms-api/categorytrees/' + constants.defaultCategoryTreeId + '/tree',
            result,
            ready = false,
            data = {
                nodeId: constants.categoryNodeId
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
            expect(result.data.length).toBe(0, 'Returned array length should be 0.');
// TODO: update default category tree not to have one child node '_0001_ - 3'.
//            var node = result.data[0];
//            api.expectBasePropertiesAreNotNull(node);
//            expect(node.name).toBe(constants.categoryNodeName, 'Correctly filtered title should be retrieved.');
//            expect(node.parentId).toBe(constants.categoryNodeId, 'Correctly filtered parentId should be retrieved.');
//            expect(node.childrenNodes).toBeDefinedAndNotNull('childrenNodes should be retrieved.');
//            expect(node.childrenNodes.length).toBe(0, 'Correctly filtered count of children nodes should be retrieved.');
        });
    });

    it('00304: Should test CRUD for category trees.', function () {
        api.testCrud(runs, waitsFor, expect, constants.defaultCategoryTreeId, "/bcms-api/categorytrees/", {
            getGetData: function () {
                return {
                    IncludeNodes: true,
                    includeAccessRules: false
                }
            },
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });

    it('00305: Should get a list of category nodes.', function () {
        var url = '/bcms-api/categorytrees/' + constants.defaultCategoryTreeId + '/nodes',
            result,
            ready = false,
            data = {
                filter: {
                    connector: 'or',
                    where: [
                        { field: 'ParentId', value: constants.categoryNodeId }
                    ]
                },
                order: {
                    by: [{ field: 'Name', direction: 'desc' }]
                },
                take: 2
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
// TODO: update default category tree not to have one child node '_0001_ - 3'.
            expect(result.data.totalCount).toBe(0, 'Total count should be 0.');
            expect(result.data.items.length).toBe(0, 'Returned array length should be 2.');

//            expect(result.data.items[0].name).toBe('_Tree_2_', 'Correctly filtered items[0].title should be retrieved.');
//            expect(result.data.items[1].name).toBe('_Tree_1_1_1_', 'Correctly filtered items[1].title should be retrieved.');
//
//            var node = result.data.items[1];
//            api.expectBasePropertiesAreNotNull(node);
//            expect(node.parentId).toBe(constants.child11Id, 'Correctly filtered parentId should be retrieved.');
//            expect(node.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
        });
    });

    it('00306: Should get a list with one sitemap node, filtered by all available columns.', function () {
        var url = '/bcms-api/categorytrees/' + constants.defaultCategoryTreeId + '/nodes',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: constants.categoryNodeId },
                    { field: 'CreatedOn', value: '2013-07-23 16:51:53.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2015-02-04 09:52:24.000' },
                    { field: 'LastModifiedBy', value: 'admin' },
                    { field: 'Version', value: '2' },

                    { field: 'ParentId', value: null },
                    { field: 'Name', value: constants.categoryNodeName },
                    { field: 'DisplayOrder', value: 5200 },
                    { field: 'Macro', value: null },
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

            expect(result.data.items[0].id).toBe(constants.categoryNodeId, 'Correctly filtered id should be retrieved.');
            
            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(api.getCountOfProperties(result.data.items[0])).toBe(data.filter.where.length+1, 'Retrieved result properties count should be equal to filter parameters count.');
        });
    });

    it('00307: Should get a category node by id.', function () {
        var url = '/bcms-api/categorytrees/' + constants.defaultCategoryTreeId + '/nodes/' + constants.categoryNodeId,
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

            var node = result.data;
            expect(node).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            api.expectBasePropertiesAreNotNull(node);
            expect(node.name).toBe(constants.categoryNodeName, 'Correctly filtered title should be retrieved.');
            expect(node.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
        });
    });

    it('00308: Should test CRUD for category nodes.', function () {
        api.testCrud(runs, waitsFor, expect, constants.categoryNodeId, '/bcms-api/categorytrees/' + constants.defaultCategoryTreeId + '/nodes/', {
            getGetData: function () {
                return {
                    
                }
            },
            getPostData: function (json) {
                json.data.name = api.createGuid();
                json.data.version = 0;
                return json.data;
            }
        });
    });
});