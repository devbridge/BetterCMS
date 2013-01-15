/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.siteSettings', ['jquery', 'bcms', 'bcms.modal', 'bcms.dynamicContent', 'bcms.tabs', 'knockout'],
    function ($, bcms, modal, dynamicContent, tabs, ko) {
    'use strict';

    var siteSettings = {},

    // Selectors used in the module to locate DOM elements
        selectors = {
            container: '#bcms-site-settings',
            menu: '#bcms-site-settings-menu',
            placeHolder: '#bcms-site-settings-placeholder',
            firstMenuButton: '#bcms-site-settings-menu .bcms-onclick-action:first',
            loaderContainer: '.bcms-rightcol',
            tabsTemplate: '#bcms-site-setting-tab-template'
        },

        links = {
            loadSiteSettingsUrl: null
        },

        globalization = {
            siteSettingsTitle: null
        },

        siteSettingsModalWindow = null;

    /**
    * Assign objects to module
    */
    siteSettings.selectors = selectors;
    siteSettings.links = links;
    siteSettings.globalization = globalization;
    siteSettings.siteSettingsModalWindow = siteSettingsModalWindow;
    siteSettings.contentId = 0;

    /**
    * Opens site settings container, loads menu items and selects first one.
    */
    siteSettings.openSiteSettings = function () {
        siteSettingsModalWindow = modal.open({
            title: siteSettings.globalization.siteSettingsTitle,
            disableAccept: true,
            onLoad: function (dialog) {
                dynamicContent.setContentFromUrl(dialog, siteSettings.links.loadSiteSettingsUrl, {
                    done: function () {
                        siteSettings.selectFirstMenuItem();
                    }
                });
            },
            onClose: function () {
                siteSettingsModalWindow = null;
            }
        });
    };

    /**
    * Sets site settings content.
    */
    siteSettings.setContent = function (content, contentId) {
        if (siteSettingsModalWindow && (!contentId || contentId == siteSettings.contentId)) {
            siteSettingsModalWindow.container.find(selectors.placeHolder).empty().append(content);

            if ($.validator && $.validator.unobtrusive) {
                $.validator.unobtrusive.parse(siteSettingsModalWindow.container);
            }

            siteSettingsModalWindow.maximizeHeight();
            
            tabs.initTabPanel(siteSettingsModalWindow.container);
        }
    };

    /**
    * Disables accept action on site settings modal window.
    */
    siteSettings.disableAccept = function () {
        if (siteSettingsModalWindow) {
            siteSettingsModalWindow.disableAccept();
        }
    };

    /**
    * Selects first menu item.
    */
    siteSettings.selectFirstMenuItem = function () {
        $(selectors.firstMenuButton).trigger("click");
    };

    /**
    * Returns site settings modal window object.
    */
    siteSettings.getModalDialog = function() {
        return siteSettingsModalWindow;
    };
    
    /**
    * Returns site settings modal window container.
    */
    siteSettings.getLoaderContainer = function () {
        if (siteSettingsModalWindow) {
            return siteSettingsModalWindow.container.find(selectors.loaderContainer);
        }
        return null;
    };

    /**
    * Site settings tab list view model
    */
    siteSettings.TabListViewModel = function (tabViewModels) {
        var self = this;

        self.tabs = [];
        
        for (var i = 0; i < tabViewModels.length; i++) {
            var tab = tabViewModels[i];
            tab.id = i + 1;
            
            self.tabs.push(tab);
        }
    };

    /**
    * Site settings tab view model
    */
    siteSettings.TabViewModel = function (title, url, onContentAvailable) {
        var self = this;

        self.title = title;
        self.id = null;
        self.url = url;
        self.onContentAvailable = onContentAvailable || function (tabContainer) { };
        self.isActive = ko.observable(false);
        self.isInitialized = false;
        self.parent = null;
        self.container = null;

        self.href = function() {
            return '#' + self.tabId();
        };
        
        self.tabId = function () {
            return 'bcms-tab-' + self.id;
        };

        self.load = function () {
            if (!self.isInitialized) {
                self.isInitialized = true;

                dynamicContent.setContentFromUrl(self, self.url);
            }
            self.isActive(true);
        };

        self.getLoaderContainer = function () {
            if (self.container == null) {
                self.container = siteSettingsModalWindow.container.find(self.href());
            }
            return self.container;
        };

        self.setContent = function(content) {
            self.container.html(content);
        };
    };

    /**
    * Inits site settings tabs
    */
    siteSettings.initContentTabs = function (tabViewModels) {
        siteSettings.contentId++;

        var tabsViewModel = new siteSettings.TabListViewModel(tabViewModels),
            content = $($(selectors.tabsTemplate).html());

        siteSettings.setContent(content);
        
        ko.applyBindings(tabsViewModel, siteSettingsModalWindow.container.get(0));

        tabs.initTabPanel(siteSettingsModalWindow.container);
        
        if (tabViewModels.length > 0) {
            tabViewModels[0].load();
        }
    };

    return siteSettings;
});
