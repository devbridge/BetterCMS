/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.grid', ['bcms.jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var grid = { },
        selectors = {
            firstTable: 'table.bcms-tables:first',
            emptyRow: 'tr.bcms-grid-empty-row',
            anyRow: 'tbody > tr:not(.bcms-grid-empty-row):first',
            sortColumnHeaders: 'a.bcms-sort-arrow',
            hiddenSortColumnField: '#bcms-grid-sort-column',
            hiddenSortDirectionField: '#bcms-grid-sort-direction'
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

            grid.submitGridForm(form, onSuccess);
        });
    };

    /**
    * Submits site settings list form
    */
    grid.submitGridForm = function (form, onSuccess) {
//        $(form).showLoading();
        $.ajax({
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded',
            dataType: 'html',
            cache: false,
            url: form.attr('action'),
            data: form.serialize(),
//            error: function() {
//                $(form).hideLoading();
//            },
            success: function (data) {
//                $(form).hideLoading();
                if ($.isFunction(onSuccess)) {
                    onSuccess(data);
                }
            }
        });
    };

    /**
    * Initializes grid module.
    */
    grid.init = function () {
        console.log('Initializing bcms.grid module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(grid.init);
    
    return grid;
});
