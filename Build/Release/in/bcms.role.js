/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.role', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.dynamicContent', 'bcms.messages', 'bcms.grid'], 
    function ($, bcms, modal, siteSettings, editor, dynamicContent, messages, grid) {
    'use strict';

    var role = {},
        selectors = {
            siteSettingsRoleCreatButton: "#bcms-create-role-button",
            usersTable: '#bcms-users-grid',

            roleForm: '#bcms-role-form',
            roleRowEditButtons: '.bcms-grid-item-edit-button',
            roleRowDeleteButtons: '.bcms-grid-item-delete-button',
            roleParentRow: 'tr:first',
            roleNameCell: '.bcms-template-name',
            roleRowTemplate: '#bcms-template-list-row-template',
            roleTableFirstRow: 'table.bcms-tables > tbody > tr:first'
                
        },

        links = {
            logoutUrl: 'unknown',
            loadSiteSettingsRoleUrl: null,
            loadEditUserUrl: null,
            loadCreatRoleUrl: null,
            loadEditRoleUrl: null,
            deleteRoleUrl: null

        },

        globalization = {
            confirmLogoutMessage: null,
            rolesListTabTitle: null,
            rolesAddNewTitle: null,
            deleteRoleConfirmMessage: null
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
         container.find(selectors.roleRowDeleteButtons).on('click', function () {
                deleteRole(container, $(this));
        });
    };
  
  
    role.openCreatRoleDialog = function (onSaveCallback) {
        modal.open({
            title: globalization.rolesAddNewTitle,
            onLoad: function (childDialog) {
                dynamicContent.bindDialog(childDialog, links.loadCreatRoleUrl, {
                    contentAvailable: initializeEditRoleForm,

                    postSuccess: onSaveCallback
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
        setRoleFields(row, data);
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

                    beforePost: function (form) {
                        editor.resetAutoGenerateNameId();
                        editor.setInputNames(form);
                    },

                    postSuccess: onSaveCallback
                });
            }
        });
    };

     role.deleteRole = function (roleId, roleVersion, roleName, onDeleteCallback) {
            var url = $.format(links.deleteRoleUrl, roleId, roleVersion),
                message = $.format(globalization.deleteRoleConfirmMessage, roleName),
                onDeleteCompleted = function (json) {
                    try {
                        if (json.Success && $.isFunction(onDeleteCallback)) {
                            onDeleteCallback(json);
                        }
                    } finally {
                        confirmDialog.close();
                    }
                },
                confirmDialog = modal.confirm({
                    content: message,
                    onAccept: function () {
                        $.ajax({
                            type: 'POST',
                            url: url,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            cache: false
                        })
                        .done(function (json) {
                            onDeleteCompleted(json);
                        })
                        .fail(function (response) {
                            onDeleteCompleted(bcms.parseFailedResponse(response));
                        });
                        return false;
                    }
                });
        };
    
        /**
        * Deletes role from site settings role list.
        */
        function deleteRole(container, self) {
            var row = self.parents(selectors.roleParentRow),
                id = row.data('id'),
                version = row.data('version'),
                name = row.find(selectors.roleNameCell).html();

            role.deleteRole(id, version, name, function(data) {
                messages.refreshBox(container, data);
                if (data.Success) {
                    row.remove();
                    grid.showHideEmptyRow(container);
                }
            });
        };

        /**
        * Set values, returned from server to row fields
        */
        function setRoleFields(row, json) {
            row.data('id', json.Data.Id);
            row.data('version', json.Data.Version);
            row.find(selectors.roleNameCell).html(json.Data.RoleName);
        };

    bcms.registerInit(role.init);

    return role;
});
