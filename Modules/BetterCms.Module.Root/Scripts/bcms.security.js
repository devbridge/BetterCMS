/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.security', ['bcms.jquery'], function($) {
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

    return security;
});
