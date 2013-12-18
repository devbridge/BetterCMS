/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.sitemap', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'bcms.ko.extenders', 'bcms.grid', 'bcms.security', 'bcms.tags'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, ko, grid, security, tags) {
        'use strict';

        var sitemap = {},
            selectors = {
                sitemapAddNodeDataBind: "#bcms-sitemap-addnode",
                sitemapAddNewPageDataBind: "#bcms-sitemap-addnewpage",
                sitemapForm: ".bcms-sitemap-form",
                
                searchField: '.bcms-search-query',
                searchButton: '#bcms-sitemaps-search-btn',

                siteSettingsSitemapsForm: "#bcms-sitemaps-form",
                siteSettingsSitemapCreateButton: '#bcms-create-sitemap-button',
                siteSettingsSitemapTitleCell: '.bcms-sitemap-title',
                siteSettingsRowCells: 'td',
                siteSettingsSitemapParentRow: 'tr:first',
                siteSettingsSitemapEditButton: '.bcms-grid-item-edit-button',
                siteSettingsSitemapDeleteButton: '.bcms-grid-item-delete-button',
                siteSettingsSitemapRowTemplate: '#bcms-sitemap-list-row-template',
                siteSettingsSitemapRowTemplateFirstRow: 'tr:first',
                siteSettingsSitemapsTableFirstRow: 'table.bcms-tables > tbody > tr:first'
            },
            links = {
                loadSiteSettingsSitemapsListUrl: null,
                saveSitemapUrl: null,
                saveSitemapNodeUrl: null,
                deleteSitemapUrl: null,
                deleteSitemapNodeUrl: null,
                sitemapEditDialogUrl: null,
                sitemapAddNewPageDialogUrl: null
            },
            globalization = {
                sitemapCreatorDialogTitle: null,
                sitemapEditorDialogTitle: null,
                sitemapEditorDialogCustomLinkTitle: null,
                sitemapAddNewPageDialogTitle: null,
                sitemapDeleteNodeConfirmationMessage: null,
                sitemapDeleteConfirmMessage: null,
                sitemapSomeNodesAreInEditingState: null,
                sitemapNodeSaveButton: null,
                sitemapNodeOkButton: null
            },
            defaultIdValue = '00000000-0000-0000-0000-000000000000',
            DropZoneTypes = {
                None: 'none',
                EmptyListZone: 'emptyListZone',
                TopZone: 'topZone',
                MiddleZone: 'middleZone',
                BottomZone: 'bottomZone'
            },
            nodeId = 0;

        /**
        * Assign objects to module.
        */
        sitemap.links = links;
        sitemap.globalization = globalization;
        sitemap.activeMapModel = null;
        sitemap.activeLoadingContainer = null;
        sitemap.activeMessageContainer = null;

        /**
        * Loads a sitemap view to the site settings container.
        */
        sitemap.loadSiteSettingsSitemapList = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsSitemapsListUrl, {
                contentAvailable: function() {
                    sitemap.initializeSitemapsList(siteSettings.getMainContainer());
                }
            });
        };

        /**
        * Initializes site settings master pages list.
        */
        sitemap.initializeSitemapsList = function (container) {
            var dialog = siteSettings.getModalDialog(),
                form = container.find(selectors.siteSettingsSitemapsForm);

            grid.bindGridForm(form, function (htmlContent, data) {
                container.html(htmlContent);
                sitemap.initializeSitemapsList(container);
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSitemaps(form, container);
                return false;
            });

            form.find(selectors.searchField).keypress(function (event) {
                if (event.which == 13) {
                    bcms.stopEventPropagation(event);
                    searchSitemaps(form, container);
                }
            });

            form.find(selectors.searchButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                searchSitemaps(form, container);
            });

            initializeListItems(container);

            // Select search.
            dialog.setFocus();
        };
        
        /*
        * Submit sitemap form to force search.
        */
        function searchSitemaps(form, container) {
            grid.submitGridForm(form, function (htmlContent, data) {
                container.html(htmlContent);
                sitemap.initializeSitemapsList(container);
                var searchInput = container.find(selectors.searchField);
                grid.focusSearchInput(searchInput);
            });
        };

        /*
        * Attach the sitemap grid events.
        */
        function initializeListItems(container, masterContainer) {
            container.find(selectors.siteSettingsSitemapCreateButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                addSitemap(masterContainer || container);
            });

            container.find(selectors.siteSettingsSitemapEditButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                editSitemap($(this), masterContainer || container);
            });

            container.find(selectors.siteSettingsSitemapDeleteButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                deleteSitemap($(this), masterContainer || container);
            });
        };

        /*
        * Create the sitemap and add it to the list.
        */
        function addSitemap(container) {
            sitemap.openCreateSitemapDialog(function (data) {
                if (data.Data != null) {
                    var template = $(selectors.siteSettingsSitemapRowTemplate),
                        newRow = $(template.html()).find(selectors.siteSettingsSitemapRowTemplateFirstRow);

                    newRow.find(selectors.siteSettingsSitemapTitleCell).html(data.Data.Title);
                    newRow.find(selectors.siteSettingsSitemapEditButton).data('id', data.Data.Id);
                    newRow.find(selectors.siteSettingsSitemapDeleteButton).data('id', data.Data.Id);
                    newRow.find(selectors.siteSettingsSitemapDeleteButton).data('version', data.Data.Version);

                    newRow.insertBefore($(selectors.siteSettingsSitemapsTableFirstRow, container));

                    initializeListItems(newRow, container);

                    grid.showHideEmptyRow(container);
                }
            }, true);
        };

        /*
        * Edit the sitemap.
        */
        function editSitemap(self, container) {
            var id = self.data('id');

            sitemap.loadAddNodeDialog(id, function (data) {
                if (data.Data != null) {
                    var row = self.parents(selectors.siteSettingsSitemapParentRow),
                        cell = row.find(selectors.siteSettingsSitemapTitleCell);
                    cell.html(data.Data.Title);
                    row.find(selectors.siteSettingsSitemapDeleteButton).data('version', data.Data.Version);
                }
            }, globalization.sitemapEditorDialogTitle);
        };

        /*
        * Delete the sitemap and remove it form the list.
        */
        function deleteSitemap(self, container) {
            var id = self.data('id'),
                version = self.data('version');

            sitemap.deleteSitemap(id, version, function (json) {
                messages.refreshBox(selectors.siteSettingsSitemapsForm, json);
                if (json.Success) {
                    self.parents(selectors.siteSettingsSitemapParentRow).remove();
                    grid.showHideEmptyRow(container);
                }
            });
        };

        /*
        * Shows sitemap creation form.
        */
        sitemap.openCreateSitemapDialog = function (callBackOnSuccess) {
            sitemap.loadAddNodeDialog(defaultIdValue, callBackOnSuccess, globalization.sitemapCreatorDialogTitle);
        };

        /**
        * Loads a sitemap view to the add node dialog container.
        */
        sitemap.loadAddNodeDialog = function (id, onClose, dialogTitle) {
            var addNodeController = new AddNodeMapController();
            modal.open({
                title: dialogTitle || globalization.sitemapEditorDialogTitle,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, $.format(links.sitemapEditDialogUrl, id), {
                        done: function (content) {
                            addNodeController.initialize(content, dialog);
                        }
                    });
                },
                onAccept: function (dialog) {
                    addNodeController.save(function (json) {
                        if (json.Success) {
                            dialog.close();
                            if (onClose && $.isFunction(onClose)) {
                                onClose(json);
                            }
                            messages.refreshBox(selectors.siteSettingsSitemapsForm, json);
                        } else {
                            sitemap.showMessage(json);
                        }
                    });
                    return false;
                }
            });
        };

        /*
        * Deletes the sitemap if user confirms.
        */
        sitemap.deleteSitemap = function (id, version, callBack) {
            var url = $.format(links.deleteSitemapUrl, id, version),
                onDeleteCompleted = function (json) {
                if ($.isFunction(callBack)) {
                    callBack(json);
                }
            };
            modal.confirm({
                content: globalization.sitemapDeleteConfirmMessage,
                onAccept: function () {
                    $.ajax({
                        type: 'POST',
                        url: url,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
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
        };

        /**
        * Shows add new page to sitemap dialog.
        */
        sitemap.loadAddNewPageDialog = function(data) {
            if (data && data.Data && (data.Data.Title || data.Data.PageTitle) && (data.Data.Url || data.Data.PageUrl) && (data.Data.Id || data.Data.PageId) && !data.Data.IsMasterPage) {
                var addPageController = new AddNewPageMapController(data.Data.Title || data.Data.PageTitle, data.Data.Url || data.Data.PageUrl, data.Data.Id || data.Data.PageId);
                modal.open({
                    title: globalization.sitemapAddNewPageDialogTitle,
                    onLoad: function(dialog) {
                        dynamicContent.setContentFromUrl(dialog, links.sitemapAddNewPageDialogUrl, {
                            done: function(content) {
                                addPageController.initialize(content, dialog);
                            }
                        });
                    },
                    onAccept: function(dialog) {
                        addPageController.save(function() {
                            dialog.close();
                        });
                        return false;
                    },
                    onClose: function() {
                        if (data.Callback && $.isFunction(data.Callback)) {
                            data.Callback(data);
                        }
                    }
                });
            } else {
                if (data.Callback && $.isFunction(data.Callback)) {
                    data.Callback(data);
                }
            }
        };


        // --- Controllers ----------------------------------------------------
        /**
        * Controller for sitemap add node dialog.
        */
        function AddNodeMapController() {
            var self = this;
            self.container = null;
            self.pageLinksModel = null;

            self.initialize = function (content, dialog) {
                self.container = dialog.container;
                sitemap.activeMessageContainer = self.container;
                sitemap.activeLoadingContainer = self.container.find(selectors.sitemapAddNodeDataBind);

                sitemap.showMessage(content);
                if (content.Success) {
                    // Create data models.
                    var sitemapModel = new SitemapViewModel(content.Data.Sitemap);
                    // TODO: update sitemap editing for read only mode.
                    sitemapModel.parseJsonNodes(content.Data.Sitemap.RootNodes);
                    self.pageLinksModel = new SearchPageLinksViewModel(sitemapModel);
                    self.pageLinksModel.parseJsonLinks(content.Data.PageLinks);
                    sitemap.activeMapModel = sitemapModel;

                    // Setup settings.
                    sitemapModel.settings.canEditNode = true;
                    sitemapModel.settings.canDeleteNode = true;
                    sitemapModel.settings.canDragNode = true;
                    sitemapModel.settings.canDropNode = true;
                    sitemapModel.settings.nodeSaveButtonTitle = globalization.sitemapNodeOkButton;
                    sitemapModel.settings.nodeSaveAfterUpdate = false;

                    // Bind models.
                    var context = self.container.find(selectors.sitemapAddNodeDataBind).get(0);
                    if (context) {
                        ko.applyBindings(self.pageLinksModel, context);
                        updateValidation();
                    }
                }
            };
            self.save = function (onDoneCallback) {
                if (sitemap.activeMapModel) {
                    sitemap.activeMapModel.save(onDoneCallback);
                }
            };
        }
        
        /**
        * Controller for sitemap add new page dialog.
        */
        function AddNewPageMapController(title, url, id) {
            var self = this;
            self.container = null;
            self.newPageModel = null;
            self.tabsModel = null;
            
            self.initialize = function (content, dialog) {
                if (!(content != null && content.Data != null && content.Data.length > 0)) {
                    // No sitemaps to place the new page.
                    dialog.close();
                    return;
                }

                self.container = dialog.container;
                sitemap.activeMessageContainer = self.container;
                sitemap.activeLoadingContainer = self.container.find(selectors.sitemapAddNewPageDataBind);

                sitemap.showMessage(content);
                if (content.Success) {
                    var tabs = [],
                        onSkip = function() {
                            dialog.close();
                        };
                    
                    // Create all the tabs.
                    for (var i = 0; i < content.Data.length; i++) {
                        // Create data models.
                        var sitemapModel = new SitemapViewModel(content.Data[i]),
                            pageLinkModel = new PageLinkViewModel(title, url, id),
                            newPageModel = new AddNewPageViewModel(sitemapModel, pageLinkModel, onSkip),
                            tabModel = new TabModel(newPageModel);
                        tabs.push(tabModel);
                        sitemapModel.parseJsonNodes(content.Data[i].RootNodes);

                        // Setup settings.
                        sitemapModel.settings.canEditNode = false;
                        sitemapModel.settings.canDeleteNode = false;
                        sitemapModel.settings.canDragNode = false;
                        sitemapModel.settings.canDropNode = true;
                        sitemapModel.settings.nodeSaveButtonTitle = globalization.sitemapNodeSaveButton;
                        sitemapModel.settings.nodeSaveAfterUpdate = false;
                    }

                    self.tabsModel = new TabsModel(tabs);

                    // Bind models.
                    var context = self.container.find(selectors.sitemapAddNewPageDataBind).parent().get(0);
                    if (context) {
                        ko.applyBindings(self.tabsModel, context);
                        updateValidation();
                    }
                }
            };
            self.save = function (onDoneCallback) {
                // TODO: refactor saving to save all the sitemaps at once.
                var tabs = self.tabsModel.tabs(),
                    totalTabs = tabs.length,
                    totalResponses = 0;
                
                for (var i = 0; i < totalTabs; i++) {
                    tabs[i].newPageViewModel.sitemap.save(function(data) {
                        totalResponses++;
                        if (totalResponses === totalTabs) {
                            if (onDoneCallback && $.isFunction(onDoneCallback)) {
                                onDoneCallback(data);
                            }
                        }
                    });
                }
            };
        }
        // --------------------------------------------------------------------
        

        // --- Helpers --------------------------------------------------------
        /**
        * Helper function to show json messages.
        */
        sitemap.showMessage = function (content) {
            messages.refreshBox(sitemap.activeMessageContainer, content);
        };
        
        /**
        * Helper function to show loading.
        */
        sitemap.showLoading = function (loading) {
            if (loading) {
                $(sitemap.activeLoadingContainer).showLoading();
            } else {
                $(sitemap.activeLoadingContainer).hideLoading();
            }
        };
        
        /**
        * Helper function to add knockout binding 'draggable'.
        */
        function addDraggableBinding() {
            ko.bindingHandlers.draggable = {
                init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
                    var dragObject = viewModel,
                        setup = {
                            revert: true,
                            revertDuration: 0,
                            refreshPositions: true,
                            scroll: true,
                            containment: $(selectors.sitemapAddNodeDataBind).get(0) || $(selectors.sitemapAddNewPageDataBind).get(0),
                            appendTo: $(selectors.sitemapAddNodeDataBind).get(0) || $(selectors.sitemapAddNewPageDataBind).get(0),
                            helper: function () {
                                if (dragObject.isExpanded) {
                                    dragObject.isExpanded(false);
                                }
                                if (dragObject.isBeingDragged) {
                                    dragObject.isBeingDragged(true);
                                }
                                return $(this).clone().width($(this).width()).css({ zIndex: 9999 });
                            },
                            start: function() {
                                $(this).hide();
                            },
                            stop: function(event, ui) {
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
        * Helper function to add knockout binding 'droppable'.
        */
        function addDroppableBinding() {
            ko.bindingHandlers.droppable = {
                init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
                    var dropZoneObject = viewModel,
                        dropZoneType = valueAccessor(),
                        setup = {
                            tolerance: "pointer",
                            over: function() {
                                dropZoneObject.activeZone(dropZoneType);
                            },
                            out: function() {
                                dropZoneObject.activeZone(DropZoneTypes.None);
                            },
                            drop: function (event, ui) {
                                var forFix = $(ui.draggable).data('draggable'),
                                    dragObject = $(ui.draggable).data("dragObject"),
                                    originalDragObject = dragObject;
                                ui.helper.remove();

                                if (dragObject.parentNode && dragObject.parentNode()) {
                                    dragObject.parentNode().childNodes.remove(dragObject);
                                } else {
                                    // Create node from page link.
                                    var node = new NodeViewModel();
                                    node.title(dragObject.title());
                                    node.url(dragObject.url());
                                    if (dropZoneType == DropZoneTypes.EmptyListZone || dropZoneType == DropZoneTypes.MiddleZone) {
                                        node.parentNode(dropZoneObject);
                                    } else {
                                        node.parentNode(dropZoneObject.parentNode());
                                    }
                                    if (dragObject.isCustom()) {
                                        node.startEditSitemapNode();
                                        node.callbackAfterSuccessSaving = function () {
                                            sitemap.activeMapModel.updateNodesOrderAndParent();
                                        };
                                        node.callbackAfterFailSaving = function (newNode) {
                                            newNode.parentNode().childNodes.remove(newNode);
                                        };
                                    }
                                    node.superDraggable(dragObject.superDraggable());
                                    dragObject = node;
                                }
                                
                                dragObject.isBeingDragged(false);
                                
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
                                
                                sitemap.activeMapModel.updateNodesOrderAndParent();

                                // Fix for jQuery drag object.
                                $(ui.draggable).data('draggable', forFix);
                                
                                if (originalDragObject.dropped && $.isFunction(originalDragObject.dropped)) {
                                    originalDragObject.dropped(dragObject);
                                }
                                
                                updateValidation();
                            }
                        };
                    if (dropZoneObject.getSitemap && !dropZoneObject.getSitemap().settings.canDropNode) {
                        return;
                    }
                    $(element).droppable(setup);
                }
            };
        }

        /**
        * Helper function to update nodes in list.
        */
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
        
        /**
        * Helper function to update validation.
        */
        function updateValidation() {
            var form = $(selectors.sitemapForm);
            bcms.updateFormValidator(form);
        }
        // --------------------------------------------------------------------
        

        // --- View Models ----------------------------------------------------
        /**
        * Responsible for sitemap structure.
        */
        function SitemapViewModel(jsonSitemap) {
            var self = this;
            
            self.id = function () { return jsonSitemap.Id; };
            self.childNodes = ko.observableArray([]);
            self.childNodes.subscribe(updateFirstLastNode);
            self.someNodeIsOver = ko.observable(false);     // Someone is dragging some node over the sitemap, but not over the particular node.
            self.activeZone = ko.observable(DropZoneTypes.None);
            self.showHasNoDataMessage = ko.observable(false);
            self.savingInProgress = false;                  // To prevent multiple saving.

            self.version = jsonSitemap.Version;
            self.title = ko.observable(jsonSitemap.Title);
            self.tags = new tags.TagsListViewModel(jsonSitemap.Tags);
            self.accessControl = security.createUserAccessViewModel(jsonSitemap.UserAccessList);

            self.settings = {
                canEditNode: false,
                canDeleteNode: false,
                canDragNode: false,
                canDropNode: false,
                nodeSaveButtonTitle: globalization.sitemapNodeOkButton,
                nodeSaveAfterUpdate: false
            };

            self.getSitemap = function () {
                return self;
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
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i],
                        saveIt = false;
                    
                    if (node.displayOrder() != i) {
                        saveIt = true;
                        node.displayOrder(i);
                    }

                    if (node.parentNode() != parent) {
                        saveIt = true;
                        node.parentNode(parent);
                    }

                    if (saveIt || node.id() == defaultIdValue) {
                        node.saveSitemapNode();
                    }

                    self.updateNodes(node.childNodes(), node);
                }
            };

            self.focusOnActiveNode = function(nodes) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].isActive()) {
                        $($('input', '#' + nodes[i].containerId).get(0)).focus();
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
                    var messagesBox = messages.box({ container: sitemap.activeMessageContainer });
                    messagesBox.clearMessages();
                    messagesBox.addWarningMessage(globalization.sitemapSomeNodesAreInEditingState);
                    return;
                }

                var dataToSend = JSON.stringify(self.getModelToSave()),
                    onSaveCompleted = function (json) {
                        messages.refreshBox(sitemap.activeMessageContainer, json);
                        sitemap.showLoading(false);
                        if (json.Success) {
                            if (onDoneCallback && $.isFunction(onDoneCallback)) {
                                if (json.Data == null) {
                                    json.Data = {
                                        Title: self.title()
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
                        url: links.saveSitemapUrl,
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        cache: false,
                        data: dataToSend,
                        beforeSend: function () { sitemap.showLoading(true); }
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
                    node.fromJson(jsonNodes[i]);
                    node.parentNode(self);
                    nodes.push(node);
                }
                self.childNodes(nodes);
            };
            self.composeJsonNodes = function () {
                return self.nodesToJson(self.childNodes());
            };
            self.nodesToJson = function(nodes) {
                var result = [];
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i];
                    result.push({
                            Id: node.id(),
                            Version: node.version(),
                            Title: node.title(),
                            Url: node.url(),
                            DisplayOrder: node.displayOrder(),
                            IsDeleted: node.isDeleted(),
                            ChildNodes: self.nodesToJson(node.childNodes())
                        }
                    );
                }
                return result;
            };
            self.getModelToSave = function () {
                var tagList = [],
                    tagModels = self.tags.items(),
                    userAccessList = [],
                    accessModels = self.accessControl.UserAccessList(),
                    i;
                
                for (i = 0; i < tagModels.length; i++) {
                    tagList.push(tagModels[i].name());
                }

                for (i = 0; i < accessModels.length; i++) {
                    userAccessList.push({
                        Identity: accessModels[i].Identity(),
                        AccessLevel: accessModels[i].AccessLevel(),
                        IsForRole: accessModels[i].IsForRole(),
                    });
                }

                return {
                    Id: self.id(),
                    Version: self.version,
                    Title: self.title(),
                    RootNodes: self.composeJsonNodes(),
                    Tags: tagList,
                    UserAccessList: userAccessList
                };
            };
        }
        
        /**
        * Responsible for sitemap node data.
        */
        function NodeViewModel() {
            var self = this;
            
            // Data fields.
            self.id = ko.observable(defaultIdValue);
            self.version = ko.observable(0);
            self.title = ko.observable();
            self.url = ko.observable();
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
            self.superDraggable = ko.observable(false);     // Used to force dragging if sitemap settings !canDragNode.

            // User for validation.
            self.containerId = 'node-' + nodeId++;
            
            // For search results.
            self.isVisible = ko.observable(true);
            self.isSearchResult = ko.observable(false);
            
            // Data manipulation.
            self.startEditSitemapNode = function () {
                self.titleOldValue = self.title();
                self.urlOldValue = self.url();
                self.isActive(true);
            };
            self.cancelEditSitemapNode = function () {
                self.isActive(false);
                self.title(self.titleOldValue);
                self.url(self.urlOldValue);
                if (self.id() == defaultIdValue) {
                    self.parentNode().childNodes.remove(self);
                }
            };
            self.saveSitemapNodeWithValidation = function () {
                var inputFields = $('input', '#' + self.containerId);
                if (inputFields.valid()) {
                    self.saveSitemapNode();
                    inputFields.blur();
                    self.isActive(false);
                }
            };
            self.saveSitemapNode = function () {
                if (self.getSitemap() == null || !self.getSitemap().settings.nodeSaveAfterUpdate) {
                    return;
                }
                
                var params = self.toJson(),
                    onSaveCompleted = function (json) {
                        sitemap.showMessage(json);
                        if (json.Success) {
                            if (json.Data) {
                                self.id(json.Data.Id);
                                self.version(json.Data.Version);
                                if (self.callbackAfterSuccessSaving && $.isFunction(self.callbackAfterSuccessSaving)) {
                                    self.callbackAfterSuccessSaving(self);
                                }
                            }
                        } else {
                            if (self.callbackAfterFailSaving && $.isFunction(self.callbackAfterFailSaving)) {
                                self.callbackAfterFailSaving(self);
                            }
                        }
                        sitemap.showLoading(false);
                    };
                sitemap.showLoading(true);
                $.ajax({
                    url: links.saveSitemapNodeUrl,
                    type: 'POST',
                    dataType: 'json',
                    cache: false,
                    data: params
                })
                    .done(function (json) {
                        onSaveCompleted(json);
                    })
                    .fail(function (response) {
                        onSaveCompleted(bcms.parseFailedResponse(response));
                    });
            };
            self.deleteSitemapNode = function () {
                // Show confirmation dialog.
                var deleting = false,
                    message = $.format(globalization.sitemapDeleteNodeConfirmationMessage, self.title()),
                    confirmDialog = modal.confirm({
                        content: message,
                        onAccept: function () {
                            if (self.getSitemap() == null || !self.getSitemap().settings.nodeSaveAfterUpdate) {
                                self.isDeleted(true);
                                confirmDialog.close();
                                return false;
                            }
                            if (!deleting) {
                                deleting = true;
                                var params = self.toJson(),
                                    onDeleteCompleted = function(json) {
                                        deleting = false;
                                        sitemap.showMessage(json);
                                        try {
                                            if (json.Success) {
                                                self.isDeleted(true);
                                                self.parentNode().childNodes.remove(self);
                                            }
                                            sitemap.showLoading(false);
                                        } finally {
                                            confirmDialog.close();
                                        }
                                    };
                                sitemap.showLoading(true);
                                $.ajax({
                                    url: links.deleteSitemapNodeUrl,
                                    type: 'POST',
                                    dataType: 'json',
                                    cache: false,
                                    data: params
                                })
                                    .done(function (json) {
                                        onDeleteCompleted(json);
                                    })
                                    .fail(function(response) {
                                        onDeleteCompleted(bcms.parseFailedResponse(response));
                                    });
                            }
                            return false;
                        }
                    });
            };
            self.callbackAfterSuccessSaving = null;
            self.callbackAfterFailSaving = null;

            self.getSitemap = function() {
                if (self.parentNode() != null) {
                    return self.parentNode().getSitemap();
                }
                return null;
            };

            self.fromJson = function(jsonNode) {
                self.id(jsonNode.Id);
                self.version(jsonNode.Version);
                self.title(jsonNode.Title);
                self.url(jsonNode.Url);
                self.displayOrder(jsonNode.DisplayOrder);

                var nodes = [];
                if (jsonNode.ChildNodes != null) {
                    for (var i = 0; i < jsonNode.ChildNodes.length; i++) {
                        var node = new NodeViewModel();
                        node.fromJson(jsonNode.ChildNodes[i]);
                        node.parentNode(self);
                        nodes.push(node);
                    }
                }
                self.childNodes(nodes);
            };
            self.toJson = function () {
                var params = {
                    SitemapId: defaultIdValue, // TODO: update.
                    Id: self.id(),
                    Version: self.version(),
                    Title: self.title(),
                    Url: self.url(),
                    DisplayOrder: self.displayOrder(),
                    ParentId: self.parentNode().id()
                };
                return params;
            };
        }

        /**
        * Responsible for page searching.
        */
        function SearchPageLinksViewModel(sitemapViewModel) {
            var self = this;
            self.searchQuery = ko.observable("");
            self.pageLinks = ko.observableArray([]);
            self.sitemap = sitemapViewModel;
            self.hasfocus = ko.observable(true);

            self.searchForPageLinks = function () {
                var showAll = $.trim(self.searchQuery()).length === 0;
                var pageLinks = self.pageLinks();
                for (var i = 0; i < pageLinks.length; i++) {
                    if (showAll) {
                        pageLinks[i].isVisible(true);
                    } else {
                        var link = pageLinks[i],
                            searchQuery = self.searchQuery().toLowerCase(),
                            title = link.title().toLowerCase(),
                            url = link.url().toLowerCase();
                        link.isVisible(title.indexOf(searchQuery) !== -1 || url.indexOf(searchQuery) !== -1);
                    }
                }
                
                self.hasfocus(true);
            };
            
            // Parse.
            self.parseJsonLinks = function (jsonLinks) {
                var pageLinks = [],
                    customLink = new PageLinkViewModel();
                
                customLink.title(globalization.sitemapEditorDialogCustomLinkTitle);
                customLink.url('/../');
                customLink.isCustom(true);
                pageLinks.push(customLink);
                
                for (var i = 0; i < jsonLinks.length; i++) {
                    var link = new PageLinkViewModel();
                    link.fromJson(jsonLinks[i]);
                    pageLinks.push(link);
                }
                self.pageLinks(pageLinks);
            };
            
            self.title = sitemapViewModel.title;
            self.tags = sitemapViewModel.tags;
            self.accessControl = sitemapViewModel.accessControl;
        }
        
        /**
        * Responsible for page link data.
        */
        function PageLinkViewModel(title, url, id) {
            var self = this;
            self.title = ko.observable(title);
            self.url = ko.observable(url);
            self.id = ko.observable(id);
            self.isVisible = ko.observable(true);
            self.isCustom = ko.observable(false);
            self.isBeingDragged = ko.observable(false);
            self.superDraggable = ko.observable(false); // Used to force dragging if sitemap settings !canDragNode.

            self.onDrop = null;
            self.dropped = function (droppedSitemapNode) {
                if(self.onDrop && $.isFunction(self.onDrop)) {
                    self.onDrop(droppedSitemapNode);
                }
            };
            self.fromJson = function (json) {
                self.isVisible(true);
                self.title(json.Title);
                self.url(json.Url);
                self.id(json.Id);
            };
        }
        
        /**
        * Responsible for new page data and sitemap.
        */
        function AddNewPageViewModel(sitemapViewModel, pageLinkViewModel, onSkip) {
            var self = this;
            self.pageLink = pageLinkViewModel;
            self.pageLink.superDraggable(true);
            self.sitemap = sitemapViewModel;
            self.onSkipClick = onSkip;
            self.linkIsDropped = ko.observable(false);
            self.sitemapNode = null;

            self.pageLink.onDrop = function (droppedSitemapNode) {
                self.linkIsDropped(true);
                self.pageLink.isVisible(false);
                self.sitemapNode = droppedSitemapNode;
            };

            self.skipClicked = function () {
                if (self.onSkipClick && $.isFunction(self.onSkipClick)) {
                    self.onSkipClick();
                }
            };
            self.undoClicked = function () {
                if (self.sitemapNode != null) {
                    // Remove newly created node from sitemap.
                    self.sitemapNode.parentNode().childNodes.remove(self.sitemapNode);
                    self.sitemapNode = null;
                }
                self.pageLink.isVisible(true);
                self.linkIsDropped(false);
            };
        }
        
        function TabsModel(tabs) {
            var self = this;
            self.activeTab = ko.observable();
            self.activeNewPageViewModel = ko.observable();
            self.activateTab = function(tabModel) {
                var active = self.activeTab();
                if (active) {
                    active.isActive(false);
                }
                tabModel.isActive(true);
                sitemap.activeMapModel = tabModel.newPageViewModel.sitemap;
                self.activeTab(tabModel);
                self.activeNewPageViewModel(tabModel.newPageViewModel);
            };

            for (var i = 0; i < tabs.length; i++) {
                tabs[i].activateTab = self.activateTab;
            }
            
            self.tabs = ko.observableArray(tabs);

            // Activate first tab.
            self.tabs()[0].activate();
        }
        
        function TabModel(newPageViewModel) {
            var self = this;
            self.newPageViewModel = newPageViewModel;
            self.isActive = ko.observable(false);
            self.activateTab = null;
            self.activate = function() {
                self.activateTab(self);
            };
        }
        // --------------------------------------------------------------------


        /**
        * Initializes module.
        */
        sitemap.init = function() {
            bcms.logger.debug('Initializing bcms.pages.sitemap module.');
            
            // Bindings for sitemap nodes Drag'n'Drop.
            addDraggableBinding();
            addDroppableBinding();
            
            // Subscribe to events.
            bcms.on(bcms.events.pageCreated, sitemap.loadAddNewPageDialog);
        };
    
        /**
        * Register initialization.
        */
        bcms.registerInit(sitemap.init);
    
        return sitemap;
});
