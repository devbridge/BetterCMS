/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.pages.properties', 'bcms.grid', 'bcms.redirect', 'bcms.messages'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, pageProperties, grid, redirect, messages) {
    'use strict';

        var page = { },            
            selectors = {
                editPermalink: '#bcms-page-editpermalink',
                editPermalinkBox: '.bcms-edit-urlpath-box',
                editPermalinkClose: 'div.bcms-edit-urlpath-box .bcms-tip-close, div.bcms-edit-urlpath-box .bcms-btn-links-small',
                editPermalinkSave: '#bcms-save-permalink',
                editPermalinkHiddenField: '#bcms-page-permalink',
                editPermalinkEditField: '#bcms-page-permalink-edit',
                editPermalinkInfoField: '#bcms-page-permalink-info',

                addNewPageTitleInput: '#PageTitle',
                addNewPageCloseInfoMessage: '#bcms-addnewpage-closeinfomessage',
                addNewPageCloseInfoMessageBox: '.bcms-info-message-box',
                addNewPageTemplateSelect: '.bcms-btn-grid',
                addNewPageTemplateId: '#TemplateId',
                addNewPageActiveTemplateBox: '.bcms-grid-box-active',
                addNewPageTemplateBox: '.bcms-grid-box',
                addNewPageActiveTemplateMessage: '.bcms-grid-active-message-text',
                addNewPageTemplatePreviewLink: '.bcms-preview-template',

                addNewPageForm: 'form:first',

                siteSettingsPagesListForm: '#bcms-pages-form',
                siteSettingsPagesSearchButton: '#bcms-pages-search-btn',
                siteSettingsPagesSearchField: '.bcms-search-query',
                siteSettingsPageCreateButton: '#bcms-create-page-button',
                siteSettingsPageEditButton: '.bcms-grid-item-edit-button',
                siteSettingsPageDeleteButton: '.bcms-grid-item-delete-button',
                siteSettingsPageRowTemplate: '#bcms-pages-list-row-template',
                siteSettingsPageBooleanTemplateTrue: '#bcms-boolean-true-template',
                siteSettingsPageBooleanTemplateFalse: '#bcms-boolean-false-template',
                siteSettingsPageStatusTemplatePublished: '#bcms-pagestatus-published-template',
                siteSettingsPageStatusTemplateUnpublished: '#bcms-pagestatus-unpublished-template',
                siteSettingsPageStatusTemplateDraft: '#bcms-pagestatus-draft-template',
                siteSettingsPageRowTemplateFirstRow: 'tr:first',
                siteSettingsPageParentRow: 'tr:first',
                siteSettingsPagesTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                siteSettingsRowCells: 'td',

                siteSettingPageTitleCell: '.bcms-page-title',
                siteSettingPageCreatedCell: '.bcms-page-created',
                siteSettingPageModifiedCell: '.bcms-page-modified',
                siteSettingPageStatusCell: '.bcms-page-ispublished',
                siteSettingPageHasSeoCell: '.bcms-page-hasseo',
            },        
            links = {
                loadEditPropertiesUrl: null,
                loadSiteSettingsPageListUrl: null,
                loadAddNewPageDialogUrl: null,
                deletePageConfirmationUrl: null,
                changePublishStatusUrl: null,
                clonePageDialogUrl: null,
                convertStringToSlugUrl: null
            },        
            globalization = {
                editPagePropertiesModalTitle: null,
                addNewPageDialogTitle: null,
                deletePageDialogTitle: null,
                pageDeletedTitle: null,
                pageDeletedMessage: null,
                clonePageDialogTitle: null,
                cloneButtonTitle: null,
                deleteButtonTitle: null
            },        
            keys = {
                addNewPageInfoMessageClosed: 'bcms.addNewPageInfoBoxClosed'
            },        
            classes = {
                addNewPageActiveTemplateBox: 'bcms-grid-box-active'
            },            
            pageUrlManuallyEdited = false;

    /**
    * Assign objects to module.
    */
    page.links = links;
    page.globalization = globalization;
    page.senderId = 0;

    page.initializePermalinkBox = function (dialog, addPrefix, actionUrl, titleField, autoGenarate) {
        pageUrlManuallyEdited = false;
        
        dialog.container.find(selectors.editPermalink).on('click', function () {
            page.showAddNewPageEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.editPermalinkClose).on('click', function () {
            page.closeAddNewPageEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.editPermalinkSave).on('click', function () {
            page.saveAddNewPageEditPermalinkBox(dialog);
        });

        if (autoGenarate) {
            dialog.container.find(titleField).on('keyup', function() {
                page.changeUrlSlug(dialog, addPrefix, actionUrl, titleField);
            });
        }

        dialog.container.find(selectors.editPermalinkEditField).on('keyup', function () {
            pageUrlManuallyEdited = true;
        });
        
        dialog.container.find(selectors.addNewPageForm).on('submit', function () {
            if (!dialog.container.find(selectors.editPermalinkEditField).valid()) {
                page.showAddNewPageEditPermalinkBox(dialog);
            }
        });
        
        bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.editPermalinkEditField), {
            preventedEnter: function () {
                dialog.container.find(selectors.editPermalinkEditField).blur();
                page.saveAddNewPageEditPermalinkBox(dialog);
            },
            preventedEsc: function () {
                dialog.container.find(selectors.editPermalinkEditField).blur();
                page.closeAddNewPageEditPermalinkBox(dialog);
            }
        });
    };

    page.isEditedPageUrlManually = function() {
        return pageUrlManuallyEdited;
    };
    /**
    * Initializes AddNewPage dialog events.
    */
    page.initAddNewPageDialogEvents = function (dialog) {
        page.initializePermalinkBox(dialog, true, links.convertStringToSlugUrl, selectors.addNewPageTitleInput, true);

        var infoMessageClosed = localStorage.getItem(keys.addNewPageInfoMessageClosed);
        if (infoMessageClosed && infoMessageClosed === '1') {
            page.hideAddNewPageInfoMessage(dialog);
        } else {
            dialog.container.find(selectors.addNewPageCloseInfoMessage).on('click', function () {
                localStorage.setItem(keys.addNewPageInfoMessageClosed, '1');
                page.hideAddNewPageInfoMessage(dialog);
            });
        }

        dialog.container.find(selectors.addNewPageTemplateSelect).on('click', function () {
            page.highlightAddNewPageActiveTemplate(dialog, this);
        });
        
        dialog.container.find(selectors.addNewPageTemplatePreviewLink).on('click', function () {
            var template = $(this),
                url = template.data('url'),
                alt = template.data('alt');

            modal.imagePreview(url, alt);
        });
    };

    /**
    * Shows edit permalink box in AddNewPage dialog.
    */
    page.showAddNewPageEditPermalinkBox = function (dialog) {
        dialog.container.find(selectors.editPermalinkBox).show();
        dialog.container.find(selectors.editPermalink).hide();
        dialog.container.find(selectors.editPermalinkEditField).focus();
    };

    /**
    * Sets changed permalink value in PageProperties dialog
    */
    page.saveAddNewPageEditPermalinkBox = function (dialog) {
        if ($(selectors.editPermalinkEditField).valid()) {
            var value = dialog.container.find(selectors.editPermalinkEditField).val();
            dialog.container.find(selectors.editPermalinkHiddenField).val(value);
            dialog.container.find(selectors.editPermalinkInfoField).html(value ? value : "&nbsp;");

            page.hideAddNewPageEditPermalinkBox(dialog);
        }
    };
    
    /**
    * Closes edit permalink box in AddNewPage dialog.
    */
    page.closeAddNewPageEditPermalinkBox = function (dialog) {
        var value = dialog.container.find(selectors.editPermalinkHiddenField).val(),
            permalinkEditField = dialog.container.find(selectors.editPermalinkEditField);
        permalinkEditField.val(value);
        permalinkEditField.blur();

        page.hideAddNewPageEditPermalinkBox(dialog);
    };

    /**
    * Hides edit permalink box in AddNewPage dialog.
    */
    page.hideAddNewPageEditPermalinkBox = function (dialog) {
        dialog.container.find(selectors.editPermalinkBox).hide();
        dialog.container.find(selectors.editPermalink).show();
    };

    /**
    * Hides info message box in AddNewPage dialog.
    */
    page.hideAddNewPageInfoMessage = function (dialog) {
        dialog.container.find(selectors.addNewPageCloseInfoMessageBox).hide();
    };
    
    /**
    * Highlights active template box in AddNewPage dialog.
    */
    page.highlightAddNewPageActiveTemplate = function (dialog, selectButton)
    {
        var active = dialog.container.find(selectors.addNewPageActiveTemplateBox),
            template = $(selectButton).parents(selectors.addNewPageTemplateBox);
        
        active.removeClass(classes.addNewPageActiveTemplateBox);
        active.find(selectors.addNewPageTemplateSelect).show();
        active.find(selectors.addNewPageActiveTemplateMessage).hide();

        if (template) {
            dialog.container.find(selectors.addNewPageTemplateId).val($(template).data('id'));
            $(template).addClass(classes.addNewPageActiveTemplateBox);
            $(template).find(selectors.addNewPageActiveTemplateMessage).show();
        }

        $(selectButton).hide();
    };

    page.changePublishStatus = function (sender) {

        var isPublished = sender.val() == "publish",
            data = {PageId: bcms.pageId, IsPublished: isPublished},
            onComplete = function (json) {
                modal.showMessages(json);
                if (json.Success) {
                    setTimeout(function() {
                        bcms.reload();
                    }, 500);
                    
                }
            };

        $.ajax({
            type: 'POST',
            url: links.changePublishStatusUrl,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data)
        })
            .done(function(result) {
                onComplete(result);
            })
            .fail(function(response) {
                onComplete(bcms.parseFailedResponse(response));
            });
    };

    page.openCreatePageDialog = function (postSuccess) {
        var permalinkValue,
            url = $.format(links.loadAddNewPageDialogUrl, window.location.pathname);

        modal.open({
            title: globalization.addNewPageDialogTitle,
            onLoad: function (dialog) {
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable: page.initAddNewPageDialogEvents,

                    beforePost: function () {
                        if (!pageUrlManuallyEdited) {
                            var pageUrlField = dialog.container.find(selectors.editPermalinkEditField);
                            permalinkValue = pageUrlField.val();
                            pageUrlField.val(null);
                        }
                    },

                    postError: function () {
                        if (!pageUrlManuallyEdited) {
                            var pageUrlField = dialog.container.find(selectors.editPermalinkEditField);
                            pageUrlField.val(permalinkValue);
                        }
                    },

                    postSuccess: function (data) {
                        if (bcms.trigger(bcms.events.pageCreated, { Data: data, Callback: postSuccess }) <= 0) {
                            if (postSuccess && $.isFunction(postSuccess)) {
                                postSuccess(data);
                            }
                        }
                    }
                });
            }
        });
    };

    page.addNewPage = function() {
        page.openCreatePageDialog(function (data) {
            if (data.Data && data.Data.Data && data.Data.Data.PageUrl) {
                redirect.RedirectWithAlert(data.Data.Data.PageUrl);
            }
        });
    };
    
    /**
    * Deletes current page
    */
    page.deleteCurrentPage = function () {
        var id = bcms.pageId;

        page.deletePage(id, function () {
            redirect.RedirectWithAlert('/', {
                title: globalization.pageDeletedTitle,
                message: globalization.pageDeletedMessage
            });
        });
    };

    /**
    * Deletes page
    */
    page.deletePage = function (id, postSuccess, title) {
        title = title || globalization.deletePageDialogTitle;
        modal.open({
            title: title,
            acceptTitle: globalization.deleteButtonTitle,
            onLoad: function(dialog) {
                var url = $.format(links.deletePageConfirmationUrl, id);
                dynamicContent.bindDialog(dialog, url, {
                    postSuccess: postSuccess
                });
            }
        });
    };

    /**
    * Changes page slug
    */
    page.changeUrlSlug = function (dialog, addPrefix, action, titleInput) {
        var oldText = $.trim(dialog.container.find(titleInput).val());
        setTimeout(function() {
            var text = $.trim(dialog.container.find(titleInput).val()),
                senderId = page.senderId++,
                onComplete = function (json) {
                    if (json && json.SenderId == senderId && json.Url) {
                        var slug = json.Url,
                            url = (slug ? slug : '');

                        if (addPrefix) {
                            url += '/'; 
                            var prefix = window.location.pathname;
                            if (prefix.substr(prefix.length - 1, 1) != '/') {
                                prefix += '/';
                            }
                            url = prefix + url;
                        }
                        
                        if (url.substr(0, 1) != '/') {
                            url = '/' + url;
                        }

                        dialog.container.find(selectors.editPermalinkEditField).val(url);
                        dialog.container.find(selectors.editPermalinkHiddenField).val(url);
                        dialog.container.find(selectors.editPermalinkInfoField).html(url);
                        
                        pageUrlManuallyEdited = false;
                    }
                };

            if (text && oldText == text) {
                $.ajax({
                    type: 'GET',
                    url: $.format(action, encodeURIComponent(text), senderId),
                    dataType: 'json',
                })
                    .done(function(result) {
                        onComplete(result);
                    })
                    .fail(function(response) {
                        onComplete(response);
                    });
            }
        }, 400);
    };

    /**
    * Loads site settings page list.
    */
    page.loadSiteSettingsPageList = function() {
        dynamicContent.bindSiteSettings(siteSettings, page.links.loadSiteSettingsPageListUrl, {
            contentAvailable: page.initializeSiteSettingsPagesList
        });
    };

    /**
    * Initializes site settings pages list and list items
    */
    page.initializeSiteSettingsPagesList = function () {
        var dialog = siteSettings.getModalDialog(),
            container = dialog.container;

        var form = dialog.container.find(selectors.siteSettingsPagesListForm);
        grid.bindGridForm(form, function (data) {
            siteSettings.setContent(data);
            page.initializeSiteSettingsPagesList(data);
        });

        form.on('submit', function (event) {
            event.preventDefault();
            page.searchSiteSettingsPages(form, container);
            return false;
        });

        form.find(selectors.siteSettingsPagesSearchButton).on('click', function () {
            page.searchSiteSettingsPages(form, container);
        });

        page.initializeSiteSettingsPagesListItems(container);
    };

    /**
    * Initializes site settings pages list items
    */
    page.initializeSiteSettingsPagesListItems = function(container) {
        container.find(selectors.siteSettingsPageCreateButton).on('click', function () {
            page.addSiteSettingsPage(container);
        });

        container.find(selectors.siteSettingsRowCells).on('click', function () {
            var editButton = $(this).parents(selectors.siteSettingsPageParentRow).find(selectors.siteSettingsPageEditButton);
            page.editSiteSettingsPage(editButton);
        });

        container.find(selectors.siteSettingPageTitleCell).on('click', function (event) {
            bcms.stopEventPropagation(event);
            var url = $(this).data('url');
            window.open(url);
        });

        container.find(selectors.siteSettingsPageDeleteButton).on('click', function (event) {
            bcms.stopEventPropagation(event);
            page.deleteSiteSettingsPage($(this), container);
        });
    };

    /**
    * Search site settings pages
    */
    page.searchSiteSettingsPages = function(form, container) {
        grid.submitGridForm(form, function (data) {
            siteSettings.setContent(data);
            page.initializeSiteSettingsPagesList(data);
            var searchInput = container.find(selectors.siteSettingsPagesSearchField);           
            var val = searchInput.val();
            searchInput.focus().val("");
            searchInput.val(val);            
        });
    };

    /**
    * Opens page create form from site settings pages list
    */
    page.addSiteSettingsPage = function(container) {
        page.openCreatePageDialog(function (data) {
            if (data.Data != null) {
                var template = $(selectors.siteSettingsPageRowTemplate),
                    newRow = $(template.html()).find(selectors.siteSettingsPageRowTemplateFirstRow);

                newRow.find(selectors.siteSettingPageTitleCell).html(data.Data.Data.Title);
                newRow.find(selectors.siteSettingPageCreatedCell).html(data.Data.Data.CreatedOn);
                newRow.find(selectors.siteSettingPageModifiedCell).html(data.Data.Data.ModifiedOn);

                page.siteSettingsPageStatusTemplate(newRow.find(selectors.siteSettingPageStatusCell), data.Data.Data.PageStatus);
                page.siteSettingsSetBooleanTemplate(newRow.find(selectors.siteSettingPageHasSeoCell), data.Data.Data.HasSEO);
                messages.refreshBox(selectors.siteSettingsPagesListForm, data.Data);
                
                newRow.find(selectors.siteSettingPageTitleCell).data('url', data.Data.Data.PageUrl);
                newRow.find(selectors.siteSettingsPageEditButton).data('id', data.Data.Data.PageId);
                newRow.find(selectors.siteSettingsPageDeleteButton).data('id', data.Data.Data.PageId);
                newRow.find(selectors.siteSettingsPageDeleteButton).data('version', data.Data.Data.Version);

                newRow.insertBefore($(selectors.siteSettingsPagesTableFirstRow, container));

                page.initializeSiteSettingsPagesListItems(newRow);
                
                grid.showHideEmptyRow(container);
            }
        });
    };

    /**
    * Creates html for boolean value, using given boolean template
    */
    page.siteSettingsSetBooleanTemplate = function (container, value) {
        var template = (value === true) ? $(selectors.siteSettingsPageBooleanTemplateTrue) : $(selectors.siteSettingsPageBooleanTemplateFalse),
            html = $(template.html());
        container.html(html);
    };

    /**
    * Creates html for page status value, using given PageStatus template
    */
    page.siteSettingsPageStatusTemplate = function (container, value) {
        var template;
        
        if (value == 2) {
            template = $(selectors.siteSettingsPageStatusTemplateDraft);
        }   else if (value == 3) {
            template = $(selectors.siteSettingsPageStatusTemplatePublished);
        } else {
            template = $(selectors.siteSettingsPageStatusTemplateUnpublished);
        }
        
        var html = $(template.html());
        container.html(html);
    };

    /**
    * Opens page edit form from site settings pages list
    */
    page.editSiteSettingsPage = function (self) {
        var id = self.data('id');

        pageProperties.openEditPageDialog(id, function (data) {
            if (data.Data != null) {
                var row = self.parents(selectors.siteSettingsPageParentRow);

                row.find(selectors.siteSettingPageTitleCell).html(data.Data.Title);
                row.find(selectors.siteSettingPageCreatedCell).html(data.Data.CreatedOn);
                row.find(selectors.siteSettingPageModifiedCell).html(data.Data.ModifiedOn);
                
                page.siteSettingsPageStatusTemplate(row.find(selectors.siteSettingPageStatusCell), data.Data.PageStatus);
                page.siteSettingsSetBooleanTemplate(row.find(selectors.siteSettingPageHasSeoCell), data.Data.HasSEO);
            }
        });
    };

    /**
    * Deletes page from site settings pages list
    */
    page.deleteSiteSettingsPage = function (self, container) {
        var id = self.data('id');

        page.deletePage(id, function (json) {
            self.parents(selectors.siteSettingsPageParentRow).remove();
            messages.refreshBox(selectors.siteSettingsPagesListForm, json);
            grid.showHideEmptyRow(container);
        });
    };

    /**
    * Opens dialog for clone the page
    */
    page.clonePage = function () {
        var permalinkValue;

        modal.open({
            title: globalization.clonePageDialogTitle,
            acceptTitle: globalization.cloneButtonTitle,
            onLoad: function (dialog) {
                var url = $.format(links.clonePageDialogUrl, bcms.pageId);
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable: function () {
                        page.initializePermalinkBox(dialog, false, links.convertStringToSlugUrl, selectors.addNewPageTitleInput, true);
                    },

                    beforePost: function () {
                        if (!pageUrlManuallyEdited) {
                            var pageUrlField = dialog.container.find(selectors.editPermalinkEditField);
                            permalinkValue = pageUrlField.val();
                            pageUrlField.val(null);
                        }
                    },

                    postError: function () {
                        if (!pageUrlManuallyEdited) {
                            var pageUrlField = dialog.container.find(selectors.editPermalinkEditField);
                            pageUrlField.val(permalinkValue);
                        }
                    },

                    postSuccess: function (json) {
                        if (json.Success && json.Data && (json.Data.Url || json.Data.PageUrl)) {
                            var postSuccess = function(data) {
                                redirect.RedirectWithAlert(json.Data.Url || json.Data.PageUrl);
                            };
                            if (bcms.trigger(bcms.events.pageCreated, { Data: json.Data, Callback: postSuccess }) <= 0) {
                                if (postSuccess && $.isFunction(postSuccess)) {
                                    postSuccess(json.Data);
                                }
                            }
                        } else {
                            modal.showMessages(json);
                        }
                    }
                });
            }
        });
    };

    /**
    * Initialize custom jQuery validators
    */
    function initializeCustomValidation() {
        $.validator.addMethod("jqpageurlvalidation", function(value, element, params) {
            if (pageUrlManuallyEdited && (!value || value.match(params.pattern) == null)) {
                return false;
            }

            return true;
        }, function (params) {
            return params.message;
        });

        $.validator.unobtrusive.adapters.add("pageurlvalidation", ['pattern'], function (options) {
            options.rules["jqpageurlvalidation"] = { message: options.message, pattern: options.params.pattern };
        });
        
        $.validator.addMethod("jqenddatevalidation", function (value, element, params) {
            var startDateString = $('#' + params.startdateproperty).val();
            if (value != null && value != "" && startDateString != null && startDateString != "") {
                return new Date(startDateString) <= new Date(value);
            }
            return true;
        }, function (params) {
            return params.message;
        });

        $.validator.unobtrusive.adapters.add("enddatevalidation", ['startdateproperty'], function (options) {
            options.rules["jqenddatevalidation"] = { message: options.message, startdateproperty: options.params.startdateproperty };
        });
    }

    /**
    * Initializes page module.
    */
    page.init = function () {
        console.log('Initializing bcms.pages module.');

        initializeCustomValidation();
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(page.init);
    
    return page;
});
