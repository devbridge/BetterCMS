/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.content', ['jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var content = {},

        // Selectors used in the module to locate DOM elements:
        selectors = {
            contentOverlay: '#bcms-content-overlay',
            contentOverlayBg: '.bcms-content-overlaybg',
            content: '.bcms-content',
            contentDelete: '.bcms-content-delete',
            contentEdit: '.bcms-content-edit',
            contentEditInnerDiv: '.bcms-content-edit .bcms-content-icon',
            contentHistory: '.bcms-content-history',
            contentConfigure: '.bcms-content-configure',

            regionOverlay: '#bcms-region-overlay-template',
            region: '.bcms-region',
            regionTop: '.bcms-region-top',
            regionBottom: '.bcms-region-bottom',
            regionLeft: '.bcms-region-left',
            regionRight: '.bcms-region-right',
            regionActions: '.bcms-region-actions',
            renderedRegions: '.bcms-render-region',
            renderedContents: 'script[type="text/html"]'
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

        regionViewModel.overlay = rectangle;
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

            var padding = 7,
                extra = padding * 2,
                rectangle = $(this),
                contentViewModel = rectangle.data('target'),
                width = (contentViewModel.width + extra) + 'px',
                height = (contentViewModel.height + extra) + 'px',
                top = (contentViewModel.top - padding) + 'px',
                left = (contentViewModel.left - padding) + 'px',
                background = $(selectors.contentOverlayBg, rectangle);

            background.css({
                'width': width,
                'height': height
            });

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
    content.refreshRegionsPosition = function () {
        
        if (!bcms.editModeIsOn() || pageViewModel == null) {
            return;
        }

        $.each(pageViewModel.regions, function () {
            this.recalculatePositions();
        });

        var actionsContainerWidth = regionRectangles.first().find(selectors.regionActions).width() + 4;

        regionRectangles.each(function () {

            var padding = 10,
                rectangle = $(this),
                regionViewModel = rectangle.data('target'),
                width = regionViewModel.width + (padding * 2),
                height = regionViewModel.height;

            $(selectors.regionTop, rectangle).css({ width: width + 'px' });
            $(selectors.regionBottom, rectangle).css({ width: width + 'px', top: (height - 2) + 'px' });
            $(selectors.regionLeft, rectangle).css({ height: height + 'px' });
            $(selectors.regionRight, rectangle).css({ height: height + 'px', left: (width - 2) + 'px' });
            $(selectors.regionActions, rectangle).css({ left: width - actionsContainerWidth + 'px' });

            rectangle.css({
                top: regionViewModel.top + 'px',
                left: regionViewModel.left - padding + 'px',
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

        $('.bcms-region-addcontent', regionViewModel.overlay).on('click', function () {
            bcms.trigger(bcms.events.addPageContent, regionId);
        });

        $('.bcms-region-sortcontent', regionViewModel.overlay).on('click', function () {
            content.turnSortModeOn(regionViewModel);
        });

        $('.bcms-region-sortdone', regionViewModel.overlay).on('click', function () {
            content.turnSortModeOff(regionViewModel);

            var pageContents = [];
            $.each(regionViewModel.contents, function () {
                pageContents.push({ 'Id': this.pageContentId, 'Version': this.pageContentVersion });
            });
            var model = { region: regionViewModel, data: { 'pageId': bcms.pageId, 'regionId': regionViewModel.id, 'pageContents': pageContents } };

            bcms.trigger(bcms.events.sortPageContent, model);
        });
    };

    /**
    * Turns region content sorting mode OFF:
    */
    content.turnSortModeOff = function (regionViewModel, cancel) {
        $('.bcms-region-button', regionViewModel.overlay).show();
        $('.bcms-region-sortdone', regionViewModel.overlay).hide();

        regionViewModel.overlay.sortable('destroy');

        var regionContents = [];

        $('.bcms-sort-wrapper', regionViewModel.overlay).each(function () {
            var viewModel = $(this).data('target');

            regionContents.push(viewModel);

            $(this).remove();
        });

        if (!cancel) {
            regionViewModel.contents = regionContents;
        }

        $.each(regionContents, function () {
            $.each(this.elements, function () {
                $(this).show();
            });
            this.overlay.show();
        });

        content.refreshRegionsPosition();
        content.refreshContentsPosition();
    };

    /**
    * Turns region content sorting mode ON:
    */
    content.turnSortModeOn = function (regionViewModel) {

        sortableRegions.push(regionViewModel);
        
        $('.bcms-region-button', regionViewModel.overlay).hide();
        $('.bcms-region-sortdone', regionViewModel.overlay).show();

        $(regionViewModel.contents).each(function () {
            var sortWrapper = $('<div class="bcms-sort-wrapper" />');

            $('<div class="bcms-sort-content" />').html(this.title).appendTo(sortWrapper);
            sortWrapper.append('<div class="bcms-sort-overlay bcms-content-overlaybg" />');

            // Store reference to content so it can be sorted later:
            sortWrapper.data('target', this);

            this.overlay.hide();

            regionViewModel.overlay.append(sortWrapper);
        });

        regionViewModel.overlay.sortable();
    };

    /**
    * Removes script blocks from HTML string:
    */
    content.removeScripts = function (html) {
        var regex = new RegExp('<script.*?>.*?</script>', 'gi');
        return html.replace(regex, '');
    };

    /**
    * Function calculates top, left positions and width and height for specified list of DOM elements
    */
    function calculatePositions(elements) {
        var right = 0,
            bottom = 0,
            maxValue = 9999999,
            top = maxValue,
            left = maxValue,
            calculatableElements = [elements[0], elements[elements.length - 1]];

        $.each(calculatableElements, function (index, self) {
            var element = $(self),
                targetOffset = element.offset(),
                targetWidth,
                targetHeight;

            // Exception handling for empty text nodes
            try {
                targetWidth = $(this).outerWidth();
                targetHeight = $(this).outerHeight();
            } catch (exc) {
                // Do nothing
                return true;
            }

            // Process only if offset is found and width or height are more that zero
            // (fix for various tags, such as scripts, text nodes, etc)
            if (targetOffset != null && (targetWidth > 0 || targetHeight > 0)) {
                if (targetOffset.top < top) {
                    top = targetOffset.top;
                }
                if (targetOffset.left < left) {
                    left = targetOffset.left;
                }
                if (targetOffset.left + targetWidth > right) {
                    right = targetOffset.left + targetWidth;
                }
                if (targetOffset.top + targetHeight > bottom) {
                    bottom = targetOffset.top + targetHeight;
                }
            }
        });

        if (left == maxValue) {
            left = 0;
        }
        if (top == maxValue) {
            top = 0;
        }

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
    function RegionViewModel(id, elements, regionContents) {
        var self = this;

        self.id = id;
        self.elements = elements;
        self.contents = regionContents;
        self.overlay = null;
        
        self.left = 0;
        self.top = 0;
        self.width = 0;
        self.height = 0;

        self.recalculatePositions = function() {
            var positions = calculatePositions(self.elements);
            
            self.left = positions.left;
            self.top = positions.top;
            self.width = positions.width;
            self.height = positions.height;
        };
    }
    
    /**
    * Page content view model
    */
    function ContentViewModel(elements, contentContainer) {
        var self = this;

        self.elements = elements;
        self.overlay = null;

        self.contentId = contentContainer.data('contentId');
        self.pageContentId = contentContainer.data('pageContentId');
        self.contentVersion = contentContainer.data('contentVersion');
        self.pageContentVersion = contentContainer.data('pageContentVersion');
        self.contentType = contentContainer.data('contentType');
        self.draft = contentContainer.data('draft');
        self.title = contentContainer.data('contentTitle');

        self.left = 0;
        self.top = 0;
        self.width = 0;
        self.height = 0;

        self.recalculatePositions = function () {
            var positions = calculatePositions(self.elements);
            
            self.left = positions.left;
            self.top = positions.top;
            self.width = positions.width;
            self.height = positions.height;
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

        var tags = $('.bcms-region-start, .bcms-region-end, .bcms-content-start, .bcms-content-end').toArray(),
            tagsCount = tags.length,
            regionStart,
            i;
        
        for (i = 0; i < tagsCount; i++) {
            regionStart = $(tags[i]);
            if (regionStart.hasClass("bcms-region-start")) {
                var regionId = regionStart.data('id'),
                    regionContentViewModels = [],
                    currentTag,
                    j,
                    contentStartFound = false,
                    contentStart = null;

                for (j = i; j < tagsCount; j++) {
                    currentTag = $(tags[j]);

                    if (currentTag.hasClass("bcms-content-start")) {
                        contentStart = currentTag;
                        contentStartFound = true;
                    } else if (currentTag.hasClass("bcms-content-end") && contentStartFound) {
                        contentStartFound = false;
                        
                        var contentViewModel = new ContentViewModel([contentStart, currentTag], contentStart);
                        regionContentViewModels.push(contentViewModel);
                    } else if (currentTag.hasClass("bcms-region-end")) {
                        var regionElements = [regionStart, currentTag],
                            regionViewModel = new RegionViewModel(regionId, regionElements, regionContentViewModels);

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
