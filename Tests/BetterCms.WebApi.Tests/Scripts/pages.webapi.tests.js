var getpPageByIdUrl = 'http://localhost:55132/bcms-api/pages/getpagebyid';
var getPagesUrl = 'http://localhost:55132/bcms-api/pages/getpages';
var pageId = '3F2504E0-4F89-11D3-9A0C-0305E82C3301';

asyncTest("should get page by id", function () {
    $.ajax(getpPageByIdUrl,
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

asyncTest("should get pages", function () {
    $.ajax(getPagesUrl,
        {
            data: {
                filter: {
                    connector: 'and',
                    filterItems: [
                        { field: 'CreatedOn', value: '\/Date(1224043200000)\/', operation: 'Greater' },
                        { field: 'Title', value: 'Africa', operation: 'NotEqual' }
                    ],
                    innerFilterItems: [
                        {
                            connector: 'or',
                            filterItems: [
                                { field: 'Title', value: 'It', operation: 'StartsWith' },
                                { field: 'Title', value: 'Af', operation: 'StartsWith' },
                                { field: 'Title', value: 'na', operation: 'EndsWith' }
                            ]
                        }
                    ]
                },
                order: {
                    orderItems: [
                        { field: 'CreatedOn' },
                        { field: 'Title', direction: 'desc' }
                    ],
                },
                startItemNumber: 3,
                itemsCount: 5
            },
            type: 'GET',
            cache: false,
            async: false,
            jsonpCallback: 'jsonCallback',
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