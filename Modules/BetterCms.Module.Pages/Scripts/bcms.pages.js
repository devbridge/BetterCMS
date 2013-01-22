/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.pages.properties', 'bcms.messages', 'bcms.grid'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, pageProperties, messages, grid) {
    'use strict';

        var page = {},
            
        selectors = {
            editPermalink: '#bcms-addnewpage-editpermalink',
            editPermalinkBox: '.bcms-edit-urlpath-box',
            editPermalinkClose: 'div.bcms-edit-urlpath-box .bcms-tip-close, div.bcms-edit-urlpath-box .bcms-btn-links-small',
            editPermalinkSave: '#bcms-save-permalink',
            permalinkHiddenField: '#bcms-page-permalink',
            permalinkEditField: '#bcms-page-permalink-edit',
            permalinkInfoField: '#bcms-page-permalink-info',
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
            siteSettingsPageCreateButton: '#bcms-create-page-button',
            siteSettingsPageEditButton: '.bcms-grid-item-edit-button',
            siteSettingsPageDeleteButton: '.bcms-grid-item-delete-button',
            siteSettingsPageRowTemplate: '#bcms-pages-list-row-template',
            siteSettingsPageBooleanTemplateTrue: '#bcms-boolean-true-template',
            siteSettingsPageBooleanTemplateFalse: '#bcms-boolean-false-template',
            siteSettingsPageRowTemplateFirstRow: 'tr:first',
            siteSettingsPageParentRow: 'tr:first',
            siteSettingsPagesTableFirstRow: 'table.bcms-tables > tbody > tr:first',
            siteSettingsRowCells: 'td',
            
            siteSettingPageTitleCell: '.bcms-page-title',
            siteSettingPageCreatedCell: '.bcms-page-created',
            siteSettingPageModifiedCell: '.bcms-page-modified',
            siteSettingPageIsPublishedCell: '.bcms-page-ispublished',
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
            cloneButtonTitle: null
        },
        
        keys = {
            addNewPageInfoMessageClosed: 'bcms.addNewPageInfoBoxClosed'
        },
        
        classes = {
            addNewPageActiveTemplateBox: 'bcms-grid-box-active'
        };

    /**
    * Assign objects to module.
    */
    page.links = links;
    page.globalization = globalization;
    page.senderId = 0;

    /**
    * Initializes AddNewPage dialog events.
    */
    page.initAddNewPageDialogEvents = function (dialog) {
        dialog.container.find(selectors.editPermalink).on('click', function() {
            page.showAddNewPageEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.editPermalinkClose).on('click', function() {
            page.closeAddNewPageEditPermalinkBox(dialog);
        });
        
        dialog.container.find(selectors.editPermalinkSave).on('click', function () {
            page.saveAddNewPageEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.addNewPageTitleInput).on('keyup', function () {
            page.changeUrlSlug(dialog);
        });

        dialog.container.find(selectors.addNewPageForm).on('submit', function () {
            if (!dialog.container.find(selectors.permalinkEditField).valid()) {
                page.showAddNewPageEditPermalinkBox(dialog);
            }
        });

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
        
        bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.permalinkEditField), {
            preventedEnter: function () {
                dialog.container.find(selectors.permalinkEditField).blur();
                page.saveAddNewPageEditPermalinkBox(dialog);
            },
            preventedEsc: function () {
                dialog.container.find(selectors.permalinkEditField).blur();
                page.closeAddNewPageEditPermalinkBox(dialog);
            }
        });
    };

    /**
    * Shows edit permalink box in AddNewPage dialog.
    */
    page.showAddNewPageEditPermalinkBox = function (dialog) {
        dialog.container.find(selectors.editPermalinkBox).show();
        dialog.container.find(selectors.editPermalink).hide();
        dialog.container.find(selectors.permalinkEditField).focus();
    };

    /**
    * Sets changed permalink value in PageProperties dialog
    */
    page.saveAddNewPageEditPermalinkBox = function (dialog) {
        if ($(selectors.permalinkEditField).valid()) {
            var value = dialog.container.find(selectors.permalinkEditField).val();
            dialog.container.find(selectors.permalinkHiddenField).val(value);
            dialog.container.find(selectors.permalinkInfoField).html(value ? value : "&nbsp;");

            page.hideAddNewPageEditPermalinkBox(dialog);
        }
    };
    
    /**
    * Closes edit permalink box in AddNewPage dialog.
    */
    page.closeAddNewPageEditPermalinkBox = function (dialog) {
        var value = dialog.container.find(selectors.permalinkHiddenField).val();
        dialog.container.find(selectors.permalinkEditField).val(value);

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
                messages.showMessages(json);
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
        modal.open({
            title: globalization.addNewPageDialogTitle,
            onLoad: function (dialog) {
                dynamicContent.bindDialog(dialog, links.loadAddNewPageDialogUrl, {
                    contentAvailable: page.initAddNewPageDialogEvents,

                    postSuccess: function(data) {
                        bcms.trigger(bcms.events.pageCreated, data);
                        if (postSuccess && $.isFunction(postSuccess)) {
                            postSuccess(data);
                        }
                    }
                });
            }
        });
    };

    page.addNewPage = function() {
        page.openCreatePageDialog(function (data) {
            // Redirect
// TODO: we can not reload the page.
//       Some addition functionality could be working (example: add page to sitemap dialog is open).
//       Can not move this to sitemap, because sitemap does not know if new page was created in site settings dialog.
            
//            if (data.Data && data.Data.PageUrl) {
//                window.location.href = data.Data.PageUrl;
//            }
        });
    };
    
    /**
    * Deletes current page
    */
    page.deleteCurrentPage = function () {
        var id = bcms.pageId;

        page.deletePage(id, function () {
            modal.alert({
                title: globalization.pageDeletedTitle,
                content: globalization.pageDeletedMessage,
                disableCancel: true,
                disableAccept: true,
            });
            
            window.location.href = "/";
        });
    };

    /**
    * Deletes page
    */
    page.deletePage = function (id, postSuccess, title) {
        title = title || globalization.deletePageDialogTitle;
        modal.open({
            title: title,
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
    page.changeUrlSlug = function (dialog) {
        var oldText = dialog.container.find(selectors.addNewPageTitleInput).val().trim();
        setTimeout(function() {
            var text = dialog.container.find(selectors.addNewPageTitleInput).val().trim(),
                senderId = page.senderId++,
                onComplete = function (json) {
                    if (json && json.SenderId == senderId && json.Url) {
                        var slug = json.Url,
                            prefix = window.location.pathname,
                            url = prefix + (slug ? slug + '/' : '');

                        dialog.container.find(selectors.permalinkEditField).val(url);
                        dialog.container.find(selectors.permalinkHiddenField).val(url);
                        dialog.container.find(selectors.permalinkInfoField).html(url);
                    }
                };

            if (text && oldText == text) {
                $.ajax({
                    type: 'GET',
                    url: $.format(links.convertStringToSlugUrl, encodeURIComponent(text), senderId),
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
            page.searchSiteSettingsPages(form);
            return false;
        });

        form.find(selectors.siteSettingsPagesSearchButton).on('click', function () {
            page.searchSiteSettingsPages(form);
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

        container.find(selectors.siteSettingsPageDeleteButton).on('click', function (event) {
            bcms.stopEventPropagation(event);
            page.deleteSiteSettingsPage($(this), container);
        });
    };

    /**
    * Search site settings pages
    */
    page.searchSiteSettingsPages = function(form) {
        grid.submitGridForm(form, function (data) {
            siteSettings.setContent(data);
            page.initializeSiteSettingsPagesList(data);
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

                newRow.find(selectors.siteSettingPageTitleCell).html(data.Data.Title);
                newRow.find(selectors.siteSettingPageCreatedCell).html(data.Data.CreatedOn);
                newRow.find(selectors.siteSettingPageModifiedCell).html(data.Data.ModifiedOn);

                page.siteSettingsSetBooleanTemplate(newRow.find(selectors.siteSettingPageIsPublishedCell), data.Data.IsPublished);
                page.siteSettingsSetBooleanTemplate(newRow.find(selectors.siteSettingPageHasSeoCell), data.Data.HasSEO);

                newRow.find(selectors.siteSettingsPageEditButton).data('id', data.Data.PageId);
                newRow.find(selectors.siteSettingsPageDeleteButton).data('id', data.Data.PageId);
                newRow.find(selectors.siteSettingsPageDeleteButton).data('version', data.Data.Version);

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
                
                page.siteSettingsSetBooleanTemplate(row.find(selectors.siteSettingPageIsPublishedCell), data.Data.IsPublished);
                page.siteSettingsSetBooleanTemplate(row.find(selectors.siteSettingPageHasSeoCell), data.Data.HasSEO);
            }
        });
    };

    /**
    * Deletes page from site settings pages list
    */
    page.deleteSiteSettingsPage = function (self, container) {
        var id = self.data('id');

        page.deletePage(id, function () {
            self.parents(selectors.siteSettingsPageParentRow).remove();
            
            grid.showHideEmptyRow(container);
        });
    };

    page.clonePage = function() {
        modal.open({
            title: globalization.clonePageDialogTitle,
            acceptTitle: globalization.cloneButtonTitle,
            onLoad: function (dialog) {
                var url = $.format(links.clonePageDialogUrl, bcms.pageId);
                dynamicContent.bindDialog(dialog, url, {
                    postSuccess: function (json) {
                        messages.showMessages(json);
                        if (json.DataType === 'redirect') {                            
                            window.setTimeout(function() {
                                bcms.redirect(json.Data);
                            }, 500);
                        }
                    }
                });
            }
        });
    };

    /**
    * Converts string to url
    */
    page.convertStringToSlug = function (delay, text, canChange, onComplete) {
        var url = $.format(links.convertStringToSlugUrl, text);

        setTimeout(function () {
            bcms.reload();
        }, 200);

        /*$.ajax({
            type: 'POST',
            url: links.changePublishStatusUrl,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data)
        })
            .done(function (result) {
                onComplete(result);
            })
            .fail(function (response) {
                onComplete(bcms.parseFailedResponse(response));
            });*/
    };

    /**
    * Initializes page module.
    */
    page.init = function () {
        console.log('Initializing bcms.pages module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(page.init);
    
    return page;
});
