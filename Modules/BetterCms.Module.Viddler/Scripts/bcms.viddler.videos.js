/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

bettercms.define('bcms.viddler.videos', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.modal', 'bcms.ko.extenders', 'bcms.messages'],
    function ($, bcms, dynamicContent, modal, ko, messages) {
        'use strict';

        var module = {},
            selectors = {
                templateDataBind: '.bcms-data-bind-container',
                searchBox: '#bcms-search-input',
                previewVideo: '.bcms-preview-image-frame iframe',
                previewVideoContainer: '.bcms-preview-image-border',
                previewFailure: '.bcms-grid-image-holder',
            },
            links = {
                selectVideoDialogUrl: null,
                saveSelectedVideosUrl: null,
                videoPreviewUrl: 'http://viddler.com/embed/{0}',
            },
            globalization = {
                selectVideoDialogTitle: null,
                selectVideoDialogSaveButtonTitle: null,
            },
            keys = {
                folderViewMode: 'bcms.mediaFolderViewMode'
            },
            staticDomId = 1;

        /**
        * Assign objects to module.
        */
        module.links = links;
        module.globalization = globalization;

        /**
        * Open dialog to upload video.
        */
        module.uploadVideo = function (folderId, onSaveCallback) {
            var listViewModel;
            modal.open({
                title: globalization.selectVideoDialogTitle,
                acceptTitle: globalization.selectVideoDialogSaveButtonTitle,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, links.selectVideoDialogUrl, {
                        done: function (content) {
                            // TODO
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
                url: links.saveSelectedVideosUrl,
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