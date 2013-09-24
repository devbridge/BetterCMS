/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms, console */
bettercms.define('bcms.images.gallery', ['bcms.jquery', 'bcms', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.options'],
    function ($, bcms, siteSettings, dynamicContent, ko, kogrid, options) {
        'use strict';

        var gallery = {},
            selectors = {},
            links = {
                loadSiteSettingsAlbumsUrl: null,
                loadAlbumsUrl: null,
                saveAlbumUrl: null,
                deleteAlbumUrl: null
            },
            globalization = {
                deleteAlbumDialogTitle: null
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

            function AlbumsListViewModel(container, items, gridOptions) {
                _super.call(this, container, links.loadAlbumsUrl, items, gridOptions);
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

            return AlbumViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Initializes loading of list of image gallery albums.
        */
        function initializeSiteSettingsAlbums(json) {
            var container = siteSettings.getMainContainer(),
                data = (json.Success == true) ? json.Data : {};

            var viewModel = new AlbumsListViewModel(container, data.Items, data.GridOptions);
            viewModel.deleteUrl = links.deleteAlbumUrl;
            viewModel.saveUrl = links.saveAlbumUrl;
            
            ko.applyBindings(viewModel, container.get(0));
        }

        /**
        * Loads an image gallery albums list to the site settings container.
        */
        gallery.loadSiteSettingsAlbums = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsAlbumsUrl, {
                contentAvailable: initializeSiteSettingsAlbums
            });
        };

        /**
        * Called when user press browse button in the options grid with type = "Images gallery album".
        */
        function onExecuteImageGalleryAlbumOption(editableValue) {
            editableValue('AAALLLBBUUUMM');
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