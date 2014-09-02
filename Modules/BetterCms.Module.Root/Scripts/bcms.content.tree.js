/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */
bettercms.define('bcms.content.tree', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.modal', 'bcms.content', 'bcms.redirect'],
    function ($, bcms, ko, modal, contentModule, redirect) {
    'use strict';

    var tree = {},
        selectors = {
            treeTemplate: '#bcms-contents-tree-template',
            contentTreeContainer: '#bcms-contents-tree'
        },
        links = {},
        globalization = {
            contentsTreeTitle: null,
            closeTreeButtonTitle: null
        },
        treeItemTypes = {
            content: 1,
            region: 2
        },
        contentStatuses = {
            draft: 2
        },
        dropZoneTypes = {
            none: 'none',
            emptyListZone: 'emptyListZone',
            topZone: 'top',
            middle: 'middle'
        },
        treeViewModel = null;

    // Assign objects to module
    tree.selectors = selectors;
    tree.links = links;
    tree.globalization = globalization;

    function setContentModelValues(contentViewModel, json) {
        if (json.Data.ContentId || json.Data.Id) {
            contentViewModel.contentId = json.Data.ContentId || json.Data.Id;
        }
        if (json.Data.DesirableStatus || json.Data.HasDraft) {
            contentViewModel.draft = json.Data.DesirableStatus == contentStatuses.draft || json.Data.HasDraft;
        }
        if (json.Data.Title || json.Data.WidgetName) {
            contentViewModel.title = json.Data.Title || json.Data.WidgetName;
        }
        if (json.Data.ContentVersion || json.Data.Version) {
            contentViewModel.contentVersion = json.Data.ContentVersion || json.Data.Version;
        }
        contentViewModel.contentType = json.Data.ContentType;
        if (json.Data.PageContentId) {
            contentViewModel.pageContentId = json.Data.PageContentId;
        }
        if (json.Data.PageContentVersion) {
            contentViewModel.pageContentVersion = json.Data.PageContentVersion;
        }
    }

    function createRegionViewModel(regionModel) {
        var model = new TreeItemViewModel(),
            childContents,
            itemModel,
            i;

        model.title(regionModel.title);
        model.model = regionModel;
        model.type = treeItemTypes.region;
        model.isInvisible = regionModel.isInvisible;

        // Collect child contents
        childContents = regionModel.getChildContents();
        for (i = 0; i < childContents.length; i++) {
            itemModel = createContentViewModel(childContents[i], model);

            model.items.push(itemModel);
        }

        model.addContent = function () {
            regionModel.onAddContent(function (json) {
                treeViewModel.reloadPage = true;

                var contentViewModel = new contentModule.ContentViewModel(null, null, regionModel.parentPageContentId);

                contentViewModel.isInvisible = true;
                setContentModelValues(contentViewModel, json);

                contentViewModel.initializeContent();
                model.items.push(createContentViewModel(contentViewModel, model));
            });
        };

        model.removeContent = function (contentViewModel) {
            model.items.remove(contentViewModel);
        };

        return model;
    }

    function createContentViewModel(contentModel, parentRegion) {
        var model = new TreeItemViewModel(),
            childRegions,
            itemModel,
            i;

        model.title(contentModel.title);
        model.model = contentModel;
        model.type = treeItemTypes.content;
        model.isInvisible = contentModel.isInvisible;
        model.parentRegion = parentRegion;

        childRegions = contentModel.getChildRegions();
        for (i = 0; i < childRegions.length; i++) {
            itemModel = createRegionViewModel(childRegions[i]);

            model.items.push(itemModel);
        }

        model.editItem = function () {
            contentModel.onEditContent(function(json) {
                treeViewModel.reloadPage = true;

                setContentModelValues(model.model, json);
                model.title(model.model.title);
            });
        };

        model.deleteItem = function () {
            contentModel.onDeleteContent(function () {
                treeViewModel.reloadPage = true;

                model.parentRegion.removeContent(model);
            });
        };

        model.history = function() {
            contentModel.onContentHistory();
        };

        model.configure = function() {
            contentModel.onConfigureContent(function() {
                treeViewModel.reloadPage = true;
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
        self.reloadPage = false;
        self.contentsSorted = false;

        // Collect child regions
        for (i = 0; i < pageModel.regions.length; i++) {
            if (pageModel.regions[i].parentRegion) {
                continue;
            }
            itemModel = createRegionViewModel(pageModel.regions[i]);
            if (itemModel.isInvisible) {
                self.invisibleItems.push(itemModel);
            } else {
                self.visibleItems.push(itemModel);
            }
            self.items.push(itemModel);
        }

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
        self.isInvisible = false;
        self.types = treeItemTypes;

        self.editItem = function () { };
        self.deleteItem = function () { };
        self.addContent = function () { };
        self.history = function () { };
        self.configure = function () { };

        self.isBeingDragged = ko.observable(false);
        self.activeZone = ko.observable(dropZoneTypes.none);

        return self;
    }

    /*
    * Opens modal window with all regions / contents listed
    */
    function onEditContentsTree(pageModel) {
        modal.open({
            title: globalization.contentsTreeTitle,
            cancelTitle: globalization.closeTreeButtonTitle,
            disableAccept: true,
            onLoad: function(dialog) {
                var container = $($(selectors.treeTemplate).html());
                dialog.setContent(container);

                treeViewModel = new TreeViewModel(pageModel);

                ko.applyBindings(treeViewModel, container.get(0));
            },

            onClose: function () {
                if (treeViewModel.contentsSorted) {
                    var changedRegions = checkIfRegionContentsChanged([], treeViewModel.items());
                    if (changedRegions.length > 0) {
                        contentModule.saveContentChanges(changedRegions);

                        return;
                    }
                }

                if (treeViewModel.reloadPage) {
                    redirect.ReloadWithAlert();
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

            //console.log("Level 1: Tree item: %s, type: %s", treeItem.type, treeItem.title());
            if (treeItem.type == treeItemTypes.region) {
                contentsBeforeReorder = treeItem.model.contents;
                contentsAfterReorder = [];

                for (j = 0; j < treeItem.items().length; j++) {
                    subTreeItem = treeItem.items()[j];

                    //console.log("Level 2: Subtree item: %s, type: %s", subTreeItem.type, subTreeItem.title());
                    if (subTreeItem.type == treeItemTypes.content) {
                        contentsAfterReorder.push(subTreeItem.model);
                    }

                    changedRegions = checkIfRegionContentsChanged(changedRegions, subTreeItem.items());
                }

                hasContentsChanged = contentModule.hasContentsOrderChanged(contentsBeforeReorder, contentsAfterReorder);
                if (hasContentsChanged) {
                    treeItem.model.contents = contentsAfterReorder;

                    for (j = 0; j < contentsAfterReorder.length; j++) {
                        contentsAfterReorder[j].region = treeItem.model;
                        contentsAfterReorder[j].parentContent = treeItem.model.parentContent;
                        contentsAfterReorder[j].parentPageContentId = treeItem.model.parentPageContentId;
                    }

                    changedRegions.push(treeItem.model);
                }
                // console.log("Region: %s, Before: %s, After: %s, Changed: %s", treeItem.title(), contentsBeforeReorder.length, contentsAfterReorder.length, hasContentsChanged);
            }
        }

        return changedRegions;
    }

    /**
    * Helper function to add knockout binding 'draggable'.
    */
    function addDraggableBinding() {
        ko.bindingHandlers.draggableContent = {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                if (!valueAccessor()) {
                    return;
                }

                var dragObject = viewModel,
                    setup = {
                        revert: true,
                        revertDuration: 0,
                        refreshPositions: true,
                        scroll: true,
                        containment: $(selectors.contentTreeContainer).get(0),
                        appendTo: $(selectors.contentTreeContainer).get(0),
                        helper: function () {
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

                $(element).draggable(setup).data("dragObject", dragObject);
            }
        };
    }

    /**
    * Helper function to add knockout binding 'droppable'.
    */
    function addDroppableBinding() {
        ko.bindingHandlers.droppableContent = {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                var accessor = valueAccessor();
                if (!accessor || !accessor.region || !accessor.type) {
                    return;
                }

                var dropRegionModel = accessor.region,
                    dropZoneObject = viewModel,
                    dropZoneType = accessor.type,
                    setup = {
                        tolerance: "pointer",
                        over: function () {
                            if (dropZoneObject.activeZone) {
                                dropZoneObject.activeZone(dropZoneType);
                            }
                        },
                        out: function () {
                            if (dropZoneObject.activeZone) {
                                dropZoneObject.activeZone(dropZoneTypes.none);
                            }
                        },
                        drop: function (event, ui) {
                            treeViewModel.contentsSorted = true;

                            var dragObject = $(ui.draggable).data("dragObject");
//                                forFix = $(ui.draggable).data('draggable'),
//                                originalDragObject = dragObject;
                            ui.helper.remove();

                            dragObject.parentRegion.removeContent(dragObject);
                            dragObject.parentRegion = dropRegionModel;

                            dragObject.isBeingDragged(false);
//                            dragObject.displayOrder(0);
//
                            // Add content to region where it should be.
                            var index;
                            if (dropZoneType == dropZoneTypes.topZone) {
                                index = $.inArray(dropZoneObject, dropRegionModel.items());
                                
                                console.log("Top zone: %s", index);
                                dropRegionModel.items.splice(index, 0, dragObject);
                            }
                            else if (dropZoneType == dropZoneTypes.middle) {
                                index = $.inArray(dropZoneObject, dropRegionModel.items());
                                if (index + 1 <= dropZoneObject, dropRegionModel.items().length) {
                                    index++;
                                }
                                console.log("Middle zone: %s", index);
                                dropRegionModel.items.splice(index, 0, dragObject);
                            } else {
                                dropRegionModel.items.push(dragObject);
                            }
                            dropZoneObject.activeZone(dropZoneTypes.none);
//
//                            sitemap.activeMapModel.updateNodesOrderAndParent();
//
                            // Fix for jQuery drag object.
//                            $(ui.draggable).data('draggable', forFix);
//
//                            if (originalDragObject.dropped && $.isFunction(originalDragObject.dropped)) {
//                                originalDragObject.dropped(dragObject);
//                            }
//
//                            updateValidation();
//                            bcms.trigger(events.sitemapNodeAdded, dragObject);
                        }
                    };
//                if (dropZoneObject.getSitemap && !dropZoneObject.getSitemap().settings.canDropNode) {
//                    return;
//                }
                $(element).droppable(setup);
            }
        };
    }

    /**
    * Initializes contents tree module.
    */
    tree.init = function () {
        bcms.logger.debug('Initializing contents tree module');

        // Bindings for sitemap nodes Drag'n'Drop.
        addDraggableBinding();
        addDroppableBinding();
    };

    /**
    * Subscribe to events
    */
    bcms.on(bcms.events.editContentsTree, onEditContentsTree);

    /**
    * Register initialization
    */
    bcms.registerInit(tree.init);

    return tree;
});