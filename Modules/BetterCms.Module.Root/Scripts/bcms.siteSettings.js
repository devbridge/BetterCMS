/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.siteSettings', ['jquery', 'bcms', 'bcms.modal', 'bcms.dynamicContent', 'bcms.tabs'], function ($, bcms, modal, dynamicContent, tabs) {
    'use strict';

    var siteSettings = {},

    // Selectors used in the module to locate DOM elements
        selectors = {
            container: '#bcms-site-settings',
            menu: '#bcms-site-settings-menu',
            placeHolder: '#bcms-site-settings-placeholder',
            firstMenuButton: '#bcms-site-settings-menu .bcms-onclick-action:first',
            loaderContainer: '.bcms-rightcol'
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
    siteSettings.setContent = function (content) {
        if (siteSettingsModalWindow) {
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

    return siteSettings;
});
