/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

bettercms.define('bcms.vimeo.videos', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.modal', 'bcms.ko.extenders', 'bcms.messages'],
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
                videoPreviewUrl: 'http://player.vimeo.com/video/{0}',
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

        function ListViewModel(container, url, messagesContainer) {
            var self = this;
                
            self.container = container;
            self.messagesContainer = messagesContainer;
            self.url = url;

            self.isGrid = ko.observable(localStorage.getItem(keys.folderViewMode) == 1);
            self.gridOptions = ko.observable();
            self.searchInAllAccounts = ko.observable(false);
            self.medias = ko.observableArray();

            self.switchViewStyle = function () {
                var isGrid = !self.isGrid();
                localStorage.setItem(keys.folderViewMode, isGrid ? 1 : 0);
                self.isGrid(isGrid);
            };
            self.searchMedia = function () {
                self.gridOptions().paging.pageNumber(1);

                self.loadMedia();
            };
            self.showNoDataInfoDiv = function () {
                return self.medias().length == 0;
            };
            self.onOpenPage = function () {
                self.loadMedia();
            };
            self.loadMedia = function () {
                var params = createParams(self),
                    onComplete = function (json) {
                        parseJsonResults(json, self);
                        $(selectors.searchBox).focus();
                    };
                loadItems(self, params, onComplete);
            };

            self.GetSelectedItems = function () {
                var medias = self.medias(),
                    result = [];
                for (var i in medias) {
                    var media = medias[i];
                    if (media.isSelected()) {
                        result.push(media);
                    }
                }
                return result;
            };
        };

        function ListOptionsViewModel(onOpenPage) {
            var self = this;
            self.searchQuery = ko.observable();
            if (!self.paging) {
                self.paging = new ko.PagingViewModel(0, 1, 0, onOpenPage);
            }
            self.fromJson = function (options) {
                self.searchQuery(options.GridOptions.SearchQuery);
                self.paging.setPaging(options.GridOptions.PageSize, options.GridOptions.PageNumber, options.TotalCount);
            };
        };

        function ListItemViewModel(item) {
            var self = this;
            self.id = ko.observable(item.Id);
            self.version = ko.observable(item.Version);
            
            self.videoId = ko.observable(item.VideoId);
            self.name = ko.observable(item.Title);
            self.description = ko.observable(item.Description);
            self.duration = ko.observable(item.Duration);
            self.width = ko.observable(item.Width);
            self.height = ko.observable(item.Height);
            self.thumbnailUrl = ko.observable(item.ThumbnailUrl);
            self.ownerName = ko.observable(item.OwnerName);
            self.isPublic = ko.observable(item.IsPublic);
            self.isTranscoding = ko.observable(item.IsTranscoding);

            self.nameDomId = 'name_' + staticDomId++;
            self.isSelected = ko.observable(false);

            self.tooltip = ko.observable(item.Tooltip);
            self.getImageUrl = function () {
                if (!self.thumbnailUrl()) {
                    return null;
                }
                return self.thumbnailUrl();
            };
            self.rowClassNames = ko.computed(function () {
                var classes = 'bcms-image-box';
                if (self.isSelected()) {
                    classes += ' bcms-media-click-active';
                }
                return $.trim(classes);
            });

            self.select = function (model, event) {
                bcms.stopEventPropagation(event);
                self.isSelected(!self.isSelected());
            };
            self.preview = function () {
                if (self.isTranscoding()) {
                    return;
                }
                var url = $.format(links.videoPreviewUrl, self.videoId());
                videoPreview(url, self.width(), self.height());
            };
        };
        

        /**
        * Open dialog to select uploaded Vimeo video.
        */
        module.addUploadedVideo = function (folderId, onSaveCallback) {
            var listViewModel;
            modal.open({
                title: globalization.selectVideoDialogTitle,
                acceptTitle: globalization.selectVideoDialogSaveButtonTitle,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, links.selectVideoDialogUrl, {
                        done: function (content) {
                            listViewModel = new ListViewModel(dialog.container, links.selectVideoDialogUrl, dialog.container);
                            initializeDialog(content, listViewModel);
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


        function initializeDialog(json, listViewModel) {
            var context = listViewModel.container.find(selectors.templateDataBind).get(0);

            if (parseJsonResults(json, listViewModel)) {
                ko.applyBindings(listViewModel, context);

                bcms.updateFormValidator(listViewModel.container.find(selectors.firstForm));
            }
        }
        
        function loadItems(listViewModel, params, complete) {
            var indicatorId = 'medialist',
                spinContainer = listViewModel.container,
                onComplete = function (result) {
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
                url: listViewModel.url,
                data: JSON.stringify(params)
            })
                .done(function (result) {
                    onComplete(result);
                })
                .fail(function (response) {
                    onComplete(bcms.parseFailedResponse(response));
                });
        }

        function saveSelectedItems(folderIdToSaveIn, listViewModel, complete) {
            var indicatorId = 'medialist',
                spinContainer = listViewModel.container,
                onComplete = function (result) {
                    spinContainer.hideLoading(indicatorId);
                    if ($.isFunction(complete)) {
                        complete(result);
                    }
                };
            spinContainer.showLoading(indicatorId);

            var videos = listViewModel.GetSelectedItems(),
                params = {
                    FolderId: folderIdToSaveIn,
                    VideosIds: []
                };
            for (var i in videos) {
                params.VideosIds.push(videos[i].videoId());
            }

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

        function parseJsonResults(json, listViewModel) {
            var i;

            messages.refreshBox(listViewModel.container, json);

            if (json.Success) {
                listViewModel.medias.removeAll();

                if (json.Data) {
                    // Map media items
                    if (json.Data.Items && json.Data.Items && $.isArray(json.Data.Items)) {
                        for (i = 0; i < json.Data.Items.length; i++) {
                            var item = json.Data.Items[i],
                                model = new ListItemViewModel(item);
                            listViewModel.medias.push(model);
                        }
                    }
                    
                    listViewModel.searchInAllAccounts(!json.Data.OnlyMine);

                    // Map grid options
                    if (!listViewModel.gridOptions()) {
                        listViewModel.gridOptions(new ListOptionsViewModel(listViewModel.onOpenPage));
                    }
                    listViewModel.gridOptions().fromJson(json.Data);

                    // Replace unobtrusive validator
                    bcms.updateFormValidator(listViewModel.container.find(selectors.firstForm));
                }

                return true;
            }

            return false;
        }

        function createParams(listViewModel) {
            var params = {};

            if (listViewModel != null && listViewModel.gridOptions() != null) {
                var query = listViewModel.gridOptions().searchQuery();
                if (query) {
                    params.SearchQuery = query;
                }
                params.OnlyMine = !listViewModel.searchInAllAccounts();
                params.PageSize = listViewModel.gridOptions().paging.pageSize;
                params.PageNumber = listViewModel.gridOptions().paging.pageNumber();
            }

            return params;
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
            console.log('Initializing bcms.vimeo.videos module.');
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(module.init);

        return module;
    });