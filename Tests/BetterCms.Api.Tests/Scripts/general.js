function jcsvSerialize(obj) {
    var output = [];

    for (var key in obj) {
        if (obj.hasOwnProperty(key)) {
            output.push(key + '=' + JSON.stringify(obj[key]) );
        }
    }

    return output.join('&');
};

function get(url, data, onSuccess, onError) {
    $.ajax(url,
        {
            data: data != null ? jcsvSerialize(data) : null,
            type: 'GET',
            cache: false,
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            success: onSuccess,
            error: onError
        });
}