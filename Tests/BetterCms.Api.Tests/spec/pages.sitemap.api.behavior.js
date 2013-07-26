/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('Pages: Sitemap', function () {
    'use strict';

    var constants = {
        rootId: '316138d8b3ff478981d6a20500a389f3',
        rootTitle: '_Tree_Root_',
        rootUrl: '/root/',
        child1Id: 'cf747da09151453393efa20500a389f6',
        child1Title: '_Tree_1_',
        child1Url: '/root/1/',
        child11Title: '_Tree_1_1_',
        child11Url: '/root/1/1/',
        child11Id: 'b35c14e28c774aa999e4a20500a389f7',
        child111Title: '_Tree_1_1_1_',
        child111Url: '/root/1/1/1/',
        child2Title: '_Tree_2_',
        child2Url: '/root/2/'
    };

    it('01400: Should get full sitemap tree', function () {
        var url = '/bcms-api/sitemap-tree/',
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
                if (result.data[i].title == constants.rootTitle) {
                    var rootNode = result.data[i];
                    rootFound = true;

                    api.expectBasePropertiesAreNotNull(rootNode);
                    expect(rootNode.parentId).toBeNull();
                    expect(rootNode.url).toBe(constants.rootUrl);
                    expect(rootNode.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
                    expect(rootNode.childrenNodes).toBeDefinedAndNotNull('childrenNodes should be retrieved.');
                    expect(rootNode.childrenNodes.length).toBe(2, 'Returned childrenNodes array length should be 2.');

                    // /root/1/
                    var child1 = findTreeChild(rootNode.childrenNodes, rootNode.id, constants.child1Title, constants.child1Url, 3);
                    // /root/1/1/
                    var child11 = findTreeChild(child1.childrenNodes, child1.id, constants.child11Title, constants.child11Url, 1);
                    // /root/1/1/1/
                    findTreeChild(child11.childrenNodes, child11.id, constants.child111Title, constants.child111Url, 0);
                }
            }

            expect(rootFound).toBe(true);
        });
    });
    
    it('01401: Should get sitemap tree, filtered by node', function () {
        var url = '/bcms-api/sitemap-tree/',
            result,
            ready = false,
            data = {
                nodeId: constants.rootId
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
            expect(result.data.length).toBe(2, 'Returned array length should be 2.');

            var child1 = result.data[0];
            expectTreeNodePropertiesAreCorrect(child1, constants.rootId, constants.child1Title, constants.child1Url, 3);

            var child11 = findTreeChild(child1.childrenNodes, child1.id, constants.child11Title, constants.child11Url, 1);
            findTreeChild(child11.childrenNodes, child11.id, constants.child111Title, constants.child111Url, 0);

            var child2 = result.data[1];
            expectTreeNodePropertiesAreCorrect(child2, constants.rootId, constants.child2Title, constants.child2Url, 0);
        });
    });

    it('01402: Should get a list of sitemap nodes', function () {
        var url = '/bcms-api/sitemap-nodes/',
            result,
            ready = false,
            data = {
                filter: {
                    connector: 'or',
                    where: [
                        {field: 'ParentId', value: constants.rootId},
                        {field: 'ParentId', value: constants.child11Id}
                    ]
                },
                order: {
                    by: [{field: 'Title', direction: 'desc'}]
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
            expect(result.data.items).not.toBeNull('JSON data.items object should be retrieved');
            expect(result.data.totalCount).toBe(3, 'Total count should be 3.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');
            
            expect(result.data.items[0].title).toBe('_Tree_2_');
            expect(result.data.items[1].title).toBe('_Tree_1_1_1_');

            var node = result.data.items[1];
            api.expectBasePropertiesAreNotNull(node);
            expect(node.parentId).toBe(constants.child11Id);
            expect(node.url).toBe(constants.child111Url);
            expect(node.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
        });
    });

    it('01403: Should get a sitemap node by id', function () {
        var url = '/bcms-api/sitemap-nodes/' + constants.child11Id,
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
            expect(node.title).toBe(constants.child11Title);
            expect(node.parentId).toBe(constants.child1Id);
            expect(node.url).toBe(constants.child11Url);
            expect(node.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
        });
    });
    
    it('01404: Should get a list with one siemap node, filtered by all available columns', function () {
        var url = '/bcms-api/sitemap-nodes/',
            result,
            ready = false;

        var data = {
            filter: {
                where: [
                    { field: 'Id', value: '390d4ac846804fa4ab18a20700aae5e6' },
                    { field: 'CreatedOn', value: '2013-07-26 10:22:13.000' },
                    { field: 'CreatedBy', value: 'Better CMS test user' },
                    { field: 'LastModifiedOn', value: '2013-07-26 10:22:13.000' },
                    { field: 'LastModifiedBy', value: 'Better CMS test user' },
                    { field: 'Version', value: '1' },

                    { field: 'ParentId', value: '4f32940497ce4df199d1a20700aae5d8' },
                    { field: 'Title', value: '01404' },
                    { field: 'Url', value: '/01404/01404/' },
                    { field: 'DisplayOrder', value: 0 }
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

            expect(result.data.items[0].id).toBe('390d4ac846804fa4ab18a20700aae5e6');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]));
        });
    });

    function findTreeChild(items, parentId, title, url, childrenCount) {
        var childFound = false,
            childNode = null;

        for (var i = 0; i < items.length; i++) {
            if (items[i].title == title) {
                childFound = true;
                childNode = items[i];

                expectTreeNodePropertiesAreCorrect(childNode, parentId, title, url, childrenCount);
            }
        }

        expect(childFound).toBe(true);

        return childNode;
    }
    
    function expectTreeNodePropertiesAreCorrect(node, parentId, title, url, childrenCount) {
        api.expectBasePropertiesAreNotNull(node);
        expect(node.title).toBe(title);
        expect(node.parentId).toBe(parentId);
        expect(node.url).toBe(url);
        expect(node.childrenNodes).toBeDefinedAndNotNull('childrenNodes should be retrieved.');
        expect(node.childrenNodes.length).toBe(childrenCount);
    }
});