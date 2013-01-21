/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.sitemap', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'knockout'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, ko) {
        'use strict';

        var sitemap = {},
            selectors = {
                sitemapSearchDataBind: "#bcms-sitemap-form",
                sitemapForm: "#bcms-sitemap-form",
            },
            links = {
                loadSiteSettingsSitemapUrl: null,
                saveSitemapNodeUrl: null,
                deleteSitemapNodeUrl: null,
                sitemapEditDialogUrl: null,
            },
            globalization = {
                sitemapEditorDialogTitle: null,
                sitemapEditorDialogClose: null,
            },
            defaultIdValue = '00000000-0000-0000-0000-000000000000',
            DropZoneTypes = {
                None: 'none',
                EmptyListZone: 'emptyListZone',
                TopZone: 'topZone',
                MiddleZone: 'middleZone',
                BottomZone: 'bottomZone'
            };

        /**
        * Assign objects to module.
        */
        sitemap.links = links;
        sitemap.globalization = globalization;

        /**
        * Loads a sitemap view to the site settings container.
        */
        sitemap.loadSiteSettingsSitmap = function () {
            var sitemapController = new SiteSettingsMapController();
            
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsSitemapUrl, {
                contentAvailable: sitemapController.initialize
            });
        };

        // --- Controllers ----------------------------------------------------
        function SiteSettingsMapController() {
            var self = this;
            self.container = null;
            self.sitemapSearchModel = null;

            self.initialize = function(content) {
                self.container = siteSettings.getModalDialog().container;

                // messages.refreshBox(messagesContainer, content);
                if (content.Success) {
                    // Create data models.
                    var sitemapModel = new SitemapViewModel();
                    sitemapModel.parseJsonNodes(content.Data.RootNodes);
                    self.sitemapSearchModel = new SearchSitemapViewModel(sitemapModel);
                    
                    // Bind models.
                    var context = self.container.find(selectors.sitemapSearchDataBind).get(0);
                    if (context) {
                        // Add additional bindings for sitemap nodes Drag'n'Drop.
                        addDraggableBinding();
                        addDroppableBinding(sitemapModel);
                        ko.applyBindings(self.sitemapSearchModel, context);

                        // Update validation.
                        var form = self.container.find(selectors.sitemapForm);
                        if ($.validator && $.validator.unobtrusive) {
                            form.removeData("validator");
                            form.removeData("unobtrusiveValidation");
                            $.validator.unobtrusive.parse(form);
                        }
                    }
                }
            };
        }
        // --------------------------------------------------------------------
        

        // --- Helpers --------------------------------------------------------
        function addDraggableBinding() {
            ko.bindingHandlers.draggable = {
                init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var dragObject = viewModel,
                        setup = {
                            revert: "invalid",
                            start: function (event, ui) {
                                dragObject.isExpanded(false);
                                dragObject.isBeingDragged(true);
                            },
                            stop: function(event, ui) {
                                dragObject.isBeingDragged(false);
                            }
                        };
                    $(element).draggable(setup).data("dragObject", dragObject);
                    $(element).disableSelection();
                }
            };
        }
        function addDroppableBinding(sitemapModel) {
            ko.bindingHandlers.droppable = {
                init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var dropZoneObject = viewModel,
                        dropZoneType = valueAccessor(),
                        setup = {
                            tolerance: "pointer",
                            over: function(event, ui) {
                                dropZoneObject.activeZone(dropZoneType);
                            },
                            out: function(event, ui) {
                                dropZoneObject.activeZone(DropZoneTypes.None);
                            },
                            drop: function (event, ui) {
                                var dragObject = $(ui.draggable).data("dragObject");
                                dragObject.parentNode.childNodes.remove(dragObject);
                                var index = dropZoneObject.parentNode.childNodes().indexOf(dropZoneObject);
                                dragObject.isBeingDragged(false);
                                if (dropZoneType == DropZoneTypes.TopZone) {
                                    dropZoneObject.parentNode.childNodes.splice(index, 0, dragObject);
                                }
                                else if (dropZoneType == DropZoneTypes.MiddleZone) {
                                    dropZoneObject.childNodes.splice(0, 0, dragObject);
                                    dropZoneObject.isExpanded(true);
                                }
                                else if (dropZoneType == DropZoneTypes.BottomZone) {
                                    dropZoneObject.parentNode.childNodes.splice(index + 1, 0, dragObject);
                                }
                                dropZoneObject.activeZone(DropZoneTypes.None);
                                sitemapModel.updateNodesOrderAndParent();
                            }
                        };
                    $(element).droppable(setup);
                }
            };
        }
        // --------------------------------------------------------------------
        

        // --- View Models ----------------------------------------------------
        // Responsible for searching in sitemap.
        function SearchSitemapViewModel(sitemapViewModel) {
            var self = this;
            self.searchQuery = ko.observable();
            self.sitemap = sitemapViewModel;
            
            self.searchForNodes = function () {
                if(sitemap)
                {
                    // TODO: implement.
                    alert('searchForNodes()!');
                }
            };

            self.editSitemapClicked = function() {
                // TODO: implement.
                alert('editSitemapClicked()!');
            };
        }
        
        // Responsible for sitemap structure.
        function SitemapViewModel() {
            var self = this;
            self.id = function() { return defaultIdValue; };
            self.childNodes = ko.observableArray([]);
            self.someNodeIsOver = ko.observable(false);     // Someone is dragging some node over the sitemap, but not over the particular node.
            self.activeZone = ko.observable(DropZoneTypes.None);

            // Expanding or collapsing nodes.
            self.expandAll = function () {
                self.expandOrCollapse(self.childNodes(), true);
            };
            self.collapseAll = function () {
                self.expandOrCollapse(self.childNodes(), false);
            };
            self.expandOrCollapse = function (nodes, expand) {
                for (var i in nodes) {
                    var node = nodes[i];
                    node.isExpanded(expand);
                    self.expandOrCollapse(node.childNodes(), expand);
                }
            };
            
            // Updating display order and parent node info.
            self.updateNodesOrderAndParent = function () {
                self.updateNodes(self.childNodes(), self);
            };
            self.updateNodes = function (nodes, parent) {
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i],
                        saveIt = false;
                    
                    node.isFirstNode(i == 0);

                    if (node.displayOrder() != i) {
                        saveIt = true;
                        node.displayOrder(i);
                    }

                    if (node.parentNode != parent) {
                        saveIt = true;
                        node.parentNode = parent;
                    }

                    if (saveIt) {
                        node.saveSitemapNode();
                    }

                    self.updateNodes(node.childNodes(), node);
                }
            };
            
            // Parse.
            self.parseJsonNodes = function (jsonNodes) {
                var nodes = [];
                for (var i = 0; i < jsonNodes.length; i++) {
                    var node = new NodeViewModel();
                    node.fromJson(jsonNodes[i]);
                    node.parentNode = self;
                    node.isFirstNode(i == 0);
                    nodes.push(node);
                }
                self.childNodes(nodes);
            };
        }
        
        // Responsible for sitemap node data.
        function NodeViewModel() {
            var self = this;
            
            // Data fields.
            self.id = ko.observable();
            self.version = ko.observable();
            self.title = ko.observable();
            self.url = ko.observable();
            self.displayOrder = ko.observable(0);
            self.childNodes = ko.observableArray([]);
            self.parentNode = null;
            
            // For behavior.
            self.isActive = ko.observable(false);           // If TRUE - show edit fields.
            self.isExpanded = ko.observable(false);         // If TRUE - show child nodes.
            self.toggleExpand = function () {
                self.isExpanded(!self.isExpanded());
            };
            self.isBeingDragged = ko.observable(false);     // Someone is dragging the node.
            self.activeZone = ko.observable(DropZoneTypes.None);
            self.isFirstNode = ko.observable(false);
            self.hasChildNodes = function () {
                return self.childNodes().length > 0;
            };
            self.containerId = function () {
                // User for validation.
                return "id-" + self.id();
            };
            
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
                    self.parentNode.childNodes.remove(self);
                }
            };
            self.saveSitemapNodeWithValidation = function () {
                if ($('input', '#' + self.containerId()).valid()) {
                    self.saveSitemapNode();
                    self.isActive(false);
                }
            };
            self.saveSitemapNode = function (onSuccessCallBack, onFailCallBack) {
                var params = self.toJson(),
                    onSaveCompleted = function (json) {
                        // TODO: messages.refreshBox(messagesContainer, json);
                        if (json.Success) {
                            if (json.Data) {
                                self.id(json.Data.Id);
                                self.version(json.Data.Version);
                            }
                            if (onSuccessCallBack && $.isFunction(onSuccessCallBack)) {
                                onSuccessCallBack();
                            }
                        } else {
                            if (onFailCallBack && $.isFunction(onFailCallBack)) {
                                onFailCallBack();
                            }
                        }
                        // TODO: unlock screen.
                    };
                // TODO: lock screen.
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
                var params = self.toJson(),
                    onDeleteCompleted = function (json) {
                        // TODO: messages.refreshBox(messagesContainer, json);
                        if (json.Success) {
                            self.parentNode.childNodes.remove(self);
                        }
                        // TODO: unlock screen.
                    };
                // TODO: lock screen.
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
                    .fail(function (response) {
                        onDeleteCompleted(bcms.parseFailedResponse(response));
                    });
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
                        node.parentNode = self;
                        node.isFirstNode(i == 0);
                        nodes.push(node);
                    }
                }
                self.childNodes(nodes);
            };
            self.toJson = function () {
                var params = {
                    Id: self.id(),
                    Version: self.version(),
                    Title: self.title(),
                    Url: self.url(),
                    DisplayOrder: self.displayOrder(),
                    ParentId: self.parentNode.id(),
                };
                return params;
            };
        }

        // Responsible for page searching.
        function SearchPageLinksViewModel() {
            var self = this;
            self.searchQuery = ko.observable();
            self.pageLinks = ko.observableArray([]);
            self.sitemap = null;

            self.searchForPageLinks = function () {
                var showAll = $.trim(self.searchQuery()).length === 0;
                var pageLinks = self.pageLinks();
                for (var i in pageLinks) {
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
            };
        }
        
        // Responsible for page link data.
        function PageLinkViewModel(title, url) {
            var self = this;
            self.title = ko.observable(title);
            self.url = ko.observable(url);
            self.isVisible = ko.observable(true);
        }
        // --------------------------------------------------------------------








// SITE MAP REFACTORING!
        
/*
        sitemap.showSitemapEditDialog = function() {
            modal.open({
                title: globalization.sitemapEditorDialogTitle,
                cancelTitle: globalization.sitemapEditorDialogClose,
                disableAccept: true,
                onLoad: function(dialog) {
                    dynamicContent.setContentFromUrl(dialog, links.sitemapEditDialogUrl, {
                        done: function (content) {
                            initializeSitemapEditDialog(content, dialog);
                        },
                    });
                },
                onAcceptClick: function (dialog) {
                    dialog.close();
                    // TODO: reload sitemap.
                }
            });
        };
        function searchSitemapNodes(form) {
            $.ajax({
                type: 'POST',
                cache: false,
                url: form.attr('action'),
                data: form.serialize(),

                success: function (data) {
                    siteSettings.setContent(data.Data.Html);
                    initializeSiteSettingsSitemap(data);
                }
            });

        }
        function attachEvents(container) {
            container.find(selectors.sitemapEditButton).on('click', function () {
                sitemap.showSitemapEditDialog();
            });

            var form = container.find(selectors.sitemapForm);
            
            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSitemapNodes(form);
                return false;
            });
            
            form.find(selectors.sitemapNodeSearchButton).on('click', function () {
                searchSitemapNodes(form);
            });
        }
        function initializeSiteMap(json, sitemapViewModel) {
            var context = sitemapViewModel.container.find(selectors.templateDataBind).get(0);

            if (parseJsonResults(json, sitemapViewModel)) {
                ko.applyBindings(sitemapViewModel, context);
                
                var form = sitemapViewModel.container.find(selectors.sitemapForm);
                if ($.validator && $.validator.unobtrusive) {
                    form.removeData("validator");
                    form.removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                }
                
                return true;
            }
            
            return false;
        }
        function initializeSiteSettingsSitemap(content) {
            var dialogContainer = siteSettings.getModalDialog().container;
            
            messagesContainer = dialogContainer; // TODO: move to private.
            
            var sitemapViewModel = new SitemapViewModel(dialogContainer);
            if (initializeSiteMap(content, sitemapViewModel)) {
                attachEvents(dialogContainer);
            };
        }
        function initializeSitemapEditDialog(content, dialog) {
            var sitemapViewModel = new SitemapViewModel(dialog.container);
            initializeSiteMap(content, sitemapViewModel);
            initializePageLinks(content, dialog.container);
        }
        function initializePageLinks(content, container) {
            var pageLinks = new PageLinksViewModel(container);
            if (jsonToPageLinksModel(content, pageLinks)) {
                var context = container.find(selectors.templatePageLinksDataBind).get(0);
                ko.applyBindings(pageLinks, context);
            }
        }
        function jsonToPageLinksModel(json, pageLinksViewModel) {
            messages.refreshBox(pageLinksViewModel.messagesContainer, json);
            if (json.Success) {
                var pageLinks = [];
                for (var i in json.Data.PageLinks) {
                    var pageLink = json.Data.PageLinks[i];
                    pageLinks.push(new PageLinkViewModel(pageLink.Title, pageLink.Url));
                }
                pageLinksViewModel.pageLinks(pageLinks);
                return true;
            }
            return false;
        }
        function PageLinksViewModel(container) {
            var self = this;
            self.messagesContainer = container;
            
            self.searchText = ko.observable();
            self.pageLinks = ko.observableArray([]);

            self.searchPageLinks = function() {
                // TODO: implement.
            };
        }
        function PageLinkViewModel(title, url) {
            var self = this;
            self.title = ko.observable(title);
            self.url = ko.observable(url);
        }
        function addSortableBinding() {
            ko.bindingHandlers.sortable = {
                init: function(element, valueAccessor) {
                    var startIndex = -1,
                        sourceArray = valueAccessor(),
                        sortableSetup = {
                            start: function(event, ui) {
                                ko.contextFor(ui.item[0]).$root.isDragStarted(true);
                                startIndex = ui.item.index();
                                ui.item.find("input:focus").change();
                            },
                            stop: function(event, ui) {
                                ko.contextFor(ui.item[0]).$root.isDragStarted(false);
                                var newIndex = ui.item.index();
                                if (startIndex > -1) {
                                    var context = ko.contextFor(ui.item.parent()[0]),
                                        destinationParent = context.$data,
                                        destinationArray = destinationParent.childNodes || destinationParent.sitemapNodes,
                                        item = sourceArray()[startIndex];

                                    sourceArray.remove(item);
                                    destinationArray.splice(newIndex, 0, item);
                                    ui.item.remove();

                                    context.$root.updateNodesOrderAndParent();
                                }
                            },
                            connectWith: selectors.sitemapChildNodesList,
                            placeholder: cssclass.sitemapNodeDropZone,
                            dropOnEmpty: true
                        };
                    $(element).sortable(sortableSetup);
                    $(element).disableSelection();
                }
            };
        }
        function addDraggableBinding() {
            ko.bindingHandlers.draggable = {
                init: function (element, valueAccessor) {
                    var startIndex = -1,
                        sourceArray = valueAccessor(),
                        draggableSetup = {
                            placeholder: cssclass.sitemapNodeDropZone,
                            connectWith: selectors.sitemapChildNodesList,
                            start: function(event, ui) {
                                startIndex = ui.item.index();
                            },
                            stop: function(event, ui) {
                                if (startIndex > -1) {
                                    var newIndex = ui.item.index(),
                                        context = ko.contextFor(ui.item.parent()[0]),
                                        destinationParent = context.$data,
                                        destinationArray = destinationParent.childNodes || destinationParent.sitemapNodes,
                                        pageLink = sourceArray()[startIndex];

                                    // Create new sitemap node.
                                    var sitemapNode = new SitemapNodeViewModel();
                                    sitemapNode.id('00000000-0000-0000-0000-000000000000');
                                    sitemapNode.version(0);
                                    sitemapNode.title(pageLink.title());
                                    sitemapNode.url(pageLink.url());
                                    sitemapNode.displayOrder(newIndex);
                                    sitemapNode.parentNode = destinationParent != context.$root ? destinationParent : null;
                                    

                                    // TODO: add spinner in the node.
                                    saveSitemapNode(sitemapNode, function() {
                                        destinationArray.splice(newIndex, 0, sitemapNode);
                                        context.$root.updateNodesOrderAndParent();
                                        // TODO: remove spinner.
                                    }, function () {
                                        // TODO: remove spinner.
                                    });
                                }
                                return false;
                            }
                        };
                    $(element).sortable(draggableSetup);
                    $(element).disableSelection();
                }
            };
        }
*/
// TODO: Remove or update for better node dragging appearance.
//        /**
//        * Hover binding to handle node adding to node without child nodes.
//        */
//        function addHoverCssBinding() {
//            ko.bindingHandlers.hovercss = {
//                update: function(element, valueAccessor) {
//                    var css = valueAccessor();
//
//                    ko.utils.registerEventHandler(element, "mouseover", function () {
//                        ko.utils.toggleDomNodeCssClass(element, ko.utils.unwrapObservable(css), true);
//                    });
//                    
//                    ko.utils.registerEventHandler(element, "mouseout", function () {
//                        ko.utils.toggleDomNodeCssClass(element, ko.utils.unwrapObservable(css), false);
//                    });
//                }
//            };
//        }

        /**
        * Initializes module.
        */
        sitemap.init = function() {
            console.log('Initializing bcms.sitemap module.');
        };
    
        /**
        * Register initialization.
        */
        bcms.registerInit(sitemap.init);
    
        return sitemap;
});
