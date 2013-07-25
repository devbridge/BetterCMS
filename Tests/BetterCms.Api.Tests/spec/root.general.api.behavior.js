/*jslint vars: true*/
/*global api, describe, it, expect, waits, waitsFor, runs, afterEach, spyOn, jcsvSerialize, $ */

describe('Root: General', function() {
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
            expect(result).not.toBeNull();
            expect(result.data).not.toBeNull();
            expect(result.data.version).toBe("1.0.0-dev");
        });               
    });
});