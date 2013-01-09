/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.blog', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.grid', 'bcms.pages', 'knockout', 'bcms.media'],
    function ($, bcms, modal, siteSettings, dynamicContent, datepicker, htmlEditor, grid, pages, ko, media) {
    'use strict';

    var blog = { },
        selectors = {
            datePickers: '.bcms-datepicker',
            htmlEditor: 'bcms-contenthtml',
            siteSettingsBlogsListForm: '#bcms-blogs-form',
            siteSettingsBlogsSearchButton: '#bcms-blogs-search-btn',
            siteSettingsBlogCreateButton: '#bcms-create-blog-button',
            siteSettingsBlogDeleteButton: '.bcms-grid-item-delete-button',
            siteSettingsBlogParentRow: 'tr:first',
            siteSettingsBlogEditButton: '.bcms-grid-item-edit-button',
            siteSettingsRowCells: 'td',
            siteSettingsBlogCellPrefix: '.bcms-blog-',
            siteSettingsBlogBooleanTemplateFalse: '#bcms-boolean-false-template',
            siteSettingsBlogBooleanTemplateTrue: '#bcms-boolean-true-template',
            siteSettingsBlogRowTemplate: '#bcms-blogs-list-row-template',
            siteSettingsBlogRowTemplateFirstRow: 'tr:first',
            siteSettingsBlogsTableFirstRow: 'table.bcms-tables > tbody > tr:first'
        },
        links = {
            loadSiteSettingsBlogsUrl: null,
            loadSiteSettingsBlogListsUrl: null,
            loadCreateNewPostDialogUrl: null,
            loadEditPostDialogUrl: null
        },
        globalization = {
            createNewPostDialogTitle: null,
            editPostDialogTitle: null,
            deleteBlogDialogTitle: null
        };

    // Assign objects to module.
    blog.links = links;
    blog.globalization = globalization;
    blog.selectors = selectors;

    function BlogPostViewModel(blogPost) {
        var self = this;

        self.imageId = ko.observable(blogPost.ImageId);
        self.imageUrl = ko.observable(blogPost.ImageUrl);
        self.thumbnailUrl = ko.observable(blogPost.ThumbnailUrl);
        self.imageTooltip = ko.observable(blogPost.ImageTooltip);

        self.selectImage = function() {
            media.openImageInsertDialog(function (imageViewModel) {
                self.thumbnailUrl(imageViewModel.thumbnailUrl);
                self.imageUrl(imageViewModel.publicUrl());
                self.imageTooltip(imageViewModel.tooltip);
                self.imageId(imageViewModel.id());
            }, false);
        };

        self.previewImage = function () {
            var previewUrl = self.imageUrl();
            if (previewUrl) {
                modal.imagePreview(previewUrl, self.imageTooltip());
            }
        };
    }

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
    function initEditBlogPostDialogEvents(dialog, content) {
        dialog.container.find(selectors.datePickers).initializeDatepicker();
        
        htmlEditor.initializeHtmlEditor(selectors.htmlEditor);

        var viewModel = new BlogPostViewModel(content.Data.Data);
        ko.applyBindings(viewModel, dialog.container.get(0));
    }

    /**
    * Posts new blog article
    */
    blog.postNewArticle = function () {
        var postSuccess = function(json) {
            if (json && json.Data && json.Data.PageUrl) {
                window.location.href = json.Data.PageUrl;
            }
        };
        createBlogPost(postSuccess);
    };

    /**
    * Created new blog post
    */
    function createBlogPost(postSuccess) {
        var url = links.loadCreateNewPostDialogUrl,
            title = globalization.createNewPostDialogTitle;

        openBlogEditForm(url, title, postSuccess);
    }
        
    /**
    * Edits blog post
    */
    function editBlogPost(id, postSuccess) {
        var url = $.format(links.loadEditPostDialogUrl, id),
            title = globalization.editPostDialogTitle;

        openBlogEditForm(url, title, postSuccess);
    }

    /**
    * Called after successfull blog post save
    */
    function onAfterSiteSettingsBlogPostSaved(json, row) {
        if (json.Data != null) {
            row.find(selectors.siteSettingsBlogCellPrefix + 'Title').html(json.Data.Title);
            row.find(selectors.siteSettingsBlogCellPrefix + 'ModifiedOn').html(json.Data.ModifiedOn);
            row.find(selectors.siteSettingsBlogCellPrefix + 'ModifiedByUser').html(json.Data.ModifiedByUser);
            row.find(selectors.siteSettingsBlogCellPrefix + 'CreatedOn').html(json.Data.CreatedOn);
            
            row.find(selectors.siteSettingsBlogEditButton).data('id', json.Data.Id);
            row.find(selectors.siteSettingsBlogDeleteButton).data('id', json.Data.Id);
            row.find(selectors.siteSettingsBlogDeleteButton).data('version', json.Data.Version);

            siteSettingsSetBooleanTemplate(row.find(selectors.siteSettingsBlogCellPrefix + 'IsPublished'), json.Data.IsPublished);
        }
    }

    /**
    * Adds boolean template to specified container
    */
    function siteSettingsSetBooleanTemplate (container, value) {
        var template = (value === true) ? $(selectors.siteSettingsBlogBooleanTemplateTrue) : $(selectors.siteSettingsBlogBooleanTemplateFalse),
            html = $(template.html());
        container.html(html);
    };

    /**
    * Initializes site settings blogs list
    */
    function initializeSiteSettingsBlogsList() {
        var dialog = siteSettings.getModalDialog(),
            container = dialog.container;

        var form = dialog.container.find(selectors.siteSettingsBlogsListForm);
        grid.bindGridForm(form, function (data) {
            siteSettings.setContent(data);
            initializeSiteSettingsBlogsList(data);
        });

        form.on('submit', function (event) {
            bcms.stopEventPropagation(event);
            searchSiteSettingsBlogs(form);
            return false;
        });

        form.find(selectors.siteSettingsBlogsSearchButton).on('click', function () {
            searchSiteSettingsBlogs(form);
        });

        container.find(selectors.siteSettingsBlogCreateButton).on('click', function () {
            createBlogPost(function (json) {
                var template = $(selectors.siteSettingsBlogRowTemplate),
                    newRow = $(template.html()).find(selectors.siteSettingsBlogRowTemplateFirstRow);

                onAfterSiteSettingsBlogPostSaved(json, newRow);
                
                newRow.insertBefore($(selectors.siteSettingsBlogsTableFirstRow, container));
                initializeSiteSettingsBlogsListItems(newRow);
                grid.showHideEmptyRow(container);
            });
        });

        initializeSiteSettingsBlogsListItems(container);
    }
     
    /**
    * Initializes site settings blogs list items for current container
    */
    function initializeSiteSettingsBlogsListItems(container) {
        container.find(selectors.siteSettingsRowCells).on('click', function () {
            var row = $(this).parents(selectors.siteSettingsBlogParentRow),
                id = row.find(selectors.siteSettingsBlogEditButton).data("id");

            editBlogPost(id, function (json) {
                onAfterSiteSettingsBlogPostSaved(json, row);
            });
        });

        container.find(selectors.siteSettingsBlogDeleteButton).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            var self = $(this),
                id = self.data('id');
            pages.deletePage(id, function () {
                self.parents(selectors.siteSettingsBlogParentRow).remove();
                grid.showHideEmptyRow(container);
            }, globalization.deleteBlogDialogTitle);
        });
    }
        
    /**
    * Search site settings blogs
    */
    function searchSiteSettingsBlogs(form) {
        grid.submitGridForm(form, function (data) {
            siteSettings.setContent(data);
            initializeSiteSettingsBlogsList(data);
        });
    }

    /**
    * Loads a media manager view to the site settings container.
    */
    blog.loadSiteSettingsBlogs = function () {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsBlogsUrl, {
            contentAvailable: initializeSiteSettingsBlogsList
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
