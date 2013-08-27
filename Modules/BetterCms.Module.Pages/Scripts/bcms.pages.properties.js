/*jslint unparam: true, white: true, browser: true, devel: true, vars: true */
/*global bettercms, console */

bettercms.define('bcms.pages.properties', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.forms', 'bcms.dynamicContent', 'bcms.tags', 'bcms.ko.extenders',
        'bcms.media', 'bcms.redirect', 'bcms.options', 'bcms.security', 'bcms.messages'],
    function ($, bcms, modal, forms, dynamicContent, tags, ko, media, redirect, options, security, messages) {
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

                pagePropertiesForm: 'form:first',
                pagePropertiesPageIsPublishedCheckbox: '#IsVisibleToEveryone',

                optionsTab: '#bcms-tab-4'
            },
            links = {
                loadEditPropertiesDialogUrl: null,
                loadLayoutOptionsUrl: null
            },
            globalization = {
                editPagePropertiesModalTitle: null,
                pageStatusChangeConfirmationMessagePublish: null,
                pageStatusChangeConfirmationMessageUnPublish: null
            },
            keys = {
                editPagePropertiesInfoMessageClosed: 'bcms.EditPagePropertiesInfoBoxClosed'
            },
            classes = {
                pagePropertiesActiveTemplateBox: 'bcms-grid-box-active'
            },
            currentPageIsPublished;

        /**
        * Assign objects to module.
        */
        page.links = links;
        page.globalization = globalization;

        /**
        * Page view model
        */
        function PageViewModel(image, secondaryImage, featuredImage, tagsViewModel, optionListViewModel, accessControlViewModel) {
            var self = this;

            self.tags = tagsViewModel;
            self.options = optionListViewModel;
            self.image = ko.observable(new media.ImageSelectorViewModel(image));
            self.secondaryImage = ko.observable(new media.ImageSelectorViewModel(secondaryImage));
            self.featuredImage = ko.observable(new media.ImageSelectorViewModel(featuredImage));
            self.accessControl = accessControlViewModel;
        }

        /**
        * Initializes EditPageProperties dialog events.
        */
        page.initEditPagePropertiesDialogEvents = function (dialog, content) {
            var optionsContainer = dialog.container.find(selectors.optionsTab),
                optionListViewModel = options.createOptionValuesViewModel(optionsContainer, content.Data.OptionValues),
                tagsViewModel = new tags.TagsListViewModel(content.Data.Tags),
                accessControlViewModel = security.createUserAccessViewModel(content.Data.UserAccessList),
                pageViewModel = new PageViewModel(content.Data.Image, content.Data.SecondaryImage, content.Data.FeaturedImage, tagsViewModel, optionListViewModel, accessControlViewModel),
                form = dialog.container.find(selectors.pagePropertiesForm);

            ko.applyBindings(pageViewModel, form.get(0));

            currentPageIsPublished = dialog.container.find(selectors.pagePropertiesPageIsPublishedCheckbox).is(':checked');

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
                page.highlightPagePropertiesActiveTemplate(dialog, this, function(id) {
                    page.loadLayoutOptions(id, dialog.container, content.Data.TemplateId, optionsContainer, optionListViewModel);
                });
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

            return pageViewModel;
        };

        /**
        * Loads layout options: when user changes layout, options are reloaded
        */
        page.loadLayoutOptions = function (id, mainContainer, layoutId, optionsContainer, optionListViewModel) {
            var onComplete = function(json) {
                var i,
                    j,
                    items = optionListViewModel.items,
                    item,
                    itemExists,
                    itemsToRemove = [],
                    length;
                        
                optionsContainer.hideLoading();
                messages.refreshBox(mainContainer, json);
                if (json.Success) {
                    // Remove items with no value
                    for (i = 0, length = items().length; i < length; i++) {
                        item = items()[i];
                        if (!item.value() && item.canEditOption === false) {
                            itemsToRemove.push(item);
                        }
                    }
                    for (i = 0, length = itemsToRemove.length; i < length; i++) {
                        item = itemsToRemove[i];
                        items.remove(item);
                    }
                            
                    // Add new items
                    for (i = 0, length = json.Data.length; i < length; i++) {
                        itemExists = false;
                                
                        for (j = 0; j < items().length; j++) {
                            item = items()[j];
                            if (item.key() == json.Data[i].OptionKey) {
                                
                                item.canEditOption = (item.type() != json.Data[i].Type);
                                item.changeFieldsEditing();
                                
                                itemExists = true;
                                break;
                            }
                        }
                                
                        // Do not add option if such already exists
                        if (itemExists) {
                            continue;
                        }

                        item = optionListViewModel.createItem(json.Data[i]);
                        optionListViewModel.items.push(item);
                    }
                }
            };

            optionsContainer.showLoading();

            $.ajax({
                type: 'GET',
                url: $.format(links.loadLayoutOptionsUrl, id)
            })
            .done(function (result) {
                onComplete(result);
            })
            .fail(function (response) {
                onComplete(bcms.parseFailedResponse(response));
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
                dialog.container.find(selectors.permalinkInfoField).html(value || "&nbsp;");

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
        page.highlightPagePropertiesActiveTemplate = function (dialog, selectButton, onChangeCallback) {
            var active = dialog.container.find(selectors.pagePropertiesActiveTemplateBox),
                template = $(selectButton).parents(selectors.pagePropertiesTemplateBox),
                id = $(template).data('id');

            active.removeClass(classes.pagePropertiesActiveTemplateBox);
            active.find(selectors.pagePropertiesTemplateSelect).show();
            active.find(selectors.pagePropertiesActiveTemplateMessage).hide();

            if (template) {
                dialog.container.find(selectors.pagePropertiesTemplateId).val(id);
                $(template).addClass(classes.pagePropertiesActiveTemplateBox);
                $(template).find(selectors.pagePropertiesActiveTemplateMessage).show();

                onChangeCallback.call(this, id);
            }

            $(selectButton).hide();
        };

        /**
        * Opens modal window for given page with page properties
        */
        page.openEditPageDialog = function (id, postSuccess) {
            var pageViewModel;

            modal.open({
                title: globalization.editPagePropertiesModalTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.loadEditPropertiesDialogUrl, id);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (childDialog, content) {
                            pageViewModel = page.initEditPagePropertiesDialogEvents(childDialog, content);
                        },

                        beforePost: function () {
                            if (!dialog.container.find(selectors.permalinkEditField).valid()) {
                                page.showAddNewPageEditPermalinkBox(dialog);
                                return false;
                            }

                            var newPageIsPublished = dialog.container.find(selectors.pagePropertiesPageIsPublishedCheckbox).is(':checked'),
                                message = newPageIsPublished ? globalization.pageStatusChangeConfirmationMessagePublish : globalization.pageStatusChangeConfirmationMessageUnPublish;
                            if (currentPageIsPublished != newPageIsPublished) {
                                modal.confirm({
                                    content: message,
                                    onAccept: function () {
                                        currentPageIsPublished = newPageIsPublished;
                                        dialog.container.find(selectors.pagePropertiesForm).submit();
                                    }
                                });
                                return false;
                            }

                            return pageViewModel.options.isValid(true);
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
