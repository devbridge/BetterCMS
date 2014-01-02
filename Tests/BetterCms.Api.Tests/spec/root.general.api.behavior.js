/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('root.general.api.behavior', function () {
    'use strict';

    it('00000: Should get a Better CMS current version', function () {
        var url = '/bcms-api/current-version/',
            result,
            ready = false;

        runs(function () {
            api.get(url, null, function(json) {
                result = json;
                ready = true;
            });
        });
        
        waitsFor(function() {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
            expect(result.data).toBeDefinedAndNotNull('JSON data object should be retrieved.');
            expect(result.data.version).toBe("1.5.0-dev", 'Correctly filtered version should be retrieved.');
        });               
    });
});