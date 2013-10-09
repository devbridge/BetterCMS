/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.media.history', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.messages', 'bcms.dynamicContent', 'bcms.redirect', 'bcms.grid'],
    function ($, bcms, modal, messages, dynamicContent, redirect, grid) {
    'use strict';

    var history = {},
        
        classes = {
            tableActiveRow: 'bcms-table-row-active'
        },
        
        selectors = {
            gridRestoreLinks: '#bcms-pagecontenthistory-form .bcms-history-cell a.bcms-icn-restore',
            gridDownloadLinks: '#bcms-pagecontenthistory-form .bcms-history-cell a.bcms-icn-download',
            gridCells: '#bcms-pagecontenthistory-form .bcms-history-cell tbody td',
            gridRowPreviewLink: 'a.bcms-icn-preview:first',
            firstRow: 'tr:first',
            gridRows: '#bcms-pagecontenthistory-form .bcms-history-cell tbody tr',
            versionPreviewContainer: '#bcms-history-preview',
            versionPreviewLoaderContainer: '.bcms-history-preview',
            mediaHistoryForm: '#bcms-pagecontenthistory-form',
            mediaHistorySearchButton: '.bcms-btn-search',
            modalContent: '.bcms-modal-content-padded'
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
    function restoreVersion(container, id) {
        modal.confirm({
            content: globalization.mediaVersionRestoreConfirmation,
            acceptTitle: globalization.restoreButtonTitle,
            onAccept: function () {
                
                var url = $.format(links.restoreMediaVersionUrl, id),
                        onComplete = function (json) {
                            messages.refreshBox(container, json);
                            if (json.Success) {
                                var form = container.find(selectors.mediaHistoryForm);
                                form.submit();
                            }
                        };

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
            }
        });
    }
   
    /**
    * Posts media history form with search query.
    */
    function searchPageContentHistory(dialog, container, form) {
        grid.submitGridForm(form, function (data) {
            container.html(data);
            history.initMediaHistoryDialogEvents(dialog, data);
        });
    }

    /**
    * Initializes EditSeo dialog events.
    */
    history.initMediaHistoryDialogEvents = function (dialog) {
        dialog.maximizeHeight();

        var container = dialog.container.find(selectors.modalContent);

        container.find(selectors.gridRestoreLinks).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            restoreVersion(container, $(this).data('id'));
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
            history.initMediaHistoryDialogEvents(dialog);
        });

        form.on('submit', function (event) {
            bcms.stopEventPropagation(event);
            searchPageContentHistory(dialog, container, form);
            return false;
        });

        form.find(selectors.mediaHistorySearchButton).on('click', function () {
            searchPageContentHistory(dialog, container, form);
        });
    };   
    
    /**
    * Loads history preview dialog.
    */
    history.openMediaHistoryDialog = function (mediaId, onClose) {
        modal.open({
            title: globalization.mediaHistoryDialogTitle,
            cancelTitle: globalization.closeButtonTitle,
            disableAccept: true,
            onLoad: function (dialog) {
                var url = $.format(links.loadMediaHistoryDialogUrl, mediaId);
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable : function () {
                        history.initMediaHistoryDialogEvents(dialog);
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
