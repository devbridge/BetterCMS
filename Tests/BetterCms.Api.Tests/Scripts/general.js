var api = (function() {
    'use strict';

    var obj = {};

    obj.emptyGuid = '00000000000000000000000000000000';

    obj.get = function (url, data, onSuccess, onError) {
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
    obj.expectValidationExceptionIsThrown = function (result, fieldName, errorMessage) {
        expect(result).toBeDefinedAndNotNull('JSON object should be retrieved.');
        expect(result.responseStatus).toBeDefinedAndNotNull('JSON responseStatus object should be retrieved.');
        expect(result.responseStatus.errorCode).toBe('Predicate', 'Correct error code \"Predicate\" should be retrieved.');
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

    return obj;
})();