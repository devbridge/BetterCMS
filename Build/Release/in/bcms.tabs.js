/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.tabs', ['bcms.jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var tabs = {},

    // Classes that are used to maintain various UI states:
        classes = {
            activeTabLink: 'bcms-tab-active'
        },

    // Selectors used in the module to locate DOM elements:
        selectors = {
            tabContent: '.bcms-tab-single',
            firstTabContent: '.bcms-tab-single:first',
            tabLink: 'a.bcms-tab',
            firstTabLink: 'a.bcms-tab:first',
            activeTabLink: '.bcms-tab-active',
            tabsHeader: '.bcms-tab-header'
        },

        links = {},

        globalization = {
        };

    /**
    /* Assign objects to module.
    */
    tabs.selectors = selectors;
    tabs.links = links;
    tabs.globalization = globalization;

    /**
    * TabsPanel instance constructor:
    */

    function TabPanel(container, options) {
        options = $.extend({
        }, options);

        this.container = container;
        this.options = options;
    }

    TabPanel.prototype = {
        init: function () {
            var instance = this,
                container = this.container;

            container.find(selectors.tabLink).on('click', function () {
                var tabId = $(this).attr('href');
                instance.selectTab(tabId);
                return false;
            });

            this.selectFirstTab();
        },

        selectTab: function (tabId) {
            if (!tabId) {
                return;
            }
            
            if (tabId.indexOf('#') !== 0) {
                tabId = '#' + tabId;
            }

            this.container.find(selectors.activeTabLink).removeClass(classes.activeTabLink);
            this.container.find('[href="' + tabId + '"]').addClass(classes.activeTabLink);

            this.container.find(selectors.tabContent).hide();
            this.container.find(tabId).show();
        },

        selectFirstTab: function () {
            var tabId = this.container.find(selectors.firstTabLink).attr('href');
            this.selectTab(tabId);
        },

        selectTabOfElement: function (element) {
            var tabId = $(element).parents(selectors.tabContent).attr('id');
            this.selectTab(tabId);
        }
    };

    tabs.initTabPanel = function (container, options) {
        var tabPanel = new TabPanel(container, options);
        tabPanel.init();
        return tabPanel;
    };

    tabs.getTabPanelOfElement = function (element) {
        var tab = $(element).parents(selectors.firstTabContent);

        if (tab.length > 0) {
            var parent = tab.parent();
            var tabsHeader;

            do {
                tabsHeader = parent.find(selectors.tabsHeader);
                parent = parent.parent();
            } while (parent.length !== 0 && tabsHeader.length === 0);

            if (tabsHeader.length !== 0) {
                var tabPanel = new TabPanel(tabsHeader.parent());
                return tabPanel;
            }
        }

        return null;
    };

    return tabs;
});
