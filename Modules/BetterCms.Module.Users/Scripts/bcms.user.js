/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.user', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.dynamicContent'], function ($, bcms, modal, siteSettings, editor, dynamicContent) {
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

        container.find(selectors.roleRowEditButtons).on('click', function () {
            editRole(container, $(this));
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
                dynamicContent.bindDialog(childDialog, links.loadEditUserUrl, {
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
                dynamicContent.bindDialog(childDialog, links.loadCreatRoleUrl, {
                    contentAvailable: initializeEditRoleForm

                    //postSuccess: onSaveCallback
                });
            }
        });
    };

    function initializeEditRoleForm() {
        var dialog = siteSettings.getModalDialog(),
            container = dialog.container;
        var form = container.find(selectors.roleForm);

        form.on('submit', function (event) {
            event.preventDefault();
            // searchTemplates(form);
            return false;
        });
    }

    /**
    * Calls function, which opens dialog for a template editing.
    */
    function editRole(container, self) {
        var row = self.parents(selectors.roleParentRow),
                id = row.data('id');

        /*role.editRole(id, function (data) {
            if (data.Data != null) {
                //setRoleFields(row, data);
                grid.showHideEmptyRow(container);
            }
        });*/
    };

    /*
    * Open a template edit dialog by the specified tempalte type.
    */
    /*user.editRole = function (templateId, onSaveCallback) {
        //template.openEditTemplateDialog(templateId, onSaveCallback);

    };*/

    bcms.registerInit(user.init);

    return user;
});
