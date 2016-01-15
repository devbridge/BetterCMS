/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.pages.history.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.pages.history', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.messages', 'bcms.dynamicContent', 'bcms.redirect', 'bcms.grid'],
    function ($, bcms, modal, messages, dynamicContent, redirect, grid) {
    'use strict';

    var history = {},
        
        classes = {
            tableActiveRow: 'bcms-table-row-active'
        },
        
        selectors = {
            gridRestoreLinks: '#bcms-pagecontenthistory-form tr .bcms-js-restore',
            gridCells: '#bcms-pagecontenthistory-form tr td',
            gridRowPreviewLink: '.bcms-js-preview:first',
            firstRow: 'tr:first',
            gridRows: '#bcms-pagecontenthistory-form tr',
            versionPreviewContainer: '#bcms-history-preview',
            versionPreviewPropertiesContainer: '#bcms-history-preview-properties',
            versionPreviewLoaderContainer: '.bcms-history-preview',
            versionPreviewTemplate: '#bcms-history-preview-template',
            pageContentHistoryForm: '#bcms-pagecontenthistory-form',
            pageContentHistorySearchButton: '.bcms-btn-search',
            pageContentHistorySearchField: '.bcms-search-query',
            modalContent: '.bcms-modal-content',
            modalFrameHolder: '.bcms-modal-frame-holder',
            activeTab: '.bcms-js-tab-header .bcms-modal-frame-holder>.bcms-active',
            inactiveTab: '.bcms-js-tab-header .bcms-modal-frame-holder>:not(.bcms-active)'
        },
        
        links = {
            loadContentHistoryDialogUrl: null,
            loadContentVersionPreviewUrl: null,
            loadContentVersionPreviewPropertiesUrl: null,
            restoreContentVersionUrl: null,
            destroyContentDraftVersionUrl: null
        },
        
        globalization = {
            contentHistoryDialogTitle: null,
            contentVersionRestoreConfirmation: null,
            contentVersionDestroyDraftConfirmation: null,
            versionPreviewNotAvailableMessage: null,
            restoreButtonTitle: null,
            closeButtonTitle: null
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
        var viewUrl = $.format(links.loadContentVersionPreviewUrl, id),
            previewIFrame = $(container.find(selectors.versionPreviewTemplate).html()),
            previewContainer = container.find(selectors.versionPreviewContainer),
            loaderContainer = container.find(selectors.versionPreviewContainer),
            activeTab = container.find(selectors.activeTab),

            propertiesUrl = $.format(links.loadContentVersionPreviewPropertiesUrl, id),
            previewPropertiesContainer = container.find(selectors.versionPreviewPropertiesContainer),
            previewPropertiesLoaderContainer = container.find(selectors.versionPreviewPropertiesContainer),
            
            previewIsNotAvailableMessage = "<div class=\"bcms-history-preview\" id=\"bcms-history-preview-properties\" style=\"height: 100%\"><div class=\"bcms-history-info\" style=\"display: block;\">" +
                                           globalization.versionPreviewNotAvailableMessage + "</div></div>";

        previewContainer.html(previewIFrame);
        if (activeTab.data('name') === '#bcms-tab-1') {
            loaderContainer.showLoading();
        } else {
            previewPropertiesLoaderContainer.showLoading();
        }

        previewIFrame.on('load', function () {
            loaderContainer.hideLoading();
        });
        
        previewIFrame.attr('src', viewUrl);

        $.ajax({
                type: 'GET',
                cache: false,
                url: propertiesUrl
            })
            .done(function(result) {
                previewPropertiesLoaderContainer.hideLoading();
                if (result === "") {
                    previewPropertiesContainer.html(previewIsNotAvailableMessage);
                } else {
                    previewPropertiesContainer.html(result);
                }
            })
            .fail(function() {
                previewPropertiesLoaderContainer.hideLoading();
                previewPropertiesContainer.html(previewIsNotAvailableMessage);
            });
    }

    /**
    * Restores specified version from history.
    */
    function restoreVersion(dialog, container, id, onContentRestore, includeChildRegions) {
        var submitRestoreIt = function (isConfirmed) {
            var url = $.format(links.restoreContentVersionUrl, id, isConfirmed, includeChildRegions),
                onComplete = function (json) {
                    messages.refreshBox(container, json);
                    if (json.Success) {
                        if ($.isFunction(onContentRestore)) {
                            dialog.close();
                            onContentRestore(json);
                        } else {
                            redirect.ReloadWithAlert();
                        }
                    } else {
                        if (json.Data && json.Data.ConfirmationMessage) {
                            modal.confirm({
                                content: json.Data.ConfirmationMessage,
                                onAccept: function() {
                                    submitRestoreIt(1);
                                }
                            });
                        }
                    }
                };

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
        };

        modal.confirm({
            content: globalization.contentVersionRestoreConfirmation,
            acceptTitle: globalization.restoreButtonTitle,
            onAccept: function() {
                submitRestoreIt(0);
            }
        });
    }
   
    /**
    * Posts content history form with search query.
    */
    function searchPageContentHistory(dialog, container, form) {
        grid.submitGridForm(form, function (data) {
            var activeTabName = dialog.container.find(selectors.activeTab).data('name'),
                inactiveTabName = dialog.container.find(selectors.inactiveTab).data('name');
            form.closest(selectors.modalFrameHolder).html(data);
            dialog.container.find(activeTabName).show();
            dialog.container.find(inactiveTabName).hide();
            history.initPageContentHistoryDialogEvents(dialog, data, true);
        });
    }

    /**
    * Initializes EditSeo dialog events.
    */
    history.initPageContentHistoryDialogEvents = function (dialog, opts, isSearchResult) {
        var container = dialog.container.find(selectors.modalContent);
        container.find(selectors.gridRestoreLinks).on('click', function (event) {
            bcms.stopEventPropagation(event);
            restoreVersion(dialog, container, $(this).data('id'), opts.onContentRestore, opts.includeChildRegions);
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
            var activeTabName = dialog.container.find(selectors.activeTab).data('name'),
                inactiveTabName = dialog.container.find(selectors.inactiveTab).data('name');
            form.closest(selectors.modalFrameHolder).html(data);
            dialog.container.find(activeTabName).show();
            dialog.container.find(inactiveTabName).hide();
            history.initPageContentHistoryDialogEvents(dialog, opts, isSearchResult);
        });

        form.on('submit', function (event) {
            bcms.stopEventPropagation(event);
            searchPageContentHistory(dialog, container, form);
            return false;
        });

        bcms.preventInputFromSubmittingForm(form.find(selectors.pageContentHistorySearchField), {
            preventedEnter: function () {
                searchPageContentHistory(dialog, container, form);
            }
        });

        form.find(selectors.pageContentHistorySearchButton).on('click', function () {
            var parent = $(this).parent();
            if (!parent.hasClass('bcms-active-search')) {
                form.find(selectors.pageContentHistorySearchField).prop('disabled', false);
                parent.addClass('bcms-active-search');
                form.find(selectors.pageContentHistorySearchField).focus();
            } else {
                form.find(selectors.pageContentHistorySearchField).prop('disabled', true);
                parent.removeClass('bcms-active-search');
                form.find(selectors.pageContentHistorySearchField).val('');
            }
        });

        if (isSearchResult === true) {
            form.find(selectors.pageContentHistorySearchButton).parent().addClass('bcms-active-search');
        } else {
            form.find(selectors.pageContentHistorySearchField).prop('disabled', true);
        }
    };   
    
    /**
    * Loads history preview dialog.
    */
    history.openPageContentHistoryDialog = function (contentId, pageContentId, opts) {
        opts = $.extend({
            onContentRestore: null,
            includeChildRegions: false
        }, opts);

        modal.open({
            title: globalization.contentHistoryDialogTitle,
            cancelTitle: globalization.closeButtonTitle,
            disableAccept: true,
            onLoad: function (dialog) {
                var url = $.format(links.loadContentHistoryDialogUrl, contentId, pageContentId);
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable : function () {
                        history.initPageContentHistoryDialogEvents(dialog, opts);
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
    history.destroyDraftVersion = function(id, version, includeChildRegions, container, onSuccess) {
        modal.confirm({
            content: globalization.contentVersionDestroyDraftConfirmation,
            acceptTitle: globalization.destroyButtonTitle,
            onAccept: function() {

                var url = $.format(links.destroyContentDraftVersionUrl, id, version, includeChildRegions ? 1 : 0),
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
                    url: url
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
