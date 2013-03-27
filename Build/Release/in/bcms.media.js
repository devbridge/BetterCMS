/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.media', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'bcms.media.upload', 'bcms.media.imageeditor', 'bcms.htmlEditor', 'bcms.ko.extenders', 'bcms.contextMenu'],
function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, mediaUpload, imageEditor, htmlEditor, ko, menu) {
    'use strict';

    var media = { },
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
            fileListMessageBox: '#bcms-site-settings-media-messages-'
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
            downloadFileUrl: null
        },
        globalization = {
            deleteImageConfirmMessage: null,
            deleteAudioConfirmMessage: null,
            deleteVideoConfirmMessage: null,
            deleteFileConfirmMessage: null,
            deleteFolderConfirmMessage: null,

            insertImageDialogTitle: null,
            insertImageFailureMessageTitle: null,
            insertImageFailureMessageMessage: null,
            imageNotSelectedMessageMessage: null,

            insertFileFailureMessageTitle: null,
            insertFileFailureMessageMessage: null,
            insertFileDialogTitle: null,
            fileNotSelectedMessageMessage: null,

            imagesTabTitle: null,
            audiosTabTitle: null,
            videosTabTitle: null,
            filesTabTitle: null,

            uploadImage: null,
            uploadAudio: null,
            uploadVideo: null,
            uploadFile: null,
        },
        keys = {
            folderViewMode: 'bcms.mediaFolderViewMode'
        },
        mediaTypes = {
            image: 1,
            video: 2,
            audio: 3,
            file: 4,
        },
        contentTypes = {
            file: 1,
            folder: 2
        },
        sortDirections = {
            ascending: 0,
            descending: 1
        },
        imagesViewModel = null,
        audiosViewModel = null,
        videosViewModel = null,
        filesViewModel = null,
        staticDomId = 1,
        contentEditor = null,
        imageInsertDialog = null,
        fileInsertDialog = null,
        blurTimer = null;

    /**
    * Assign objects to module.
    */
    media.links = links;
    media.globalization = globalization;

    /**
    * Media's current folder sort / search / paging option view model
    */
    function MediaItemsOptionsViewModel(options) {
        var self = this;

        self.searchQuery = ko.observable(options.GridOptions.SearchQuery);
        if (options.GridOptions) {
            self.column = ko.observable(options.GridOptions.Column);
            self.isDescending = ko.observable(options.GridOptions.Direction == sortDirections.descending);
        }
    }

    /**
    * Media's current folder view model
    */
    function MediaItemsViewModel(container, url, messagesContainer) {
        var self = this,
            onUploadFiles = function(newFilesData) {
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
            };

        self.container = container;
        self.messagesContainer = messagesContainer;
        self.url = url;
        self.onMediaSelect = null;

        self.medias = ko.observableArray();
        self.path = ko.observable();
        self.isGrid = ko.observable(localStorage.getItem(keys.folderViewMode) == 1);
        self.canSelectMedia = ko.observable(false);
        self.canInsertMedia = ko.observable(false);
        self.canInsertMediaWithOptions = ko.observable(false);

        self.gridOptions = ko.observable();

        self.isRootFolder = function () {
            if (self.path != null && self.path().folders != null && self.path().folders().length > 1) {
                return false;
            }
            return true;
        };

        self.showNoDataInfoDiv = function () {
            return self.isRootFolder() && self.medias().length == 0;
        };

        self.addNewFolder = function () {
            var newFolder = new MediaFolderViewModel({
                IsActive: true,
                Type: self.path().currentFolder().type
            });

            self.medias.unshift(newFolder);
            
            // Replace unobtrusive validator
            bcms.updateFormValidator(self.container.find(selectors.firstForm));
        };

        self.uploadMedia = function () {
            mediaUpload.openUploadFilesDialog(self.path().currentFolder().id(), self.path().currentFolder().type, onUploadFiles);
            messages.refreshBox($(selectors.fileListMessageBox + self.path().type), {});
        };

        self.searchMedia = function () {
            var params = createFolderParams(self.path().currentFolder().id(), self),
                onComplete = function (json) {
                    parseJsonResults(json, self);
                    $(selectors.searchBox).focus();                    
                };
            loadTabData(self, params, onComplete);
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
            localStorage.setItem(keys.folderViewMode, isGrid ? 1 : 0);
            
            // Loop through all media view models
            $.each([imagesViewModel, audiosViewModel, videosViewModel, filesViewModel], function(index, viewModel) {
                if (viewModel && viewModel.isGrid) {
                    viewModel.isGrid(isGrid);
                }
            });
        };

        self.isSortedAscending = function(column) {
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

        self.openRoot = function() {
            changeFolder(null, self);
        };

        self.domId = function() {
            return 'bcms-site-settings-media-messages-' + self.path().type;
        };
    }

    /**
    * Media path view model
    */
    function MediaPathViewModel(type) {
        var self = this;

        self.currentFolder = ko.observable();
        self.folders = ko.observableArray();
        self.type = type;

        self.pathFolders = ko.computed(function () {
            var range = self.folders(),
                maxFoldersCount = 4,
                i;
            if (range.length > maxFoldersCount) {
                for (i = 0; i < range.length; i++) {
                    if (i <= maxFoldersCount) {
                        range.remove(range[i]);
                    }
                }
            }
            if (range.length > 0) {
                switch (self.type) {
                    case mediaTypes.image:
                        range[0].name(globalization.imagesTabTitle);
                        break;
                    case mediaTypes.audio:
                        range[0].name(globalization.audiosTabTitle);
                        break;
                    case mediaTypes.video:
                        range[0].name(globalization.videosTabTitle);
                        break;
                    case mediaTypes.file:
                        range[0].name(globalization.filesTabTitle);
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

            self.isActive = ko.observable(item.IsActive || false);
            self.isSelected = ko.observable(false);

            self.contextMenu = new MediaItemContextMenuViewModel();

            self.isFile = function () {
                return !self.isFolder();
            };

            self.canBeEdited = function () {
                return self.isImage();
            };

            self.canBeDownloaded = function () {
                return self.publicUrl();
            };

            self.stopEvent = function(data, event) {
                bcms.stopEventPropagation(event);
            };

            self.downloadMedia = function () {
                window.open($.format(links.downloadFileUrl, self.id()), '_newtab');
            };

            self.rowClassNames = ko.computed(function () {
                var classes = '';
                if (self.isFolder()) {
                    classes += ' bcms-folder-box';
                    if (self.isActive()) {
                        classes += ' bcms-folder-box-active';
                    }
                }
                if (self.isFile() && !self.isImage()) {
                    classes += ' bcms-file-box';
                }
                if (self.isImage()) {
                    classes += ' bcms-image-box';
                }
                if (self.isFile() && self.isActive()) {
                    if (!self.isImage()) {
                        classes += ' bcms-file-box-active';
                    } else {
                        classes += ' bcms-image-box-active';
                    }
                }
                if (self.isSelected()) {
                    classes += ' bcms-media-click-active';
                }
                return $.trim(classes);
            });

            self.iconClassNames = ko.computed(function () {
                var classes = '';
                    
                if (self.isFolder()) {
                    classes += ' bcms-system-folder';
                }
                if (self.isImage()) {
                    classes += ' bcms-media-file-holder';
                }
                if (self.isFile() && !self.isImage()) {
                    classes += ' bcms-system-file';
                }
                if (!self.isImage() && self.extension) {
                    classes += getFileExtensionCssClassName(self.extension);
                }

                return $.trim(classes);
            });
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

        MediaItemBaseViewModel.prototype.openMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            editOrSelectMedia(folderViewModel, this, data, event);
        };

        MediaItemBaseViewModel.prototype.editMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            this.renameMedia(folderViewModel, data, event);
        };
            
        MediaItemBaseViewModel.prototype.renameMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            this.isActive(true);
        };
            
        MediaItemBaseViewModel.prototype.openMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);
            editOrSelectMedia(folderViewModel, this, data, event);
            messages.refreshBox($(selectors.fileListMessageBox + this.type), {});
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

        return MediaItemBaseViewModel;
    })();

    /**
    * Media image view model
    */
    var MediaImageViewModel = (function(_super) {
        bcms.extendsClass(MediaImageViewModel, _super);

        function MediaImageViewModel(item) {
            _super.call(this, item);

            var self = this;

            self.tooltip = item.Tooltip;
            self.thumbnailUrl = item.ThumbnailUrl;

            self.getImageUrl = function() {
                if (!self.thumbnailUrl) {
                    return null;
                }
                return self.thumbnailUrl + '?version=' + self.version();
            };

            self.previewImage = function () {
                var previewUrl = self.publicUrl() + '?version=' + self.version();
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

        MediaImageViewModel.prototype.editMedia = function (folderViewModel, data, event) {
            bcms.stopEventPropagation(event);

            var self = this;
            imageEditor.onEditImage(self.id(), function (json) {
                self.version(json.Version);
                self.name(json.Title);
            });
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
    var MediaAudioViewModel = (function(_super) {
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
    var MediaVideoViewModel = (function(_super) {
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

        return MediaVideoViewModel;
    })(MediaItemBaseViewModel);

    /**
    * File view model
    */
    var MediaFileViewModel = (function(_super) {
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

        return MediaFileViewModel;
    })(MediaItemBaseViewModel);

    /**
    * Media folder view model
    */
    var MediaFolderViewModel = (function(_super) {
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
            changeFolder(this.id(), folderViewModel);
        };

        return MediaFolderViewModel;
    })(MediaItemBaseViewModel);

    /**
    * Opens media for seleting or editing
    */
    function editOrSelectMedia(folderViewModel, item, data, event) {
        if (item.isProcessing() || item.isFailed()) {
            return;
        }

        // Call open, if view model is not in selectable mode
        if (!folderViewModel.canSelectMedia()) {
            item.editMedia(folderViewModel, data, event);
            return;
        }

        // Select item, if view model is in selectable mode
        for (var i = 0; i < folderViewModel.medias().length; i++) {
            folderViewModel.medias()[i].isSelected(false);
        }
        item.isSelected(true);
    }

    /**
    * Function is called, when editable item losts focus
    */
    function cancelOrSaveMedia(folderViewModel, item) {
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
        if (!item.id()) {
            folderViewModel.medias.remove(item);
        } else {
            item.name(item.oldName);
        }
    }

    /**
    * Saves media after inline edit
    */
    function saveMedia(folderViewModel, item) {
        var idSelector = '#' + item.nameDomId,
            input = folderViewModel.container.find(idSelector),
            loaderContainer = $(input.closest(siteSettings.selectors.loaderContainer).get(0) || input.closest(modal.selectors.scrollWindow).get(0));

        clearTimeout(blurTimer);

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
                        } else {
                            item.isActive(true);
                        }
                        item.isActive(false);
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
                    .done(function(json) {
                        onSaveCompleted(json);
                    })
                    .fail(function(response) {
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
        if (extension.indexOf('.') === 0) {
            extension = extension.substring(1, extension.length);
        }
        switch (extension.toLowerCase()) {
            case "pdf":
                return ' bcms-pdf-icn';
            case "doc":
            case "docx":
                return ' bcms-word-icn';
            case "xls":
            case "xlsx":
                return ' bcms-xls-icn';
            case "mp3":
                return ' bcms-mp3-icn';
            case "mp4":
                return ' bcms-mp4-icn';
            case "ppt":
            case "pptx":
                return ' bcms-ppt-icn';
            case "rar":
                return ' bcms-rar-icn';
            case "wav":
                return ' bcms-wav-icn';
            case "zip":
                return ' bcms-zip-icn';
            default:
                return ' bcms-uknown-icn';
        }
    }

    /**
    * Loads a media manager view to the site settings container.
    */
    media.loadSiteSettingsMediaManager = function() {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsMediaManagerUrl, {
            contentAvailable: initializeSiteSettingsMediaManager
        });
    };

    /**
    * Shows image selection window.
    */
    media.openImageInsertDialog = function (onAccept, canInsertWithOptions, folderViewModelOptions, onClose) {
        modal.open({
            title: globalization.insertImageDialogTitle,
            acceptTitle: 'Insert',
            onClose: onClose,
            onLoad: function (dialog) {
                imageInsertDialog = dialog;
                dynamicContent.setContentFromUrl(dialog, links.insertImageDialogUrl, {
                    done: function (content) {
                        imagesViewModel = new MediaItemsViewModel(dialog.container, links.loadImagesUrl, dialog.container);
                        imagesViewModel.canSelectMedia(true);
                        imagesViewModel.canInsertMedia(true);
                        imagesViewModel.canInsertMediaWithOptions(canInsertWithOptions);
                        imagesViewModel.spinContainer = dialog.container.find(selectors.insertContentContainer);
                        if (folderViewModelOptions) {
                            imagesViewModel = $.extend(imagesViewModel, folderViewModelOptions);
                        }
                        initializeTab(content, imagesViewModel);
                    },
                });
            },
            onAcceptClick: function () {
                var i,
                    item,
                    selectedItem = null;

                for (i = 0; i < imagesViewModel.medias().length; i++)
                {
                    item = imagesViewModel.medias()[i];
                    if (item.isSelected() && item.type == mediaTypes.image) {
                        selectedItem = item;
                        break;
                    }
                }

                if (selectedItem == null) {
                    modal.info({
                        content: globalization.imageNotSelectedMessageMessage,
                        disableCancel: true,
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
    * Function is called, when insert image event is trigerred
    */ 
    function onOpenImageInsertDialog(htmlContentEditor) {
        contentEditor = htmlContentEditor;
        media.openImageInsertDialog(function(imageViewModel) {
            insertImage(imageViewModel, false);
        }, true);
    }

    /**
    * Called when image is selected from images list.
    */
    function insertImage(selectedMedia, withOptions, onImageInsert) {
        var mediaId = selectedMedia.id(),
            alertOnError = function() {
                modal.alert({
                    title: globalization.insertImageFailureMessageTitle,
                    content: globalization.insertImageFailureMessageMessage,
                });
            },
            imageInsert = function(selectedMedia, url, caption, align) {

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
        if (contentEditor != null) {
            var align = "left";
            if (imageAlign == 2) {
                align = "center";
            } else if (imageAlign == 3) {
                align = "right";
            }
            if (imageUrl.indexOf('?') < 0) {
                imageUrl += '?version=' + version;
            } else {
                imageUrl += '&version=' + version;
            }
            var img = '<img src="' + imageUrl + '" alt="' + caption + '" align="' + align + '"/>';
            if (contentEditor.mode == 'source') {
                var oldData = contentEditor.getData();

                contentEditor.setData(oldData + img);
            } else {
                contentEditor.insertHtml(img);
            }
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
                    },
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
                        disableCancel: true,
                    });

                    return false;
                } else {
                    if ($.isFunction(onAccept)) {
                        onAccept(selectedItem);
                    }
                }

                return true;
            },
        });
    };

    /**
    * Called when file is selected from files list.
    */
    function insertFile(selectedMedia) {
        addFileToEditor($.format(links.downloadFileUrl, selectedMedia.id()), selectedMedia.name());

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
        if (item.isProcessing()) {
            return;
        }

        var onDeleteCompleted = function (json) {
                messages.refreshBox(folderViewModel.container, json);
                if (json.Success) {
                    folderViewModel.medias.remove(item);
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
                        onDeleteCompleted(json);
                    })
                    .fail(function (response) {
                        onDeleteCompleted(bcms.parseFailedResponse(response));
                    });
                    return false;
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
        }

        return params;
    }

    /**
    * Changes current folder.
    */
    function changeFolder(id, folderViewModel) {
        var params = createFolderParams(id, null),
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
        var i;

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
                folderViewModel.gridOptions(new MediaItemsOptionsViewModel(json.Data));

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
            onComplete = function(result) {
                spinContainer.hideLoading(indicatorId);
                if ($.isFunction(complete)) {
                    complete(result);
                }
            };
        spinContainer.showLoading(indicatorId);
            
        $.ajax({
            type: 'POST',
            cache: false,
            url: folderViewModel.url,
            data: params
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

        var dialogContainer = siteSettings.getModalDialog().container;

        // Attach to audios tab selector
        dialogContainer.find(selectors.tabAudiosSelector).on('click', function () {
            var tabContainer = dialogContainer.find(selectors.tabAudiosContainer);
            if (audiosViewModel == null) {
                audiosViewModel = new MediaItemsViewModel(tabContainer, links.loadAudiosUrl, dialogContainer);
                audiosViewModel.spinContainer = tabContainer.parents(selectors.spinContainer);

                loadTabData(audiosViewModel, null, function (json) {
                    initializeTab(json, audiosViewModel);
                });
            }
        });

        // Attach to videos tab selector
        dialogContainer.find(selectors.tabVideosSelector).on('click', function () {
            var tabContainer = dialogContainer.find(selectors.tabVideosContainer);
            if (videosViewModel == null) {
                videosViewModel = new MediaItemsViewModel(tabContainer, links.loadVideosUrl, dialogContainer);
                videosViewModel.spinContainer = tabContainer.parents(selectors.spinContainer);

                loadTabData(videosViewModel, null, function (json) {
                    initializeTab(json, videosViewModel);
                });
            }
        });

        // Attach to files tab selector
        dialogContainer.find(selectors.tabFilesSelector).on('click', function () {
            var tabContainer = dialogContainer.find(selectors.tabFilesContainer);
            if (filesViewModel == null) {
                filesViewModel = new MediaItemsViewModel(tabContainer, links.loadFilesUrl, dialogContainer);
                filesViewModel.spinContainer = tabContainer.parents(selectors.spinContainer);

                loadTabData(filesViewModel, null, function (json) {
                    initializeTab(json, filesViewModel);
                });
            }
        });

        var imagesTabContainer = dialogContainer.find(selectors.tabImagesContainer);
        imagesViewModel = new MediaItemsViewModel(imagesTabContainer, links.loadImagesUrl, dialogContainer);
        imagesViewModel.spinContainer = imagesTabContainer.parents(selectors.spinContainer);
        initializeTab(content, imagesViewModel);
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

            self.createUrl = function (url) {
                if (!url) {
                    return url;
                }

                if (url.indexOf('?') < 0) {
                    url = url + '?version=' + self.version();
                } else {
                    url = url + '&version=' + self.version();
                }

                return url;
            };

            self.versionedThumnailUrl = ko.computed(function () {
                return self.createUrl(self.thumbnailUrl());
            });

            self.setImage = function (imageData) {
                self.thumbnailUrl(imageData.ThumbnailUrl);
                self.url(imageData.ImageUrl);
                self.tooltip(imageData.ImageTooltip);
                self.id(imageData.ImageId);
                self.version(imageData.ImageVersion);
            };

            if (image) {
                self.setImage(image);
            }
        };

        media.ImageSelectorViewModel.prototype.preview = function (data, event) {
            bcms.stopEventPropagation(event);

            var previewUrl = this.createUrl(this.url()),
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
                onMediaSelect = function (insertedImage) {
                    self.thumbnailUrl(insertedImage.thumbnailUrl);
                    self.url(insertedImage.publicUrl());
                    self.tooltip(insertedImage.tooltip);
                    self.id(insertedImage.id());
                    self.version(insertedImage.version());

                    self.onAfterSelect();
                },
                mediasViewModelExtender = {
                    onMediaSelect: function (image) {
                        onMediaSelect(image);
                    }
                },
                onMediaSelectClose = function () {
                    self.onSelectClose();
                };
            
            bcms.stopEventPropagation(event);
            media.openImageInsertDialog(onMediaSelect, false, mediasViewModelExtender, onMediaSelectClose);
        };

        media.ImageSelectorViewModel.prototype.onAfterSelect = function () { };

        media.ImageSelectorViewModel.prototype.onAfterRemove = function () { };

        media.ImageSelectorViewModel.prototype.onAfterPreview = function () { };
        
        media.ImageSelectorViewModel.prototype.onSelectClose = function () { };

        return media.ImageSelectorViewModel;
    })();

    /**
    * Initializes media module.
    */
    media.init = function () {
        console.log('Initializing bcms.media module.');

        /**
        * Subscribe to events.
        */
        bcms.on(htmlEditor.events.insertImage, onOpenImageInsertDialog);
        bcms.on(htmlEditor.events.insertFile, onOpenFileInsertDialog);
    };

    /**
    * Register initialization.
    */
    bcms.registerInit(media.init);

    return media;
});