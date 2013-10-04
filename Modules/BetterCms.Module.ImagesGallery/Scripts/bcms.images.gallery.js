/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */
bettercms.define('bcms.images.gallery', ['bcms.jquery', 'bcms', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.options', 'bcms.modal', 'bcms.media'],
    function ($, bcms, siteSettings, dynamicContent, ko, kogrid, options, modal, media) {
        'use strict';

        var gallery = {},
            selectors = {
                insertAlbumContainer: '.bcms-scroll-window',
                albumEditForm: 'form:first',
                albumFolder: '#bcms-edit-album-folder',
                albumTitle: '#bcms-edit-album-title'
            },
            links = {
                loadSiteSettingsAlbumsUrl: null,
                loadAlbumsUrl: null,
                editAlbumUrl: null,
                createAlbumUrl: null,
                deleteAlbumUrl: null,
                selectAlbumUrl: null
            },
            globalization = {
                deleteAlbumDialogTitle: null,
                selectAlbumDialogTitle: null,
                selectAlbumDialogAcceptButton: null,
                albumNotSelectedMessage: null
            },
            classes = {
                validationError: 'bcms-input-validation-error'
            };

        /**
        * Assign objects to module.
        */
        gallery.links = links;
        gallery.globalization = globalization;
        gallery.selectors = selectors;

        /**
        * Album folder selector view model
        */
        var AlbumFolderSelectorViewModel = (function (_super) {
            bcms.extendsClass(AlbumFolderSelectorViewModel, _super);
            
            function AlbumFolderSelectorViewModel(folder, parent) {
                _super.call(this, folder);

                this.parent = parent;
            }
            
            AlbumFolderSelectorViewModel.prototype.onAfterSelect = function () {
                if (!this.parent.title()) {
                    this.parent.title(this.title());
                }
            };

            return AlbumFolderSelectorViewModel;
        })(media.ImageFolderSelectorViewModel);

        /**
        * Called after album edit form was loaded
        */
        function initializeEditAlbumForm(dialog, content) {
            var form = dialog.container.find(selectors.albumEditForm),
                data = content.Data || {},
                coverImageViewModel = ko.observable(new media.ImageSelectorViewModel(data.CoverImage)),
                viewModel = {
                    form: form,
                    coverImage: coverImageViewModel,
                    
                    title: ko.observable(data.Title),
                    version: ko.observable(data.Version),
                    id: ko.observable(data.Id)
                };
            
            viewModel.folder = ko.observable(new AlbumFolderSelectorViewModel(data.Folder, viewModel));

            // Change image selector's default folder, when changing album folder
            viewModel.folder().id.subscribe(function (newFolderId) {
                var id = viewModel.coverImage().id();

                if (!id || bcms.isEmptyGuid(id)) {
                    viewModel.coverImage().currentFolder(newFolderId);
                    
                    if (viewModel.form.find(selectors.albumTitle).hasClass(classes.validationError)
                        || viewModel.form.find(selectors.albumFolder).hasClass(classes.validationError)) {
                        setTimeout(function() {
                            viewModel.form.valid();
                        }, 50);
                    }
                }
            });
            // Set current folder, whn cleatring an image
            viewModel.coverImage().id.subscribe(function (newImageId) {
                setTimeout(function() {
                    if (!newImageId || bcms.isEmptyGuid(newImageId)) {
                        viewModel.coverImage().currentFolder(viewModel.folder().id());
                    }
                }, 50);
            });

            ko.applyBindings(viewModel, form.get(0));

            return viewModel;
        }

        /**
        * Opens album edit modal window
        */
        function openAlbumEditForm(url, title, onAlbumSaved) {
            var viewModel;

            modal.open({
                title: title,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function(childDialog, content) {
                            viewModel = initializeEditAlbumForm(childDialog, content);
                        },

                        postSuccess: function (json) {
                            viewModel.id(json.Data.Id);
                            viewModel.version(json.Data.Version);
                            
                            onAlbumSaved.call(this, viewModel);
                        }
                    });
                }
            });
        }

        /**
        * Albums list view model
        */
        var AlbumsListViewModel = (function (_super) {

            bcms.extendsClass(AlbumsListViewModel, _super);

            function AlbumsListViewModel(container, items, gridOptions, isSelectable) {
                _super.call(this, container, links.loadAlbumsUrl, items, gridOptions);

                var self = this;

                self.isSelectable = isSelectable;
            }

            AlbumsListViewModel.prototype.createItem = function (item) {
                var newItem = new AlbumViewModel(this, item);
                
                return newItem;
            };

            AlbumsListViewModel.prototype.addNewItem = function () {
                var self = this,
                    onAlbumSaved = function (albumViewModel) {
                        var item = self.createItem({
                            Title: albumViewModel.title(),
                            Version: albumViewModel.version(),
                            Id: albumViewModel.id(),
                        });

                        self.items.unshift(item);
                    };

                openAlbumEditForm(links.createAlbumUrl, globalization.createAlbumDialogTitle, onAlbumSaved);
            };

            return AlbumsListViewModel;

        })(kogrid.ListViewModel);

        /**
        * Album view model
        */
        var AlbumViewModel = (function (_super) {

            bcms.extendsClass(AlbumViewModel, _super);

            function AlbumViewModel(parent, item) {
                _super.call(this, parent, item);

                var self = this;

                self.title = ko.observable().extend({ required: "", title: "", maxLength: { maxLength: ko.maxLength.name } });

                self.registerFields(self.title);

                self.title(item.Title);
            }

            AlbumViewModel.prototype.getDeleteConfirmationMessage = function () {
                return $.format(globalization.deleteAlbumDialogTitle, this.title());
            };

            AlbumViewModel.prototype.getSaveParams = function () {
                var params = _super.prototype.getSaveParams.call(this);
                params.Title = this.title();

                return params;
            };
            
            AlbumViewModel.prototype.openItem = function () {
                if (!this.parent.isSelectable) {
                    this.editItem();
                    return;
                }

                this.selectForInsert();
            };

            AlbumViewModel.prototype.editItem = function () {
                var self = this,
                    url = $.format(links.editAlbumUrl, self.id()),
                    onAlbumSaved = function (albumViewModel) {
                        self.version(albumViewModel.version());
                        self.title(albumViewModel.title());
                    };

                openAlbumEditForm(url, globalization.editAlbumDialogTitle, onAlbumSaved);
            };

            return AlbumViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Initializes loading of list of image gallery albums.
        */
        function initializeSiteSettingsAlbums(json, container, isSelectable) {
            var data = (json.Success == true) ? json.Data : {};

            var viewModel = new AlbumsListViewModel(container, data.Items, data.GridOptions, isSelectable);
            viewModel.deleteUrl = links.deleteAlbumUrl;

            ko.applyBindings(viewModel, container.get(0));

            // Select search.
            var firstVisibleInputField = container.find('input[type=text],textarea,select').filter(':visible:first');
            if (firstVisibleInputField) {
                firstVisibleInputField.focus();
            }

            return viewModel;
        }

        /**
        * Loads an image gallery albums list to the site settings container.
        */
        gallery.loadSiteSettingsAlbums = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsAlbumsUrl, {
                contentAvailable: function(content) {
                    initializeSiteSettingsAlbums(content, siteSettings.getMainContainer(), false);
                }
            });
        };

        /**
        * Called when user press browse button in the options grid with type = "Images gallery album".
        */
        function onExecuteImageGalleryAlbumOption(valueObservable, titleObservable, optionModel) {
            var viewModel;

            modal.open({
                title: globalization.selectAlbumDialogTitle,
                acceptTitle: globalization.selectAlbumDialogAcceptButton,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, links.selectAlbumUrl, {
                        done: function (content) {
                            var container = dialog.container.find(selectors.insertAlbumContainer);

                            viewModel = initializeSiteSettingsAlbums(content, container, true);
                        }
                    });
                },
                onAcceptClick: function () {
                    if (viewModel != null) {
                        var selectedItem = viewModel.getSelectedItem();

                        if (selectedItem == null) {
                            modal.info({
                                content: globalization.albumNotSelectedMessage,
                                disableCancel: true
                            });

                            return false;
                        } else {
                            valueObservable(selectedItem.id());
                            titleObservable(selectedItem.title());
                            
                            if (optionModel.key && !optionModel.key()) {
                                optionModel.key(selectedItem.title());
                            }

                            optionModel.hasFocus(true);
                        }
                    }

                    return true;
                }
            });
        }

        /**
        * Initializes images gallery module.
        */
        gallery.init = function () {
            bcms.logger.debug('Initializing bcms.images.gallery module.');

            options.registerCustomOption('images-gallery-album', onExecuteImageGalleryAlbumOption);
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(gallery.init);

        return gallery;
    });