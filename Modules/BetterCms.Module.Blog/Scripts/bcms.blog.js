/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.blog', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent'],
    function ($, bcms, modal, siteSettings, dynamicContent) {
    'use strict';

    var blog = { },
        selectors = { },
        links = {
            loadSiteSettingsBlogsUrl: null
        },
        globalization = { };

    // Assign objects to module.
    blog.links = links;
    blog.globalization = globalization;
    blog.selectors = selectors;

    function initializeSiteSettingsBlogs(content) {
    }

    /**
    * Loads a media manager view to the site settings container.
    */
    blog.loadSiteSettingsBlogs = function () {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsBlogsUrl, {
            contentAvailable: initializeSiteSettingsBlogs
        });
    };

    /**
    * Posts new blog article
    */
    blog.postNewArticle = function () {
    };

    /**
    * Initializes blog module.
    */
    blog.initActions = function () {
    };

    /**
    * Initializes blog module.
    */
    blog.init = function () {
        console.log('Initializing blog module');
        blog.initActions();
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(blog.init);
    
    return blog;
});
