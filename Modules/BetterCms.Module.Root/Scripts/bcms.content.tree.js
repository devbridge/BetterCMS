/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */
bettercms.define('bcms.content.tree', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.modal', 'bcms.content', 'bcms.redirect', 'bcms.antiXss'],
function ($, bcms, ko, modal, contentModule, redirect, antiXss) {
    'use strict';

    var tree = {},
        selectors = {
            treeTemplate: '#bcms-contents-tree-template',
            contentTreeContainer: '#bcms-contents-tree',
            sortableContentConnectors: '.bcms-contents-tree-sort-block',
            firstParentRegion: '.bcms-contents-tree-region:first',
            childContents: '.bcms-contents-tree-content'
        },
        links = {},
        globalization = {
            contentsTreeTitle: null,
            closeTreeButtonTitle: null,
            saveSortChanges: null,
            resetSortChanges: null,
            saveSortChangesConfirmation: null
        },
        classes = {
            sortableContentPlaceholder: "bcms-contents-tree-drop-area"
        },
        treeItemTypes = {
            content: 1,
            region: 2
        },
        contentStatuses = {
            draft: 2
        },
        treeViewModel = null,
        isOpenedAddContent = false,
        suspendCloseAddContent = false;

    // Assign objects to module
    tree.selectors = selectors;
    tree.links = links;
    tree.globalization = globalization;

    function setContentModelValues(contentViewModel, json) {
        if (json.Data.ContentId || json.Data.Id) {
            contentViewModel.contentId = json.Data.ContentId || json.Data.Id;
        }
        if (json.Data.DesirableStatus || typeof json.Data.HasDraft == "boolean") {
            contentViewModel.draft = json.Data.DesirableStatus == contentStatuses.draft || json.Data.HasDraft;
        }
        if (json.Data.Title || json.Data.WidgetName) {
            contentViewModel.title = json.Data.Title || json.Data.WidgetName;
        }
        if (json.Data.ContentVersion || json.Data.Version || json.Data.OriginalVersion) {
            contentViewModel.contentVersion = json.Data.ContentVersion || json.Data.Version || json.Data.OriginalVersion;
        }
        if (json.Data.ContentType) {
            contentViewModel.contentType = json.Data.ContentType;
        }
        if (json.Data.PageContentId && !bcms.isEmptyGuid(json.Data.PageContentId)) {
            contentViewModel.pageContentId = json.Data.PageContentId;
        }
        if (json.Data.PageContentVersion) {
            contentViewModel.pageContentVersion = json.Data.PageContentVersion;
        }
    }

    function setContentModelValuesAndRegions(model, json) {
        var regionModels,
            regionTreeItemViewModel;

        setContentModelValues(model.model, json);
        model.title(model.model.title);
        model.draft(model.model.draft);

        // Check if new regions where added
        regionModels = createRegionViewModels(json.Data.Regions, model.parentRegion.model.id, model.model.pageContentId);
        bcms
            .asEnumerable(regionModels)
            .where(function (x) {
                return bcms.asEnumerable(model.items()).where(function (y) {
                    return x.id === y.model.id;
                }).toArray().length == 0;
            })
            .forEach(function (x) {
                regionTreeItemViewModel = createRegionTreeItemViewModel(x, model, model.level() + 1);
                model.items.push(regionTreeItemViewModel);
            });

        // Check if regions where removed
        bcms
            .asEnumerable(model.items())
            .where(function (y) {
                return bcms.asEnumerable(regionModels).where(function (x) {
                    return x.id === y.model.id;
                }).toArray().length == 0;
            })
            .forEach(function (x) {
                model.items.remove(x);
            });
    }

    function createRegionViewModels(json, parentRegionId, parentPageContentId) {
        var regions = [],
            i,
            regionViewModel;

        if (json && json.length > 0) {
            for (i = 0; i < json.length; i++) {
                regionViewModel = new contentModule.RegionViewModel(null, null, [], parentRegionId, parentPageContentId);
                regionViewModel.isInvisible = true;
                regionViewModel.id = json[i].RegionId;
                regionViewModel.title = json[i].RegionIdentifier;
                regionViewModel.initializeRegion();

                regions.push(regionViewModel);
            }
        }

        return regions;
    }

    function closeAllAddContentButtons() {
        var items = treeViewModel.items(),
                j = 0,
                item;

        for (; j < items.length; j++) {
            item = items[j];
            if (item.isOpened && $.isFunction(item.isOpened)) {
                item.isOpened(false);
            }
        }

    }

    function createRegionTreeItemViewModel(regionModel, parentContent, level) {
        var model = new TreeItemViewModel(),
            itemModel,
            i,
            onAfterAddContent = function (json) {
                treeViewModel.reloadPage = true;

                var regionModels,
                    contentViewModel = new contentModule.ContentViewModel(null, null, regionModel.parentPageContentId),
                    contentTreeItemViewModel,
                    regionTreeItemViewModel;

                contentViewModel.isInvisible = true;
                setContentModelValues(contentViewModel, json);

                contentViewModel.initializeContent();
                contentTreeItemViewModel = createContentTreeItemViewModel(contentViewModel, model, model.level() + 1);

                // Set content regions
                regionModels = createRegionViewModels(json.Data.Regions, regionModel.id, contentViewModel.pageContentId);
                for (i = 0; i < regionModels.length; i++) {
                    regionTreeItemViewModel = createRegionTreeItemViewModel(regionModels[i], contentViewModel, model.level() + 2);

                    contentTreeItemViewModel.items.push(regionTreeItemViewModel);
                }

                model.items.push(contentTreeItemViewModel);
            };

        model.itemId = bcms.createGuid();
        model.title(regionModel.title);
        model.model = regionModel;
        model.type = treeItemTypes.region;
        model.isInvisible = regionModel.isInvisible;
        model.parentContent = parentContent;
        model.level(level);

        // Collect child contents
        level++;
        for (i = 0; i < regionModel.contents.length; i++) {
            itemModel = createContentTreeItemViewModel(regionModel.contents[i], model, level);

            model.items.push(itemModel);
        }

        model.isOpened = ko.observable(false);

        model.toggleAddContent = function () {
            var isOpened = this.isOpened();
            closeAllAddContentButtons();

            if (!isOpened) {
                this.isOpened(true);

                isOpenedAddContent = true;
                suspendCloseAddContent = true;
                setTimeout(function() {
                    suspendCloseAddContent = false;
                }, 100);
            }
        };

        model.addContent = function () {
            regionModel.onAddHtml(onAfterAddContent, true);
        };

        model.addMarkdown = function() {
            regionModel.onAddMarkdown(onAfterAddContent, true);
        }

        model.addSimpleText = function () {
            regionModel.onAddSimpleText(onAfterAddContent);
        }

        model.insertWidget = function () {
            regionModel.onInsertWidget(onAfterAddContent);
        }

        model.removeContent = function (contentViewModel) {
            model.items.remove(contentViewModel);
        };

        return model;
    }

    function createContentTreeItemViewModel(contentModel, parentRegion, level) {
        var model = new TreeItemViewModel(),
            childRegions,
            itemModel,
            i;

        model.itemId = bcms.createGuid();
        model.title(contentModel.title);
        model.model = contentModel;
        model.type = treeItemTypes.content;
        model.isInvisible = contentModel.isInvisible;
        model.parentRegion = parentRegion;
        model.level(level);
        model.draft(contentModel.draft);

        childRegions = contentModel.getChildRegions();
        level++;
        for (i = 0; i < childRegions.length; i++) {
            itemModel = createRegionTreeItemViewModel(childRegions[i], model, level);

            model.items.push(itemModel);
        }

        model.editItem = function () {
            contentModel.onEditContent(function (json) {
                treeViewModel.reloadPage = true;

                setContentModelValuesAndRegions(model, json);
            }, true);
        };

        model.deleteItem = function () {
            contentModel.onDeleteContent(function () {
                treeViewModel.reloadPage = true;

                model.parentRegion.removeContent(model);
            });
        };

        model.history = function () {
            contentModel.onContentHistory(function (json) {
                treeViewModel.reloadPage = true;

                setContentModelValuesAndRegions(model, json);
            });
        };

        model.configure = function () {
            contentModel.onConfigureContent(function (json) {
                treeViewModel.reloadPage = true;

                setContentModelValues(model.model, json);
            });
        };

        return model;
    }

    /*
    * Regions/contents tree list view model
    */
    function TreeViewModel(pageModel) {
        var self = this,
            i,
            itemModel;

        self.pageModel = pageModel;
        self.items = ko.observableArray();
        self.visibleItems = ko.observableArray();
        self.invisibleItems = ko.observableArray();
        self.isBeingDragged = ko.observable(false);
        self.reloadPage = false;
        self.contentsSorted = false;

        self.currentLevel = ko.observable(0);
        // Collect child regions
        for (i = 0; i < pageModel.regions.length; i++) {
            if (pageModel.regions[i].parentRegion) {
                continue;
            }
            itemModel = createRegionTreeItemViewModel(pageModel.regions[i], null, 1);
            if (itemModel.isInvisible) {
                self.invisibleItems.push(itemModel);
            } else {
                self.visibleItems.push(itemModel);
            }
            self.items.push(itemModel);
        }

        self.getItemById = function (itemId, items) {
            var j,
                item;

            if (!items) {
                items = treeViewModel.items();
            }

            for (j = 0; j < items.length; j++) {
                if (items[j].itemId == itemId) {
                    return items[j];
                }

                item = self.getItemById(itemId, items[j].items());
                if (item) {
                    return item;
                }
            }

            return null;
        };

        return self;
    }

    /*
    * Tree item (region or content) view model
    */
    function TreeItemViewModel() {
        var self = this;

        self.items = ko.observableArray();
        self.model = null;
        self.title = ko.observable();
        self.type = null;
        self.itemId = null;
        self.isInvisible = false;
        self.types = treeItemTypes;
        self.parentRegion = null;
        self.parentContent = null;

        self.editItem = function () { };
        self.deleteItem = function () { };
        self.addContent = function () { };
        self.history = function () { };
        self.configure = function () { };

        self.isActive = ko.observable(false);
        self.level = ko.observable(0);
        self.draft = ko.observable(false);

        self.isBeingDragged = ko.observable(false);
        self.isBeingDragged.subscribe(function (newValue) {
            treeViewModel.isBeingDragged(newValue);
        });

        self.onMouseLeave = function () {
            this.isActive(false);

            treeViewModel.currentLevel(self.level() - 1);
        };

        self.onMouseEnter = function () {
            this.isActive(true);

            treeViewModel.currentLevel(this.level());
        };

        self.isHover = ko.computed(function () {
            return (!treeViewModel || !treeViewModel.isBeingDragged())
                && self.isActive()
                && self.level() == treeViewModel.currentLevel();
        });

        return self;
    }

    /*
    * Opens modal window with all regions / contents listed
    */
    function onEditContentsTree(data) {
        var pageModel = data.pageViewModel,
            changedRegions = contentModule.getChangedRegions(),
            onSave = function () {
                // Open pages structure modal after user accepts changes
                openContentsTree(pageModel, function () {
                    treeViewModel.reloadPage = true;
                });
            },
            onReset = function () {
                // Open pages structure modal after user resets changes
                openContentsTree(pageModel);
            };

        if (changedRegions.length > 0) {
            contentModule.showConfirmationAboutContentsSave(onSave, onReset);
        } else {
            // Open pages structure modal, when there are no changes
            openContentsTree(pageModel);
        }
    }

    function openContentsTree(pageModel, onModelCreated) {
        var manageDialog = modal.open({
            title: globalization.contentsTreeTitle,
            cancelTitle: globalization.closeTreeButtonTitle,
            disableAccept: true,
            onLoad: function (dialog) {
                var container = $($(selectors.treeTemplate).html());
                dialog.setContent(container);
                treeViewModel = new TreeViewModel(pageModel);
                
                if ($.isFunction(onModelCreated)) {
                    onModelCreated();
                }

                ko.applyBindings(treeViewModel, container.get(0));
            },

            onClose: function () {
                if (manageDialog.isClosing) {
                    return true;
                }

                if (treeViewModel.contentsSorted) {
                    var doNotsaveButton,
                        dialog;

                    doNotsaveButton = new modal.button(globalization.resetSortChanges, null, 5, function () {
                        dialog.close();
                        manageDialog.isClosing = true;
                        manageDialog.close();
                        if (treeViewModel.reloadPage) {
                            redirect.ReloadWithAlert();
                        }
                    });

                    dialog = modal.confirm({
                        content: globalization.saveSortChangesConfirmation,
                        acceptTitle: globalization.saveSortChanges,
                        buttons: [doNotsaveButton],
                        onAccept: function () {
                            dialog.close();
                            var changedRegions = checkIfRegionContentsChanged([], treeViewModel.items());
                            if (changedRegions.length > 0) {
                                contentModule.saveContentChanges(changedRegions, null);
                            } else {
                                if (treeViewModel.reloadPage) {
                                    redirect.ReloadWithAlert();
                                } else {
                                    manageDialog.isClosing = true;
                                    manageDialog.close();
                                }
                            }
                        }
                    });

                    return false;
                } else {
                    if (treeViewModel.reloadPage) {
                        redirect.ReloadWithAlert();
                    }

                    return true;
                }
            }
        });
    }

    /*
    * Checks if order of contents has changed and if there are changes, saves the changes. 
    */
    function checkIfRegionContentsChanged(changedRegions, treeItems) {
        var l = treeItems.length,
            i,
            j,
            treeItem,
            subTreeItem,
            contentsBeforeReorder,
            contentsAfterReorder,
            hasContentsChanged;

        for (i = 0; i < l; i++) {
            treeItem = treeItems[i];

            if (treeItem.type == treeItemTypes.region) {
                contentsBeforeReorder = treeItem.model.contents;
                contentsAfterReorder = [];

                for (j = 0; j < treeItem.items().length; j++) {
                    subTreeItem = treeItem.items()[j];

                    if (subTreeItem.type == treeItemTypes.content) {
                        contentsAfterReorder.push(subTreeItem.model);
                    }

                    changedRegions = checkIfRegionContentsChanged(changedRegions, subTreeItem.items());
                }

                hasContentsChanged = contentModule.hasContentsOrderChanged(contentsBeforeReorder, contentsAfterReorder);
                if (hasContentsChanged) {
                    treeItem.model.setContents(contentsAfterReorder);

                    changedRegions.push(treeItem.model);
                }
            }
        }

        return changedRegions;
    }

    /**
    * Helper function to add knockout binding 'draggable'.
    */
    function addDraggableBinding() {
        ko.bindingHandlers.draggableContent = {
            init: function (element, valueAccessor, allBindingsAccessor, koViewModel) {
                if (!valueAccessor()) {
                    return;
                }

                var viewModel;
                if ($.isArray(koViewModel)) {
                    viewModel = koViewModel[0];
                } else {
                    viewModel = koViewModel;
                }

                var isUpdating = false,
                    setup = {
                        connectWith: selectors.sortableContentConnectors,
                        dropOnEmpty: true,
                        placeholder: classes.sortableContentPlaceholder,
                        tolerance: "intersect",
                        start: function () {
                            isUpdating = true;
                            if (viewModel.isBeingDragged) {
                                viewModel.isBeingDragged(true);
                            }
                        },
                        stop: function () {
                            if (viewModel.isBeingDragged) {
                                viewModel.isBeingDragged(false);
                            }
                        },
                        update: function (e, data) {
                            if (isUpdating) {

                                isUpdating = false;

                                var regionContainer = data.item.parents(selectors.firstParentRegion),
                                    regionId = regionContainer.data('itemId'),
                                    regionModelBefore = viewModel.parentRegion,
                                    regionModelAfter = regionId == regionModelBefore.itemId ? regionModelBefore : treeViewModel.getItemById(regionId),
                                    correctOrder = [],
                                    updateOrder = false,
                                    allItems = regionModelAfter.items();

                                if (regionModelBefore != regionModelAfter) {
                                    viewModel.parentRegion = regionModelAfter;

                                    regionModelBefore.removeContent(viewModel);
                                }

                                i = 0;
                                regionContainer.find(selectors.childContents).each(function () {
                                    var id = $(this).data('itemId'),
                                        itemModel = (id == viewModel.itemId && regionModelBefore != regionModelAfter)
                                            ? viewModel
                                            : treeViewModel.getItemById(id, allItems);

                                    if (itemModel != null && itemModel.type == treeItemTypes.content && itemModel.parentRegion == regionModelAfter) {
                                        correctOrder.push(itemModel);
                                        if (!updateOrder && (!allItems[i] || itemModel.itemId != allItems[i].itemId)) {
                                            updateOrder = true;
                                        }
                                    }

                                    i++;
                                });

                                if (regionId != regionModelBefore.itemId || updateOrder) {
                                    treeViewModel.contentsSorted = true;
                                }

                                if (updateOrder) {
                                    regionModelAfter.items.removeAll();
                                    for (var i = 0; i < correctOrder.length; i++) {
                                        regionModelAfter.items.push(correctOrder[i]);
                                    }
                                }
                            }
                        }
                    };

                $(element).sortable(setup);
            }
        };
    }

    /**
    * Is called everytime user clicks a mouse anywhere in the browser
    */
    function onBodyClick() {
        if (isOpenedAddContent && !suspendCloseAddContent) {
            closeAllAddContentButtons();
            isOpenedAddContent = false;
        }
    }

    /**
    * Initializes contents tree module.
    */
    tree.init = function () {
        bcms.logger.debug('Initializing contents tree module');

        addDraggableBinding();
    };

    /**
    * Subscribe to events
    */
    bcms.on(bcms.events.editContentsTree, onEditContentsTree);
    bcms.on(bcms.events.bodyClick, onBodyClick);

    /**
    * Register initialization
    */
    bcms.registerInit(tree.init);

    return tree;
});