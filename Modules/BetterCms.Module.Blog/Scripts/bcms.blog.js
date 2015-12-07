/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.blog.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.blog', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.grid', 'bcms.pages', 'bcms.categories', 'bcms.ko.extenders', 'bcms.media', 'bcms.tags', 'bcms.ko.grid', 'bcms.messages', 'bcms.redirect', 'bcms.pages.history', 'bcms.preview', 'bcms.security', 'bcms.blog.filter', 'bcms.sidemenu', 'bcms.forms', 'bcms.pages.widgets', 'bcms.antiXss', 'bcms.content'],
    function ($, bcms, modal, siteSettings, dynamicContent, datepicker, htmlEditor, grid, pages, categories, ko, media, tags, kogrid, messages, redirect, history, preview, security, filter, sidemenu, forms, widgets, antiXss, bcmsContent) {
        'use strict';
        
        var blog = {},
            selectors = {
                datePickers: '.bcms-datepicker',
                htmlEditor: 'bcms-blogcontent',
                firstForm: 'form:first',
                secondForm: 'form:nth-child(2)',
                siteSettingsBlogsListForm: '#bcms-blogs-form',
                siteSettingsBlogsSearchButton: '#bcms-blogs-search-btn',
                siteSettingsBlogsSearchInput: '.bcms-search-query',
                siteSettingsBlogCreateButton: '#bcms-create-blog-button',
                siteSettingsBlogExportButton: '#bcms-export-blogs',
                siteSettingsBlogImportButton: '#bcms-import-blogs',
                siteSettingsBlogContentWindow: '.bcms-window-options',
                siteSettingsBlogDeleteButton: '.bcms-grid-item-delete-button',
                siteSettingsBlogParentRow: '.bcms-js-list-row:first',
                siteSettingsBlogEditButton: '.bcms-js-edit-button',
                siteSettingsRowCells: '.bcms-js-list-row',
                siteSettingsBlogCellPrefix: '.bcms-blog-',
                siteSettingsBlogTitleCell: '.bcms-page-title',
                siteSettingsPageStatusTemplatePublished: '#bcms-pagestatus-published-template',
                siteSettingsPageStatusTemplateUnpublished: '#bcms-pagestatus-unpublished-template',
                siteSettingsPageStatusTemplateDraft: '#bcms-pagestatus-draft-template',
                siteSettingsBlogRowTemplate: '#bcms-blogs-list-row-template',
                siteSettingsBlogRowTemplateFirstRow: '.bcms-js-list-row:first',
                siteSettingsBlogsTableFirstRow: '.bcms-list-pages > .bcms-js-list-row:first',
                overlayConfigure: '.bcms-content-configure',
                overlayDelete: '.bcms-content-delete',
                destroyDraftVersionLink: '.bcms-messages-draft-destroy',
                blogTitle: "#bcms-editor-blog-title",
                editPermalinkEditField: '#bcms-page-permalink-edit',
                contentUserConfirmationHiddenField: '#bcms-user-confirmed-region-deletion',
                blogPostFormDatePickers: 'input.bcms-datepicker',
                importBlogPostsForm: '#bcms-import-blog-posts',
                fileUploadingTarget: '#bcms-import-form-target',
                fileUploadingResult: '#jsonResult',
                categorySelection: "input[name*='Categories']",
                siteSettingsBlogCloseInfoMessage: "#bcms-addnewblog-closeinfomessage",
                siteSettingsBlogInfoMessageBox: ".bcms-warning-messages",
                siteSettingsButtonOpener: ".bcms-btn-opener",
                siteSettingsButtonHolder: ".bcms-btn-opener-holder",
                siteSettingsBlogCategoriesSelect: '#bcms-js-categories-select',
                siteSettingsBlogAuthorsSelect: '#bcms-js-authors-select',
                siteSettingsBlogLanguagesSelect: '#bcms-js-languages-select',
                siteSettingsDefaultBlogContentModeSelect: '#bcms-js-content-mode-select',
                siteSettingsWindow: '.bcms-window-settings',
                hiddenPageNumberField: '#bcms-grid-page-number',

                contentTab: '#bcms-tab-1',
                editorContainer: '.bcms-window-tabbed-options',
                editorTitle: '.bcms-content-titles',
                editorInfoBlock: '.bcms-content-info-block',
                blogPermalinkField: '.bcms-js-blog-permalink',
                markdownEditorHeader: '.markItUpHeader',
                markdownEditorFooter: '.markItUpFooter',
                siteSettingsPager: '.bcms-top-block-pager',
                siteSettingsBlogsGrid: '#bcms-blogs-grid'
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
                loadBlogPostSettingsUrl: null,
                saveDefaultTemplateUrl: null,
                saveBlogPostSettingUrl: null,
                convertStringToSlugUrl: null,
                uploadBlogPostsImportFileUrl: null,
                startImportUrl: null,
                deleteUploadedFileUrl: null,
                exportBlogPostsUrl: null,
                loadTemplatesUrl: null
            },
            globalization = {
                createNewPostDialogTitle: null,
                editPostDialogTitle: null,
                deleteBlogDialogTitle: null,
                deleteAuthorDialogTitle: null,
                blogPostsTabTitle: null,
                authorsTabTitle: null,
                settingsTabTitle: null,
                datePickerTooltipTitle: null,
                importBlogPostsTitle: null,
                closeButtonTitle: null,
                importButtonTitle: null,
                uploadButtonTitle: null,
                multipleFilesWarningMessage: null,
                pleaseSelectAFile: null,
                noBlogPostsSelectedToImport: null,
                editModeHtmlTitle: null,
                editModeMarkdownTitle: null,
                templatesTabTitle: null,
                created: null,
                lastEdited: null,
                lastEditedBy: null
            },
            classes = {
                inactive: 'bcms-inactive'
            },
            contentTypes = {
                blogContent: 'blog-post-content'
            },
        rowId = 0;

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
        function BlogPostViewModel(image, tagsViewModel, id, version, editInSourceMode, categoriesModel) {
            var self = this;

            self.tags = tagsViewModel;
            self.image = ko.observable(new media.ImageSelectorViewModel(image));
            self.id = ko.observable(id);
            self.version = ko.observable(version);
            self.editInSourceMode = ko.observable(editInSourceMode);
            self.categories = categoriesModel;
        }

        /**
        * Opens blog edit form
        */
        function openBlogEditForm(url, title, opts) {

            opts = $.extend({
                postSuccess: null,
                onClose: null,
                calledFromPage: null,
                includeChildRegions: false
            }, opts);

            var canEdit = security.IsAuthorized(["BcmsEditContent"]),
                postSuccess = opts.postSuccess,
                onClose = opts.onClose,
                calledFromPage = opts.calledFromPage,
                includeChildRegions = (opts.includeChildRegions == true),
                blogViewModel,
                permalinkValue;

            modal.edit({
                title: title,
                disableSaveDraft: !canEdit,
                isPreviewAvailable: canEdit,
                disableSaveAndPublish: !security.IsAuthorized(["BcmsPublishContent"]),
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (dialog, content) {
                            blogViewModel = initEditBlogPostDialogEvents(dialog, content, calledFromPage, postSuccess, includeChildRegions);
                        },

                        beforePost: function () {
                            if (!pages.isEditedPageUrlManually()) {
                                var blogUrlField = dialog.container.find(selectors.editPermalinkEditField);
                                permalinkValue = blogUrlField.val();
                                blogUrlField.val(null);
                            }

                            var editInSourceMode = htmlEditor.isSourceMode(selectors.htmlContentWidgetContentHtmlEditor);
                            blogViewModel.editInSourceMode(editInSourceMode);
                        },

                        postError: function (json) {
                            if (!pages.isEditedPageUrlManually()) {
                                var blogUrlField = dialog.container.find(selectors.editPermalinkEditField);
                                blogUrlField.val(permalinkValue);
                            }

                            if (json.Data && json.Data.ConfirmationMessage) {
                                modal.confirm({
                                    content: json.Data.ConfirmationMessage,
                                    onAccept: function () {
                                        dialog.container.find(selectors.contentUserConfirmationHiddenField).val(true);
                                        dialog.submitForm();
                                        return true;
                                    }
                                });
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
                        },

                        formSerialize: function (form) {
                            return widgets.serializeFormWithChildWidgetOptions(form, selectors.htmlEditor, null, function (serializedData) {
                                if (includeChildRegions) {
                                    serializedData.IncludeChildRegions = true;
                                }
                            });
                        },
                        formContentType: 'application/json; charset=utf-8'
                    });
                },
                onAccept: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
                },
                onClose: function () {
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
        function initEditBlogPostDialogEvents(dialog, content, calledFromPage, postSuccess, includeChildRegions) {
            var data = content.Data,
                image = data.Image,
                tagsList = data.Tags,
                contentTextMode = data.ContentTextMode,
                newPost = false,
                canEdit = security.IsAuthorized(["BcmsEditContent"]),
                canPublish = security.IsAuthorized(["BcmsPublishContent"]),
                form = dialog.container.find(selectors.firstForm),
                categorySelector = form.find(selectors.categorySelection),
                getCategoryId = function () {
                    var categorySelector = dialog.container.find(selectors.secondForm).find(selectors.categorySelection);
                    if (categorySelector != null && categorySelector.length > 0) {
                        var categories = categorySelector.map(function () { return $(this).val(); }).get();
                        return categories;
                    }

                    return categorySelector != null ? categorySelector.val() : null;
                },
                heightOptions;

            if (contentTextMode == bcmsContent.contentTextModes.markdown) {
                heightOptions = {
                    topElements: [
                        {
                            element: selectors.editorInfoBlock,
                            takeMargins: true
                        },
                        {
                            element: selectors.blogPermalinkField,
                            takeMargins: true
                        },
                        {
                            element: selectors.editorTitle,
                            takeMargins: true
                        },
                        {
                            element: selectors.markdownEditorHeader,
                            takeMargins: true
                        }
                    ],
                    parent: selectors.contentTab,
                    container: selectors.editorContainer,
                    bottomElement: selectors.markdownEditorFooter,
                    marginBottom: 3
                };

                htmlEditor.initializeMarkdownEditor(selectors.htmlEditor, '', {}, heightOptions);
            }

            if (contentTextMode == bcmsContent.contentTextModes.simpleText) {
                heightOptions = {
                    topElements: [
                        {
                            element: selectors.editorInfoBlock,
                            takeMargins: true
                        },
                        {
                            element: selectors.blogPermalinkField,
                            takeMargins: true
                        },
                        {
                            element: selectors.editorTitle,
                            takeMargins: true
                        }
                    ],
                    parent: selectors.contentTab,
                    container: selectors.editorContainer,
                    bottomElement: selectors.markdownEditorFooter,
                    marginBottom: 3
                };

                htmlEditor.initializeMarkdownEditor(selectors.htmlEditor, '', { hideIcons: true });
            }

            if (contentTextMode == bcmsContent.contentTextModes.html) {
                heightOptions = {
                    topElements: [
                    {
                        element: selectors.editorInfoBlock,
                        takeMargins: true
                    },{
                        element: selectors.blogPermalinkField,
                        takeMargins: true
                    },
                    {
                        element: selectors.editorTitle,
                        takeMargins: true
                    }],
                    container: selectors.editorContainer,
                    parent: selectors.contentTab,
                    marginBottom: 1
                };

                htmlEditor.initializeHtmlEditor(selectors.htmlEditor, data.ContentId, {}, data.EditInSourceMode, heightOptions);
            }

            if (data.Version == 0) {
                newPost = true;
            }

            var generator = pages.initializePermalinkBox(dialog, false, links.convertStringToSlugUrl, selectors.blogTitle, newPost, null, null, getCategoryId);
            if (newPost && categorySelector != null) {
                categorySelector.on('change', function () {
                    generator.Regenerate();
                });
            }

            var tagsViewModel = new tags.TagsListViewModel(tagsList);
            var categoriesModel = new categories.CategoriesSelectListModel(data.Categories, dialog.container);
            var blogViewModel = new BlogPostViewModel(image, tagsViewModel, data.Id, data.Version, data.EditInSourceMode, categoriesModel);

            categories.initCategoriesSelect(categoriesModel, content.Data.CategoriesLookupList, dialog.container);
            $(selectors.siteSettingsBlogLanguagesSelect).select2({
                minimumResultsForSearch: -1
            });
            $(selectors.siteSettingsBlogAuthorsSelect).select2({
                minimumResultsForSearch: -1
            });

            ko.applyBindings(blogViewModel, dialog.container.find(selectors.firstForm).get(0));

            dialog.container.find(selectors.destroyDraftVersionLink).on('click', function () {
                history.destroyDraftVersion(data.ContentId, data.ContentVersion, includeChildRegions, dialog.container, function (publishedId, json) {
                    var onSave = postSuccess,
                        onClose = null;

                    dialog.close();
                    if (calledFromPage) {
                        onClose = function () {
                            onSave(json);
                        };
                    }

                    editBlogPost(data.Id, {
                        postSuccess: onSave,
                        calledFromPage: calledFromPage,
                        onClose: onClose,
                        includeChildRegions: includeChildRegions
                    });
                });
            });

            // User with only BcmsPublishContent but without BcmsEditContent can only publish and change publish dates
            if (form.data('readonly') !== true && canPublish && !canEdit) {
                form.addClass(classes.inactive);
                forms.setFieldsReadOnly(form);

                // Enable date pickers for editing
                $.each(form.find(selectors.blogPostFormDatePickers), function () {
                    var self = $(this);

                    self.removeAttr('readonly');
                    self.parent('div').css('z-index', bcms.getHighestZindex() + 1);
                });
            }

            dialog.container.find(selectors.siteSettingsBlogCloseInfoMessage).on('click', function () {
                dialog.container.find(selectors.siteSettingsBlogInfoMessageBox).hide();
            });

            dialog.container.find(selectors.datePickers).initializeDatepicker(globalization.datePickerTooltipTitle);

            return blogViewModel;
        }

        /**
        * Posts new blog article
        */
        blog.postNewArticle = function () {
            var postSuccess = function (json) {
                if (json && json.Data && json.Data.PageUrl) {
                    sidemenu.turnEditModeOn();
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

            openBlogEditForm(url, title, {
                postSuccess: postSuccess
            });
        }

        /**
        * Edits blog post
        */
        function editBlogPost(id, opts) {

            opts = $.extend({
                postSuccess: null,
                calledFromPage: false,
                onClose: null,
                includeChildRegions: false
            }, opts);

            var url = $.format(links.loadEditPostDialogUrl, id),
                title = globalization.editPostDialogTitle;

            openBlogEditForm(url, title, {
                postSuccess: opts.postSuccess,
                onClose: opts.onClose,
                calledFromPage: opts.calledFromPage,
                includeChildRegions: opts.includeChildRegions
            });
        }

        /**
        * Called after successfull blog post save
        */
        function onAfterSiteSettingsBlogPostSaved(json, item) {
            if (json.Data != null) {
                item.update(json.Data);
            }
        }

        function BlogsGridViewModel(data, form, container) {
            var self = this;

            self.items = ko.observableArray();
            self.items.extend({ rateLimit: 50 });
            self.setItems = function (_items) {
                self.items.removeAll();
                for (var i = 0; i < _items.length; i++) {
                    self.items.push(new BlogItemViewModel(_items[i]));
                }
            }

            self.onOpenPage = function (pageNumber) {
                form.find(selectors.hiddenPageNumberField).val(pageNumber);
                grid.submitGridFormPaged(form, function (content, data) {
                    self.setItems(data.Items);
                });
            }
            self.setItems(data.Items);
            self.paging = new ko.PagingViewModel(data.GridOptions.PageSize, data.GridOptions.PageNumber, data.GridOptions.TotalCount, self.onOpenPage);
            return self;
        };

        function BlogItemViewModel(item) {
            var self = this;

            self.id = ko.observable(item.Id);
            self.title = ko.observable(item.Title);
            self.createdOn = ko.observable(globalization.created + ' ' + item.CreatedOn);
            self.modifiedOn = ko.observable(globalization.lastEdited + ' ' + item.ModifiedOn);
            self.modifiedByUser = ko.observable(globalization.lastEditedBy + ' ' + item.ModifiedByUser);
            self.pageStatus = ko.observable(item.Status);
            self.url = ko.observable(item.PageUrl);
            self.version = ko.observable(item.Version);

            self.update = function(_item) {
                self.id(_item.Id);
                self.title(_item.Title);
                self.createdOn(globalization.created + ' ' + _item.CreatedOn);
                self.modifiedOn(globalization.lastEdited + ' ' + _item.ModifiedOn);
                self.modifiedByUser(globalization.lastEditedBy + ' ' + _item.ModifiedByUser);
                self.pageStatus(_item.Status);
                self.url(_item.PageUrl);
                self.version(_item.Version);
            };

            self.onEditClick = function(event) {
                var onSave = function(json) {
                    onAfterSiteSettingsBlogPostSaved(json, self);
                };
                editBlogPost(self.id(), {
                    postSuccess: onSave,
                    calledFromPage: false
                });
            };

            self.onTitleClick = function(event) {
                window.open(self.url());
            };

            self.onDeleteClick = function(event) {
                pages.deletePage(self.id(), function(json) {
                    blog.blogsGridViewModel.items.remove(self);
                }, globalization.deleteBlogDialogTitle);
            };
        }

        /**
        * Initializes site settings blogs list
        */
        function initializeSiteSettingsBlogsList(container, content, jsonData, isSearchResult, isClearFilterResult) {

            var form = container.find(selectors.siteSettingsBlogsListForm);

            blog.blogsGridViewModel = new BlogsGridViewModel(((content.Data) ? content.Data : jsonData), form, container);
            var gridDOM = container.find(selectors.siteSettingsBlogsGrid);
            ko.cleanNode(gridDOM.get(0));
            var pagingDOM = container.find(selectors.siteSettingsPager);
            ko.applyBindings(blog.blogsGridViewModel, gridDOM.get(0));
            ko.applyBindings(blog.blogsGridViewModel.paging, pagingDOM.get(0));

            grid.bindGridForm(form, function (html, data) {
                container.html(html);
                initializeSiteSettingsBlogsList(container, html, data);
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSiteSettingsBlogs(container, form, true);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.siteSettingsBlogsSearchInput), {
                preventedEnter: function () {
                    searchSiteSettingsBlogs(container, form, true);
                }
            });

            form.find(selectors.siteSettingsBlogTitleCell).on('click', function (event) {
                bcms.stopEventPropagation(event);
            });

            form.find(selectors.siteSettingsBlogsSearchButton).on('click', function () {
                var parent = $(this).parent();
                if (!parent.hasClass('bcms-active-search')) {
                    form.find(selectors.siteSettingsBlogsSearchInput).prop('disabled', false);
                    parent.addClass('bcms-active-search');
                    form.find(selectors.siteSettingsBlogsSearchInput).focus();
                } else {
                    form.find(selectors.siteSettingsBlogsSearchInput).prop('disabled', true);
                    parent.removeClass('bcms-active-search');
                    form.find(selectors.siteSettingsBlogsSearchInput).val('');
                }
            });

            form.find(selectors.siteSettingsButtonOpener).on('click', function (event) {
                bcms.stopEventPropagation(event);
                var holder = form.find(selectors.siteSettingsButtonHolder);
                if (!holder.hasClass('bcms-opened')) {
                    holder.addClass('bcms-opened');
                } else {
                    holder.removeClass('bcms-opened');
                }
            });

            bcms.on(bcms.events.bodyClick, function (event) {
                var holder = form.find(selectors.siteSettingsButtonHolder);
                if (holder.hasClass('bcms-opened')) {
                    holder.removeClass('bcms-opened');
                }
            });

            container.find(selectors.siteSettingsBlogCreateButton).on('click', function () {
                createBlogPost(function (json) {
                    blog.blogsGridViewModel.items.unshift(new BlogItemViewModel(json.Data));
                });
            });

            container.find(selectors.siteSettingsBlogExportButton).on('click', function () {
                var url = $.format("{0}?{1}", links.exportBlogPostsUrl, form.serialize());

                window.location.href = url;
            });

            container.find(selectors.siteSettingsBlogImportButton).on('click', function () {
                openImportBlogPostsForm(container, form);
            });

            if (isSearchResult) {
                form.find(selectors.siteSettingsBlogsSearchButton).parent().addClass('bcms-active-search');
            } else {
                form.find(selectors.siteSettingsBlogsSearchInput).prop('disabled', true);
            }

            filter.bind(container, ((content.Data) ? content.Data : jsonData), function (isClearFilterResult) {
                searchSiteSettingsBlogs(container, form, isSearchResult, isClearFilterResult);
            }, isClearFilterResult);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(container.find(selectors.siteSettingsBlogsSearchInput), true);
        }

        /**
        * Search site settings blogs
        */
        function searchSiteSettingsBlogs(container, form, usePaging) {
            if (usePaging) {
                grid.submitGridFormPaged(form, function(htmlContent, data) {
                    blog.blogsGridViewModel.setItems(data.Items);
                    blog.blogsGridViewModel.paging.setPaging(data.GridOptions.PageSize, data.GridOptions.PageNumber, data.GridOptions.TotalCount);
                });
            } else {
                grid.submitGridForm(form, function (htmlContent, data) {
                    blog.blogsGridViewModel.setItems(data.Items);
                    blog.blogsGridViewModel.paging.setPaging(data.GridOptions.PageSize, data.GridOptions.PageNumber, data.GridOptions.TotalCount);
                });
            }

        }

        /**
        * Loads a media manager view to the site settings container.
        */
        blog.loadSiteSettingsBlogs = function () {
            var tabs = [],
                onShow = function (container) {
                    var firstVisibleInputField = container.find('input[type=text],textarea,select').filter(':visible:first');
                    if (firstVisibleInputField) {
                        firstVisibleInputField.focus();
                    }
                };

            var blogs = new siteSettings.TabViewModel(globalization.blogPostsTabTitle, links.loadSiteSettingsBlogsUrl, initializeSiteSettingsBlogsList, onShow);
            tabs.push(blogs);

            if (security.IsAuthorized(["BcmsEditContent"])) {
                var authors = new siteSettings.TabViewModel(globalization.authorsTabTitle, links.loadAuthorsTemplateUrl, initializeSiteSettingsAuthorsList, onShow);
                var templates = new siteSettings.TabViewModel(globalization.templatesTabTitle, links.loadTemplatesUrl, initializeSiteSettingsTemplatesList, onShow);
                var blogSettings = new siteSettings.TabViewModel(globalization.settingsTabTitle, links.loadBlogPostSettingsUrl, initializeBlogPostSettingsList, onShow);
                tabs.push(authors);
                tabs.push(templates);
                tabs.push(blogSettings);
            }

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

                self.onBeforeAction = function () {
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

                self.name = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name }, activeDirectoryCompliant: "" });
                self.description = ko.observable();
                self.image = ko.observable(new AuthorImageViewModel(self));
                self.oldImageId = item.Image != null ? item.Image.ImageId : '';

                self.registerFields(self.name, self.image().id, self.image().url, self.image().thumbnailUrl, self.image().tooltip, self.description);

                self.name(item.Name);
                self.description(item.Description);
                self.image().setImage(item.Image || {});
            }

            AuthorViewModel.prototype.getDeleteConfirmationMessage = function () {
                return $.format(globalization.deleteAuthorDialogTitle, antiXss.encodeHtml(this.name()));
            };

            AuthorViewModel.prototype.getSaveParams = function () {
                var params = _super.prototype.getSaveParams.call(this);
                params.Name = this.name();
                params.Description = this.description();
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
        * Blog settings list view model
        */
        var SettingsListViewModel = (function (_super) {

            bcms.extendsClass(SettingsListViewModel, _super);

            function SettingsListViewModel(container, items, gridOptions) {
                _super.call(this, container, links.loadAuthorsUrl, items, gridOptions);
            }

            SettingsListViewModel.prototype.createItem = function (item) {
                var newItem = new SettingItemViewModel(this, item);
                return newItem;
            };

            return SettingsListViewModel;

        })(kogrid.ListViewModel);

        /**
        * Setting item view model
        */
        var SettingItemViewModel = (function (_super) {

            bcms.extendsClass(SettingItemViewModel, _super);

            function SettingItemViewModel(parent, item) {
                _super.call(this, parent, item);

                var self = this;

                self.name = ko.observable();
                self.key = ko.observable();
                self.value = ko.observable().extend({ required: "" });
                self.valueTitle = ko.observable();

                self.registerFields(self.value);

                self.contentEditModes = [];
                self.contentEditModes.push({
                    'id': 1,
                    'name': globalization.editModeHtmlTitle
                });
                self.contentEditModes.push({
                    'id': 2,
                    'name': globalization.editModeMarkdownTitle
                });

                self.value.subscribe(function (value) {
                    for (var i = 0; i < self.contentEditModes.length; i++) {
                        if (self.contentEditModes[i].id == value) {
                            self.valueTitle(self.contentEditModes[i].name);
                            break;
                        }
                    }
                });

                self.key(item.Key);
                self.name(item.Name);
                self.value(item.Value);
            }

            SettingItemViewModel.prototype.getSaveParams = function () {
                var params = _super.prototype.getSaveParams.call(this);
                params.Value = this.value();
                params.Key = this.key();

                return params;
            };

            SettingItemViewModel.prototype.onAfterItemSaved = function (json) {
                _super.prototype.onAfterItemSaved.call(this, json);

                // this.valueTitle(json.Data.ValueTitle);
            };

        SettingItemViewModel.prototype.getRowId = function() {
            if (!this.rowId) {
                this.rowId = 'bcms-blog-settings-row-' + rowId++;
            }
            return this.rowId;
        };
        return SettingItemViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Initializes blog post settings tab
        */
        function initializeSiteSettingsTemplatesList(container, json) {
            var html = json.Html,
                templates = (json.Success == true) ? json.Data : null;

            container.html(html);

            blog.templatesViewModel = new TemplatesListViewModel(templates, container);

            ko.applyBindings(blog.templatesViewModel, container.get(0));
        }

        /**
        * Initializes blog post settings tab
        */
        function initializeBlogPostSettingsList(container, json) {
            var html = json.Html,
                blogSettings = (json.Success == true && json.Data) ? json.Data.Items : null,
                gridOptions = (json.Success == true && json.Data) ? json.Data.GridOptions : {},
                model = new SettingsListViewModel(container, blogSettings, gridOptions);

            model.saveUrl = links.saveBlogPostSettingUrl;

            container.html(html);

            ko.applyBindings(model, container.get(0));
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
            self.displayedTemplates = ko.observableArray();
            self.searchQuery = ko.observable();
            self.searchEnabled = ko.observable(false);
            self.hasFocus = ko.observable(false);

            for (var j = 0; j < templates.length; j++) {
                var currentTemplate = new TemplateViewModel(templates[j], this, container);
                self.templates.push(currentTemplate);
            }

            self.displayTemplates = function () {
                if (self.templates() != null) {
                    self.displayedTemplates.removeAll();

                    var query = (self.searchQuery() || '').toLowerCase();

                    for (var j = 0; j < self.templates().length; j++) {
                        var currentTemplate = self.templates()[j];
                        if (query && currentTemplate.title.toLowerCase().indexOf(query) < 0) {
                            continue;
                        }
                        self.displayedTemplates.push(currentTemplate);
                    }
                }
            };

            self.search = function () {
                self.displayTemplates();
            };

            self.toggleSearch = function () {
                if (!self.searchEnabled()) {
                    self.searchEnabled(true);
                    self.hasFocus(true);
                } else {
                    self.searchEnabled(false);
                    self.searchQuery('');
                }
            };

            self.displayTemplates();
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
            self.container = container.closest(selectors.siteSettingsWindow).length > 0
                            ? container.closest(selectors.siteSettingsWindow)
                            : container;

            self.isActive = ko.observable(template.IsActive);
            self.isCompatible = template.IsCompatible;
            self.isMasterPage = template.IsMasterPage;

            self.select = function () {
                var url = self.isMasterPage
                            ? $.format(links.saveDefaultTemplateUrl, "00000000-0000-0000-0000-000000000000", self.id)
                            : $.format(links.saveDefaultTemplateUrl, self.id),
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
                    .done(function (result) {
                        onComplete(result);
                    })
                    .fail(function (response) {
                        onComplete(bcms.parseFailedResponse(response));
                    });
            };

            self.unselect = function () {
                self.isActive(false);
            };

            self.previewImage = function () {
                modal.imagePreview(self.previewUrl, self.title);
            };
        }

        /**
        * Called when content view model is created
        */
        function onContentModelCreated(contentViewModel) {
            if (contentViewModel.contentType == contentTypes.blogContent) {
                // Edit
                contentViewModel.onEditContent = function (onSuccess, includeChildRegions) {
                    var onSave = function (json) {
                        if ($.isFunction(onSuccess)) {
                            onSuccess(json);
                        } else {
                            redirect.ReloadWithAlert();
                        }
                    },
                        opts = {
                            postSuccess: onSave,
                            calledFromPage: true,
                            includeChildRegions: includeChildRegions
                        };

                    editBlogPost(bcms.pageId, opts);
                };

                contentViewModel.visibleButtons.configure = false;
                contentViewModel.visibleButtons["delete"] = false;
            }
        }

        /*
        * Import results view model 
        */
        function ImportResultViewModel(item) {
            var self = this;

            self.success = item.Success === true;
            self.checked = ko.observable(item.Success === true);
            self.id = item.Id;
            self.title = item.Title || '&nbsp;';
            self.url = item.PageUrl;
            self.errorMessage = item.ErrorMessage;
            self.warnMessage = item.WarnMessage;
            self.skipped = false;


            self.checked.subscribe(function (checked) {
                self.skipped = !checked;
            });
            self.toJson = function () {
                return {
                    Id: self.id,
                    Title: self.title,
                    PageUrl: self.url
                };
            };
        }

        /**
        * Updates import results
        */
        function updateImportResults(importModel, results) {
            var i, item;

            for (i = 0; i < results.length; i++) {
                item = results[i];
                importModel.results.push(new ImportResultViewModel(item));
            }
        }

        /*
        * Opens form for importing blog posts XML
        */
        function openImportBlogPostsForm(parentContainer, parentForm) {
            var importModel, i, item, form, contentWindow;

            modal.open({
                title: globalization.importBlogPostsTitle,
                acceptTitle: globalization.uploadButtonTitle,
                cancelTitle: globalization.closeButtonTitle,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, links.uploadBlogPostsImportFileUrl, {
                        contentAvailable: function (dialog, json) {
                            var iframe = dialog.container.find($(selectors.fileUploadingTarget)),
                                onLoadCallback = function () {
                                    importModel.started = false;
                                    form.hideLoading();
                                    var result = iframe.contents().find(selectors.fileUploadingResult).get(0);
                                    if (!result) {
                                        return true;
                                    }

                                    try {
                                        json = $.parseJSON(result.innerHTML);
                                        messages.refreshBox(form, json);

                                        if (json.Success) {
                                            importModel.uploaded(true);
                                            importModel.dialog.model.buttons()[0].title(globalization.importButtonTitle);

                                            if (json.Data && $.isArray(json.Data.Results)) {
                                                updateImportResults(importModel, json.Data.Results);
                                                importModel.fileId = json.Data.FileId;
                                            }
                                        }
                                    } catch (exc) {
                                        bcms.logger.error(exc);
                                    }
                                };

                            form = dialog.container.find(selectors.importBlogPostsForm);
                            contentWindow = dialog.container.find(selectors.siteSettingsBlogContentWindow);

                        importModel = {
                            createRedirects: ko.observable(true),
                            reuseExistingCategories: ko.observable(false),
                            recreateCategoryTree: ko.observable(true),
                            fileName: ko.observable(''),
                            fixedFileName: ko.observable(''),
                            messageBox: messages.box({ container: form }),
                            form: form,
                            results: ko.observableArray(),
                            uploaded: ko.observable(false),
                            finished: ko.observable(false),
                            dialog: dialog,
                            fileId: '',
                            checkedAll: ko.observable(true)
                        };
                        importModel.fileName.subscribe(function (fileName) {
                            if (fileName) {
                                fileName = fileName.toUpperCase().replace('C:\\FAKEPATH\\', '');
                            }
                            importModel.fixedFileName(fileName);
                        });
                        importModel.reuseExistingCategories.subscribe(function (reuseExistingCategories) {
                            importModel.recreateCategoryTree(!reuseExistingCategories);
                        });
                        importModel.recreateCategoryTree.subscribe(function (recreateCategoryTree) {
                            importModel.reuseExistingCategories(!recreateCategoryTree);
                        });
                        importModel.checkedAll.subscribe(function(checked) {
                            for (i = 0; i < importModel.results().length; i ++) {
                                item = importModel.results()[i];

                                    item.checked(checked);
                                }
                            });

                            iframe.on('load', onLoadCallback);

                            ko.applyBindings(importModel, form.get(0));
                        },
                    });
                },
                onAcceptClick: function () {
                    var params,
                        onImportComplete = function (json) {
                            var items = [],
                                j;

                            contentWindow.hideLoading();
                            messages.refreshBox(form, json);

                            if (json.Success == true) {
                                importModel.finished(true);
                                importModel.dialog.model.buttons()[0].disabled(true);

                                for (i = 0; i < importModel.results().length; i++) {
                                    item = importModel.results()[i];

                                    if (item.success && !item.skipped) {
                                        for (j = 0; j < json.Data.Results.length; j++) {
                                            if (json.Data.Results[j].PageUrl == item.url
                                                // When exception occurs before calculating URL
                                                || (item.id && (item.id == json.Data.Results[j].Id))) {
                                                item = new ImportResultViewModel(json.Data.Results[j]);
                                                break;
                                            }
                                        }
                                    }

                                    items.push(item);
                                }

                                importModel.results.removeAll();
                                for (i = 0; i < items.length; i++) {
                                    importModel.results.push(items[i]);
                                }
                            }
                        };

                    if (!importModel.uploaded()) {
                        // Upload file
                        if (!importModel.fileName()) {
                            importModel.messageBox.clearMessages();
                            importModel.messageBox.addErrorMessage(globalization.pleaseSelectAFile);
                        } else {
                            if (!importModel.started) {
                                contentWindow.showLoading();
                                importModel.started = true;
                                importModel.form.submit();
                                contentWindow.hideLoading();
                            }
                        }
                    } else {
                    params = {
                        BlogPosts: [],
                        CreateRedirects: importModel.createRedirects(),
                        ReuseExistingCategories: importModel.reuseExistingCategories(),
                        RecreateCategoryTree: importModel.recreateCategoryTree(),
                        FileId: importModel.fileId
                    };

                        // Start import
                        for (i = 0; i < importModel.results().length; i++) {
                            item = importModel.results()[i];
                            if (item.success && item.id && item.checked()) {
                                params.BlogPosts.push(item.toJson());
                            }
                        }

                        if (params.BlogPosts.length > 0) {
                            contentWindow.showLoading();
                            $.ajax({
                                type: 'POST',
                                contentType: 'application/json; charset=utf-8',
                                cache: false,
                                url: links.startImportUrl,
                                data: JSON.stringify(params)
                            })
                                .done(function (result) {
                                    onImportComplete(result);
                                })
                                .fail(function (response) {
                                    onImportComplete(bcms.parseFailedResponse(response));
                                });
                        } else {
                            importModel.messageBox.clearMessages();
                            importModel.messageBox.addWarningMessage(globalization.noBlogPostsSelectedToImport);
                        }
                    }
                },
                onCloseClick: function () {
                    if (importModel.finished()) {
                        searchSiteSettingsBlogs(parentContainer, parentForm);
                    } else if (importModel.uploaded() && importModel.fileId) {
                        // Call method for deleting uploaded file
                        $.ajax({
                            type: 'POST',
                            cache: false,
                            url: $.format(links.deleteUploadedFileUrl, importModel.fileId)
                        });
                    }
                }
            });
        }

        /**
        * Initializes blog module.
        */
        blog.init = function () {
            bcms.logger.debug('Initializing bcms.blog module.');
        };

        /**
        * Subscribe to events
        */
        bcms.on(bcms.events.contentModelCreated, onContentModelCreated);


        /**
        * Register initialization
        */
        bcms.registerInit(blog.init);

        return blog;
    });