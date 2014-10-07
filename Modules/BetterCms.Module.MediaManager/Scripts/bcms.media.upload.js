/*jslint unparam: true, white: true, browser: true, devel: true, vars: true */
/*global bettercms */

bettercms.define('bcms.media.upload', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.modal', 'bcms.html5Upload', 'bcms.ko.extenders', 'bcms.messages', 'bcms.security'],
    function ($, bcms, dynamicContent, modal, html5Upload, ko, messages, security) {
    'use strict';

        var mediaUpload = {},
            selectors = {
                dragZone: '#bcms-files-dropzone',
                messageBox: "#bcms-multi-file-upload-messages",
                fileUploadingContext: '#bcms-media-uploads',
                fileUploadingMasterForm: '#SaveForm',
                fileUploadingForm: '#ImgForm',
                fileUploadingTarget: '#UploadTarget',
                fileUploadingInput: '#uploadFile',
                fileUploadingResult: '#jsonResult',
                folderDropDown: '#SelectedFolderId',
                uploadButtonLabel: '.bcms-btn-upload-files-text',
                userAccessControlContainer: '#bcms-accesscontrol-context',
                overrideSelect: "bcms-media-reupload-override"
            },
            classes = {
                dragZoneActive: 'bcms-dropzone-active'
            },
            links = {
                loadUploadFilesDialogUrl: null,
                uploadFileToServerUrl: null,
                undoFileUploadUrl: null,
                loadUploadSingleFileDialogUrl: null,
                checkUploadedFileStatuses: null
            },
            globalization = {
                uploadFilesDialogTitle: null,
                failedToProcessFile: null,
                multipleFilesWarningMessageOnReupload: null
            },
            constants = {
                defaultReuploadMediaId: '00000000-0000-0000-0000-000000000000'
            },
            fileApiSupported = html5Upload.fileApiSupported();

    /**
    * Assign objects to module.
    */
    mediaUpload.links = links;
    mediaUpload.globalization = globalization;    

    mediaUpload.openUploadFilesDialog = function (rootFolderId, rootFolderType, onSaveCallback, reuploadMediaId, onCloseCallback) {
        reuploadMediaId = reuploadMediaId || constants.defaultReuploadMediaId;
        var options = {
                uploads: new UploadsViewModel(),
                rootFolderId: rootFolderId,
                rootFolderType: rootFolderType,
                reuploadMediaId: reuploadMediaId
            };

        options.uploads.filesToAccept(rootFolderType == 1 ? 'image/*' : '');



        if (fileApiSupported) {
            modal.open({
                title: globalization.uploadFilesDialogTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.loadUploadFilesDialogUrl, rootFolderId, rootFolderType, reuploadMediaId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (dialogRef, content) {
                            initUploadFilesDialogEvents(dialog, options);
                            
                            var context = dialog.container.find(selectors.userAccessControlContainer).get(0);
                            if (context) {
                                var viewModel = {
                                    accessControl: security.createUserAccessViewModel(content.Data.UserAccessList)
                                };
                                ko.applyBindings(viewModel, context);
                            }
                        },

                        beforePost: function () {
                            dialog.container.showLoading();
                        },

                        postSuccess: function (json) {
                            options.uploads.stopStatusChecking();
                            options.uploads.removeFailedUploads();
                            if (onSaveCallback && $.isFunction(onSaveCallback)) {
                                onSaveCallback(json);
                            }
                        },

                        postComplete: function() {
                            dialog.container.hideLoading();
                        },
                        
                        postError: function () {
                            options.uploads.filesToAccept(rootFolderType == 1 ? 'image/*' : '');
                        }
                    });
                },
                onCloseClick: function () {
                    options.uploads.removeAllUploads();
                    options.uploads.stopStatusChecking();
                    
                    if (onCloseCallback && $.isFunction(onCloseCallback)) {
                        onCloseCallback();
                    }
                },
                onAcceptClick: function() {
                    // IE10 fix: remove accept tag from upload box.
                    options.uploads.filesToAccept('');
                }
            });
        } else {
            modal.open({
                title: globalization.uploadFilesDialogTitle,
                onLoad: function(dialog) {
                    var url = $.format(links.loadUploadSingleFileDialogUrl, rootFolderId, rootFolderType, reuploadMediaId);

                    dynamicContent.setContentFromUrl(dialog, url, {
                        done: function (content) {
                            options.uploads.accessControl = security.createUserAccessViewModel(content.Data.UserAccessList);
                            SingleFileUpload(dialog, options);
                        }
                    });
                },
                onAcceptClick: function(dialog) {
                    var formToSubmit = $(selectors.fileUploadingMasterForm),
                        onComplete = function (json) {
                            messages.refreshBox(dialog.container.find(selectors.messageBox), json);
                            if (json.Success) {
                                try {
                                    if (onSaveCallback && $.isFunction(onSaveCallback)) {
                                        onSaveCallback(json);
                                    }
                                } finally {
                                    options.uploads.stopStatusChecking();
                                    options.uploads.removeFailedUploads();
                                    dialog.close();
                                }
                            }
                        };
                    $.ajax({
                        type: 'POST',
                        cache: false,
                        url: formToSubmit.attr('action'),
                        data: formToSubmit.serialize()
                    })
                        .done(function(response) {
                            onComplete(response);
                        })
                        .fail(function(response) {
                            onComplete(bcms.parseFailedResponse(response));
                        });
                    
                    return false;
                },
                onCloseClick: function () {
                    options.uploads.removeAllUploads();
                    options.uploads.stopStatusChecking();
                    
                    if (onCloseCallback && $.isFunction(onCloseCallback)) {
                        onCloseCallback();
                    }
                }
            });
        }
    };

    mediaUpload.openReuploadFilesDialog = function (mediaId, rootFolderId, rootFolderType, onSaveCallback, onCloseCallback) {
        mediaUpload.openUploadFilesDialog(rootFolderId, rootFolderType, onSaveCallback, mediaId, onCloseCallback);
    };

    function SingleFileUpload(dialog, options) {
        var context = dialog.container.find(selectors.fileUploadingContext).get(0),
            messageBox = messages.box({ container: dialog.container }),
            uploadsModel = options.uploads,
            fakeData = {
                fileName: "Uploading",
                fileSize: 0,
                fileId: null,
                version: null,
                type: null,
                abort: function() { /* File upload canceling is not supported. */ }
            },
            uploadFile = new FileViewModel(fakeData),
            resetUploadForm = function() {
                var folderDropDown = dialog.container.find(selectors.folderDropDown);
                if (folderDropDown.length > 0) {
                    var selectedFolderIndex = dialog.container.find(selectors.folderDropDown).get(0).selectedIndex;
                    dialog.container.find(selectors.fileUploadingForm).get(0).reset();
                    dialog.container.find(selectors.folderDropDown).get(0).selectedIndex = selectedFolderIndex;
                }
            },
            uploadAnimationId,
            animateUpload = function (fileModel) {
                uploadAnimationId = setInterval(function() {
                    if (fileModel.uploadProgress() >= 100) {
                        fileModel.uploadProgress(0);
                    } else {
                        fileModel.uploadProgress(fileModel.uploadProgress() + 20);
                    };
                }, 200);
            },
            stopUploadAnimation = function () {
                if (uploadAnimationId) {
                    clearInterval(uploadAnimationId);
                }
            };

        dialog.container.find(selectors.uploadButtonLabel).on('click', fixUploadButtonForMozilla);

        // On folder changed
        dialog.container.find(selectors.fileUploadingForm).find(selectors.folderDropDown).on('change', function () {
            var value = $(this).val(),
                hidden = dialog.container.find(selectors.fileUploadingMasterForm).find(selectors.folderDropDown);

            hidden.val(value);
        });

        // On file selected.
        dialog.container.find(selectors.fileUploadingInput).change(function () {
            var fileName = dialog.container.find(selectors.fileUploadingInput).val();
            if (fileName != null && fileName != "") {
                // Do not allow multiple file upload on re-upload functionality.
                if (options.reuploadMediaId && options.reuploadMediaId != constants.defaultReuploadMediaId && uploadsModel.uploads().length > 0) {
                    messageBox.clearMessages();
                    messageBox.addWarningMessage(globalization.multipleFilesWarningMessageOnReupload);
                    var uploadedFiles = uploadsModel.uploads();
                    for (var i = 0; i < uploadedFiles.length; i++) {
                        uploadedFiles[i].activate();
                    }
                    return;
                }
                // Add fake file model for upload indication.
                uploadFile.uploadCompleted(false);
                uploadFile.fileName = fileName;
                uploadFile.file.fileName = fileName;
                uploadFile.uploadProgress(0);
                uploadsModel.activeUploads.push(uploadFile);
                uploadsModel.uploads.push(uploadFile);
                animateUpload(uploadFile);
                // Send file to server.
                dialog.container.find($(selectors.fileUploadingForm)).submit();
            }
        });
        
        // On file submitted.
        dialog.container.find($(selectors.fileUploadingTarget)).on('load', function () {
            stopUploadAnimation();
            uploadsModel.uploads.remove(uploadFile);
            uploadsModel.activeUploads.remove(uploadFile);
            resetUploadForm();
            
            // Check the result.
            var result = $(selectors.fileUploadingTarget).contents().find(selectors.fileUploadingResult).get(0);
            if (result == null) {
                return;
            }
            var newImg = $.parseJSON(result.innerHTML);
            if (newImg.Success == false) {
                var failModel = new FileViewModel(uploadFile.file);
                failModel.uploadFailed(true);
                failModel.failureMessage('');
                if (newImg.Messages) {
                    var failureMessages = '';
                    for (var i = 0; i < newImg.Messages.length; i++) {
                        failureMessages += newImg.Messages[i] + ' ';
                    }
                    failModel.failureMessage(failureMessages);
                }
                uploadsModel.uploads.push(failModel);
                return;
            }
            
            // Add uploaded file model.
            var fileModel = new FileViewModel(newImg);
            
            fileModel.uploadCompleted(true);
            fileModel.fileId(newImg.Id);
            fileModel.version(newImg.Version);
            fileModel.type(newImg.Type);
            fileModel.uploadProgress(100);

            if (newImg.IsFailed) {
                fileModel.uploadFailed(true);
                fileModel.failureMessage(globalization.failedToProcessFile);
            } else if (newImg.IsProcessing) {
                fileModel.uploadProcessing(true);

                uploadsModel.startStatusChecking(uploadsModel.firstTimeout);
            }

            uploadsModel.uploads.push(fileModel);
            uploadsModel.activeUploads.remove(fileModel);
        });
        
        ko.applyBindings(uploadsModel, context);
    }
        
    function UploadsViewModel() {
        var self = this,

            undoUpload = function (fileViewModel) {
                $.post($.format(links.undoFileUploadUrl, fileViewModel.fileId(), fileViewModel.version(), fileViewModel.type()));
            },
            
            abortUpload = function (fileViewModel) {
                self.uploads.remove(fileViewModel);
                
                if (self.activeUploads.indexOf(fileViewModel) !== -1) {
                    self.activeUploads.remove(fileViewModel);
                }
                
                if (fileViewModel.uploadCompleted() === false && fileViewModel.uploadFailed() === false) {
                    fileViewModel.file.abort();
                } else if (fileViewModel.uploadCompleted() === true) {
                    undoUpload(fileViewModel);
                }
            };
        
        self.uploads = ko.observableArray();
        self.activeUploads = ko.observableArray();
        self.filesToAccept = ko.observable('');
        
        self.cancelAllActiveUploads = function () {
            var uploads = self.activeUploads.removeAll();
            for (var i = 0; i < uploads.length; i++) {                                                
                abortUpload(uploads[i]);
            }
        };
        
        self.removeUpload = function (fileViewModel) {            
            abortUpload(fileViewModel);
        };

        self.removeAllUploads = function() {
            var uploads = self.uploads.removeAll();
            for (var i = 0; i < uploads.length; i++) {
                abortUpload(uploads[i]);
            }
        };
        
        self.removeFailedUploads = function () {
            for (var i = 0; i < self.uploads().length; i++) {
                if (self.uploads()[i].uploadFailed()) {
                    abortUpload(self.uploads()[i]);
                }
            }
        };
        
        // When one of file status is "Processing", checking file status repeatedly
        self.timeout = 5000;
        self.firstTimeout = 1000;
        self.timer = null;

        self.startStatusChecking = function (timeout) {
            if (!self.timer) {
                if (!timeout) {
                    timeout = self.timeout;
                }
                self.timer = setTimeout(self.checkStatus, timeout);
            }
        };
        
        self.stopStatusChecking = function () {
            if (self.timer) {
                clearTimeout(self.timer);
                self.timer = null;
            }
        };

        self.checkStatus = function () {
            var ids = self.getProcessingIds(),
                onFail = function() {
                    bcms.logger.error('Failed to check uploaded files statuses');
                    self.startStatusChecking();
                },
                hasProcessing = false;

            self.timer = null;

            if (ids.length > 0) {
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: links.checkUploadedFileStatuses,
                    data: JSON.stringify({ ids: ids }),
                    contentType: 'application/json; charset=utf-8'
                })
                    .done(function (response) {
                        if (response.Success) {
                            if (response.Data && response.Data.length  > 0) {
                                for (var i = 0; i < response.Data.length; i++) {
                                    var item = response.Data[i],
                                        id = item.Id,
                                        isProcessing = item.IsProcessing === true,
                                        isFailed = item.IsFailed === true;
                                    if (id) {
                                        for (var j = 0; j < self.uploads().length; j ++) {
                                            if (self.uploads()[j].fileId() == id) {
                                                if (isFailed) {
                                                    self.uploads()[j].uploadFailed(true);
                                                    self.uploads()[j].uploadProcessing(false);
                                                    self.uploads()[j].failureMessage(globalization.failedToProcessFile);
                                                } else if (!isProcessing) {
                                                    self.uploads()[j].uploadProcessing(false);
                                                } else if (isProcessing) {
                                                    hasProcessing = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (hasProcessing) {
                                self.startStatusChecking();
                            }
                        } else {
                            onFail();
                        }
                    })
                    .fail(function (response) {
                        onFail();
                    });
            }
        };

        self.getProcessingIds = function() {
            var ids = [],
                i;
            for (i = 0; i < self.uploads().length; i++) {
                if (self.uploads()[i].uploadProcessing()) {
                    ids.push(self.uploads()[i].fileId());
                }
            }
            return ids;
        };
    } 

    function FileViewModel(file) {
        var self = this;
        self.file = file;
        self.fileId = ko.observable('');
        self.version = ko.observable(0);
        self.type = ko.observable(0);
        self.uploadProgress = ko.observable(0);
        self.uploadCompleted = ko.observable(false);
        self.uploadFailed = ko.observable(false);
        self.uploadProcessing = ko.observable(false);
        self.failureMessage = ko.observable("");
        self.uploadSpeedFormatted = ko.observable();
        self.fileName = file.fileName;
        self.fileSizeFormated = formatFileSize(file.fileSize);
        self.isProgressVisible = ko.observable(true);
        self.isActive = ko.observable(false);

        self.uploadCompleted.subscribe(function (newValue) {
            if (newValue === true) {
                self.uploadProgress(100);
            }
        });

        self.activate = function() {
            self.isActive(true);
            setTimeout(function() {
                self.isActive(false);
            }, 4000);
        };
    }
        
    function initUploadFilesDialogEvents(dialog, options) {
        var uploadsModel = options.uploads,
            dragZone = dialog.container.find(selectors.dragZone),
            messageBox = messages.box({ container: dialog.container }),
            cancelEvent = function (e) {
                e.preventDefault();
                e.stopPropagation();
            };
        
        dragZone.on("dragenter dragover", function (e) {
            cancelEvent(e);
            dragZone.addClass(classes.dragZoneActive);
            dragZone.children().hide();
            return false;
        });
        
        dragZone.on("dragleave drop", function (e) {
            cancelEvent(e);
            dragZone.removeClass(classes.dragZoneActive);
            dragZone.children().show();
            return false;
        });

        dialog.container.find(selectors.uploadButtonLabel).on('click', fixUploadButtonForMozilla);

        if (fileApiSupported) {

            var context = document.getElementById('bcms-media-uploads');

            html5Upload.initialize({
                uploadUrl: links.uploadFileToServerUrl,
                dropContainer: document.getElementById('bcms-files-dropzone'),
                inputField: document.getElementById('bcms-files-upload-input'),
                key: 'File',
                data: { rootFolderId: options.rootFolderId, rootFolderType: options.rootFolderType, reuploadMediaId: options.reuploadMediaId },
                maxSimultaneousUploads: 4,
                onFileAdded: function (file) {
                    var overrideSelect = document.getElementById(selectors.overrideSelect);
                    if (overrideSelect) {
                        this.data['shouldOverride'] = overrideSelect.options[overrideSelect.selectedIndex].value;
                    } else {
                        this.data['shouldOverride'] = "true";
                    }
                    
                    if (options.reuploadMediaId && options.reuploadMediaId != constants.defaultReuploadMediaId && uploadsModel.uploads().length > 0) {
                        messageBox.clearMessages();
                        messageBox.addWarningMessage(globalization.multipleFilesWarningMessageOnReupload);
                        var uploadedFiles = uploadsModel.uploads();
                        for (var i = 0; i < uploadedFiles.length; i++) {
                            uploadedFiles[i].activate();
                        }
                        return;
                    }
                    var fileModel = new FileViewModel(file);
                    uploadsModel.activeUploads.push(fileModel);
                    uploadsModel.uploads.push(fileModel);
                    var transferAnimationId;
                    file.on({
                        // Called after received response from the server
                        onCompleted: function(data) {
                            var result = JSON.parse(data);
                            if (result.Success) {
                                uploadsModel.activeUploads.remove(fileModel);
                                fileModel.fileId(result.Data.FileId);
                                fileModel.version(result.Data.Version);
                                fileModel.type(result.Data.Type);
                                clearInterval(transferAnimationId);
                                fileModel.isProgressVisible(true);
                                fileModel.uploadCompleted(true);
                                
                                if (result.Data.IsFailed) {
                                    fileModel.uploadFailed(true);
                                    fileModel.failureMessage(globalization.failedToProcessFile);
                                } else if (result.Data.IsProcessing) {
                                    fileModel.uploadProcessing(true);
                                    fileModel.isProgressVisible(false);
                                    
                                    uploadsModel.startStatusChecking(uploadsModel.firstTimeout);
                                }
                            } else {
                                fileModel.uploadFailed(true);
                                fileModel.failureMessage('');
                                if (result.Messages) {
                                    var failureMessages = '';
                                    for (var i = 0; i < result.Messages.length; i++) {
                                        failureMessages += result.Messages[i] + ' ';
                                    }
                                    clearInterval(transferAnimationId);
                                    fileModel.isProgressVisible(true);
                                    fileModel.failureMessage(failureMessages);
                                }
                            }
                        },

                        // Called during upload progress, first parameter is decimal value from 0 to 100.
                        onProgress: function(progress) {
                            fileModel.uploadProgress(parseInt(progress, 10));
                        },

                        onError: function() {
                            fileModel.uploadFailed(true);
                            uploadsModel.activeUploads.remove(fileModel);
                        },

                        onTransfer: function () {
                            fileModel.isProgressVisible(false);

                            if (overrideSelect) {
                                overrideSelect.disabled = true;
                            }

                            transferAnimationId = setInterval(function () {
                                if (fileModel.uploadProgress() >= 100) {
                                    fileModel.uploadProgress(0);
                                } else {
                                    fileModel.uploadProgress(fileModel.uploadProgress() + 20);
                                };
                            }, 200);
                        }
                    });
                }
            });

            ko.applyBindings(uploadsModel, context);
        } 
    }
        
    function fixUploadButtonForMozilla() {
        if ($.browser.mozilla) {
            $('#' + $(this).attr('for')).click();
            return false;
        }
        return true;
    }

    function trimTrailingZeros(number) {
        return number.toFixed(1).replace(/\.0+$/, '');
    }

    function formatFileSize(sizeInBytes) {
        var kiloByte = 1024,
            megaByte = Math.pow(kiloByte, 2),
            gigaByte = Math.pow(kiloByte, 3);

        if (sizeInBytes < kiloByte) {
            return sizeInBytes + ' B';
        }

        if (sizeInBytes < megaByte) {
            return trimTrailingZeros(sizeInBytes / kiloByte) + ' KB';
        }

        if (sizeInBytes < gigaByte) {
            return trimTrailingZeros(sizeInBytes / megaByte) + ' MB';
        }

        return trimTrailingZeros(sizeInBytes / gigaByte) + ' GB';
    }
        
    /**
    * Initializes page module.
    */
    mediaUpload.init = function () {        
    };

    /**
    * Register initialization
    */
    bcms.registerInit(mediaUpload.init);
        
    return mediaUpload;
});
