/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.sitemap', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent) {
    'use strict';

        var sitemap = {},
            
        selectors = {
        },
        
        links = {
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
