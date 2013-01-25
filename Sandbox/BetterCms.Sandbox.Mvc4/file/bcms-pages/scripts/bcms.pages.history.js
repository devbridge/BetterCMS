/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.history', ['jquery', 'bcms', 'bcms.modal', 'bcms.messages', 'bcms.dynamicContent', 'bcms.redirect', 'bcms.grid'],
    function ($, bcms, modal, messages, dynamicContent, redirect, grid) {
    'use strict';

    var history = {},
        
        classes = {
            tableActiveRow: 'bcms-table-row-active'
        },
        
        selectors = {
            gridRestoreLinks: '#bcms-pagecontenthistory-form .bcms-history-table a.bcms-icn-restore',
            gridCells: '#bcms-pagecontenthistory-form .bcms-history-table tbody td',
            gridRowPreviewLink: 'a.bcms-icn-preview:first',
            firstRow: 'tr:first',
            gridRows: '#bcms-pagecontenthistory-form .bcms-history-table tbody tr',
            versionPreviewContainer: '.bcms-history-preview',
            versionPreviewTemplate: '#bcms-history-preview-template',
            pageContentHistoryForm: '#bcms-pagecontenthistory-form',
            pageContentHistorySearchButton: '.bcms-btn-search',
            modalContent: '.bcms-modal-content-padded'
        },
        
        links = {
            loadPageContentHistoryDialogUrl: null,
            loadPageContentVersionPreviewUrl: null,
            restorePageContentVersionUrl: null,
            loadPageContentHistoryUrl: null
        },
        
        globalization = {
            pageContentHistoryDialogTitle: null,
            pageContentVersionRestoryConfirmation: null
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
        var url = $.format(links.loadPageContentVersionPreviewUrl, id),
            template = $(container.find(selectors.versionPreviewTemplate).html()).attr('src', url),
            previewContainer = container.find(selectors.versionPreviewContainer);

        previewContainer.html(template);
    }

    /**
    * Restores specified version from history
    */
    function restoreVersion(container, id) {
        modal.confirm({
            content: globalization.pageContentVersionRestoryConfirmation,
            onAccept: function () {
                
                var url = $.format(links.restorePageContentVersionUrl, id),
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
            dialog.maximizeHeight();
        });
    }

    /**
    * Initializes EditSeo dialog events.
    */
    history.initPageContentHistoryDialogEvents = function (dialog) {
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
    * Loads edit SEO dialog.
    */
    history.openPageContentHistoryDialog = function (contentId, pageContentId) {
        modal.open({
            title: globalization.pageContentHistoryDialogTitle,            
            onLoad: function (dialog) {
                var url = $.format(links.loadPageContentHistoryDialogUrl, contentId, pageContentId);
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
    
    return history;
});
