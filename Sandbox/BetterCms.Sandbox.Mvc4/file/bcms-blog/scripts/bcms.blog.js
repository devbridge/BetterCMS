/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.blog', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.grid', 'bcms.pages', 'knockout', 'bcms.media', 'bcms.pages.tags', 'bcms.ko.grid'],
    function ($, bcms, modal, siteSettings, dynamicContent, datepicker, htmlEditor, grid, pages, ko, media, tags, kogrid) {
    'use strict';

    var blog = { },
        selectors = {
            datePickers: '.bcms-datepicker',
            htmlEditor: 'bcms-contenthtml',
            addTagsField: '.bcms-add-tags-field',
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
            siteSettingsBlogsTableFirstRow: 'table.bcms-tables > tbody > tr:first',
            siteSettingsAuthorsTabLink: '.bcms-tab[href="#bcms-tab-2"]',
            siteSettingsTemplatesTabLink: '.bcms-tab[href="#bcms-tab-3"]',
            siteSettingsBlogsTab: '#bcms-tab-1',
            siteSettingsAuthorsTab: '#bcms-tab-2',
            siteSettingsTemplatesTab: '#bcms-tab-3'
        },
        links = {
            loadSiteSettingsBlogsUrl: null,
            loadSiteSettingsBlogListsUrl: null,
            loadCreateNewPostDialogUrl: null,
            loadEditPostDialogUrl: null,
            loadAuthorsTemplateUrl: null,
            loadAuthorsUrl: null,
            deleteAuthorsUrl: null,
            saveAuthorsUrl: null
        },
        globalization = {
            createNewPostDialogTitle: null,
            editPostDialogTitle: null,
            deleteBlogDialogTitle: null,
            deleteAuthorDialogTitle: null
        };

    // Assign objects to module.
    blog.links = links;
    blog.globalization = globalization;
    blog.selectors = selectors;

    blog.authorsViewModel = null;
    blog.templatesViewModel = null;

    var ImageViewModel = (function () {
        
        ImageViewModel = function (image) {
            
            var self = this;

            self.id = ko.observable();
            self.url = ko.observable();
            self.thumbnailUrl = ko.observable();
            self.tooltip = ko.observable();

            self.preview = function(data, event) {
                bcms.stopEventPropagation(event);

                var previewUrl = self.url();
                if (previewUrl) {
                    modal.imagePreview(previewUrl, self.tooltip());
                }
            };

            self.setImage = function(imageData) {
                self.thumbnailUrl(imageData.ThumbnailUrl);
                self.url(imageData.ImageUrl);
                self.tooltip(imageData.ImageTooltip);
                self.id(imageData.ImageId);
            };

            if (image) {
                self.setImage(image);
            }
        };

        ImageViewModel.prototype.select = function (data, event) {
            var self = this;
            bcms.stopEventPropagation(event);

            media.openImageInsertDialog(function (insertedImage) {
                self.thumbnailUrl(insertedImage.thumbnailUrl);
                self.url(insertedImage.publicUrl());
                self.tooltip(insertedImage.tooltip);
                self.id(insertedImage.id());

                self.onAfterSelect();
            }, false);
        };

        ImageViewModel.prototype.onAfterSelect = function() {
        };
        
        return ImageViewModel;
    })();

    function BlogPostViewModel(blogPost, tagsViewModel) {
        var self = this;

        self.tags = tagsViewModel;
        self.image = ko.observable(new ImageViewModel(blogPost));
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
        var data = content.Data.Data.Data,
            tagsList = content.Data.Data.Data.Tags;
        dialog.container.find(selectors.datePickers).initializeDatepicker();
        
        htmlEditor.initializeHtmlEditor(selectors.htmlEditor);

        var tagsViewModel = new tags.TagsListViewModel(tagsList);

        var blogViewModel = new BlogPostViewModel(data, tagsViewModel);
        ko.applyBindings(blogViewModel, dialog.container.get(0));
        
        bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.addTagsField), {
            preventedEnter: function () {
                blogViewModel.tags.addTag();
            },
            preventedEsc: function () {
                blogViewModel.tags.clearTag();
            }
        });
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
            container = dialog.container.find(selectors.siteSettingsBlogsTab);

        var form = container.find(selectors.siteSettingsBlogsListForm);
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

        // Init other tabs
        blog.authorsViewModel = null;
        blog.templatesViewModel = null;
        dialog.container.find(selectors.siteSettingsAuthorsTabLink).on('click', openAuthorsTab);
        dialog.container.find(selectors.siteSettingsTemplatesTabLink).on('click', openTemplatesTab);
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

        /*
        // TODO
        var tabs = [];
        tabs.push({
            title: globalization.postsTitle,
            url: links.postsUrl,
            onContentAvailable: function (tabContainer) {
                
            }
        });
        tabs.push({
            title: globalization.optionsTitle,
            url: links.optionsUrl,
            onContentAvailable: function(tabContainer) {

            }
        });
        siteSettings.initContentTabs(tabs);*/
    };

    /**
    * Loads authors tab
    */
    function openAuthorsTab() {
        if (blog.authorsViewModel == null) {
            loadTabData(links.loadAuthorsTemplateUrl, null, initializeSiteSettingsAuthorsList);
        }
    }

    /**
    * Loads templates tab
    */
    function openTemplatesTab() {
        if (blog.templatesViewModel == null) {
            blog.templatesViewModel = '// TODO: needs implementation';
            alert('// TODO: needs implementation');
        }
    }

    /**
    * Loads tab data
    */
    function loadTabData(url, params, onComplete) {
        $.ajax({
            type: 'POST',
            cache: false,
            url: url,
            data: params
        })
            .done(function(result) {
                onComplete(result);
            })
            .fail(function(response) {
                onComplete(bcms.parseFailedResponse(response));
            });
    }

    /**
    * Initializes site settings authors tab
    */
    function initializeSiteSettingsAuthorsList(json) {
        var dialog = siteSettings.getModalDialog(),
            container = dialog.container.find(selectors.siteSettingsAuthorsTab),
            html = json.Data.Html,
            data = json.Data.Data.Data;

        container.html(html);

        blog.authorsViewModel = new AuthorsListViewModel(container, data.Items, data.GridOptions);
        blog.authorsViewModel.deleteUrl = links.deleteAuthorsUrl;
        blog.authorsViewModel.saveUrl = links.saveAuthorsUrl;

        ko.applyBindings(blog.authorsViewModel, container.get(0));
    }
     
    /**
    * Author image view model
    */
    var AuthorImageViewModel = (function (_super) {
        
        bcms.extendsClass(AuthorImageViewModel, _super);
        
        function AuthorImageViewModel(parent) {
            var self = this;
            self.parent = parent;

            _super.call(this);
        }
        
        AuthorImageViewModel.prototype.select = function (data, event) {
            this.parent.isSelected = true;
            this.parent.isActive(true);

            _super.prototype.select.call(this, data, event);
        };

        AuthorImageViewModel.prototype.onAfterSelect = function () {
            this.parent.hasFocus(true);
        };

        return AuthorImageViewModel;
    })(ImageViewModel);

    /**
    * Authors list view model
    */
    var AuthorsListViewModel = (function (_super) {

        bcms.extendsClass(AuthorsListViewModel, _super);

        function AuthorsListViewModel(container, items, gridOptions) {
            _super.call(this, container, links.loadAuthorsUrl, items, gridOptions);
        }
        
        AuthorsListViewModel.prototype.createItem = function (item) {
            var newItem = new AuthorViewModel(this, item);
            return newItem;
        };

        return AuthorsListViewModel;

    })(kogrid.ListViewModel);

    /**
    * Author view model
    */
    var AuthorViewModel = (function (_super) {
        
        bcms.extendsClass(AuthorViewModel, _super);
        
        function AuthorViewModel(parent, item) {
            _super.call(this, parent, item);

            var self = this;

            self.name = ko.observable().extend({ required: "" });
            self.image = ko.observable(new AuthorImageViewModel(self));
            self.oldImageId = item.ImageId;

            self.registerFields(self.name, self.image().id);

            self.name(item.Name);
            self.image().setImage(item);
        }

        AuthorViewModel.prototype.getDeleteConfirmationMessage = function () {
            return $.format(globalization.deleteAuthorDialogTitle, this.name());
        };
        
        AuthorViewModel.prototype.getSaveParams = function () {
            var params = _super.prototype.getSaveParams.call(this);
            params.Name = this.name();
            params.ImageId = this.image().id();

            return params;
        };

        AuthorViewModel.prototype.hasChanges = function () {
            var hasChanges = _super.prototype.hasChanges.call(this);
            if (!hasChanges) {
                hasChanges = this.image().id() != this.oldImageId;
            }

            return hasChanges;
        };

        return AuthorViewModel;
        
    })(kogrid.ItemViewModel);

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
