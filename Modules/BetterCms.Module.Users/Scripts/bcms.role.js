/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.role', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.dynamicContent'], function ($, bcms, modal, siteSettings, editor, dynamicContent) {
    'use strict';

    var role = {},
        selectors = {
            siteSettingsRoleCreatButton: "#bcms-create-role-button",
            usersTable: '#bcms-users-grid',

            roleForm: '#bcms-role-form',
            roleRowEditButtons: '.bcms-grid-item-edit-button',
            roleParentRow: 'tr:first'
        },

        links = {
            logoutUrl: 'unknown',
            loadSiteSettingsRoleUrl: null,
            loadEditUserUrl: null,
            loadCreatRoleUrl: null,
            loadEditRoleUrl: null

        },

        globalization = {
            confirmLogoutMessage: null,
            rolesListTabTitle: null,
            rolesAddNewTitle: null
        };

    // Assign objects to module.
    role.links = links;
    role.globalization = globalization;
    role.selectors = selectors;


    role.init = function () {
    };


    role.initSiteSettingsRoleEvents = function(container, json) {
        var html = json.Html,
            data = (json.Success == true) ? json.Data : null;

        container.html(html);

        container.find(selectors.roleRowEditButtons).on('click', function() {
            editRole(container, $(this));
        });
         container.find(selectors.siteSettingsRoleCreatButton).on('click', function() {
            role.openCreatRoleDialog();
        });
    };
  
  
    role.openCreatRoleDialog = function () {//(onSaveCallback) {
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
    * Calls function, which opens dialog for a role editing.
    */
    function editRole(container, self) {
        var row = self.parents(selectors.roleParentRow),
                id = row.data('id');

        editRoleWindow(id, function (data) {
        if (data.Data != null) {
        //setRoleFields(row, data);
        grid.showHideEmptyRow(container);
        }
        });
    };

    /*
    * Open a template edit dialog by the specified tempalte type.
    */
    function editRoleWindow(templateId, onSaveCallback) {
    role.openEditTemplateDialog(templateId, onSaveCallback);

    };

    role.openEditTemplateDialog = function (templateId, onSaveCallback) {
        modal.open({
            title: globalization.rolesAddNewTitle,
            onLoad: function (childDialog) {
                dynamicContent.bindDialog(childDialog, $.format(links.loadEditRoleUrl, templateId), {
                    contentAvailable: initializeEditRoleForm,

                    //beforePost: function (form) {
                      //  editor.resetAutoGenerateNameId();
                        //editor.setInputNames(form);
                    //},

                    //postSuccess: onSaveCallback
                });
            }
        });
    };


    bcms.registerInit(role.init);

    return role;
});
