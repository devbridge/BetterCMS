bettercms.define('bcms.pages.languages', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.ko.extenders', 'bcms.autocomplete', 'bcms.antiXss'],
    function ($, bcms, modal, ko, autocomplete, antiXss) {
        'use strict';

        var pageLanguages = {
            openPageSelectDialog: null
        },
            selectors = {

            },
            links = {
                suggestUntranslatedPagesUrl: null,
                searchUntranslatedPagesUrl: null
            },
            globalization = {
                unassignTranslationConfirmation: null,
                invariantLanguage: null,
                replaceItemWithCurrentLanguageConfirmation: null,
                replaceItemWithLanguageConfirmation: null,
                assigningPageHasSameCultureAsCurrentPageMessage: null
            };

        /**
        * Assign objects to module.
        */
        pageLanguages.links = links;
        pageLanguages.globalization = globalization;
        pageLanguages.selectors = selectors;

        /**
        * Untranslated pages autocomplete list view model
        */
        var UntranslatedPagesAutocompleteViewModel = (function (_super) {
            bcms.extendsClass(UntranslatedPagesAutocompleteViewModel, _super);

            function UntranslatedPagesAutocompleteViewModel(onItemSelect, onAppendWithIncludedIds) {
                var opts = {
                    serviceUrl: links.suggestUntranslatedPagesUrl,
                    onItemSelect: onItemSelect
                };

                _super.call(this, opts);

                this.onAppendWithIncludedIds = onAppendWithIncludedIds;
            }

            UntranslatedPagesAutocompleteViewModel.prototype.getAdditionalParameters = function () {
                var params = _super.prototype.getAdditionalParameters.call(this);

                if ($.isFunction(this.onAppendWithIncludedIds)) {
                    params = this.onAppendWithIncludedIds(params);
                }

                return params;
            };

            return UntranslatedPagesAutocompleteViewModel;
        })(autocomplete.AutocompleteViewModel);

        /**
        * Page language view model
        */
        pageLanguages.PageLanguageViewModel = function (languages, languageId) {
            var self = this,
                i, l;

            self.languageId = ko.observable(languageId);
            self.suspendLanguageCheck = false;

            self.languages = [];
            self.languages.push({ key: '', value: globalization.invariantLanguage });
            for (i = 0, l = languages.length; i < l; i++) {
                self.languages.push({
                    key: languages[i].Key,
                    value: languages[i].Value
                });
            }

            return self;
        };

        /**
        * Page translations list view model
        */
        pageLanguages.PageTranslationsListViewModel = function (items, languages, languageId, pageId) {
            var self = this;

            self.language = new pageLanguages.PageLanguageViewModel(languages, languageId);
            self.items = ko.observableArray();
            self.isInAddMode = ko.observable(false);
            self.hasFocus = ko.observable(false);
            self.addLanguageId = ko.observable();
            self.addPageId = ko.observable();
            self.addPageTitle = ko.observable();

            self.oldCurrentPageLanguageId = languageId;
            self.currentPageId = pageId;
            self.pageLanguageId = null;
            self.pageUrl = null;
            self.originalItems = [];

            function onSelectPage(pageId, pageLanguageId, pageTitle, pageUrl) {

                if (pageLanguageId == self.language.languageId()) {
                    modal.info({
                        content: $.format(globalization.assigningPageHasSameCultureAsCurrentPageMessage, pageTitle)
                    });
                    return false;
                }

                var languagesList;

                self.pageLanguageId = pageLanguageId;
                self.pageUrl = pageUrl;
                self.addPageId(pageId);
                self.addPageTitle(pageTitle);

                languagesList = self.addingPageLanguages();
                if (languagesList.length > 0) {
                    self.addLanguageId(languagesList[0].key);
                } else {
                    self.addLanguageId('');
                }

                return true;
            }

            function closeAddMode() {
                self.hasFocus(false);
                self.isInAddMode(false);
                self.addLanguageId('');
                self.addPageId('');

                self.pageLanguageId = null;
                self.addPageTitle('');
                self.pageUrl = null;
            }

            function appendId(ids, id) {
                if (ids !== '') {
                    ids += '|';
                }
                ids += id;

                return ids;
            }

            function getAdditionalParameters() {
                var params = {},
                    include = '',
                    exclude = self.currentPageId,
                    il = self.originalItems.length,
                    jl = self.items().length,
                    i, j, exists, id;

                for (i = 0; i < il; i++) {
                    id = self.originalItems[i];
                    exists = false;

                    for (j = 0; j < jl; j++) {
                        if (self.items()[j].id() == id) {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists) {
                        include = appendId(include, id);
                    }
                }

                for (j = 0; j < jl; j++) {
                    exclude = appendId(exclude, self.items()[j].id());
                }

                params.ExcplicitlyIncludedPages = include;
                params.ExistingItems = exclude;
                params.ExcludedLanguageId = self.language.languageId();

                return params;
            }

            function setAdditionalParameters(params) {
                return $.extend(getAdditionalParameters(), params);
            };

            function onLanguageChange(newValue) {
                var il = self.items().length,
                    i, item,
                    hasError = false;

                closeAddMode();

                for (i = 0; i < il; i++) {
                    item = self.items()[i];

                    if (!self.language.suspendLanguageCheck && item.languageId() == newValue) {
                        hasError = true;
                        modal.confirm({
                            content: $.format(globalization.replaceItemWithCurrentLanguageConfirmation, item.languageName()),
                            onAccept: function() {
                                self.items.remove(item);
                                self.oldCurrentPageLanguageId = newValue;

                                return true;
                            },
                            onClose: function() {
                                self.language.suspendLanguageCheck = true;
                                self.language.languageId(self.oldCurrentPageLanguageId);
                                self.language.suspendLanguageCheck = false;

                                return true;
                            }
                        });

                        break;
                    }
                }

                if (!hasError) {
                    self.oldCurrentPageLanguageId = newValue;
                }
            }

            self.autocompleteViewModel = new UntranslatedPagesAutocompleteViewModel(function (suggestionItem, jsonItem) {
                return onSelectPage(suggestionItem.id(), jsonItem.LanguageId, suggestionItem.name(), jsonItem.PageUrl);
            }, setAdditionalParameters);

            self.language.languageId.subscribe(onLanguageChange);

            self.startEditMode = function () {
                if (!self.isInAddMode()) {
                    self.hasFocus(true);
                    self.isInAddMode(true);
                }
            };

            self.endEditMode = function () {
                closeAddMode();
            };

            self.selectPage = function () {
                pageLanguages.openPageSelectDialog({
                    onAccept: function (selectedPage) {
                        return onSelectPage(selectedPage.Id, selectedPage.LanguageId, selectedPage.Title, selectedPage.PageUrl);
                    },
                    params: getAdditionalParameters(),
                    url: links.searchUntranslatedPagesUrl
                });
            };

            self.unassignPage = function (item) {
                modal.confirm({
                    content: globalization.unassignTranslationConfirmation,
                    onAccept: function () {
                        closeAddMode();
                        self.items.remove(item);

                        return true;
                    }
                });
            };

            self.addingPageLanguages = ko.computed(function () {
                if (!self.addPageId()) {
                    return [];
                }

                var lang = [],
                    li = self.language.languages.length,
                    currentLanguageId = self.language.languageId(),
                    selectedLanguageId = self.pageLanguageId,
                    i, language;

                for (i = 0; i < li; i++) {
                    language = self.language.languages[i];

                    if (language.key != currentLanguageId && (selectedLanguageId == language.key || !selectedLanguageId)) {
                        lang.push({
                            key: language.key || bcms.constants.emptyGuid,
                            value: language.value || globalization.invariantLanguage
                        });
                    }
                }

                return lang;
            });

            self.addTranslation = function () {
                if (self.isInAddMode() && self.addLanguageId() && self.addPageId()) {
                    var currentLanguageId = self.addLanguageId() == bcms.constants.emptyGuid ? '' : self.addLanguageId(),
                        addTranslation = function (viewModel) {
                            var li = self.language.languages.length,
                                i, language;

                            for (i = 0; i < li; i++) {
                                language = self.language.languages[i];

                                if (language.key == currentLanguageId) {
                                    if (viewModel == null) {
                                        viewModel = new pageLanguages.PageTranslationViewModel(self);
                                        self.items.push(viewModel);
                                    }
                                    viewModel.id(self.addPageId());
                                    viewModel.title(self.addPageTitle());
                                    viewModel.url(self.pageUrl);
                                    viewModel.languageId(currentLanguageId);
                                    viewModel.languageName(language.value || globalization.invariantLanguage);

                                    closeAddMode();
                                }
                            }
                        },
                        lj = self.items().length,
                        j, item;

                    for (j = 0; j < lj; j++) {
                        item = self.items()[j];

                        if (item.languageId() == currentLanguageId) {
                            self.hasFocus(false);
                            modal.confirm({
                                content: globalization.replaceItemWithLanguageConfirmation,
                                onAccept: function () {
                                    addTranslation(item);

                                    return true;
                                },
                                onClose: function () {
                                    self.hasFocus(true);
                                }
                            });

                            return;
                        }
                    }

                    addTranslation();
                }
            };

            function addItems() {
                if (!items) {
                    items = [];
                }

                var lj = items.length,
                    li = self.language.languages.length,
                    viewModel, i, j, language, item, translation;

                // Add all available languages
                for (i = 0; i < li; i++) {
                    language = self.language.languages[i];
                    translation = null;

                    for (j = 0; j < lj; j++) {
                        item = items[j];

                        if ((item.LanguageId || '') == language.key) {
                            translation = item;
                            break;
                        }
                    }

                    if (translation && translation.Id != self.currentPageId) {
                        viewModel = new pageLanguages.PageTranslationViewModel(self);
                        viewModel.id(translation.Id);
                        viewModel.title(antiXss.encodeHtml(translation.Title));
                        viewModel.url(translation.PageUrl);
                        viewModel.languageName(language.value || globalization.invariantLanguage);
                        viewModel.languageId(language.key);

                        self.items.push(viewModel);
                        self.originalItems.push(translation.Id);
                    }
                }
            };

            addItems();

            return self;
        };

        /**
        * Page translation view model
        */
        pageLanguages.PageTranslationViewModel = function (language, translation, parent) {
            var self = this;

            self.parent = parent;
            self.id = ko.observable(translation ? translation.Id : '');
            self.title = ko.observable(translation ? translation.Title : '');
            self.url = ko.observable(translation ? translation.PageUrl : '');
            self.languageName = ko.observable(language ? language.value || globalization.invariantLanguage : '');
            self.languageId = ko.observable(language ? language.key : '');

            self.getPropertyIndexer = function (i, propName) {
                return 'Translations[' + i + '].' + propName;
            };

            return self;
        };

        /**
        * Initializes page languages module.
        */
        pageLanguages.init = function () {
            bcms.logger.debug('Initializing bcms.pages.languages module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(pageLanguages.init);

        return pageLanguages;
    });