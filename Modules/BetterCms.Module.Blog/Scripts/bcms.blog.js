/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.blog', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.grid', 'bcms.pages', 'bcms.ko.extenders', 'bcms.media', 'bcms.pages.tags', 'bcms.ko.grid', 'bcms.messages', 'bcms.redirect', 'bcms.pages.history', 'bcms.preview'],
    function ($, bcms, modal, siteSettings, dynamicContent, datepicker, htmlEditor, grid, pages, ko, media, tags, kogrid, messages, redirect, history, preview) {
    'use strict';

    var blog = { },
        selectors = {
            datePickers: '.bcms-datepicker',
            htmlEditor: 'bcms-contenthtml',
            firstForm: 'form:first',
            siteSettingsBlogsListForm: '#bcms-blogs-form',
            siteSettingsBlogsSearchButton: '#bcms-blogs-search-btn',
            siteSettingsBlogsSearchInput: '.bcms-search-query',
            siteSettingsBlogCreateButton: '#bcms-create-blog-button',
            siteSettingsBlogDeleteButton: '.bcms-grid-item-delete-button',
            siteSettingsBlogParentRow: 'tr:first',
            siteSettingsBlogEditButton: '.bcms-grid-item-edit-button',
            siteSettingsRowCells: 'td',
            siteSettingsBlogCellPrefix: '.bcms-blog-',
            siteSettingsBlogTitleCell: '.bcms-blog-Title',
            siteSettingsPageStatusTemplatePublished: '#bcms-pagestatus-published-template',
            siteSettingsPageStatusTemplateUnpublished: '#bcms-pagestatus-unpublished-template',
            siteSettingsPageStatusTemplateDraft: '#bcms-pagestatus-draft-template',
            siteSettingsBlogRowTemplate: '#bcms-blogs-list-row-template',
            siteSettingsBlogRowTemplateFirstRow: 'tr:first',
            siteSettingsBlogsTableFirstRow: 'table.bcms-tables > tbody > tr:first',
            overlayConfigure: '.bcms-content-configure',
            overlayDelete: '.bcms-content-delete',
            destroyDraftVersionLink: '.bcms-messages-draft-destroy',
            
            blogTitle: "#bcms-editor-blog-title",
            editPermalink: '#bcms-page-editpermalink',
	        editPermalinkBox: '.bcms-edit-urlpath-box',
	        editPermalinkClose: 'div.bcms-edit-urlpath-box .bcms-tip-close, div.bcms-edit-urlpath-box .bcms-btn-links-small',
	        editPermalinkSave: '#bcms-save-permalink',
	        editPermalinkHiddenField: '#bcms-page-permalink',
	        editPermalinkEditField: '#bcms-page-permalink-edit',
	        editPermalinkInfoField: '#bcms-page-permalink-info'
        },
        links = {
            loadSiteSettingsBlogsUrl: null,
            loadSiteSettingsBlogListsUrl: null,
            loadCreateNewPostDialogUrl: null,
            loadEditPostDialogUrl: null,
            loadAuthorsTemplateUrl: null,
            loadAuthorsUrl: null,
            deleteAuthorsUrl: null,
            saveAuthorsUrl: null,
            loadTemplatesUrl: null,
            saveDefaultTemplateUrl: null,
            convertStringToSlugUrl: null
        },
        globalization = {
            createNewPostDialogTitle: null,
            editPostDialogTitle: null,
            deleteBlogDialogTitle: null,
            deleteAuthorDialogTitle: null,
            blogPostsTabTitle: null,
            authorsTabTitle: null,
            templatesTabTitle: null,
            datePickerTooltipTitle: null
        },
        contentTypes = {
            blogContent: 'blog-post-content'
        };

    // Assign objects to module.
    blog.links = links;
    blog.globalization = globalization;
    blog.selectors = selectors;
    blog.senderId = 0;

    blog.authorsViewModel = null;
    blog.templatesViewModel = null;

    /**
    * Blog post view model
    */
    function BlogPostViewModel(image, tagsViewModel, id, version) {
        var self = this;

        self.tags = tagsViewModel;
        self.image = ko.observable(new media.ImageSelectorViewModel(image));
        self.id = ko.observable(id);
        self.version = ko.observable(version);
    }

    /**
    * Opens blog edit form
    */
    function openBlogEditForm(url, title, postSuccess, onClose, calledFromPage) {
        var blogViewModel;
        var permalinkValue;
        modal.edit({
            title: title,
            onLoad: function (dialog) {
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable: function (dialog, content) {
                        blogViewModel = initEditBlogPostDialogEvents(dialog, content, calledFromPage, postSuccess);
                    },

                    beforePost: function () {
                        if (!pages.isEditedPageUrlManually()) {
                            var blogUrlField = dialog.container.find(selectors.editPermalinkEditField);
                            permalinkValue = blogUrlField.val();
                            blogUrlField.val(null);
                        }
                        htmlEditor.updateEditorContent();
                    },
                    
                     postError: function () {
                        if (!pages.isEditedPageUrlManually()) {
                            var blogUrlField = dialog.container.find(selectors.editPermalinkEditField);
                            blogUrlField.val(permalinkValue);
                        }
                    },
                    
                    postSuccess: function (json) {
                        if (json.Data.DesirableStatus == bcms.contentStatus.preview) {
                            try {
                                var result = json.Data;
                                blogViewModel.version(result.Version);
                                blogViewModel.id(result.Id);
                                preview.previewPageContent(result.Id, result.PageContentId);
                            } finally {
                                return false;
                            }
                        } else {
                            if ($.isFunction(postSuccess)) {
                                postSuccess(json);
                            }
                        }
                        return true;
                    }
                });
            },
            onAccept: function() {
                htmlEditor.destroyAllHtmlEditorInstances();
            },
            onClose: function() {
                htmlEditor.destroyAllHtmlEditorInstances();
                if ($.isFunction(onClose)) {
                    onClose();
                }
            }
        });
    }

    /**
    * Initializes blog edit form
    */
    function initEditBlogPostDialogEvents(dialog, content, calledFromPage, postSuccess) {
        var data = content.Data,
            image = data.Image,
            tagsList = data.Tags,
            newPost = false;
        dialog.container.find(selectors.datePickers).initializeDatepicker(globalization.datePickerTooltipTitle);
        
        htmlEditor.initializeHtmlEditor(selectors.htmlEditor);
        
        if (data.Version == 0) {
            newPost = true;
        }
        
        pages.initializePermalinkBox(dialog, false, links.convertStringToSlugUrl, selectors.blogTitle, newPost);
        
        var tagsViewModel = new tags.TagsListViewModel(tagsList);

        var blogViewModel = new BlogPostViewModel(image, tagsViewModel, data.Id, data.Version);
        ko.applyBindings(blogViewModel, dialog.container.find(selectors.firstForm).get(0));
        
        dialog.container.find(selectors.destroyDraftVersionLink).on('click', function () {
            history.destroyDraftVersion(data.ContentId, dialog.container, function () {
                var onSave = postSuccess,
                    onClose = null;

                dialog.close();
                if (calledFromPage) {
                    onClose = postSuccess;
                }
                editBlogPost(data.Id, onSave, calledFromPage, onClose);
            });
        });

        return blogViewModel;
    }

    /**
    * Posts new blog article
    */
    blog.postNewArticle = function () {
        var postSuccess = function(json) {
            if (json && json.Data && json.Data.PageUrl) {
                redirect.RedirectWithAlert(json.Data.PageUrl);
            }
        };
        createBlogPost(postSuccess);
    };

    /**
    * Created new blog post
    */
    function createBlogPost(postSuccess) {
        var url = $.format(links.loadCreateNewPostDialogUrl, window.location.pathname),
            title = globalization.createNewPostDialogTitle;

        openBlogEditForm(url, title, postSuccess);
    }
        
    /**
    * Edits blog post
    */
    function editBlogPost(id, postSuccess, calledFromPage, onClose) {
        var url = $.format(links.loadEditPostDialogUrl, id),
            title = globalization.editPostDialogTitle;

        openBlogEditForm(url, title, postSuccess, onClose, calledFromPage);
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

            messages.refreshBox(selectors.siteSettingsBlogsListForm, json);

            row.find(selectors.siteSettingsBlogTitleCell).data('url', json.Data.PageUrl);
            row.find(selectors.siteSettingsBlogEditButton).data('id', json.Data.Id);
            row.find(selectors.siteSettingsBlogDeleteButton).data('id', json.Data.Id);
            row.find(selectors.siteSettingsBlogDeleteButton).data('version', json.Data.Version);

            siteSettingsPageStatusTemplate(row.find(selectors.siteSettingsBlogCellPrefix + 'IsPublished'), json.Data.PageStatus);
        }
    }

    /**
    * Creates html for page status value, using given PageStatus template
    */
    function siteSettingsPageStatusTemplate(container, value) {
        var template;
        
        if (value == 2) {
            template = $(selectors.siteSettingsPageStatusTemplateDraft);
        }   else if (value == 3) {
            template = $(selectors.siteSettingsPageStatusTemplatePublished);
        } else {
            template = $(selectors.siteSettingsPageStatusTemplateUnpublished);
        }
        
        var html = $(template.html());
        container.html(html);
    };
        
    /**
    * Initializes site settings blogs list
    */
    function initializeSiteSettingsBlogsList(container, content) {
        
        var form = container.find(selectors.siteSettingsBlogsListForm);
        grid.bindGridForm(form, function (data) {
            container.html(data);
            initializeSiteSettingsBlogsList(container, data);
        });

        form.on('submit', function (event) {
            bcms.stopEventPropagation(event);
            searchSiteSettingsBlogs(container, form);
            return false;
        });
        
        form.find(selectors.siteSettingsBlogTitleCell).on('click', function (event) {
            bcms.stopEventPropagation(event);
            var url = $(this).data('url');
            window.open(url);
        });

        form.find(selectors.siteSettingsBlogsSearchButton).on('click', function () {
            searchSiteSettingsBlogs(container, form);
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
                id = row.find(selectors.siteSettingsBlogEditButton).data("id"),
                onSave = function (json) {
                    onAfterSiteSettingsBlogPostSaved(json, row);
                };

            editBlogPost(id, onSave, false);
        });

        container.find(selectors.siteSettingsBlogTitleCell).on('click', function (event) {
            bcms.stopEventPropagation(event);
            var url = $(this).data('url');
            window.open(url);
        });

        container.find(selectors.siteSettingsBlogDeleteButton).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            var self = $(this),
                id = self.data('id');
            pages.deletePage(id, function (json) {
                var row = self.parents(selectors.siteSettingsBlogParentRow);
                messages.refreshBox(row, json);
                row.remove();
                grid.showHideEmptyRow(container);
            }, globalization.deleteBlogDialogTitle);
        });
    }
        
    /**
    * Search site settings blogs
    */
    function searchSiteSettingsBlogs(container, form) {
        grid.submitGridForm(form, function (data) {
            container.html(data);
            initializeSiteSettingsBlogsList(container, data);           
            var searchInput = container.find(selectors.siteSettingsBlogsSearchInput);
            var val = searchInput.val();
            searchInput.focus().val("");
            searchInput.val(val);       
        });
    }

    /**
    * Loads a media manager view to the site settings container.
    */
    blog.loadSiteSettingsBlogs = function () {
        var tabs = [];

        var blogs = new siteSettings.TabViewModel(globalization.blogPostsTabTitle, links.loadSiteSettingsBlogsUrl, initializeSiteSettingsBlogsList);
        var authors = new siteSettings.TabViewModel(globalization.authorsTabTitle, links.loadAuthorsTemplateUrl, initializeSiteSettingsAuthorsList);
        var templates = new siteSettings.TabViewModel(globalization.templatesTabTitle, links.loadTemplatesUrl, initializeSiteSettingsTemplatesList);
        
        tabs.push(blogs);
        tabs.push(authors);
        tabs.push(templates);
        
        siteSettings.initContentTabs(tabs);
    };

    /**
    * Initializes site settings authors tab
    */
    function initializeSiteSettingsAuthorsList(container, json) {
        var html = json.Html,
            data = (json.Success == true) ? json.Data : null;

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

            self.onAfterAction = function () {
                if (self.parent.isActive()) {
                    self.parent.hasFocus(true);
                }

                return true;
            };

            self.onBeforeAction = function() {
                self.parent.isSelected = true;
                self.parent.isActive(true);
                
                return true;
            };
        }

        AuthorImageViewModel.prototype.select = function (data, event) {
            this.onBeforeAction();

            _super.prototype.select.call(this, data, event);
        };

        AuthorImageViewModel.prototype.preview = function (data, event) {
            bcms.stopEventPropagation();
            if (this.parent.isActive()) {
                this.onBeforeAction();
            }

            _super.prototype.preview.call(this, data, event);
        };

        AuthorImageViewModel.prototype.remove = function (data, event) {
            this.onBeforeAction();

            _super.prototype.remove.call(this, data, event);
        };

        AuthorImageViewModel.prototype.onAfterSelect = function () {
            this.onAfterAction();
        };
        
        AuthorImageViewModel.prototype.onAfterRemove = function () {
            this.onAfterAction();
        };

        AuthorImageViewModel.prototype.onAfterPreview = function () {
            this.onAfterAction();
        };

        AuthorImageViewModel.prototype.onSelectClose = function () {
            this.onAfterAction();
        };

        return AuthorImageViewModel;
        
    })(media.ImageSelectorViewModel);

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

            self.name = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name } });
            self.image = ko.observable(new AuthorImageViewModel(self));
            self.oldImageId = item.Image != null ? item.Image.ImageId : '';

            self.registerFields(self.name, self.image().id, self.image().url, self.image().thumbnailUrl, self.image().tooltip);

            self.name(item.Name);
            self.image().setImage(item.Image || {});
        }

        AuthorViewModel.prototype.getDeleteConfirmationMessage = function () {
            return $.format(globalization.deleteAuthorDialogTitle, this.name());
        };
        
        AuthorViewModel.prototype.getSaveParams = function () {
            var params = _super.prototype.getSaveParams.call(this);
            params.Name = this.name();
            params.Image = {
                ImageId: this.image().id()
            };

            return params;
        };

        AuthorViewModel.prototype.hasChanges = function () {
            var hasChanges = _super.prototype.hasChanges.call(this);
            if (!hasChanges) {
                hasChanges = (this.image().id() || '') != this.oldImageId;
            }

            return hasChanges;
        };

        AuthorViewModel.prototype.onAfterItemSaved = function (json) {
            _super.prototype.onAfterItemSaved.call(this, json);

            this.oldImageId = this.image().id();
        };

        return AuthorViewModel;
        
    })(kogrid.ItemViewModel);

    /**
    * Initializes site settings templates tab
    */
    function initializeSiteSettingsTemplatesList(container, json) {
        var html = json.Html,
            templates = (json.Success == true) ? json.Data : null;

        container.html(html);

        blog.templatesViewModel = new TemplatesListViewModel(templates, container);

        ko.applyBindings(blog.templatesViewModel, container.get(0));
    }

    /**
    * Template row view model
    */
    function TemplateRowViewModel() {
        var self = this;
        
        self.templates = ko.observableArray();
    }

    /**
    * Templates list view model
    */
    function TemplatesListViewModel(templates, container) {
        var self = this;

        self.templates = ko.observableArray();
        self.templateRows = ko.observableArray();
        self.searchQuery = ko.observable();
        
        if (templates != null) {
            for (var i = 0; i < templates.length; i++) {
                var template = new TemplateViewModel(templates[i], self, container);
                self.templates.push(template);
            }
        }

        self.fillTemplateRows = function () {
            self.templateRows.removeAll();

            var row = new TemplateRowViewModel(),
                query = (self.searchQuery() || '').toLowerCase(),
                items = 0;

            for (var j = 0; j < self.templates().length; j++) {
                var currentTemplate = self.templates()[j];
                if (query && currentTemplate.title.toLowerCase().indexOf(query) < 0) {
                    continue;
                }

                if (items == 0 || items % 3 == 0) {
                    row = new TemplateRowViewModel();
                    self.templateRows.push(row);
                }

                items++;
                row.templates.push(currentTemplate);
            }
        };

        self.search = function() {
            self.fillTemplateRows();
        };

        self.fillTemplateRows();
    }

    /**
    * Template view model
    */
    function TemplateViewModel(template, parent, container) {
        var self = this;

        self.id = template.TemplateId;
        self.parent = parent;
        self.previewUrl = template.PreviewUrl;
        self.title = template.Title;
        self.container = container;
        
        self.isActive = ko.observable(template.IsActive);
        self.isCompatible = template.IsCompatible;

        self.select = function () {
            var url = $.format(links.saveDefaultTemplateUrl, self.id),
                onComplete = function (json) {
                    self.container.hideLoading();
                    messages.refreshBox(self.container, json);
                    if (json.Success == true) {
                        for (var i = 0; i < self.parent.templates().length; i++) {
                            self.parent.templates()[i].unselect();
                        }

                        self.isActive(true);
                    }
                };

            self.container.showLoading();

            $.ajax({
                type: 'POST',
                cache: false,
                url: url
            })
                .done(function(result) {
                    onComplete(result);
                })
                .fail(function(response) {
                    onComplete(bcms.parseFailedResponse(response));
                });
        };

        self.unselect = function() {
            self.isActive(false);
        };

        self.previewImage = function() {
            modal.imagePreview(self.previewUrl, self.title);
        };
    }

    /**
    * Called when content overlay is created
    */
    function onCreateContentOverlay(contentViewModel) {
        var onSave = function () {
                redirect.ReloadWithAlert();
            };
        
        if (contentViewModel.contentType == contentTypes.blogContent) {
            contentViewModel.removeConfigureButton();
            contentViewModel.removeDeleteButton();

            // Edit
            contentViewModel.onEditContent = function() {
                editBlogPost(bcms.pageId, onSave, true);
            };
        }
    }

    /**
    * Initializes blog module.
    */
    blog.init = function () {
        console.log('Initializing blog module');
    };

    /**
    * Subscribe to events
    */
    bcms.on(bcms.events.createContentOverlay, onCreateContentOverlay);
    

    /**
    * Register initialization
    */
    bcms.registerInit(blog.init);
    
    return blog;
});
