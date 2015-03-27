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
            expect(result.data.version).toBe("1.11.0-dev", 'Correctly filtered version should be retrieved.');
        });
    });

    it('00001: Should login', function () {
        var url = '/login/',
            result,
            ready = false,
            onResults = function (json) {
                result = json;
                ready = true;
            };

        runs(function () {
            var options = {
                type: 'POST',
                data: JSON.stringify({ UserName: "admin", Password: "admin" }),
                cache: false,
                async: false,
                contentType: 'application/json',
                dataType: 'json',
                success: onResults,
                error: onResults,
                beforeSend: function (request) {
                    // Hack for phantomjs runner (it ignores a regularly provided contentType).
                    request.setRequestHeader("X-Content-Type", "application/json");
                },
            };

            $.ajax(url, options);
        });
        
        waitsFor(function() {
            return ready;
        }, 'The ' + url + ' timeout.');

        runs(function () {
            // Do nothing.
        });
    });
});