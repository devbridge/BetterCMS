/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.content', ['bcms.jquery', 'bcms'], function ($, bcms) {
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

            regionsAndContents: '.bcms-region-start, .bcms-region-end, .bcms-content-start, .bcms-content-end',
            regionOverlay: '#bcms-region-overlay-template',
            
            regionAddContentButtons: '.bcms-region-addcontent',
            regionSortButtons: '.bcms-region-sortcontent',
            regionSortDoneButtons: '.bcms-region-sortdone',
            regionButtons: '.bcms-region-button',
            regionSortWrappers: '.bcms-sort-wrapper',
            regionSortBlock: '.bcms-sorting-block'
        },
        classes = {
            regionStart: 'bcms-region-start',
            regionEnd: 'bcms-region-end',
            contentStart: 'bcms-content-start',
            contentEnd: 'bcms-content-end',
            regionSortOverlay: 'bcms-show-overlay'
        },
        resizeTimer,
        currentContentDom,
        regionRectangles = $(),
        contentRectangles = $(),
        sortableRegions = [],
        links = {},
        globalization = {},
        pageViewModel,
        opacityAnimationSpeed = 50;

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
    * Draws visual line over CMS region:
    */
    content.highlightRegion = function (regionViewModel) {
        var container = $(selectors.regionOverlay),
            template = container.html(),
            rectangle = $(template);

        rectangle.data('target', regionViewModel);
        rectangle.insertBefore(container);
        regionRectangles = regionRectangles.add(rectangle);

        if (bcms.editModeIsOn()) {
            rectangle.show();
        }

        regionViewModel.overlay = rectangle;
        regionViewModel.sortBlock = regionViewModel.overlay.find(selectors.regionSortBlock);
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
            this.recalculatePositions();
        });

        contentRectangles.each(function () {

            var rectangle = $(this),
                contentViewModel = rectangle.data('target'),
                width = contentViewModel.width + 'px',
                height = contentViewModel.height + 'px',
                top = contentViewModel.top + 'px',
                left = contentViewModel.left + 'px';

            rectangle.css({
                'width': width,
                'height': height,
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
            this.recalculatePositions();
        });

        $.each(regionViewModels, function () {

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

        overlay.on('mouseleave', function () {
            // console.log('Content mouse leave');
            content.hideOverlay(contentViewModel);
        });

        overlay.on('mouseover', function () {
            if (!bcms.editModeIsOn() || currentContentDom === overlay) {
                // console.log('Exit content mouse over');
                return;
            }

            // console.log('Content mouse over');
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
    * Initializes events for region buttons:
    */
    content.initRegionEvents = function (regionViewModel) {

        var regionId = regionViewModel.id;

        $(selectors.regionAddContentButtons, regionViewModel.overlay).on('click', function () {
            bcms.trigger(bcms.events.addPageContent, regionId);
        });

        $(selectors.regionSortButtons, regionViewModel.overlay).on('click', function () {
            content.turnSortModeOn(regionViewModel);
        });

        $(selectors.regionSortDoneButtons, regionViewModel.overlay).on('click', function() {
            var hasChanges = content.turnSortModeOff(regionViewModel);

            if (hasChanges) {
                var pageContents = [],
                    model;

                $.each(regionViewModel.contents, function() {
                    pageContents.push({ 'Id': this.pageContentId, 'Version': this.pageContentVersion });
                });
                model = {
                    region: regionViewModel,
                    data: {
                        'pageId': bcms.pageId,
                        'regionId': regionViewModel.id,
                        'pageContents': pageContents,
                        'hasChanges': hasChanges,
                    }
                };

                bcms.trigger(bcms.events.sortPageContent, model);
            }
        });
    };

    /**
    * Checks if contents order has changed
    */
    function hasContentsOrderChanged(before, after) {
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
    content.turnSortModeOff = function (regionViewModel, cancel) {
        var regionContents = [],
            hasChanges = false;

        $(selectors.regionButtons, regionViewModel.overlay).show();
        $(selectors.regionSortDoneButtons, regionViewModel.overlay).hide();

        regionViewModel.sortBlock.sortable('destroy');
        regionViewModel.overlay.removeClass(classes.regionSortOverlay);
        regionViewModel.isSorting = false;
        regionViewModel.sortingContents = [];

        $(selectors.regionSortWrappers, regionViewModel.overlay).each(function () {
            var viewModel = $(this).data('target');

            regionContents.push(viewModel);

            $(this).remove();
        });

        if (!cancel) {
            hasChanges = hasContentsOrderChanged(regionViewModel.contents, regionContents);
            regionViewModel.contents = regionContents;
        }

        $.each(regionContents, function () {
            this.overlay.show();
        });

        content.refreshRegionsPosition();
        content.refreshContentsPosition();

        return hasChanges;
    };

    /**
    * Turns region content sorting mode ON:
    */
    content.turnSortModeOn = function (regionViewModel) {

        regionViewModel.isSorting = true;
        regionViewModel.sortingContents = [];

        sortableRegions.push(regionViewModel);
        
        $(selectors.regionButtons, regionViewModel.overlay).hide();
        $(selectors.regionSortDoneButtons, regionViewModel.overlay).show();

        $(regionViewModel.contents).each(function () {
            var sortWrapper = $('<div class="bcms-sort-wrapper" />');

            $('<div class="bcms-sort-content" />').html(this.title).appendTo(sortWrapper);
            sortWrapper.append('<div class="bcms-sort-overlay bcms-content-overlaybg" />');

            // Store reference to content so it can be sorted later:
            sortWrapper.data('target', this);

            this.overlay.hide();

            regionViewModel.sortBlock.append(sortWrapper);

            regionViewModel.sortingContents.push(sortWrapper);
        });

        regionViewModel.sortBlock.sortable();
        regionViewModel.overlay.addClass(classes.regionSortOverlay);
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
    function RegionViewModel(regionStart, regionEnd, regionContents) {
        var self = this;

        self.id = regionStart.data('id');
        self.regionStart = regionStart;
        self.regionEnd = regionEnd;
        self.contents = regionContents;
        self.overlay = null;
        self.sortBlock = null;

        self.isSorting = false;
        self.sortingContents = [];
        
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
            
            if (self.isSorting && self.sortingContents.length > 0) {
                for (var i = 0; i < self.sortingContents.length; i ++) {
                    var sortPositions = calculatePositions(self.regionStart, self.sortingContents[i]);
                    if (sortPositions.height > self.height) {
                        self.height = sortPositions.height;
                    }
                }
            }
        };
    }
    
    /**
    * Page content view model
    */
    function ContentViewModel(contentStart, contentEnd) {
        var self = this;

        self.contentStart = contentStart;
        self.contentEnd = contentEnd;
        self.overlay = null;

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
            self.height = positions.height ;
        };

        self.onEditContent = function() {};
        self.onDeleteContent = function() {};
        self.onConfigureContent = function() {};
        self.onContentHistory = function() {};

        self.removeConfigureButton = function () {
            self.overlay.find(selectors.contentConfigure).remove();
        };

        self.removeDeleteButton = function () {
            self.overlay.find(selectors.contentDelete).remove();
        };

        self.addDraftIcon = function () {
            self.overlay.find(selectors.contentEditInnerDiv).html('<div>*</div>');
        };

        return self;
    }

    /**
    * Initializes events for regions:
    */
    content.initRegions = function () {
        console.log('Highlight regions');

        pageViewModel = new PageViewModel();

        var tags = $(selectors.regionsAndContents).toArray(),
            tagsCount = tags.length,
            regionStart,
            i;
        
        for (i = 0; i < tagsCount; i++) {
            regionStart = $(tags[i]);
            if (regionStart.hasClass(classes.regionStart)) {
                var regionContentViewModels = [],
                    currentTag,
                    j,
                    contentStartFound = false,
                    contentStart = null;

                for (j = i; j < tagsCount; j++) {
                    currentTag = $(tags[j]);

                    if (currentTag.hasClass(classes.contentStart)) {
                        contentStart = currentTag;
                        contentStartFound = true;
                    } else if (currentTag.hasClass(classes.contentEnd) && contentStartFound) {
                        contentStartFound = false;
                        
                        var contentViewModel = new ContentViewModel(contentStart, currentTag);
                        regionContentViewModels.push(contentViewModel);
                    } else if (currentTag.hasClass(classes.regionEnd)) {
                        var regionViewModel = new RegionViewModel(regionStart, currentTag, regionContentViewModels);

                        pageViewModel.regions.push(regionViewModel);
                        $.each(regionContentViewModels, function() {
                            pageViewModel.contents.push(this);
                        });

                        i = j;
                        break;
                    }
                }
            }
        }
        
        $.each(pageViewModel.regions, function () {
            content.highlightRegion(this);
            content.initRegionEvents(this);
        });
        
        $.each(pageViewModel.contents, function () {
            content.createContentOverlay(this);
            content.initOverlayEvents(this);
        });

        content.refreshRegionsPosition();
        content.refreshContentsPosition();

        $(window).on('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {
                content.refreshRegionsPosition();
                content.refreshContentsPosition();
            }, 100);
        });
    };

    /**
    * Cancels sort mode after the 'editModeOff' event is triggered.
    */
    content.cancelSortMode = function () {
        $.each(sortableRegions, function (i, regionViewModel) {
            content.turnSortModeOff(regionViewModel, true);
        });

        sortableRegions = [];
        console.log('Cancel Sort Mode');
    };

    /**
    * Occurs when edit mode is turned off
    */
    function onEditModeOff() {
        content.cancelSortMode();
        
        if (pageViewModel != null) {
            $.each(pageViewModel.contents, function () {
                this.overlay.hide();
            });
        }
    }

    /**
    * Occurs when edit mode is turned on
    */
    function onEditModeOn() {
        if (pageViewModel != null) {
            $.each(pageViewModel.contents, function () {
                this.overlay.show();
            });
        }

        content.refreshContentsPosition();
        content.refreshRegionsPosition();
    }

    /**
    * Initializes sidebar module.
    */
    content.init = function () {
        console.log('Initializing content module');
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
