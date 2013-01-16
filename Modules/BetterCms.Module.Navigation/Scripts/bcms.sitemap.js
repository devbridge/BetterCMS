/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.sitemap', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'knockout'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, ko) {
        'use strict';

        var sitemap = {},
            selectors = {
                templateDataBind: ".bcms-sitemap-data-bind-container",
                sitemapForm: "#bcms-sitemap-form",
                sitemapNodeSearchButton: "#bcms-btn-sitemap-search",
                sitemapEditButton: "#bcms-btn-sitemap-edit",
                
                templatePageLinksDataBind: "#bcms-pagelinks-data-bind-container",
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
            messagesContainer = null;

        /**
        * Assign objects to module.
        */
        sitemap.links = links;
        sitemap.globalization = globalization;

        /**
        * Loads a sitemap view to the site settings container.
        */
        sitemap.loadSiteSettingsSitmap = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsSitemapUrl, {
                contentAvailable: initializeSiteSettingsSitemap
            });
        };

        /**
        * Opens sitemap edit dialog.
        */
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
                }
            });
        };


        /**
        * Sitemap view model.
        */
        function SitemapViewModel(container) {
            var self = this;
            self.isRootModel = true;

            self.container = container;

            self.sitemapNodes = ko.observableArray([]);
            self.isDragStarted = ko.observable(false);
        
            self.expandAll = function () {
                var nodes = self.sitemapNodes();
                for (var i in nodes) {
                    nodes[i].expandAll();
                }
            };
            
            self.collapseAll = function () {
                var nodes = self.sitemapNodes();
                for (var i in nodes) {
                    nodes[i].collapseAll();
                }
            };
        };

        /**
        * Sitemap node view model.
        */
        function SitemapNodeViewModel() {
            var self = this;
            self.isRootModel = false;

            self.id = ko.observable();
            self.version = ko.observable();
            self.title = ko.observable();
            self.url = ko.observable();
            self.displayOrder = ko.observable(0);
            self.childNodes = ko.observableArray([]);
            self.parentNode = null;

            self.containerId = function() {
                return "id-" + self.id();
            };

            self.isEditable = ko.observable(true);
            self.isActive = ko.observable(false);
            self.isExpanded = ko.observable(false);

            self.hasChildNodes = function () {
                return self.childNodes().length > 0;
            };
            
            self.expandChildNodes = function () {
                self.isExpanded(true);
            };
            self.collapseChildNodes = function () {
                self.isExpanded(false);
            };
            self.expandOrCollapseChildNodes = function () {
                self.isExpanded(!self.isExpanded());
            };
            self.expandAll = function () {
                self.isExpanded(true);
                var nodes = self.childNodes();
                for (var i in nodes) {
                    nodes[i].expandAll();
                }
            };
            self.collapseAll = function () {
                self.isExpanded(false);
                var nodes = self.childNodes();
                for (var i in nodes) {
                    nodes[i].collapseAll();
                }
            };

            self.editSitemapNode = function (parent, event) {
                bcms.stopEventPropagation(event);
                self.isActive(true);
            };
            
            self.deleteSitemapNode = function (parent, data, event) {
                bcms.stopEventPropagation(event);
                deleteSitemapNode(self, parent);
            };
            
            self.saveSitemapNode = function (parent, data, event) {
                bcms.stopEventPropagation(event);
                if ($('input', '#' + self.containerId()).valid()) {
                    saveSitemapNode(self);
                    self.isActive(false);
                }
            };
            
            self.cancelEditSitemapNode = function (parent, data, event) {
                bcms.stopEventPropagation(event);
                self.isActive(false);
            };
            
            self.toJson = function () {
                var params = {
                    Id: self.id(),
                    Version: self.version(),
                    Title: self.title(),
                    Url: self.url(),
                    DisplayOrder: self.displayOrder(),
                    ParentId: self.parentNode != null ? self.parentNode.id() : '00000000-0000-0000-0000-000000000000',
                };
                return params;
            };
        };

        /**
        * Saves sitemap node.
        */
        function saveSitemapNode(sitemapNodeViewModel) {
            var params = sitemapNodeViewModel.toJson(),
                onSaveCompleted = function(json) {
                    messages.refreshBox(messagesContainer, json);
                    if (json.Success) {
                        if (json.Data) {
                            sitemapNodeViewModel.id(json.Data.Id);
                            sitemapNodeViewModel.version(json.Data.Version);
                        }
                    }
                };
            
            $.ajax({
                url: links.saveSitemapNodeUrl,
                type: 'POST',
                dataType: 'json',
                cache: false,
                data: params
            })
                .done(function(json) {
                    onSaveCompleted(json);
                })
                .fail(function(response) {
                    onSaveCompleted(bcms.parseFailedResponse(response));
                });
        };
        
        /**
        * Delete sitemap node.
        */
        function deleteSitemapNode(sitemapNodeViewModel, parent) {
            var params = sitemapNodeViewModel.toJson(),
                onDeleteCompleted = function (json) {
                    messages.refreshBox(messagesContainer, json);
                    if (json.Success) {
                        var childNodes = parent.childNodes || parent.sitemapNodes;
                        childNodes.remove(sitemapNodeViewModel);
                    }
                };

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

        /**
        * Make site map node view model.
        */
        function makeSitemapNodeViewModel(jsonSitemapNode) {
            var sitemapNodeViewModel = new SitemapNodeViewModel();

            sitemapNodeViewModel.id(jsonSitemapNode.Id);
            sitemapNodeViewModel.version(jsonSitemapNode.Version);
            sitemapNodeViewModel.title(jsonSitemapNode.Title);
            sitemapNodeViewModel.url(jsonSitemapNode.Url);
            sitemapNodeViewModel.displayOrder(jsonSitemapNode.DisplayOrder);

            var childNodes = [];
            if (jsonSitemapNode.ChildNodes != null) {
                for (var i = 0; i < jsonSitemapNode.ChildNodes.length; i++) {
                    var node = makeSitemapNodeViewModel(jsonSitemapNode.ChildNodes[i]);
                    node.parentNode = sitemapNodeViewModel;
                    childNodes.push(node);
                }
            }
            sitemapNodeViewModel.childNodes(childNodes);

            return sitemapNodeViewModel;
        }

        /**
        * Parse json result and map data to view model.
        */
        function parseJsonResults(json, sitemapViewModel) {
            messages.refreshBox(messagesContainer, json);
            
            if (json.Success) {
                var nodes = [];
                for (var i = 0; i < json.Data.RootNodes.length; i++) {
                    var node = makeSitemapNodeViewModel(json.Data.RootNodes[i]);
                    nodes.push(node);
                }
                sitemapViewModel.sitemapNodes(nodes);
                return true;
            }
            return false;
        }

        /**
        * Search sitemap node.
        */
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

        /**
        * Attach events.
        */
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

        /**
        * Initializes tab, when data is loaded.
        */
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

        /**
        * Initializes sitemap in site settings.
        */
        function initializeSiteSettingsSitemap(content) {
            var dialogContainer = siteSettings.getModalDialog().container;
            
            messagesContainer = dialogContainer; // TODO: move to private.
            
            var sitemapViewModel = new SitemapViewModel(dialogContainer);
            if (initializeSiteMap(content, sitemapViewModel)) {
                attachEvents(dialogContainer);
            };
        }
        
        /**
        * Initializes sitemap edit dialog.
        */
        function initializeSitemapEditDialog(content, dialog) {
            var sitemapViewModel = new SitemapViewModel(dialog.container);
            initializeSiteMap(content, sitemapViewModel);
        }
        
        /**
        * Sortable binding to handle node ordering.
        */
        function addSortableBinding() {
            ko.bindingHandlers.sortable = {
                init: function(element, valueAccessor) {
                    var startIndex = -1,
                        sourceArray = valueAccessor();
                    var sortableSetup = {
                        start: function(event, ui) {
                            ko.contextFor(ui.item[0]).$root.isDragStarted(true);
                            startIndex = ui.item.index();
                            ui.item.find("input:focus").change();
                        },
                        stop: function(event, ui) {
                            ko.contextFor(ui.item[0]).$root.isDragStarted(false);
                            var newIndex = ui.item.index(),
                                renewNodeData = function (nodesToReorder, parentNode) {
                                    for (var i = 0; i < nodesToReorder.length; i++) {
                                        var node = nodesToReorder[i],
                                            nodeUpdated = false;
                                        // Update order index.
                                        if (node.displayOrder() != i) {
                                            node.displayOrder(i);
                                            nodeUpdated = true;
                                        }
                                        // Update parent node.
                                        if (parentNode != null && node.parentNode != parentNode) {
                                            if (parentNode.isRootModel) {
                                                node.parentNode = null;
                                            } else {
                                                node.parentNode = parentNode;
                                            }
                                            nodeUpdated = true;
                                        }
                                        // Save changes if any.
                                        if (nodeUpdated) {
                                            saveSitemapNode(node);
                                        }
                                    }
                                };

                            if (startIndex > -1) {
                                var context = ko.contextFor(ui.item.parent()[0]),
                                    destinationParent = context.$data,
                                    destinationArray = destinationParent.childNodes || destinationParent.sitemapNodes,
                                    item = sourceArray()[startIndex];

                                sourceArray.remove(item);
                                destinationArray.splice(newIndex, 0, item);
                                ui.item.remove();

                                if (destinationArray != sourceArray) {
                                    renewNodeData(sourceArray(), null);
                                }
                                renewNodeData(destinationArray(), destinationParent);
                            }
                        },
                        connectWith: '.bcms-connected-sortable',
                        placeholder: 'bcms-placement-dropzone',
                        dropOnEmpty: true
                    };
                    $(element).sortable(sortableSetup);
                    $(element).disableSelection();
                }
            };
        }

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
            addSortableBinding();
        };
    
        /**
        * Register initialization.
        */
        bcms.registerInit(sitemap.init);
    
        return sitemap;
});
