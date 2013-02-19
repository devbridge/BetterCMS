/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.user', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.dynamicContent', 'bcms.role', 'bcms.media'], 
    function ($, bcms, modal, siteSettings, editor, dynamicContent, role, media) {
    'use strict';

    var user = {},
        selectors = {
            siteSettingsUserCreateButton: "#bcms-create-user-button",
            usersTable: '#bcms-users-grid',
            userUploadImageButton: "#bcms-open-uploader-button",
            userImageId: ".bcms-user-image-id",
            userImage: ".bcms-user-image-url",
            userRowEditButtons: '.bcms-grid-item-edit-button',
            userParentRow: 'tr:first',
            userNameCell: '.bcms-user-name',
            userRowTemplate: '#bcms-role-list-row-template',
            userTableFirstRow: 'table.bcms-tables > tbody > tr:first',
            userRowTemplateFirstRow: 'tr:first'
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
            editUserTitle: null,
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

        var roles = new siteSettings.TabViewModel(role.globalization.rolesListTabTitle, role.links.loadSiteSettingsRoleUrl, role.initializeRoleListForm);

        tabs.push(users);

        tabs.push(roles);

        siteSettings.initContentTabs(tabs);
        editor.initialize(container, {});
    };


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
        container.find(selectors.userRowEditButtons).on('click', function() {
            editUser(container, $(this));
        });

    }

    user.openCreatUserDialog = function () {//(onSaveCallback) {
        modal.open({
            title: globalization.usersAddNewTitle,
            onLoad: function (childDialog) {
                dynamicContent.bindDialog(childDialog, links.loadEditUserUrl, {
                    contentAvailable: initUserCreatEvents,

                    //postSuccess: onSaveCallback
                });
            }
        });
    };
        
    function initUserCreatEvents(dialog) {
        var onImageInsert = function(image) {            
            dialog.container.find(selectors.userImageId).val(image.id());
            dialog.container.find(selectors.userImage).attr('src', image.thumbnailUrl);
        };
        dialog.container.find(selectors.userUploadImageButton).on('click', function () {            
            media.openImageInsertDialog(onImageInsert);          
        });        
    };

     /**
    * Calls function, which opens dialog for a user editing.
    */
    function editUser(container, self) {
        var row = self.parents(selectors.userParentRow),
                id = row.data('id');

        editUserWindow(id, function (data) {
        if (data.Data != null) {
            setUserFields(row, data);
        grid.showHideEmptyRow(container);
        }
        });
    };

    function editUserWindow(templateId, onSaveCallback) {
        user.openEditUserDialog(templateId, onSaveCallback);
    };
        
    user.openEditUserDialog = function (templateId, onSaveCallback) {
    modal.open({
            title: globalization.editUserTitle,
            onLoad: function (childDialog) {
                dynamicContent.bindDialog(childDialog, $.format(links.loadEditUserUrl, templateId), {
                    contentAvailable: initializeEditUserForm,

                    beforePost: function (form) {
                        editor.resetAutoGenerateNameId();
                        editor.setInputNames(form);
                    },

                    postSuccess: onSaveCallback
                });
            }
        });
    };
        
    function initializeEditUserForm() {
        var dialog = siteSettings.getModalDialog(),
            container = dialog.container;
        
        var form = container.find(selectors.userForm);

        form.on('submit', function (event) {
            event.preventDefault();
            return false;
        });
    }
        
    /**
    * Set values, returned from server to row fields
    */
    function setUserFields(row, json) {
        row.data('id', json.Data.Id);
        row.data('version', json.Data.Version);
        row.find(selectors.userNameCell).html(json.Data.UserName);
    };

    bcms.registerInit(user.init);

    return user;
});
