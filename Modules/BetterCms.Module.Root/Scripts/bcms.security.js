/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.security', ['bcms.jquery', 'bcms.ko.extenders'], function ($, ko) {
    'use strict';

    var security = {},
        // Classes that are used to maintain various UI states:
        classes = {},
        // Selectors used in the module to locate DOM elements:
        selectors = {},
        links = {
            isAuthorized: null
        },
        globalization = {},
        authorizedFor = [];

    // Assign objects to module
    security.classes = classes;
    security.selectors = selectors;
    security.links = links;
    security.globalization = globalization;

    function isUserAuthorized(roles) {
        try {
            var url = $.format(links.isAuthorized, roles),
                response = $.parseJSON($.ajax({
                    type: 'POST',
                    url: url,
                    cache: false,
                    async: false
                }).responseText);
            if (response.Success === false || response.Success === true) {
                authorizedFor[roles] = response.Success;
                if (response.Success === true) {
                    return true;
                }
            }
        } catch (e) {
            console.log('Error occurred while checking if role(s) is authorized.');
        }

        return false;
    }

    security.IsAuthorized = function (accessRoleArray) {
        for (var i in accessRoleArray) {
            var role = accessRoleArray[i];
            var value = authorizedFor[role];
            if (value === true || value === false) {
                if (value === true) {
                    return true;
                }
            } else {
                if (isUserAuthorized(role)) {
                    return true;
                }
            }
        }
        return false;
    };

    function UserAccessViewModel(item) {
        this.Identity = ko.observable(item.Identity);
        this.AccessLevel = ko.observable(item.AccessLevel || 3);
    }

    security.createUserAccessViewModel = function(accessList) {
        var model = {
            UserAccessList: ko.observableArray(),
            newUser: ko.observable(''),
            addNewUser: function() {
                if (!model.newUser()) {
                    return;
                }
                model.UserAccessList.push(new UserAccessViewModel({ Identity: model.newUser() }));
                model.newUser('');
            },
            removeUser: function(userAccessViewModel) {
                model.UserAccessList.remove(userAccessViewModel);
            },
            getPropertyIndexer: function(i, propName) {
                return 'UserAccessList[' + i + '].' + propName;
            }
        };

        $.each(accessList, function(i, item) {
            model.UserAccessList.push(new UserAccessViewModel(item));
        });

        return model;
    };

    return security;
});
