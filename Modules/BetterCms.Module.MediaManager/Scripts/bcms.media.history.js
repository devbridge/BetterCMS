/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.media.history.js" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

bettercms.define('bcms.media.history', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.messages', 'bcms.dynamicContent', 'bcms.redirect', 'bcms.grid'],
    function ($, bcms, modal, messages, dynamicContent, redirect, grid) {
    'use strict';

    var history = {},
        
        classes = {
            tableActiveRow: 'bcms-table-row-active'
        },
        
        selectors = {
            gridRestoreLinks: '#bcms-pagecontenthistory-form tr .bcms-js-restore',
            gridDownloadLinks: '#bcms-pagecontenthistory-form tr .bcms-js-download',
            gridCells: '#bcms-pagecontenthistory-form tr td',
            gridRowPreviewLink: '.bcms-js-preview:first',
            firstRow: 'tr:first',
            gridRows: '#bcms-pagecontenthistory-form tr',
            versionPreviewContainer: '#bcms-history-preview',
            versionPreviewLoaderContainer: '.bcms-history-preview-holder',
            mediaHistoryForm: '#bcms-pagecontenthistory-form',
            mediaHistorySearchButton: '.bcms-btn-search',
            mediaHistorySearchField: '.bcms-js-search-box',
            modalContent: '.bcms-modal-content',
            popinfoFrame: '.bcms-popinfo-frame'
        },
        
        links = {
            loadMediaHistoryDialogUrl: null,
            loadMediaVersionPreviewUrl: null,
            restoreMediaVersionUrl: null,
            downloadFileUrl: null
        },
        
        globalization = {
            mediaHistoryDialogTitle: null,
            mediaVersionRestoreConfirmation: null,
            restoreButtonTitle: null,
            restoreWithOverrideButtonTitle: null,
            restoreAsNewVersionButtonTitle: null,
            closeButtonTitle: null
        };

    /**
    * Assign objects to module.
    */
    history.links = links;
    history.globalization = globalization;

    /**
    * Preview specified media version.
    */
    function previewVersion(container, id) {
        var url = $.format(links.loadMediaVersionPreviewUrl, id),
            previewContainer = container.find(selectors.versionPreviewContainer),
            loaderContainer = container.find(selectors.versionPreviewLoaderContainer),
            onComplete = function(result) {
                loaderContainer.hideLoading();
                previewContainer.html(result);
            };

        previewContainer.html("");
        loaderContainer.showLoading();

        $.ajax({
            type: 'GET',
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
    * Restores specified version from history
    */
    function restoreVersion(container, id, isImage) {
        var dialog;

        var extraButton = [];
        var overrideAcceptButtonTitle = null;
        var overrideOnShowFunction = null;

        var loaderContainer = container.find(selectors.versionPreviewLoaderContainer);
        if (isImage && isImage == true) {
            var createNewVersion = new modal.button(globalization.restoreAsNewVersionButtonTitle, null, 5, function() {

                var url = $.format(links.restoreMediaVersionUrl, id, "false"),
                    onComplete = function (json) {
                        messages.refreshBox(container, json);
                        loaderContainer.hideLoading();
                        if (json.Success) {
                            var form = container.find(selectors.mediaHistoryForm);
                            form.submit();
                        }
                    };

                loaderContainer.showLoading();

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

                dialog.close();
            });

            extraButton = [createNewVersion];
            overrideAcceptButtonTitle = globalization.restoreWithOverrideButtonTitle;
            overrideOnShowFunction = function() {
                $(selectors.popinfoFrame).width(400);
            };
        }

        dialog = modal.confirm({
            content: globalization.mediaVersionRestoreConfirmation,
            acceptTitle: globalization.restoreButtonTitle,
// TODO: temporary disabling feature #1055.
//            acceptTitle: overrideAcceptButtonTitle,
//            buttons: extraButton,
            onAccept: function () {
                
                var url = $.format(links.restoreMediaVersionUrl, id, "true"),
                        onComplete = function (json) {
                            messages.refreshBox(container, json);
                            if (json.Success) {
                                var form = container.find(selectors.mediaHistoryForm);
                                form.submit();
                            }
                        };

                loaderContainer.showLoading();

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
            },
            onShow: overrideOnShowFunction
        });
    }
   
    /**
    * Posts media history form with search query.
    */
    function searchPageContentHistory(dialog, container, form, isImage) {
        grid.submitGridForm(form, function (data) {
            container.html(data);
            history.initMediaHistoryDialogEvents(dialog, isImage, data, true);
        });
    }

    /**
    * Initializes EditSeo dialog events.
    */
    history.initMediaHistoryDialogEvents = function (dialog, isImage, data, isSearchResult) {
        var container = dialog.container.find(selectors.modalContent);

        container.find(selectors.gridRestoreLinks).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            restoreVersion(container, $(this).data('id'), isImage);
        });
        
        container.find(selectors.gridDownloadLinks).on('click', function (event) {
            bcms.stopEventPropagation(event);
            window.open($.format(links.downloadFileUrl, $(this).data('id')), '_newtab');
        });
        
        container.find(selectors.gridCells).on('click', function () {
            var self = $(this),
                row = self.parents(selectors.firstRow),
                previewLink = row.find(selectors.gridRowPreviewLink),
                id = previewLink.data('id');

            container.find(selectors.gridRows).removeClass(classes.tableActiveRow);
            row.addClass(classes.tableActiveRow);

            previewVersion(container, id);
        });
        
        var form = container.find(selectors.mediaHistoryForm);
        grid.bindGridForm(form, function (data) {
            container.html(data);
            history.initMediaHistoryDialogEvents(dialog, isImage);
        });

        form.on('submit', function (event) {
            bcms.stopEventPropagation(event);
            searchPageContentHistory(dialog, container, form, isImage);
            return false;
        });

        form.find(selectors.mediaHistorySearchButton).on('click', function () {
            var parent = $(this).parent();
            if (!parent.hasClass('bcms-active-search')) {
                form.find(selectors.mediaHistorySearchField).prop('disabled', false);
                parent.addClass('bcms-active-search');
                form.find(selectors.mediaHistorySearchField).focus();
            } else {
                form.find(selectors.mediaHistorySearchField).prop('disabled', true);
                parent.removeClass('bcms-active-search');
                form.find(selectors.mediaHistorySearchField).val('');
            }
        });

        if (isSearchResult === true) {
            form.find(selectors.mediaHistorySearchButton).parent().addClass('bcms-active-search');
        } else {
            form.find(selectors.mediaHistorySearchField).prop('disabled', true);
        }
    };   
    
    /**
    * Loads history preview dialog.
    */
    history.openMediaHistoryDialog = function (mediaId, isImage, onClose) {
        modal.open({
            title: globalization.mediaHistoryDialogTitle,
            cancelTitle: globalization.closeButtonTitle,
            disableAccept: true,
            onLoad: function (dialog) {
                var url = $.format(links.loadMediaHistoryDialogUrl, mediaId);
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable : function () {
                        history.initMediaHistoryDialogEvents(dialog, isImage);
                    },
                        
                    beforePost: function () {
                        dialog.container.showLoading();
                    },
                                   
                    postComplete: function () {
                        dialog.container.hideLoading();
                    }
                });
            },
            onClose: onClose
        });
    };
        
    return history;
});
