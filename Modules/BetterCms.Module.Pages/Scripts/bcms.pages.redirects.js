/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.redirects', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.grid'], function ($, bcms, dynamicContent, siteSettings, editor, grid) {
    'use strict';

    var redirect = {},
        selectors = {
            redirectsForm: '#bcms-redirects-form',
            searchLink: '#bcms-redirects-search-btn',
            searchField: '#SearchQuery',
            createLink: '#bcms-create-redirect-button',
            deleteLink: '.bcms-grid-item-delete-button',
            pageUrlEditor: 'input.bcms-page-url',
            redirectUrlEditor: 'input.bcms-redirect-url',
            pageUrl: 'a.bcms-page-url',
            redirectUrl: 'a.bcms-redirect-url',
            oldPageUrl: 'input.bcms-old-page-url',
            oldRedirectUrl: 'input.bcms-old-redirect-url'
        },
        links = {
            loadSiteSettingsRedirectListUrl: null,
            deleteRedirectUrl: null,
            saveRedirectUrl: null
        },
        globalization = {
            deleteRedirectMessage: null
        };

    /**
    * Assign objects to module.
    */
    redirect.links = links;
    redirect.globalization = globalization;

    /**
    * Loads site settings redirects list.
    */
    redirect.loadSiteSettingsRedirectList = function () {
        dynamicContent.bindSiteSettings(siteSettings, redirect.links.loadSiteSettingsRedirectListUrl, {
            contentAvailable: redirect.initializeSiteSettingsRedirectsList
        });
    };

    /**
    * Initializes site settings redirects list and list items
    */
    redirect.initializeSiteSettingsRedirectsList = function() {
        var dialog = siteSettings.getModalDialog(),
            container = dialog.container;
        
        var form = dialog.container.find(selectors.redirectsForm);
        grid.bindGridForm(form, function (data) {
            siteSettings.setContent(data);
            redirect.initializeSiteSettingsRedirectsList(data);
        });

        form.on('submit', function (event) {
            event.preventDefault();
            redirect.searchSiteSettingsRedirects(form, container);
            return false;
        });

        form.find(selectors.searchLink).on('click', function () {
            redirect.searchSiteSettingsRedirects(form, container);
        });

        form.find(selectors.createLink).on('click', function () {
            editor.addNewRow(container);
        });
        
        editor.initialize(container, {
            saveUrl: links.saveRedirectUrl,
            deleteUrl: links.deleteRedirectUrl,
            onSaveSuccess: redirect.setRedirectFields,
            rowDataExtractor: redirect.getRedirectData,
            deleteRowMessageExtractor: function () {
                return globalization.deleteRedirectMessage;
            }
        });

        // Select search.
        dialog.setFocus();
    };

    /**
    * Search site settings redirects
    */
    redirect.searchSiteSettingsRedirects = function(form, container) {
        grid.submitGridForm(form, function (data) {
            siteSettings.setContent(data);
            redirect.initializeSiteSettingsRedirectsList(data);
            var searchInput = container.find(selectors.searchField);  
            grid.focusSearchInput(searchInput);
        });
    };

    /**
    * Set values, returned from server to row fields
    */
    redirect.setRedirectFields = function(row, json) {
        if (json.Data) {
            row.find(selectors.pageUrl).html(json.Data.PageUrl);
            row.find(selectors.pageUrlEditor).val(json.Data.PageUrl);
            row.find(selectors.oldPageUrl).val(json.Data.PageUrl);
            
            row.find(selectors.redirectUrl).html(json.Data.RedirectUrl);
            row.find(selectors.redirectUrlEditor).val(json.Data.RedirectUrl);
            row.find(selectors.oldRedirectUrl).val(json.Data.RedirectUrl);
        }
    };
    
    /**
    * Retrieves redirect field values from table row.
    */
    redirect.getRedirectData = function(row) {
        var tagId = row.find(selectors.deleteLink).data('id'),
            tagVersion = row.find(selectors.deleteLink).data('version'),
            pageUrl = row.find(selectors.pageUrlEditor).val(),
            redirectUrl = row.find(selectors.redirectUrlEditor).val();

        return {
            Id: tagId,
            Version: tagVersion,
            PageUrl: pageUrl,
            RedirectUrl: redirectUrl
        };
    };

    /**
    * Initializes redirects module.
    */
    redirect.init = function () {
        bcms.logger.debug('Initializing bcms.pages.redirects module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(redirect.init);
    
    return redirect;
});
