/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.preview', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.dynamicContent'], function ($, bcms, modal, dynamicContent) {
    'use strict';

    var preview = {},

    // Classes that are used to maintain various UI states:
        classes = {
        },

    // Selectors used in the module to locate DOM elements:
        selectors = {
            previewZoom: '.bcms-zoom-overlay',
            elementsToDisable: '.bcms-modal-content a, .bcms-modal-content input, .bcms-modal-content select'
        },

        links = {
            previewPageUrl: null
        },
        
        globalization = {};

    // Assign objects to module
    preview.classes = classes;
    preview.selectors = selectors;
    preview.links = links;
    preview.globalization = globalization;

    preview.initialize = function (container, selector) {
        selector = selector || selectors.previewZoom;
        container.find(selector).on('click', function () {
            var self = $(this),
                title = self.data('previewTitle'),
                url = self.data('previewUrl');
            
            modal.open({
                title: title,
                disableAccept: true,
                onLoad: function(previewDialog) {
                    dynamicContent.bindDialog(previewDialog, url, {
                        contentAvailable: function () {
                            previewDialog.container
                                .find(selectors.elementsToDisable)
                                .attr('disabled', 'disabled')
                                .on('click', function () { return false; });
                        }
                    });
                }
            });
        });
    };

    preview.previewPageContent = function (pageId, pageContentId) {
        var link = $.format(links.previewPageUrl, pageId, pageContentId);
        window.open(link, bcms.previewWindow, 'toolbar=yes,location=yes,directories=yes,status=yes,menubar=yes,scrollbars=yes,copyhistory=yes,resizable=yes');
    };
    
    return preview;
});
