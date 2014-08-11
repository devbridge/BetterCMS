// TODO: remove after tests
window.cms = {};

/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.content', ['bcms.jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var content = {},

        // Selectors used in the module to locate DOM elements:
        selectors = {
            contentOverlay: '#bcms-content-overlay',
            contentDelete: '.bcms-content-delete',
            contentEdit: '.bcms-content-edit',
            contentEditInnerDiv: '.bcms-content-edit .bcms-content-icon',
            contentHistory: '.bcms-content-history',
            contentConfigure: '.bcms-content-configure',
            enterChildContent: '.bcms-enter-child-content',

            regionsAndContents: '.bcms-region-start, .bcms-region-end, .bcms-content-start, .bcms-content-end',
            regionOverlay: '#bcms-region-overlay-template',
            
            regionAddContentButtons: '.bcms-region-addcontent',
            regionSortButtons: '.bcms-region-sortcontent',
            regionSortDoneButtons: '.bcms-region-sortdone',
            regionButtons: '.bcms-region-button',
            regionActions: '.bcms-region-actions',
            regionSortWrappers: '.bcms-sort-wrapper',
            regionSortBlock: '.bcms-sorting-block',
            regionLeaveChildContentButtons: '.bcms-leave-child-content',
            
            masterPagesPathContainer: '.bcms-layout-path',
            masterPagesPathHandler: '.bcms-layout-path-handle',
            masterPagesPathItem: '.bcms-layout-path-item',
            masterPagesPathInnerContainer: '.bcms-layout-path-inner',
            masterPagesPathSliderLeft: '.bcms-path-arrow-left',
            masterPagesPathSliderRight: '.bcms-path-arrow-right'
        },
        classes = {
            regionStart: 'bcms-region-start',
            regionEnd: 'bcms-region-end',
            contentStart: 'bcms-content-start',
            contentEnd: 'bcms-content-end',
            regionSortOverlay: 'bcms-show-overlay',
            masterPagesPathToggler: 'bcms-path-toggler',
            masterPagesPathInactiveArrow: 'bcms-path-arrow-inactive',
            masterPagesPathItem: 'bcms-layout-path-item'
        },
        keys = {
            showMasterPagesPath: 'bcms.showMasterPagesPath',
        },
        resizeTimer,
        currentContentDom,
        regionRectangles = $(),
        contentRectangles = $(),
        links = {},
        globalization = {
            showMasterPagesPath: null,
            hideMasterPagesPath: null,
            // TODO: remove globalization from all the levels
            currentPage: null
        },
        pageViewModel,
        opacityAnimationSpeed = 50,
        isSortMode = false,
        masterPagesModel = null;

    // Assign objects to module
    content.selectors = selectors;
    content.links = links;
    content.globalization = globalization;

    /**
    * Shows overlay over content region:
    */
    content.showOverlay = function (contentViewModel) {
        var overlay = contentViewModel.overlay;
        
        overlay.animate({ 'opacity': 1 }, opacityAnimationSpeed);
    };
    
    /**
    * Creates overlay for content:
    */
    content.createContentOverlay = function (contentViewModel) {
        var container = $(selectors.contentOverlay),
            template = container.html(),
            rectangle = $(template);

        rectangle.data('target', contentViewModel);
        rectangle.insertBefore(container);
        contentRectangles = contentRectangles.add(rectangle);

        if (bcms.editModeIsOn()) {
            rectangle.show();
        }
        
        contentViewModel.overlay = rectangle;
        
        if (contentViewModel.getChildRegions().length == 0) {
            contentViewModel.removeEnterChildContentButton();
        }

        bcms.trigger(bcms.events.createContentOverlay, contentViewModel);
    };

    /**
    * Forces each content outline to update it's position:
    */
    content.refreshContentsPosition = function () {

        if (!bcms.editModeIsOn() || pageViewModel == null) {
            return;
        }

        $.each(pageViewModel.contents, function () {
            if (!this.isContentVisible()) {
                this.overlay.hide();

                return;
            } else {
                this.overlay.show();
            }

            this.recalculatePositions();
        });

        contentRectangles.each(function () {

            var rectangle = $(this),
                contentViewModel = rectangle.data('target'),
                width = contentViewModel.width + 'px',
                height = contentViewModel.height,
                top = contentViewModel.top + 'px',
                left = contentViewModel.left + 'px';

            if (height < 20) {
                height = 20;
            }

            rectangle.css({
                'width': width,
                'height': height + 'px',
                'top': top,
                'left': left,
                'opacity': 0
            });
        });
    };

    /**
    * Forces each region outline to update it's position:
    */
    content.refreshRegionsPosition = function (regionViewModels) {
        
        if (!bcms.editModeIsOn() || pageViewModel == null) {
            return;
        }

        regionViewModels = regionViewModels || pageViewModel.regions;

        $.each(regionViewModels, function () {
            if (!this.isRegionVisible()) {
                this.overlay.hide();

                return;
            } else {
                this.overlay.show();
            }

            this.recalculatePositions();
        });

        $.each(regionViewModels, function () {
            if (!this.isRegionVisible()) {
                return;
            }

            var regionViewModel = this,
                overlay = regionViewModel.overlay,
                width = regionViewModel.width,
                height = regionViewModel.height;

            overlay.css({
                top: regionViewModel.top + 'px',
                left: regionViewModel.left + 'px',
                width: width + 'px',
                height: height + 'px'
            });
        });
    };

    /**
    * Initializes events for content overlay:
    */
    content.initOverlayEvents = function (contentViewModel) {

        var overlay = contentViewModel.overlay;

        $(selectors.contentDelete, overlay).on('click', function () {
            contentViewModel.onDeleteContent();
        });

        $(selectors.contentEdit, overlay).on('click', function () {
            contentViewModel.onEditContent();
        });

        $(selectors.contentHistory, overlay).on('click', function () {
            contentViewModel.onContentHistory();
        });

        $(selectors.contentConfigure, overlay).on('click', function () {
            contentViewModel.onConfigureContent();
        });

        $(selectors.enterChildContent, overlay).on('click', function () {
            contentViewModel.enterChildContent();
        });

        overlay.on('mouseleave', function () {
            bcms.logger.trace('Content mouse leave');
            content.hideOverlay(contentViewModel);
        });

        overlay.on('mouseover', function () {
            if (!bcms.editModeIsOn() || currentContentDom === overlay) {
                bcms.logger.trace('Exit content mouse over');
                return;
            }

            bcms.logger.trace('Content mouse over');
            currentContentDom = overlay;
            content.showOverlay(contentViewModel);
        });
    };

    /**
    * Hides content overlay:
    */
    content.hideOverlay = function (contentViewModel) {
        currentContentDom = null;

        contentViewModel.overlay.animate({ 'opacity': 0 }, opacityAnimationSpeed);
    };

    /**
    * Saves content sorting and other changes
    */
    function saveContentChanges(regionViewModels) {
        var models = [],
            i, l, regionViewModel;

        for (i = 0, l = regionViewModels.length; i < l; i++) {
            regionViewModel = regionViewModels[i];
            
            $.each(regionViewModel.contents, function () {
                models.push({
                    'RegionId': regionViewModel.id,
                    'PageContentId': this.pageContentId,
                    'Version': this.pageContentVersion
                });
            });
        }

        bcms.trigger(bcms.events.sortPageContent, models);
    }

    /**
    * Checks if contents order has changed
    */
    function hasContentsOrderChanged(before, after) {
        if (before.length != after.length) {
            return true;
        }

        for (var i = 0; i < before.length; i ++) {
            if (!after[i] || after[i].contentId != before[i].contentId) {
                return true;
            }
        }

        return false;
    }

    /**
    * Turns region content sorting mode OFF:
    */
    content.turnSortModeOff = function (cancel) {

        var changedRegions = [];

        $.each(pageViewModel.regions, function () {
            if (!this.isRegionVisible()) {
                return;
            }

            var regionContents = [],
                regionViewModel = this;

            $(selectors.regionActions, regionViewModel.overlay).show();
            $(selectors.regionButtons, regionViewModel.overlay).show();
            $(selectors.regionSortDoneButtons, regionViewModel.overlay).hide();

            if (isSortMode) {
                regionViewModel.sortBlock.sortable('destroy');
            }
            regionViewModel.overlay.removeClass(classes.regionSortOverlay);

            $(selectors.regionSortWrappers, regionViewModel.overlay).each(function () {
                var viewModel = $(this).data('target');

                regionContents.push(viewModel);

                $(this).remove();
            });

            if (!cancel) {
                if (hasContentsOrderChanged(regionViewModel.contents, regionContents)) {
                    changedRegions.push(regionViewModel);
                }
                regionViewModel.contents = regionContents;
            }
            
            $.each(regionContents, function () {
                this.overlay.show();
            });
        });

        content.refreshRegionsPosition();
        content.refreshContentsPosition();

        isSortMode = false;

        return changedRegions;
    };

    /**
    * Turns region content sorting mode ON:
    */
    content.turnSortModeOn = function (currentRegionViewModel) {
        isSortMode = true;
        
        $.each(pageViewModel.regions, function () {
            if (!this.isRegionVisible()) {
                return;
            }

            var regionViewModel = this;
            
            if (regionViewModel != currentRegionViewModel) {
                $(selectors.regionActions, regionViewModel.overlay).hide();
            } else {
                $(selectors.regionButtons, regionViewModel.overlay).hide();
                $(selectors.regionSortDoneButtons, regionViewModel.overlay).show();
            }

            $(regionViewModel.contents).each(function () {
                var sortWrapper = $('<div class="bcms-sort-wrapper" />');

                $('<div class="bcms-sort-content" />').html(this.title).appendTo(sortWrapper);
                sortWrapper.append('<div class="bcms-sort-overlay bcms-content-overlaybg" />');

                // Store reference to content so it can be sorted later:
                sortWrapper.data('target', this);

                this.overlay.hide();

                regionViewModel.sortBlock.append(sortWrapper);
            });

            regionViewModel.sortBlock.sortable({
                connectWith: '.bcms-sorting-block',
                dropOnEmpty: true,
                placeholder: "bcms-sort-wrapper-placeholder",
                tolerance: "intersect"
            });

            regionViewModel.overlay.addClass(classes.regionSortOverlay);
        });
        
        content.refreshRegionsPosition();
    };

    /**
    * Function calculates top, left positions and width and height for specified list of DOM elements
    */
    function calculatePositions(start, end) {
        var $start = $(start),
            $end = $(end),
            startOffset = $start.offset() || {},
            endOffset = $end.offset() || {},
            endWidth = $end.outerWidth(true),
            endHeight = $end.outerHeight(true),
            top = startOffset.top,
            left = startOffset.left,
            right = endOffset.left + endWidth,
            bottom = endOffset.top + endHeight;

        return {
            left: left,
            top: top,
            width: right - left,
            height: bottom - top
        };
    }

    /**
    * Page view model
    */
    function PageViewModel() {
        var self = this;

        self.regions = [];
        self.contents = [];
    }

    /**
    * Page region view model
    */
    function RegionViewModel(regionStart, regionEnd, regionContents, parentRegionId, parentPageContentId) {
        var self = this;

        self.id = regionStart.data('id');
        self.regionStart = regionStart;
        self.regionEnd = regionEnd;
        self.contents = regionContents;
        self.overlay = null;
        self.sortBlock = null;
        self.parentRegionId = parentRegionId;
        self.parentRegion = null;
        self.parentPageContentId = parentPageContentId;
        self.parentContent = null;
        self.isCurrent = false;
        self.childRegions = null;
        self.allContents = null;
        
        self.left = 0;
        self.top = 0;
        self.width = 0;
        self.height = 0;

        self.recalculatePositions = function() {
            var positions = calculatePositions(self.regionStart, self.regionEnd);
            
            self.left = positions.left;
            self.top = positions.top;
            self.width = positions.width;
            self.height = positions.height;
            
            if (isSortMode) {
                for (var i = 0; i < self.contents.length; i ++) {
                    var sortPositions = calculatePositions(self.regionStart, self.contents[i].contentEnd);
                    if (sortPositions.height > self.height) {
                        self.height = sortPositions.height;
                    }
                }
            }
        };

        self.initializeRegion = function () {

            var i,
                container = $(selectors.regionOverlay),
                template = container.html(),
                rectangle = $(template);

            rectangle.data('target', self);
            rectangle.insertBefore(container);
            regionRectangles = regionRectangles.add(rectangle);

            self.overlay = rectangle;
            self.sortBlock = self.overlay.find(selectors.regionSortBlock);

            $(selectors.regionAddContentButtons, self.overlay).on('click', function () {
                bcms.trigger(bcms.events.addPageContent, self);
            });

            $(selectors.regionSortButtons, self.overlay).on('click', function () {
                content.turnSortModeOn(self);
            });

            $(selectors.regionSortDoneButtons, self.overlay).on('click', function () {
                var changedRegions = content.turnSortModeOff();

                if (changedRegions.length > 0) {
                    saveContentChanges(changedRegions);
                }
            });

            if (self.parentRegionId && !self.parentRegion) {
                for (i = 0; i < pageViewModel.regions.length; i++) {
                    if (pageViewModel.regions[i].id == self.parentRegionId) {
                        self.parentRegion = pageViewModel.regions[i];

                        break;
                    }
                }
            }

            if (self.parentPageContentId && !self.parentContent) {
                for (i = 0; i < pageViewModel.contents.length; i++) {
                    if (pageViewModel.contents[i].pageContentId == self.parentPageContentId) {
                        self.parentContent = pageViewModel.contents[i];
                        break;
                    }
                }
            }

            if (self.parentRegion) {
                var regionActions = $(selectors.regionActions, self.overlay),
                    actionsWidht = $(selectors.regionActions, self.overlay).width(),
                    leaveContent = $(selectors.regionLeaveChildContentButtons, self.overlay);

                regionActions.css('width', (actionsWidht + 35) + 'px');
                leaveContent.show();

                leaveContent.on('click', function() {
                    self.leaveChildContent(false);
                });
            }

            if (bcms.editModeIsOn()) {
                rectangle.show();
            }
        };

        self.getChildRegions = function() {
            if (self.childRegions == null) {
                var i, j;

                self.childRegions = [];

                for (i = 0; i < pageViewModel.contents.length; i++) {
                    if (pageViewModel.contents[i].region == self) {
                        //console.log("Region:", self);
                        //console.log(self.childRegions);

                        var childRegions = pageViewModel.contents[i].getChildRegions();
                        for (j = 0; j < childRegions.length; j++) {
                            self.childRegions.push(childRegions[j]);
                        }
                    }
                }
            }

            return self.childRegions;
        };

        self.getAllChildContents = function () {
            if (self.allContents == null) {
                self.allContents = [];

                var i;

                for (i = 0; i < pageViewModel.contents.length; i ++) {
                    if (pageViewModel.contents[i].region == self) {
                        self.allContents.push(pageViewModel.contents[i]);
                    }
                }
            }

            return self.allContents;
        };

        self.isRegionVisible = function () {
            return self.isCurrent;
        };

        self.enterChildContent = function() {
            var childRegions = self.getChildRegions(),
                i;

            self.isCurrent = false;

            for (i = 0; i < childRegions.length; i++) {
                console.log('Entering regions: ');
                console.log(childRegions[i]);
                childRegions[i].isCurrent = true;
            }

            content.refreshRegionsPosition();
            content.refreshContentsPosition();
        };

        self.leaveChildContent = function (/*isChild*/) {
            var i, j;

            // Set region and it's contents as invisible
            self.isCurrent = false;
            self.parentContent.isCurrent = true;

            var childContents = self.getAllChildContents();
            for (i = 0; i < childContents.length; i++) {
                childContents[i].isCurrent = false;
            }

            self.parentContent.region.setVisibility();

//            var childRegions = self.parentRegion.getChildRegions();
//            for (i = 0; i < childRegions.length; i++) {
//                childRegions[i].isCurrent = false;
//
//                var childChildRegions = childRegions[i].getChildRegions();
//                for (j = 0; j < childChildRegions.length; j++) {
//                    childChildRegions[j].leaveChildContent(true);
//                }
//            }
//
//            if (isChild === false) {
//                self.parentRegion.isCurrent = true;
//            }

            content.refreshRegionsPosition();
            content.refreshContentsPosition();
        };

        self.setVisibility = function() {
            var allContents = self.getAllChildContents(),
                i,
                isCurrent = 0;

            for (i = 0; i < allContents.length; i++) {
                if (allContents[i].isCurrent) {
                    isCurrent++;
                } else {
                    break;
                }
            }

            self.isCurrent = (allContents.length == isCurrent);
        };
    }

    /**
    * Page content view model
    */
    function ContentViewModel(contentStart, contentEnd, parentPageContentId) {
        var self = this;

        self.contentStart = contentStart;
        self.contentEnd = contentEnd;
        self.overlay = null;
        self.region = null;
        self.parentPageContentId = parentPageContentId;
        self.parentContent = null;
        self.childRegions = null;
        self.hideEndingDiv = contentEnd.data('hide') === true;
        self.isCurrent = false;

        self.contentId = contentStart.data('contentId');
        self.pageContentId = contentStart.data('pageContentId');
        self.contentVersion = contentStart.data('contentVersion');
        self.pageContentVersion = contentStart.data('pageContentVersion');
        self.contentType = contentStart.data('contentType');
        self.draft = contentStart.data('draft');
        self.title = contentStart.data('contentTitle');

        self.left = 0;
        self.top = 0;
        self.width = 0;
        self.height = 0;

        self.recalculatePositions = function () {
            var positions = calculatePositions(self.contentStart, self.contentEnd);
            
            self.left = positions.left + 1;
            self.top = positions.top + 1;
            self.width = positions.width ;
            self.height = positions.height;
        };

        self.onEditContent = function() {};
        self.onDeleteContent = function() {};
        self.onConfigureContent = function() {};
        self.onContentHistory = function() {};

        self.enterChildContent = function () {
            self.region.enterChildContent();
        };

        self.removeHistoryButton = function () {
            self.overlay.find(selectors.contentHistory).remove();
        };

        self.removeEditButton = function () {
            self.overlay.find(selectors.contentEdit).remove();
        };

        self.removeConfigureButton = function () {
            self.overlay.find(selectors.contentConfigure).remove();
        };

        self.removeEnterChildContentButton = function () {
            self.overlay.find(selectors.enterChildContent).remove();
        };

        self.removeDeleteButton = function () {
            self.overlay.find(selectors.contentDelete).remove();
        };

        self.addDraftIcon = function () {
            self.overlay.find(selectors.contentEditInnerDiv).html('<div>*</div>');
        };

        self.initializeContent = function () {
            if (self.parentPageContentId && !self.parentContent) {
                for (var i = 0; i < pageViewModel.contents.length; i++) {
                    if (pageViewModel.contents[i].pageContentId == self.parentPageContentId) {
                        self.parentContent = pageViewModel.contents[i];
                        break;
                    }
                }
            }
        };

        self.getChildRegions = function () {
            if (self.childRegions == null) {
                self.childRegions = [];

                for (var i = 0; i < pageViewModel.regions.length; i++) {
                    if (pageViewModel.regions[i].parentContent == self) {
                        self.childRegions.push(pageViewModel.regions[i]);
                    }
                }
            }

            return self.childRegions;
        };

        self.isContentVisible = function () {
            return self.isCurrent;
        };

        return self;
    }

    function collectRegionsAndContents(tags) {
        var tagsCount = tags.length,
            currentTag,
            i,
            allContents = [],
            allRegions = [],
            regionId,
            pageContentId,
            currentContent,
            currentRegion,
            parentRegionId,
            parentPageContentId;

        for (i = 0; i < tagsCount; i++) {
            currentTag = $(tags[i]);

            if (currentTag.hasClass(classes.regionStart)) {
                // Region start
                regionId = currentTag.data('id');
                currentRegion = {
                    regionId: regionId,
                    contents: [],
                    startTag: currentTag
                };
                allRegions.push(currentRegion);
            } else if (currentTag.hasClass(classes.regionEnd)) {
                // Region end
                currentRegion = allRegions.pop();
                parentRegionId = null;
                parentPageContentId = null;
                if (allRegions.length > 0) {
                    parentRegionId = allRegions[allRegions.length - 1].regionId;
                    parentPageContentId = allContents[allContents.length - 1].pageContentId;
                }

                var regionViewModel = new RegionViewModel(currentRegion.startTag, currentTag, currentRegion.contents, parentRegionId, parentPageContentId);
                pageViewModel.regions.push(regionViewModel);
                
                $.each(currentRegion.contents, function () {
                    pageViewModel.contents.push(this);
                    this.region = regionViewModel;
                });
            } else if (currentTag.hasClass(classes.contentStart)) {
                // Content start
                pageContentId = currentTag.data('pageContentId');
                currentContent = {
                    pageContentId: pageContentId,
                    startTag: currentTag
                };
                allContents.push(currentContent);
            } else if (currentTag.hasClass(classes.contentEnd)) {
                // Content end
                currentContent = allContents.pop();
                currentRegion = allRegions[allRegions.length - 1];
                parentPageContentId = null;
                if (allContents.length > 0) {
                    parentPageContentId = allContents[allContents.length - 1].pageContentId;
                }

                var contentViewModel = new ContentViewModel(currentContent.startTag, currentTag, parentPageContentId);
                currentRegion.contents.push(contentViewModel);
            }
        }

        console.log('Regions found: ' + pageViewModel.regions.length);
    }

    /**
    * Initializes events for regions:
    */
    content.initRegions = function () {
        bcms.logger.trace('Highlight regions');

        pageViewModel = new PageViewModel();

        // TODO: remove after tests
        window.cms.page = pageViewModel;
        window.cms.path = masterPagesModel;
        window.cms.content = content;

        var tags = $(selectors.regionsAndContents).toArray();
        collectRegionsAndContents(tags, 0);
        
        $.each(pageViewModel.regions, function () {
            this.initializeRegion();
        });
        
        $.each(pageViewModel.contents, function () {
            this.initializeContent();

            content.createContentOverlay(this);
            content.initOverlayEvents(this);
            if (!bcms.editModeIsOn() && this.hideEndingDiv) {
                this.contentEnd.hide();
            }
        });

        $.each(cms.page.contents, function() {
            var childRegions = this.getChildRegions();

            if (childRegions.length == 0) {
                this.isCurrent = true;
            }
        });

        $.each(cms.page.regions, function () {
            this.setVisibility();
        });

        content.refreshRegionsPosition();
        content.refreshContentsPosition();

        $(window).on('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {
                content.refreshRegionsPosition();
                content.refreshContentsPosition();
                if (masterPagesModel != null) {
                    masterPagesModel.calculatePathPositions();
                }
            }, 100);
        });
    };

    /**
    * Cancels sort mode after the 'editModeOff' event is triggered.
    */
    content.cancelSortMode = function () {
        content.turnSortModeOff(true);

        bcms.logger.trace('Cancel Sort Mode');
    };

    /**
    * Occurs when edit mode is turned off
    */
    function onEditModeOff() {
        content.cancelSortMode();
        
        if (pageViewModel != null) {
            $.each(pageViewModel.contents, function () {
                this.overlay.hide();
                
                if (this.hideEndingDiv) {
                    this.contentEnd.hide();
                }
            });
        }
    }

    /**
    * Occurs when edit mode is turned on
    */
    function onEditModeOn() {
        if (pageViewModel != null) {
            $.each(pageViewModel.contents, function () {
                if (!this.isContentVisible()) {
                    return;
                }

                this.overlay.show();
                
                if (this.hideEndingDiv) {
                    this.contentEnd.show();
                }
            });
        }

        content.refreshContentsPosition();
        content.refreshRegionsPosition();

        content.refreshMasterPagesPath();
    }

    /**
    * Master page path item view model
    */
    function PathViewModel(element, index) {
        var self = this;

        self.element = element;
        self.index = index;

        return self;
    }

    /**
    * View model for controling the path of master pages
    */
    function MasterPagesPathModel() {
        var self = this,
            pathContainer = $(selectors.masterPagesPathContainer),
            innerContainer = pathContainer.find(selectors.masterPagesPathInnerContainer),
            handle = pathContainer.find(selectors.masterPagesPathHandler),
            leftSlider = pathContainer.find(selectors.masterPagesPathSliderLeft),
            rightSlider = pathContainer.find(selectors.masterPagesPathSliderRight),
            items = [],
            currentItem = 0,
            maxItem = 0;
       
        function hasPath() {
            return items.length > 0;
        }

        function getPathVisibility() {
            var showPage = localStorage.getItem(keys.showMasterPagesPath);
            
            if (showPage === undefined) {
                showPage = 1;
                setPathVisibility(showPage);
            }
            
            return showPage;
        }

        function setPathVisibility(isVisible) {
            localStorage.setItem(keys.showMasterPagesPath, isVisible);

            if (isVisible == 1) {
                pathContainer.removeClass(classes.masterPagesPathToggler);
                handle.html(globalization.hideMasterPagesPath);
            } else {
                pathContainer.addClass(classes.masterPagesPathToggler);
                handle.html(globalization.showMasterPagesPath);
            }
        }

        function onHandleClick() {
            if (pathContainer.hasClass(classes.masterPagesPathToggler)) {
                setPathVisibility(1);
            } else {
                setPathVisibility(0);
            }
        }

        function slide(step) {
            var itemNr = currentItem + step,
                length = items.length,
                margin,
                i,
                margins = [];

            if (itemNr >= 0 && itemNr <= maxItem) {
                currentItem += step;
                for (i = 0; i < length; i++) {
                    if (i < currentItem) {
                        margin = -items[i].element.outerWidth();
                    } else {
                        margin = 0;
                    }

                    margins.push({
                        item: i,
                        margin: margin
                    });
                }
            }

            for (i = 0; i < margins.length; i++) {
                items[margins[i].item].element.css('margin-left', margins[i].margin);
            }

            updateSliders();
        }

        function updateSliders() {
            if (currentItem > 0) {
                leftSlider.removeClass(classes.masterPagesPathInactiveArrow);
            } else {
                leftSlider.addClass(classes.masterPagesPathInactiveArrow);
            }
            
            if (currentItem < maxItem) {
                rightSlider.removeClass(classes.masterPagesPathInactiveArrow);
            } else {
                rightSlider.addClass(classes.masterPagesPathInactiveArrow);
            }
        }
        
        function slideToTheFirstParent() {
            if (!hasPath()) {
                return;
            }

            var width = innerContainer.width(),
                length = items.length,
                sum = items[length - 1].element.outerWidth(),
                i,
                slidesToLeave = 0,
                step;

            for (i = length - 2; i >= 0; i--) {
                sum += items[i].element.outerWidth();
                if (sum <= width) {
                    slidesToLeave++;
                }
                bcms.logger.trace('Sliding to first: slidesToLeave: ' + slidesToLeave + ' sum: ' + sum + ' currentItem: ' + items[i].element.outerWidth() + ' width: ' + width);
            }

            maxItem = length - 1 - slidesToLeave;
            bcms.logger.trace('Slide to: ' + maxItem);
            step = maxItem - currentItem;
            if (step > 0) {
                slide(step);
            }
        }

        function redraw() {
            if (!hasPath()) {
                pathContainer.hide();

                return;
            } else {
                pathContainer.show();
            }

            self.calculatePathPositions();
        }

        self.calculatePathPositions = function () {
            if (!hasPath()) {
                return;
            }

            var ww = $(window).width(),
                cw = ww * 0.8,
                totalItemsWidth = leftSlider.outerWidth() + leftSlider.outerWidth() + 30;

            $.each(items, function (index) {
                totalItemsWidth += items[index].element.outerWidth();
                bcms.logger.trace('Item: ' + items[index].element.outerWidth() + '; total: ' + totalItemsWidth + '; cw: ' + cw);
            });

            pathContainer.css('width', cw > totalItemsWidth ? totalItemsWidth : cw);
            pathContainer.css('left', ww / 2);
            pathContainer.css('margin-left', cw > totalItemsWidth ? totalItemsWidth / -2 : cw / -2);
        };

        self.initialize = function () {
            pathContainer.find(selectors.masterPagesPathItem).each(function (index) {
                var item = $(this),
                    html = item.html();

                if (html.length > 50) {
                    item.attr('title', html);
                    html = html.substr(0, 50) + '...';
                    item.html(html);
                }
                items.push(new PathViewModel(item, index));

                item.on('click', function () {
                    var url = $(this).data('url');

                    window.location.href = url;
                });
            });

            setPathVisibility(getPathVisibility());
            handle.on('click', onHandleClick);

            leftSlider.on('click', function () {
                slide(-1);
            });
            rightSlider.on('click', function () {
                slide(1);
            });

            redraw();
            slideToTheFirstParent();
        };

        return self;
    }

    /**
    * Recalculates and shows / hides master page path
    */
    content.refreshMasterPagesPath = function () {
        if (masterPagesModel != null) {
            masterPagesModel.calculatePathPositions();
        }
    };

    /**
    * Initializes sidebar module.
    */
    content.init = function () {
        bcms.logger.debug('Initializing content module');
        
        masterPagesModel = new MasterPagesPathModel();
        masterPagesModel.initialize();

        content.initRegions();
    };

    /**
    * Subscribe to event, so that when edit mode is turned off cancel sort mode
    */
    bcms.on(bcms.events.editModeOff, onEditModeOff);
    bcms.on(bcms.events.editModeOn, onEditModeOn);

    /**
    * Register initialization
    */
    bcms.registerInit(content.init);

    return content;
});
