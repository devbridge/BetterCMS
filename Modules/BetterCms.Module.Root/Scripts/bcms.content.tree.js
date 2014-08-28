/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */
bettercms.define('bcms.content.tree', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.modal', 'bcms.content', 'bcms.redirect'],
    function ($, bcms, ko, modal, contentModule, redirect) {
    'use strict';

    var tree = {},
        selectors = {
            treeTemplate: '#bcms-contents-tree-template'
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
        treeViewModel = null;

    // Assign objects to module
    tree.selectors = selectors;
    tree.links = links;
    tree.globalization = globalization;

    function setContentModelValues(contentViewModel, json) {
        contentViewModel.contentId = json.Data.ContentId;
        contentViewModel.pageContentId = json.Data.PageContentId;
        contentViewModel.draft = json.Data.DesirableStatus == contentStatuses.draft;
        contentViewModel.title = json.Data.Title;
        contentViewModel.contentVersion = json.Data.ContentVersion;
        contentViewModel.pageContentVersion = json.Data.PageContentVersion;
        contentViewModel.contentType = json.Data.ContentType;
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
                model.title(json.Data.Title);
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
            contentModel.onConfigureContent();
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

            onClose: function() {
                if (treeViewModel.reloadPage) {
                    redirect.ReloadWithAlert();
                }
            }
        });
    }

    /**
    * Initializes contents tree module.
    */
    tree.init = function () {
        bcms.logger.debug('Initializing contents tree module');
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