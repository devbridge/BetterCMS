var api = (function() {
    'use strict';

    var obj = {};

    obj.get = function(url, data, onSuccess, onError) {
        $.ajax(url,
            {
                data: "data=" + JSON.stringify(data),
                type: 'GET',
                cache: false,
                async: false,
                contentType: 'application/json',
                dataType: 'json',
                success: onSuccess,
                error: onError
            });
    };

    return obj;
})();