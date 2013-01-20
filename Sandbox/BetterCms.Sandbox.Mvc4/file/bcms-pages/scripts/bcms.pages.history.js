/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.history', ['jquery', 'bcms', 'bcms.modal', 'bcms.messages', 'bcms.dynamicContent'], function ($, bcms, modal, messages, dynamicContent) {
    'use strict';

    var history = {},
        
        classes = {
        },
        
        selectors = {          
        },
        
        links = {
            loadPageContentHistoryDialogUrl: null
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
    * Initializes EditSeo dialog events.
    */
    history.initPageContentHistoryDialogEvents = function (dialog) {                
     
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
