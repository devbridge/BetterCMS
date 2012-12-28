/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.media', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'bcms.media.upload', 'bcms.media.imageeditor', 'bcms.htmlEditor', 'knockout'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, mediaUpload, imageEditor, htmlEditor, ko) {
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
                firstForm: 'form:first'
            },
            links = {
                loadSiteSettingsMediaManagerUrl: null,
                loadImagesUrl: null,
                loadFilesUrl: null,
                insertImageDialogUrl: null,
                deleteImageUrl: null,
                getImageUrl: null,
                saveFolderUrl: null
            },
            globalization = {
                insertImageDialogTitle: null,
                insertImageFailureMessageTitle: null,
                insertImageFailureMessageMessage: null,
                deleteImageConfirmMessage: null,
                confirmDeleteFolderMessage: null,
                imageNotSelectedMessageTitle: null,
                imageNotSelectedMessageMessage: null,
                
                imagesTabTitle: null,
                audiosTabTitle: null,
                videosTabTitle: null,
                filesTabTitle: null,
                
                uploadImage: null,
                uploadAudio: null,
                uploadVideo: null,
                uploadFile: null,
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
            staticDomId = 1;

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
            var self = this;

            self.container = container;
            self.url = url;
            self.messagesContainer = messagesContainer;
            
            self.medias = ko.observableArray();
            self.path = ko.observable();
            self.isGrid = ko.observable(false);
            self.canSelectMedia = ko.observable(false);
            
            self.gridOptions = ko.observable();

            self.isRootFolder = function () {
                if (self.path != null && self.path().folders != null && self.path().folders().length > 1) {
                    return false;
                }
                return true;
            };

            self.addNewFolder = function () {
                var newFolder = new MediaFolderViewModel({
                    IsActive: true,
                    Type: self.path().currentFolder().type
                });

                self.medias.unshift(newFolder);
            };

            self.uploadMedia = function () {
                mediaUpload.openUploadFilesDialog(self.path().currentFolder().id(), self.path().currentFolder().type);
            };

            self.searchMedia = function () {
                var params = createFolderParams(self.path().currentFolder().id(), self),
                    onComplete = function (json) {
                        parseJsonResults(json, self);
                    };
                loadTabData(self.url, params, onComplete);
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
                loadTabData(self.url, params, onComplete);
            };

            self.switchViewStyle = function() {
                self.isGrid(!self.isGrid());
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
        * Media item base view model
        */
        var MediaItemBaseViewModel = (function () {
            function MediaItemBaseViewModel(item) {
                var self = this;

                self.id = ko.observable(item.Id);
                self.name = ko.observable(item.Name);
                self.oldName = item.Name;
                self.version = ko.observable(item.Version);
                self.type = item.Type;
                self.nameDomId = 'name_' + staticDomId++;
                
                self.isActive = ko.observable(item.IsActive || false);
                self.isSelected = ko.observable(false);

                self.isFile = function () {
                    return !self.isFolder();
                };
            }
            
            MediaItemBaseViewModel.prototype.isFolder = function () {
                return false;
            };

            MediaItemBaseViewModel.prototype.isImage = function () {
                return false;
            };

            MediaItemBaseViewModel.prototype.deleteMedia = function (folderViewModel) {
                throw new Error("Delete method is not implemented");
            };
            
            MediaItemBaseViewModel.prototype.openMedia = function (folderViewModel) {
                throw new Error("Open method is not implemented");
            };
            
            MediaItemBaseViewModel.prototype.editMedia = function (folderViewModel) {
                throw new Error("Edit media method is not implemented");
            };
            
            MediaItemBaseViewModel.prototype.saveMedia = function (folderViewModel) {
                throw new Error("Save media method is not implemented");
            };
            
            MediaItemBaseViewModel.prototype.cancelEditMedia = function (folderViewModel) {
                cancelEditMedia(folderViewModel, this);
            };

            MediaItemBaseViewModel.prototype.selectMedia = function (folderViewModel) {
                this.openMedia(folderViewModel);
            };

            MediaItemBaseViewModel.prototype.blurMediaField = function (folderViewModel) {
                throw new Error("Blur editable media field method is not implemented");
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

                self.tooltip = item.PreviewUrl;
                self.previewUrl = item.Tooltip;
            }

            MediaImageViewModel.prototype.isImage = function () {
                return true;
            };

            MediaImageViewModel.prototype.deleteMedia = function (folderViewModel) {
                var url = $.format(links.deleteImageUrl, this.id(), this.version()),
                    message = $.format(globalization.deleteImageConfirmMessage, this.name());

                deleteMediaItem(url, message, folderViewModel, this);
            };

            MediaImageViewModel.prototype.editMedia = function (folderViewModel) {
                var self = this;
                imageEditor.onEditImage(this.id(), function (data) {
                    self.version(data.Version);
                    self.name(data.FileName);
                });
            };
            
            MediaImageViewModel.prototype.selectMedia = function (folderViewModel) {
                // Call base, if view model is not in selectable mode
                if (!folderViewModel.canSelectMedia()) {
                    _super.prototype.selectMedia.call(this, folderViewModel);
                    return;
                }
                
                for (var i = 0; i < folderViewModel.medias().length; i++) {
                    folderViewModel.medias()[i].isSelected(false);
                }
                this.isSelected(true);
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
        });
        
        /**
        * Media video view model
        */
        var MediaVideoViewModel = (function(_super) {
            bcms.extendsClass(MediaVideoViewModel, _super);

            function MediaVideoViewModel(item) {
                _super.call(this, item);
            }
        });
        
        /**
        * File view model
        */
        var MediaFileViewModel = (function(_super) {
            bcms.extendsClass(MediaFileViewModel, _super);

            function MediaFileViewModel(item) {
                _super.call(this, item);
            }
        });

        /**
        * Media folder view model
        */
        var MediaFolderViewModel = (function(_super) {
            bcms.extendsClass(MediaFolderViewModel, _super);

            function MediaFolderViewModel(item) {
                _super.call(this, item);

                var self = this;

                self.pathName = ko.computed(function () {
                    return self.name() + '/';
                });
            }

            MediaFolderViewModel.prototype.isFolder = function () {
                return true;
            };

            MediaFolderViewModel.prototype.deleteMedia = function (folderViewModel) {
                var url = $.format(links.deleteFolderUrl, this.id(), this.version()),
                    message = $.format(globalization.confirmDeleteFolderMessage, this.name());

                deleteMediaItem(url, message, folderViewModel, this);
            };

            MediaFolderViewModel.prototype.openMedia = function (folderViewModel) {
                changeFolder(this.id(), folderViewModel);
            };
            
            MediaFolderViewModel.prototype.editMedia = function (folderViewModel) {
                this.isActive(true);
            };
            
            MediaFolderViewModel.prototype.saveMedia = function (folderViewModel) {
                saveMedia(folderViewModel, this, links.saveFolderUrl);
            };
            
            MediaFolderViewModel.prototype.blurMediaField = function (folderViewModel) {
                var item = this;
                setTimeout(function () {
                    if (!item.name() && !item.id()) {
                        cancelEditMedia(folderViewModel, item);
                    } else {
                        saveMedia(folderViewModel, item, links.saveFolderUrl);
                    }
                }, 500);
            };

            return MediaFolderViewModel;
        })(MediaItemBaseViewModel);

        /**
        * Cancels inline editing of media
        */
        function cancelEditMedia(folderViewModel, item) {
            item.isActive(false);
            if (!item.id()) {
                folderViewModel.medias.remove(item);
            } else {
                item.name(item.oldName);
                item.isActive(false);
            }
        }
        
        /**
        * Saves media after inline edit
        */
        function saveMedia(folderViewModel, item, url) {
            var idSelector = '#' + item.nameDomId,
                input = folderViewModel.container.find(idSelector);
            
            if (item.oldName != item.name() && item.isActive() && input != null) {

                if (input.valid()) {
                    var onSaveCompleted = function(json) {
                        messages.refreshBox(folderViewModel.container, json);
                        if (json.Success) {
                            if (json.Data) {
                                item.version(json.Data.Version);
                                item.id(json.Data.Id);
                                item.oldName = item.name();
                            }
                            item.isActive(false);
                        }
                    },
                        params = item.toJson();

                    $.ajax({
                        url: url,
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
                }
            } else {
                item.isActive(false);
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
        media.onInsertImage = function(imgEditor) {
            modal.open({
                title: globalization.insertImageDialogTitle,
                acceptTitle: 'Insert',
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, links.insertImageDialogUrl, {
                        done: function (content) {
                            imagesViewModel = new MediaItemsViewModel(dialog.container, links.loadImagesUrl, dialog.container);
                            imagesViewModel.canSelectMedia(true);
                            initializeTab(content.Data.Data, imagesViewModel);
                        },
                    });
                },
                onAcceptClick: function (dialog) {
                    var i,
                        item,
                        selectedItem = null,
                        alertOnError = function() {
                            modal.alert({
                                title: globalization.insertImageFailureMessageTitle,
                                content: globalization.insertImageFailureMessageMessage,
                            });
                        };
                    
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
                            title: globalization.imageNotSelectedMessageTitle,
                            content: globalization.imageNotSelectedMessageMessage,
                            disableCancel: true,
                        });
                        return false;
                    }

                    var url = $.format(links.getImageUrl, selectedItem.id());
                    $.ajax({
                        url: url,
                        type: "POST",
                        dataType: 'json'
                    }).done(function (json) {
                        if (json.Success && json.Data != null) {
                            addImageToEditor(imgEditor, json.Data.Url, json.Data.ImageAlign);
                            dialog.close();
                        } else {
                            alertOnError();
                        }
                    }).fail(function () {
                        alertOnError();
                    });

                    /*var selectedItem = dialog.container.find(selectors.selectedMediaImage);
                    var editItem = $(selectedItem).find(selectors.editingIcon);
                    if (editItem.data('mediaType') == 1) {
                        var imageId = editItem.data('id'),
                            url = $.format(links.getImageUrl, imageId),
                            alertOnError = function () {
                                modal.alert({
                                    title: globalization.insertImageFailureMessageTitle,
                                    content: globalization.insertImageFailureMessageMessage,
                                });
                            };
                        $.ajax({
                            url: url,
                            type: "POST",
                            dataType: 'json'
                        }).done(function (json) {
                            if (json.Success && json.Data != null) {
                                media.addImageToEditor(imgEditor, json.Data.Url, json.Data.ImageAlign);
                                dialog.close();
                            } else {
                                alertOnError();
                            }
                        }).fail(function () {
                            alertOnError();
                        });
                    } else {
                        modal.info({
                            title: globalization.imageNotSelectedMessageTitle,
                            content: globalization.imageNotSelectedMessageMessage,
                            disableCancel: true,
                        });
                        return false;
                    }
                    return true;*/
                },
            });
        };

        /**
        * Insert image to htmlEditor.
        */
        function addImageToEditor(imgEditor, imageUrl, imageAlign) {
            var align = "left";
            if (imageAlign == 2) {
                align = "center";
            }
            else if (imageAlign == 3) {
                align = "right";
            }
            imgEditor.insertHtml('<img src="' + imageUrl + '" align="' + align + '"/>');
        };

        /**
        * Open delete confirmation and delete media item.
        */
        function deleteMediaItem(url, message, folderViewModel, item) {
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
        * Attach links to actions.
        */
        function attachEvents(tabContainer) {
            var form = tabContainer.find(selectors.firstForm);
            if ($.validator && $.validator.unobtrusive) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }
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
                    parseJsonResults(json, folderViewModel);
                };
            loadTabData(folderViewModel.url, params, onComplete);
        };
        
        /**
        * Parse json result and map data to view model
        */
        function parseJsonResults(json, folderViewModel) {
            var i;

            messages.refreshBox(folderViewModel.messagesContainer, json);

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
                                itemViewModel;

                            if (item.ContentType == contentTypes.folder) {
                                itemViewModel = new MediaFolderViewModel(item);
                            } else {
                                switch (item.Type) {
                                    case mediaTypes.image:
                                        itemViewModel = new MediaImageViewModel(item);
                                        break;
                                    case mediaTypes.audio:
                                        itemViewModel = new MediaAudioViewModel(item);
                                        break;
                                    case mediaTypes.video:
                                        itemViewModel = new MediaVideoViewModel(item);
                                        break;
                                    case mediaTypes.file:
                                        itemViewModel = new MediaFileViewModel(item);
                                        break;
                                    default:
                                        itemViewModel = null;
                                        break;
                                }
                            }

                            if (itemViewModel != null) {
                                folderViewModel.medias.push(itemViewModel);
                            }
                        }
                    }
                    
                    // Map grid options
                    folderViewModel.gridOptions(new MediaItemsOptionsViewModel(json.Data));
                }

                return true;
            }

            return false;
        }

        /**
        * Load tab data
        */
        function loadTabData(url, params, onComplete) {
            $.ajax({
                type: 'POST',
                cache: false,
                url: url,
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

                attachEvents(folderViewModel.container);
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
                    audiosViewModel = new MediaItemsViewModel(tabContainer, null /* TODO: add audios url */, dialogContainer);

                    loadTabData(audiosViewModel.url, null, function (json) {
                        initializeTab(json, audiosViewModel);
                    });
                }
            });
            
            // Attach to videos tab selector
            dialogContainer.find(selectors.tabVideosSelector).on('click', function () {
                var tabContainer = dialogContainer.find(selectors.tabVideosContainer);
                if (videosViewModel == null) {
                    videosViewModel = new MediaItemsViewModel(tabContainer, null /* TODO: add video url */, dialogContainer);

                    loadTabData(videosViewModel.url, null, function (json) {
                        initializeTab(json, videosViewModel);
                    });
                }
            });
            
            // Attach to files tab selector
            dialogContainer.find(selectors.tabFilesSelector).on('click', function () {
                var tabContainer = dialogContainer.find(selectors.tabFilesContainer);
                if (filesViewModel == null) {
                    filesViewModel = new MediaItemsViewModel(tabContainer, links.loadFilesUrl, dialogContainer);

                    loadTabData(filesViewModel.url, null, function (json) {
                        initializeTab(json, filesViewModel);
                    });
                }
            });
            
            var imagesTabContainer = dialogContainer.find(selectors.tabImagesContainer);
            imagesViewModel = new MediaItemsViewModel(imagesTabContainer, links.loadImagesUrl, dialogContainer);
            initializeTab(content.Data.Data, imagesViewModel);
        };

        /**
        * Initializes media module.
        */
        media.init = function () {
            console.log('Initializing bcms.media module.');

            /**
            * Subscribe to events.
            */
            bcms.on(htmlEditor.events.insertImage, media.onInsertImage);
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(media.init);

        return media;
    });