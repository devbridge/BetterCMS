bettercms.define('bcms.pages.cultures', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.ko.extenders','bcms.autocomplete'],
    function($, bcms, modal, ko, autocomplete) {
        'use strict';

        var pageCultures = {
                openPageSelectDialog: null
            },
            selectors = {
                
            },
            links = {
                suggestUntranslatedPagesUrl: null
            },
            globalization = {
                unassignTranslationConfirmation: null,
                invariantCulture: null
            };
        
        /**
        * Assign objects to module.
        */
        pageCultures.links = links;
        pageCultures.globalization = globalization;
        pageCultures.selectors = selectors;

        /**
        * Culture autocomplete list view model
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
                    this.onAppendWithIncludedIds(params);
                }

                return params;
            };

            return UntranslatedPagesAutocompleteViewModel;
        })(autocomplete.AutocompleteViewModel);

        /**
        * Page culture view model
        */
        pageCultures.PageCultureViewModel = function (cultures, cultureId) {
            var self = this,
                i, l;

            self.cultureId = ko.observable(cultureId);

            self.cultures = [];
            self.cultures.push({ key: '', value: '' });
            for (i = 0, l = cultures.length; i < l; i++) {
                self.cultures.push({
                    key: cultures[i].Key,
                    value: cultures[i].Value
                });
            }

            return self;
        };

        /**
        * Page translations list view model
        */
        pageCultures.PageTranslationsListViewModel = function (items, cultures, cultureId) {
            var self = this;

            self.culture = new pageCultures.PageCultureViewModel(cultures, cultureId);
            self.items = ko.observableArray();
            self.isInAddMode = ko.observable(false);
            self.hasFocus = ko.observable(false);
            self.addCultureId = ko.observable();
            self.addPageId = ko.observable();
            self.addPageTitle = ko.observable();

            self.oldCurrentPageCultureId = cultureId;
            self.pageCultureId = null;
            self.pageUrl = null;
            self.originalItems = [];

            function onSelectPage(pageId, pageCultureId, pageTitle, pageUrl) {
                self.pageCultureId = pageCultureId;
                self.pageUrl = pageUrl;
                self.addCultureId('');
                self.addPageId(pageId);
                self.addPageTitle(pageTitle);

                return true;
            }

            function closeAddMode() {
                self.hasFocus(false);
                self.isInAddMode(false);
                self.addCultureId('');
                self.addPageId('');

                self.pageCultureId = null;
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

            function setAdditionalParameters(params) {
                var include = '',
                    exclude = bcms.pageId,
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
                params.ExcludedCultureId = self.culture.cultureId();
            };

            function onCultureChange(newValue) {
                var il = self.items().length,
                    i, item;

                closeAddMode();

                for (i = 0; i < il; i++) {
                    item = self.items()[i];

                    if (item.cultureId() == newValue) {
                        modal.confirm({
                            // TODO: translate
                            content: $.format("There is already translation with current culture. If you change current page's culture to {0}, translation will be removed from list. Are you sure you want to continue?", item.cultureName()),
                            onAccept: function () {
                                self.items.remove(item);
                                self.oldCurrentPageCultureId = newValue;

                                return true;
                            },
                            onClose: function () {
                                self.culture.cultureId(self.oldCurrentPageCultureId);

                                return true;
                            }
                        });

                        break;
                    }
                }
            }

            self.autocompleteViewModel = new UntranslatedPagesAutocompleteViewModel(function (suggestionItem, jsonItem) {
                return onSelectPage(suggestionItem.id(), jsonItem.CultureId, suggestionItem.name(), jsonItem.PageUrl);
            }, setAdditionalParameters);

            self.culture.cultureId.subscribe(onCultureChange);

            self.clickPlus = function () {
                var isInAddMode = !self.isInAddMode();
                if (!isInAddMode) {
                    closeAddMode();
                } else {
                    self.hasFocus(true);
                    self.isInAddMode(true);
                }
            };

//            self.selectPage = function() {
//                pageCultures.openPageSelectDialog({
//                    onAccept: function (selectedPage) {
//                        onSelectPage(selectedPage.Id, selectedPage.CultureId, selectedPage.Title, selectedPage.PageUrl);
//                    }
//                });
//            };

            self.unassignPage = function (item) {
                modal.confirm({
                    content: globalization.unassignTranslationConfirmation,
                    onAccept: function () {
                        self.items.remove(item);

                        return true;
                    }
                });
            };

            self.addingPageCultures = ko.computed(function () {
                if (!self.addPageId()) {
                    return [];
                }

                var cult = [],
                    li = self.culture.cultures.length,
                    currentCultureId = self.culture.cultureId(),
                    selectedCultureId = self.pageCultureId,
                    i, culture;

                for (i = 0; i < li; i++) {
                    culture = self.culture.cultures[i];

                    if (culture.key != currentCultureId && (selectedCultureId == culture.key || !selectedCultureId)) {
                        cult.push({
                            key: culture.key || '-',
                            value: culture.value || globalization.invariantCulture
                        });
                    }
                }

                return cult;
            });

            self.addTranslation = function () {
                if (self.isInAddMode() && self.addCultureId() && self.addPageId()) {
                    var currentCultureId = self.addCultureId() == '-' ? '' : self.addCultureId(),
                        addTranslation = function (viewModel) {
                            var li = self.culture.cultures.length,
                                i, culture;

                            for (i = 0; i < li; i++) {
                                culture = self.culture.cultures[i];

                                if (culture.key == currentCultureId) {
                                    if (viewModel == null) {
                                        viewModel = new pageCultures.PageTranslationViewModel(self);
                                        self.items.push(viewModel);
                                    }
                                    viewModel.id(self.addPageId());
                                    viewModel.title(self.addPageTitle());
                                    viewModel.url(self.pageUrl);
                                    viewModel.cultureId(currentCultureId);
                                    viewModel.cultureName(culture.value || globalization.invariantCulture);

                                    closeAddMode();
                                }
                            }
                        },
                        lj = self.items().length,
                        j, item;

                    for (j = 0; j < lj; j++) {
                        item = self.items()[j];

                        if (item.cultureId() == currentCultureId) {
                            self.hasFocus(false);
                            modal.confirm({
                                // TODO: translate
                                content: 'There is already an item with selected culture. Are you sure you want to replace it?',
                                onAccept: function () {
                                    addTranslation(item);

                                    return true;
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
                    li = self.culture.cultures.length,
                    viewModel, i, j, culture, item, translation;

                // Add all available cultures
                for (i = 0; i < li; i++) {
                    culture = self.culture.cultures[i];
                    translation = null;

                    for (j = 0; j < lj; j++) {
                        item = items[j];

                        if ((item.CultureId || '') == culture.key) {
                            translation = item;
                            break;
                        }
                    }

                    if (translation && translation.Id != bcms.pageId) {
                        viewModel = new pageCultures.PageTranslationViewModel(self);
                        viewModel.id(translation.Id);
                        viewModel.title(translation.Title);
                        viewModel.url(translation.PageUrl);
                        viewModel.cultureName(culture.value || globalization.invariantCulture);
                        viewModel.cultureId(culture.key);

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
        pageCultures.PageTranslationViewModel = function (culture, translation, parent) {
            var self = this;

            self.parent = parent;
            self.id = ko.observable(translation ? translation.Id : '');
            self.title = ko.observable(translation ? translation.Title : '');
            self.url = ko.observable(translation ? translation.PageUrl : '');
            self.cultureName = ko.observable(culture ? culture.value || globalization.invariantCulture : '');
            self.cultureId = ko.observable(culture ? culture.key : '');

            self.getPropertyIndexer = function (i, propName) {
                return 'Translations[' + i + '].' + propName;
            };

            return self;
        };

        /**
        * Initializes page cultures module.
        */
        pageCultures.init = function() {
            bcms.logger.debug('Initializing bcms.pages.cultures module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(pageCultures.init);

        return pageCultures;
    });