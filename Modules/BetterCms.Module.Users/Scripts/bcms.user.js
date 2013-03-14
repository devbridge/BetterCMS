/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.user', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.dynamicContent', 'bcms.role'], function ($, bcms, modal, siteSettings, editor, dynamicContent, role) {
    'use strict';

    var user = {},
        selectors = {
            siteSettingsUserCreateButton: "#bcms-create-user-button",
            siteSettingsRoleCreatButton: "#bcms-create-role-button",
            usersTable: '#bcms-users-grid',

            roleForm: '#bcms-role-form',
            roleRowEditButtons: '.bcms-grid-item-edit-button',
            roleParentRow: 'tr:first'
        },

        links = {
            logoutUrl: 'unknown',
            loadSiteSettingsUserUrl: null,
            loadSiteSettingsRoleUrl: null,
            loadEditUserUrl: null,
            loadCreatRoleUrl: null,
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

        var roles = new siteSettings.TabViewModel(role.globalization.rolesListTabTitle, role.links.loadSiteSettingsRoleUrl, role.initSiteSettingsRoleEvents);

        tabs.push(users);

        tabs.push(roles);

        siteSettings.initContentTabs(tabs);
        editor.initialize(container, {});
    };

    /*roles.initSiteSettingsRoleEvents = function(container, json) {
        var html = json.Html,
            data = (json.Success == true) ? json.Data : null;

        container.html(html);

        container.find(selectors.siteSettingsRoleCreatButton).on('click', function() {
            user.openCreatRoleDialog();
        });
    };*/

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
                dynamicContent.bindDialog(childDialog, links.loadEditUserUrl, {
                    //contentAvailable: initializeEditTemplateForm,

                    //postSuccess: onSaveCallback
                });
            }
        });
    };


    bcms.registerInit(user.init);

    return user;
});
