/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.history', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.messages', 'bcms.dynamicContent', 'bcms.redirect', 'bcms.grid'],
    function ($, bcms, modal, messages, dynamicContent, redirect, grid) {
    'use strict';

    var history = {},
        
        classes = {
            tableActiveRow: 'bcms-table-row-active'
        },
        
        selectors = {
            gridRestoreLinks: '#bcms-pagecontenthistory-form .bcms-history-cell a.bcms-icn-restore',
            gridCells: '#bcms-pagecontenthistory-form .bcms-history-cell tbody td',
            gridRowPreviewLink: 'a.bcms-icn-preview:first',
            firstRow: 'tr:first',
            gridRows: '#bcms-pagecontenthistory-form .bcms-history-cell tbody tr',
            versionPreviewContainer: '#bcms-history-preview',
            versionPreviewLoaderContainer: '.bcms-history-preview',
            versionPreviewTemplate: '#bcms-history-preview-template',
            pageContentHistoryForm: '#bcms-pagecontenthistory-form',
            pageContentHistorySearchButton: '.bcms-btn-search',
            modalContent: '.bcms-modal-content-padded'
        },
        
        links = {
            loadContentHistoryDialogUrl: null,
            loadContentVersionPreviewUrl: null,
            restoreContentVersionUrl: null,
            destroyContentDraftVersionUrl: null
        },
        
        globalization = {
            contentHistoryDialogTitle: null,
            contentVersionRestoreConfirmation: null,
            contentVersionDestroyDraftConfirmation: null,
            restoreButtonTitle: null
        };

    /**
    * Assign objects to module.
    */
    history.links = links;
    history.globalization = globalization;

    /**
    * Preview specified content version
    */
    function previewVersion(container, id) {
        var url = $.format(links.loadContentVersionPreviewUrl, id),
            iFrame = $(container.find(selectors.versionPreviewTemplate).html()),
            previewContainer = container.find(selectors.versionPreviewContainer),
            loaderContainer = container.find(selectors.versionPreviewLoaderContainer);

        previewContainer.html(iFrame);
        loaderContainer.showLoading();

        iFrame.on('load', function () {
            loaderContainer.hideLoading();
        });
        
        iFrame.attr('src', url);
    }

    /**
    * Restores specified version from history
    */
    function restoreVersion(container, id) {
        modal.confirm({
            content: globalization.contentVersionRestoreConfirmation,
            acceptTitle: globalization.restoreButtonTitle,
            onAccept: function () {
                
                var url = $.format(links.restoreContentVersionUrl, id),
                        onComplete = function (json) {
                            messages.refreshBox(container, json);
                            
                            if (json.Success) {
                                redirect.ReloadWithAlert();
                            }
                        };

                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: url,
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
    * Posts content history form with search query
    */
    function searchPageContentHistory(dialog, container, form) {
        grid.submitGridForm(form, function (data) {
            container.html(data);
            history.initPageContentHistoryDialogEvents(dialog, data);
        });
    }

    /**
    * Initializes EditSeo dialog events.
    */
    history.initPageContentHistoryDialogEvents = function (dialog) {
        dialog.maximizeHeight();

        var container = dialog.container.find(selectors.modalContent);

        container.find(selectors.gridRestoreLinks).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            restoreVersion(container, $(this).data('id'));
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
        
        var form = container.find(selectors.pageContentHistoryForm);
        grid.bindGridForm(form, function (data) {
            container.html(data);
            history.initPageContentHistoryDialogEvents(dialog);
        });

        form.on('submit', function (event) {
            bcms.stopEventPropagation(event);
            searchPageContentHistory(dialog, container, form);
            return false;
        });

        form.find(selectors.pageContentHistorySearchButton).on('click', function () {
            searchPageContentHistory(dialog, container, form);
        });
    };   
    
    /**
    * Loads history preview dialog.
    */
    history.openPageContentHistoryDialog = function (contentId, pageContentId) {
        modal.open({
            title: globalization.contentHistoryDialogTitle,
            disableAccept: true,
            onLoad: function (dialog) {
                var url = $.format(links.loadContentHistoryDialogUrl, contentId, pageContentId);
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable : function () {
                        history.initPageContentHistoryDialogEvents(dialog);
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
        
    /**
    * Destroys draft version of the content
    */
    history.destroyDraftVersion = function(id, container, onSuccess) {
        modal.confirm({
            content: globalization.contentVersionDestroyDraftConfirmation,
            acceptTitle: globalization.destroyButtonTitle,
            onAccept: function() {

                var url = $.format(links.destroyContentDraftVersionUrl, id),
                    onComplete = function (json) {
                        container.hideLoading();

                        var messagesContainer = container.find(messages.selectors.messages).last();
                        if (messagesContainer.length == 0) {
                            messagesContainer = container;
                        }
                        messages.refreshBox(messagesContainer, json);

                        if (json.Success) {
                            if ($.isFunction(onSuccess)) {
                                var publishedId = json.Data ? json.Data.PublishedId : null;
                                
                                onSuccess.call(this, publishedId, json);
                            }
                        }
                    };

                container.showLoading();

                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: url,
                })
                    .done(function(result) {
                        onComplete(result);
                    })
                    .fail(function(response) {
                        onComplete(bcms.parseFailedResponse(response));
                    });
            }
        });
    };
    
    return history;
});
