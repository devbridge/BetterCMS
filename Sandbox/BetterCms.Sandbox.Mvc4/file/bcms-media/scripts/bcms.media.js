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
                pathCurrentFolderIdField: '#bcms-current-media-folder-id',
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
                folderNameOldValue: '.bcms-editor-item-old-name',
                changeFolderLink: '.bcms-media-folder-box .bcms-system-folder-name',
                pathFolderLink: '.bcms-breadcrumbs-root'
            },
            links = {
                loadSiteSettingsMediaManagerUrl: null,
                getImageTabUrl: null,
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
                mediaEdit: 'mediaEdit',
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
            };

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
        function MediaItemsViewModel() {
            var self = this;
            
            self.medias = ko.observableArray();
            self.path = ko.observable();

            self.isRootFolder = function () {
                return false;
                if (self.path != null && self.path.folders != null && self.path.folders.length > 1) {
                    return false;
                }
                return true;
            };
        }
        
        /**
        * Media path view model
        */
        function MediaPathViewModel() {
            var self = this;

            self.currentFolder = ko.observable();
            self.folders = ko.observableArray();
        }

        /**
        * Media view model
        */
        function MediaViewModel(item) {
            var self = this;
            
            self.id = item.Id;
            self.name = item.Name;
            self.version = item.Version;
            self.type = item.Type;
            self.isActive = ko.observable(false);
            self.contentType = item.ContentType;
            
            self.isFolder = function() {
                return self.contentType == contentTypes.folder;
            };
            
            self.isFile = function () {
                return self.contentType == contentTypes.file;
            };
        }

        /**
        * Image view model
        */
        function MediaImageViewModel(item) {
            var self = this;

            self.media = new MediaViewModel(item);
            
            self.tooltip = item.PreviewUrl;
            self.previewUrl = item.Tooltip;
        }
        
        /**
        * Audio view model
        */
        function MediaAudioViewModel(item) {
            var self = this;

            self.media = new MediaViewModel(item);
        }
        
        /**
        * Video view model
        */
        function MediaVideoViewModel(item) {
            var self = this;

            self.media = new MediaViewModel(item);
        }
        
        /**
        * File view model
        */
        function MediaFileViewModel(item) {
            var self = this;

            self.media = new MediaViewModel(item);
        }

        /**
        * Folder view model
        */
        function MediaFolderViewModel(item) {
            var self = this;

            self.media = new MediaViewModel(item);
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
                            media.attachEvents(dialog.container);
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
        * Open image editor.
        */
        media.imageEditClicked = function (item) {
            var id = $(item).data('id');
            imageEditor.onEditImage(id, function (data) {
                $(item).data('version', data.Version);
                $(item).parents(selectors.editMediaItem).find(selectors.editMediaItemFileName).html(data.FileName);
            });
        };

        /**
        * TODO: implement.
        */
        media.fileEditClicked = function (item) {
            alert('Edit file name clicked!');
            return;
            var container = $(item).parents(selectors.editMediaItem),
                showHideEditControls = function(show) {
                    if (show) {
                        container.addClass(classes.editableListItem);
                    }
                    else {
                        container.removeClass(classes.editableListItem);
                    }
                };
            showHideEditControls(true);

            // Attach OK and Cancel.
            container.find(selectors.mediaEditOk).on('click', function() {
                alert('Ok clicked.');
                // OK: post to rename, change back GUI.
                var id = $(item).data('id'),
                    version = $(item).data('version'),
                    name = container.find(selectors.mediaEditFileNameInput).val();
                // TODO: implement.
                showHideEditControls(false);
            });
            container.find(selectors.mediaEditCancel).on('click', function () {
                showHideEditControls(false);
            });
        };

        /**
        * Open image delete confirmation and delete image.
        */
        media.imageDeleteClicked = function (item, container) {
            var id = $(item).data("id"),
                version = $(item).data("version"),
                name = $(item).data("name"),
                url = $.format(links.deleteImageUrl, id, version),
                message = $.format(globalization.deleteImageConfirmMessage, name),
                onDeleteCompleted = function (json) {
                    messages.refreshBox(container, json);
                    if (json.Success) {
                        $(item).parents(selectors.imageItemParentContainer).first().remove();
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
            // Attach to the upload image link.
            tabContainer.find(selectors.uploadFileLink).on('click', function () {
                var link = $(this),
                    currentFolderId = link.parents(selectors.firstForm).find(selectors.pathCurrentFolderIdField).val(),
                    currentFolderType = link.parents(selectors.firstForm).find(selectors.pathCurrentFolderTypeField).val();
                mediaUpload.openUploadFilesDialog(currentFolderId, currentFolderType);
            });

            // Attach to switch view layout link.
            tabContainer.find(selectors.switchViewLink).on('click', function () {
                media.switchView(this, tabContainer);
            });

            // Attach to switch path.
            tabContainer.find(selectors.pathFolderLink).on('click', function () {
                media.changeFolder(tabContainer, $(this).data('folderId'));
            });

            // Setup inline editor.
            var inlineEditSelectors = {
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
            editor.initialize(tabContainer, inlineEditOpts, inlineEditSelectors);

            // Attach to add new folder link.
            tabContainer.find(selectors.addNewFolderLink).on('click', function () {
                editor.addNewRow(tabContainer);
            });

            // Attach sort events.
            var form = tabContainer.find(selectors.imageListForm),
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
            });

            // Attach to the media edit link.
            tabContainer.find(selectors.editingIcon).on('click', function () {
                var type = $(this).data('mediaType');
                if (type == 1) {
                    media.imageEditClicked(this);
                } else if (type == 2) {
                    alert('TODO: edit video file.');
                } else if (type == 3) {
                    alert('TODO: edit audio file.');
                } else if (type == 4) {
                    media.fileEditClicked(this);
                }
            });

            // Attach to the media delete link.
            tabContainer.find(selectors.deletingIconNonFolder).on('click', function () {
                var type = $(this).data('mediaType');
                if (type == 1) {
                    media.imageDeleteClicked(this, tabContainer);
                } else if (type == 2) {
                    alert('TODO: delete video file.');
                } else if (type == 3) {
                    alert('TODO: delete audio file.');
                } else if (type == 4) {
                    alert('TODO: delete file.');
                }
            });

            // Attach to the media select.
            tabContainer.find(selectors.editMediaItem).on('click', function () {
                tabContainer.find(selectors.selectedMediaImage).each(function () {
                    $(this).removeClass(classes.activeListItem);
                });
                var type = $(this).find(selectors.editingIcon).data('mediaType');
                if (type == 1) {
                    $(this).addClass(classes.activeListItem);
                }
            });

            // Attach to folder link click.
            tabContainer.find(selectors.changeFolderLink).on('click', function () {
                media.changeFolder(tabContainer, $(this).data('id'));
            });
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
        media.changeFolder = function(tabContainer, id) {
            var url = $.format("{0}?{1}={2}", links.getImageTabUrl, parameterNames.currentFolderId, id),
                wasGrid = $(selectors.switchViewLinkActive).hasClass(classes.gridView),
                form = tabContainer.find(selectors.imageListForm);

            $.ajax({
                type: 'GET',
                contentType: 'application/x-www-form-urlencoded',
                dataType: 'html',
                cache: false,
                url: url,
                success: function(data) {
                    form.parent().empty().append(data);
                    attachEvents(tabContainer);
                    // Restore grid
                    if (wasGrid) {
                        var switcher = $(selectors.switchViewLinkGrid);
                        media.switchView(switcher, tabContainer);
                    }
                }
            });
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
        * Load tab contents
        */
        function initializeTab(tabContainer, url) {
            var dialog = siteSettings.getModalDialog(),
                onComplete = function (json) {
                    var folderViewModel = new MediaItemsViewModel(),
                        pathViewModel = new MediaPathViewModel(),
                        context = tabContainer.find(selectors.templateDataBind).get(0),
                        i;

                    messages.refreshBox(dialog.container, json);
                    if (json.Success) {
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

                                folderViewModel.path = pathViewModel;
                            }

                            // Map media items
                            if (json.Data.Items && $.isArray(json.Data.Items)) {
                                for (i = 0; i < json.Data.Items.length; i ++) {
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

                            ko.applyBindings(folderViewModel, context);
                        }
                        attachEvents(tabContainer);
                    }
                };

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
        }

        /**
        * Initializes images tabs
        */
        function initializeImagesTab(dialogContainer) {
            var tabContainer = dialogContainer.find(selectors.tabImagesContainer);

            if (!tabsInitialized.images) {
                tabsInitialized.images = true;

                initializeTab(tabContainer, links.loadImagesUrl);
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

                    initializeTab(tabContainer, 'TODO: audio url');
                }
            });
            
            // Attach to videos tab selector
            dialogContainer.find(selectors.tabVideosSelector).on('click', function () {
                var tabContainer = dialogContainer.find(selectors.tabVideosContainer);
                if (!tabsInitialized.videos) {
                    tabsInitialized.videos = true;

                    initializeTab(tabContainer, 'TODO: video url');
                }
            });
            
            // Attach to files tab selector
            dialogContainer.find(selectors.tabFilesSelector).on('click', function () {
                var tabContainer = dialogContainer.find(selectors.tabFilesContainer);
                if (!tabsInitialized.files) {
                    tabsInitialized.files = true;

                    initializeTab(tabContainer, 'TODO: files url');
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
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(media.init);

        return media;
    });