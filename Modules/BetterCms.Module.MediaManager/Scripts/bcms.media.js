/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.media', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'bcms.media.upload', 'bcms.media.imageeditor', 'bcms.htmlEditor', 'bcms.ko.extenders', 'bcms.contextMenu', 'bcms.security', 'bcms.media.history', 'bcms.media.fileeditor', 'bcms.tags', 'bcms.categories', 'bcms.options', 'bcms.store'],
function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, mediaUpload, imageEditor, htmlEditor, ko, menu, security, history, fileEditor, tags, categories, optionsModule, store) {
    'use strict';

    var media = {},
        selectors = {
            tabImagesContainer: '#bcms-tab-1',
            tabVideosContainer: '#bcms-tab-2',
            tabAudiosContainer: '#bcms-tab-3',
            tabFilesContainer: '#bcms-tab-4',

            tabImagesSelector: '#bcms-tab-images',
            tabVideosSelector: '#bcms-tab-videos',
            tabAudiosSelector: '#bcms-tab-audios',
            tabFilesSelector: '#bcms-tab-files',

            templateDataBind: '.bcms-data-bind-container',
            firstForm: 'form:first',
            spinContainer: '.bcms-rightcol:first',
            insertContentContainer: '.bcms-insert-content-modal:first',
            searchBox: '#bcms-search-input',
            fileListMessageBox: '#bcms-site-settings-media-messages-',

            previewBox: '#bcms-media-properties-preview'
        },
        links = {
            loadSiteSettingsMediaManagerUrl: null,
            loadImagesUrl: null,
            loadFilesUrl: null,
            loadAudiosUrl: null,
            loadVideosUrl: null,
            insertImageDialogUrl: null,
            insertFileDialogUrl: null,
            deleteAudioUrl: null,
            deleteVideoUrl: null,
            deleteImageUrl: null,
            deleteFileUrl: null,
            getImageUrl: null,
            saveFolderUrl: null,
            renameMediaUrl: null,
            getMediaUrl: null,
            getFileUrl: null,
            downloadFileUrl: null,
            unarchiveMediaUrl: null,
            archiveMediaUrl: null
        },
        globalization = {
            deleteImageConfirmMessage: null,
            deleteAudioConfirmMessage: null,
            deleteVideoConfirmMessage: null,
            deleteFileConfirmMessage: null,
            deleteFolderConfirmMessage: null,

            archiveMediaConfirmMessage: null,
            unarchiveMediaConfirmMessage: null,
            archiveVideoConfirmMessage: null,
            unarchiveVideoConfirmMessage: null,
            archiveFileConfirmMessage: null,
            unarchiveFileConfirmMessage: null,
            archiveImageConfirmMessage: null,
            unarchiveImageConfirmMessage: null,

            insertImageDialogTitle: null,
            insertImageFailureMessageTitle: null,
            insertImageFailureMessageMessage: null,
            imageNotSelectedMessageMessage: null,
            insertImageInsertButtonTitle: null,

            selectFolderDialogTitle: null,
            selectFolderFailureMessageTitle: null,
            selectFolderFailureMessageMessage: null,
            selectFolderSelectButtonTitle: null,
            rootFolderTitle: null,

            insertFileFailureMessageTitle: null,
            insertFileFailureMessageMessage: null,
            insertFileDialogTitle: null,
            fileNotSelectedMessageMessage: null,

            searchedInPathPrefix: null,
            noResultFoundMessage: null,

            imagesTabTitle: null,
            audiosTabTitle: null,
            videosTabTitle: null,
            filesTabTitle: null,

            uploadImage: null,
            uploadAudio: null,
            uploadVideo: null,
            uploadFile: null
        },
        keys = {
            folderViewMode: 'bcms.mediaFolderViewMode'
        },
        mediaTypes = {
            image: 1,
            video: 2,
            audio: 3,
            file: 4
        },
        classes = {
            customImageLeftAlign: 'content-image-left',
            customImageRightAlign: 'content-image-right'
        },
        contentTypes = {
            file: 1,
            folder: 2
        },
        sortDirections = {
            ascending: 0,
            descending: 1
        },
        fileExtensions = {
            officeFiles: '|thmx|pdf|txt|doc|dot|docx|dotx|dotm|docm|'
                + 'xls|xlt|xlm|xlsx|xlsm|xltx|xltm|xlsb|xla|xlam|xll|xlw|'
                + 'ppt|pptx|pptm|potx|potm|ppam|ppsx|ppsm|sldx|sldm|csv|txt|rtf|msg|',
            archiveFiles: '|rar|zip|7z|tar|gz|bz2|ace|arc|arj|cab|pak|zoo|',
            audioFiles: '|mp3|wav|aiif|aac|flac|m4a|m4p|ogg|ra|vox|wma|',
            imageFiles: '|gif|bmp|ico|jpg|jpeg|png|tif|tiff|raw|psd|svg|ai|cdr|',
            videoFiles: '|mp4|mp2v|mp4v|mpe|mpeg|mpg|mpg2|mts|avi|3gp|mov|wmv|rm|asf|flv|flc|m2t|m2v|m4v|ogv|ogx|swf|vob|xfl|'
        },
        imagesViewModel = null,
        audiosViewModel = null,
        videosViewModel = null,
        filesViewModel = null,
        staticDomId = 1,
        contentEditor = null,
        imageInsertDialog = null,
        fileInsertDialog = null,
        blurTimer = null,
        events = {
            mediaListViewModeChanged: 'bcms-media-list-view-mode-changed',
        };

    /**
    * Assign objects to module.
    */
    media.links = links;
    media.globalization = globalization;
    media.events = events;

    /**
    * Media's current folder sort / search / paging option view model
    */
    function MediaItemsOptionsViewModel(onOpenPage, folderType) {
        var self = this;

        var categorizableItemKey = folderType == mediaTypes.image ? 'Images' : 'Files';

        self.searchQuery = ko.observable();
        self.includeArchived = ko.observable(false);
        self.isFilterVisible = ko.observable(false);
        self.column = ko.observable();
        self.isDescending = ko.observable(false);
        self.tags = new tags.TagsListViewModel();
        self.categories = new categories.CategoriesListViewModel(null, categorizableItemKey),
        self.isEdited = ko.computed(function () {
            if (self.includeArchived()) {
                return true;
            }
            if (self.tags != null && self.tags.items() != null && self.tags.items().length > 0) {
                return true;
            }
            if (self.categories != null && self.categories.items() != null && self.categories.items().length > 0) {
                return true;
            }
            return false;
        });

        if (!self.paging) {
            self.paging = new ko.PagingViewModel(0, 1, 0, onOpenPage);
        }

        self.clearFilter = function () {
            self.includeArchived(false);
            self.tags.removeAll();
            self.categories.removeAll();
        };

        self.toggleFilter = function () {
            self.isFilterVisible(!self.isFilterVisible());
        };

        self.closeFilter = function () {
            self.isFilterVisible(false);
        };

        self.changeIncludeArchived = function () {
            self.includeArchived(!(self.includeArchived()));
        };

        self.fromJson = function (options) {
            self.searchQuery(options.GridOptions.SearchQuery);
            self.column(options.GridOptions.Column);
            self.isDescending(options.GridOptions.Direction == sortDirections.descending);

            self.paging.setPaging(options.GridOptions.PageSize, options.GridOptions.PageNumber, options.TotalCount);

            self.tags.applyItemList(options.GridOptions.Tags);
            self.categories.applyItemList(options.GridOptions.Categories);
        };

        self.isSearchEmpty = function () {
            return self.searchQuery() == null || self.searchQuery() == '';
        };
    }

    /**
    * Current media's preview view model
    */
    function MediaItemPreviewViewModel() {
        var self = this,
            maxHeight = 250,
            maxWidth = 400,
            thumbnailWidth = 150,
            thumbnailHeight = 150;

        self.dimensions = ko.observable();
        self.size = ko.observable();
        self.imageUrl = ko.observable();
        self.previewUrl = ko.observable();
        self.imageAlt = ko.observable();
        self.top = ko.observable();
        self.left = ko.observable();
        self.width = ko.observable();
        self.height = ko.observable();
        self.containerWidth = ko.observable();
        self.containerHeight = ko.observable();

        self.lastX = null;
        self.lastY = null;

        function setCoords(clientX, clientY) {
            if (!clientX) {
                clientX = self.lastX;
            }
            if (!clientY) {
                clientY = self.lastY;
            }
            if (!clientX || !clientY) {
                return;
            }

            var top = clientY,
                previewHeight = $(selectors.previewBox).outerHeight();

            if (top > $(window).height() - maxHeight || top > $(window).height() - previewHeight) {
                top -= previewHeight;
            }
            self.top(top + 10 + "px");
            self.left(clientX + 10 + "px");

            self.lastX = clientX;
            self.lastY = clientY;
        }

        self.setItem = function (item) {
            var src = item.publicUrl();

            self.imageUrl(src + (src.indexOf('?') != -1 ? '&' : '?') + (new Date()).getTime());
            self.previewUrl(item.thumbnailUrl());

            self.dimensions(item.width + ' x ' + item.height);
                
            var dimensions = imageEditor.calculateImageDimensionsToFit(item.width, item.height, maxWidth, maxHeight);
            self.containerWidth(parseInt(dimensions.width) + 'px');
            self.containerHeight(parseInt(dimensions.height) + 'px');
                
            dimensions = imageEditor.calculateImageDimensionsToFit(thumbnailWidth, thumbnailHeight, dimensions.width, dimensions.height, true);
            self.width(parseInt(dimensions.width) + 'px');
            self.height(parseInt(dimensions.height) + 'px');

            self.size(item.sizeText);
            self.imageAlt(item.tooltip());
        };

        self.onImageLoad = function () {
            self.previewUrl(self.imageUrl());
            self.width('');
            self.height('');
                
            setCoords();
        };

        self.clearItem = function () {
            self.imageUrl('');
            self.imageAlt('');
        };

        self.setCoords = function (clientX, clientY) {
            setCoords(clientX, clientY);
        };

        return self;
    }

    /**
    * Media's current folder view model
    */
    function MediaItemsViewModel(container, url, messagesContainer) {
        var self = this,
            onUploadFiles = function (newFilesData) {
                if (newFilesData && newFilesData.Data) {

                    // Do not add files, uploaded to different folder
                    var currentFolder = self.path().currentFolder().id();
                    var uploadedFolder = newFilesData.Data.SelectedFolderId;

                    if (currentFolder == uploadedFolder && newFilesData.Data.Medias && newFilesData.Data.Medias.length > 0) {
                        var medias = newFilesData.Data.Medias;
                        for (var i = medias.length - 1; i >= 0; i--) {
                            var mediaItem = convertToMediaModel(medias[i]);
                            self.medias.unshift(mediaItem);

                            // Replace unobtrusive validator
                            bcms.updateFormValidator(self.container.find(selectors.firstForm));
                        }
                    }
                }
            },
            previewTimer;

        self.container = container;
        self.messagesContainer = messagesContainer;
        self.url = url;
        self.onMediaSelect = null;

        self.medias = ko.observableArray();
        self.path = ko.observable();
        self.isGrid = ko.observable(store.get(keys.folderViewMode) == 1);
        self.canSelectMedia = ko.observable(false);
        self.canInsertMedia = ko.observable(false);
        self.canInsertMediaWithOptions = ko.observable(false);
        self.searchInHistory = false;
        self.canSearchInHistory = ko.observable(false);
        
        self.showPropertiesPreview = ko.observable(false);
        self.previewItem = new MediaItemPreviewViewModel();

        self.rowAdded = false;

        self.gridOptions = ko.observable();

        self.isRootFolder = function () {
            if (self.path != null && self.path().folders != null && self.path().folders().length > 1) {
                return false;
            }
            return true;
        };

        self.showNoDataInfoDiv = function () {
            return ((self.isRootFolder() || self.isSearchResults()) && self.medias().length == 0);
        };

        self.isSearchResults = ko.observable(false);
        self.noSearchResultFound = ko.observable('');

        self.addNewFolder = function () {
            if (!self.rowAdded) {
                self.rowAdded = true;

                var newFolder = new MediaFolderViewModel({
                    IsActive: true,
                    Type: self.path().currentFolder().type,
                    ParentFolderId: self.path().currentFolder().id()
                });

                self.medias.unshift(newFolder);

                // Replace unobtrusive validator
                bcms.updateFormValidator(self.container.find(selectors.firstForm));
            }
        };

        self.uploadMedia = function () {
            mediaUpload.openUploadFilesDialog(self.path().currentFolder().id(), self.path().currentFolder().type, onUploadFiles);
            messages.refreshBox($(selectors.fileListMessageBox + self.path().type), {});
        };

        self.reuploadMedia = function (item) {
            if (item.isProcessing() || item.isDeleting()) {
                return;
            }

            mediaUpload.openReuploadFilesDialog(item.id(), self.path().currentFolder().id(), self.path().currentFolder().type, function (filesData) {
                if (filesData && filesData.Data && filesData.Data.Medias && filesData.Data.Medias.length > 0) {
                    var mediaItem = convertToMediaModel(filesData.Data.Medias[0]);
                    if (self.path().currentFolder().type == mediaTypes.file) {
                        mediaItem.thumbnailUrl(item.thumbnailUrl());
                        mediaItem.tooltip(item.tooltip());
                    }
                    var index = $.inArray(item, self.medias());
                    self.medias.splice(index, 1, mediaItem);
                    // Replace unobtrusive validator.
                    bcms.updateFormValidator(self.container.find(selectors.firstForm));
                }
            });
            messages.refreshBox($(selectors.fileListMessageBox + self.path().type), {});
        };

        self.searchMedia = function () {
            self.gridOptions().paging.pageNumber(1);
            self.gridOptions().closeFilter();
            self.loadMedia();
        };

        self.searchWithFilter = function (searchInHistory) {
            if (searchInHistory) {
                self.searchInHistory = true;
            }
            self.searchMedia();
        };

        self.clearFilter = function () {
            self.gridOptions().clearFilter();
            self.searchMedia();
        };

        self.onOpenPage = function () {
            self.loadMedia();
        };

        self.sortMedia = function (column) {
            var columnBefore = self.gridOptions().column(),
                wasDescending = self.gridOptions().isDescending();
            if (columnBefore == column) {
                self.gridOptions().isDescending(!wasDescending);
            } else {
                self.gridOptions().isDescending(false);
            }
            self.gridOptions().column(column);
            var params = createFolderParams(self.path().currentFolder().id(), self),
                onComplete = function (json) {
                    parseJsonResults(json, self);
                };
            loadTabData(self, params, onComplete);
        };

        self.switchViewStyle = function () {
            var isGrid = !self.isGrid();
            store.set(keys.folderViewMode, isGrid ? 1 : 0);
            bcms.trigger(media.events.mediaListViewModeChanged, isGrid);
        };

        self.isSortedAscending = function (column) {
            if (column == self.gridOptions().column() && !self.gridOptions().isDescending()) {
                return true;
            }
            return false;
        };

        self.isSortedDescending = function (column) {
            if (column == self.gridOptions().column() && self.gridOptions().isDescending()) {
                return true;
            }
            return false;
        };

        self.uploadButtonTitle = function () {
            switch (self.path().type) {
                case mediaTypes.image:
                    return globalization.uploadImage;
                case mediaTypes.audio:
                    return globalization.uploadAudio;
                case mediaTypes.video:
                    return globalization.uploadVideo;
                case mediaTypes.file:
                    return globalization.uploadFile;
            }
            return null;
        };

        self.openRoot = function () {
            changeFolder(null, self);
        };

        self.openParent = function () {
            changeFolder(self.path().currentFolder().parentFolderId(), self);
        };

        self.domId = function () {
            return 'bcms-site-settings-media-messages-' + self.path().type;
        };

        self.loadMedia = function () {
            var params = createFolderParams(self.path().currentFolder().id(), self),
                onComplete = function (json) {
                    parseJsonResults(json, self);
                    $(selectors.searchBox).focus();
                };
            params.SearchInHistory = self.searchInHistory;
            loadTabData(self, params, onComplete);
        };

        self.unselectAllMedias = function (keepSelected) {
            var item, i, l;

            for (i = 0, l = self.medias().length; i < l; i++) {
                item = self.medias()[i];
                
                item.isSelected(item == keepSelected);
            }
        };

        self.movePreview = function (data, event) {
            var showProperties = self.showPropertiesPreview(),
                clientX = event.clientX,
                clientY = event.clientY;
            
            if (menu.isVisible || !data.isImage()) {
                if (showProperties) {
                    self.showPropertiesPreview(false);
                }
                return;
            }

            if (!showProperties) {
                if (previewTimer != null) {
                    clearTimeout(previewTimer);
                }

                previewTimer = setTimeout(function() {
                    showPreview(data, clientX, clientY);
                }, 300);
            } else {
                self.previewItem.setCoords(clientX, clientY);
            }
        };

        self.isImage = function () {
            if (self.path().type == mediaTypes.image)
                return true;
            return false;
        };

        function showPreview(data, clientX, clientY) {
            if (menu.isVisible || !data.isImage()) {
                return;
            }

            self.previewItem.setItem(data);
            self.previewItem.setCoords(clientX, clientY);
            self.showPropertiesPreview(true);
        };
        
        self.hidePreview = function () {
            if (previewTimer != null) {
                clearTimeout(previewTimer);
            }
            
            self.showPropertiesPreview(false);
            self.previewItem.clearItem();
        };

        bcms.on(media.events.mediaListViewModeChanged, function(currentMode) {
            self.isGrid(currentMode);
        });
    }

    /**
    * Media path view model
    */
    function MediaPathViewModel(type) {
        var self = this;

        self.isSearchResults = ko.observable(false);
        self.currentFolder = ko.observable();
        self.folders = ko.observableArray();
        self.type = type;

        self.pathFolders = ko.computed(function () {
            var range = self.folders(),
                prefix = self.isSearchResults() ? globalization.searchedInPathPrefix + ' ' : '';
            if (range.length > 0) {
                switch (self.type) {
                    case mediaTypes.image:
                        range[0].name(prefix + globalization.imagesTabTitle);
                        break;
                    case mediaTypes.audio:
                        range[0].name(prefix + globalization.audiosTabTitle);
                        break;
                    case mediaTypes.video:
                        range[0].name(prefix + globalization.videosTabTitle);
                        break;
                    case mediaTypes.file:
                        range[0].name(prefix + globalization.filesTabTitle);
                        break;
                }
            }
            return range;
        });
    }

    /**
    * Media item context menu view model
    */
    function MediaItemContextMenuViewModel() {
        var self = this;

        self.initialized = false;
        self.domId = 'cmenu_' + staticDomId++;

        self.show = function (data, event) {
            var menuContainer = $('#' + self.domId);

            if (!self.initialized) {
                self.initialized = true;
                menu.initContext(menuContainer, event.target, false);
            }

            menu.contextShow(event, menuContainer);
        };

        self.close = function (data, event) {
            menu.closeContext();
        };
    }

    /**
    * Media item base view model
    */
    var MediaItemBaseViewModel = (function () {
        function MediaItemBaseViewModel(item) {
            var self = this;

            self.id = ko.observable(item.Id);
            self.name = ko.observable(item.Name);
            self.oldName = item.Name;
            self.version = ko.observable(item.Version);
            self.publicUrl = ko.observable(item.PublicUrl);
            self.extension = item.FileExtension;
            self.type = item.Type;
            self.nameDomId = 'name_' + staticDomId++;
            self.updateUrl = links.renameMediaUrl;
            self.isProcessing = ko.observable(item.IsProcessing || false);
            self.isFailed = ko.observable(item.IsFailed || false);
            self.savePressed = false;
            self.isArchived = ko.observable(item.IsArchived);

            self.isActive = ko.observable(item.IsActive || false);
            self.isSelected = ko.observable(false);
            self.isDeleting = ko.observable(false);

            self.parentFolderId = ko.observable(item.ParentFolderId);
            self.parentFolderName = ko.observable(item.ParentFolderName);

            self.tooltip = ko.observable(item.Tooltip);
            self.thumbnailUrl = ko.observable(item.ThumbnailUrl);
            self.isReadOnly = ko.observable(item.IsReadOnly);

            self.getImageUrl = function () {
                if (!self.publicUrl()) {
                    return null;
                }
                return self.publicUrl();
            };

            self.getImageThumbnailUrl = function() {
                if (!self.thumbnailUrl()) {
                    return null;
                }
                return self.thumbnailUrl();
            }

            self.contextMenu = new MediaItemContextMenuViewModel();

            self.isFile = function () {
                return !self.isFolder();
            };

            self.canBeDownloaded = function () {
                return !!self.publicUrl();
            };

            self.stopEvent = function (data, event) {
                bcms.stopEventPropagation(event);
            };

            self.downloadMedia = function () {
                window.open($.format(links.downloadFileUrl, self.id()), '_newtab');
            };

            self.rowClassNames = ko.computed(function () {
                var rowClasses = '';
                if (self.isFolder()) {
                    rowClasses += ' bcms-folder-box';
                    if (self.isActive()) {
                        rowClasses += ' bcms-folder-box-active';
                    }
                }
                if (self.isFile() && !self.isImage()) {
                    rowClasses += ' bcms-file-box';
                }
                if (self.isImage()) {
                    rowClasses += ' bcms-image-box';
                }
                if (self.isFile() && self.isActive()) {
                    if (!self.isImage()) {
                        rowClasses += ' bcms-file-box-active';
                    } else {
                        rowClasses += ' bcms-image-box-active';
                    }
                }
                if (self.isSelected()) {
                    rowClasses += ' bcms-media-click-active';
                }
                return $.trim(rowClasses);
            });

            self.iconClassNames = ko.computed(function () {
                var iconClasses = '';

                if (self.isFolder()) {
                    iconClasses += ' bcms-system-folder';
                }
                if (self.isImage()) {
                    iconClasses += ' bcms-media-file-holder';
                }
                if (self.isFile() && !self.isImage()) {
                    iconClasses += ' bcms-system-file';
                }
                if (!self.isImage() && self.extension) {
                    iconClasses += getFileExtensionCssClassName(self.extension);
                }

                return $.trim(iconClasses);
            });

            self.archiveMedia = function (folderViewModel, data, event) {
                bcms.stopEventPropagation(event);

                var url = $.format(links.archiveMediaUrl, this.id(), this.version()),
                    message = $.format(self.getArchiveMediaConfirmationMessage(), this.name());

                archiveUnarchiveMediaItem(true, url, message, folderViewModel, this);
            };

            self.unarchiveMedia = function (folderViewModel, data, event) {
                bcms.stopEventPropagation(event);

                var url = $.format(links.unarchiveMediaUrl, this.id(), this.version()),
                    message = $.format(self.getUnarchiveMediaConfirmationMessage(), this.name());

                archiveUnarchiveMediaItem(false, url, message, folderViewModel, this);
            };

            self.showParentLink = function (mediaItemsViewModel, data) {
                return mediaItemsViewModel.isSearchResults();
            };

            self.openParent = function (mediaItemsViewModel, data) {
                changeFolder(self.parentFolderId(), mediaItemsViewModel);
            };

            self.selectThis = function (mediaItemsViewModel, element) {
                $(element).select();
            };
        }

        MediaItemBaseViewModel.prototype.isFolder = function () {
            return false;
        };

        MediaItemBaseViewModel.prototype.isImage = function () {
            return false;
        };

        MediaItemBaseViewModel.prototype.deleteMedia = function (folderViewModel) {
            throw new Error("Delete method is not implemented.");
        };

        MediaItemBaseViewModel.prototype.getArchiveMediaConfirmationMessage = function () {
            return globalization.archiveMediaConfirmMessage;
        };

        MediaItemBaseViewModel.prototype.getUnarchiveMediaConfirmationMessage = function () {
            return globalization.unarchiveMediaConfirmMessage;
        };

        MediaItemBaseViewModel.prototype.editMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            if (this.isDeleting()) {
                return;
            }
            this.renameMedia(folderViewModel, data, event);
        };

        MediaItemBaseViewModel.prototype.renameMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            if (this.isDeleting()) {
                return;
            }
            this.isActive(true);
        };

        MediaItemBaseViewModel.prototype.reuploadMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            if (this.isDeleting()) {
                return;
            }
            folderViewModel.reuploadMedia(this);
        };

        MediaItemBaseViewModel.prototype.openMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            editOrSelectMedia(folderViewModel, this, data, event);
            messages.refreshBox($(selectors.fileListMessageBox + this.type), {});
        };

        MediaItemBaseViewModel.prototype.onIconClicked = function (folderViewModel, data, event) {
            this.openMedia(folderViewModel, data, event);
        };

        MediaItemBaseViewModel.prototype.onTitleClicked = function (folderViewModel, data, event) {
            this.openMedia(folderViewModel, data, event);
        };

        MediaItemBaseViewModel.prototype.saveMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            this.savePressed = true;
            saveMedia(folderViewModel, this);
        };

        MediaItemBaseViewModel.prototype.blurMediaField = function (folderViewModel) {
            cancelOrSaveMedia(folderViewModel, this);
        };

        MediaItemBaseViewModel.prototype.insertMedia = function (folderViewModel) {
            throw new Error("Insert media method is not implemented.");
        };

        MediaItemBaseViewModel.prototype.insertMediaWithOptions = function (folderViewModel) {
            throw new Error("Insert media with options method is not implemented.");
        };

        MediaItemBaseViewModel.prototype.cancelEditMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            cancelEditMedia(folderViewModel, this);
        };

        MediaItemBaseViewModel.prototype.toJson = function () {
            var params = {
                Id: this.id(),
                Name: this.name(),
                Version: this.version(),
                Type: this.type
            };
            return params;
        };

        MediaItemBaseViewModel.prototype.showHistory = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            if (this.isDeleting()) {
                return;
            }
            history.openMediaHistoryDialog(this.id(), folderViewModel.isImage(), function () {
                folderViewModel.searchMedia();
            });
        };

        return MediaItemBaseViewModel;
    })();

    /**
    * Media image view model
    */
    var MediaImageViewModel = (function (_super) {
        bcms.extendsClass(MediaImageViewModel, _super);

        function MediaImageViewModel(item) {
            _super.call(this, item);

            var self = this;

            self.width = item.Width;
            self.height = item.Height;
            self.sizeText = item.SizeText;

            self.previewImage = function () {
                var previewUrl = self.publicUrl();
                if (previewUrl) {
                    modal.imagePreview(previewUrl, self.tooltip);
                }
            };
        }

        MediaImageViewModel.prototype.isImage = function () {
            return true;
        };

        MediaImageViewModel.prototype.deleteMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);

            var url = $.format(links.deleteImageUrl, this.id(), this.version()),
                message = $.format(globalization.deleteImageConfirmMessage, this.name());

            deleteMediaItem(url, message, folderViewModel, this);
        };

        MediaImageViewModel.prototype.getArchiveMediaConfirmationMessage = function () {
            return globalization.archiveImageConfirmMessage;
        };

        MediaImageViewModel.prototype.getUnarchiveMediaConfirmationMessage = function () {
            return globalization.unarchiveImageConfirmMessage;
        };

        MediaImageViewModel.prototype.editMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            if (this.isDeleting()) {
                return;
            }

            if (security.IsAuthorized(["BcmsEditContent"])) {
                var self = this;
                imageEditor.onEditImage(self.id(), function (json) {
                    // Accepts json from reupload and/or from save properties
                    self.version(json.Version);
                    self.tooltip(json.Caption || json.Tooltip);
                    self.thumbnailUrl(json.ThumbnailUrl);
                    self.publicUrl(json.Url || json.PublicUrl);
                    self.name(json.Title || json.Name);
                    self.oldName = json.Title || json.Name;
                    self.sizeText = json.FileSize || json.SizeText;
                    self.width = json.CroppedWidth || json.Width;
                    self.height = json.CroppedHeight || json.Height;
                });
            }
        };

        MediaImageViewModel.prototype.insertMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);

            insertImage(this, false, folderViewModel.onMediaSelect);
        };

        MediaImageViewModel.prototype.insertMediaWithOptions = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);

            insertImage(this, true, folderViewModel.onMediaSelect);
        };

        return MediaImageViewModel;
    })(MediaItemBaseViewModel);

    /**
    * Media audio view model
    */
    var MediaAudioViewModel = (function (_super) {
        bcms.extendsClass(MediaAudioViewModel, _super);

        function MediaAudioViewModel(item) {
            _super.call(this, item);
        }

        MediaAudioViewModel.prototype.deleteMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);

            var url = $.format(links.deleteAudioUrl, this.id(), this.version()),
                message = $.format(globalization.deleteAudioConfirmMessage, this.name());

            deleteMediaItem(url, message, folderViewModel, this);
        };

        return MediaAudioViewModel;
    })(MediaItemBaseViewModel);

    /**
    * Media video view model
    */
    var MediaVideoViewModel = (function (_super) {
        bcms.extendsClass(MediaVideoViewModel, _super);

        function MediaVideoViewModel(item) {
            _super.call(this, item);
        }

        MediaVideoViewModel.prototype.deleteMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);

            var url = $.format(links.deleteVideoUrl, this.id(), this.version()),
                message = $.format(globalization.deleteVideoConfirmMessage, this.name());

            deleteMediaItem(url, message, folderViewModel, this);
        };

        MediaVideoViewModel.prototype.getArchiveMediaConfirmationMessage = function () {
            return globalization.archiveVideoConfirmMessage;
        };

        MediaVideoViewModel.prototype.getUnarchiveMediaConfirmationMessage = function () {
            return globalization.unarchiveVideoConfirmMessage;
        };

        return MediaVideoViewModel;
    })(MediaItemBaseViewModel);

    /**
    * File view model
    */
    var MediaFileViewModel = (function (_super) {
        bcms.extendsClass(MediaFileViewModel, _super);

        function MediaFileViewModel(item) {
            _super.call(this, item);
        }

        MediaFileViewModel.prototype.deleteMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);

            var url = $.format(links.deleteFileUrl, this.id(), this.version()),
                message = $.format(globalization.deleteFileConfirmMessage, this.name());

            deleteMediaItem(url, message, folderViewModel, this);
        };

        MediaFileViewModel.prototype.insertMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            insertFile(this);
        };

        MediaFileViewModel.prototype.editMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            if (this.isDeleting()) {
                return;
            }

            if (security.IsAuthorized(["BcmsEditContent"])) {
                var self = this;
                fileEditor.onEditFile(self.id(), function (json) {
                    // Accepts json from reupload and/or from save properties
                    self.version(json.Version);
                    self.name(json.Title || json.Name);
                    self.oldName = json.Title || json.Name;
                    if (json.Image) {
                        self.tooltip(json.Image.ImageTooltip);
                        self.thumbnailUrl(json.Image.ThumbnailUrl);
                    } else {
                        self.thumbnailUrl(json.ThumbnailUrl);
                        self.tooltip(json.Tooltip);
                    }
                });
            }
        };

        MediaFileViewModel.prototype.getArchiveMediaConfirmationMessage = function () {
            return globalization.archiveFileConfirmMessage;
        };

        MediaFileViewModel.prototype.getUnarchiveMediaConfirmationMessage = function () {
            return globalization.unarchiveFileConfirmMessage;
        };

        return MediaFileViewModel;
    })(MediaItemBaseViewModel);

    /**
    * Media folder view model
    */
    var MediaFolderViewModel = (function (_super) {
        bcms.extendsClass(MediaFolderViewModel, _super);

        function MediaFolderViewModel(item) {
            _super.call(this, item);

            var self = this;
            self.updateUrl = links.saveFolderUrl;

            self.pathName = ko.computed(function () {
                return self.name() + '/';
            });
        }

        MediaFolderViewModel.prototype.isFolder = function () {
            return true;
        };

        MediaFolderViewModel.prototype.deleteMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);

            var url = $.format(links.deleteFolderUrl, this.id(), this.version()),
                message = $.format(globalization.deleteFolderConfirmMessage, this.name());

            deleteMediaItem(url, message, folderViewModel, this);
        };

        MediaFolderViewModel.prototype.openMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            if (this.isDeleting()) {
                return;
            }

            changeFolder(this.id(), folderViewModel);
        };

        MediaFolderViewModel.prototype.toJson = function () {
            var params = {
                Id: this.id(),
                Name: this.name(),
                Version: this.version(),
                Type: this.type,
                ParentFolderId: this.parentFolderId()
            };
            return params;
        };

        return MediaFolderViewModel;
    })(MediaItemBaseViewModel);

    /**
    * Selects an item by id
    */
    function selectItemById(folderViewModel, selectedId) {
        var i, l, item;

        if (!selectedId || bcms.isEmptyGuid(selectedId)) {
            return;
        }

        for (i = 0, l = folderViewModel.medias().length; i < l; i++) {
            item = folderViewModel.medias()[i];

            if (item.id() == selectedId) {
                item.isSelected(true);
            }
        }
    }

    /**
    * Opens media for selecting or editing.
    */
    function editOrSelectMedia(folderViewModel, item, data, event) {
        if (item.isProcessing() || item.isFailed() || item.isDeleting()) {
            return;
        }

        // Call open, if view model is not in selectable mode
        if (!folderViewModel.canSelectMedia()) {
            item.editMedia(folderViewModel, data, event);
            return;
        }

        folderViewModel.unselectAllMedias(item);
    }

    /**
    * Function is called, when editable item losts focus
    */
    function cancelOrSaveMedia(folderViewModel, item) {
        if (!item.isActive()) {
            return;
        }
        blurTimer = setTimeout(function () {
            if (!item.name() && !item.id()) {
                cancelEditMedia(folderViewModel, item);
            } else {
                saveMedia(folderViewModel, item);
            }
        }, 500);
    }

    /**
    * Cancels inline editing of media
    */
    function cancelEditMedia(folderViewModel, item) {
        item.isActive(false);
        folderViewModel.rowAdded = false;

        if (!item.id()) {
            folderViewModel.medias.remove(item);
        } else {
            item.name(item.oldName);

            // Re-validate after old value is set
            var idSelector = '#' + item.nameDomId,
                input = folderViewModel.container.find(idSelector);
            input.valid();
        }
    }

    /**
    * Saves media after inline edit
    */
    function saveMedia(folderViewModel, item) {
        if (item.isDeleting()) {
            return;
        }

        var idSelector = '#' + item.nameDomId,
            input = folderViewModel.container.find(idSelector),
            loaderContainer = $(input.closest(siteSettings.selectors.loaderContainer).get(0) || input.closest(modal.selectors.scrollWindow).get(0));

        clearTimeout(blurTimer);
        folderViewModel.rowAdded = false;

        if (item.savePressed || (item.oldName != item.name() && item.isActive())) {
            if (input.valid()) {
                var params = item.toJson(),
                    onSaveCompleted = function (json) {
                        loaderContainer.hideLoading();
                        messages.refreshBox(folderViewModel.container, json);
                        if (json.Success && json.Data) {
                            item.version(json.Data.Version);
                            item.id(json.Data.Id);
                            item.oldName = item.name();
                            item.isActive(false);
                        } else {
                            item.isActive(true);
                        }
                    };
                loaderContainer.showLoading();
                item.isActive(false);
                $.ajax({
                    url: item.updateUrl,
                    type: 'POST',
                    dataType: 'json',
                    cache: false,
                    data: params
                })
                    .done(function (json) {
                        onSaveCompleted(json);
                    })
                    .fail(function (response) {
                        onSaveCompleted(bcms.parseFailedResponse(response));
                    });
            } else {
                if (item.savePressed) {
                    input.focus();
                }
            }
        } else {
            item.isActive(false);
        }
        item.savePressed = false;
    }

    /**
    * Returns css class for given file extension
    */
    function getFileExtensionCssClassName(extension) {
        extension = extension.toLowerCase();
        if (!extension) {
            return ' bcms-uknown-icn';
        }

        if (extension.indexOf('.') === 0) {
            extension = extension.substring(1, extension.length);
        }
        extension = $.format('|{0}|', extension);

        if (fileExtensions.archiveFiles.indexOf(extension) >= 0) {
            return ' bcms-archive-icn';
        }
        if (fileExtensions.audioFiles.indexOf(extension) >= 0) {
            return ' bcms-audio-icn';
        }
        if (fileExtensions.videoFiles.indexOf(extension) >= 0) {
            return ' bcms-video-icn';
        }
        if (fileExtensions.imageFiles.indexOf(extension) >= 0) {
            return ' bcms-image-icn';
        }
        if (fileExtensions.officeFiles.indexOf(extension) >= 0) {
            return ' bcms-office-icn';
        }
        return ' bcms-uknown-icn';
    }

    /**
    * Loads a media manager view to the site settings container.
    */
    media.loadSiteSettingsMediaManager = function () {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsMediaManagerUrl, {
            contentAvailable: initializeSiteSettingsMediaManager
        });
    };

    /**
    * Show folder selection window
    */
    media.openFolderSelectDialog = function (opts) {
        
        var options = $.extend({
            onAccept: function () { },
            folderViewModelOptions: null,
            onClose: null,
            parentFolderId: '',
        }, opts);

        modal.open({
            title: globalization.selectFolderDialogTitle,
            acceptTitle: globalization.selectFolderSelectButtonTitle,
            onClose: options.onClose,
            onLoad: function (dialog) {
                dynamicContent.setContentFromUrl(dialog, $.format(links.insertImageDialogUrl, options.parentFolderId || ''), {
                    done: function (content) {
                        imagesViewModel = new MediaItemsViewModel(dialog.container, links.loadImagesUrl, dialog.container);
                        imagesViewModel.spinContainer = dialog.container.find(selectors.insertContentContainer);
                        if (options.folderViewModelOptions) {
                            imagesViewModel = $.extend(imagesViewModel, options.folderViewModelOptions);
                        }
                        imagesViewModel.selectFolder = function (selectedFolder) {
                            dialog.close();
                            options.onAccept(selectedFolder);
                        };
                        initializeTab(content, imagesViewModel);
                    }
                });
            },
            onAcceptClick: function () {
                
                if ($.isFunction(options.onAccept)) {
                    options.onAccept(imagesViewModel.path().currentFolder());
                }

                return true;
            }
        });
    };

    /**
    * Shows image selection window.
    */
    media.openImageInsertDialog = function (opts) {
        
        var options = $.extend({
            onAccept: function () { },
            canInsertWithOptions: false,
            folderViewModelOptions: null,
            onClose: null,
            currentFolder: '',
            selectedId: ''
        }, opts);

        modal.open({
            title: globalization.insertImageDialogTitle,
            acceptTitle: globalization.insertImageInsertButtonTitle,
            onClose: options.onClose,
            onLoad: function (dialog) {
                imageInsertDialog = dialog;
                dynamicContent.setContentFromUrl(dialog, $.format(links.insertImageDialogUrl, options.currentFolder || ''), {
                    done: function (content) {
                        imagesViewModel = new MediaItemsViewModel(dialog.container, links.loadImagesUrl, dialog.container);
                        imagesViewModel.canSelectMedia(true);
                        imagesViewModel.canInsertMedia(true);
                        imagesViewModel.canInsertMediaWithOptions(options.canInsertWithOptions);
                        imagesViewModel.spinContainer = dialog.container.find(selectors.insertContentContainer);
                        if (options.folderViewModelOptions) {
                            imagesViewModel = $.extend(imagesViewModel, options.folderViewModelOptions);
                        }
                        initializeTab(content, imagesViewModel);
                        selectItemById(imagesViewModel, options.selectedId);
                    }
                });
            },
            onAcceptClick: function () {
                var i,
                    item,
                    selectedItem = null;

                for (i = 0; i < imagesViewModel.medias().length; i++) {
                    item = imagesViewModel.medias()[i];
                    if (item.isSelected() && item.type == mediaTypes.image) {
                        selectedItem = item;
                        break;
                    }
                }

                if (selectedItem == null) {
                    modal.info({
                        content: globalization.imageNotSelectedMessageMessage,
                        disableCancel: true
                    });

                    return false;
                } else {
                    if ($.isFunction(options.onAccept)) {
                        options.onAccept(selectedItem);
                    }
                }

                return true;
            }
        });
    };

    /**
    * Function is called, when insert image event is trigerred
    */
    function onOpenImageInsertDialog(htmlContentEditor) {
        contentEditor = htmlContentEditor;

        var options = {
            onAccept: function (imageViewModel) {
                insertImage(imageViewModel, false);
            },
            canInsertWithOptions: true
        };
        
        media.openImageInsertDialog(options);
    }

    /**
    * Called when image is selected from images list.
    */
    function insertImage(selectedMedia, withOptions, onImageInsert) {
        if (selectedMedia.isDeleting()) {
            return;
        }

        var mediaId = selectedMedia.id(),
            alertOnError = function () {
                modal.alert({
                    title: globalization.insertImageFailureMessageTitle,
                    content: globalization.insertImageFailureMessageMessage
                });
            },
            imageInsert = function (selectedMedia, url, caption, align) {

                if ($.isFunction(onImageInsert)) {
                    onImageInsert(selectedMedia, url, caption, align);
                } else {
                    addImageToEditor(url, caption, align, selectedMedia.version());
                }

                if (imageInsertDialog != null) {
                    imageInsertDialog.close();
                    imageInsertDialog = null;
                }
            };

        if (withOptions === true) {
            imageEditor.onInsertImage(selectedMedia, imageInsert);
        } else {
            var url = $.format(links.getImageUrl, mediaId);
            $.ajax({
                url: url,
                type: "POST",
                dataType: 'json'
            }).done(function (json) {
                if (json.Success && json.Data != null) {
                    imageInsert(selectedMedia, json.Data.Url, json.Data.Caption, json.Data.ImageAlign);
                } else {
                    alertOnError();
                }
            }).fail(function () {
                alertOnError();
            });
        }
    };

    /**
    * Insert image to html content editor.
    */
    function addImageToEditor(imageUrl, caption, imageAlign, version) {
        caption = caption || '';
        if (contentEditor != null) {
            var align = "left",
                cssClass = classes.customImageLeftAlign,
                img;
            
            if (imageAlign == 2) {
                align = "";
                cssClass = "";
            } else if (imageAlign == 3) {
                cssClass = classes.customImageRightAlign,
                align = "right";
            }

            if (imageAlign == 2) {
                img = '<img src="' + imageUrl + '" alt="' + caption + '"/>';
            } else {
                img = '<img src="' + imageUrl + '" alt="' + caption + '" style="float:' + align + '" class="' + cssClass + '" />';
            }

            contentEditor.addHtml(img);
        }
    };

    /**
    * Shows file selection window.
    */
    media.openFileInsertDialog = function (onAccept) {
        modal.open({
            title: globalization.insertFileDialogTitle,
            acceptTitle: 'Insert',
            onLoad: function (dialog) {
                fileInsertDialog = dialog;
                dynamicContent.setContentFromUrl(dialog, links.insertFileDialogUrl, {
                    done: function (content) {
                        filesViewModel = new MediaItemsViewModel(dialog.container, links.loadFilesUrl, dialog.container);
                        filesViewModel.canSelectMedia(true);
                        filesViewModel.canInsertMedia(true);
                        filesViewModel.spinContainer = dialog.container.find(selectors.insertContentContainer);
                        initializeTab(content, filesViewModel);
}
                });
            },
            onAcceptClick: function () {
                var i,
                    item,
                    selectedItem = null;

                for (i = 0; i < filesViewModel.medias().length; i++) {
                    item = filesViewModel.medias()[i];
                    if (item.isSelected() && item.type == mediaTypes.file) {
                        selectedItem = item;
                        break;
                    }
                }

                if (selectedItem == null) {
                    modal.info({
                        content: globalization.fileNotSelectedMessageMessage,
                        disableCancel: true
                    });

                    return false;
                } else {
                    if ($.isFunction(onAccept)) {
                        onAccept(selectedItem);
                    }
                }

                return true;
            }
        });
    };

    /**
    * Called when file is selected from files list.
    */
    function insertFile(selectedMedia) {
        addFileToEditor($.format(links.getFileUrl, selectedMedia.id()), selectedMedia.name());

        if (fileInsertDialog != null) {
            fileInsertDialog.close();
            fileInsertDialog = null;
        }
    };

    /**
    * Function is called, when insert file event is triggered.
    */
    function onOpenFileInsertDialog(htmlContentEditor) {
        contentEditor = htmlContentEditor;
        media.openFileInsertDialog(function (fileViewModel) {
            insertFile(fileViewModel);
        });
    }

    /**
    * Insert file to html content editor.
    */
    function addFileToEditor(fileUrl, fileName) {
        if (contentEditor != null) {
            if (contentEditor.mode == 'source') {
                var file = '<a href="' + fileUrl + '">' + fileName + '</a>';
                var oldData = contentEditor.getData();

                contentEditor.setData(oldData + file);
            } else {
                contentEditor.insertHtml('<a href="' + fileUrl + '">' + fileName + '</a>');
            }
        }
    };

    /**
    * Open delete confirmation and delete media item.
    */
    function deleteMediaItem(url, message, folderViewModel, item) {
        if (item.isProcessing() || item.isDeleting()) {
            return;
        }

        var onDeleteCompleted = function (json) {
            messages.refreshBox(folderViewModel.container, json);
            if (json.Success) {
                folderViewModel.medias.remove(item);
            } else {
                item.isDeleting(false);
            }
            confirmDialog.close();
        },
            confirmDialog = modal.confirm({
                content: message,
                onAccept: function () {
                    item.isDeleting(true);
                    $.ajax({
                        type: 'POST',
                        url: url,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        cache: false
                    })
                    .done(function (json) {
                        onDeleteCompleted(json);
                    })
                    .fail(function (response) {
                        onDeleteCompleted(bcms.parseFailedResponse(response));
                    });
                }
            });
    };

    /**
    * Open archive/unarchive confirmation and archive/unarchive media item.
    */
    function archiveUnarchiveMediaItem(isArchiving, url, message, folderViewModel, item) {
        if (item.isDeleting()) {
            return;
        }

        var onArchivingCompleted = function (json) {
            messages.refreshBox(folderViewModel.container, json);
            if (json.Success) {
                item.isArchived(isArchiving);
                if (json.Data && json.Data.Version) {
                    item.version(json.Data.Version);
                }
                if (isArchiving && !folderViewModel.gridOptions().includeArchived()) {
                    folderViewModel.medias.remove(item);
                }
            }

            confirmDialog.close();
        },
            confirmDialog = modal.confirm({
                content: message,
                onAccept: function () {
                    $.ajax({
                        type: 'POST',
                        url: url,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        cache: false
                    })
                    .done(function (json) {
                        onArchivingCompleted(json);
                    })
                    .fail(function (response) {
                        onArchivingCompleted(bcms.parseFailedResponse(response));
                    });
                }
            });
    };

    /**
    * Creates params for getting folder with filter/search/sort options
    */
    function createFolderParams(folderId, folderViewModel) {
        var params = {
            CurrentFolderId: folderId
        };

        if (folderViewModel != null && folderViewModel.gridOptions() != null) {
            var query = folderViewModel.gridOptions().searchQuery();
            if (query) {
                params.SearchQuery = query;
            }

            params.Column = folderViewModel.gridOptions().column();
            params.Direction = folderViewModel.gridOptions().isDescending()
                ? sortDirections.descending
                : sortDirections.ascending;
            params.PageSize = folderViewModel.gridOptions().paging.pageSize;
            params.PageNumber = folderViewModel.gridOptions().paging.pageNumber();
            params.IncludeArchivedItems = folderViewModel.gridOptions().includeArchived();

            if (folderViewModel.gridOptions().tags.items().length > 0) {
                params.Tags = [];
                for (var i = 0; i < folderViewModel.gridOptions().tags.items().length; i++) {
                    params.Tags.push({
                        Key: folderViewModel.gridOptions().tags.items()[i].id(),
                        Value: folderViewModel.gridOptions().tags.items()[i].name()
                    });
                }
            }

            if (folderViewModel.gridOptions().categories.items().length > 0) {
                params.Categories = [];
                for (var i = 0; i < folderViewModel.gridOptions().categories.items().length; i++) {
                    params.Categories.push({
                        Key: folderViewModel.gridOptions().categories.items()[i].id(),
                        Value: folderViewModel.gridOptions().categories.items()[i].name()
                    });
                }
            }
        }

        return params;
    }

    /**
    * Changes current folder.
    */
    function changeFolder(id, folderViewModel) {
        // Clear search.
        if (folderViewModel != null && folderViewModel.gridOptions() != null) {
            folderViewModel.gridOptions().searchQuery('');
            folderViewModel.gridOptions().closeFilter();
        }
        var params = createFolderParams(id, folderViewModel),
            onComplete = function (json) {
                messages.refreshBox(folderViewModel.container, {});
                parseJsonResults(json, folderViewModel);
            };
        loadTabData(folderViewModel, params, onComplete);
    };

    /**
    * Parse json result and map data to view model
    */
    function parseJsonResults(json, folderViewModel) {
        var i,
            isSearchResult;

        messages.refreshBox(folderViewModel.container, json);

        if (json.Success) {
            folderViewModel.medias.removeAll();

            if (json.Data) {
                // Map media path
                if (json.Data.Path) {
                    var pathViewModel = new MediaPathViewModel(json.Data.Path.CurrentFolder.Type);
                    if (json.Data.Path.CurrentFolder) {
                        var currentFolderViewModel = new MediaFolderViewModel(json.Data.Path.CurrentFolder);
                        pathViewModel.currentFolder(currentFolderViewModel);
                    }

                    if (json.Data.Path.Folders && $.isArray(json.Data.Path.Folders)) {
                        for (i = 0; i < json.Data.Path.Folders.length; i++) {
                            var pathFolderViewModel = new MediaFolderViewModel(json.Data.Path.Folders[i]);
                            pathViewModel.folders.push(pathFolderViewModel);
                        }
                    }

                    folderViewModel.path(pathViewModel);
                }

                // Map media items
                if (json.Data.Items && json.Data.Items && $.isArray(json.Data.Items)) {
                    for (i = 0; i < json.Data.Items.length; i++) {
                        var item = json.Data.Items[i],
                            model = convertToMediaModel(item);

                        if (model != null) {
                            folderViewModel.medias.push(model);
                        }
                    }
                }

                // Map grid options
                if (!folderViewModel.gridOptions()) {
                    folderViewModel.gridOptions(new MediaItemsOptionsViewModel(folderViewModel.onOpenPage, json.Data.Path.CurrentFolder.Type));
                }
                folderViewModel.gridOptions().fromJson(json.Data);

                isSearchResult = !folderViewModel.gridOptions().isSearchEmpty();
                folderViewModel.isSearchResults(isSearchResult);
                folderViewModel.path().isSearchResults(isSearchResult);
                if (isSearchResult) {
                    folderViewModel.noSearchResultFound($.format(globalization.noResultFoundMessage, folderViewModel.gridOptions().searchQuery()));
                } else {
                    folderViewModel.noSearchResultFound('');
                }
                folderViewModel.canSearchInHistory(!folderViewModel.searchInHistory);
                folderViewModel.searchInHistory = false;

                // Replace unobtrusive validator
                bcms.updateFormValidator(folderViewModel.container.find(selectors.firstForm));
            }

            return true;
        }

        return false;
    }

    function convertToMediaModel(item) {
        var model;
        if (item.ContentType == contentTypes.folder) {
            model = new MediaFolderViewModel(item);
        } else {
            switch (item.Type) {
                case mediaTypes.image:
                    model = new MediaImageViewModel(item);
                    break;
                case mediaTypes.audio:
                    model = new MediaAudioViewModel(item);
                    break;
                case mediaTypes.video:
                    model = new MediaVideoViewModel(item);
                    break;
                case mediaTypes.file:
                    model = new MediaFileViewModel(item);
                    break;
                default:
                    model = null;
                    break;
            }
        }

        return model;
    }

    /**
    * Load tab data
    */
    function loadTabData(folderViewModel, params, complete) {
        var indicatorId = 'mediatab',
            spinContainer = folderViewModel.spinContainer,
            onComplete = function (result) {
                spinContainer.hideLoading(indicatorId);
                if ($.isFunction(complete)) {
                    complete(result);
                }
            };
        spinContainer.showLoading(indicatorId);

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            url: folderViewModel.url,
            data: JSON.stringify(params)
        })
            .done(function (result) {
                onComplete(result);
            })
            .fail(function (response) {
                onComplete(bcms.parseFailedResponse(response));
            });
    }

    /**
    * Initializes tab, when data is loaded
    */
    function initializeTab(json, folderViewModel) {
        var context = folderViewModel.container.find(selectors.templateDataBind).get(0);

        if (parseJsonResults(json, folderViewModel)) {
            ko.applyBindings(folderViewModel, context);

            bcms.updateFormValidator(folderViewModel.container.find(selectors.firstForm));
        }
    }

    /**
    * Initializes media manager.
    */
    function initializeSiteSettingsMediaManager(content) {
        filesViewModel = null;
        audiosViewModel = null;
        videosViewModel = null;

        var dialogContainer = siteSettings.getModalDialog().container,
            selectSearch = function() {
                var firstVisibleInputField = dialogContainer.find('input[type=text],textarea,select').filter(':visible:first');
                if (firstVisibleInputField) {
                    firstVisibleInputField.focus();
                }
            };

        // Attach to audios tab selector
        dialogContainer.find(selectors.tabAudiosSelector).on('click', function () {
            var tabContainer = dialogContainer.find(selectors.tabAudiosContainer);
            if (audiosViewModel == null) {
                audiosViewModel = new MediaItemsViewModel(tabContainer, links.loadAudiosUrl, dialogContainer);
                audiosViewModel.spinContainer = tabContainer.parents(selectors.spinContainer);

                loadTabData(audiosViewModel, null, function (json) {
                    initializeTab(json, audiosViewModel);
                    selectSearch();
                });
            } else {
                selectSearch();
            }
        });

        // Attach to videos tab selector
        dialogContainer.find(selectors.tabVideosSelector).on('click', function () {
            var tabContainer = dialogContainer.find(selectors.tabVideosContainer);
            if (videosViewModel == null) {
                videosViewModel = new MediaItemsViewModel(tabContainer, links.loadVideosUrl, dialogContainer);
                videosViewModel.spinContainer = tabContainer.parents(selectors.spinContainer);

                loadTabData(videosViewModel, null, function(json) {
                    initializeTab(json, videosViewModel);
                    selectSearch();
                });
            } else {
                selectSearch();
            }
        });

        // Attach to files tab selector
        dialogContainer.find(selectors.tabFilesSelector).on('click', function () {
            var tabContainer = dialogContainer.find(selectors.tabFilesContainer);
            if (filesViewModel == null) {
                filesViewModel = new MediaItemsViewModel(tabContainer, links.loadFilesUrl, dialogContainer);
                filesViewModel.spinContainer = tabContainer.parents(selectors.spinContainer);

                loadTabData(filesViewModel, null, function(json) {
                    initializeTab(json, filesViewModel);
                    selectSearch();
                });
            } else {
                selectSearch();
            }
        });
        
        // Attach to images tab selector
        dialogContainer.find(selectors.tabImagesSelector).on('click', function () {
            selectSearch();
        });
        
        var imagesTabContainer = dialogContainer.find(selectors.tabImagesContainer);
        imagesViewModel = new MediaItemsViewModel(imagesTabContainer, links.loadImagesUrl, dialogContainer);
        imagesViewModel.spinContainer = imagesTabContainer.parents(selectors.spinContainer);
        initializeTab(content, imagesViewModel);
        selectSearch();
    };

    /**
    * Image selector view model
    */
    media.ImageSelectorViewModel = (function () {

        media.ImageSelectorViewModel = function (image) {

            var self = this;

            self.id = ko.observable();
            self.url = ko.observable();
            self.thumbnailUrl = ko.observable();
            self.tooltip = ko.observable();
            self.version = ko.observable();
            self.currentFolder = ko.observable();

            self.setImage = function (imageData) {
                self.thumbnailUrl(imageData.ThumbnailUrl);
                self.url(imageData.ImageUrl);
                self.tooltip(imageData.ImageTooltip);
                self.id(imageData.ImageId);
                self.version(imageData.ImageVersion);
                self.currentFolder(imageData.FolderId);
            };

            if (image) {
                self.setImage(image);
            }
        };

        media.ImageSelectorViewModel.prototype.preview = function (data, event) {
            bcms.stopEventPropagation(event);

            var previewUrl = this.url(),
                self = this;

            if (previewUrl) {
                var options = {
                    onClose: function () {
                        self.onAfterPreview();
                    }
                };

                modal.imagePreview(previewUrl, this.tooltip(), options);
            }
        };

        media.ImageSelectorViewModel.prototype.remove = function (data, event) {
            bcms.stopEventPropagation(event);

            this.setImage({});
            this.onAfterRemove();
        };

        media.ImageSelectorViewModel.prototype.select = function (data, event) {
            var self = this,
                onMediaSelect = function(insertedImage) {
                    self.thumbnailUrl(insertedImage.thumbnailUrl());
                    self.url(insertedImage.publicUrl());
                    self.tooltip(insertedImage.tooltip);
                    self.id(insertedImage.id());
                    self.version(insertedImage.version());
                    self.currentFolder(insertedImage.parentFolderId());

                    self.onAfterSelect();
                },
                mediasViewModelExtender = {
                    onMediaSelect: function(image) {
                        onMediaSelect(image);
                    }
                },
                onMediaSelectClose = function() {
                    self.onSelectClose();
                }, options = {
                    onAccept: onMediaSelect,
                    canInsertWithOptions: false,
                    folderViewModelOptions: mediasViewModelExtender,
                    onClose: onMediaSelectClose,
                    currentFolder: self.currentFolder(),
                    selectedId: self.id()
                };

            bcms.stopEventPropagation(event);
            media.openImageInsertDialog(options);
        };

        media.ImageSelectorViewModel.prototype.onAfterSelect = function () { };

        media.ImageSelectorViewModel.prototype.onAfterRemove = function () { };

        media.ImageSelectorViewModel.prototype.onAfterPreview = function () { };

        media.ImageSelectorViewModel.prototype.onSelectClose = function () { };

        return media.ImageSelectorViewModel;
    })();

    /**
    * Image folder selector view model
    */
    media.ImageFolderSelectorViewModel = (function () {

        media.ImageFolderSelectorViewModel = function (folder) {

            var self = this;

            self.id = ko.observable();
            self.title = ko.observable();
            self.parentFolderId = ko.observable();

            self.setFolder = function (folderData) {
                self.id(folderData.FolderId);
                self.title(folderData.FolderTitle);
                self.parentFolderId(folderData.ParentFolderId);
            };

            if (folder) {
                self.setFolder(folder);
            }
        };

        media.ImageFolderSelectorViewModel.prototype.select = function (data, event) {
            var self = this,
                onMediaSelect = function (insertedFolder) {
                    self.id(insertedFolder.id());
                    self.title(insertedFolder.name());
                    self.parentFolderId(insertedFolder.parentFolderId());

                    self.onAfterSelect();
                },
                mediasViewModelExtender = {
                    onMediaSelect: function (folder) {
                        onMediaSelect(folder);
                    }
                },
                onMediaSelectClose = function () {
                    self.onSelectClose();
                },
                parentFolderId = self.parentFolderId(),
                options = {
                    onAccept: onMediaSelect,
                    folderViewModelOptions: mediasViewModelExtender, 
                    onClose: onMediaSelectClose,
                    parentFolderId: !parentFolderId || bcms.isEmptyGuid(parentFolderId) ? '' : parentFolderId
                };

            bcms.stopEventPropagation(event);
            media.openFolderSelectDialog(options);
        };

        media.ImageFolderSelectorViewModel.prototype.onAfterSelect = function () { };
        
        media.ImageFolderSelectorViewModel.prototype.onSelectClose = function () { };

        return media.ImageFolderSelectorViewModel;
    })();

    /**
    * Called when user press browse button in the options grid with type = "Media Manager Folder".
    */
    function onExecuteMediaManagerFolderOption(valueObservable, titleObservable, optionModel) {
        var onMediaSelect = function(selectedFolder) {
            var id = selectedFolder.id(),
                name = selectedFolder.name();

            if (!id || bcms.isEmptyGuid(id)) {
                id = '';
                name = globalization.rootFolderTitle;
            }

            valueObservable(id);
            titleObservable(name);

            if (optionModel.key && !optionModel.key()) {
                optionModel.key(name);
            }

            optionModel.hasFocus(true);
        },
            onMediaClose = function() {
                optionModel.hasFocus(true);
            },
            mediasViewModelExtender = {
                onMediaSelect: function(selectedFolder) {
                    onMediaSelect(selectedFolder);
                }
            },
            options = {
                onAccept: onMediaSelect,
                onClose: onMediaClose,
                folderViewModelOptions: mediasViewModelExtender,
                parentFolderId: valueObservable()
            };

        media.openFolderSelectDialog(options);
    }

    /**
    * Called when user press browse button in the options grid with type = "Media Manager Image Url".
    */
    function onExecuteMediaManagerImageOption(valueObservable, titleObservable, optionModel) {
        var onMediaSelect = function(selectedImage) {
            var url = selectedImage.getImageUrl();


                valueObservable(url);
                titleObservable(url);

                if (optionModel.key && !optionModel.key()) {
                    optionModel.key(name);
                }

                optionModel.hasFocus(true);
            },
            onMediaClose = function() {
                optionModel.hasFocus(true);
            },

            options = {
                onAccept: onMediaSelect,
                onClose: onMediaClose,
            };

        media.openImageInsertDialog(options);
    }


    /**
    * Called when context menu is shown.
    */
    function onShowContextMenu() {
        if (imagesViewModel && $.isFunction(imagesViewModel.showPropertiesPreview)) {
            imagesViewModel.showPropertiesPreview(false);
        }
    }

    /**
    * Initializes media module.
    */
    media.init = function () {
        bcms.logger.debug('Initializing bcms.media module.');

        /**
        * Subscribe to events.
        */
        bcms.on(htmlEditor.events.insertImage, onOpenImageInsertDialog);
        bcms.on(htmlEditor.events.insertFile, onOpenFileInsertDialog);
        bcms.on(menu.events.menuOn, onShowContextMenu);

        fileEditor.SetMedia(media);
        
        optionsModule.registerCustomOption('media-images-folder', onExecuteMediaManagerFolderOption);
        optionsModule.registerCustomOption('media-images-url', onExecuteMediaManagerImageOption);
    };

    /**
    * Register initialization.
    */
    bcms.registerInit(media.init);

    return media;
});