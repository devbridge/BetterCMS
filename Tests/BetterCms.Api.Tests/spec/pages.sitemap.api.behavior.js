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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.length).toBeGreaterThan(0);

            var rootFound = false;
            for (var i = 0; i < result.data.length; i++) {
                if (result.data[i].title == constants.rootTitle) {
                    var rootNode = result.data[i];
                    rootFound = true;

                    api.expectBasePropertiesAreNotNull(rootNode);
                    expect(rootNode.parentId).toBeUndefined();
                    expect(rootNode.url).toBe(constants.rootUrl);
                    expect(rootNode.displayOrder).toBeDefined();
                    expect(rootNode.childrenNodes).toBeDefined();
                    expect(rootNode.childrenNodes.length).toBe(2);

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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.length).toBe(2);

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
            expect(result).toBeDefined();
            expect(result.data).toBeDefined();
            expect(result.data.items).toBeDefined();
            expect(result.data.totalCount).toBe(3);
            expect(result.data.items.length).toBe(2);
            
            expect(result.data.items[0].title).toBe('_Tree_2_');
            expect(result.data.items[1].title).toBe('_Tree_1_1_1_');

            var node = result.data.items[1];
            api.expectBasePropertiesAreNotNull(node);
            expect(node.parentId).toBe(constants.child11Id);
            expect(node.url).toBe(constants.child111Url);
            expect(node.displayOrder).toBeDefined();
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
            expect(result).toBeDefined();

            var node = result.data;
            expect(node).toBeDefined();
            api.expectBasePropertiesAreNotNull(node);
            expect(node.title).toBe(constants.child11Title);
            expect(node.parentId).toBe(constants.child1Id);
            expect(node.url).toBe(constants.child11Url);
            expect(node.displayOrder).toBeDefined();
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
        expect(node.childrenNodes).toBeDefined();
        expect(node.childrenNodes.length).toBe(childrenCount);
    }
});