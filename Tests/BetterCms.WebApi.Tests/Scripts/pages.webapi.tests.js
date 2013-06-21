var url = 'http://localhost:55132/bcms-api/pages/getpagebyid';
var pageId = '3F2504E0-4F89-11D3-9A0C-0305E82C3301';

asyncTest("should get page by id", function () {
    $.ajax(url,
        {
            data: { pageId: pageId },
            type: 'GET',
            cache: false,
            async: false,
            jsonpCallback: 'jsonCallback',
            contentType: 'application/json',
            dataType: 'json',
            success: function(json) {
                ok(json, 'JSON result received.');
                equal(json.status.toLowerCase(), 'ok', 'Status ok received.');
                equal(json.data.id.toLowerCase(), pageId.toLowerCase(), 'The same page id received.');
                start();
            },
            error: function(jqxhr, textStatus, error) {
                var err = textStatus + ', ' + error;
                ok(false, 'Request Failed: ' + err);
                start();
            }
        });
});