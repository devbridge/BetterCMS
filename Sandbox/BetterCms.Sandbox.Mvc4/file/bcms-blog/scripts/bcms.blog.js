/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.blog', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.datepicker', 'bcms.htmlEditor'],
    function ($, bcms, modal, siteSettings, dynamicContent, datepicker, htmlEditor) {
    'use strict';

    var blog = { },
        selectors = {
            datePickers: '.bcms-datepicker',
            htmlEditor: 'bcms-contenthtml'
        },
        links = {
            loadSiteSettingsBlogsUrl: null,
            loadCreateNewPostDialogUrl: null,
            loadEditPostDialogUrl: null
        },
        globalization = {
            createNewPostDialogTitle: null
        };

    // Assign objects to module.
    blog.links = links;
    blog.globalization = globalization;
    blog.selectors = selectors;

    /**
    * Opens blog edit form
    */
    function openBlogEditForm(url, title, postSuccess) {
        modal.open({
            title: title,
            onLoad: function (dialog) {
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable: initEditBlogPostDialogEvents,

                    beforePost: function () {
                        htmlEditor.updateEditorContent();
                    },
                    
                    postSuccess: postSuccess
                });
            }
        });
    }

    /**
    * Initializes blog edit form
    */
    function initEditBlogPostDialogEvents(dialog) {
        dialog.container.find(selectors.datePickers).initializeDatepicker();
        
        htmlEditor.initializeHtmlEditor(selectors.htmlEditor);
    }

    /**
    * Posts new blog article
    */
    blog.postNewArticle = function () {
        var url = links.loadCreateNewPostDialogUrl, 
            title = globalization.createNewPostDialogTitle,
            postSuccess = function (json) {
                if (json && json.Data && json.Data.PageUrl) {
                    window.location.href = json.Data.PageUrl;
                }
            };
        
        openBlogEditForm(url, title, postSuccess);
    };

    /**
    * Initializes site settings blogs list
    */
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
