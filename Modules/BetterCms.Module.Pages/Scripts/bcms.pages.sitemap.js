/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.sitemap', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'bcms.ko.extenders', 'bcms.grid', 'bcms.security', 'bcms.tags', 'bcms.antiXss'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, ko, grid, security, tags, antiXss) {
        'use strict';

        var sitemap = {},
            selectors = {
                sitemapAddNodeDataBind: "#bcms-sitemap-addnode",
                sitemapAddNewPageDataBind: "#bcms-sitemap-addnewpage",
                sitemapForm: ".bcms-sitemap-form",
                scrollDiv: "#bcms-scroll-window",
                
                searchField: '.bcms-search-query',
                searchButton: '#bcms-sitemaps-search-btn',

                siteSettingsSitemapsForm: "#bcms-sitemaps-form",
                siteSettingsSitemapCreateButton: '#bcms-create-sitemap-button',
                siteSettingsSitemapTitleCell: '.bcms-sitemap-title',
                siteSettingsSitemapRow: 'tr',
                siteSettingsRowCells: 'td',
                siteSettingsSitemapParentRow: 'tr:first',
                siteSettingsSitemapEditButton: '.bcms-grid-item-edit-button',
                siteSettingsSitemapHistoryButton: '.bcms-grid-item-history-button',
                siteSettingsSitemapDeleteButton: '.bcms-grid-item-delete-button',
                siteSettingsSitemapRowTemplate: '#bcms-sitemap-list-row-template',
                siteSettingsSitemapRowTemplateFirstRow: 'tr:first',
                siteSettingsSitemapsTableFirstRow: 'table.bcms-tables > tbody > tr:first',

                gridRestoreLinks: '#bcms-sitemaphistory-form .bcms-history-cell a.bcms-icn-restore',
                gridCells: '#bcms-sitemaphistory-form .bcms-history-cell tbody td',
                gridRowPreviewLink: 'a.bcms-icn-preview:first',
                firstRow: 'tr:first',
                gridRows: '#bcms-sitemaphistory-form .bcms-history-cell tbody tr',
                versionPreviewContainer: '#bcms-history-preview',
                versionPreviewLoaderContainer: '.bcms-history-preview',
                sitemapHistoryForm: '#bcms-sitemaphistory-form',
                sitemapHistorySearchButton: '.bcms-btn-search',
                modalContent: '.bcms-modal-content-padded',
                firstTab: '#bcms-tab-1',
                secondTab: '#bcms-tab-2',
                secondTabButton: '.bcms-tab-item[data-name="#bcms-tab-2"]',
                leftColumn: '.bcms-leftcol',
                
                tabsSlider: '.bcms-tab-header:first',
                tabsSliderLeftArrow: '.bcms-sitemaps-arrow-left',
                tabsSliderRightArrow: '.bcms-sitemaps-arrow-right'
            },
            links = {
                loadSiteSettingsSitemapsListUrl: null,
                saveSitemapUrl: null,
                saveSitemapNodeUrl: null,
                deleteSitemapUrl: null,
                deleteSitemapNodeUrl: null,
                sitemapEditDialogUrl: null,
                sitemapAddNewPageDialogUrl: null,
                saveMultipleSitemapsUrl: null,
                sitemapHistoryDialogUrl: null,
                loadSitemapVersionPreviewUrl: null,
                restoreSitemapVersionUrl: null,
                getPageTranslations: null
            },
            globalization = {
                sitemapCreatorDialogTitle: null,
                sitemapEditorDialogTitle: null,
                sitemapEditorDialogCustomLinkTitle: null,
                sitemapAddNewPageDialogTitle: null,
                sitemapDeleteNodeConfirmationMessage: null,
                sitemapDeleteConfirmMessage: null,
                sitemapSomeNodesAreInEditingState: null,
                sitemapNodeSaveButton: null,
                sitemapNodeOkButton: null,
                
                sitemapPlaceLinkHere: null,
                sitemapIsEmpty: null,
                
                sitemapHistoryDialogTitle: null,
                sitemapVersionRestoreConfirmation: null,
                restoreButtonTitle: null,
                closeButtonTitle: null,
                
                invariantLanguage: null
            },
            classes = {
                tableActiveRow: 'bcms-table-row-active'
            },
            events = {
                sitemapNodeAdded: 'sitemapNodeAdded',
                sitemapNodeRemoved: 'sitemapNodeRemoved',
                sitemapNodeTranslationsUpdated: 'sitemapNodeTranslationsUpdated'
            },
            defaultIdValue = '00000000-0000-0000-0000-000000000000',
            DropZoneTypes = {
                None: 'none',
                EmptyListZone: 'emptyListZone',
                TopZone: 'topZone',
                MiddleZone: 'middleZone',
                BottomZone: 'bottomZone'
            },
            nodeId = 0,
            tabId = 0;

        /**
        * Assign objects to module.
        */
        sitemap.links = links;
        sitemap.globalization = globalization;
        sitemap.activeMapModel = null;
        sitemap.activeLoadingContainer = null;
        sitemap.activeMessageContainer = null;
        sitemap.events = events;
        /**
        * Loads a sitemap view to the site settings container.
        */
        sitemap.loadSiteSettingsSitemapList = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsSitemapsListUrl, {
                contentAvailable: sitemap.initializeSitemapsList
            });
        };

        /**
        * Initializes site settings master pages list.
        */
        sitemap.initializeSitemapsList = function () {
            var dialog = siteSettings.getModalDialog(),
                container = dialog.container,
                form = container.find(selectors.siteSettingsSitemapsForm);

            grid.bindGridForm(form, function (data) {
                siteSettings.setContent(data);
                sitemap.initializeSitemapsList();
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSitemaps(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.searchField), {
                preventedEnter: function () {
                    searchSitemaps(form);
                },
            });

            form.find(selectors.searchButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                searchSitemaps(form);
            });

            initializeListItems(container);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(container.find(selectors.searchField), true);
        };
        
        /*
        * Submit sitemap form to force search.
        */
        function searchSitemaps(form) {
            grid.submitGridForm(form, function (htmlContent) {
                siteSettings.setContent(htmlContent);
                sitemap.initializeSitemapsList();
            });
        };

        /*
        * Attach the sitemap grid events.
        */
        function initializeListItems(container, masterContainer) {
            container.find(selectors.siteSettingsSitemapCreateButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                addSitemap(masterContainer || container);
            });

            container.find(selectors.siteSettingsRowCells).on('click', function (event) {
                bcms.stopEventPropagation(event);
                var editButton = $(this).parents(selectors.siteSettingsSitemapRow).find(selectors.siteSettingsSitemapEditButton);
                if (editButton.length > 0) {
                    editSitemap(editButton, masterContainer || container);
                }
            });

            container.find(selectors.siteSettingsSitemapHistoryButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                viewSitemapHistory($(this), masterContainer || container);
            });
            
            container.find(selectors.siteSettingsSitemapDeleteButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                deleteSitemap($(this), masterContainer || container);
            });
        };

        /*
        * Create the sitemap and add it to the list.
        */
        function addSitemap(container) {
            sitemap.openCreateSitemapDialog(function (data) {
                if (data.Data != null) {
                    var template = $(selectors.siteSettingsSitemapRowTemplate),
                        newRow = $(template.html()).find(selectors.siteSettingsSitemapRowTemplateFirstRow);

                    newRow.find(selectors.siteSettingsSitemapTitleCell).html(antiXss.encodeHtml(data.Data.Title));
                    newRow.find(selectors.siteSettingsSitemapEditButton).data('id', data.Data.Id);
                    newRow.find(selectors.siteSettingsSitemapHistoryButton).data('id', data.Data.Id);
                    newRow.find(selectors.siteSettingsSitemapDeleteButton).data('id', data.Data.Id);
                    newRow.find(selectors.siteSettingsSitemapDeleteButton).data('version', data.Data.Version);

                    newRow.insertBefore($(selectors.siteSettingsSitemapsTableFirstRow, container));

                    initializeListItems(newRow, container);

                    grid.showHideEmptyRow(container);
                }
            }, true);
        };

        /*
        * Edit the sitemap.
        */
        function editSitemap(self, container) {
            var id = self.data('id');

            sitemap.loadAddNodeDialog(id, function (data) {
                if (data.Data != null) {
                    var row = self.parents(selectors.siteSettingsSitemapParentRow),
                        cell = row.find(selectors.siteSettingsSitemapTitleCell);
                    cell.html(antiXss.encodeHtml(data.Data.Title));
                    row.find(selectors.siteSettingsSitemapDeleteButton).data('version', data.Data.Version);
                }
            }, globalization.sitemapEditorDialogTitle);
        };

        /*
        * View sitemap history.
        */
        function viewSitemapHistory(self, container) {
            var id = self.data('id');

            sitemap.showSitemapHistoryDialog(id, function (data) {
                if (data.Data != null) {
                    var row = self.parents(selectors.siteSettingsSitemapParentRow),
                        cell = row.find(selectors.siteSettingsSitemapTitleCell);
                    cell.html(antiXss.encodeHtml(data.Data.Title));
                    row.find(selectors.siteSettingsSitemapDeleteButton).data('version', data.Data.Version);
                }
            }, globalization.sitemapHistoryDialogTitle);
        };

        /*
        * Delete the sitemap and remove it form the list.
        */
        function deleteSitemap(self, container) {
            var id = self.data('id'),
                version = self.data('version');

            sitemap.deleteSitemap(id, version, function (json) {
                messages.refreshBox(selectors.siteSettingsSitemapsForm, json);
                if (json.Success) {
                    self.parents(selectors.siteSettingsSitemapParentRow).remove();
                    grid.showHideEmptyRow(container);
                }
            });
        };

        /*
        * Shows sitemap creation form.
        */
        sitemap.openCreateSitemapDialog = function (callBackOnSuccess) {
            sitemap.loadAddNodeDialog(defaultIdValue, callBackOnSuccess, globalization.sitemapCreatorDialogTitle);
        };

        /**
        * Loads a sitemap view to the add node dialog container.
        */
        sitemap.loadAddNodeDialog = function (id, onClose, dialogTitle) {
            var addNodeController = new AddNodeMapController();
            modal.open({
                title: dialogTitle || globalization.sitemapEditorDialogTitle,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, $.format(links.sitemapEditDialogUrl, id), {
                        done: function (content) {
                            addNodeController.initialize(content, dialog);
                        }
                    });
                },
                onAccept: function (dialog) {
                    var canContinue = forms.valid(dialog.container.find(selectors.sitemapForm));
                    
                    if (canContinue) {
                        addNodeController.save(function (json) {
                            if (json.Success) {
                                dialog.close();
                                if (onClose && $.isFunction(onClose)) {
                                    onClose(json);
                                }
                                messages.refreshBox(selectors.siteSettingsSitemapsForm, json);
                            } else {
                                sitemap.showMessage(json);
                            }
                        });
                    }
                    return false;
                }
            });
        };


        /**
        * Loads a sitemap history dialog.
        */
        sitemap.showSitemapHistoryDialog = function(id, afterSitemapRestored, dialogTitle) {
            modal.open({
                title: dialogTitle || globalization.sitemapHistoryDialogTitle,
                cancelTitle: globalization.closeButtonTitle,
                disableAccept: true,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, $.format(links.sitemapHistoryDialogUrl, id), {
                        contentAvailable: function () {
                            initSitemapHistoryDialogEvents(dialog, afterSitemapRestored);
                        },

                        beforePost: function () {
                            dialog.container.showLoading();
                        },

                        postComplete: function () {
                            dialog.container.hideLoading();
                        }
                    });
                },
            });
        };

        /*
        * Deletes the sitemap if user confirms.
        */
        sitemap.deleteSitemap = function (id, version, callBack) {
            var url = $.format(links.deleteSitemapUrl, id, version),
                onDeleteCompleted = function (json) {
                if ($.isFunction(callBack)) {
                    callBack(json);
                }
            };
            modal.confirm({
                content: globalization.sitemapDeleteConfirmMessage,
                onAccept: function () {
                    $.ajax({
                        type: 'POST',
                        url: url,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        cache: false
                    })
                    .done(function (json) {
                        onDeleteCompleted(json);
                    })
                    .fail(function (response) {
                        onDeleteCompleted(bcms.parseFailedResponse(response));
                    });
                }
            });
        };

        /**
        * Shows add new page to sitemap dialog.
        */
        sitemap.loadAddNewPageDialog = function(data) {
            if (data && data.Data && (data.Data.IsSitemapActionEnabled !== false) && (data.Data.Title || data.Data.PageTitle) && (data.Data.Url || data.Data.PageUrl) && (data.Data.Id || data.Data.PageId) && !data.Data.IsMasterPage) {
                var addPageController = new AddNewPageMapController(data.Data.Title || data.Data.PageTitle, data.Data.Url || data.Data.PageUrl, data.Data.Id || data.Data.PageId, data.Data.LanguageId || data.Data.PageLanguageId);
                modal.open({
                    title: globalization.sitemapAddNewPageDialogTitle,
                    onLoad: function(dialog) {
                        dynamicContent.setContentFromUrl(dialog, links.sitemapAddNewPageDialogUrl, {
                            done: function(content) {
                                addPageController.initialize(content, dialog);
                            }
                        });
                    },
                    onAccept: function(dialog) {
                        addPageController.save(function() {
                            dialog.close();
                        });
                        return false;
                    },
                    onClose: function() {
                        if (data.Callback && $.isFunction(data.Callback)) {
                            data.Callback(data);
                        }
                    }
                });
            } else {
                if (data.Callback && $.isFunction(data.Callback)) {
                    data.Callback(data);
                }
            }
        };


        // --- Controllers ----------------------------------------------------
        /**
        * Controller for sitemap add node dialog.
        */
        function AddNodeMapController() {
            var self = this;
            self.container = null;
            self.pageLinksModel = null;

            self.initialize = function (content, dialog) {
                self.container = dialog.container;
                sitemap.activeMessageContainer = self.container;
                sitemap.activeLoadingContainer = self.container.find(selectors.sitemapAddNodeDataBind);

                if (content.Success) {
                    // Create data models.
                    var sitemapModel = new SitemapViewModel(content.Data.Sitemap),
                        isReadOnly = content.Data.Sitemap.IsReadOnly;
                    
                    if (!isReadOnly) {
                        sitemap.showMessage(content);
                    } else {
                        // Allow user navigate in sitemap.
                        dialog.container.find(modal.selectors.readonly).removeClass(modal.classes.inactive);
                        dialog.container.find(selectors.firstTab).addClass(modal.classes.inactive);
                    }

                    sitemapModel.parseJsonNodes(content.Data.Sitemap.RootNodes);
                    self.pageLinksModel = new SearchPageLinksViewModel(sitemapModel, dialog.container);
                    self.pageLinksModel.parseJsonLinks(content.Data.PageLinks);
                    sitemap.activeMapModel = sitemapModel;

                    // Setup settings.
                    sitemapModel.settings.canEditNode = !isReadOnly;
                    sitemapModel.settings.canDeleteNode = !isReadOnly;
                    sitemapModel.settings.canDragNode = !isReadOnly;
                    sitemapModel.settings.canDropNode = !isReadOnly;
                    sitemapModel.settings.nodeSaveButtonTitle = globalization.sitemapNodeOkButton;
                    sitemapModel.settings.nodeSaveAfterUpdate = false;

                    // Bind models.
                    var context = self.container.find(selectors.sitemapAddNodeDataBind).get(0);
                    if (context) {
                        ko.applyBindings(self.pageLinksModel, context);
                        updateValidation();
                    }
                } else {
                    sitemap.showMessage(content);
                }
            };
            self.save = function (onDoneCallback) {
                if (sitemap.activeMapModel) {
                    sitemap.activeMapModel.save(onDoneCallback);
                }
            };
        }
        
        /**
        * Controller for sitemap add new page dialog.
        */
        function AddNewPageMapController(title, url, pageId, languageId) {
            var self = this;
            self.container = null;
            self.newPageModel = null;
            self.tabsModel = null;
            
            self.initialize = function (content, dialog) {
                if (!(content != null && content.Data != null && content.Data.length > 0)) {
                    // No sitemaps to place the new page.
                    dialog.close();
                    return;
                }

                self.container = dialog.container;
                sitemap.activeMessageContainer = self.container;
                sitemap.activeLoadingContainer = self.container.find(selectors.sitemapAddNewPageDataBind);

                sitemap.showMessage(content);
                if (content.Success) {
                    var tabsArray = [],
                        onSkip = function() {
                            dialog.close();
                        };

                    tabId = 0;
                    
                    // Create all the tabs.
                    for (var i = 0; i < content.Data.length; i++) {
                        // Create data models.
                        var sitemapModel = new SitemapViewModel(content.Data[i]),
                            pageLinkModel = new PageLinkViewModel(title, url, pageId, languageId),
                            newPageModel = new AddNewPageViewModel(sitemapModel, pageLinkModel, onSkip),
                            tabModel = new TabModel(newPageModel);
                        tabsArray.push(tabModel);
                        sitemapModel.parseJsonNodes(content.Data[i].RootNodes);

                        // Setup settings.
                        sitemapModel.settings.canEditNode = false;
                        sitemapModel.settings.canDeleteNode = false;
                        sitemapModel.settings.canDragNode = false;
                        sitemapModel.settings.canDropNode = true;
                        sitemapModel.settings.nodeSaveButtonTitle = globalization.sitemapNodeSaveButton;
                        sitemapModel.settings.nodeSaveAfterUpdate = false;
                    }

                    self.tabsModel = new TabsModel(tabsArray, self.container);

                    // Bind models.
                    var context = self.container.find(selectors.sitemapAddNewPageDataBind).parent().get(0);
                    if (context) {
                        ko.applyBindings(self.tabsModel, context);
                        updateValidation();
                    }

                    self.tabsModel.slider.initialize();
                }
            };
            self.save = function (onDoneCallback) {
                var tabsArray = self.tabsModel.tabs(),
                    sitemapsToSave = [],
                    onSaveCompleted = function (json) {
                        messages.refreshBox(sitemap.activeMessageContainer, json);
                        sitemap.showLoading(false);
                        if (json.Success) {
                            if (onDoneCallback && $.isFunction(onDoneCallback)) {
                                if (json.Data == null) {
                                    json.Data = {
                                        Title: title,
                                        PageUrl: url,
                                        PageId: pageId
                                    };
                                }
                                onDoneCallback(json);
                            }
                        }
                    };
                
                for (var i = 0; i < tabsArray.length; i++) {
                    if (tabsArray[i].newPageViewModel.linkIsDropped()) {
                        sitemapsToSave.push(tabsArray[i].newPageViewModel.sitemap.getModelToSave());
                    }
                }
                
                if (sitemapsToSave.length > 0) {
                    $.ajax({
                        url: links.saveMultipleSitemapsUrl,
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        cache: false,
                        data: JSON.stringify(sitemapsToSave),
                        beforeSend: function() {
                            sitemap.showLoading(true);
                        }
                    })
                        .done(function (json) {
                            onSaveCompleted(json);
                        })
                        .fail(function (response) {
                            onSaveCompleted(bcms.parseFailedResponse(response));
                        });
                }
            };
        }
        // --------------------------------------------------------------------
        

        // --- Helpers --------------------------------------------------------
        /**
        * Helper function to show json messages.
        */
        sitemap.showMessage = function (content) {
            messages.refreshBox(sitemap.activeMessageContainer, content);
        };
        
        /**
        * Helper function to show loading.
        */
        sitemap.showLoading = function (loading) {
            if (loading) {
                $(sitemap.activeLoadingContainer).showLoading();
            } else {
                $(sitemap.activeLoadingContainer).hideLoading();
            }
        };
        
        /**
        * Helper function to add knockout binding 'draggable'.
        */
        function addDraggableBinding() {
            ko.bindingHandlers.draggable = {
                init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
                    var dragObject = viewModel,
                        setup = {
                            revert: true,
                            revertDuration: 0,
                            refreshPositions: true,
                            scroll: true,
                            containment: $($(selectors.sitemapAddNodeDataBind).get(0) || $(selectors.sitemapAddNewPageDataBind).get(0)).find(selectors.scrollDiv).get(0),
                            appendTo: $($(selectors.sitemapAddNodeDataBind).get(0) || $(selectors.sitemapAddNewPageDataBind).get(0)).find(selectors.scrollDiv).get(0),
                            helper: function () {
                                if (dragObject.isExpanded) {
                                    dragObject.isExpanded(false);
                                }
                                if (dragObject.isBeingDragged) {
                                    dragObject.isBeingDragged(true);
                                }
                                return $(this).clone().width($(this).width()).css({ zIndex: 9999 });
                            },
                            start: function() {
                                $(this).hide();
                            },
                            stop: function(event, ui) {
                                ui.helper.remove();
                                $(this).show();
                                if (dragObject.isBeingDragged) {
                                    dragObject.isBeingDragged(false);

                                }
                            }
                        };
                    if (!dragObject.superDraggable() && dragObject.getSitemap && !dragObject.getSitemap().settings.canDragNode) {
                        return;
                    }
                    $(element).draggable(setup).data("dragObject", dragObject);
                    //$(element).disableSelection(); //commented because it is not possible to put pointer using mouse to URL field.
                }
            };
        }
        
        /**
        * Helper function to add knockout binding 'droppable'.
        */
        function addDroppableBinding() {
            ko.bindingHandlers.droppable = {
                init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
                    var dropZoneObject = viewModel,
                        dropZoneType = valueAccessor(),
                        setup = {
                            tolerance: "pointer",
                            over: function() {
                                dropZoneObject.activeZone(dropZoneType);
                            },
                            out: function() {
                                dropZoneObject.activeZone(DropZoneTypes.None);
                            },
                            drop: function (event, ui) {
                                var forFix = $(ui.draggable).data('draggable'),
                                    dragObject = $(ui.draggable).data("dragObject"),
                                    originalDragObject = dragObject;
                                ui.helper.remove();

                                if (dragObject.parentNode && dragObject.parentNode()) {
                                    dragObject.parentNode().childNodes.remove(dragObject);
                                } else {
                                    // Create node from page link.
                                    var node = new NodeViewModel();
                                    node.title(dragObject.title());
                                    node.url(dragObject.url());
                                    node.pageId(dragObject.pageId());
                                    node.pageTitle(dragObject.title());
                                    node.usePageTitleAsNodeTitle(dragObject.pageId() != null && dragObject.pageId() != defaultIdValue ? true : false);
                                    if (dropZoneType == DropZoneTypes.EmptyListZone || dropZoneType == DropZoneTypes.MiddleZone) {
                                        node.parentNode(dropZoneObject);
                                    } else {
                                        node.parentNode(dropZoneObject.parentNode());
                                    }
                                    if (dragObject.isCustom()) {
                                        node.dropOnCancel = true;
                                        node.startEditSitemapNode();
                                        node.callbackAfterSuccessSaving = function () {
                                            sitemap.activeMapModel.updateNodesOrderAndParent();
                                        };
                                        node.callbackAfterFailSaving = function (newNode) {
                                            newNode.parentNode().childNodes.remove(newNode);
                                            bcms.trigger(events.sitemapNodeRemoved, self);
                                        };
                                    }
                                    if(sitemap.activeMapModel.showLanguages()){
                                        node.updateLanguageOnDropNewNode(dragObject.languageId(), sitemap.activeMapModel.languageId());
                                    }
                                    node.superDraggable(dragObject.superDraggable());
                                    dragObject = node;
                                }
                                
                                dragObject.isBeingDragged(false);
                                dragObject.displayOrder(0);

                                // Add node to tree.
                                var index;
                                if (dropZoneType == DropZoneTypes.EmptyListZone) {
                                    dropZoneObject.childNodes.splice(0, 0, dragObject);
                                }
                                else if (dropZoneType == DropZoneTypes.TopZone) {
                                    index = $.inArray(dropZoneObject, dropZoneObject.parentNode().childNodes());
                                    dropZoneObject.parentNode().childNodes.splice(index, 0, dragObject);
                                }
                                else if (dropZoneType == DropZoneTypes.MiddleZone) {
                                    dropZoneObject.childNodes.splice(0, 0, dragObject);
                                    dropZoneObject.isExpanded(true);
                                }
                                else if (dropZoneType == DropZoneTypes.BottomZone) {
                                    index = $.inArray(dropZoneObject, dropZoneObject.parentNode().childNodes());
                                    dropZoneObject.parentNode().childNodes.splice(index + 1, 0, dragObject);
                                }
                                dropZoneObject.activeZone(DropZoneTypes.None);

                                sitemap.activeMapModel.updateNodesOrderAndParent();

                                // Fix for jQuery drag object.
                                $(ui.draggable).data('draggable', forFix);
                                
                                if (originalDragObject.dropped && $.isFunction(originalDragObject.dropped)) {
                                    originalDragObject.dropped(dragObject);
                                }
                                
                                updateValidation();
                                bcms.trigger(events.sitemapNodeAdded, dragObject);
                            }
                        };
                    if (dropZoneObject.getSitemap && !dropZoneObject.getSitemap().settings.canDropNode) {
                        return;
                    }
                    $(element).droppable(setup);
                }
            };
        }

        /**
        * Helper function to update nodes in list.
        */
        function updateFirstLastNode(nodes) {
            var firstNotDeletedNode = null,
                lastNotDeletedNode = null;
            for (var i = 0; i < nodes.length; i++) {
                var node = nodes[i];
                if (!node.isDeleted() && !node.isBeingDragged()) {
                    if (firstNotDeletedNode == null) {
                        firstNotDeletedNode = node;
                    }
                    lastNotDeletedNode = node;
                }
                node.isFirstNode(false);
                node.isLastNode(false);
            }
            if (firstNotDeletedNode != null) {
                firstNotDeletedNode.isFirstNode(true);
            }
            if (lastNotDeletedNode != null) {
                lastNotDeletedNode.isLastNode(true);
            }
        }
        
        /**
        * Helper function to update validation.
        */
        function updateValidation() {
            var form = $(selectors.sitemapForm);
            bcms.updateFormValidator(form);
        }
        // --------------------------------------------------------------------
        

        // --- View Models ----------------------------------------------------
        /**
        * Responsible for sitemap structure.
        */
        function SitemapViewModel(jsonSitemap) {
            var self = this;
            
            self.id = function () { return jsonSitemap.Id; };
            self.childNodes = ko.observableArray([]);
            self.childNodes.subscribe(updateFirstLastNode);
            self.someNodeIsOver = ko.observable(false);     // Someone is dragging some node over the sitemap, but not over the particular node.
            self.activeZone = ko.observable(DropZoneTypes.None);
            self.showHasNoDataMessage = ko.observable(false);
            self.savingInProgress = false;                  // To prevent multiple saving.

            self.version = jsonSitemap.Version;
            self.title = ko.observable(jsonSitemap.Title);
            self.tags = new tags.TagsListViewModel(jsonSitemap.Tags);
            self.accessControl = security.createUserAccessViewModel(jsonSitemap.UserAccessList);

            self.languageId = ko.observable("");
            self.onLanguageChanged = function (languageId) {
                if (languageId != self.languageId()) {
                    self.languageId(languageId);
                    self.changeLanguageForNodes(self.childNodes());
                    bcms.logger.debug('In sitemap language switched to "' + languageId + '".');
                }
            };
            self.changeLanguageForNodes = function (nodes) {
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i];
                    node.activateTranslation(self.languageId());
                    self.changeLanguageForNodes(node.childNodes());
                }
            };
            self.showMacros = ko.observable(jsonSitemap.ShowMacros);
            self.showLanguages = ko.observable(jsonSitemap.ShowLanguages);
            self.language = jsonSitemap.ShowLanguages ? new LanguageViewModel(jsonSitemap.Languages, null, self.onLanguageChanged) : null,

            self.settings = {
                canEditNode: false,
                canDeleteNode: false,
                canDragNode: false,
                canDropNode: false,
                nodeSaveButtonTitle: globalization.sitemapNodeOkButton,
                nodeSaveAfterUpdate: false
            };

            self.getSitemap = function () {
                return self;
            };
            self.getNoDataMessage = function () {
                if (self.settings.canDeleteNode || self.settings.canDropNode) {
                    return globalization.sitemapPlaceLinkHere;
                }
                return globalization.sitemapIsEmpty;
            };

            // Expanding or collapsing nodes.
            self.expandAll = function () {
                self.expandOrCollapse(self.childNodes(), true);
            };
            self.collapseAll = function () {
                self.expandOrCollapse(self.childNodes(), false);
            };
            self.expandOrCollapse = function (nodes, expand) {
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i];
                    node.isExpanded(expand);
                    self.expandOrCollapse(node.childNodes(), expand);
                }
            };

            self.hasChildNodes = function () {
                // Has at least one not deleted child node.
                var nodes = self.childNodes();
                for (var i = 0; i < nodes.length; i++) {
                    if (!nodes[i].isDeleted()) {
                        return true;
                    }
                }
                return false;
            };
            
            // Updating display order and parent node info.
            self.updateNodesOrderAndParent = function () {
                self.updateNodes(self.childNodes(), self);
            };
            self.updateNodes = function (nodes, parent) {
                if (nodes.length === 0) {
                    return;
                }

                for (var h = 0; h < nodes.length; h++) {
                    self.updateNodes(nodes[h].childNodes(), nodes[h]);
                }
                
                var isFullReorderNeeded = false,
                    zeroNoCount = 0,
                    zeroNoPlace = -1,
                    maxNo = 5000 + nodes.length * 50;
                
                // Analyze if full reordering is needed.
                for (var j = 0; j < nodes.length; j++) {
                    if (j == 0 && nodes[j].displayOrder() > 0 && nodes[j].displayOrder() < 100) {
                        isFullReorderNeeded = true;
                        break;
                    }
                    if (j == 1 && nodes[j - 1].displayOrder() == 0 && nodes[j].displayOrder() < 10) {
                        isFullReorderNeeded = true;
                        break;
                    }
                    if (nodes[j].displayOrder() > maxNo) {
                        isFullReorderNeeded = true;
                        break;
                    }
                    if (nodes[j].displayOrder() == 0) {
                        zeroNoCount++;
                        zeroNoPlace = j;
                        if (zeroNoCount > 1) {
                            isFullReorderNeeded = true;
                            break;
                        }
                    }
                }
                
                if (!isFullReorderNeeded) {
                    if (zeroNoCount == 0) {
                        return;
                    }
                    
                    isFullReorderNeeded = true;
                    var nodeToUpdate = nodes[zeroNoPlace],
                            prevNode = nodes[zeroNoPlace - 1],
                            nextNode = nodes[zeroNoPlace + 1];
                    
                    if (nodeToUpdate != null) {
                        // Is first.
                        if (prevNode == null && nextNode != null) {
                            if (nextNode.displayOrder() - 50 > 0) {
                                self.updateNode(nodeToUpdate, nextNode.displayOrder() - 50, parent);
                                return;
                            }
                        }
                        // Is last.
                        if (prevNode != null && nextNode == null) {
                            if (prevNode.displayOrder() + 50 < 2147483647) {
                                self.updateNode(nodeToUpdate, prevNode.displayOrder() + 50, parent);
                                return;
                            }
                        }
                        // Is in between.
                        if (prevNode != null && nextNode != null) {
                            var middleNo = Math.ceil((nextNode.displayOrder() - prevNode.displayOrder()) / 2) + prevNode.displayOrder();
                            if (prevNode.displayOrder() < middleNo && middleNo < nextNode.displayOrder()) {
                                self.updateNode(nodeToUpdate, middleNo, parent);
                                return;
                            }
                        }
                    }
                }
                
                if (isFullReorderNeeded) {
                    for (var k = 0; k < nodes.length; k++) {
                        var node = nodes[k],
                            no = k == 0 ? 5000 : nodes[k - 1].displayOrder() + 50;
                        self.updateNode(node, no, parent);
                    }
                }
            };
            self.updateNode = function(node, orderNo, parent) {
                var saveIt = false;
                if (node.displayOrder() != orderNo) {
                    saveIt = true;
                    node.displayOrder(orderNo);
                }

                if (node.parentNode() != parent) {
                    saveIt = true;
                    node.parentNode(parent);
                }

                if (saveIt || node.id() == defaultIdValue) {
                    node.saveSitemapNode();
                }
            };

            self.focusOnActiveNode = function(nodes) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].isActive()) {
                        $($('input', '#' + nodes[i].containerId).get(0)).focus();
                        return true;
                    }
                    if (self.focusOnActiveNode(nodes[i].childNodes())) {
                        nodes[i].isExpanded(true);
                        return true;
                    }
                }
                return false;
            };
            self.save = function (onDoneCallback) {
                if (self.focusOnActiveNode(self.childNodes())) {
                    var messagesBox = messages.box({ container: sitemap.activeMessageContainer });
                    messagesBox.clearMessages();
                    messagesBox.addWarningMessage(globalization.sitemapSomeNodesAreInEditingState);
                    return;
                }

                var dataToSend = JSON.stringify(self.getModelToSave()),
                    onSaveCompleted = function (json) {
                        messages.refreshBox(sitemap.activeMessageContainer, json);
                        sitemap.showLoading(false);
                        if (json.Success) {
                            if (onDoneCallback && $.isFunction(onDoneCallback)) {
                                if (json.Data == null) {
                                    json.Data = {
                                        Title: self.title()
                                    };
                                }
                                onDoneCallback(json);
                            }
                        }
                        self.savingInProgress = false;
                    };
                
                if (!self.savingInProgress) {
                    self.savingInProgress = true;
                    $.ajax({
                        url: links.saveSitemapUrl,
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        cache: false,
                        data: dataToSend,
                        beforeSend: function () { sitemap.showLoading(true); }
                    })
                        .done(function (json) {
                            onSaveCompleted(json);
                        })
                        .fail(function (response) {
                            onSaveCompleted(bcms.parseFailedResponse(response));
                        });
                }
            };

            // Parsing / composing.
            self.parseJsonNodes = function (jsonNodes) {
                var nodes = [];
                jsonNodes = jsonNodes || [];
                for (var i = 0; i < jsonNodes.length; i++) {
                    var node = new NodeViewModel();
                    node.fromJson(jsonNodes[i], self.showLanguages);
                    node.parentNode(self);
                    nodes.push(node);
                }
                self.childNodes(nodes);
            };
            self.composeJsonNodes = function () {
                return self.nodesToJson(self.childNodes());
            };
            self.nodesToJson = function(nodes) {
                var result = [];
                for (var i = 0; i < nodes.length; i++) {
                    var nodeToSave = nodes[i].toJson();
                    nodeToSave.ChildNodes = self.nodesToJson(nodes[i].childNodes());
                    result.push(nodeToSave);
                }
                return result;
            };
            self.getModelToSave = function () {
                var tagList = [],
                    tagModels = self.tags.items(),
                    userAccessList = [],
                    accessModels = self.accessControl.UserAccessList(),
                    i;
                
                for (i = 0; i < tagModels.length; i++) {
                    tagList.push(tagModels[i].name());
                }

                for (i = 0; i < accessModels.length; i++) {
                    userAccessList.push({
                        Identity: accessModels[i].Identity(),
                        AccessLevel: accessModels[i].AccessLevel(),
                        IsForRole: accessModels[i].IsForRole(),
                    });
                }

                return {
                    Id: self.id(),
                    Version: self.version,
                    Title: self.title(),
                    RootNodes: self.composeJsonNodes(),
                    Tags: tagList,
                    UserAccessList: userAccessList
                };
            };
        }
        
        /**
        * Responsible for sitemap node data.
        */
        function NodeViewModel() {
            var self = this;
            
            // Data fields.
            self.id = ko.observable(defaultIdValue);
            self.version = ko.observable(0);
            self.title = ko.observable();
            self.macro = ko.observable();
            self.url = ko.observable();
            self.pageId = ko.observable(defaultIdValue);
            self.defaultPageId = ko.observable();
            self.pageTitle = ko.observable();
            self.usePageTitleAsNodeTitle = ko.observable(false);
            self.displayOrder = ko.observable(0);
            self.isDeleted = ko.observable(false);
            self.isDeleted.subscribe(function () {
                if (self.parentNode()) {
                    updateFirstLastNode(self.parentNode().childNodes());
                }
            });
            self.childNodes = ko.observableArray([]);
            self.childNodes.subscribe(updateFirstLastNode);
            self.parentNode = ko.observable();

            self.translationsEnabled = false;
            self.translations = {};
            self.activeTranslation = ko.observable(null);
            
            // For behavior.
            self.isActive = ko.observable(false);           // If TRUE - show edit fields.
            self.isExpanded = ko.observable(false);         // If TRUE - show child nodes.
            self.toggleExpand = function () {
                self.isExpanded(!self.isExpanded());
            };
            self.isBeingDragged = ko.observable(false);     // Someone is dragging the node.
            self.isBeingDragged.subscribe(function () {
                if (self.parentNode()) {
                    updateFirstLastNode(self.parentNode().childNodes());
                }
            });
            self.activeZone = ko.observable(DropZoneTypes.None);
            self.isFirstNode = ko.observable(false);
            self.isLastNode = ko.observable(false);
            self.hasChildNodes = function () {
                // Has at least one not deleted child node.
                var nodes = self.childNodes();
                for (var i = 0; i < nodes.length; i++) {
                    if (!nodes[i].isDeleted()) {
                        return true;
                    }
                }
                return false;
            };
            self.superDraggable = ko.observable(false);     // Used to force dragging if sitemap settings !canDragNode.
            self.isUrlReadonly = ko.computed(function () {
                return self.pageId() == null || self.pageId() === defaultIdValue ? undefined : 'readonly';
            });
            self.getUrlReadonlyState = ko.computed(function () {
                return self.isUrlReadonly();
            });
            self.getNodeHeight = ko.computed(function () {
                if (self.isActive()) {
                    if (self.getSitemap().showMacros) {
                        return self.isUrlReadonly() ? '89px' : '136px';
                    }
                    return self.isUrlReadonly() ? '54px' : '101px';
                }
                return '';
            });
            self.dropOnCancel = false;

            // User for validation.
            self.containerId = 'node-' + nodeId++;
            
            // For search results.
            self.isVisible = ko.observable(true);
            self.isSearchResult = ko.observable(false);
            
            // Data manipulation.
            self.startEditSitemapNode = function () {
                self.titleOldValue = self.title();
                self.urlOldValue = self.url();
                self.macroOldValue = self.macro();
                self.isActive(true);
            };
            self.cancelEditSitemapNode = function () {
                self.isActive(false);
                self.title(self.titleOldValue);
                self.url(self.urlOldValue);
                self.macro(self.macroOldValue);
                if (self.dropOnCancel) {
                    self.parentNode().childNodes.remove(self);
                    bcms.trigger(events.sitemapNodeRemoved, self);
                }
            };
            self.saveSitemapNodeWithValidation = function () {
                self.dropOnCancel = false;
                var inputFields = $('input', '#' + self.containerId);
                if (inputFields.valid()) {
                    if (self.usePageTitleAsNodeTitle() && self.titleOldValue != self.title()) {
                        self.usePageTitleAsNodeTitle(false);
                    }
                    self.saveSitemapNode();
                    inputFields.blur();
                    self.isActive(false);
                }
            };
            self.saveSitemapNode = function () {
                if (self.getSitemap() == null || !self.getSitemap().settings.nodeSaveAfterUpdate) {
                    return;
                }
                
                var params = self.toJson(),
                    onSaveCompleted = function (json) {
                        sitemap.showMessage(json);
                        if (json.Success) {
                            if (json.Data) {
                                self.id(json.Data.Id);
                                self.version(json.Data.Version);
                                if (self.callbackAfterSuccessSaving && $.isFunction(self.callbackAfterSuccessSaving)) {
                                    self.callbackAfterSuccessSaving(self);
                                }
                            }
                        } else {
                            if (self.callbackAfterFailSaving && $.isFunction(self.callbackAfterFailSaving)) {
                                self.callbackAfterFailSaving(self);
                            }
                        }
                        sitemap.showLoading(false);
                    };
                sitemap.showLoading(true);
                $.ajax({
                    url: links.saveSitemapNodeUrl,
                    type: 'POST',
                    dataType: 'json',
                    cache: false,
                    data: params
                })
                    .done(function (json) {
                        onSaveCompleted(json);
                    })
                    .fail(function (response) {
                        onSaveCompleted(bcms.parseFailedResponse(response));
                    });
            };
            self.deleteSitemapNode = function () {
                // Show confirmation dialog.
                var deleting = false,
                    message = $.format(globalization.sitemapDeleteNodeConfirmationMessage, self.title()),
                    confirmDialog = modal.confirm({
                        content: message,
                        onAccept: function () {
                            if (self.getSitemap() == null || !self.getSitemap().settings.nodeSaveAfterUpdate) {
                                self.isDeleted(true);
                                confirmDialog.close();
                                bcms.trigger(events.sitemapNodeRemoved, self);
                                return false;
                            }
                            if (!deleting) {
                                deleting = true;
                                var params = self.toJson(),
                                    onDeleteCompleted = function(json) {
                                        deleting = false;
                                        sitemap.showMessage(json);
                                        try {
                                            if (json.Success) {
                                                self.isDeleted(true);
                                                self.parentNode().childNodes.remove(self);
                                                bcms.trigger(events.sitemapNodeRemoved, self);
                                            }
                                            sitemap.showLoading(false);
                                        } finally {
                                            confirmDialog.close();
                                        }
                                    };
                                sitemap.showLoading(true);
                                $.ajax({
                                    url: links.deleteSitemapNodeUrl,
                                    type: 'POST',
                                    dataType: 'json',
                                    cache: false,
                                    data: params
                                })
                                    .done(function (json) {
                                        onDeleteCompleted(json);
                                    })
                                    .fail(function(response) {
                                        onDeleteCompleted(bcms.parseFailedResponse(response));
                                    });
                            }
                            return false;
                        }
                    });
            };
            self.callbackAfterSuccessSaving = null;
            self.callbackAfterFailSaving = null;

            self.activateTranslation = function (languageId) {
                languageId = languageId == null ? "" : languageId;
                if (!self.translationsEnabled) {
                    return;
                }
                if (self.isActive()) {
                    self.cancelEditSitemapNode();
                }
                if (self.activeTranslation() != null) {
                    var active = self.activeTranslation(),
                        isModified = active.isModified();

                    if (active.title() != self.title()) {
                        active.title(self.title());
                        active.usePageTitleAsNodeTitle(false);
                        isModified = true;
                    }
                    if (active.url() != self.url()) {
                        active.url(self.url());
                        isModified = true;
                    }
                    if (active.macro() != self.macro()) {
                        active.macro(self.macro());
                        isModified = true;
                    }

                    active.isModified(isModified);
                    if (active.languageId() == languageId) {
                        return;
                    }
                }

                if (self.translations[languageId] == null) {
                    self.translations[languageId] = new TranslationViewModel(self, languageId);
                    if (languageId == "") {
                        self.translations[languageId].macro(self.macro());
                        self.translations[languageId].version(self.version());
                        self.translations[languageId].isModified(true);
                    } else {
                        self.translations[languageId].title(self.pageId() != null && !bcms.isEmptyGuid(self.pageId())
                            ? self.translations[""].originalTitle() || self.translations[""].title()
                            : self.translations[languageId].title());
                    }
                }

                self.activeTranslation(self.translations[languageId]);

                self.title(self.translations[languageId].title());
                self.url(self.translations[languageId].url());
                self.version(self.translations[languageId].version());
                self.macro(self.translations[languageId].macro());
            };
            self.updateLanguageOnDropNewNode = function (pageLanguageId, currentLanguageId) {
                sitemap.showLoading(true);
                var isActive = self.isActive();
                self.translationsEnabled = true;
                if (self.pageId() == null || self.pageId() == defaultIdValue) {
                    self.activateTranslation("");
                    self.activateTranslation(currentLanguageId);
                    self.isActive(isActive);
                    sitemap.showLoading(false);
                    return;
                }
                var onSaveCompleted = function(json) {
                        sitemap.showMessage(json);
                        if (json.Success) {
                            if (json.Data != null && json.Data != null) {
                                for (var i = 0; i < json.Data.length; i++) {
                                    var translationJson = json.Data[i],
                                        languageId = translationJson.LanguageId == null ? "" : translationJson.LanguageId,
                                        translation = new TranslationViewModel(self, languageId);
                                    translation.pageId(translationJson.Id);
                                    translation.title(translationJson.Title);
                                    translation.originalTitle(translationJson.Title);
                                    translation.url(translationJson.PageUrl);
                                    translation.usePageTitleAsNodeTitle(self.usePageTitleAsNodeTitle());
                                    translation.isModified(true);
                                    self.translations[languageId] = translation;
                                    if (languageId === "") {
                                        self.title(translationJson.Title);
                                        self.url(translationJson.PageUrl);
                                    }
                                }
                                if (self.translations[""] == null) {
                                    self.activateTranslation("");
                                }
                                self.activateTranslation(currentLanguageId);
                                self.isActive(isActive);
                            }
                            bcms.trigger(events.sitemapNodeTranslationsUpdated);
                        } else {
                            self.isActive(false);
                            self.isDeleted(true);
                        }
                        sitemap.showLoading(false);
                    };
                $.ajax({
                    url: $.format(links.getPageTranslations, self.pageId()),
                    type: 'GET',
                    dataType: 'json',
                    cache: false,
                })
                    .done(function(json) {
                        onSaveCompleted(json);
                    })
                    .fail(function(response) {
                        onSaveCompleted(bcms.parseFailedResponse(response));
                    });

            };

            self.getSitemap = function() {
                if (self.parentNode() != null) {
                    return self.parentNode().getSitemap();
                }
                return null;
            };

            self.fromJson = function(jsonNode, translationsEnabled) {
                self.id(jsonNode.Id);
                self.version(jsonNode.Version);
                self.title(jsonNode.Title);
                self.macro(jsonNode.Macro);
                self.url(jsonNode.Url);
                self.pageId(jsonNode.PageId);
                self.defaultPageId(jsonNode.DefaultPageId);
                self.pageTitle(jsonNode.PageTitle);
                self.usePageTitleAsNodeTitle(jsonNode.UsePageTitleAsNodeTitle);
                self.displayOrder(jsonNode.DisplayOrder);
                if (translationsEnabled) {
                    self.translationsEnabled = true;
                    if (jsonNode.Translations != null) {
                        for (var j = 0; j < jsonNode.Translations.length; j++) {
                            var translationJson = jsonNode.Translations[j],
                                translation = new TranslationViewModel(self, translationJson.LanguageId);
                            translation.id(translationJson.Id);
                            translation.pageId(translationJson.PageId);
                            translation.title(translationJson.Title);
                            translation.usePageTitleAsNodeTitle(translationJson.UsePageTitleAsNodeTitle);
                            translation.url(translationJson.Url);
                            translation.version(translationJson.Version);
                            translation.macro(translationJson.Macro);
                            self.translations[translationJson.LanguageId] = translation;
                        }
                    }
                    self.activateTranslation("");
                }

                var nodes = [];
                if (jsonNode.ChildNodes != null) {
                    for (var i = 0; i < jsonNode.ChildNodes.length; i++) {
                        var node = new NodeViewModel();
                        node.fromJson(jsonNode.ChildNodes[i], translationsEnabled);
                        node.parentNode(self);
                        nodes.push(node);
                    }
                }
                self.childNodes(nodes);
            };
            self.translationsToJson = function () {
                if (!self.translationsEnabled) {
                    return null;
                }
                var result = [];
                for (var i in self.translations) {
                    var translation = self.translations[i];
                    if (translation.isModified() && translation.languageId() != "") {
                        result.push({
                            Id: translation.id(),
                            LanguageId: translation.languageId(),
                            Title: translation.title(),
                            Macro: translation.macro(),
                            UsePageTitleAsNodeTitle: translation.usePageTitleAsNodeTitle(),
                            Url: translation.url(),
                            Version: translation.version(),
                        });
                    }
                }
                return result;
            };
            self.toJson = function () {
                var currentTranslation = null;
                if (self.translationsEnabled) {
                    currentTranslation = self.activeTranslation();
                    self.activateTranslation("");
                }
                var params = {
                    Id: self.id(),
                    Version: self.translationsEnabled ? self.translations[""].version() : self.version(),
                    Title: self.translationsEnabled ? self.translations[""].title() : self.title(),
                    Macro: self.translationsEnabled ? self.translations[""].macro() : self.macro(),
                    UsePageTitleAsNodeTitle: self.usePageTitleAsNodeTitle(),
                    Url: self.translationsEnabled ? self.translations[""].url() : self.url(),
                    PageId: self.pageId(),
                    DisplayOrder: self.displayOrder(),
                    IsDeleted: self.isDeleted(),
                    Translations: self.translationsToJson(),
                    ChildNodes: null
                };
                if (self.translationsEnabled && currentTranslation != null) {
                    self.activateTranslation(currentTranslation.languageId());
                }
                return params;
            };
        }

        /**
        * Language view model.
        */
        function LanguageViewModel(languages, languageId, onLanguageChanged) {
            var self = this,
                i, l;

            self.languageId = ko.observable(languageId);

            self.languages = [];
            self.languages.push({ key: '', value: globalization.invariantLanguage });
            for (i = 0, l = languages.length; i < l; i++) {
                self.languages.push({
                    key: languages[i].Key,
                    value: languages[i].Value
                });
            }
            
            self.languageId.subscribe(function (newValue) {
                if ($.isFunction(onLanguageChanged)) {
                    onLanguageChanged(newValue);
                }
            });

            return self;
        };

        /*
        * Sitemap node translation view model.
        */
        function TranslationViewModel(node, languageId) {
            var self = this;
            self.node = node;
            self.id = ko.observable();
            self.pageId = ko.observable();
            self.macro = ko.observable();
            self.usePageTitleAsNodeTitle = ko.observable(false);
            self.languageId = ko.observable(languageId == null ? "" : languageId);
            var defaultTranslation = node.translations[""];
            if (defaultTranslation != null) {
                self.title = ko.observable(defaultTranslation.title());
                self.url = ko.observable(defaultTranslation.url());
                self.originalTitle = ko.observable(defaultTranslation.title());
            } else {
                self.title = ko.observable(node.title());
                self.url = ko.observable(node.url());
                self.originalTitle = ko.observable(node.title());
            }
            self.version = ko.observable(0);
            self.isModified = ko.observable(false);
        }

        /**
        * Responsible for page searching.
        */
        function SearchPageLinksViewModel(sitemapViewModel, container) {
            var self = this;
            self.searchQuery = ko.observable("");
            self.pageLinks = ko.observableArray([]);
            self.sitemap = sitemapViewModel;
            self.hasfocus = ko.observable(false);

            container.find(selectors.secondTabButton).on('click', function () {
                self.hasfocus(true);
            });

            self.searchForPageLinks = function () {
                var showAll = $.trim(self.searchQuery()).length === 0;
                var pageLinks = self.pageLinks();
                for (var i = 0; i < pageLinks.length; i++) {
                    if (showAll) {
                        pageLinks[i].isVisible(true);
                    } else {
                        var link = pageLinks[i],
                            searchQuery = self.searchQuery().toLowerCase(),
                            title = link.title().toLowerCase(),
                            url = link.url().toLowerCase();
                        link.isVisible(title.indexOf(searchQuery) !== -1 || url.indexOf(searchQuery) !== -1);
                    }
                }
                
                // IE8 fix
                var val = self.searchQuery();
                self.hasfocus(true);
                self.searchQuery("");
                self.searchQuery(val);
            };
            
            // Parse.
            self.parseJsonLinks = function (jsonLinks) {
                var pageLinks = [],
                    customLink = new PageLinkViewModel();
                
                customLink.title(globalization.sitemapEditorDialogCustomLinkTitle);
                customLink.url('/../');
                customLink.pageId(defaultIdValue);
                customLink.isCustom(true);
                pageLinks.push(customLink);
                
                for (var i = 0; i < jsonLinks.length; i++) {
                    var link = new PageLinkViewModel();
                    link.fromJson(jsonLinks[i]);
                    pageLinks.push(link);
                }
                self.pageLinks(pageLinks);
                self.updateStatusOfLinks();
            };
            
            self.title = sitemapViewModel.title;
            self.tags = sitemapViewModel.tags;
            self.accessControl = sitemapViewModel.accessControl;

            self.updateStatusOfLinks = function () {
                var pagesInSitemap = [],
                    pagesInSitemapSoftLink =[],
                    getAllPages = function (nodes) {
                        var i, k, j, translation, translationId, translationUrl;

                        for (i = 0; i < nodes.length; i++) {
                            if (!nodes[i].isDeleted()) {
                                var pageId = nodes[i].defaultPageId() || nodes[i].pageId();
                                if (pageId != null && pageId != defaultIdValue) {
                                    pagesInSitemap[pageId] = true;
                                    if (nodes[i].translations != null) {
                                        for (k in nodes[i].translations) {
                                            translation = nodes[i].translations[k];

                                            if (translation && translation.pageId && $.isFunction(translation.pageId)) {
                                                translationId = translation.pageId();
                                                if (translationId) {
                                                    pagesInSitemap[translationId] = true;
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    var url = nodes[i].url();
                                    if (url) {
                                        pagesInSitemapSoftLink[url] = true;
                                        if (nodes[i].translations != null) {
                                            for (j in nodes[i].translations) {
                                                translation = nodes[i].translations[j];
                                                if (translation && translation.url && $.isFunction(translation.url)) {
                                                    translationUrl = translation.url();
                                                    if (translationUrl) {
                                                        pagesInSitemapSoftLink[translationUrl] = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            getAllPages(nodes[i].childNodes());
                        }
                    };
                getAllPages(self.sitemap.childNodes());
                var pageLinks = self.pageLinks();
                for (var j = 0; j < pageLinks.length; j++) {
                    var link = pageLinks[j],
                        onSitemap = pagesInSitemap[link.pageId()] === true || pagesInSitemapSoftLink[link.url()] === true;
                    if (link.isOnSitemap() != onSitemap) {
                        link.isOnSitemap(onSitemap);
                    }
                }
            };
            
            bcms.on(events.sitemapNodeAdded, self.updateStatusOfLinks);
            bcms.on(events.sitemapNodeRemoved, self.updateStatusOfLinks);
            bcms.on(events.sitemapNodeTranslationsUpdated, self.updateStatusOfLinks);
        }
        
        /**
        * Responsible for page link data.
        */
        function PageLinkViewModel(title, url, pageId, languageId) {
            var self = this;
            self.title = ko.observable(title);
            self.url = ko.observable(url);
            self.pageId = ko.observable(pageId);
            self.languageId = ko.observable(languageId);
            self.isVisible = ko.observable(true);
            self.isCustom = ko.observable(false);
            self.isBeingDragged = ko.observable(false);
            self.superDraggable = ko.observable(false); // Used to force dragging if sitemap settings !canDragNode.
            self.isOnSitemap = ko.observable(false);

            self.onDrop = null;
            self.dropped = function (droppedSitemapNode) {
                if(self.onDrop && $.isFunction(self.onDrop)) {
                    self.onDrop(droppedSitemapNode);
                }
            };
            self.fromJson = function (json) {
                self.isVisible(true);
                self.title(json.Title);
                self.url(json.Url);
                self.pageId(json.Id);
                self.languageId(json.LanguageId);
            };
        }
        
        /**
        * Responsible for new page data and sitemap.
        */
        function AddNewPageViewModel(sitemapViewModel, pageLinkViewModel, onSkip) {
            var self = this;
            self.pageLink = pageLinkViewModel;
            self.pageLink.superDraggable(true);
            self.sitemap = sitemapViewModel;
            self.onSkipClick = onSkip;
            self.linkIsDropped = ko.observable(false);
            self.sitemapNode = null;

            self.pageLink.onDrop = function (droppedSitemapNode) {
                self.linkIsDropped(true);
                self.pageLink.isVisible(false);
                self.sitemapNode = droppedSitemapNode;
            };

            self.skipClicked = function () {
                if (self.onSkipClick && $.isFunction(self.onSkipClick)) {
                    self.onSkipClick();
                }
            };
            self.undoClicked = function () {
                if (self.sitemapNode != null) {
                    // Remove newly created node from sitemap.
                    self.sitemapNode.parentNode().childNodes.remove(self.sitemapNode);
                    self.sitemapNode = null;
                }
                self.pageLink.isVisible(true);
                self.linkIsDropped(false);
            };
        }
        
        /**
        * View model for controling horizontal slider
        */
        function TabsSliderViewModel(container, parent) {
            
            var self = this,
                sliderContainer = container.find(selectors.tabsSlider),
                leftSlider = sliderContainer.find(selectors.tabsSliderLeftArrow),
                rightSlider = sliderContainer.find(selectors.tabsSliderRightArrow),
                items = parent.tabs(),
                currentItem = 0,
                maxWidth = 0,
                maxItem = 0;

            self.canSlideLeft = ko.observable(false);
            self.canSlideRight = ko.observable(false);
            self.showSliders = ko.observable(true);

            self.slideLeft = function () {
                if (self.canSlideLeft()) {
                    slide(-1);
                }
            };
            self.slideRight = function () {
                if (self.canSlideRight()) {
                    slide(1);
                }
            };

            self.initialize = function () {
                var sum = 0;
                
                maxWidth = sliderContainer.width() - leftSlider.outerWidth(true) - rightSlider.outerWidth(true) - 20;

                for (var i = 0; i < items.length; i++) {
                    items[i].width = sliderContainer.find('#' + items[i].tabId).outerWidth(true);
                    sum += items[i].width;
                }

                if (sum < maxWidth) {
                    self.showSliders(false);
                }

                slide(0);
            };

            function slide(step) {
                var length = items.length,
                    sum = 0,
                    activeItem = 0,
                    visible,
                    i;

                currentItem += step;
                for (i = 0; i < length; i++) {
                    visible = true;
                        
                    if (items[i].isActive()) {
                        activeItem = i;
                    }
                    if (i < currentItem) {
                        visible = false;
                    } else {
                        sum += items[i].width;
                        if (sum > maxWidth) {
                            visible = false;
                        } else {
                            maxItem = i;
                        }
                    }
                        
                    items[i].isVisible(visible);
                }
                
                if (activeItem < currentItem || activeItem > maxItem) {
                    if (step > 0) {
                        items[currentItem].activate();
                    } else {
                        items[maxItem].activate();
                    }
                }

                self.canSlideLeft(currentItem > 0);
                self.canSlideRight(maxItem < items.length - 1);
            }

            return self;
        }

        /**
        * Responsible for tabs handling for new page placement into multiple sitemaps.
        */
        function TabsModel(tabsArray, container) {
            var self = this;
            self.activeTab = ko.observable();
            self.activeNewPageViewModel = ko.observable();

            self.activateTab = function (tabModel) {
                var active = self.activeTab();
                if (active) {
                    active.isActive(false);
                }
                tabModel.isActive(true);
                sitemap.activeMapModel = tabModel.newPageViewModel.sitemap;
                self.activeTab(tabModel);
                self.activeNewPageViewModel(tabModel.newPageViewModel);
            };

            for (var i = 0; i < tabsArray.length; i++) {
                tabsArray[i].activateTab = self.activateTab;
            }
            
            self.tabs = ko.observableArray(tabsArray);

            // Activate first tab.
            self.tabs()[0].activate();
            
            self.slider = new TabsSliderViewModel(container, self);
        }
        
        /**
        * Responsible for single tab behavior for new page placement into sitemap.
        */
        function TabModel(newPageViewModel) {
            var self = this;
            self.newPageViewModel = newPageViewModel;
            self.isActive = ko.observable(false);
            self.activateTab = null;
            self.width = 0;
            self.isVisible = ko.observable(false);
            self.activate = function() {
                self.activateTab(self);
            };
            self.tabId = 'bms-sitemaps-tab-' + tabId;
            tabId++;
        }

        /**
        * Responsible for sitemap preview.
        */
        function VersionViewModel(jsonSitemap) {
            var self = this;
            self.sitemap = new SitemapViewModel(jsonSitemap);
            self.sitemap.parseJsonNodes(jsonSitemap.RootNodes);

            // Setup settings.
            self.sitemap.settings.canEditNode = false;
            self.sitemap.settings.canDeleteNode = false;
            self.sitemap.settings.canDragNode = false;
            self.sitemap.settings.canDropNode = false;
            self.sitemap.settings.nodeSaveAfterUpdate = false;
        }
        // --------------------------------------------------------------------

        /**
        * Initializes sitemap history dialog events.
        */
        function initSitemapHistoryDialogEvents(dialog, afterSitemapRestored) {
            dialog.maximizeHeight();

            var container = dialog.container.find(selectors.modalContent);

            container.find(selectors.gridRestoreLinks).on('click', function (event) {
                bcms.stopEventPropagation(event);
                restoreVersion(dialog, $(this).data('id'), function (json) {
                    if ($.isFunction(afterSitemapRestored)) {
                        afterSitemapRestored(json);
                    }
                    searchSitemapHistory(dialog, container, form, afterSitemapRestored);
                });
            });

            container.find(selectors.gridCells).on('click', function () {
                var self = $(this),
                    row = self.parents(selectors.firstRow),
                    previewLink = row.find(selectors.gridRowPreviewLink),
                    id = previewLink.data('id');

                container.find(selectors.gridRows).removeClass(classes.tableActiveRow);
                row.addClass(classes.tableActiveRow);
                previewVersion(dialog, id);
            });

            var form = container.find(selectors.sitemapHistoryForm);
            grid.bindGridForm(form, function (data) {
                container.html(data);
                initSitemapHistoryDialogEvents(dialog, afterSitemapRestored);
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSitemapHistory(dialog, container, form, afterSitemapRestored);
                return false;
            });

            form.find(selectors.sitemapHistorySearchButton).on('click', function () {
                searchSitemapHistory(dialog, container, form, afterSitemapRestored);
            });
        };

        /**
        * Preview specified sitemap version.
        */
        function previewVersion(dialog, id) {
            var url = $.format(links.loadSitemapVersionPreviewUrl, id),
                previewContainer = dialog.container.find(selectors.versionPreviewContainer),
                loaderContainer = dialog.container.find(selectors.versionPreviewLoaderContainer),
                onSuccess = function (html, json) {
                    previewContainer.html(html);
                    if (json && json.Success) {
                        initializeSitemapPreview(previewContainer, json.Data);
                    }
                    loaderContainer.hideLoading();
                };
            loaderContainer.showLoading();
            $.ajax({
                type: 'GET',
                cache: false,
                url: url,
                error: function () {
                    loaderContainer.hideLoading();
                },
                success: function (data, status, response) {
                    if (response.getResponseHeader('Content-Type').indexOf('application/json') === 0 && data.Html) {
                        messages.refreshBox(dialog.container, data);
                        onSuccess(data.Html, data);
                    } else {
                        onSuccess(data, null);
                    }
                }
            });
        }

        function initializeSitemapPreview(container, json) {
            if (json) {
                var context = container.get(0),
                    model = new VersionViewModel(json);
                if (context) {
                    ko.applyBindings(model, context);
                }
            }
        }

        /**
        * Restores specified version from history.
        */
        function restoreVersion(dialog, id, afterSitemapRestored) {
            var submitRestoreIt = function (isConfirmed) {
                var url = $.format(links.restoreSitemapVersionUrl, id, isConfirmed),
                    onComplete = function (json) {
                        dialog.container.hideLoading();
                        messages.refreshBox(dialog.container, json);
                        if (json.Success) {
                            afterSitemapRestored(json);
                        } else {
                            if (json.Data && json.Data.ConfirmationMessage) {
                                modal.confirm({
                                    content: json.Data.ConfirmationMessage,
                                    onAccept: function () {
                                        submitRestoreIt(1);
                                    }
                                });
                            }
                        }
                    };
                dialog.container.showLoading();
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
            };

            modal.confirm({
                content: globalization.sitemapVersionRestoreConfirmation,
                acceptTitle: globalization.restoreButtonTitle,
                onAccept: function () {
                    submitRestoreIt(0);
                }
            });
        }

        /**
        * Posts sitemap history form with search query.
        */
        function searchSitemapHistory(dialog, container, form, afterSitemapRestored) {
            grid.submitGridForm(form, function (data) {
                container.html(data);
                initSitemapHistoryDialogEvents(dialog, data, afterSitemapRestored);
            });
        }

        /**
        * Initializes module.
        */
        sitemap.init = function() {
            bcms.logger.debug('Initializing bcms.pages.sitemap module.');
            
            // Bindings for sitemap nodes Drag'n'Drop.
            addDraggableBinding();
            addDroppableBinding();
            
            // Subscribe to events.
            bcms.on(bcms.events.pageCreated, sitemap.loadAddNewPageDialog);
        };
    
        /**
        * Register initialization.
        */
        bcms.registerInit(sitemap.init);
    
        return sitemap;
});
