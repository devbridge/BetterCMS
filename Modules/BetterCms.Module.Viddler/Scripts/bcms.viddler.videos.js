/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

bettercms.define('bcms.viddler.videos', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.modal', 'bcms.ko.extenders', 'bcms.messages'],
    function ($, bcms, dynamicContent, modal, ko, messages) {
        'use strict';

        var module = {},
            selectors = {
                previewVideo: '.bcms-preview-image-frame iframe',
                previewVideoContainer: '.bcms-preview-image-border',
                previewFailure: '.bcms-grid-image-holder',
                dragZone: '#bcms-files-dropzone',
                messageBox: "#bcms-multi-file-upload-messages",
                fileUploadingContext: '#bcms-media-uploads',
                fileUploadingMasterForm: '#SaveForm',
                fileUploadingForm: '#ImgForm',
                fileUploadingTarget: '#UploadTarget',
                fileUploadingInput: '#uploadFile',
                fileUploadingResult: '#jsonResult',
                folderDropDown: '#SelectedFolderId',
                uploadButtonLabel: '.bcms-btn-upload-files-text'
            },
            links = {
                uploadVideoDialogUrl: null,
                getUploadDataUrl: null,
                checkUploadedFileStatuses: null,
                saveUploadedVideosUrl: null,
                deleteVideoUrl: null,
                videoPreviewUrl: 'http://viddler.com/embed/{0}',
            },
            globalization = {
                uploadVideoDialogTitle: null,
                uploadVideoDialogSaveButtonTitle: null,
                deleteVideoConfirmMessage: null,
            };

        /**
        * Assign objects to module.
        */
        module.links = links;
        module.globalization = globalization;
        
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

            self.endpoint = ko.observable();
            self.token = ko.observable();
            self.callbackUrl = ko.observable();
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

            self.removeAllUploads = function () {
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
            self.timeout = 100000;
            self.firstTimeout = 500;
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
                    onFail = function () {
                        console.log('Failed to check uploaded files statuses');
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
                                if (response.Data && response.Data.length > 0) {
                                    for (var i = 0; i < response.Data.length; i++) {
                                        var item = response.Data[i],
                                            id = item.Id,
                                            isProcessing = item.IsProcessing === true,
                                            isFailed = item.IsFailed === true;
                                        if (id) {
                                            for (var j = 0; j < self.uploads().length; j++) {
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

            self.getProcessingIds = function () {
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

        function VideoUpload(dialog, options) {
            var context = dialog.container.find(selectors.fileUploadingContext).get(0),
                messageBox = messages.box({ container: dialog.container }),
                uploadsModel = options.uploads,
                fakeData = {
                    fileName: "Uploading",
                    fileSize: 0,
                    fileId: null,
                    version: null,
                    type: null,
                    abort: function() { /* File upload canceling is not supported. */
                    }
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
                animateUpload = function(fileModel) {
                    uploadAnimationId = setInterval(function() {
                        if (fileModel.uploadProgress() >= 100) {
                            fileModel.uploadProgress(0);
                        } else {
                            fileModel.uploadProgress(fileModel.uploadProgress() + 20);
                        }
                        ;
                    }, 200);
                },
                stopUploadAnimation = function() {
                    if (uploadAnimationId) {
                        clearInterval(uploadAnimationId);
                    }
                },
                onDone = function() {
                    stopUploadAnimation();
                    uploadsModel.uploads.remove(uploadFile);
                    uploadsModel.activeUploads.remove(uploadFile);
                    resetUploadForm();
                },
                onFail = function(jsonData) {
                    onDone();
                    var failModel = new FileViewModel(uploadFile.file);
                    failModel.uploadFailed(true);
                    failModel.failureMessage('');
                    if (jsonData && jsonData.Messages) {
                        var failureMessages = '';
                        for (var i in jsonResult.Messages) {
                            failureMessages += jsonData.Messages[i] + ' ';
                        }
                        failModel.failureMessage(failureMessages);
                    }
                    uploadsModel.uploads.push(failModel);
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
                    if (options.reuploadMediaId && uploadsModel.uploads().length > 0) {
                        messageBox.addWarningMessage(globalization.multipleFilesWarningMessageOnReupload);
                        var uploadedFiles = uploadsModel.uploads();
                        for (var i = 0; i < uploadedFiles.length; i++) {
                            uploadedFiles[i].activate();
                        }
                        return;
                    }
                    // Add fake file model for upload indication.
                    uploadFile.uploadCompleted(false);
                    uploadFile.fileName(fileName);
                    uploadFile.file.fileName = fileName;
                    uploadFile.uploadProgress(0);
                    uploadsModel.activeUploads.push(uploadFile);
                    uploadsModel.uploads.push(uploadFile);
                    animateUpload(uploadFile);
                    
                    requestUploadData(function(json) {
                        if (json.Success) {
                            uploadsModel.endpoint(json.Data.Endpoint);
                            uploadsModel.token(json.Data.Token);
                            uploadsModel.callbackUrl(json.Data.CallbackUrl);
                            // Send file to server.
                            dialog.container.find($(selectors.fileUploadingForm)).submit();
                        } else {
                            onFail();
                        }
                    });
                }
            });

            // On file submitted.
            dialog.container.find($(selectors.fileUploadingTarget)).on('load', function () {
                // Check the result.
                var result = $(selectors.fileUploadingTarget).contents().find(selectors.fileUploadingResult).get(0);
                if (result == null) {
                    onFail();
                    return;
                }
                
                var jsonResult = $.parseJSON(result.innerHTML);
                if (jsonResult.Success) {
                    onDone();
                    // Add uploaded file model.
                    var fileModel = new FileViewModel(jsonResult);

                    fileModel.uploadCompleted(true);
                    fileModel.fileId(jsonResult.Id);
                    fileModel.fileName(jsonResult.FileName);
                    fileModel.version(jsonResult.Version);
                    fileModel.type(jsonResult.Type);
                    fileModel.uploadProgress(100);

                    if (jsonResult.IsFailed) {
                        fileModel.uploadFailed(true);
                        fileModel.failureMessage(globalization.failedToProcessFile);
                    } else if (jsonResult.IsProcessing) {
                        fileModel.uploadProcessing(true);
                        // NOTE: video encoding process is not so short, so there is no need to question for it...
                        // uploadsModel.startStatusChecking(uploadsModel.firstTimeout);
                    }

                    uploadsModel.uploads.push(fileModel);
                    uploadsModel.activeUploads.remove(fileModel);
                } else {
                    onFail(jsonResult);
                }
            });

            ko.applyBindings(uploadsModel, context);
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
            self.fileName = ko.observable(file.fileName);
            self.fileSizeFormated = formatFileSize(file.fileSize);
            self.isProgressVisible = ko.observable(true);
            self.isActive = ko.observable(false);

            self.uploadCompleted.subscribe(function (newValue) {
                if (newValue === true) {
                    self.uploadProgress(100);
                }
            });

            self.activate = function () {
                self.isActive(true);
                setTimeout(function () {
                    self.isActive(false);
                }, 4000);
            };
        }

        module.uploadVideo = function(folderId, onSaveCallback, reuploadMediaId) {
            var options = {
                uploads: new UploadsViewModel(),
                rootFolderId: folderId,
                reuploadMediaId: reuploadMediaId
            };
            options.uploads.filesToAccept('video/*');
            modal.open({
                title: globalization.uploadFilesDialogTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.uploadVideoDialogUrl, folderId, reuploadMediaId);
                    dynamicContent.setContentFromUrl(dialog, url, {
                        done: function () {
                            VideoUpload(dialog, options);
                        }
                    });
                },
                onAcceptClick: function (dialog) {
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
                        .done(function (response) {
                            onComplete(response);
                        })
                        .fail(function (response) {
                            onComplete(bcms.parseFailedResponse(response));
                        });

                    return false;
                },
                onCloseClick: function () {
                    options.uploads.removeAllUploads();
                    options.uploads.stopStatusChecking();
                }
            });
        };

        module.deleteVideo = function (id, version, title, callback) {
            var url = $.format(links.deleteVideoUrl, id, version),
                message = $.format(globalization.deleteVideoConfirmMessage, title),
                onDeleteCompleted = function(json) {
                    if ($.isFunction(callback)) {
                        callback(json);
                    }
                    confirmDialog.close();
                },
                confirmDialog = modal.confirm({
                    content: message,
                    onAccept: function() {
                        $.ajax({
                            type: 'POST',
                            url: url,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            cache: false
                        })
                            .done(function(json) {
                                onDeleteCompleted(json);
                            })
                            .fail(function(response) {
                                onDeleteCompleted(bcms.parseFailedResponse(response));
                            });
                    },
                    onClose: function() {
                        if ($.isFunction(callback)) {
                            callback();
                        }
                    }
                });
        };

        function requestUploadData(complete) {
            var onComplete = function (result) {
                if ($.isFunction(complete)) {
                    complete(result);
                }
            };
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                cache: false,
                url: links.getUploadDataUrl,
            })
                .done(function (result) {
                    onComplete(result);
                })
                .fail(function (response) {
                    onComplete(bcms.parseFailedResponse(response));
                });
        };

        function fixUploadButtonForMozilla() {
            if ($.browser.mozilla) {
                $('#' + $(this).attr('for')).click();
                return false;
            }
            return true;
        }

        function videoPreview(url, width, height, options) {
            options = $.extend({}, options);
            options.templateId = 'bcms-video-preview-template';
            options.disableAnimation = true;

            var dialog = modal.open(options),
                iframe = dialog.container.find(selectors.previewVideo),
                iframeContainer = dialog.container.find(selectors.previewVideoContainer),
                frameLoaded = false,
                visibleWidth = $(window).width() - 150,
                visibleHeight = $(window).height() - 150,
                aspecRatio = (1.0 * width) / (1.0 * height);
            
            if (width > visibleWidth) {
                width = visibleWidth;
                height = Math.round(width / aspecRatio);
            }
            if (height > visibleHeight) {
                height = visibleHeight;
                width = Math.round(height * aspecRatio);
            }
            
            iframe.attr('width', width + 'px');
            iframe.attr('height', height + 'px');
            var margin = (width + 60) / -2;
            iframeContainer.css('width', width + 'px');
            iframeContainer.css('margin-left', margin + 'px');

            iframe.on('load', function () {
                frameLoaded = true;
                iframeContainer.find(modal.selectors.loader).hide();
                iframe.show();
            });

            iframe.on('error', function () {
                // IE && other browsers compatibility fix: checking if frame is not loaded yet
                if (!frameLoaded) {
                    var imgContainer = dialog.container.find(selectors.previewVideoContainer),
                        previewFailure = imgContainer.find(selectors.previewFailure);

                    imgContainer.find(selectors.loader).hide();
                    previewFailure.show();
                }
            });

            iframe.attr('src', url);

            return dialog;
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
        * Initializes module.
        */
        module.init = function() {
            console.log('Initializing bcms.viddler.videos module.');
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(module.init);

        return module;
    });