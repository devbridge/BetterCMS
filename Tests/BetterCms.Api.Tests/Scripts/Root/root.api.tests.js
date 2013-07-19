module('root.api.test');

asyncTest("_0000: Should get a Better CMS current version.", function () {
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

asyncTest("_0001: Should get a filtered, sorted and paged list.", function () {
    $.ajax('/bcms-api/layouts/',
      {
          data: JSON.stringify({
                  filter: {
                      connector: 'and',
                      where: [
                          { field: 'isDeleted', operation: 'NotEqual', value: 'true' },
                          { field: 'name', operation: 'StartsWith', value: '_0001_' }
                      ],
                      inner: [
                          {
                              connector: 'or',
                              where: [
                                  { field: 'Name', operation: 'Contains', value: 'Layout1' },
                                  { field: 'Name', operation: 'NotContains', value: 'NOT_FOUND' }
                              ]
                          }
                      ]
                  },
                  order: {
                      by: [
                          { field: 'Name' },
                          { field: 'ModifiedOn', Direction: 'desc' }
                      ]
                  },
                  skip: 2,
                  take: 2
              })
          ,
          type: 'GET',
          cache: false,
          async: false,
          contentType: 'application/json',
          dataType: 'json',

          success: function (json) {
              ok(json, 'JSON result received.');
              ok(json.data, 'Version received.');
              ok(json.data.totalCount === 4, '4 items totally found.');
              ok(json.data.items.length === 2, '2 items returned.');
              ok(json.data.items[0].name === '_0001_Layout3', '');
              ok(json.data.items[1].name === '_0001_Layout4', '');
              start();
          },

          error: function (jqxhr, textStatus, error) {
              var err = textStatus + ', ' + error;
              ok(false, 'Request Failed: ' + err);
              start();
          }
      });
});

asyncTest("_0002: Should get a filtered, sorted and paged list.", function () {
    var request = {
        filter: {
            connector: 'and',
            where: [
                { field: 'isDeleted', operation: 'NotEqual', value: 'true' },
                { field: 'name', operation: 'StartsWith', value: '_0001_' }
            ],
            inner: [
                {
                    connector: 'or',
                    where: [
                        { field: 'Name', operation: 'Contains', value: 'Layout1' },
                        { field: 'Name', operation: 'NotContains', value: 'NOT_FOUND' }
                    ]
                }
            ]
        },
        order: {
            by: [
                { field: 'Name' },
                { field: 'ModifiedOn', Direction: 'desc' }
            ]
        },
        skip: 2,
        take: 2
    };
    
    $.ajax('/bcms-api/tags/',
      {
          data: request,
          type: 'POST',
          cache: false,
          async: false,        
          contentType: "application/json", 
          dataType: "json", 
          success: function (json) {
              ok(json, 'JSON result received.');
              ok(json.data, 'Version received.');
              ok(json.data.totalCount === 4, '4 items totally found.');
              ok(json.data.items.length === 2, '2 items returned.');
              ok(json.data.items[0].name === '_0001_Layout3', '');
              ok(json.data.items[1].name === '_0001_Layout4', '');
              start();
          },

          error: function (jqxhr, textStatus, error) {
              var err = textStatus + ', ' + error;
              ok(false, 'Request Failed: ' + err);
              start();
          }
      });
});