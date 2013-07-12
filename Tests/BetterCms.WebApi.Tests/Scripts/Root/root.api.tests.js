module('root.api.test');

asyncTest("Should get a Better CMS current version", function () {
    $.ajax('/bcms-api/current-version/',
        {
            type: 'GET',
            cache: false,
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            
            success: function (json) {
                ok(json, 'JSON result received.');
                ok(json.data, 'Version received.');
                start();
            },
            
            error: function (jqxhr, textStatus, error) {
                var err = textStatus + ', ' + error;
                ok(false, 'Request Failed: ' + err);
                start();
            }
        });
});

asyncTest("Should get a filtered list", function () {
    $.ajax('/bcms-api/layouts/',
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
              ok(json.data, 'Version received.');
              start();
          },

          error: function (jqxhr, textStatus, error) {
              var err = textStatus + ', ' + error;
              ok(false, 'Request Failed: ' + err);
              start();
          }
      });
});