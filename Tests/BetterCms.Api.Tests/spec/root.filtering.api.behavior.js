/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('root.filtering.api.behavior', function () {
    'use strict';

    // Types: Guid, DateTime, String, Boolean, Int, BigInt
    // Opeators: Equal, NotEqual
    // Opeators: Less, LessOrEqual, Greater, GreaterOrEqual
    // Operators: Contains, NotContains, StartsWith, EndsWith
    var tests = [
        { id: '09010', type: 'DateTime', operation: 'Equal', value: '2013-04-01', count: 1 },
        { id: '09011', type: 'DateTime', operation: 'NotEqual', value: '2013-04-01', count: 4 },
        { id: '09012', type: 'DateTime', operation: 'Less', value: '2013-04-01', count: 1 },
        { id: '09013', type: 'DateTime', operation: 'LessOrEqual', value: '2013-04-01', count: 2 },
        { id: '09014', type: 'DateTime', operation: 'Greater', value: '2013-04-01', count: 1 },
        { id: '09015', type: 'DateTime', operation: 'GreaterOrEqual', value: '2013-04-01', count: 2 },
        { id: '09016', type: 'DateTime', operation: 'Equal', value: null, count: 2 },
        { id: '09017', type: 'DateTime', operation: 'NotEqual', value: null, count: 3 },

        { id: '09020', type: 'String', operation: 'Equal', value: '0902X: AAA', count: 1 },
        { id: '09021', type: 'String', operation: 'NotEqual', value: '0902X: AAA', count: 6 },
        { id: '09022', type: 'String', operation: 'Less', value: '0902X: BBB', count: 3 },
        { id: '09023', type: 'String', operation: 'LessOrEqual', value: '0902X: BBB', count: 4 },
        { id: '09024', type: 'String', operation: 'Greater', value: '0902X: AAA BBB CCC', count: 4 },
        { id: '09025', type: 'String', operation: 'GreaterOrEqual', value: '0902X: AAA BBB CCC', count: 5 },
        { id: '09026', type: 'String', operation: 'Contains', value: 'AAA', count: 4 },
        { id: '09027', type: 'String', operation: 'NotContains', value: 'AAA', count: 3 },
        { id: '09028', type: 'String', operation: 'StartsWith', value: '0902X: AAA', count: 3 },
        { id: '09029', type: 'String', operation: 'EndsWith', value: 'CCC', count: 2 },
        { id: '09030', type: 'String-null', operation: 'NotContains', value: '1', count: 2 },
        { id: '09030', type: 'String-null', operation: 'Equal', value: null, count: 1 },
        { id: '09031', type: 'String-null', operation: 'NotEqual', value: null, count: 2 },

        { id: '09040', type: 'Boolean', operation: 'Equal', value: 'true', count: 3 },
        { id: '09041', type: 'Boolean', operation: 'NotEqual', value: 'true', count: 2 },

        { id: '09050', type: 'Int', operation: 'Equal', value: '10000', count: 2 },
        { id: '09051', type: 'Int', operation: 'NotEqual', value: '10000', count: 3 },
        { id: '09052', type: 'Int', operation: 'Less', value: '10000', count: 2 },
        { id: '09053', type: 'Int', operation: 'LessOrEqual', value: '10000', count: 4 },
        { id: '09054', type: 'Int', operation: 'Greater', value: '10000', count: 1 },
        { id: '09055', type: 'Int', operation: 'GreaterOrEqual', value: '10000', count: 3 },

        { id: '09060', type: 'BigInt', operation: 'Equal', value: '10000', count: 2 },
        { id: '09061', type: 'BigInt', operation: 'NotEqual', value: '10000', count: 3 },
        { id: '09062', type: 'BigInt', operation: 'Less', value: '10000', count: 2 },
        { id: '09063', type: 'BigInt', operation: 'LessOrEqual', value: '10000', count: 4 },
        { id: '09064', type: 'BigInt', operation: 'Greater', value: '10000', count: 1 },
        { id: '09065', type: 'BigInt', operation: 'GreaterOrEqual', value: '10000', count: 3 }
    ];

    for (var i = 0; i < tests.length; i++) {
        createTest(9000 + i, tests[i]);
    }
    
    function createTest(nr, test) {
        var message = createMessage(test);

        it(message, function () {
            var url = null,
                columnName = null,
                data = {
                    take: 1,
                    filter: {
                        where: []
                    }
                },
                result,
                ready = false;

            // Get URL and filtering data
            switch (test.type) {
                case 'DateTime':
                    url = '/bcms-api/pages';
                    columnName = 'PublishedOn';
                    data.filter.where.push({ field: 'Title', operation: 'StartsWith', value: '0901X: ' });
                    data.includeUnpublished = true;
                    break;
                case 'BigInt':
                    url = '/bcms-api/images';
                    data.folderId = 'cb9dd0113955446c8415a20600f8bc96';
                    columnName = 'FileSize';
                    break;
                case 'Int':
                    url = '/bcms-api/tags';
                    data.filter.where.push({ field: 'Name', operation: 'StartsWith', value: '0905X: ' });
                    columnName = 'Version';
                    break;
                case 'String':
                    url = '/bcms-api/tags';
                    data.filter.where.push({ field: 'Name', operation: 'StartsWith', value: '0902X: ' });
                    columnName = 'Name';
                    break;
                case 'String-null':
                    url = '/bcms-api/authors';
                    data.filter.where.push({ field: 'Name', operation: 'StartsWith', value: '0902X: ' });
                    columnName = 'ImageCaption';
                    break;
                case 'Boolean':
                    url = '/bcms-api/images';
                    columnName = 'IsArchived';
                    data.includeArchived = true;
                    data.folderId = '902c287b9eef4de18975a20601052b9a';
                    break;
            }

            // Construct data filter
            expect(url).toBeDefinedAndNotNull('url value should be assigned');
            expect(columnName).toBeDefinedAndNotNull('columnName value should be assigned');

            var filter = {
                field: columnName,
                operation: test.operation
            };
            if (test.value) {
                filter.value = test.value;
            }
            data.filter.where.push(filter);

            // Run
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
                expect(result.data.totalCount).toBe(test.count, 'Total count should be ' + test.count + '.');
                expect(result.data.items.length).toBe(1, 'Returned array length should be 1.');
            });
        });
    }

    function createMessage(testItem) {
        var message = testItem.id + ': Should get a list, filtered by: {Type: ' + testItem.type + ', Operation: ' + testItem.operation + '';

        if (!testItem.value) {
            message += ', Value: null';
        }

        message += '}';

        return message;
    }
});