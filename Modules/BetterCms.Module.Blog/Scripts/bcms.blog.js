/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.page', ['jquery', 'bcms', 'bcms.modal'], function ($, bcms, modal) {
    'use strict';

    var page = { },
        module = {
            name: 'bcms.page',
            actions: {
                editPageProperties: 'editPageProperties',
                addNewPage: 'addNewPage',
                deleteCurrentPage: 'deleteCurrentPage'
            }
        },
        links = {
            loadEditPropertiesUrl : null,
            loadPagesListUrl : null
        },
        globalization = {
            editPagePropertiesModalTitle : null
        };

    // Assign objects to module.
    page.links = links;
    page.globalization = globalization;
           
    /**
    * Loads edit properties dialog:
    */
    page.loadEditProperties = function (popup) {
        console.log('Loading Edit Properties');

        $.ajax({
            url: page.links.loadEditPropertiesUrl
        }).done(function (content) {
            console.log('Done Loading Edit Properties');            
            popup.setTitle('Edit Page Properties');
            popup.resize(900, function () {
                popup.setContent(content);
            });
        });
    };

    page.loadAddNew = function(popup) {
        console.log('Loading Add New Page');

        $.ajax({
            url : '/add-new-page'
        }).done(function (content) {
            console.log('Done Loading Add New Page');
            popup.setTitle('Add New Page');
            popup.setContent(content);            
        });
    };

    page.loadDeleteCurrentPageConfirmation = function() {
        console.log('Loading Delete Current Page Confirmation');
        confirm("Are you sure you want to delete current page?");
    };

    page.allPagesList = function() {
        $.ajax(page.links.loadPagesListUrl).done(function() {
            alert("fill modal");
        });
    };
    
    /**
    * Initializes page module.
    */
    page.initActions = function () {
        bcms.on(bcms.events.onClickAction, function(data) {
            if (data.module === module.name) {
                
                if (data.action === module.actions.editPageProperties) {
                    modal.open({
                        title: 'Edit Page Properties - Loading...',
                        onLoad: page.loadEditProperties
                    });                    
                }
                else if (data.action === module.actions.addNewPage) {
                    modal.open({
                        title: 'Add New Page - Loading...',
                        onLoad: page.loadAddNew
                    });
                }
                else if (data.action === module.actions.deleteCurrentPage) {
                    page.loadDeleteCurrentPageConfirmation();
                }
            }
        });
    };

    /**
    * Initializes page module.
    */
    page.init = function () {
        console.log('Initializing page module');
        page.initActions();
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(page.init);
    
    return page;
});
