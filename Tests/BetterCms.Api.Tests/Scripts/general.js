var api = (function() {
    'use strict';

    var obj = {};

    obj.emptyGuid = '00000000000000000000000000000000';

    obj.get = function (url, data, onSuccess, onError) {
        obj.getSecured(url, data, null, onSuccess, onError);
    };

    obj.getSecured = function (url, data, user, onSuccess, onError) {
        var options = {
            type: 'GET',
            data: {},
            cache: false,
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            success: onSuccess,
            error: onError,
            beforeSend: function (request) {
                // Hack for phantomjs runner (it ignores a regularly provided contentType).
                request.setRequestHeader("X-Content-Type", "application/json");
            },
        };

        if (data != null) {
            options.data = "data=" + JSON.stringify(data);

            if (user != null) {
                if (options.data) {
                    options.data = options.data + "&";
                }
                options.data += "user=" + JSON.stringify(user);
            }
        }

        $.ajax(url, options);
    };

    obj.parseJsonDate = function (jsonDate) {
        return new Date(jsonDate);
        

        var offset = new Date().getTimezoneOffset() * 60000;
        var parts = /\/Date\((-?\d+)([+-]\d{2})?(\d{2})?.*/.exec(jsonDate);

        if (parts[2] == undefined)
            parts[2] = 0;

        if (parts[3] == undefined)
            parts[3] = 0;

        return new Date(+parts[1]);// + offset + parts[2] * 3600000 + parts[3] * 60000);
    };

    obj.post = function (url, data, onSuccess, onError) {
        obj.postSecured(url, data, null, onSuccess, onError);
    };

    obj.postSecured = function(url, data, user, onSuccess, onError) {
        var options = {
            type: 'POST',
            data: JSON.stringify({ Data: data || {}, User: user || {} }),
            cache: false,
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            success: onSuccess,
            error: onError,
            beforeSend: function (request) {
                // Hack for phantomjs runner (it ignores a regularly provided contentType).
                request.setRequestHeader("X-Content-Type", "application/json");
            },
        };

        $.ajax(url, options);
    };

    obj.put = function (url, data, onSuccess, onError) {
        obj.putSecured(url, data, null, onSuccess, onError);
    };

    obj.putSecured = function(url, data, user, onSuccess, onError) {
        var options = {
            type: 'PUT',
            data: JSON.stringify({ Data: data || {}, User: user || {} }),
            cache: false,
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            success: onSuccess,
            error: onError,
            beforeSend: function (request) {
                // Hack for phantomjs runner (it ignores a regularly provided contentType).
                request.setRequestHeader("X-Content-Type", "application/json");
            },
        };

        $.ajax(url, options);
    };

    obj.delete = function (url, data, onSuccess, onError) {
        obj.deleteSecured(url, data, null, onSuccess, onError);
    };

    obj.deleteSecured = function(url, data, user, onSuccess, onError) {
        var options = {
            type: 'DELETE',
            data: JSON.stringify({ Data: data || {}, User: user || {} }),
            cache: false,
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            success: onSuccess,
            error: onError,
            beforeSend: function (request) {
                // Hack for phantomjs runner (it ignores a regularly provided contentType).
                request.setRequestHeader("X-Content-Type", "application/json");
            },
        };

        $.ajax(url, options);
    };

    obj.createGuid = function() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16).toUpperCase();
        });
    };

    /**
    * Checks if all properties of base model are not null
    */
    obj.expectBasePropertiesAreNotNull = function (entity) {
        expect(entity.id).toBeDefinedAndNotNull('Id should be retrieved.');
        expect(entity.version).toBeDefinedAndNotNull('Version should be retrieved.');
        expect(entity.createdBy).toBeDefinedAndNotNull('CreatedBy should be retrieved.');
        expect(entity.lastModifiedBy).toBeDefinedAndNotNull('LastModifiedBy should be retrieved.');
        expect(entity.createdOn).toBeDefinedAndNotNull('CreatedOn should be retrieved.');
        expect(entity.lastModifiedOn).toBeDefinedAndNotNull('LastModifiedOn should be retrieved.');

        expect(entity.version).toBeGreaterThan(0, 'Version should be greater than 0.');

        var createdOn = obj.parseJsonDate(entity.createdOn);
        var lastModifiedOn = obj.parseJsonDate(entity.lastModifiedOn);
        
        expect(new Date(entity.createdOn).getTime()).toBe(new Date(createdOn.toJSON()).getTime(), 'Invalid CreatedOn date.');
        expect(new Date(entity.lastModifiedOn).getTime()).toBe(new Date(lastModifiedOn.toJSON()).getTime(), 'Invalid LastModifiedOn date.');
        expect(entity.id.length).toBe(32, 'Invalid Id.');
    };

    /**
    * Checks if validation exception is thrown
    */
    obj.expectValidationExceptionIsThrown = function (result, fieldName, errorMessage, errorCode) {
        expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
        expect(result.responseStatus).toBeDefinedAndNotNull('JSON responseStatus object should be retrieved.');
        expect(result.responseStatus.errorCode).toBe(errorCode || 'Predicate', 'Correct error code \"' + (errorCode || 'Predicate') + '\" should be retrieved.');
        expect(result.responseStatus.errors).toBeDefinedAndNotNull('responseStatus.errors should be retrieved.');
        expect(result.responseStatus.errors.length).toBeGreaterThan(0, 'responseStatus.errors should not be empty.');
        expect(result.responseStatus.errors[0].fieldName).toBe(fieldName, errorMessage || fieldName + ' should be invalidated.');
    };

    /**
    * Retrieved count of passed object properties
    */
    obj.getCountOfProperties = function (object) {
        return Object.keys(object).length;
    };

    obj.testCrud = function (runs, waitsFor, expect, itemId, url, opts) {
        var getResult, createResult, readResult, updateResult, deleteResult,
            getResultReady = false,
            createResultReady = false,
            readResultReady = false,
            updateResultReady = false,
            deleteResultReady = false,
            options = $.extend({
                getGetData: function() {
                    return {};
                },
                getPostData: function(json) {
                    return json.data;
                },
                getPutData: function(json) {
                    return json.data;
                }
            }, opts);

        // Get existing item.
        runs(function() {
            var getUrl = url + itemId,
                dataToGet = options.getGetData(getResult);
            obj.get(getUrl, dataToGet, function (json) { getResult = json; getResultReady = true; });
        });
        waitsFor(function () { return getResultReady; }, 'GET existing item timeout.');
        runs(function () {
            expect(getResult).toBeDefinedAndNotNull('CRUD scenario: get result must be not null.');
            expect(getResult.data).toBeDefinedAndNotNull('CRUD scenario: get result.data must be not null.');
        });

        // Create.
        runs(function () {
            var dataToPost = options.getPostData(getResult);
            obj.post(url, dataToPost, function (json) { createResult = json; createResultReady = true; });
        });
        waitsFor(function () { return createResultReady; }, 'POST timeout.');
        runs(function () {
            expect(createResult).toBeDefinedAndNotNull('CRUD scenario: post result must be not null.');
            expect(createResult.data).toBeDefinedAndNotNull('CRUD scenario: post result.data must be not null.');
        });

        // Read new item.
        runs(function () {
            var readUrl = url + createResult.data;
            obj.get(readUrl, null, function (json) { readResult = json; readResultReady = true; });
        });
        waitsFor(function () { return readResultReady; }, 'GET timeout.');
        runs(function () {
            expect(readResult).toBeDefinedAndNotNull('CRUD scenario: get result must be not null.');
            expect(readResult.data).toBeDefinedAndNotNull('CRUD scenario: get result.data must be not null.');
            expect(readResult.data.id).toBe(createResult.data, 'CRUD scenario: ids must be the same.');
        });

        // Update.
        runs(function () {
            var updateUrl = url + readResult.data.id,
                dataToPut = options.getPutData(readResult);
            obj.put(updateUrl, dataToPut, function (json) { updateResult = json; updateResultReady = true; });
        });
        waitsFor(function () { return updateResultReady; }, 'PUT timeout.');
        runs(function () {
            expect(updateResult).toBeDefinedAndNotNull('CRUD scenario: post result must be not null.');
            expect(updateResult.data).toBeDefinedAndNotNull('CRUD scenario: post result.data must be not null.');
            expect(updateResult.data).toBe(readResult.data.id, 'CRUD scenario: ids must be the same.');
        });

        // Delete.
        runs(function () {
            var deleteUrl = url + readResult.data.id;
            obj.delete(deleteUrl, null, function (json) { deleteResult = json; deleteResultReady = true; });
        });
        waitsFor(function () { return deleteResultReady; }, 'DELETE timeout.');
        runs(function () {
            expect(deleteResult).toBeDefinedAndNotNull('CRUD scenario: delete result must be not null.');
            expect(deleteResult.data).toBeDefinedAndNotNull('CRUD scenario: delete result.data must be not null.');
            expect(deleteResult.data).toBe(true, 'CRUD scenario: delete result.data must be true.');
        });
    }

    /**
    * Create custom matchers
    */
    beforeEach(function () {
        this.addMatchers({
            toBeDefinedAndNotNull: function () {
                return this.actual != jasmine.undefined && this.actual != null;
            }
        });
    });

    // NOTE: default timeout is 5000.
    // jasmine.getEnv().defaultTimeoutInterval = 20000;

    return obj;
})();