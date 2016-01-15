/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.preview.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.preview', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.dynamicContent'], function ($, bcms, modal, dynamicContent) {
    'use strict';

    var preview = {},

    // Classes that are used to maintain various UI states:
        classes = {
        },

    // Selectors used in the module to locate DOM elements:
        selectors = {
            previewZoom: '.bcms-js-zoom-overlay',
            elementsToDisable: '.bcms-modal-content a, .bcms-modal-content input, .bcms-modal-content select'
        },

        links = {
            previewPageUrl: null
        },
        
        globalization = {
            closeButtonTitle: null
        };

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
                cancelTitle: globalization.closeButtonTitle,
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
        window.open(link, bcms.previewWindow);
    };
    
    return preview;
});
