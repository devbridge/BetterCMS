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
            },
            links = {
                uploadVideoDialogUrl: null,
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
            };

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
                                fileName = $this.val(),
                                files = $this.files;
                            viewModel[property](fileName, files);
                        }
                    });
                }
            },
        };
        
        // --- View Models ----------------------------------------------------
        function FolderViewModel() {
            var self = this;
            self.id = "";
            self.name = "";
        }

        function UploadWorkerViewModel() {
            var self = this;
            self.fileInputDomId = 'bcms-files-upload-input' + getNewDomIdExtension();
            self.endpoint = ko.observable();
            self.token = ko.observable();
            self.callbackUrl = ko.observable();
            self.fileName = ko.observable();
            self.fileSize = ko.observable();
            self.fileSizeFormated = ko.computed(function () {
                return formatFileSize(self.fileSize());
            });
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
                return self.status() == workerStatus.RequestingUploadData || self.status() == workerStatus.UploadingFileFailed;
            });
            self.uploadCompleted = ko.computed(function () {
                return self.status() == workerStatus.Done;
            });
            
            self.onFileSelected = function (fileName, files) {
                if (files != null && files.length > 0) {
                    self.fileName(files[0].name);
                    self.fileSize(files[0].size);
                } else {
                    self.fileName(fileName);
                }
                if (self.fileName()) {
                    self.upload();
                }
            };
            self.upload = function() {
                // Change status
                self.status(workerStatus.RequestingUploadData);

                // Request upload data // Change status on fail
                // TODO: implement

                // Upload file // Change status on fail
                // TODO: implement

                // Change status
                self.status(workerStatus.Done);
            };
            self.onCancelUpload = function() {
                // TODO: implement
                // Change status
            };
        }

        function UploaderViewModel() {
            var self = this;
            self.subFolders = ko.observableArray([]);
            self.selectedFolder = ko.observableArray([]);
            self.showFolderSelection = ko.observable(false);
            self.isReupload = ko.observable(false);

            self.uploadWorkers = ko.observableArray([new UploadWorkerViewModel()]); // TODO
            self.activeUploads = ko.observableArray([]);

            self.cancelAllActiveUploads = function() {
                // TODO: implement
            };
        }

        /**
        * Open dialog to upload video.
        */
        module.uploadVideo = function (folderId, onSaveCallback) {
            var listViewModel;
            modal.open({
                title: globalization.uploadVideoDialogTitle,
                acceptTitle: globalization.uploadVideoDialogSaveButtonTitle,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, links.uploadVideoDialogUrl, {
                        done: function (content) {
                            var uploader = new UploaderViewModel();
                            // TODO: content.Html, content.Data
                            var context = dialog.container.find(selectors.templateDataBind).get(0);
                            if (context) {
                                ko.applyBindings(uploader, context);
                            }
                        }
                    });
                },
                onAccept: function (dialog) {
                    saveSelectedItems(folderId, listViewModel, function(json) {
                        if (onSaveCallback && $.isFunction(onSaveCallback)) {
                            onSaveCallback(json);
                        }
                        dialog.close();
                    });
                    return false;
                }
            });
        };


        function saveSelectedItems(folderIdToSaveIn, videoId, spinContainer, complete) {
            var indicatorId = 'medialist',
                params = {
                    FolderId: folderIdToSaveIn,
                    VideoId: videoId
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