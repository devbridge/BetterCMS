/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.user', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.dynamicContent'], function ($, bcms, modal, siteSettings, editor, dynamicContent) {
    'use strict';

    var user = {},
        selectors = {
            siteSettingsUserCreateButton: "#bcms-create-user-button",
            siteSettingsRoleCreatButton: "#bcms-create-role-button",
            usersTable: '#bcms-users-grid'

        },

        links = {
            logoutUrl: 'unknown',
            loadSiteSettingsUserUrl: null,
            loadSiteSettingsRoleUrl: null,
            loadEditUserUrl: null,
            loadEditRoleUrl: null
            
        },

        globalization = {
            confirmLogoutMessage: null,
            usersListTabTitle: null,
            usersAddNewTitle: null,
            rolesListTabTitle: null,
            rolesAddNewTitle: null
        };

    // Assign objects to module.
    user.links = links;
    user.globalization = globalization;
    user.selectors = selectors;


    user.init = function () {
    };

    user.loadSiteSettingsUsers = function () {


        var tabs = [];

        var users = new siteSettings.TabViewModel(globalization.usersListTabTitle, links.loadSiteSettingsUsersUrl, initSiteSettingsUserEvents);

        var roles = new siteSettings.TabViewModel(globalization.rolesListTabTitle, links.loadSiteSettingsRoleUrl, initSiteSettingsRoleEvents);

        tabs.push(users);

        tabs.push(roles);

        siteSettings.initContentTabs(tabs);
        editor.initialize(container, {});
    };

    function initSiteSettingsRoleEvents(container, json) {
        var html = json.Html,
            data = (json.Success == true) ? json.Data : null;

        container.html(html);

        container.find(selectors.siteSettingsRoleCreatButton).on('click', function () {
            user.openCreatRoleDialog();
        });
    }

    /**
    * Initializes site settings user list
    */
    function initSiteSettingsUserEvents(container, json) {
        var html = json.Html,
            data = (json.Success == true) ? json.Data : null;

        container.html(html);

        container.find(selectors.siteSettingsUserCreateButton).on('click', function () {
            user.openCreatUserDialog();
            //editor.addNewRow(container);
        });
    }

    user.openCreatUserDialog = function () {//(onSaveCallback) {
        modal.open({
            title: globalization.usersAddNewTitle,
            onLoad: function (childDialog) {
                dynamicContent.bindDialog(childDialog, links.loadEditRoleUrl, {
                    //contentAvailable: initializeEditTemplateForm,

                    //postSuccess: onSaveCallback
                });
            }
        });
    };

    user.openCreatRoleDialog = function () {//(onSaveCallback) {
        modal.open({
            title: globalization.rolesAddNewTitle,
            onLoad: function (childDialog) {
                dynamicContent.bindDialog(childDialog, links.loadEditRoleUrl, {
                    //contentAvailable: initializeEditTemplateForm,

                    //postSuccess: onSaveCallback
                });
            }
        });
    };

    bcms.registerInit(user.init);

    return user;
});
