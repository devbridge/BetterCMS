/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('root.categorizableItems.api.behavior', function () {
	'use strict';

	var constants = {
		defaultCategoryTreeId: '98fd87b4a25c4dde933c83826b6a94d7',

		categoryNodeId: 'e87bfb18cdf74fd3a5dfa2040115ed1d',
		categoryNodeName: '_0001_ - 3',
	};

	it('00500: Should get a list of categorizableItems.', function () {
		var url = '/bcms-api/categorizable-items',
            result,
            ready = false,
            data = {
                take : 5
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

			expect(result.data.totalCount).toBe(5, 'Total count should be 5.');
			expect(result.data.items.length).toBe(5, 'Returned array length should be 5.');
		});
	});
});