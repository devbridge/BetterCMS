/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.sitemap', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'knockout'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, ko) {
        'use strict';

        var sitemap = {},
            selectors = {
                templateDataBind: ".bcms-sitemap-data-bind-container",
            },
            links = {
                loadSiteSettingsSitemapUrl: null,
            },
            globalization = {};

        /**
        * Assign objects to module.
        */
        sitemap.links = links;
        sitemap.globalization = globalization;

        /**
        * Loads a media manager view to the site settings container.
        */
        sitemap.loadSiteSettingsSitmap = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsSitemapUrl, {
                contentAvailable: initializeSiteSettingsSitemap
            });
        };

        function SitemapViewModel(container) {
            var self = this;

            self.container = container;
            self.messagesContainer = container;

            self.sitemapNodes = ko.observableArray([]);
        
            self.expandAll = function() {
                alert("Expand all clicked!"); // TODO: implement.
            };
            
            self.collapseAll = function () {
                alert("Expand all clicked!"); // TODO: implement.
            };
        };

        function SitemapNodeViewModel() {
            var self = this;
            
            self.id = ko.observable();
            self.version = ko.observable();
            self.title = ko.observable();
            self.url = ko.observable();
            self.displayOrder = ko.observable(0);
            self.childNodes = ko.observableArray([]);
            self.parentNode = null;
            
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

            self.editSitemapNode = function (parent, event) {
                bcms.stopEventPropagation(event);
                editSitemapNode(self);
            };
            
            self.deleteSitemapNode = function (parent, data, event) {
                bcms.stopEventPropagation(event);
                deleteSitemapNode(self);
            };
            
            self.saveSitemapNode = function (parent, data, event) {
                bcms.stopEventPropagation(event);
                saveSitemapNode(self);
            };
            
            self.cancelEditSitemapNode = function (parent, data, event) {
                bcms.stopEventPropagation(event);
                cancelEditSitemapNode(self);
            };
        };

        /**
        * Saves sitemap node after inline edit.
        */
        function saveSitemapNode(sitemapNodeViewModel) {
            // TODO: implement.
            alert('Save node "' + sitemapNodeViewModel.title() + '"!');
        };
        
        function editSitemapNode(sitemapNodeViewModel) {
            // TODO: implement.
            alert('Edit node "' + sitemapNodeViewModel.title() + '"!');
        };

        function deleteSitemapNode(sitemapNodeViewModel) {
            // TODO: implement.
            alert('Delete node "' + sitemapNodeViewModel.title() + '"!');
        };

        /**
        * Cancels inline editing of sitemap node.
        */
        function cancelEditSitemapNode(sitemapNodeViewModel) {
            // TODO: implement.
            alert('Cancel node edit "' + sitemapNodeViewModel.title() + '"!');
        };

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
            messages.refreshBox(sitemapViewModel.messagesContainer, json);
            
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
        * Attach links to actions.
        */
        function attachEvents(container) {
            var form = container.find(selectors.firstForm);
            if ($.validator && $.validator.unobtrusive) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }
        }

        /**
        * Initializes tab, when data is loaded
        */
        function initializeSiteMap(json, sitemapViewModel) {
            var context = sitemapViewModel.container.find(selectors.templateDataBind).get(0);

            if (parseJsonResults(json, sitemapViewModel)) {
                ko.applyBindings(sitemapViewModel, context);

                attachEvents(sitemapViewModel.container);
            }
        }

        /**
        * Initializes sitemap in site settings.
        */
        function initializeSiteSettingsSitemap(content) {
            var dialogContainer = siteSettings.getModalDialog().container;

            var sitemapViewModel = new SitemapViewModel(dialogContainer);
            
            initializeSiteMap(content.Data.Data, sitemapViewModel);
        }

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
