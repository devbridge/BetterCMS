/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.pages.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.pages', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent',
        'bcms.pages.properties', 'bcms.grid', 'bcms.redirect', 'bcms.messages', 'bcms.pages.filter', 'bcms.options', 'bcms.ko.extenders', 'bcms.security', 'bcms.sidemenu', 'bcms.datepicker', 'bcms.pages.languages', 'bcms.store', 'bcms.antiXss', ],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, pageProperties, grid, redirect, messages, filter, options, ko, security, sidemenu, datepicker, pageLanguages, store, antiXss) {
        'use strict';

        var page = {},
            selectors = {
                editPermalink: '#bcms-page-editpermalink',
                editPermalinkBox: '.bcms-js-edit-box',
                editPermalinkClose: 'div.bcms-js-edit-box .bcms-tip-close, div.bcms-js-edit-box .bcms-btn-cancel',
                editPermalinkSave: '#bcms-save-permalink',
                editPermalinkHiddenField: '#bcms-page-permalink',
                editPermalinkEditField: '#bcms-page-permalink-edit',
                editPermalinkInfoField: '#bcms-page-permalink-info',

                addNewPageTitleInput: '#PageTitle',
                addNewPageCloseInfoMessage: '#bcms-addnewpage-closeinfomessage',
                addNewPageCloseInfoMessageBox: '.bcms-js-info-message',
                addNewPageTemplateSelect: '.bcms-js-grid-box',
                addNewPageTemplateId: '#TemplateId',
                addNewPageMasterPageId: '#MasterPageId',


                addNewPageForm: 'form:first',
                addNewPageTabContent: '.bcms-window-tabbed-options',
                addNewPageOptionsTab: '#bcms-tab-2',
                addNewPageUserAccess: '#bcms-accesscontrol-context',

                siteSettingsPagesListForm: '#bcms-pages-form',
                siteSettingsPagesListFormFilterIncludeArchived: "#IncludeArchived",
                siteSettingsPagesSearchButton: '#bcms-pages-search-btn',
                siteSettingsPagesSearchField: '.bcms-search-query',
                siteSettingsPageCreateButton: '#bcms-create-page-button',
                siteSettingsPageEditButton: '.bcms-js-edit-button',
                siteSettingsPageDeleteButton: '.bcms-grid-item-delete-button',
                siteSettingsPageRowTemplate: '#bcms-pages-list-row-template',
                siteSettingsPageBooleanTemplateTrue: '#bcms-boolean-true-template',
                siteSettingsPageBooleanTemplateFalse: '#bcms-boolean-false-template',
                siteSettingsPageRowTemplateFirstRow: '.bcms-list-row:first',
                siteSettingsPageParentRow: '.bcms-js-list-row:first',
                siteSettingsPagesTableFirstRow: '.bcms-list-pages > .bcms-js-list-row:first',
                siteSettingsPagesTableRows: '.bcms-js-list-row',
                siteSettingsPagesTableActiveRow: '.bcms-list-pages > div.bcms-list-row-active:first',
                siteSettingsPagesParentTable: '.bcms-list-pages:first',
                siteSettingsRowCells: '.bcms-js-list-row',
                siteSettingsPagesGrid: '#bcms-pages-grid',

                siteSettingPageTitleCell: '.bcms-page-title',
                siteSettingPageCreatedCell: '.bcms-page-created',
                siteSettingPageModifiedCell: '.bcms-page-modified',
                siteSettingPageStatusCell: '.bcms-page-ispublished',
                siteSettingPageHasSeoCell: '.bcms-page-hasseo',
                siteSettingsPager: '.bcms-top-block-pager',

                hiddenPageNumberField: '#bcms-grid-page-number',
                clonePageForm: 'form:first',
                cloneWithLanguageGoToPagePropertiesLink: '#bcms-open-page-translations',
                pagePropertiesTranslationsTab: '.bcms-js-tab-header .bcms-tab-item[data-name="#bcms-tab-5"]',
                languageSelection: '#LanguageId'
            },
            links = {
                loadEditPropertiesUrl: null,
                loadSiteSettingsPageListUrl: null,
                loadSiteSettingsPagesJsonUrl: null,
                loadAddNewPageDialogUrl: null,
                deletePageConfirmationUrl: null,
                changePublishStatusUrl: null,
                clonePageDialogUrl: null,
                clonePageWithLanguageDialogUrl: null,
                convertStringToSlugUrl: null,
                loadSelectPageUrl: null,
            },
            globalization = {
                editPagePropertiesModalTitle: null,
                addNewPageDialogTitle: null,
                addNewMasterPageDialogTitle: null,
                deletePageDialogTitle: null,
                pageDeletedTitle: null,
                pageDeletedMessage: null,
                clonePageDialogTitle: null,
                clonePageWithLanguageDialogTitle: null,
                cloneButtonTitle: null,
                deleteButtonTitle: null,
                pageStatusChangeConfirmationMessagePublish: null,
                pageStatusChangeConfirmationMessageUnPublish: null,
                selectPageDialogTitle: null,
                selectPageSelectButtonTitle: null,
                pageNotSelectedMessage: null,
                pagesListTitle: null,
                created: null,
                lastEdited: null
            },
            keys = {
                addNewPageInfoMessageClosed: 'bcms.addNewPageInfoBoxClosed'
            },
            classes = {
                inputValidationError: 'bcms-input-validation-error'
            },
            pageUrlManuallyEdited = false,
            oldTitleValue = '';

        /**
        * Assign objects to module.
        */
        page.links = links;
        page.globalization = globalization;
        page.senderId = 0;

        function urlGenerator(dialog, addPrefix, actionUrl, titleField, autoGenerate, getParentPageId, getLanguageId, getCategoryId) {
            var self = this;

            self.dialog = dialog;
            self.addPrefix = addPrefix;
            self.actionUrl = actionUrl;
            self.titleField = titleField;
            self.autoGenerate = autoGenerate;

            self.getParentPageId = getParentPageId;
            self.getLanguageId = getLanguageId;
            self.getCategoryId = getCategoryId;

            self.Regenerate = function () {
                var parentPageId, languageId, categoryId;

                if (self.getParentPageId != null && $.isFunction(self.getParentPageId)) {
                    parentPageId = self.getParentPageId();
                }

                if (self.getLanguageId != null && $.isFunction(self.getLanguageId)) {
                    languageId = self.getLanguageId();
                }

                if (self.getCategoryId != null && $.isFunction(self.getCategoryId)) {
                    categoryId = self.getCategoryId();
                }

                page.changeUrlSlug(self.dialog, self.actionUrl, self.titleField, self.addPrefix, parentPageId, languageId, categoryId);
            }

            return self;
        }

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
        function TemplatesListViewModel(templates, dialog, optionsContainer, optionListViewModel, accessControlViewModel) {
            var self = this;

            self.templates = ko.observableArray();
            self.displayedTemplates = ko.observableArray();
            self.searchQuery = ko.observable();
            self.searchEnabled = ko.observable(false);
            self.hasFocus = ko.observable(false);
            self.optionsContainer = optionsContainer;
            self.optionListViewModel = optionListViewModel;
            self.accessControlViewModel = accessControlViewModel;
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
                    dialog.container.find(selectors.addNewPageMasterPageId).val(template.id);
                    dialog.container.find(selectors.addNewPageTemplateId).val('');
                } else {
                    dialog.container.find(selectors.addNewPageTemplateId).val(template.id);
                    dialog.container.find(selectors.addNewPageMasterPageId).val('');
                }

                template.isActive(true);

                pageProperties.loadLayoutOptions(template.id, template.isMasterPage, dialog.container, self.optionsContainer, self.optionListViewModel);
                pageProperties.loadLayoutUserAccess(template.id, template.isMasterPage, dialog.container, self.accessControlViewModel);
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

        page.initializePermalinkBox = function (dialog, addPrefix, actionUrl, titleField, autoGenerate, getParentPageId, getLanguageId, getCategoryId) {
            var generator = new urlGenerator(dialog, addPrefix, actionUrl, titleField, autoGenerate, getParentPageId, getLanguageId, getCategoryId);
            pageUrlManuallyEdited = false;
            oldTitleValue = '';

            dialog.container.find(selectors.editPermalink).on('click', function () {
                page.showAddNewPageEditPermalinkBox(dialog);
            });

            dialog.container.find(selectors.editPermalinkClose).on('click', function () {
                page.closeAddNewPageEditPermalinkBox(dialog);
            });

            dialog.container.find(selectors.editPermalinkSave).on('click', function () {
                page.saveAddNewPageEditPermalinkBox(dialog);
            });

            if (autoGenerate) {
                dialog.container.find(titleField).on('keyup', function () {
                    var newValue = $(this).val() || '';
                    if (newValue != oldTitleValue) {
                        oldTitleValue = newValue;
                        generator.Regenerate();
                    }
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

            return generator;
        };

        page.isEditedPageUrlManually = function () {
            return pageUrlManuallyEdited;
        };
        /**
        * Initializes AddNewPage dialog events.
        */
        page.initAddNewPageDialogEvents = function (dialog, content) {
            var infoMessageClosed = store.get(keys.addNewPageInfoMessageClosed),
                optionsContainer = dialog.container.find(selectors.addNewPageOptionsTab),
                accessControlViewModel = security.createUserAccessViewModel(content.Data.UserAccessList),
                languageViewModel = content.Data.Languages ? new pageLanguages.PageLanguageViewModel(content.Data.Languages) : null,
                optionsViewModel = options.createOptionValuesViewModel(optionsContainer, content.Data.OptionValues, content.Data.CustomOptions),
                viewModel = {
                    accessControl: accessControlViewModel,
                    language: languageViewModel,
                    options: optionsViewModel,
                    templatesList: new TemplatesListViewModel(content.Data.Templates, dialog, optionsContainer, optionsViewModel, accessControlViewModel)
                },
                getLanguageId = function () {
                    if (languageViewModel != null) {
                        return languageViewModel.languageId();
                    }
                    return null;
                };

            var generator = page.initializePermalinkBox(dialog, true, links.convertStringToSlugUrl, selectors.addNewPageTitleInput, true, null, getLanguageId, null);
            if (languageViewModel != null) {
                languageViewModel.languageId.subscribe(function () {
                    generator.Regenerate();
                });
            }

            if (infoMessageClosed && infoMessageClosed === '1') {
                page.hideAddNewPageInfoMessage(dialog);
            } else {
                dialog.container.find(selectors.addNewPageCloseInfoMessage).on('click', function () {
                    store.set(keys.addNewPageInfoMessageClosed, '1');
                    page.hideAddNewPageInfoMessage(dialog);
                });
            }

            ko.applyBindings(viewModel, dialog.container.find(selectors.addNewPageForm).get(0));

            return viewModel;
        };

        /**
        * Shows edit permalink box in AddNewPage dialog.
        */
        page.showAddNewPageEditPermalinkBox = function (dialog) {
            dialog.container.find(selectors.editPermalinkBox).show();
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
        };

        /**
        * Hides info message box in AddNewPage dialog.
        */
        page.hideAddNewPageInfoMessage = function (dialog) {
            dialog.container.find(selectors.addNewPageCloseInfoMessageBox).hide();
        };

        page.changePublishStatus = function (sender) {
            var publish = !sender.hasClass('bcms-switch-on'),
                message = publish ? globalization.pageStatusChangeConfirmationMessagePublish : globalization.pageStatusChangeConfirmationMessageUnPublish,
                data = { PageId: bcms.pageId, IsPublished: publish },
                onComplete = function (json) {
                    modal.showMessages(json);
                    if (json.Success) {
                        setTimeout(function () {
                            bcms.reload();
                        }, 500);

                    }
                };

            modal.confirm({
                content: message,
                onAccept: function () {
                    if (data.IsPublished) {
                        sender.removeClass("bcms-switch-off");
                        sender.addClass("bcms-switch-on");
                    } else {
                        sender.removeClass("bcms-switch-on");
                        sender.addClass("bcms-switch-off");
                    }
                    $.ajax({
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
                        });
                    return true;
                },
                onClose: function () {
                }
            });
        };

        page.openCreatePageDialog = function (postSuccess, addMaster) {
            var permalinkValue,
                url = $.format(links.loadAddNewPageDialogUrl, window.location.pathname, addMaster || false),
                viewModel;

            modal.open({
                title: addMaster === true ? globalization.addNewMasterPageDialogTitle : globalization.addNewPageDialogTitle,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (childDialog, content) {
                            viewModel = page.initAddNewPageDialogEvents(childDialog, content);
                        },

                        beforePost: function () {
                            if (!pageUrlManuallyEdited) {
                                var pageUrlField = dialog.container.find(selectors.editPermalinkEditField);
                                permalinkValue = pageUrlField.val();
                                pageUrlField.val(null);
                            }

                            return viewModel.options.isValid(true);
                        },

                        postError: function () {
                            if (!pageUrlManuallyEdited) {
                                var pageUrlField = dialog.container.find(selectors.editPermalinkEditField);
                                pageUrlField.val(permalinkValue);
                            }
                        },

                        postSuccess: function (data) {
                            if (bcms.trigger(bcms.events.pageCreated, { Data: data.Data, Callback: postSuccess }) <= 0) {
                                if (postSuccess && $.isFunction(postSuccess)) {
                                    postSuccess(data);
                                }
                            }
                        }
                    });
                }
            });
        };

        page.addNewPage = function () {
            page.openCreatePageDialog(function (data) {
                if (data.Data && data.Data.PageUrl) {
                    sidemenu.turnEditModeOn();
                    redirect.RedirectWithAlert(data.Data.PageUrl);
                }
            });
        };

        /**
        * Deletes current page
        */
        page.deleteCurrentPage = function () {
            var id = bcms.pageId;

            page.deletePage(id, function (json) {
                if (json && json.Messages && json.Messages.length > 1) {
                    var message = json.Messages[0];
                    for (var i = 1; i < json.Messages.length; i++) {
                        message = message + "<br>" + json.Messages[i];
                    }
                    modal.info({
                        title: globalization.pageDeletedTitle,
                        content: message,
                        onAcceptClick: function () {
                            redirect.RedirectWithAlert('/', {
                                title: globalization.pageDeletedTitle,
                                message: globalization.pageDeletedMessage
                            });
                        }
                    });
                } else {
                    redirect.RedirectWithAlert('/', {
                        title: globalization.pageDeletedTitle,
                        message: globalization.pageDeletedMessage
                    });
                }
            });
        };

        /**
        * Deletes page
        */
        page.deletePage = function (id, postSuccess, title) {
            title = title || globalization.deletePageDialogTitle;
            modal.confirmDelete({
                title: title,
                acceptTitle: globalization.deleteButtonTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.deletePageConfirmationUrl, id);
                    dynamicContent.bindDialog(dialog, url, {
                        postSuccess: postSuccess
                    });
                },
                onAcceptClick: function() {
                    $('.bcms-error-frame').showLoading();
                }
            });
        };

        /**
        * Changes page slug
        */
        page.changeUrlSlug = function (dialog, actionUrl, titleInput, addPrefix, parentPageId, languageId, categoryId) {
            var oldText = $.trim(dialog.container.find(titleInput).val());
            setTimeout(function () {
                var text = $.trim(dialog.container.find(titleInput).val()),
                    senderId = page.senderId++,
                    onComplete = function (json) {
                        if (json && json.SenderId == senderId && json.Url) {
                            var slug = json.Url,
                                url = (slug ? slug : '');

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
                    var prefix = (addPrefix) ? encodeURIComponent(window.location.pathname) : '';

                    var url = $.format(actionUrl, encodeURIComponent(text), senderId, prefix, parentPageId, languageId);

                    if (categoryId) {
                        for (var i = 0; i < categoryId.length; i++) {
                            var connector = '&';
                            if (url.indexOf('?') === -1) {
                                connector = '?';
                            }
                            url = url.concat(connector + 'categoryId=' + categoryId[i]);
                        }
                    }

                    $.ajax({
                        type: 'GET',
                        url: url,
                        dataType: 'json'
                    })
                        .done(function (result) {
                            onComplete(result);
                        })
                        .fail(function (response) {
                            onComplete(response);
                        });
                }
            }, 400);
        };

        /**
        * Loads site settings page list.
        */
        page.loadSiteSettingsPageList = function (sender) {
            dynamicContent.bindSiteSettings(siteSettings, page.links.loadSiteSettingsPageListUrl, {
                contentAvailable: page.initializeSiteSettingsPagesList
            });
        };

        function onAfterSiteSettingsPageItemSaved(json, item) {
            if (json.Data != null) {
                item.update(json.Data);
            }
        }

        function PagesGridViewModel(data, form, container, opts) {
            var self = this;

            self.selectedRowId = ko.observable("");
            self.items = ko.observableArray();
            self.items.extend({ rateLimit: 50 });
            self.setItems = function (_items) {
                self.items.removeAll();
                for (var i = 0; i < _items.length; i++) {
                    self.items.push(new PageItemViewModel(_items[i], self, opts.canBeSelected));
                }
            }

            self.onOpenPage = function (pageNumber) {
                form.find(selectors.hiddenPageNumberField).val(pageNumber);
                grid.submitGridFormPaged(form, function (content, data) {
                    self.setItems(data.Items);
                });
            }
            self.setItems(data.Items);
            self.paging = new ko.PagingViewModel(data.GridOptions.PageSize, data.GridOptions.PageNumber, data.GridOptions.TotalCount, self.onOpenPage);
            return self;
        };

        /**
        * Initializes site settings pages list and list items
        */
        page.initializeSiteSettingsPagesList = function (content, jsonData, opts, isSearchResult) {
            opts = $.extend({
                dialog: siteSettings.getModalDialog(),
                dialogContainer: siteSettings,
                canBeSelected: false
            }, opts);

            var dialog = opts.dialog,
                container = dialog.container,
                form = dialog.container.find(selectors.siteSettingsPagesListForm);

            page.pagesGridViewModel = new PagesGridViewModel(((content.Data) ? content.Data : jsonData), form, container, opts);
            var gridDOM = container.find(selectors.siteSettingsPagesGrid);
            ko.cleanNode(gridDOM.get(0));
            var pagingDOM = container.find(selectors.siteSettingsPager);
            ko.applyBindings(page.pagesGridViewModel, gridDOM.get(0));
            ko.applyBindings(page.pagesGridViewModel.paging, pagingDOM.get(0));

            form.on('submit', function (event) {
                event.preventDefault();
                page.searchSiteSettingsPages(form, container);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.siteSettingsPagesSearchField), {
                preventedEnter: function () {
                    page.searchSiteSettingsPages(form, container);
                }
            });

            form.find(selectors.siteSettingsPagesSearchButton).on('click', function () {
                var parent = $(this).parent();
                if (!parent.hasClass('bcms-active-search')) {
                    form.find(selectors.siteSettingsPagesSearchField).prop('disabled', false);
                    parent.addClass('bcms-active-search');
                    form.find(selectors.siteSettingsPagesSearchField).focus();
                } else {
                    form.find(selectors.siteSettingsPagesSearchField).prop('disabled', true);
                    parent.removeClass('bcms-active-search');
                    form.find(selectors.siteSettingsPagesSearchField).val('');
                }
            });

            if (isSearchResult) {
                form.find(selectors.siteSettingsPagesSearchButton).parent().addClass('bcms-active-search');
            } else {
                form.find(selectors.siteSettingsPagesSearchField).prop('disabled', true);
            }
            container.find(selectors.siteSettingsPageCreateButton).on('click', function () {
                page.addSiteSettingsPage(container, opts);
            });

            filter.bind(container, ((content.Data) ? content.Data : jsonData), function (usePaging) {
                page.searchSiteSettingsPages(form, container, usePaging);
            }, opts);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(dialog.container.find(selectors.siteSettingsPagesSearchField), true);
        };

        function PageItemViewModel(item, parent, canBeSelected) {
            var self = this;
            self.id = ko.observable(item.Id);
            self.title = ko.observable(item.Title);
            self.createdOn = ko.observable(globalization.created + ' ' + item.CreatedOn);
            self.modifiedOn = ko.observable(globalization.lastEdited + ' ' + item.ModifiedOn);
            self.pageStatus = ko.observable(item.PageStatus);
            self.url = ko.observable(item.PageUrl);
            self.version = ko.observable(item.Version);

            self.update = function(_item) {
                self.id(_item.PageId);
                self.title(_item.Title);
                self.createdOn(globalization.created + ' ' + _item.CreatedOn);
                self.modifiedOn(globalization.lastEdited + ' ' + _item.ModifiedOn);
                self.pageStatus(_item.PageStatus);
                self.url(_item.PageUrl);
                self.version(_item.Version);
            };
            self.canBeSelected = canBeSelected;

            self.onEditClick = function () {
                page.editSiteSettingsPage(self);
            };

            self.onTitleClick = function() {
                window.open(self.url());
            };

            self.onDeleteClick = function() {
                page.deleteSiteSettingsPage(self);
            };
            self.onSelect = function () {
                if (!self.canBeSelected) {
                    return;
                }
                parent.selectedRowId(self.id());
            }
            return self;
        }

        /**
        * Search site settings pages
        */
        page.searchSiteSettingsPages = function (form, container, usePaging) {
            if (usePaging) {
                grid.submitGridFormPaged(form, function(htmlContent, data) {
                    container.find(selectors.siteSettingsPagesSearchField).blur();
                    page.pagesGridViewModel.setItems(data.Items);
                    page.pagesGridViewModel.paging.setPaging(data.GridOptions.PageSize, data.GridOptions.PageNumber, data.GridOptions.TotalCount);
                });
            } else {
                grid.submitGridForm(form, function (htmlContent, data) {
                    // Blur searh field - IE11 fix
                    container.find(selectors.siteSettingsPagesSearchField).blur();
                    page.pagesGridViewModel.setItems(data.Items);
                    page.pagesGridViewModel.paging.setPaging(data.GridOptions.PageSize, data.GridOptions.PageNumber, data.GridOptions.TotalCount);
                });
            }
        };

        /**
        * Opens page create form from site settings pages list
        */
        page.addSiteSettingsPage = function (container, opts) {
            page.openCreatePageDialog(function (json) {
                if (json.Data != null) {
                    onAfterSiteSettingsPageItemSaved(json);
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

        /*
         * Opens edit page form
         */
        page.openEditPageDialog = function (id, postSuccess, title, onLoad) {
            pageProperties.openEditPageDialog(id, postSuccess, title, onLoad);
        };

        /**
            * Opens page edit form from site settings pages list
            */
        page.editSiteSettingsPage = function (item) {
            var id = item.id();
            var container = siteSettings.getModalDialog().container;
            page.openEditPageDialog(id, function (data) {
                if (data.Data != null) {
                    if (data.Data.IsArchived) {
                        var form = container.find(selectors.siteSettingsPagesListForm),
                            includeArchivedField = form.find(selectors.siteSettingsPagesListFormFilterIncludeArchived);
                        if (!includeArchivedField.is(":checked")) {
                            page.pagesGridViewModel.items.remove(item);
                            return;
                        }
                    }
                    onAfterSiteSettingsPageItemSaved(data, item);
                }
            }, item.title());
        };

        /**
        * Deletes page from site settings pages list
        */
        page.deleteSiteSettingsPage = function (item, container) {

            page.deletePage(item.id(), function (json) {
                page.pagesGridViewModel.items.remove(item);
            });
        };

        function clonePage(url, title, onLoad) {
            var permalinkValue;

            modal.open({
                title: title,
                acceptTitle: globalization.cloneButtonTitle,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (childDialog, content) {
                            var viewModel = {
                                accessControl: security.createUserAccessViewModel(content.Data.UserAccessList)
                            },
                                form = dialog.container.find(selectors.clonePageForm),
                                languageSelector = form.find(selectors.languageSelection),
                                getParentPageId = function () {
                                    return bcms.pageId;
                                },
                                getLanguageId = function () {
                                    return languageSelector != null ? languageSelector.val() : null;
                                },
                                generator = page.initializePermalinkBox(dialog, false, links.convertStringToSlugUrl, selectors.addNewPageTitleInput, true, getParentPageId, getLanguageId);
                            if (languageSelector != null) {
                                languageSelector.on('change', function () {
                                    generator.Regenerate();
                                });
                                $(languageSelector[0]).select2({
                                    minimumResultsForSearch: -1
                                });
                            }

                            if (form.length > 0) {
                                ko.applyBindings(viewModel, form.get(0));
                            } else {
                                $(modal.selectors.accept).hide();
                            }

                            if ($.isFunction(onLoad)) {
                                onLoad(childDialog, content);
                            }
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
                                var postSuccess = function (data) {
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
        }

        /**
        * Opens dialog for clone the page
        */
        page.clonePage = function () {
            var url = $.format(links.clonePageDialogUrl, bcms.pageId),
                title = globalization.clonePageDialogTitle;

            clonePage(url, title);
        };

        /**
        * Opens dialog for cloning the page to different cultre
        */
        page.translatePage = function () {
            var url = $.format(links.clonePageWithLanguageDialogUrl, bcms.pageId),
                title = globalization.clonePageWithLanguageDialogTitle;

            clonePage(url, title, function (clonePageDialog) {
                clonePageDialog.container.find(selectors.languageSelection).on('change', function () {
                    var select = $(this);
                    if (select.hasClass(classes.inputValidationError)) {
                        select.removeClass(classes.inputValidationError);
                    }
                });
                clonePageDialog.container.find(selectors.cloneWithLanguageGoToPagePropertiesLink).on('click', function () {
                    clonePageDialog.close();
                    pageProperties.editPageProperties(function (pagePropertiesDialog) {
                        // Open translations tab
                        pagePropertiesDialog.container.find(selectors.pagePropertiesTranslationsTab).click();
                    });
                });
            });
        };

        /**
        * Initialize custom jQuery validators
        */
        function initializeCustomValidation() {
            $.validator.addMethod("jqpageurlvalidation", function (value, element, params) {
                if (pageUrlManuallyEdited) {
                    if (!value) {
                        return false;
                    }
                    var match = new RegExp(params.pattern).exec(value);
                    return (match && (match.index === 0) && (match[0].length === value.length));
                }

                return true;
            }, function (params) {
                return params.message;
            });

            $.validator.unobtrusive.adapters.add("pageurlvalidation", ['pattern'], function (opts) {
                opts.rules["jqpageurlvalidation"] = { message: opts.message, pattern: opts.params.pattern };
            });

            $.validator.addMethod("jqenddatevalidation", function (value, element, params) {
                var startDateString = $('#' + params.startdateproperty).val();
                if (value != null && value != "" && startDateString != null && startDateString != "") {
                    return datepicker.parseDate(startDateString) <= datepicker.parseDate(value);
                }
                return true;
            }, function (params) {
                return params.message;
            });

            $.validator.unobtrusive.adapters.add("enddatevalidation", ['startdateproperty'], function (opts) {
                opts.rules["jqenddatevalidation"] = { message: opts.message, startdateproperty: opts.params.startdateproperty };
            });
        }

        /**
        * Show folder selection window
        */
        page.openPageSelectDialog = function (opts, pageGridOptions) {

            opts = $.extend({
                onAccept: function () { return true; },
                onClose: function () { return true; },
                url: links.loadSelectPageUrl,
                params: {},
                canBeSelected: true,
                title: globalization.selectPageDialogTitle,
                disableAccept: false
            }, opts);

            var url = opts.params
                    ? (opts.url.indexOf('?') > 0
                        ? opts.url + '&' + $.param(opts.params)
                        : opts.url + '?' + $.param(opts.params))
                    : links.loadSelectPageUrl,
                selectDialog = modal.open({
                    title: opts.title,
                    acceptTitle: globalization.selectPageSelectButtonTitle,
                    disableAccept: opts.disableAccept,
                    onClose: function () {
                        var result = true;
                        if ($.isFunction(opts.onClose)) {
                            result = opts.onClose();
                        }
                        return result;
                    },
                    onLoad: function (dialog) {
                        dynamicContent.setContentFromUrl(dialog, url, {
                            done: function (content) {
                                page.initializeSiteSettingsPagesList(content.Html, content.Data, $.extend({
                                    dialog: dialog,
                                    dialogContainer: selectDialog,
                                    canBeSelected: opts.canBeSelected
                                }, pageGridOptions));
                            }
                        });
                    },
                    onAcceptClick: function () {
                        var selectedItem = selectDialog.container.find(selectors.siteSettingsPagesTableActiveRow),
                            id = selectedItem.find(selectors.siteSettingsPageEditButton).data('id'),
                            titleLink = selectedItem.find(selectors.siteSettingPageTitleCell);

                        if (opts.canBeSelected) {
                            if (selectedItem.length == 0) {
                                modal.info({
                                    content: globalization.pageNotSelectedMessage,
                                    disableCancel: true
                                });

                                return false;
                            }
                        }

                        if ($.isFunction(opts.onAccept)) {
                            return opts.onAccept({
                                Id: id,
                                Title: titleLink.html(),
                                PageUrl: titleLink.data('url'),
                                LanguageId: titleLink.data('languageId')
                            });
                        }

                        return true;
                    }
                });

            return selectDialog;
        };

        /**
        * Initializes page module.
        */
        page.init = function () {
            bcms.logger.debug('Initializing bcms.pages module.');

            initializeCustomValidation();

            // Pass method to page languagess (it can be reached directly because of circular references)
            pageLanguages.openPageSelectDialog = page.openPageSelectDialog;
        };

        /**
        * Register initialization
        */
        bcms.registerInit(page.init);

        return page;
    });