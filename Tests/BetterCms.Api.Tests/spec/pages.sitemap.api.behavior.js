/*jslint vars: true*/
/*global describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, $*/

describe('pages.sitemap.api.behavior', function () {
    'use strict';

    var constants = {
        defaultSitemapId: '17abfee95ae6470c92e1c2905036574b',
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
        child2Url: '/root/2/',
        
        sitemapId: 'c993894775fe4e64bca1a2b600b45019',
        languageId: '5dc9f98f97894201b73ba2b600b5e2f9',
        nodeTitleInLanguage: '_014xx_1 in language 1',
        nodeUrlInLanguage: '/014xx-1-in-language-1/',
        pageIdWithLanguage: '9ea01e0b6a5d44d88478a2b600b65f34',
    };

    it('01400: [Obsolete] Should get a list of sitemaps.', function () {
        var url = '/bcms-api/sitemap-trees/',
            result,
            ready = false,
            data = {
                filter: {
                    connector: 'or',
                    where: []
                },
                order: {
                    by: [{ field: 'Title', direction: 'desc' }]
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

    it('01401: [Obsolete] Should get the full sitemap tree.', function () {
        var url = '/bcms-api/sitemap-tree/' + constants.defaultSitemapId,
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
                    expect(rootNode.parentId).toBeNull('parentId should be null.');
                    expect(rootNode.url).toBe(constants.rootUrl, 'Correctly filtered root node url should be retrieved.');
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

            expect(rootFound).toBe(true, 'Root node should be retrieved.');
        });
    });
    
    it('01402: [Obsolete] Should get sitemap tree, filtered by node.', function () {
        var url = '/bcms-api/sitemap-tree/' + constants.defaultSitemapId,
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

    it('01403: [Obsolete] Should get sitemap tree, filtered by language.', function () {
        var url = '/bcms-api/sitemap-tree/' + constants.sitemapId,
            result,
            ready = false,
            data = {
                languageId: constants.languageId
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
            expect(result.data.length).toBeGreaterThan(0);

            var rootFound = false;
            for (var i = 0; i < result.data.length; i++) {
                if (result.data[i].title == constants.nodeTitleInLanguage) {
                    var rootNode = result.data[i];
                    rootFound = true;

                    api.expectBasePropertiesAreNotNull(rootNode);
                    expect(rootNode.parentId).toBeNull('parentId should be null.');
                    expect(rootNode.pageId).toBeDefinedAndNotNull('pageId should be not null.');
                    expect(rootNode.pageId).toBe(constants.pageIdWithLanguage, 'pageId should be not null.');
                    expect(rootNode.title).toBeDefinedAndNotNull('title should be retrieved.');
                    expect(rootNode.url).toBe(constants.nodeUrlInLanguage, 'Correctly filtered root node url should be retrieved.');
                    expect(rootNode.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
                }
            }

            expect(rootFound).toBe(true, 'Root node should be retrieved.');
        });
    });

    it('01404: [Obsolete] Should get a list of sitemap nodes.', function () {
        var url = '/bcms-api/sitemap-nodes/' + constants.defaultSitemapId,
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
            expect(result.data.items).not.toBeNull('JSON data.items object should be retrieved.');
            expect(result.data.totalCount).toBe(3, 'Total count should be 3.');
            expect(result.data.items.length).toBe(2, 'Returned array length should be 2.');
            
            expect(result.data.items[0].title).toBe('_Tree_2_', 'Correctly filtered items[0].title should be retrieved.');
            expect(result.data.items[1].title).toBe('_Tree_1_1_1_', 'Correctly filtered items[1].title should be retrieved.');

            var node = result.data.items[1];
            api.expectBasePropertiesAreNotNull(node);
            expect(node.parentId).toBe(constants.child11Id, 'Correctly filtered parentId should be retrieved.');
            expect(node.url).toBe(constants.child111Url, 'Correctly filtered url should be retrieved.');
            expect(node.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
        });
    });

    it('01405: [Obsolete] Should get a list with one sitemap node, filtered by all available columns.', function () {
        var url = '/bcms-api/sitemap-nodes/' + constants.defaultSitemapId,
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
                    { field: 'DisplayOrder', value: 0 },
                    { field: 'PageId', value: null },
                    { field: 'PageIsPublished', value: false },
                    { field: 'PageLanguageId', value: null },
                    { field: 'Macro', value: null }
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

            expect(result.data.items[0].id).toBe('390d4ac846804fa4ab18a20700aae5e6', 'Correctly filtered id should be retrieved.');

            // Check if model properties count didn't changed. If so - update current test filter and another tests.
            expect(data.filter.where.length).toBe(api.getCountOfProperties(result.data.items[0]), 'Retrieved result properties count should be equal to filter parameters count.');
        });
    });

    it('01406: [Obsolete] Should get a sitemap node by id.', function () {
        var url = '/bcms-api/sitemap-node/' + constants.child11Id,
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
            expect(node.title).toBe(constants.child11Title, 'Correctly filtered title should be retrieved.');
            expect(node.parentId).toBe(constants.child1Id, 'Correctly filtered parentId should be retrieved.');
            expect(node.url).toBe(constants.child11Url, 'Correctly filtered url should be retrieved.');
            expect(node.displayOrder).toBeDefinedAndNotNull('displayOrder should be retrieved.');
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

        expect(childFound).toBe(true, 'Correct child with title \"' + title + '\" should be retrieved.');

        return childNode;
    }
    
    function expectTreeNodePropertiesAreCorrect(node, parentId, title, url, childrenCount) {
        api.expectBasePropertiesAreNotNull(node);
        expect(node.title).toBe(title, 'Correctly filtered title should be retrieved.');
        expect(node.parentId).toBe(parentId, 'Correctly filtered parentId should be retrieved.');
        expect(node.url).toBe(url, 'Correctly filtered url should be retrieved.');
        expect(node.childrenNodes).toBeDefinedAndNotNull('childrenNodes should be retrieved.');
        expect(node.childrenNodes.length).toBe(childrenCount, 'Correctly filtered count of children nodes should be retrieved.');
    }
});