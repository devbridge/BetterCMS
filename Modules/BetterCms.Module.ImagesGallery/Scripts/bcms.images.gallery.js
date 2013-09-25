/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms, console */
bettercms.define('bcms.images.gallery', ['bcms.jquery', 'bcms', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.options', 'bcms.modal'],
    function ($, bcms, siteSettings, dynamicContent, ko, kogrid, options, modal) {
        'use strict';

        var gallery = {},
            selectors = {
                insertAlbumContainer: '.bcms-scroll-window'
            },
            links = {
                loadSiteSettingsAlbumsUrl: null,
                loadAlbumsUrl: null,
                saveAlbumUrl: null,
                deleteAlbumUrl: null,
                selectAlbumUrl: null
            },
            globalization = {
                deleteAlbumDialogTitle: null,
                selectAlbumDialogTitle: null,
                selectAlbumDialogAcceptButton: null,
                albumNotSelectedMessage: null
            };

        /**
        * Assign objects to module.
        */
        gallery.links = links;
        gallery.globalization = globalization;
        gallery.selectors = selectors;

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

            return AlbumViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Initializes loading of list of image gallery albums.
        */
        function initializeSiteSettingsAlbums(json, container, isSelectable) {
            var data = (json.Success == true) ? json.Data : {};

            var viewModel = new AlbumsListViewModel(container, data.Items, data.GridOptions, isSelectable);
            viewModel.deleteUrl = links.deleteAlbumUrl;
            viewModel.saveUrl = links.saveAlbumUrl;

            ko.applyBindings(viewModel, container.get(0));

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
        function onExecuteImageGalleryAlbumOption(valueObservable, titleObservable) {
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
            console.log('Initializing bcms.images.gallery module.');

            options.registerCustomOption('images-gallery-album', onExecuteImageGalleryAlbumOption);
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(gallery.init);

        return gallery;
    });