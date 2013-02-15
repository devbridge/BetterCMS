/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.media.upload', ['jquery', 'bcms', 'bcms.dynamicContent', 'bcms.modal', 'html5Upload', 'bcms.ko.extenders', 'bcms.messages'],
    function ($, bcms, dynamicContent, modal, html5Upload, ko, messages) {
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
            folderDropDown: '#SelectedFolderId'
        },

        classes = {
            dragZoneActive: 'bcms-dropzone-active'
        },

        links = {
            loadUploadFilesDialogUrl: null,
            uploadFileToServerUrl: null,
            undoFileUploadUrl: null,
            loadUploadSingleFileDialogUrl: null
        },

        globalization = {
            uploadFilesDialogTitle: null
        };

    /**
    * Assign objects to module.
    */
    mediaUpload.links = links;
    mediaUpload.globalization = globalization;    

    mediaUpload.openUploadFilesDialog = function (rootFolderId, rootFolderType, onSaveCallback) {
        var options = {
                uploads: new UploadsViewModel(),
                rootFolderId: rootFolderId,
                rootFolderType: rootFolderType
            };

        options.uploads.filesToAccept(rootFolderType == 1 ? 'image/*' : '');

        if (html5Upload.fileApiSupported()) {
            modal.open({
                title: globalization.uploadFilesDialogTitle,
                onLoad: function(dialog) {
                    var url = $.format(links.loadUploadFilesDialogUrl, rootFolderId, rootFolderType);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function() {
                            initUploadFilesDialogEvents(dialog, options);
                        },

                        beforePost: function() {
                            dialog.container.showLoading();
                        },

                        postSuccess: function(json) {
                            if (onSaveCallback && $.isFunction(onSaveCallback)) {
                                onSaveCallback(json);
                            }
                        },

                        postComplete: function() {
                            dialog.container.hideLoading();
                        }
                    });
                },
                onCancel: function() {
                    options.uploads.removeAllUploads();
                }
            });
        } else {
            modal.open({
                title: globalization.uploadFilesDialogTitle,
                onLoad: function(dialog) {
                    var url = $.format(links.loadUploadSingleFileDialogUrl, rootFolderId, rootFolderType);
                    dynamicContent.setContentFromUrl(dialog, url, {
                        done: function() {
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
                onCancel: function() {
                    options.uploads.removeAllUploads();
                }
            });
        }
    };

    function SingleFileUpload(dialog, options) {
        var context = dialog.container.find(selectors.fileUploadingContext).get(0),
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
                    for (var i in newImg.Messages) {
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
        self.failureMessage = ko.observable("");
        self.uploadSpeedFormatted = ko.observable();
        self.fileName = file.fileName;
        self.fileSizeFormated = formatFileSize(file.fileSize);

        self.uploadCompleted.subscribe(function (newValue) {
            if (newValue === true) {
                self.uploadProgress(100);
            }
        });
    }
        
    function initUploadFilesDialogEvents(dialog, options) {
        var uploadsModel = options.uploads,
            dragZone = dialog.container.find(selectors.dragZone),
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

        if (html5Upload.fileApiSupported()) {

            var context = document.getElementById('bcms-media-uploads');

            html5Upload.initialize({
                uploadUrl: links.uploadFileToServerUrl,
                dropContainer: document.getElementById('bcms-files-dropzone'),
                inputField: document.getElementById('bcms-files-upload-input'),
                key: 'File',
                data: { rootFolderId: options.rootFolderId, rootFolderType: options.rootFolderType },
                maxSimultaneousUploads: 4,
                onFileAdded: function(file) {
                    var fileModel = new FileViewModel(file);
                    uploadsModel.activeUploads.push(fileModel);
                    uploadsModel.uploads.push(fileModel);

                    file.on({
                        // Called after received response from the server
                        onCompleted: function(data) {
                            var result = JSON.parse(data);
                            if (result.Success) {
                                uploadsModel.activeUploads.remove(fileModel);
                                fileModel.uploadCompleted(true);
                                fileModel.fileId(result.Data.FileId);
                                fileModel.version(result.Data.Version);
                                fileModel.type(result.Data.Type);
                            } else {
                                fileModel.uploadFailed(true);
                                fileModel.failureMessage('');
                                if (result.Messages) {
                                    var failureMessages = '';
                                    for (var i in result.Messages) {
                                        failureMessages += result.Messages[i] + ' ';
                                    }
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
                        }
                    });
                }
            });

            ko.applyBindings(uploadsModel, context);
        } 
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
