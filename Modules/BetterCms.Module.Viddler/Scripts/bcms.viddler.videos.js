/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

bettercms.define('bcms.viddler.videos', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.modal', 'bcms.ko.extenders', 'bcms.messages'],
    function ($, bcms, dynamicContent, modal, ko, messages) {
        'use strict';

        var module = {},
            selectors = {
                templateDataBind: '.bcms-file-manager-inner',
                previewVideo: '.bcms-preview-image-frame iframe',
                previewVideoContainer: '.bcms-preview-image-border',
                previewFailure: '.bcms-grid-image-holder',
                fileUploadingResult: '#jsonResult',
            },
            links = {
                uploadVideoDialogUrl: null,
                getUploadDataUrl: null,
                saveUploadedVideosUrl: null,
                videoPreviewUrl: 'http://viddler.com/embed/{0}',
            },
            globalization = {
                uploadVideoDialogTitle: null,
                uploadVideoDialogSaveButtonTitle: null,
            },
            staticDomId = 0,
            workerStatus = {
                Idle: 1,
                RequestingUploadData: 2,
                FailedToGetUploadData: 3,
                UploadingFile: 4,
                UploadingFileFailed: 5,
                Done: 6
            },
            defaultIdValue = '00000000-0000-0000-0000-000000000000';

        /**
        * Assign objects to module.
        */
        module.links = links;
        module.globalization = globalization;
        
        // --- Helpers --------------------------------------------------------
        function getNewDomIdExtension() {
            return "_" + staticDomId++;
        }

        ko.bindingHandlers.fileSelectionChanged = {
            init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var options = ko.utils.unwrapObservable(valueAccessor()),
                    property = ko.utils.unwrapObservable(options.property);
                if (property) {
                    $(element).change(function() {
                        if (element.files.length) {
                            var $this = $(this),
                                fileName = $this.val();
                            viewModel[property](fileName);
                        }
                    });
                }
            },
        };
        
        // --- View Models ----------------------------------------------------
        function FolderViewModel(id, name) {
            var self = this;
            self.id = id;
            self.name = name;
        }

        function UploadWorkerViewModel(onStartUploadCallback, onFinishUploadCallback) {
            var self = this,
                id = getNewDomIdExtension();
            self.fileInputDomId = 'bcms-files-upload-input' + id;
            self.targetFormDomId = 'bcms-files-upload-form' + id;
            self.endpoint = ko.observable();
            self.token = ko.observable();
            self.callbackUrl = ko.observable();
            self.fileName = ko.observable();
            self.status = ko.observable(workerStatus.Idle);
            self.uploadProgress = ko.observable(0);
            self.failureMessage = ko.observableArray([]);
            
            self.showFileSelection = ko.computed(function () {
                return self.status() == workerStatus.Idle;
            });
            self.showProgress = ko.computed(function() {
                return self.status() != workerStatus.Idle;
            });
            self.uploadFailed = ko.computed(function () {
                return self.status() == workerStatus.FailedToGetUploadData || self.status() == workerStatus.UploadingFileFailed;
            });
            self.uploadProcessing = ko.computed(function () {
                return self.status() == workerStatus.RequestingUploadData || self.status() == workerStatus.UploadingFile;
            });
            self.uploadCompleted = ko.computed(function () {
                return self.status() == workerStatus.Done;
            });
            
            self.onFileSelected = function (fileName) {
                self.fileName(fileName);
                if (fileName) {
                    self.upload();
                }
            };
            self.upload = function() {
                var onDone = function () {
                    if (onFinishUploadCallback && $.isFunction(onFinishUploadCallback)) {
                        onFinishUploadCallback(self);
                    }
                };
                
                // Change status
                self.status(workerStatus.RequestingUploadData);
                if (onStartUploadCallback && $.isFunction(onStartUploadCallback)) {
                    onStartUploadCallback(self);
                }

                // Request upload data
                self.requestUploadData(function(uploadData) {
                    if (uploadData && uploadData.Data && uploadData.Data.Token && uploadData.Data.Endpoint && uploadData.Data.CallbackUrl) {
                        self.token(uploadData.Data.Token);
                        self.endpoint(uploadData.Data.Endpoint);
                        self.callbackUrl(uploadData.Data.CallbackUrl);
                        self.uploadVideo(function (result) {
                            if (result.Success) {
                                // Change status
                                self.status(workerStatus.Done);
                                onDone();
                            } else {
                                // Upload failed
                                self.status(workerStatus.UploadingFileFailed);
                                onDone();
                            }
                        });
                    } else {
                        // Failed.
                        self.status(workerStatus.FailedToGetUploadData);
                        onDone();
                    }
                });
            };
            self.requestUploadData = function (complete) {
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
            self.uploadVideo = function (complete) {
                // Get form to submit.
                var form = $("#" + self.fileInputDomId).parent("form");
                var resultForm = $("#" + self.targetFormDomId);
                if (form && resultForm) {
                    // On file submitted.
                    resultForm.on('load', function () {
                        // Check the result.
                        var result = resultForm.contents().find(selectors.fileUploadingResult).get(0);
                        if (result == null) {
                            return;
                        }
                        var resultData = $.parseJSON(result.innerHTML);
                        if ($.isFunction(complete)) {
                            complete(resultData);
                        }
                    });
                    form.submit();
                }
            };

            self.onCancelUpload = function() {
                // TODO: implement
                // Change status
            };
        }

        function UploaderViewModel() {
            var self = this;
            self.rootFolderId = null;
            self.reuploadMediaId = null;
            self.subFolders = ko.observableArray([]);
            self.selectedFolder = ko.observableArray([]);
            self.showFolderSelection = ko.observable(false);
            self.isReupload = ko.observable(false);
            
            self.uploadWorkers = ko.observableArray([]);
            self.activeUploads = ko.observableArray([]);

            self.cancelAllActiveUploads = function() {
                // TODO: implement
            };
            self.addNewWorker = function() {
                self.uploadWorkers.push(new UploadWorkerViewModel(self.onStartUpload, self.onFinishUpload));
            };
            self.onUpload = function (worker) {
                self.addNewWorker();
                self.activeUploads.push(worker);
            };
            self.onFinishUpload = function (worker) {
                self.activeUploads.remove(worker);
            };
            
            self.addNewWorker();
        }

        /**
        * Open dialog to upload video.
        */
        module.uploadVideo = function (folderId, onSaveCallback) {
            var uploader;
            modal.open({
                title: globalization.uploadVideoDialogTitle,
                acceptTitle: globalization.uploadVideoDialogSaveButtonTitle,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, links.uploadVideoDialogUrl, {
                        done: function (content) {
                            if (content && content.Data) {
                                uploader = new UploaderViewModel();
                                uploader.rootFolderId = content.Data.RootFolderId;
                                uploader.reuploadMediaId = content.Data.ReuploadMediaId;
                                if (content.Data.Folders) {
                                    for (var i in content.Data.Folders) {
                                        var folder = content.Data.Folders[i];
                                        uploader.subFolders.push(new FolderViewModel(folder.Item1, folder.Item2));
                                    }
                                }
                                uploader.showFolderSelection(uploader.reuploadMediaId == defaultIdValue);
                                var context = dialog.container.find(selectors.templateDataBind).get(0);
                                if (context) {
                                    ko.applyBindings(uploader, context);
                                }
                            }
                            // TODO: show error message.
                        }
                    });
                },
                onAccept: function (dialog) {
                    saveSelectedItems(folderId, uploader, function (json) {
                        if (onSaveCallback && $.isFunction(onSaveCallback)) {
                            onSaveCallback(json);
                        }
                        dialog.close();
                    });
                    return false;
                }
            });
        };


        function saveSelectedItems(folderIdToSaveIn, uploaderViewModel, spinContainer, complete) {
            return;
            // TODO implement.
            var indicatorId = 'medialist',
                params = {
                    FolderId: folderIdToSaveIn,
                    VideoIds: null // TODO: fill with data from uploaderViewModel
                },
                onComplete = function(result) {
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
                url: links.saveUploadedVideosUrl,
                data: JSON.stringify(params)
            })
                .done(function (result) {
                    onComplete(result);
                })
                .fail(function (response) {
                    onComplete(bcms.parseFailedResponse(response));
                });
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