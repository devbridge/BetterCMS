var api = (function() {
    'use strict';

    var obj = {};
    
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

    /**
    * Checks if all properties of base model are not null
    */
    obj.expectBasePropertiesAreNotNull = function (entity) {
        expect(entity.id).toBeDefinedAndNotNull('Id should be retrieved');
        expect(entity.version).toBeDefinedAndNotNull('Version should be retrieved');
        expect(entity.createdBy).toBeDefinedAndNotNull('CreatedBy should be retrieved');
        expect(entity.lastModifiedBy).toBeDefinedAndNotNull('LastModifiedBy should be retrieved');
        expect(entity.createdOn).toBeDefinedAndNotNull('CreatedOn should be retrieved');
        expect(entity.lastModifiedOn).toBeDefinedAndNotNull('LastModifiedOn should be retrieved');

        expect(entity.version).toBeGreaterThan(0, 'Version should be greater than 0.');
        expect(entity.createdOn.length).toBe(26, 'Invalid CreatedOn date.');
        expect(entity.lastModifiedOn.length).toBe(26, 'Invalid LastModifiedOn date.');
        expect(entity.id.length).toBe(32, 'Invalid Id.');
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