/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.properties', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.forms', 'bcms.dynamicContent', 'bcms.pages.tags', 'bcms.ko.extenders', 'bcms.media', 'bcms.redirect'],
    function ($, bcms, modal, forms, dynamicContent, tags, ko, media, redirect) {
    'use strict';

    var page = {},
        selectors = {
            editPagePropertiesCloseInfoMessageBox: '.bcms-info-message-box',

            editPermalink: '#bcms-pageproperties-editpermalink',
            editPermalinkBox: '.bcms-edit-urlpath-box',
            editPermalinkClose: 'div.bcms-edit-urlpath-box .bcms-tip-close, div.bcms-edit-urlpath-box .bcms-btn-links-small',
            editPermalinkSave: '#bcms-save-permalink',
            permalinkHiddenField: '#bcms-page-permalink',
            permalinkEditField: '#bcms-page-permalink-edit',
            permalinkInfoField: '#bcms-page-permalink-info',

            pagePropertiesTemplateSelect: '.bcms-btn-grid',
            pagePropertiesTemplateId: '#TemplateId',
            pagePropertiesActiveTemplateBox: '.bcms-grid-box-active',
            pagePropertiesTemplateBox: '.bcms-grid-box',
            pagePropertiesActiveTemplateMessage: '.bcms-grid-active-message-text',
            pagePropertiesTemplatePreviewLink: '.bcms-preview-template',

            pagePropertiesForm: 'form:first'

        },
        links = {
            loadEditPropertiesDialogUrl: null
        },
        globalization = {
            editPagePropertiesModalTitle: null,
        },
        keys = {
            editPagePropertiesInfoMessageClosed: 'bcms.EditPagePropertiesInfoBoxClosed'
        },
        classes = {
            pagePropertiesActiveTemplateBox: 'bcms-grid-box-active'
        };

    /**
    * Assign objects to module.
    */
    page.links = links;
    page.globalization = globalization;

    /**
    * Page view model
    */
    function PageViewModel(image, tagsViewModel) {
        var self = this;

        self.tags = tagsViewModel;
        self.image = ko.observable(new media.ImageSelectorViewModel(image));
    }

    /**
    * Initializes EditPageProperties dialog events.
    */
    page.initEditPagePropertiesDialogEvents = function (dialog, content) {
        var tagsViewModel = new tags.TagsListViewModel(content.Data.Tags),
            pageViewModel = new PageViewModel(content.Data.Image, tagsViewModel),
            form = dialog.container.find(selectors.pagePropertiesForm);
        ko.applyBindings(pageViewModel, form.get(0));

        dialog.container.find(selectors.editPermalink).on('click', function () {
            page.showPagePropertiesEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.editPermalinkClose).on('click', function () {
            page.closePagePropertiesEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.editPermalinkSave).on('click', function () {
            page.savePagePropertiesEditPermalinkBox(dialog);
        });

        form.on('submit', function () {
            if (!dialog.container.find(selectors.permalinkEditField).valid()) {
                page.showPagePropertiesEditPermalinkBox(dialog);
            }
        });

        var infoMessageClosed = localStorage.getItem(keys.editPagePropertiesInfoMessageClosed);
        if (infoMessageClosed && infoMessageClosed === '1') {
            page.hideEditPagePropertiesInfoMessage(dialog);
        } else {
            dialog.container.find(selectors.editPagePropertiesCloseInfoMessageBox).on('click', function () {
                localStorage.setItem(keys.editPagePropertiesInfoMessageClosed, '1');
                page.hideEditPagePropertiesInfoMessage(dialog);
            });
        }

        dialog.container.find(selectors.pagePropertiesTemplateSelect).on('click', function () {
            page.highlightPagePropertiesActiveTemplate(dialog, this);
        });

        dialog.container.find(selectors.pagePropertiesTemplatePreviewLink).on('click', function () {
            var template = $(this),
                url = template.data('url'),
                alt = template.data('alt');
            
            modal.imagePreview(url, alt);
        });

        bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.permalinkEditField), {
            preventedEnter: function () {
                dialog.container.find(selectors.permalinkEditField).blur();
                page.savePagePropertiesEditPermalinkBox(dialog);
            },
            preventedEsc: function () {
                dialog.container.find(selectors.permalinkEditField).blur();
                page.closePagePropertiesEditPermalinkBox(dialog);
            }
        });
    };

    /**
    * Shows edit permalink box in PageProperties dialog.
    */
    page.showPagePropertiesEditPermalinkBox = function (dialog) {
        dialog.container.find(selectors.editPermalinkBox).show();
        dialog.container.find(selectors.editPermalink).hide();
        dialog.container.find(selectors.permalinkEditField).focus();
    };

    /**
    * Sets changed permalink value in PageProperties dialog
    */
    page.savePagePropertiesEditPermalinkBox = function (dialog) {
        if ($(selectors.permalinkEditField).valid()) {
            var value = dialog.container.find(selectors.permalinkEditField).val();
            dialog.container.find(selectors.permalinkHiddenField).val(value);
            dialog.container.find(selectors.permalinkInfoField).html(value ? value : "&nbsp;");

            page.hidePagePropertiesEditPermalinkBox(dialog);
        }
    };

    /*
    * Closes edit permalink box in PageProperties dialog.
    */
    page.closePagePropertiesEditPermalinkBox = function (dialog) {
        var value = dialog.container.find(selectors.permalinkHiddenField).val(),
            permalinkEditField = dialog.container.find(selectors.permalinkEditField);
        permalinkEditField.val(value);
        permalinkEditField.blur();

        page.hidePagePropertiesEditPermalinkBox(dialog);
    };

    /**
    * Hides edit permalink box in PageProperties dialog.
    */
    page.hidePagePropertiesEditPermalinkBox = function (dialog) {
        dialog.container.find(selectors.editPermalinkBox).hide();
        dialog.container.find(selectors.editPermalink).show();
    };

    /**
    * Hides info message box in EditPageProperties dialog.
    */
    page.hideEditPagePropertiesInfoMessage = function (dialog) {
        dialog.container.find(selectors.editPagePropertiesCloseInfoMessageBox).hide();
    };

    /**
    * highlights active template box in PageProperties dialog.
    */
    page.highlightPagePropertiesActiveTemplate = function (dialog, selectButton) {
        var active = dialog.container.find(selectors.pagePropertiesActiveTemplateBox),
            template = $(selectButton).parents(selectors.pagePropertiesTemplateBox);

        active.removeClass(classes.pagePropertiesActiveTemplateBox);
        active.find(selectors.pagePropertiesTemplateSelect).show();
        active.find(selectors.pagePropertiesActiveTemplateMessage).hide();

        if (template) {
            dialog.container.find(selectors.pagePropertiesTemplateId).val($(template).data('id'));
            $(template).addClass(classes.pagePropertiesActiveTemplateBox);
            $(template).find(selectors.pagePropertiesActiveTemplateMessage).show();
        }

        $(selectButton).hide();
    };

    /**
    * Opens modal window for given page with page properties
    */
    page.openEditPageDialog = function (id, postSuccess) {
        modal.open({
            title: globalization.editPagePropertiesModalTitle,
            onLoad: function (dialog) {
                var url = $.format(links.loadEditPropertiesDialogUrl, id);
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable: page.initEditPagePropertiesDialogEvents,

                    beforePost: function () {
                        if (!dialog.container.find(selectors.permalinkEditField).valid()) {
                            page.showAddNewPageEditPermalinkBox(dialog);
                        }
                    },

                    postSuccess: postSuccess
                });
            }
        });
    };

    /**
    * Opens modal window for current page with page properties
    */
    page.editPageProperties = function () {
        page.openEditPageDialog(bcms.pageId, function (data) {
            // Redirect
            if (data.Data && data.Data.PageUrl) {
                redirect.RedirectWithAlert(data.Data.PageUrl);
            }
        });
    };

    /**
    * Initializes page module.
    */
    page.init = function () {
        console.log('Initializing bcms.pages.properties module.');
    };

    /**
    * Register initialization
    */
    bcms.registerInit(page.init);

    return page;
});
