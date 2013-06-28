var getLayoutsUrl = 'http://localhost:55132/bcms-api/layouts';

asyncTest("should get layouts", function () {
    $.ajax(getLayoutsUrl,
        {
            data: {
                filter: {
                    connector: 'and',
                    where: [
                        { field: 'isDeleted', operation: 'NotEqual', value: 'true' },
                        { field: 'Name', operation: 'Equal', value: 'qwerty' }
                    ],
                    inner: [
                        {
                            connector: 'or',
                            where: [
                                { field: 'Name', operation: 'StartsWith', value: 'a' }
                            ]
                        }
                    ]
                },
                order: {
                    by: [
                        { field: 'CreatedOn' },
                        { field: 'ModifiedOn', direction: 'desc' }
                    ],
                },
                skip: 0,
                take: 5
            },
            type: 'GET',
            cache: false,
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            success: function (json) {
                ok(json, 'JSON result received.');
                equal(json.status.toLowerCase(), 'ok', 'Status ok received.');
                start();
            },
            error: function (jqxhr, textStatus, error) {
                var err = textStatus + ', ' + error;
                ok(false, 'Request Failed: ' + err);
                start();
            }
        });
});