/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.pages.properties.js" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

bettercms.define('bcms.pages.properties', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.forms', 'bcms.dynamicContent', 'bcms.tags', 'bcms.categories', 'bcms.ko.extenders',
        'bcms.media', 'bcms.redirect', 'bcms.options', 'bcms.security', 'bcms.messages', 'bcms.codeEditor', 'bcms.pages.languages', 'bcms.store', 'bcms.htmlEditor'],
    function ($, bcms, modal, forms, dynamicContent, tags, categories, ko, media, redirect, options, security, messages, codeEditor, pageLanguages, store, htmlEditor) {
        'use strict';

        var page = {},
            selectors = {
                editPagePropertiesCloseInfoMessageBox: '.bcms-js-info-message',
                editPagePropertiesCloseInfoMessage: '#bcms-editpage-closeinfomessage',
                editPagePropertiesInfoMessageBox: '.bcms-info-messages',


                editPermalink: '#bcms-pageproperties-editpermalink',
                editPermalinkBox: '.bcms-js-edit-box',
                editPermalinkClose: 'div.bcms-js-edit-box .bcms-tip-close, div.bcms-js-edit-box .bcms-btn-cancel',
                editPermalinkSave: '#bcms-save-permalink',

                permalinkHiddenField: '#bcms-page-permalink',
                permalinkEditField: '#bcms-page-permalink-edit',
                permalinkInfoField: '#bcms-page-permalink-info',

                pagePropertiesTemplateSelect: '.bcms-js-grid-box',
                pagePropertiesTemplateId: '#TemplateId',
                pagePropertiesMasterPageId: '#MasterPageId',
                pagePropertiesCategoriesSelect: '#bcms-js-categories-select',
                pagePropertiesForceAccessProtocolSelect: '#ForceAccessProtocol',

                pagePropertiesForm: 'form:first',
                pagePropertiesPageIsPublishedCheckbox: '#IsPagePublished',
                pagePropertiesPageIsMasterCheckbox: '#IsMasterPage',

                optionsTab: '#bcms-tab-3',
                translationsTabContent: '#bcms-tab-5 .bcms-page-translations-content',
                javascriptCssTabOpener: '.bcms-tab-item[data-name="#bcms-tab-4"]',

                codeEditorsContainer: '.bcms-window-tabbed-options',
                codeEditorsTitles: '.bcms-content-titles',
                codeEditorsParent: '.bcms-input-list-holder'
            },
            links = {
                loadEditPropertiesDialogUrl: null,
                loadLayoutOptionsUrl: null,
                loadLayoutUserAccessUrl: null,
                checkForMissingContentUrl: null
            },
            globalization = {
                editPagePropertiesModalTitle: null,
                editMasterPagePropertiesModalTitle: null,
                pageStatusChangeConfirmationMessagePublish: null,
                pageStatusChangeConfirmationMessageUnPublish: null,
                pageConversionToMasterConfirmationMessage: null,
                selectedMasterIsChildPage: null,
                missingContentConfirmationMessage: null
            },
            keys = {
                editPagePropertiesInfoMessageClosed: 'bcms.EditPagePropertiesInfoBoxClosed'
            },
            classes = {
                pagePropertiesActiveTemplateBox: 'bcms-grid-box-active',
                inactive: 'bcms-inactive'
            },
            currentPageIsPublished,
            currentPageIsMaster,
            currentPageTemplateId,
            currentPageMasterId;

        /**
        * Assign objects to module.
        */
        page.links = links;
        page.globalization = globalization;

        /**
        * Template view model
        */
        function TemplateViewModel(template) {
            var self = this;

            self.id = template.TemplateId;
            self.previewUrl = template.PreviewUrl;
            self.previewThumbnailUrl = template.PreviewThumbnailUrl;
            self.title = template.Title;

            self.isActive = ko.observable(template.IsActive);
            self.isCircularToCurrent = template.IsCircularToCurrent;
            self.isCompatible = template.IsCompatible;
            self.isMasterPage = template.IsMasterPage;

            self.select = function () {

            };

            self.previewImage = function () {
                modal.imagePreview(self.previewUrl, self.title);
            };
        }

        /**
        * Templates list view model
        */
        function TemplatesListViewModel(templates, dialog, optionsContainer, optionListViewModel) {
            var self = this;

            self.templates = ko.observableArray();
            self.displayedTemplates = ko.observableArray();
            self.searchQuery = ko.observable();
            self.searchEnabled = ko.observable(false);
            self.hasFocus = ko.observable(false);
            self.optionsContainer = optionsContainer;
            self.optionListViewModel = optionListViewModel;
            self.dialog = dialog;

            for (var j = 0; j < templates.length; j++) {
                var currentTemplate = new TemplateViewModel(templates[j]);
                self.templates.push(currentTemplate);
            }

            self.searchQuery.subscribe(function () {
                self.search();
            });

            self.displayTemplates = function () {
                if (self.templates() != null) {
                    self.displayedTemplates.removeAll();

                    var query = (self.searchQuery() || '').toLowerCase();

                    for (var j = 0; j < self.templates().length; j++) {
                        var currentTemplate = self.templates()[j];
                        if (query && currentTemplate.title.toLowerCase().indexOf(query) < 0) {
                            continue;
                        }
                        self.displayedTemplates.push(currentTemplate);
                    }
                }
            };

            self.search = function () {
                self.displayTemplates();
            };

            self.toggleSearch = function () {
                if (!self.searchEnabled()) {
                    self.searchEnabled(true);
                    self.hasFocus(true);
                } else {
                    self.searchEnabled(false);
                    self.searchQuery('');
                }
            };

            self.setActive = function (template) {
                var active = self.findCurrentActive();

                if (active.id === template.id) {
                    return;
                }

                var messagesBox = messages.box({ container: dialog.container });
                messagesBox.clearMessages();
                if (template.isCircular) {
                    messagesBox.addWarningMessage(globalization.selectedMasterIsChildPage);
                    return;
                }

                active.isActive(false);
                if (template.isMasterPage) {
                    dialog.container.find(selectors.pagePropertiesMasterPageId).val(template.id);
                    dialog.container.find(selectors.pagePropertiesTemplateId).val('');
                } else {
                    dialog.container.find(selectors.pagePropertiesTemplateId).val(template.id);
                    dialog.container.find(selectors.pagePropertiesMasterPageId).val('');
                }

                template.isActive(true);

                page.loadLayoutOptions(template.id, template.isMasterPage, dialog.container, optionsContainer, optionListViewModel);
            };

            self.findCurrentActive = function () {
                for (var i = 0; i < self.templates().length; i++) {
                    if (self.templates()[i].isActive()) {
                        return self.templates()[i];
                    }
                }

                return null;
            };

            self.displayTemplates();
        }

        /**
        * Page view model
        */
        function PageViewModel(image, secondaryImage, featuredImage, tagsViewModel, optionListViewModel, accessControlViewModel, translationsViewModel, categoriesModel, templatesViewModel) {
            var self = this;

            self.tags = tagsViewModel;
            self.options = optionListViewModel;
            self.image = ko.observable(new media.ImageSelectorViewModel(image));
            self.secondaryImage = ko.observable(new media.ImageSelectorViewModel(secondaryImage));
            self.featuredImage = ko.observable(new media.ImageSelectorViewModel(featuredImage));
            self.accessControl = accessControlViewModel;
            self.translations = translationsViewModel;
            self.categories = categoriesModel;
            self.templatesList = templatesViewModel;
        }

        /**
        * Initializes EditPageProperties dialog events.
        */
        page.initEditPagePropertiesDialogEvents = function (dialog, content) {
            var optionsContainer = dialog.container.find(selectors.optionsTab),
                optionListViewModel = options.createOptionValuesViewModel(optionsContainer, content.Data.OptionValues, content.Data.CustomOptions),
                tagsViewModel = new tags.TagsListViewModel(content.Data.Tags),
                categoriesModel = new categories.CategoriesSelectListModel(content.Data.Categories, dialog.container),
                accessControlViewModel = security.createUserAccessViewModel(content.Data.UserAccessList),
                translationsViewModel = content.Data.Languages ? new pageLanguages.PageTranslationsListViewModel(content.Data.Translations, content.Data.Languages, content.Data.LanguageId, content.Data.PageId) : null,
                templatesViewModel = new TemplatesListViewModel(content.Data.Templates, dialog, optionsContainer, optionListViewModel),
                pageViewModel = new PageViewModel(content.Data.Image, content.Data.SecondaryImage, content.Data.FeaturedImage, tagsViewModel,
                    optionListViewModel, accessControlViewModel, translationsViewModel, categoriesModel, templatesViewModel),
                form = dialog.container.find(selectors.pagePropertiesForm),
                codeEditorInitialized = false;

            categories.initCategoriesSelect(categoriesModel, content.Data.CategoriesLookupList, dialog.container);

            $(selectors.pagePropertiesForceAccessProtocolSelect).select2({
                minimumResultsForSearch: -1
            });

            ko.applyBindings(pageViewModel, form.get(0));

            currentPageIsPublished = dialog.container.find(selectors.pagePropertiesPageIsPublishedCheckbox).is(':checked');
            currentPageIsMaster = dialog.container.find(selectors.pagePropertiesPageIsMasterCheckbox).is(':checked');
            currentPageTemplateId = dialog.container.find(selectors.pagePropertiesTemplateId).val();
            currentPageMasterId = dialog.container.find(selectors.pagePropertiesMasterPageId).val();

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

            var infoMessageClosed = store.get(keys.editPagePropertiesInfoMessageClosed);
            if (infoMessageClosed && infoMessageClosed === '1') {
                page.hideEditPagePropertiesInfoMessage(dialog);
            } else {
                dialog.container.find(selectors.editPagePropertiesCloseInfoMessageBox).on('click', function () {
                    store.set(keys.editPagePropertiesInfoMessageClosed, '1');
                    page.hideEditPagePropertiesInfoMessage(dialog);
                });
            }

            dialog.container.find(selectors.editPagePropertiesCloseInfoMessage).on('click', function () {
                dialog.container.find(selectors.editPagePropertiesInfoMessageBox).hide();
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

            dialog.container.find(selectors.javascriptCssTabOpener).on('click', function () {
                if (!codeEditorInitialized) {
                    var heightOptions = {
                        marginTop: 30,
                        topElements: [{
                            element: selectors.codeEditorsTitles,
                            takeMargins: true
                        }],
                        container: selectors.codeEditorsContainer,
                        parent: selectors.codeEditorsParent,
                        marginBottom: 1
                    };
                    codeEditor.initialize(dialog.container, dialog, null, heightOptions);
                    codeEditorInitialized = true;
                }
            });

            // Translations tab
            if (content.Data.ShowTranslationsTab && (!content.Data.Languages || content.Data.Languages.length == 0)) {
                dialog.container.find(selectors.translationsTabContent).addClass(classes.inactive);
            }

            return pageViewModel;
        };

        /**
        * Loads layout options: when user changes layout, options are reloaded
        */
        page.loadLayoutOptions = function (id, isMasterPage, mainContainer, optionsContainer, optionListViewModel) {
            var onComplete = function (json) {
                var i,
                    j,
                    items = optionListViewModel.items,
                    item,
                    itemExists,
                    itemsToRemove = [],
                    length;

                optionsContainer.hideLoading();
                if (json.Messages && json.Messages.length > 0) {
                    messages.refreshBox(mainContainer, json);
                }
                if (json.Success) {
                    // Remove unchanged items
                    for (i = 0, length = items().length; i < length; i++) {
                        item = items()[i];
                        if (item.useDefaultValue() === true && item.canEditOption() === false) {
                            itemsToRemove.push(item);
                        } else {
                            item.defaultValue('');
                            item.canEditOption(true);
                            item.useDefaultValue(false);
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

                                var canEditOption = item.type() != json.Data[i].Type && item.customType() != json.Data[i].CustomType;
                                item.canEditOption(canEditOption);
                                item.changeFieldsEditing();

                                if (!canEditOption) {
                                    item.defaultValue(json.Data[i].OptionDefaultValue);
                                }

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

                    // Set fields editings
                    for (i = 0, length = items().length; i < length; i++) {
                        item = items()[i];
                        item.changeFieldsEditing();
                    }
                }
            };

            optionsContainer.showLoading();

            $.ajax({
                type: 'GET',
                url: $.format(links.loadLayoutOptionsUrl, id, isMasterPage)
            })
            .done(function (result) {
                onComplete(result);
            })
            .fail(function (response) {
                onComplete(bcms.parseFailedResponse(response));
            });
        };

        /**
        * Loads layout user access: when user changes layout, user access are reloaded.
        */
        page.loadLayoutUserAccess = function (id, isMasterPage, mainContainer, accessControlViewModel) {
            var onComplete = function (json) {
                if (json.Messages && json.Messages.length > 0) {
                    messages.refreshBox(mainContainer, json);
                }
                if (json.Success) {
                    security.updateUserAccessViewModel(accessControlViewModel, json.Data);
                }
            };
            $.ajax({
                type: 'GET',
                url: $.format(links.loadLayoutUserAccessUrl, id, isMasterPage)
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
        };

        /**
        * Hides info message box in EditPageProperties dialog.
        */
        page.hideEditPagePropertiesInfoMessage = function (dialog) {
            dialog.container.find(selectors.editPagePropertiesCloseInfoMessageBox).hide();
        };

        /**
        * Opens modal window for given page with page properties
        */
        page.openEditPageDialog = function (id, postSuccess, title, onLoad) {
            var pageViewModel,
                canEdit = security.IsAuthorized(["BcmsEditContent"]),
                canEditMaster = security.IsAuthorized(["BcmsAdministration"]),
                canPublish = security.IsAuthorized(["BcmsPublishContent"]);

            modal.open({
                title: title || globalization.editPagePropertiesModalTitle,
                disableAccept: !canEdit && !canPublish && !canEditMaster,
                onLoad: function (dialog) {
                    var url = $.format(links.loadEditPropertiesDialogUrl, id);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (childDialog, content) {
                            var form = dialog.container.find(selectors.pagePropertiesForm),
                                publishCheckbox,
                                publishCheckboxParent;

                            // User with only BcmsPublishContent but without BcmsEditContent can only publish - only publish checkbox needs to be enabled.
                            if (form.data('readonly') !== true && canPublish && !canEdit && !canEditMaster) {
                                form.data('readonlyWithPublishing', true);
                                form.addClass(classes.inactive);
                                forms.setFieldsReadOnly(form);

                                publishCheckbox = form.find(selectors.pagePropertiesPageIsPublishedCheckbox);
                                publishCheckboxParent = publishCheckbox.parents('.bcms-input-list-holder:first');

                                publishCheckbox.removeAttr('readonly');
                                publishCheckboxParent.find('input[type="checkbox"]').attr("disabled", "disabled");
                                publishCheckbox.removeAttr("disabled");
                                publishCheckboxParent.css('z-index', 100);
                            }
                            pageViewModel = page.initEditPagePropertiesDialogEvents(childDialog, content);
                            if (content.Data && content.Data.IsMasterPage === true) {
                                childDialog.setTitle(globalization.editMasterPagePropertiesModalTitle);
                            }

                            if ($.isFunction(onLoad)) {
                                onLoad(childDialog, content);
                            }
                        },

                        beforePost: function () {
                            if (!dialog.container.find(selectors.permalinkEditField).valid()) {
                                page.showAddNewPageEditPermalinkBox(dialog);
                                return false;
                            }

                            if (!pageViewModel.options.isValid(true)) {
                                return false;
                            }

                            var newPageIsPublished = dialog.container.find(selectors.pagePropertiesPageIsPublishedCheckbox).is(':checked'),
                                message = newPageIsPublished ? globalization.pageStatusChangeConfirmationMessagePublish : globalization.pageStatusChangeConfirmationMessageUnPublish,
                                isMasterPage = dialog.container.find(selectors.pagePropertiesPageIsMasterCheckbox).is(':checked'),
                                newPageTemplateId = dialog.container.find(selectors.pagePropertiesTemplateId).val(),
                                newPageMasterId = dialog.container.find(selectors.pagePropertiesMasterPageId).val();

                            if (newPageMasterId != currentPageMasterId || newPageTemplateId != currentPageTemplateId) {

                                var isContentWillBeMissing = false;

                                $.ajax({
                                    type: 'GET',
                                    url: $.format(links.checkForMissingContentUrl, id, newPageTemplateId, newPageMasterId),
                                    async: false
                                })
                                    .done(function (result) {
                                        if (result.Data) {
                                            isContentWillBeMissing = result.Data.IsMissingContents;
                                        }
                                    });

                                if (isContentWillBeMissing) {
                                    modal.confirm({
                                        content: globalization.missingContentConfirmationMessage,
                                        onAccept: function () {
                                            currentPageMasterId = newPageMasterId;
                                            currentPageTemplateId = newPageTemplateId;
                                            dialog.container.find(selectors.pagePropertiesForm).submit();
                                        }
                                    });
                                    return false;
                                }
                            }

                            if (currentPageIsMaster != isMasterPage) {
                                modal.confirm({
                                    content: globalization.pageConversionToMasterConfirmationMessage,
                                    onAccept: function () {
                                        // Skip page publishing confirmation, because making master will force to publish.
                                        if (currentPageIsPublished != newPageIsPublished) {
                                            currentPageIsPublished = newPageIsPublished;
                                        }
                                        currentPageIsMaster = isMasterPage;
                                        dialog.container.find(selectors.pagePropertiesForm).submit();
                                    }
                                });
                                return false;
                            }

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

                            return true;
                        },

                        postSuccess: postSuccess
                    });
                },
                onAccept: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
                },
                onClose: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
                }
            });
        };

        /**
        * Opens modal window for current page with page properties
        */
        page.editPageProperties = function (onLoad) {
            page.openEditPageDialog(bcms.pageId, function (data) {
                // Redirect
                if (data.Data && data.Data.PageUrl) {
                    redirect.RedirectWithAlert(data.Data.PageUrl);
                }
            }, globalization.editPagePropertiesModalTitle, onLoad);
        };

        /**
        * Opens modal window for current page with page properties
        */
        page.editMasterPageProperties = function () {
            page.openEditPageDialog(bcms.pageId, function (data) {
                // Redirect
                if (data.Data && data.Data.PageUrl) {
                    redirect.RedirectWithAlert(data.Data.PageUrl);
                }
            }, globalization.editMasterPagePropertiesModalTitle);
        };

        /**
        * Initializes page module.
        */
        page.init = function () {
            bcms.logger.debug('Initializing bcms.pages.properties module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(page.init);

        return page;
    });