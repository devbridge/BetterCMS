/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.media.upload', ['jquery', 'bcms', 'bcms.dynamicContent', 'bcms.modal', 'html5Upload', 'knockout'],
    function ($, bcms, dynamicContent, modal, html5Upload, ko) {
    'use strict';

        var mediaUpload = {},
            
        selectors = {
            dragZone: '#bcms-files-dropzone'
        },
        
        classes = {
            dragZoneActive: 'bcms-dropzone-active'
        },
            
        links = {
            loadUploadFilesDialogUrl: null,
            uploadFileToServerUrl: null,
            cancelFileUploadUrl: null
        },
        
        globalization = {
           uploadFilesDialogTitle: null
        };

    /**
    * Assign objects to module.
    */
    mediaUpload.links = links;
    mediaUpload.globalization = globalization;    

    mediaUpload.openUploadFilesDialog = function (rootFolderId, rootFolderType) {
        var options = {
            rootFolderId: rootFolderId,
            rootFolderType: rootFolderType
        };
        
        modal.open({
            title: globalization.uploadFilesDialogTitle,            
            onLoad: function (dialog) {
                var url = $.format(links.loadUploadFilesDialogUrl, rootFolderId);
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable: function () {
                        initUploadFilesDialogEvents(dialog, options);
                    },

                    beforePost: function () {
                        dialog.container.showLoading();
                    },

                    postComplete: function () {
                        dialog.container.hideLoading();
                    }
                });
            }
        });
    };
        
    function UploadsViewModel() {
        var self = this,
            abortUpload = function(fileViewModel) {
                var file = fileViewModel.file;
                if (file.uploadCompleted() == false && file.uploadFailed() == false) {
                    file.abort();
                }
                self.uploads.remove(fileViewModel);
            },
            removeUploads = function(fileNamesToRemove) {

            };
        
        self.uploads = ko.observableArray();
        self.showProgress = ko.observable(false);

        self.cancelAllUploads = function () {
            var fileNamesToRemove = [];
            $.each(self.uploads, function () {
                fileNamesToRemove.push(this.fileName);
                abortUpload(this);
            });
            removeUploads(fileNamesToRemove);
        };
        
        self.cancelUpload = function (fileViewModel) {
            var fileNamesToRemove = [];
            fileNamesToRemove.push(fileViewModel.fileName);
            abortUpload(fileViewModel);
            removeUploads(fileNamesToRemove);
        };
    } 

    function FileViewModel(file) {
        var self = this;
        self.file = file;
        self.uploadProgress = ko.observable(0);
        self.uploadCompleted = ko.observable(false);
        self.uploadFailed = ko.observable(false);
        self.uploadSpeedFormatted = ko.observable();
        self.fileName = file.fileName;
        self.fileSizeFormated = formatFileSize(file.fileSize);
    }
        
    function initUploadFilesDialogEvents(dialog, options) {
        var dragZone = dialog.container.find(selectors.dragZone),
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
            
            var context = document.getElementById('bcms-media-uploads'),
                uploadsModel = new UploadsViewModel();

            html5Upload.initialize({
                uploadUrl: links.uploadFileToServerUrl,
                dropContainer: document.getElementById('bcms-files-dropzone'),
                inputField: document.getElementById('bcms-files-upload-input'),
                key: 'File',
                data: { rootFolderId: options.rootFolderId, rootFolderType: options.rootFolderType },
                maxSimultaneousUploads: 4,
                onFileAdded: function (file) {
                    var fileModel = new FileViewModel(file);
                    uploadsModel.uploads.push(fileModel);
                    
                    file.on({
                        // Called after received response from the server
                        onCompleted: function(response) {
                            fileModel.uploadCompleted(true);
                        },

                        // Called during upload progress, first parameter is decimal value from 0 to 100.
                        onProgress: function (progress, fileSize, uploadedBytes) {
                            fileModel.uploadProgress(parseInt(progress, 10));                            
                        },
                        
                        onError: function(response) {
                            fileModel.uploadFailed(true);
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
        
    return mediaUpload;
});
