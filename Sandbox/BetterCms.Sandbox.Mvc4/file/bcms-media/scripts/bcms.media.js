/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.media', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'bcms.media.upload', 'bcms.media.imageeditor', 'bcms.htmlEditor', 'bcms.inlineEdit', 'bcms.grid', 'knockout'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, mediaUpload, imageEditor, htmlEditor, editor, grid, ko) {
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
                
                // Other:
                uploadFileLink: '.bcms-media-new-file',
                firstForm: 'form:first',
                currentFolderField: '.bcms-breadcrumbs-holder>input:hidden',
                pathCurrentFolderTypeField: '#bcms-current-media-folder-type',
                editingIcon: '.bcms-icn-edit',
                editMediaItem: '.bcms-media-file-box',
                editMediaItemFileName: '.bcms-system-folder-name',
                deletingIcon: '.bcms-icn-delete',
                deletingIconNonFolder: '.bcms-media-file-box .bcms-icn-delete',
                imageItemParentContainer: '.bcms-media-file-box, .bcms-media-folder-box',
                switchViewLink: '.bcms-view-switch',
                switchViewLinkActive: '.bcms-view-switch.bcms-select-active',
                switchViewLinkGrid: '#bcms-grid-view',
                selectedMediaImage: '.bcms-table-click-active',
                switchContainer: '.bcms-grid-style, .bcms-list-style',
                mediaEditControls: '.bcms-media-edit-controls',
                imageListForm: '#bcms-images-tag-form',
                imageListSearch: '.bcms-btn-search',
                imageListSearchBox: '.bcms-search-block .bcms-editor-field-box',
                addNewFolderLink: '.bcms-media-new-folder, #bcms-add-new-folder, .bcms-system-addfolder',
                sortingHeader: '.bcms-media-sorting-block:first',
                listFirstRow: '.bcms-media-folder-box:first',
                listFirstCell: '.bcms-media-folder-box:first',
                listFirstTable: '.bcms-list-style:first, .bcms-grid-style:first',
                templateFirstRow: '.bcms-media-folder-box:first',
                listEditFolderLink: '.bcms-icn-edit',
                listSaveFolderLink: '.bcms-media-inner-controls .bcms-btn-small',
                listCancelFolderLink: '.bcms-media-inner-controls .bcms-btn-links-small',
                listDeleteFolderLink: '.bcms-media-folder-box a.bcms-icn-delete',
                listFolderInputField: '.bcms-media-edit-controls .bcms-editor-field-box',
                listFolderInputValue: '.bcms-system-folder-name',
                listEmptyRow: '.bcms-list-empty-row',
                listAnyRow: '.bcms-media-folder-box, .bcms-media-file-box',
                folderNameEditor: '.bcms-editor-item-name',
                folderNameDiv: '.bcms-system-folder-name',
                folderNameOldValue: '.bcms-editor-item-old-name'
            },
            links = {
                loadSiteSettingsMediaManagerUrl: null,
                loadImagesUrl: null,
                
                // Other:
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
            },
            events = {
                mediaEdit: 'mediaEdit'
            },
            classes = {
                gridView: 'bcms-select-grid',
                listView: 'bcms-select-list',
                gridViewStyle: 'bcms-grid-style',
                listViewStyle: 'bcms-list-style',
                activeSwitch: 'bcms-select-active',
                activeListItem: 'bcms-table-click-active',
                editableListItem: 'bcms-media-row-active',
                activeMediaRow: 'bcms-media-row-active'
            },
            parameterNames = {
                currentFolderId: 'CurrentFolderId'
            },
            tabsInitialized = {
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
            imagesViewModel = null,
            audiosViewModel = null,
            videosViewModel = null,
            filesViewModel = null;

        /**
        * Assign objects to module.
        */
        media.links = links;
        media.globalization = globalization;
        media.events = events;
        media.classes = classes;
        media.parameterNames = parameterNames;

        /**
        * Media's current folder view model
        */
        function MediaItemsViewModel(container) {
            var self = this;
            
            self.medias = ko.observableArray();
            self.path = ko.observable();
            self.container = container;
            self.sortOptions = ko.observable();

            self.isRootFolder = function () {
                if (self.path != null && self.path().folders != null && self.path().folders().length > 1) {
                    return false;
                }
                return true;
            };

            self.addNewFolder = function () {
                var newFolder = new MediaFolderViewModel({
                    isActive: true,
                    type: self.path().currentFolder().type
                });

                self.medias.unshift(newFolder);
            };

            self.uploadMedia = function () {
                mediaUpload.openUploadFilesDialog(self.path().currentFolder().id(), self.path().currentFolder().type);
            };

            self.searchMedia = function() {
                alert("TODO: Search media...");
            };

            self.sortMedia = function() {
                alert("TODO: Sort media...");
            };
        }
        
        /**
        * Media path view model
        */
        function MediaPathViewModel() {
            var self = this;

            self.currentFolder = ko.observable();
            self.folders = ko.observableArray();
            
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
                    range[0].name('...');
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
                self.version = ko.observable(item.Version);
                self.type = item.Type;
                
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
                throw new Error("Cancel edit media method is not implemented");
            };

            MediaItemBaseViewModel.prototype.selectMedia = function (folderViewModel) {
                // Nothing to do
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
            
            MediaItemBaseViewModel.prototype.selectMedia = function (folderViewModel) {
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

                self.pathName = ko.computed(function() {
                    return '\\' + self.name();
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

            return MediaFolderViewModel;
        })(MediaItemBaseViewModel);

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
                            attachEvents(dialog.container);
                        },
                    });
                },
                onAcceptClick: function (dialog) {
                    var selectedItem = dialog.container.find(selectors.selectedMediaImage);
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
                    return true;
                },
            });
        };

        /**
        * Insert image to htmlEditor.
        */
        media.addImageToEditor = function (imgEditor, imageUrl, imageAlign) {
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
            // Attach to switch view layout link.
            tabContainer.find(selectors.switchViewLink).on('click', function () {
                media.switchView(this, tabContainer);
            });

            // Setup inline editor.
            /*var inlineEditSelectors = {
                firstRow: selectors.listFirstRow,
                firstCell: selectors.listFirstCell,
                firstTable: selectors.listFirstTable,
                templateFirstRow: selectors.templateFirstRow,
                editRowLink: selectors.listEditFolderLink,
                saveRowLink: selectors.listSaveFolderLink,
                cancelLink: selectors.listCancelFolderLink,
                deleteRowLink: selectors.listDeleteFolderLink,
                fieldInputs: selectors.listFolderInputField,
                fieldVisibleValue: selectors.listFolderInputValue
            };
            var inlineEditOpts = {
                saveUrl: links.saveFolderUrl,
                deleteUrl: links.deleteFolderUrl,
                newRowAdder: media.appenNewRow,
                switchRowToEdit: media.switchRowToEdit,
                switchRowToView: media.switchRowToView,
                onSaveSuccess: media.setFolderFields,
                rowDataExtractor: media.getFolderData,
                showHideEmptyRow: media.showHideEmptyRow,
                deleteRowMessageExtractor: function (rowData) {
                    return $.format(globalization.confirmDeleteFolderMessage, rowData.Name);
                }
            };
            editor.initialize(tabContainer, inlineEditOpts, inlineEditSelectors);*/

            // Attach to add new folder link.
            /*tabContainer.find(selectors.addNewFolderLink).on('click', function () {
                editor.addNewRow(tabContainer);
            });*/

            // Attach sort events.
            /*var form = tabContainer.find(selectors.imageListForm),
                callBack = function (data) {
                    var wasGrid = $(selectors.switchViewLinkActive).hasClass(classes.gridView);
                    form.parent().empty().append(data);
                    attachEvents(tabContainer);
                    // Restore grid
                    if (wasGrid) {
                        var switcher = $(selectors.switchViewLinkGrid);
                        media.switchView(switcher, tabContainer);
                    }
                };
            grid.bindGridForm(form, callBack);
            form.on('submit', function (event) {
                event.preventDefault();
                grid.submitGridForm(form, callBack);
                return false;
            });

            // Attach search events.
            form.find(selectors.imageListSearch).on('click', function () {
                grid.submitGridForm(form, callBack);
            });
            bcms.preventInputFromSubmittingForm(form.find(selectors.imageListSearchBox), {
                preventedEnter: function () {
                    grid.submitGridForm(form, callBack);
                },
            });*/
        };

        /**
        * Shows or hides div with information about empty folder
        */
        media.showHideEmptyRow = function(container) {
            if (container.find(selectors.listAnyRow).length == 0) {
                container.find(selectors.listEmptyRow).show();
            } else {
                container.find(selectors.listEmptyRow).hide();
            }
        };

        /**
        * Changes current folder.
        */
        function changeFolder(id, folderViewModel) {
            var url = $.format("{0}?{1}={2}", links.loadImagesUrl, parameterNames.currentFolderId, id),
                onComplete = function (json) {
                    parseJsonResults(json, folderViewModel);
                };
            loadTabData(url, onComplete);
        };

        /**
        * Switches items view style.
        */
        media.switchView = function (switcher, tabContainer) {
            switcher = $(switcher);
            if (!switcher.hasClass(classes.activeSwitch)) {
                tabContainer.find(selectors.switchViewLink).removeClass(classes.activeSwitch);
                switcher.addClass(classes.activeSwitch);

                var container = tabContainer.find(selectors.switchContainer);
                if (switcher.hasClass(classes.gridView)) {
                    container.removeClass(classes.listViewStyle);
                    container.addClass(classes.gridViewStyle);
                }
                else {
                    container.removeClass(classes.gridViewStyle);
                    container.addClass(classes.listViewStyle);
                }
            }
        };

        /**
        * Prepends new editable row for inline editing.
        */
        media.appenNewRow = function(newRow, container) {
            container.find(selectors.sortingHeader).after(newRow);
        };

        /**
        * Switches row to inline edit mode.
        */
        media.switchRowToEdit = function (row) {
            row.addClass(classes.activeMediaRow);
            row.find(selectors.folderNameEditor).focus();
            row.find(selectors.listSaveFolderLink).show();
            row.find(selectors.listCancelFolderLink).show();
        };

        /**
        * Switches row from inline edit mode.
        to view mode
        */
        media.switchRowToView = function (row) {
            row.removeClass(classes.activeMediaRow);
        };

        /**
        * Retrieves folder field values from row.
        */
        media.getFolderData = function (row) {
            var folderId = row.find(selectors.deletingIcon).data('id'),
                folderVersion = row.find(selectors.deletingIcon).data('version'),
                name = row.find(selectors.folderNameEditor).val(),
                type = row.parents(selectors.firstForm).find(selectors.pathCurrentFolderTypeField).val();

            return {
                Id: folderId,
                Version: folderVersion,
                Name: name,
                Type: type
            };
        };

        /**
        * Set values, returned from server to row fields.
        */
        media.setFolderFields = function (row, json) {
            if (json.Data) {
                row.find(selectors.folderNameDiv).html(json.Data.Name);
                row.find(selectors.folderNameEditor).val(json.Data.Name);
                row.find(selectors.folderNameOldValue).val(json.Data.Name);
            }
        };

        /**
        * Parse json result and map data to view model
        */
        function parseJsonResults(json, folderViewModel) {
            var dialog = siteSettings.getModalDialog(),
                pathViewModel = new MediaPathViewModel(),
                i;

            messages.refreshBox(dialog.container, json);

            if (json.Success) {
                
                folderViewModel.medias.removeAll();

                if (json.Data) {

                    // Map media path
                    if (json.Data.Path) {
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
                    if (json.Data.Items && $.isArray(json.Data.Items)) {

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
                }

                return true;
            }

            return false;
        }

        /**
        * Load tab data
        */
        function loadTabData(url, onComplete) {
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
        }

        /**
        * Load tab contents and attach events
        */
        function initializeTab(tabContainer, url, folderViewModel, onAfterComplete) {
            var onComplete = function(json) {

                var context = tabContainer.find(selectors.templateDataBind).get(0);

                if (parseJsonResults(json, folderViewModel)) {
                    ko.applyBindings(folderViewModel, context);
                    
                    if ($.isFunction(onAfterComplete)) {
                        onAfterComplete();
                    }
                }
            };

            loadTabData(url, onComplete);
        }

        /**
        * Initializes images tabs
        */
        function initializeImagesTab(dialogContainer) {
            var tabContainer = dialogContainer.find(selectors.tabImagesContainer);

            if (!tabsInitialized.images) {
                tabsInitialized.images = true;
                imagesViewModel = new MediaItemsViewModel(tabContainer);

                initializeTab(tabContainer, links.loadImagesUrl, imagesViewModel, function () {
                    attachEvents(tabContainer);
                });
            }
        }

        /**
        * Initializes media manager.
        */
        function initializeSiteSettingsMediaManager() {
            tabsInitialized = {
                images: false,
                audios: false,
                videos: false,
                files: false
            };

            var dialogContainer = siteSettings.getModalDialog().container;

            // Attach to images tab selector
            dialogContainer.find(selectors.tabImagesSelector).on('click', function () {
                initializeImagesTab(dialogContainer);
            });
            
            // Attach to audios tab selector
            dialogContainer.find(selectors.tabAudiosSelector).on('click', function () {
                var tabContainer = dialogContainer.find(selectors.tabAudiosContainer);
                if (!tabsInitialized.audios) {
                    tabsInitialized.audios = true;
                    audiosViewModel = new MediaItemsViewModel(tabContainer);

                    initializeTab(tabContainer, 'TODO: audio url', audiosViewModel, function () {
                        attachEvents(tabContainer);
                    });
                }
            });
            
            // Attach to videos tab selector
            dialogContainer.find(selectors.tabVideosSelector).on('click', function () {
                var tabContainer = dialogContainer.find(selectors.tabVideosContainer);
                if (!tabsInitialized.videos) {
                    tabsInitialized.videos = true;
                    videosViewModel = new MediaItemsViewModel(tabContainer);

                    initializeTab(tabContainer, 'TODO: video url', videosViewModel, function () {
                        attachEvents(tabContainer);
                    });
                }
            });
            
            // Attach to files tab selector
            dialogContainer.find(selectors.tabFilesSelector).on('click', function () {
                var tabContainer = dialogContainer.find(selectors.tabFilesContainer);
                if (!tabsInitialized.files) {
                    tabsInitialized.files = true;
                    filesViewModel = new MediaItemsViewModel(tabContainer);

                    initializeTab(tabContainer, 'TODO: files url', filesViewModel, function () {
                        attachEvents(tabContainer);
                    });
                }
            });
            
            initializeImagesTab(dialogContainer);
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
            
            //
            // TODO: remove after tests
            // 
            $(function() {
                siteSettings.openSiteSettings();
                media.loadSiteSettingsMediaManager();
            });
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(media.init);

        return media;
    });