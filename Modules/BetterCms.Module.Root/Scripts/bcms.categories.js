bettercms.define("bcms.categories", ["bcms.jquery", "bcms", "bcms.siteSettings", "bcms.dynamicContent", "bcms.grid", "bcms.messages", "bcms.modal", "bcms.forms", "bcms.ko.extenders", 'bcms.autocomplete', 'bcms.antiXss'],
    function ($, bcms, siteSettings, dynamicContent, grid, messages, modal, forms, ko, autocomplete, antiXss) {
        "use strict";

        var module = {},
            links = {
                loadSiteSettingsCategoryTreesListUrl: null,
                categoryTreeEditDialogUrl: null,
                saveCategoryTreeUrl: null,
                deleteCategoryTreeUrl: null,
                categoriesSuggestionServiceUrl: null
            },
            globalization = {
                categoryTreeCreatorDialogTitle: null,
                categoryTreeEditorDialogTitle: null,
                nodeOkButton: null,
                placeNodeHere: null,
                categoryTreeIsEmpty: null,
                deleteCategoryNodeConfirmationMessage: null,
                someCategoryNodesAreInEditingState: null,
                categoryTreeDeleteConfirmMessage: null
            },
            selectors = {
                siteSettingsCategoryTreesForm: "#bcms-categorytrees-form",
                searchField: ".bcms-search-query",
                searchButton: "#bcms-categorytrees-search-btn",

                siteSettingsGridItemCreateButton: "#bcms-create-categorytree-button",
                siteSettingsGridItemEditButton: ".bcms-grid-item-edit-button",
                siteSettingsGridItemDeleteButton: ".bcms-grid-item-delete-button",

                siteSettingsGridTableFirstRow: "table.bcms-tables > tbody > tr:first",
                siteSettingsGridRowTitleCell: ".bcms-grid-item-title",
                siteSettingsGridRowCells: "td",
                siteSettingsGridRow: "tr",
                siteSettingsGridRowTemplate: "#bcms-categorytree-list-row-template",
                siteSettingsGridRowTemplateFirstRow: "tr:first",

                categoryTreeForm: ".bcms-categorytree-form",

                categoryTreeAddNodeDataBind: "#bcms-categorytree-addnode",
                firstTab: "#bcms-tab-1",
                scrollDiv: "#bcms-scroll-window"
            },
            defaultIdValue = "00000000-0000-0000-0000-000000000000",
            DropZoneTypes = {
                None: "none",
                EmptyListZone: "emptyListZone",
                TopZone: "topZone",
                MiddleZone: "middleZone",
                BottomZone: "bottomZone"
            },
            nodeId = 0;

        /**
        * Assign objects to module.
        */
        module.links = links;
        module.globalization = globalization;
        module.activeMapModel = null;
        module.activeMessageContainer = null;
        module.activeLoadingContainer = null;


        // --- Helpers --------------------------------------------------------

        function showMessage(content) {
            messages.refreshBox(module.activeMessageContainer, content);
        }

        function updateValidation() {
            var form = $(selectors.categoryTreeForm);
            bcms.updateFormValidator(form);
        }

        function updateFirstLastNode(nodes) {
            var firstNotDeletedNode = null,
                lastNotDeletedNode = null;
            for (var i = 0; i < nodes.length; i++) {
                var node = nodes[i];
                if (!node.isDeleted() && !node.isBeingDragged()) {
                    if (firstNotDeletedNode == null) {
                        firstNotDeletedNode = node;
                    }
                    lastNotDeletedNode = node;
                }
                node.isFirstNode(false);
                node.isLastNode(false);
            }
            if (firstNotDeletedNode != null) {
                firstNotDeletedNode.isFirstNode(true);
            }
            if (lastNotDeletedNode != null) {
                lastNotDeletedNode.isLastNode(true);
            }
        }

        function showLoading(loading) {
            if (loading) {
                $(module.activeLoadingContainer).showLoading();
            } else {
                $(module.activeLoadingContainer).hideLoading();
            }
        }

        /**
        * Helper function to add knockout binding 'draggableCategoryNode'.
        */
        function addDraggableBinding() {
            ko.bindingHandlers.draggableCategoryNode = {
                init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                    var dragObject = viewModel,
                        setup = {
                            revert: true,
                            revertDuration: 0,
                            refreshPositions: true,
                            scroll: true,
                            containment: $($(selectors.categoryTreeAddNodeDataBind).get(0)).find(selectors.scrollDiv).get(0),
                            appendTo: $($(selectors.categoryTreeAddNodeDataBind).get(0)).find(selectors.scrollDiv).get(0),
                            helper: function () {
                                if (dragObject.isExpanded) {
                                    dragObject.isExpanded(false);
                                }
                                if (dragObject.isBeingDragged) {
                                    dragObject.isBeingDragged(true);
                                }
                                return $(this).clone().width($(this).width()).css({ zIndex: 9999 });
                            },
                            start: function () {
                                $(this).hide();
                            },
                            stop: function (event, ui) {
                                ui.helper.remove();
                                $(this).show();
                                if (dragObject.isBeingDragged) {
                                    dragObject.isBeingDragged(false);

                                }
                            }
                        };
                    if (!dragObject.superDraggable() && dragObject.getSitemap && !dragObject.getSitemap().settings.canDragNode) {
                        return;
                    }
                    $(element).draggable(setup).data("dragObject", dragObject);
                    //$(element).disableSelection(); //commented because it is not possible to put pointer using mouse to URL field.
                }
            };
        }

        /**
        * Helper function to add knockout binding 'droppableCategoryNode'.
        */
        function addDroppableBinding() {
            ko.bindingHandlers.droppableCategoryNode = {
                init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                    var dropZoneObject = viewModel,
                        dropZoneType = valueAccessor(),
                        setup = {
                            tolerance: "pointer",
                            over: function () {
                                dropZoneObject.activeZone(dropZoneType);
                            },
                            out: function () {
                                dropZoneObject.activeZone(DropZoneTypes.None);
                            },
                            drop: function (event, ui) {
                                var forFix = $(ui.draggable).data("draggable"),
                                    dragObject = $(ui.draggable).data("dragObject"),
                                    originalDragObject = dragObject;
                                ui.helper.remove();

                                if (dragObject.parentNode && dragObject.parentNode()) {
                                    dragObject.parentNode().childNodes.remove(dragObject);
                                } else {
                                    // Create node from page link.
                                    var node = new NodeViewModel();
                                    node.title(dragObject.title());
                                    if (dropZoneType == DropZoneTypes.EmptyListZone || dropZoneType == DropZoneTypes.MiddleZone) {
                                        node.parentNode(dropZoneObject);
                                    } else {
                                        node.parentNode(dropZoneObject.parentNode());
                                    }
                                    if (dragObject.isCustom()) {
                                        node.dropOnCancel = true;
                                        node.startEditCategoryTreeNode();
                                        node.callbackAfterSuccessSaving = function () {
                                            module.activeMapModel.updateNodesOrderAndParent();
                                        };
                                        node.callbackAfterFailSaving = function (newNode) {
                                            newNode.parentNode().childNodes.remove(newNode);
                                        };
                                    }
// TODO:                              if (module.activeMapModel.showLanguages()) {
//                                        node.updateLanguageOnDropNewNode(dragObject.languageId(), sitemap.activeMapModel.languageId());
//                                    }
                                    node.superDraggable(dragObject.superDraggable());
                                    dragObject = node;
                                }

                                dragObject.isBeingDragged(false);
                                dragObject.displayOrder(0);

                                // Add node to tree.
                                var index;
                                if (dropZoneType == DropZoneTypes.EmptyListZone) {
                                    dropZoneObject.childNodes.splice(0, 0, dragObject);
                                }
                                else if (dropZoneType == DropZoneTypes.TopZone) {
                                    index = $.inArray(dropZoneObject, dropZoneObject.parentNode().childNodes());
                                    dropZoneObject.parentNode().childNodes.splice(index, 0, dragObject);
                                }
                                else if (dropZoneType == DropZoneTypes.MiddleZone) {
                                    dropZoneObject.childNodes.splice(0, 0, dragObject);
                                    dropZoneObject.isExpanded(true);
                                }
                                else if (dropZoneType == DropZoneTypes.BottomZone) {
                                    index = $.inArray(dropZoneObject, dropZoneObject.parentNode().childNodes());
                                    dropZoneObject.parentNode().childNodes.splice(index + 1, 0, dragObject);
                                }
                                dropZoneObject.activeZone(DropZoneTypes.None);

                                module.activeMapModel.updateNodesOrderAndParent();

                                // Fix for jQuery drag object.
                                $(ui.draggable).data("draggable", forFix);

                                if (originalDragObject.dropped && $.isFunction(originalDragObject.dropped)) {
                                    originalDragObject.dropped(dragObject);
                                }

                                updateValidation();
                            }
                        };
                    if (dropZoneObject.getCategoryTree && !dropZoneObject.getCategoryTree().settings.canDropNode) {
                        return;
                    }
                    $(element).droppable(setup);
                }
            };
        }

        // --------------------------------------------------------------------

        function NodeViewModel() {
            var self = this;

            // Data fields.
            self.id = ko.observable(defaultIdValue);
            self.version = ko.observable(0);
            self.title = ko.observable();
            self.macro = ko.observable();
            self.displayOrder = ko.observable(0);
            self.isDeleted = ko.observable(false);
            self.isDeleted.subscribe(function () {
                if (self.parentNode()) {
                    updateFirstLastNode(self.parentNode().childNodes());
                }
            });
            self.childNodes = ko.observableArray([]);
            self.childNodes.subscribe(updateFirstLastNode);
            self.parentNode = ko.observable();
// TODO:
//            self.translationsEnabled = false;
//            self.translations = {};
//            self.activeTranslation = ko.observable(null);

            // For behavior.
            self.isActive = ko.observable(false);           // If TRUE - show edit fields.
            self.isExpanded = ko.observable(false);         // If TRUE - show child nodes.
            self.toggleExpand = function () {
                self.isExpanded(!self.isExpanded());
            };
            self.isBeingDragged = ko.observable(false);     // Someone is dragging the node.
            self.isBeingDragged.subscribe(function () {
                if (self.parentNode()) {
                    updateFirstLastNode(self.parentNode().childNodes());
                }
            });
            self.activeZone = ko.observable(DropZoneTypes.None);
            self.isFirstNode = ko.observable(false);
            self.isLastNode = ko.observable(false);
            self.hasChildNodes = function () {
                // Has at least one not deleted child node.
                var nodes = self.childNodes();
                for (var i = 0; i < nodes.length; i++) {
                    if (!nodes[i].isDeleted()) {
                        return true;
                    }
                }
                return false;
            };
            self.superDraggable = ko.observable(false);     // Used to force dragging if tree settings !canDragNode.
            self.getNodeHeight = ko.computed(function () {
                if (self.isActive()) {
                    if (self.getCategoryTree().showMacros) {
                        return "70px";
                    }
                    return "33px";
                }
                return "";
            });
            self.dropOnCancel = false;

            // User for validation.
            self.containerId = "node-" + nodeId++;

            // For search results.
            self.isVisible = ko.observable(true);
            self.isSearchResult = ko.observable(false);

            // Data manipulation.
            self.startEditCategoryTreeNode = function () {
                self.titleOldValue = self.title();
                self.macroOldValue = self.macro();
                self.isActive(true);
            };
            self.cancelEditCategoryTreeNode = function () {
                self.isActive(false);
                self.title(self.titleOldValue);
                self.macro(self.macroOldValue);
                if (self.dropOnCancel) {
                    self.parentNode().childNodes.remove(self);
                }
            };
            self.saveCategoryTreeNodeWithValidation = function () {
                var inputFields = $('input', '#' + self.containerId);
                if (inputFields.valid()) {
                    inputFields.blur();
                    self.dropOnCancel = false;
                    self.isActive(false);
                }
            };
            self.deleteCategoryTreeNode = function () {
                // Show confirmation dialog.
                var message = $.format(globalization.deleteCategoryNodeConfirmationMessage, self.title()),
                    confirmDialog = modal.confirm({
                        content: message,
                        onAccept: function () {
                            RemoveCategoryItemAndItsChildren(self);
                            confirmDialog.close();
                            return false;
                        }
                    });
            };
            self.callbackAfterSuccessSaving = null;
            self.callbackAfterFailSaving = null;
// TODO:
//            self.activateTranslation = function (languageId) {
//                languageId = languageId == null ? "" : languageId;
//                if (!self.translationsEnabled) {
//                    return;
//                }
//                if (self.isActive()) {
//                    self.cancelEditCategoryTreeNode();
//                }
//                if (self.activeTranslation() != null) {
//                    var active = self.activeTranslation(),
//                        isModified = active.isModified();
//
//                    if (active.title() != self.title()) {
//                        active.title(self.title());
//                        active.usePageTitleAsNodeTitle(false);
//                        isModified = true;
//                    }
//                    if (active.url() != self.url()) {
//                        active.url(self.url());
//                        isModified = true;
//                    }
//                    if (active.macro() != self.macro()) {
//                        active.macro(self.macro());
//                        isModified = true;
//                    }
//
//                    active.isModified(isModified);
//                    if (active.languageId() == languageId) {
//                        return;
//                    }
//                }
//
//                if (self.translations[languageId] == null) {
//                    self.translations[languageId] = new TranslationViewModel(self, languageId);
//                    if (languageId == "") {
//                        self.translations[languageId].macro(self.macro());
//                        self.translations[languageId].version(self.version());
//                        self.translations[languageId].isModified(true);
//                    } else {
//                        self.translations[languageId].title(self.pageId() != null && !bcms.isEmptyGuid(self.pageId())
//                            ? self.translations[""].originalTitle() || self.translations[""].title()
//                            : self.translations[languageId].title());
//                    }
//                }
//
//                self.activeTranslation(self.translations[languageId]);
//
//                self.title(self.translations[languageId].title());
//                self.url(self.translations[languageId].url());
//                self.version(self.translations[languageId].version());
//                self.macro(self.translations[languageId].macro());
//            };
//            self.updateLanguageOnDropNewNode = function (pageLanguageId, currentLanguageId) {
//                sitemap.showLoading(true);
//                var isActive = self.isActive();
//                self.translationsEnabled = true;
//                if (self.pageId() == null || self.pageId() == defaultIdValue) {
//                    self.activateTranslation("");
//                    self.activateTranslation(currentLanguageId);
//                    self.isActive(isActive);
//                    sitemap.showLoading(false);
//                    return;
//                }
//                var onSaveCompleted = function (json) {
//                    sitemap.showMessage(json);
//                    if (json.Success) {
//                        if (json.Data != null && json.Data != null) {
//                            for (var i = 0; i < json.Data.length; i++) {
//                                var translationJson = json.Data[i],
//                                    languageId = translationJson.LanguageId == null ? "" : translationJson.LanguageId,
//                                    translation = new TranslationViewModel(self, languageId);
//                                translation.pageId(translationJson.Id);
//                                translation.title(translationJson.Title);
//                                translation.originalTitle(translationJson.Title);
//                                translation.url(translationJson.PageUrl);
//                                translation.usePageTitleAsNodeTitle(self.usePageTitleAsNodeTitle());
//                                translation.isModified(true);
//                                self.translations[languageId] = translation;
//                                if (languageId === "") {
//                                    self.title(translationJson.Title);
//                                    self.url(translationJson.PageUrl);
//                                }
//                            }
//                            if (self.translations[""] == null) {
//                                self.activateTranslation("");
//                            }
//                            self.activateTranslation(currentLanguageId);
//                            self.isActive(isActive);
//                        }
//                    } else {
//                        self.isActive(false);
//                        self.isDeleted(true);
//                    }
//                    sitemap.showLoading(false);
//                };
//                $.ajax({
//                    url: $.format(links.getPageTranslations, self.pageId()),
//                    type: 'GET',
//                    dataType: 'json',
//                    cache: false,
//                })
//                    .done(function (json) {
//                        onSaveCompleted(json);
//                    })
//                    .fail(function (response) {
//                        onSaveCompleted(bcms.parseFailedResponse(response));
//                    });
//
//            };

            self.getCategoryTree = function () {
                if (self.parentNode() != null) {
                    return self.parentNode().getCategoryTree();
                }
                return null;
            };

            self.fromJson = function (jsonNode, translationsEnabled) {
                self.id(jsonNode.Id);
                self.version(jsonNode.Version);
                self.title(jsonNode.Title);
                self.macro(jsonNode.Macro);
                self.displayOrder(jsonNode.DisplayOrder);
// TODO:
//                self.url(jsonNode.Url);
//                self.pageId(jsonNode.PageId);
//                self.defaultPageId(jsonNode.DefaultPageId);
//                self.pageTitle(jsonNode.PageTitle);
//                self.usePageTitleAsNodeTitle(jsonNode.UsePageTitleAsNodeTitle);

//                if (translationsEnabled) {
//                    self.translationsEnabled = true;
//                    if (jsonNode.Translations != null) {
//                        for (var j = 0; j < jsonNode.Translations.length; j++) {
//                            var translationJson = jsonNode.Translations[j],
//                                translation = new TranslationViewModel(self, translationJson.LanguageId);
//                            translation.id(translationJson.Id);
//                            translation.pageId(translationJson.PageId);
//                            translation.title(translationJson.Title);
//                            translation.usePageTitleAsNodeTitle(translationJson.UsePageTitleAsNodeTitle);
//                            translation.url(translationJson.Url);
//                            translation.version(translationJson.Version);
//                            translation.macro(translationJson.Macro);
//                            self.translations[translationJson.LanguageId] = translation;
//                        }
//                    }
//                    self.activateTranslation("");
//                }

                var nodes = [];
                if (jsonNode.ChildNodes != null) {
                    for (var i = 0; i < jsonNode.ChildNodes.length; i++) {
                        var node = new NodeViewModel();
                        node.fromJson(jsonNode.ChildNodes[i], translationsEnabled);
                        node.parentNode(self);
                        nodes.push(node);
                    }
                }
                self.childNodes(nodes);
            };
// TODO:
//            self.translationsToJson = function () {
//                if (!self.translationsEnabled) {
//                    return null;
//                }
//                var result = [];
//                for (var i in self.translations) {
//                    var translation = self.translations[i];
//                    if (translation.isModified() && translation.languageId() != "") {
//                        result.push({
//                            Id: translation.id(),
//                            LanguageId: translation.languageId(),
//                            Title: translation.title(),
//                            Macro: translation.macro(),
//                            UsePageTitleAsNodeTitle: translation.usePageTitleAsNodeTitle(),
//                            Url: translation.url(),
//                            Version: translation.version(),
//                        });
//                    }
//                }
//                return result;
//            };
            self.toJson = function () {
// TODO:                var currentTranslation = null;
//                if (self.translationsEnabled) {
//                    currentTranslation = self.activeTranslation();
//                    self.activateTranslation("");
//                }
                var params = {
                    Id: self.id(),
                    Version: /* TODO: self.translationsEnabled ? self.translations[""].version() :*/ self.version(),
                    Title: /* TODO: self.translationsEnabled ? self.translations[""].title() :*/ self.title(),
                    Macro: /* TODO: self.translationsEnabled ? self.translations[""].macro() :*/ self.macro(),
// TODO:              UsePageTitleAsNodeTitle: self.usePageTitleAsNodeTitle(),
//                    Url: self.translationsEnabled ? self.translations[""].url() : self.url(),
//                    PageId: self.pageId(),
                    DisplayOrder: self.displayOrder(),
                    IsDeleted: self.isDeleted(),
// TODO:                    Translations: self.translationsToJson(),
                    ChildNodes: null
                };
// TODO:          if (self.translationsEnabled && currentTranslation != null) {
//                    self.activateTranslation(currentTranslation.languageId());
//                }
                return params;
            };
        }

        function CategorizableItemViewModel(jsonItem) {
            var self = this;
            self.id = ko.observable(jsonItem.Id);
            self.name = ko.observable(jsonItem.Name);
            self.isSelected = ko.observable(jsonItem.IsSelected);
            self.isDisabled = ko.observable(jsonItem.IsDisabled);

            self.inverseSelection = function () {
                if (!self.isDisabled()) {
                    self.isSelected(!self.isSelected());
                }
            }
        }

        function CategoryTreeViewModel(jsonCategoryTree) {
            var self = this;

            self.id = function () { return jsonCategoryTree.Id; };
            self.childNodes = ko.observableArray([]);
            self.childNodes.subscribe(updateFirstLastNode);
            self.someNodeIsOver = ko.observable(false);     // Someone is dragging some node over the tree, but not over the particular node.
            self.activeZone = ko.observable(DropZoneTypes.None);
            self.showHasNoDataMessage = ko.observable(false);
            self.savingInProgress = false;                  // To prevent multiple saving.

            self.version = jsonCategoryTree.Version;
            self.title = ko.observable(jsonCategoryTree.Title);
            self.macro = ko.observable(jsonCategoryTree.Macro);
            
            var items = [];
            if (jsonCategoryTree.CategorizableItems != null) {
                for (var k = 0; k < jsonCategoryTree.CategorizableItems.length; k++) {
                    items.push(new CategorizableItemViewModel(jsonCategoryTree.CategorizableItems[k]));
                }
            }
            self.categorizableItems = ko.observableArray(items);

            // TODO: self.accessControl = security.createUserAccessViewModel(jsonCategoryTree.UserAccessList);

// TODO:      self.languageId = ko.observable("");
//            self.onLanguageChanged = function (languageId) {
//                if (languageId != self.languageId()) {
//                    self.languageId(languageId);
//                    self.changeLanguageForNodes(self.childNodes());
//                    bcms.logger.debug('In sitemap language switched to "' + languageId + '".');
//                }
//            };
//            self.changeLanguageForNodes = function (nodes) {
//                for (var i = 0; i < nodes.length; i++) {
//                    var node = nodes[i];
//                    node.activateTranslation(self.languageId());
//                    self.changeLanguageForNodes(node.childNodes());
//                }
//            };
            self.showMacros = ko.observable(jsonCategoryTree.ShowMacros);
            // TODO: self.showLanguages = ko.observable(jsonCategoryTree.ShowLanguages);
            // TODO: self.language = jsonCategoryTree.ShowLanguages ? new LanguageViewModel(jsonCategoryTree.Languages, null, self.onLanguageChanged) : null,

            self.settings = {
                canEditNode: false,
                canDeleteNode: false,
                canDragNode: false,
                canDropNode: false,
                nodeSaveButtonTitle: globalization.nodeOkButton,
                nodeSaveAfterUpdate: false
            };

            self.getCategoryTree = function () {
                return self;
            };
            self.getNoDataMessage = function () {
                if (self.settings.canDeleteNode || self.settings.canDropNode) {
                    return globalization.placeNodeHere;
                }
                return globalization.categoryTreeIsEmpty;
            };

            self.addNewNode = function () {
                var node = new NodeViewModel();
                node.parentNode(self);
                node.title("");
                node.superDraggable(false);
                node.startEditCategoryTreeNode();
                node.callbackAfterSuccessSaving = function () {
                    self.updateNodesOrderAndParent();
                };
                node.callbackAfterFailSaving = function (newNode) {
                    newNode.parentNode().childNodes.remove(newNode);
                };
                node.isBeingDragged(false);
                node.displayOrder(0);
                node.dropOnCancel = true;
                self.childNodes.splice(0, 0, node);
                self.updateNodesOrderAndParent();
            };

            // Expanding or collapsing nodes.
            self.expandAll = function () {
                self.expandOrCollapse(self.childNodes(), true);
            };
            self.collapseAll = function () {
                self.expandOrCollapse(self.childNodes(), false);
            };
            self.expandOrCollapse = function (nodes, expand) {
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i];
                    node.isExpanded(expand);
                    self.expandOrCollapse(node.childNodes(), expand);
                }
            };

            self.hasChildNodes = function () {
                // Has at least one not deleted child node.
                var nodes = self.childNodes();
                for (var i = 0; i < nodes.length; i++) {
                    if (!nodes[i].isDeleted()) {
                        return true;
                    }
                }
                return false;
            };

            // Updating display order and parent node info.
            self.updateNodesOrderAndParent = function () {
                self.updateNodes(self.childNodes(), self);
            };
            self.updateNodes = function (nodes, parent) {
                if (nodes.length === 0) {
                    return;
                }

                for (var h = 0; h < nodes.length; h++) {
                    self.updateNodes(nodes[h].childNodes(), nodes[h]);
                }

                var isFullReorderNeeded = false,
                    zeroNoCount = 0,
                    zeroNoPlace = -1,
                    maxNo = 5000 + nodes.length * 50;

                // Analyze if full reordering is needed.
                for (var j = 0; j < nodes.length; j++) {
                    if (j == 0 && nodes[j].displayOrder() > 0 && nodes[j].displayOrder() < 100) {
                        isFullReorderNeeded = true;
                        break;
                    }
                    if (j == 1 && nodes[j - 1].displayOrder() == 0 && nodes[j].displayOrder() < 10) {
                        isFullReorderNeeded = true;
                        break;
                    }
                    if (nodes[j].displayOrder() > maxNo) {
                        isFullReorderNeeded = true;
                        break;
                    }
                    if (nodes[j].displayOrder() == 0) {
                        zeroNoCount++;
                        zeroNoPlace = j;
                        if (zeroNoCount > 1) {
                            isFullReorderNeeded = true;
                            break;
                        }
                    }
                }

                if (!isFullReorderNeeded) {
                    if (zeroNoCount == 0) {
                        return;
                    }

                    isFullReorderNeeded = true;
                    var nodeToUpdate = nodes[zeroNoPlace],
                            prevNode = nodes[zeroNoPlace - 1],
                            nextNode = nodes[zeroNoPlace + 1];

                    if (nodeToUpdate != null) {
                        // Is first.
                        if (prevNode == null && nextNode != null) {
                            if (nextNode.displayOrder() - 50 > 0) {
                                self.updateNode(nodeToUpdate, nextNode.displayOrder() - 50, parent);
                                return;
                            }
                        }
                        // Is last.
                        if (prevNode != null && nextNode == null) {
                            if (prevNode.displayOrder() + 50 < 2147483647) {
                                self.updateNode(nodeToUpdate, prevNode.displayOrder() + 50, parent);
                                return;
                            }
                        }
                        // Is in between.
                        if (prevNode != null && nextNode != null) {
                            var middleNo = Math.ceil((nextNode.displayOrder() - prevNode.displayOrder()) / 2) + prevNode.displayOrder();
                            if (prevNode.displayOrder() < middleNo && middleNo < nextNode.displayOrder()) {
                                self.updateNode(nodeToUpdate, middleNo, parent);
                                return;
                            }
                        }
                    }
                }

                if (isFullReorderNeeded) {
                    for (var k = 0; k < nodes.length; k++) {
                        var node = nodes[k],
                            no = k == 0 ? 5000 : nodes[k - 1].displayOrder() + 50;
                        self.updateNode(node, no, parent);
                    }
                }
            };
            self.updateNode = function (node, orderNo, parent) {
                if (node.displayOrder() != orderNo) {
                    node.displayOrder(orderNo);
                }

                if (node.parentNode() != parent) {
                    node.parentNode(parent);
                }
            };

            self.focusOnActiveNode = function (nodes) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].isActive()) {
                        $($("input", "#" + nodes[i].containerId).get(0)).focus();
                        return true;
                    }
                    if (self.focusOnActiveNode(nodes[i].childNodes())) {
                        nodes[i].isExpanded(true);
                        return true;
                    }
                }
                return false;
            };
            self.save = function (onDoneCallback) {
                if (self.focusOnActiveNode(self.childNodes())) {
                    var messagesBox = messages.box({ container: module.activeMessageContainer });
                    messagesBox.clearMessages();
                    messagesBox.addWarningMessage(globalization.someCategoryNodesAreInEditingState);
                    return;
                }

                var dataToSend = JSON.stringify(self.getModelToSave()),
                    onSaveCompleted = function (json) {
                        messages.refreshBox(module.activeMessageContainer, json);
                        showLoading(false);
                        if (json.Success) {
                            if (onDoneCallback && $.isFunction(onDoneCallback)) {
                                if (json.Data == null) {
                                    json.Data = {
                                        Title: self.title(),
                                        Macro: self.macro()
                                    };
                                }
                                onDoneCallback(json);
                            }
                        }
                        self.savingInProgress = false;
                    };

                if (!self.savingInProgress) {
                    self.savingInProgress = true;
                    $.ajax({
                        url: links.saveCategoryTreeUrl,
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        cache: false,
                        data: dataToSend,
                        beforeSend: function () { showLoading(true); }
                    })
                        .done(function (json) {
                            onSaveCompleted(json);
                        })
                        .fail(function (response) {
                            onSaveCompleted(bcms.parseFailedResponse(response));
                        });
                }
            };

            // Parsing / composing.
            self.parseJsonNodes = function (jsonNodes) {
                var nodes = [];
                jsonNodes = jsonNodes || [];
                for (var i = 0; i < jsonNodes.length; i++) {
                    var node = new NodeViewModel();
                    node.fromJson(jsonNodes[i], false); // TODO: self.showLanguages);
                    node.parentNode(self);
                    nodes.push(node);
                }
                self.childNodes(nodes);
            };
            self.composeJsonNodes = function () {
                return self.nodesToJson(self.childNodes());
            };
            self.nodesToJson = function (nodes) {
                var result = [];
                for (var i = 0; i < nodes.length; i++) {
                    var nodeToSave = nodes[i].toJson();
                    if (!nodeToSave.IsDeleted) {
                        nodeToSave.ChildNodes = self.nodesToJson(nodes[i].childNodes());
                        result.push(nodeToSave);
                    }
                }
                return result;
            };
            self.getModelToSave = function () {
// TODO:          var userAccessList = [],
//                    accessModels = self.accessControl.UserAccessList(),
//                    i;

//                for (i = 0; i < accessModels.length; i++) {
//                    userAccessList.push({
//                        Identity: accessModels[i].Identity(),
//                        AccessLevel: accessModels[i].AccessLevel(),
//                        IsForRole: accessModels[i].IsForRole(),
//                    });
//                }

                var categorizableItems = [],
                    categorizableItemModels = self.categorizableItems(),
                    i;

                for (i = 0; i < categorizableItemModels.length; i++) {
                    categorizableItems.push({
                        Id: categorizableItemModels[i].id(),
                        Name: categorizableItemModels[i].name(),
                        IsSelected: categorizableItemModels[i].isSelected(),
                        IsDisabled: categorizableItemModels[i].isDisabled()
                    });
                }

                return {
                    Id: self.id(),
                    Version: self.version,
                    Title: self.title(),
                    Macro: self.macro(),
                    RootNodes: self.composeJsonNodes(),
                    CategorizableItems: categorizableItems,
                    // TODO: UserAccessList: userAccessList
                };
            };
        }

        function RemoveCategoryItemAndItsChildren(catItem) {
            var children = catItem.childNodes();
            for (var i = 0; i < children.length; i++) {
                RemoveCategoryItemAndItsChildren(children[i]);
            }
            catItem.isActive(false);
            catItem.isDeleted(true);
        }

        function AddNodeMapController() {
            var self = this;
            self.container = null;

            self.initialize = function (content, dialog) {
                self.container = dialog.container;
                module.activeMessageContainer = self.container;
                module.activeLoadingContainer = self.container.find(selectors.categoryTreeAddNodeDataBind);

                if (content.Success) {
                    // Create data models.
                    var treeViewModel = new CategoryTreeViewModel(content.Data),
                        isReadOnly = content.Data.IsReadOnly;

                    if (!isReadOnly) {
                        showMessage(content);
                    } else {
                        // Allow user navigate in tree.
                        dialog.container.find(modal.selectors.readonly).removeClass(modal.classes.inactive);
                        dialog.container.find(selectors.firstTab).addClass(modal.classes.inactive);
                    }

                    treeViewModel.parseJsonNodes(content.Data.RootNodes);
                    module.activeMapModel = treeViewModel;

                    // Setup settings.
                    treeViewModel.settings.canEditNode = !isReadOnly;
                    treeViewModel.settings.canDeleteNode = !isReadOnly;
                    treeViewModel.settings.canDragNode = !isReadOnly;
                    treeViewModel.settings.canDropNode = !isReadOnly;
                    treeViewModel.settings.nodeSaveButtonTitle = globalization.nodeOkButton;

                    // Bind models.
                    var context = self.container.find(selectors.categoryTreeAddNodeDataBind).get(0);
                    if (context) {
                        ko.applyBindings(treeViewModel, context);
                        updateValidation();
                    }
                } else {
                    showMessage(content);
                }
            };
            self.save = function (onDoneCallback) {
                if (module.activeMapModel) {
                    module.activeMapModel.save(onDoneCallback);
                }
            };
        }

        function loadAddNodeDialog(id, onClose, dialogTitle) {
            var addNodeController = new AddNodeMapController();
            modal.open({
                title: dialogTitle || globalization.categoryTreeEditorDialogTitle,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, $.format(links.categoryTreeEditDialogUrl, id), {
                        done: function (content) {
                            addNodeController.initialize(content, dialog);
                        }
                    });
                },
                onAccept: function (dialog) {
                    var canContinue = forms.valid(dialog.container.find(selectors.categoryTreeForm));

                    if (canContinue) {
                        addNodeController.save(function (json) {
                            if (json.Success) {
                                dialog.close();
                                if (onClose && $.isFunction(onClose)) {
                                    onClose(json);
                                }
                                messages.refreshBox(selectors.siteSettingsCategoryTreesForm, json);
                            } else {
                                showMessage(json);
                            }
                        });
                    }
                    return false;
                }
            });
        };

        function openCreateCategoryTreeDialog(callBackOnSuccess) {
            loadAddNodeDialog(defaultIdValue, callBackOnSuccess, globalization.categoryTreeCreatorDialogTitle);
        };


        // --- Site Settings Dialog -------------------------------------------
        
        function searchCategoryTrees(form) {
            grid.submitGridForm(form, function (htmlContent) {
                siteSettings.setContent(htmlContent);
                initializeListOfCategoryTrees();
            });
        };

        function addCategoryTree(container) {
            openCreateCategoryTreeDialog(function (data) {
                if (data.Data != null) {
                    var template = $(selectors.siteSettingsGridRowTemplate),
                        newRow = $(template.html()).find(selectors.siteSettingsGridRowTemplateFirstRow);

                    newRow.find(selectors.siteSettingsGridRowTitleCell).html(antiXss.encodeHtml(data.Data.Title));
                    newRow.find(selectors.siteSettingsGridItemEditButton).data("id", data.Data.Id);
                    newRow.find(selectors.siteSettingsGridItemDeleteButton).data("id", data.Data.Id);
                    newRow.find(selectors.siteSettingsGridItemDeleteButton).data("version", data.Data.Version);

                    newRow.insertBefore($(selectors.siteSettingsGridTableFirstRow, container));

                    initializeListItems(newRow, container);

                    grid.showHideEmptyRow(container);
                }
            }, true);
        };

        function editCategoryTree(self, container) {
            var id = self.data("id");

            loadAddNodeDialog(id, function (data) {
                if (data.Data != null) {
                    var row = self.parents(selectors.siteSettingsGridRowTemplateFirstRow),
                        cell = row.find(selectors.siteSettingsGridRowTitleCell);
                    cell.html(antiXss.encodeHtml(data.Data.Title));
                    row.find(selectors.siteSettingsGridItemDeleteButton).data("version", data.Data.Version);
                }
            }, globalization.categoryTreeEditorDialogTitle);
        };

        function deleteCategoryTree(self, container) {
            var id = self.data("id"),
                version = self.data("version");

            module.deleteCategoryTree(id, version, function (json) {
                messages.refreshBox(selectors.siteSettingsCategoryTreesForm, json);
                if (json.Success) {
                    self.parents(selectors.siteSettingsGridRowTemplateFirstRow).remove();
                    grid.showHideEmptyRow(container);
                }
            });
        };

        function initializeListItems(container, masterContainer) {
            container.find(selectors.siteSettingsGridItemCreateButton).on("click", function (event) {
                bcms.stopEventPropagation(event);
                addCategoryTree(masterContainer || container);
            });

            container.find(selectors.siteSettingsGridRowCells).on("click", function (event) {
                bcms.stopEventPropagation(event);
                var editButton = $(this).parents(selectors.siteSettingsGridRow).find(selectors.siteSettingsGridItemEditButton);
                if (editButton.length > 0) {
                    editCategoryTree(editButton, masterContainer || container);
                }
            });

            container.find(selectors.siteSettingsGridItemDeleteButton).on("click", function (event) {
                bcms.stopEventPropagation(event);
                deleteCategoryTree($(this), masterContainer || container);
            });
        };

        function initializeListOfCategoryTrees() {
            var dialog = siteSettings.getModalDialog(),
                container = dialog.container,
                form = container.find(selectors.siteSettingsCategoryTreesForm);

            grid.bindGridForm(form, function (data) {
                siteSettings.setContent(data);
                initializeListOfCategoryTrees();
            });

            form.on("submit", function (event) {
                bcms.stopEventPropagation(event);
                searchCategoryTrees(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.searchField), {
                preventedEnter: function () {
                    searchCategoryTrees(form);
                }
            });

            form.find(selectors.searchButton).on("click", function (event) {
                bcms.stopEventPropagation(event);
                searchCategoryTrees(form);
            });

            initializeListItems(container);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(container.find(selectors.searchField), true);
        };

        // --------------------------------------------------------------------


        /**
*  autocomplete list view model
*/
        var CategoriesListViewModel = (function (_super) {
            bcms.extendsClass(CategoriesListViewModel, _super);

            function CategoriesListViewModel(categoriesList, categoryTreeForKey) {
                var options = {
                    serviceUrl: links.categoriesSuggestionServiceUrl,
                    pattern: 'Categories[{0}].key',
                    params : {
                        categoryTreeForKey: categoryTreeForKey
                    }                    
                };

                _super.call(this, categoriesList, options);
            }

            return CategoriesListViewModel;
        })(autocomplete.AutocompleteListViewModel);

        module.CategoriesListViewModel = CategoriesListViewModel;

        /**
        * Loads a categories view to the site settings container.
        */
        module.loadSiteSettingsCategoryTreesList = function() {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsCategoryTreesListUrl, {
                contentAvailable: initializeListOfCategoryTrees
            });
        };

        module.deleteCategoryTree = function(id, version, callBack) {
            var url = $.format(links.deleteCategoryTreeUrl, id, version),
                onDeleteCompleted = function (json) {
                    if ($.isFunction(callBack)) {
                        callBack(json);
                    }
                };
            modal.confirm({
                content: globalization.categoryTreeDeleteConfirmMessage,
                onAccept: function () {
                    $.ajax({
                        type: "POST",
                        url: url,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        cache: false
                    })
                    .done(function (json) {
                        onDeleteCompleted(json);
                    })
                    .fail(function (response) {
                        onDeleteCompleted(bcms.parseFailedResponse(response));
                    });
                }
            });
        }

        /**
        * Initializes module.
        */
        module.init = function() {
            // Bindings for nodes Drag'n'Drop.
            /// <summary>
            /// s this instance.
            /// </summary>
            /// <returns></returns>
            addDraggableBinding();
            addDroppableBinding();

            bcms.logger.debug("Initializing bcms.categories module.");
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(module.init);

        return module;
    });