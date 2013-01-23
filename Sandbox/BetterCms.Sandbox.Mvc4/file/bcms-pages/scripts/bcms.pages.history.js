/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.history', ['jquery', 'bcms', 'bcms.modal', 'bcms.messages', 'bcms.dynamicContent'], function ($, bcms, modal, messages, dynamicContent) {
    'use strict';

    var history = {},
        
        classes = {
        },
        
        selectors = {
            gridRestoreLinks: '#bcms-pagecontenthistory-form .bcms-history-table a.bcms-icn-restore',
            gridCells: '#bcms-pagecontenthistory-form .bcms-history-table tbody td',
            gridRowPreviewLink: 'a.bcms-icn-preview:first',
            firstRow: 'tr:first',
            versionPreviewContainer: '.bcms-history-preview',
            versionPreviewTemplate: '#bcms-history-preview-template'
        },
        
        links = {
            loadPageContentHistoryDialogUrl: null,
            loadPageContentVersionPreviewUrl: null,
            restorePageContentVersionUrl: null
        },
        
        globalization = {
            pageContentHistoryDialogTitle: null            
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
    function restoreVersion(id) {
        alert('TODO: restore version');
    }
   
    /**
    * Initializes EditSeo dialog events.
    */
    history.initPageContentHistoryDialogEvents = function (dialog) {
        var container = dialog.container;

        container.find(selectors.gridRestoreLinks).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            restoreVersion($(this).data('id'));
        });
        
        container.find(selectors.gridCells).on('click', function () {
            var self = $(this),
                previewLink = self.parents(selectors.firstRow).find(selectors.gridRowPreviewLink),
                id = previewLink.data('id');

            previewVersion(container, id);
        });
    };   
    
    /**
    * Loads edit SEO dialog.
    */
    history.openPageContentHistoryDialog = function (contentId, contentVersion, pageContentId, pageContentVersion) {
        modal.open({
            title: globalization.pageContentHistoryDialogTitle,            
            onLoad: function (dialog) {
                var url = $.format(links.loadPageContentHistoryDialogUrl, pageContentId, pageContentVersion, contentId, contentVersion);
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
