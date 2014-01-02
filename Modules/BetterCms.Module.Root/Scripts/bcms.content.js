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

            regionsAndContents: '.bcms-region-start, .bcms-region-end, .bcms-content-start, .bcms-content-end',
            regionOverlay: '#bcms-region-overlay-template',
            
            regionAddContentButtons: '.bcms-region-addcontent',
            regionSortButtons: '.bcms-region-sortcontent',
            regionSortDoneButtons: '.bcms-region-sortdone',
            regionButtons: '.bcms-region-button',
            regionActions: '.bcms-region-actions',
            regionSortWrappers: '.bcms-sort-wrapper',
            regionSortBlock: '.bcms-sorting-block',
            
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
            masterPagesPathInactiveArrow: 'bcms-path-arrow-inactive'
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
            hideMasterPagesPath: null
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
            var changedRegions = content.turnSortModeOff();

            if (changedRegions.length > 0) {
                saveContentChanges(changedRegions);
            }
        });
    };

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
        
        $.each(pageViewModel.regions, function() {
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
    function RegionViewModel(regionStart, regionEnd, regionContents) {
        var self = this;

        self.id = regionStart.data('id');
        self.regionStart = regionStart;
        self.regionEnd = regionEnd;
        self.contents = regionContents;
        self.overlay = null;
        self.sortBlock = null;
        
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
    }
    
    /**
    * Page content view model
    */
    function ContentViewModel(contentStart, contentEnd) {
        var self = this;

        self.contentStart = contentStart;
        self.contentEnd = contentEnd;
        self.overlay = null;
        self.hideEndingDiv = contentEnd.data('hide') === true;

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

        self.removeHistoryButton = function () {
            self.overlay.find(selectors.contentHistory).remove();
        };

        self.removeEditButton = function () {
            self.overlay.find(selectors.contentEdit).remove();
        };

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
        bcms.logger.trace('Highlight regions');

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
            if (!bcms.editModeIsOn() && this.hideEndingDiv) {
                this.contentEnd.hide();
            }
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
            hasPath = pathContainer.length > 0,
            handle = pathContainer.find(selectors.masterPagesPathHandler),
            leftSlider = pathContainer.find(selectors.masterPagesPathSliderLeft),
            rightSlider = pathContainer.find(selectors.masterPagesPathSliderRight),
            items = [],
            currentItem = 0,
            maxItem = 0;
       
        function getPathVisibility() {
            var showPage = localStorage.getItem(keys.showMasterPagesPath);
            
            if (showPage === undefined) {
                showPage = 1;
                setPathVisibility(showPage);
            }
            
            return showPage;
        };

        function setPathVisibility(isVisible) {
            localStorage.setItem(keys.showMasterPagesPath, isVisible);

            if (isVisible == 1) {
                pathContainer.removeClass(classes.masterPagesPathToggler);
                handle.html(globalization.hideMasterPagesPath);
            } else {
                pathContainer.addClass(classes.masterPagesPathToggler);
                handle.html(globalization.showMasterPagesPath);
            }
        };

        function onHandleClick() {
            if (pathContainer.hasClass(classes.masterPagesPathToggler)) {
                setPathVisibility(1);
            } else {
                setPathVisibility(0);
            }
        };

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
            var width = pathContainer.find(selectors.masterPagesPathInnerContainer).width(),
                length = items.length,
                sum = items[length - 1].element.outerWidth(),
                i,
                slidesToLeave = 0;

            for (i = length - 2; i >= 0; i--) {
                sum += items[i].element.outerWidth();
                if (sum <= width) {
                    slidesToLeave++;
                }
                bcms.logger.trace('Sliding to first: slidesToLeave: ' + slidesToLeave + ' sum: ' + sum + ' currentItem: ' + items[i].element.outerWidth() + ' width: ' + width);
            }

            maxItem = length - 1 - slidesToLeave;
            bcms.logger.trace('Slide to: ' + maxItem);
            slide(maxItem);
        }

        self.calculatePathPositions = function () {
            if (!hasPath) {
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
            if (!hasPath) {
                return;
            }
            
            setPathVisibility(getPathVisibility());
            handle.on('click', onHandleClick);
            pathContainer.show();
            
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

            leftSlider.on('click', function () {
                slide(-1);
            });
            rightSlider.on('click', function () {
                slide(1);
            });
            
            self.calculatePathPositions();
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
