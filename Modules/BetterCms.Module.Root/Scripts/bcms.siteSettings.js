/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.siteSettings.js" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
            loaderContainer: '.bcms-js-settings-window',
            mainContainer: '.bcms-js-settings-window',
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
                        $('bcms-btn-search').on('click', function() {
                            $(this).parent().addClass("bcms-active-search");
                        });
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

            //siteSettingsModalWindow.maximizeHeight();

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

        ko.cleanNode(siteSettingsModalWindow.container.find(selectors.placeHolder).get(0));
        ko.applyBindings(tabsViewModel, siteSettingsModalWindow.container.find(selectors.placeHolder).get(0));

        if (tabViewModels.length > 0) {
            tabViewModels[0].tabClick();
        }
    };

    return siteSettings;
});
