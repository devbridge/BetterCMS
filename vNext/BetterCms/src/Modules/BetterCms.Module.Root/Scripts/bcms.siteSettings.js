/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.siteSettings', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.dynamicContent', 'bcms.tabs', 'bcms.ko.extenders', 'bcms.messages', 'bcms.forms'],
    function ($, bcms, modal, dynamicContent, tabs, ko, messages, forms) {
    'use strict';

    var siteSettings = {},

    // Selectors used in the module to locate DOM elements
        selectors = {
            container: '#bcms-site-settings',
            menu: '#bcms-site-settings-menu',
            placeHolder: '#bcms-site-settings-placeholder',
            firstMenuButton: '#bcms-site-settings-menu .bcms-onclick-action:first',
            loaderContainer: '.bcms-rightcol',
            mainContainer: '.bcms-rightcol',
            tabsTemplate: '#bcms-site-setting-tab-template',
            tabsTemplateChildDiv: 'div',
            modalMessages: '#bcms-modal-messages'
        },

        links = {
            loadSiteSettingsUrl: null
        },

        globalization = {
            siteSettingsTitle: null,
            closeButtonTitle: null
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
            cancelTitle: globalization.closeButtonTitle,
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
    siteSettings.setContent = function (content, contentId, doNotInitTabs) {
        if (siteSettingsModalWindow && (!contentId || contentId == siteSettings.contentId)) {
            messages.refreshBox(siteSettingsModalWindow.container.find(selectors.modalMessages), {});

            siteSettingsModalWindow.container.find(selectors.placeHolder).empty().append(content);

            if ($.validator && $.validator.unobtrusive) {
                $.validator.unobtrusive.parse(siteSettingsModalWindow.container);
            }

            siteSettingsModalWindow.maximizeHeight();

            if (!doNotInitTabs) {
                tabs.initTabPanel(siteSettingsModalWindow.container);
            }

            forms.bindCheckboxes(siteSettingsModalWindow.container);
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
    * Returns site settings modal window object.
    */
    siteSettings.getMainContainer = function () {
        return siteSettingsModalWindow.container.find(selectors.mainContainer);
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
    siteSettings.TabListViewModel = function (tabViewModels, tabsPanel) {
        var self = this;

        self.tabs = [];
        
        for (var i = 0; i < tabViewModels.length; i++) {
            var tab = tabViewModels[i];
            tab.tabId = 'bcms-tab-' + (i+1);
            tab.href = '#' + tab.tabId;
            tab.parent = tabsPanel;
            
            self.tabs.push(tab);
        }
    };

    /**
    * Site settings tab view model
    */
    siteSettings.TabViewModel = function (title, url, onContentAvailable, onShow) {
        var self = this;

        self.title = title;
        self.url = url;
        self.isInitialized = false;
        self.container = false;
        self.spinContainer = false;
        self.parent = null;
        self.tabId = null;
        self.href = null;
        self.contentId = null;

        self.onContentAvailable = function (content) {
            if (self.contentId == siteSettings.contentId) {
                self.isInitialized = true;

                if (onContentAvailable && $.isFunction(onContentAvailable)) {
                    onContentAvailable(self.container, content);
                }

                self.onContentShow();
            }
        };

        self.tabClick = function () {
            self.parent.selectTab(self.tabId);
            messages.refreshBox(siteSettingsModalWindow.container.find(selectors.modalMessages), {});

            if (!self.isInitialized) {
                self.spinContainer = siteSettingsModalWindow.container.find(selectors.loaderContainer);
                self.container = siteSettingsModalWindow.container.find(self.href).find(selectors.tabsTemplateChildDiv);
                self.contentId = siteSettings.contentId;

                dynamicContent.setContentFromUrl(self, self.url, {
                    done: self.onContentAvailable
                });
            } else {
                self.onContentShow();
            }

            return true;
        };

        self.getLoaderContainer = function () {
            return self.spinContainer;
        };

        self.setContent = function (content) {
            if (self.contentId == siteSettings.contentId) {
                self.container.html(content);
            }
        };

        self.onContentShow = function() {
            if ($.isFunction(onShow)) {
                onShow(self.container);
            }
        };
    };

    /**
    * Inits site settings tabs
    */
    siteSettings.initContentTabs = function (tabViewModels) {
        siteSettings.contentId++;

        var panel = tabs.initTabPanel(siteSettingsModalWindow.container, {
            skipInit: true
        }),
            tabsViewModel = new siteSettings.TabListViewModel(tabViewModels, panel),
            content = $($(selectors.tabsTemplate).html());

        siteSettings.setContent(content, null, true);
        
        ko.applyBindings(tabsViewModel, siteSettingsModalWindow.container.find(selectors.placeHolder).get(0));

        if (tabViewModels.length > 0) {
            tabViewModels[0].tabClick();
        }
    };

    return siteSettings;
});
