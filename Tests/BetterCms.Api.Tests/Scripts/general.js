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
        expect(entity.id).not.toBeNull();
        expect(entity.version).not.toBeNull();
        expect(entity.createdBy).not.toBeNull();
        expect(entity.lastModifiedBy).not.toBeNull();
        expect(entity.createdOn).not.toBeNull();
        expect(entity.lastModifiedOn).not.toBeNull();

        expect(entity.version).toBeGreaterThan(0);
        expect(entity.createdOn.length).toBe(26);
        expect(entity.lastModifiedOn.length).toBe(26);
        expect(entity.id.length).toBe(32);
    };

    /**
    * Retrieved count of passed object properties
    */
    obj.getCountOfProperties = function (object) {
        return Object.keys(object).length;
    };

    return obj;
})();