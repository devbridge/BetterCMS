/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.sidemenu', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.jqueryui'], function ($, bcms) {
    'use strict';

    var sidemenu = {},

    // Classes that are used to maintain various UI states:
        classes = {
            stateOpen: 'bcms-state-open',
            sideMenuRight: 'bcms-sidemenu-right',
            editingOnClass: 'bcms-on'
        },

    // Selectors used in the module to locate DOM elements:
        selectors = {
            container: '#bcms-sidemenu',
            leftRightHandle: '#bcms-sidemenu-position-handle',
            openCloseHandle: '#bcms-sidemenu-hide',
            editOnOffHandle: '#bcms-sidemenu-switch',
            draggHandle: '#bcms-sidemenu-draghandle',
            html: 'html',
            button: '.bcms-sidemenu-button',
            footerTextHolder: '.bcms-sidemenu-stick-text'
        },

    // Keys used for localStorage:
        keys = {
            sideMenuOnTheRight: 'bcms.sideMenuOnTheRight',
            sideMenuOffsetTop: 'bcms.sideMenuOffsetTop',
            sideMenuOpen: 'bcms.sideMenuOpen',
            editingOn: 'bcms.editingOn'
        },

    // Holds jQuery sidemenu object:
        sidemenuContainer,

        links = {},
        globalization = {
            stickRightMenuTitle: null,
            stickLeftMenuTitle: null
        };

    // Assign objects to module
    sidemenu.classes = classes;
    sidemenu.selectors = selectors;
    sidemenu.links = links;
    sidemenu.globalization = globalization;

    /**
    * Opens sidebar menu.
    */
    sidemenu.open = function (doNotAnimate) {
        var isOnRightSide = sidemenuContainer.hasClass(classes.sideMenuRight),
            sideMenuWidth = '0px',
            css = isOnRightSide
                ? { 'right': sideMenuWidth }
                : { 'left': sideMenuWidth };

        localStorage.setItem(keys.sideMenuOpen, '1');

        if (doNotAnimate) {
            sidemenuContainer.css(css).addClass(classes.stateOpen);
            return;
        }

        sidemenuContainer.animate(css, 100, function () {
            sidemenuContainer.addClass(classes.stateOpen);
        });
    };

    /**
    * Closes sidebar menu.
    */
    sidemenu.close = function (doNotAnimate) {
        var isOnRightSide = sidemenuContainer.hasClass(classes.sideMenuRight),
			sideMenuWidth = '-260px',
			css = isOnRightSide
					? { 'right': sideMenuWidth }
					: { 'left': sideMenuWidth };

        localStorage.removeItem(keys.sideMenuOpen);

        if (doNotAnimate) {
            sidemenuContainer.removeClass(classes.stateOpen);
            return;
        }

        sidemenuContainer.animate(css, 100, function () {
            sidemenuContainer.removeClass(classes.stateOpen);
        });
    };

    /**
    * Indicates whether sidebar is open.
    */
    sidemenu.isOpen = function () {
        return sidemenuContainer.hasClass(classes.stateOpen);
    };

    /**
    * Moves sidebar menu to the right.
    */
    sidemenu.moveToRight = function () {
        localStorage.setItem(keys.sideMenuOnTheRight, '1');
        sidemenuContainer.css({
            'left': 'auto',
            'right': sidemenu.isOpen() ? '0px' : '-250px'
        }).addClass(classes.sideMenuRight);
        sidemenuContainer.find(selectors.footerTextHolder).html(globalization.stickLeftMenuTitle);
    };

    /**
    * Moves sidebar menu to the left.
    */
    sidemenu.moveToLeft = function () {
        localStorage.removeItem(keys.sideMenuOnTheRight);
        sidemenuContainer.css({
            'left': sidemenu.isOpen() ? '0px' : '-250px',
            'right': 'auto'
        }).removeClass(classes.sideMenuRight);
        sidemenuContainer.find(selectors.footerTextHolder).html(globalization.stickRightMenuTitle);
    };

    /**
    * Turns edit mode ON.
    */
    sidemenu.turnEditModeOn = function () {
        localStorage.setItem(keys.editingOn, '1');
        $(selectors.html).addClass(classes.editingOnClass);
        bcms.trigger(bcms.events.editModeOn);
    };

    /**
    * Turns edit mode OFF.
    */
    sidemenu.turnEditModeOff = function () {
        localStorage.removeItem(keys.editingOn);
        $(selectors.html).removeClass(classes.editingOnClass);
        bcms.trigger(bcms.events.editModeOff);
    };

    /**
    * Initializes sidebar module.
    */
    sidemenu.init = function () {

        console.log('Initialize sidebar');
        $(selectors.container).hide();
        sidemenuContainer = $(selectors.container);

        var offsetTop = localStorage.getItem(keys.sideMenuOffsetTop),
            isMenuOnTheRight = localStorage.getItem(keys.sideMenuOnTheRight),
            isSideMenuOpen = localStorage.getItem(keys.sideMenuOpen),
            editingOn = localStorage.getItem(keys.editingOn),
            middleOfTheScreen;

        if (offsetTop) {
            var windowHeight = $(window).height();
            var sidemenuHeight = sidemenuContainer.height();
            if (offsetTop > windowHeight - sidemenuHeight) {
                sidemenuContainer.css('top', (windowHeight - sidemenuHeight - 15) + 'px');
            } else if (offsetTop >= 0) {
                sidemenuContainer.css('top', offsetTop + 'px');
            }
        }

        if (isMenuOnTheRight) {
            sidemenu.moveToRight(true);
        }

        if (isSideMenuOpen) {
            sidemenu.open(true);
        }

        if (editingOn) {
            sidemenu.turnEditModeOn();
        }

        $(selectors.leftRightHandle).on('click', function () {
            if (sidemenuContainer.hasClass(classes.sideMenuRight)) {
                sidemenu.moveToLeft();
            } else {
                sidemenu.moveToRight();
            }
        });

        $(selectors.openCloseHandle).on('click', function () {
            if (sidemenu.isOpen()) {
                sidemenu.close();
            } else {
                sidemenu.open();
            }
        });

        $(selectors.editOnOffHandle).on('click', function () {
            if ($(selectors.html).hasClass(classes.editingOnClass)) {
                sidemenu.turnEditModeOff();
            } else {
                sidemenu.turnEditModeOn();
            }
        });

        sidemenuContainer.draggable({
            axis: 'y',
            handle: selectors.draggHandle,
            containment: 'window',
            stop: function (event, ui) {
                var top = ui.position.top;

                var wHeight = $(window).height();
                var sHeight = sidemenuContainer.height();
                if (top > wHeight - sHeight) {
                    top = (windowHeight - sidemenuHeight - 15);
                    sidemenuContainer.css('top', top +'px');
                } else if (top < 0) {
                    top = 0;
                    sidemenuContainer.css('top', '0px');
                } 
                localStorage.setItem(keys.sideMenuOffsetTop, top);
            },
            start: function () {
                middleOfTheScreen = $(window).width() / 2;
            },
            drag: function (event) {
                var left = event.screenX;
                if (isMenuOnTheRight) {
                    if (left < middleOfTheScreen) {
                        isMenuOnTheRight = false;
                        sidemenu.moveToLeft();
                    }
                } else {
                    if (left > middleOfTheScreen) {
                        isMenuOnTheRight = true;
                        sidemenu.moveToRight();
                    }
                }
            }
        });

        sidemenuContainer.show();

        console.log('Initialize sidebar done');
    };

    bcms.registerInit(sidemenu.init);
    
    return sidemenu;
});
