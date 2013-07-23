var api = (function() {
    'use strict';

    var obj = {};

    obj.get = function (url, data, onSuccess, onError) {
        var options = {
            type: 'GET',
            cache: false,
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            success: onSuccess,
            error: onError
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
        expect(entity.id).toBeDefined();
        expect(entity.version).toBeDefined();
        expect(entity.createdBy).toBeDefined();
        expect(entity.lastModifiedBy).toBeDefined();
        expect(entity.createdOn).toBeDefined();
        expect(entity.lastModifiedOn).toBeDefined();

        expect(entity.version).toBeGreaterThan(0);
        expect(entity.createdOn.length).toBe(26);
        expect(entity.lastModifiedOn.length).toBe(26);
        expect(entity.id.length).toBe(32);
    };

    return obj;
})();