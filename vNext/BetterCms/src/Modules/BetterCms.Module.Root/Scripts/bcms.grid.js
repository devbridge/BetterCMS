/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.grid', ['bcms.jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var grid = { },
        selectors = {
            firstTable: 'table.bcms-tables:first',
            emptyRow: 'tr.bcms-grid-empty-row',
            anyRow: 'tbody > tr:not(.bcms-grid-empty-row):first',
            sortColumnHeaders: 'a.bcms-sort-arrow',
            hiddenSortColumnField: '#bcms-grid-sort-column',
            hiddenSortDirectionField: '#bcms-grid-sort-direction',
            hiddenPageNumberField: '#bcms-grid-page-number',
            formLoaderContainer: '.bcms-rightcol:first',
            pageNumbers: '.bcms-pager-no, .bcms-pager-prev, .bcms-pager-next',
            scrollWindow: '.bcms-scroll-window:first'
        },
        links = {
            
        },
        globalization = {
            
        };

    /**
    * Assign objects to module.
    */
    grid.links = links;
    grid.globalization = globalization;
    grid.selectors = selectors;

    /**
    * Shows or hides empty row
    */
    grid.showHideEmptyRow = function(container) {
        var table = container.find(selectors.firstTable);
        if (grid.gridIsEmpty(table)) {
            table.find(selectors.emptyRow).show();
        } else {
            table.find(selectors.emptyRow).hide();
        }
    };
    
    /**
    * Checks if grid has any rows
    */
    grid.gridIsEmpty = function (table) {
        return table.find(selectors.anyRow).length == 0;
    };

    /**
    * Binds site settings list form
    */
    grid.bindGridForm = function (form, onSuccess) {
        form.find(selectors.sortColumnHeaders).on('click', function () {
            var self = $(this);
            
            form.find(selectors.hiddenSortColumnField).val(self.data('column'));
            form.find(selectors.hiddenSortDirectionField).val(self.data('direction'));

            submitGridForm(form, onSuccess);
        });
        
        form.find(selectors.pageNumbers).on('click', function () {
            var self = $(this),
                pageNumber = self.data('pageNumber');

            if (pageNumber) {
                form.find(selectors.hiddenPageNumberField).val(pageNumber);
                submitGridForm(form, onSuccess);
            }
        });
    };

    /**
    * Submits site settings list form
    */
    grid.submitGridForm = function (form, onSuccess) {
        form.find(selectors.hiddenPageNumberField).val(1);

        return submitGridForm(form, onSuccess);
    };
    
    /**
    * Submits site settings list form
    */
    function submitGridForm(form, onSuccess) {
        var container = form.parents(selectors.formLoaderContainer);
        if (container.length == 0) {
            container = form.parents(selectors.scrollWindow);
        }
        if (container.length == 0) {
            container = form;
        }
        $(container).showLoading();
        $.ajax({
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded',
            cache: false,
            url: form.attr('action'),
            data: form.serialize(),
            error: function () {
                $(container).hideLoading();
            },
            success: function (data, status, response) {
                $(container).hideLoading();
                if ($.isFunction(onSuccess)) {
                    if (response.getResponseHeader('Content-Type').indexOf('application/json') === 0 && data.Html) {
                        onSuccess(data.Html, data.Data);
                    } else {
                        onSuccess(data, null);
                    }
                }
            }
        });
    }

    /**
    * Focuses search input and puts cursor to end of input
    */
    grid.focusSearchInput = function (searchInput, withTimeout) {
        var focusInput = function() {
            var val = searchInput.val();
            searchInput.focus().val("");
            searchInput.val(val);
        };

        // Timeout is required to work on IE11
        if (!withTimeout) {
            focusInput();
        } else {
            setTimeout(focusInput, 200);
        }
    };

    /**
    * Initializes grid module.
    */
    grid.init = function () {
        bcms.logger.debug('Initializing bcms.grid module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(grid.init);
    
    return grid;
});
