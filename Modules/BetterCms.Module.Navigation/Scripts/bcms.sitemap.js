/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.sitemap', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent) {
    'use strict';

        var sitemap = {},
            
        selectors = {
        },
        
        links = {
            loadSiteSettingsSitmapUrl: null,
        },
        
        globalization = {
        },
        
        keys = {
        },
        
        classes = {
        };

    /**
    * Assign objects to module.
    */
    sitemap.links = links;
    sitemap.globalization = globalization;

    /**
    * Loads a media manager view to the site settings container.
    */
    sitemap.loadSiteSettingsSitmap = function () {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsSitmapUrl, {
            contentAvailable: initializeSiteSettingsSitemap
        });
    };

    /**
    * Initializes media manager.
    */
    function initializeSiteSettingsSitemap(content) {
        // TODO: implement.
        alert("Implement sitemap!");
    };

    /**
    * Initializes page module.
    */
    sitemap.init = function () {
        console.log('Initializing bcms.sitemap module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(sitemap.init);
    
    return sitemap;
});
