/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.properties', ['jquery', 'bcms', 'bcms.modal', 'bcms.forms', 'bcms.dynamicContent'], function ($, bcms, modal, forms, dynamicContent) {
    'use strict';

    var page = {},
        selectors = {
            editPagePropertiesCloseInfoMessageBox: '.bcms-info-message-box',

            editPermalink: '#bcms-pageproperties-editpermalink',
            editPermalinkBox: '.bcms-edit-urlpath-box',
            editPermalinkClose: 'div.bcms-edit-urlpath-box .bcms-edit-btn-close, div.bcms-edit-urlpath-box .bcms-btn-links-small',
            editPermalinkSave: '#bcms-save-permalink',
            permalinkHiddenField: '#bcms-page-permalink',
            permalinkEditField: '#bcms-page-permalink-edit',
            permalinkInfoField: '#bcms-page-permalink-info',

            addTagField: '.bcms-add-tags-field',
            addTagTemplate: '#bcms-add-page-tag-template',
            addedTagClass: '.bcms-single-tag',
            tagsContainer: '#bcms-tags-container',
            tagRemoveLink: '.bcms-single-tag a',
            tagHiddenValue: '.bcms-single-tag-hidden-value',
            tagHiddenIndex: '.bcms-single-tag-hidden-indexer',

            addCategoryField: '#bcms-add-categories-field',
            pagePropertiesTemplateSelect: '.bcms-btn-grid',
            pagePropertiesTemplateId: '#TemplateId',

            addCategoryTemplate: '#bcms-add-page-category-template',
            addedCategoryClass: '.bcms-single-tag',
            categoryHiddenValue: '.bcms-single-category-hidden-value',
            categoryHiddenIndex: '.bcms-single-category-hidden-indexer',
            categoriesContainer: '#bcms-categories-container',

            buttonTagExpand: '.bcms-tag-btn-expand',
            buttonCategoryExpand: '.bcms-categories-btn-expand',
            tagsInputHolder: '.bcms-tags-field-holder',

            pagePropertiesActiveTemplateBox: '.bcms-grid-box-active',
            pagePropertiesTemplateBox: '.bcms-grid-box',
            pagePropertiesActiveTemplateMessage: '.bcms-grid-active-message-text',

            pagePropertiesForm: 'form:first',

            firstInvalidFormInput: ':input.input-validation-error:first',
            firstInvalidFormInputParentTab: '.bcms-tab-single',
            firstInvalidFormInputParentTabClick: '.bcms-tab[href="#{0}"]'

        },
        links = {
            loadEditPropertiesDialogUrl: null
        },
        globalization = {
            editPagePropertiesModalTitle: null,
            tagExpandTitle: null,
            tagCollapse: null
        },
        keys = {
            editPagePropertiesInfoMessageClosed: 'bcms.EditPagePropertiesInfoBoxClosed'
        },
        classes = {
            pagePropertiesActiveTemplateBox: 'bcms-grid-box-active'
        },
        pagePropertiesTagCounter = 0,
        pagePropertiesCategoryCounter = 0;

    /**
    * Assign objects to module.
    */
    page.links = links;
    page.globalization = globalization;

    /*
   * Remove page tag or category from PageProperties dialog.
   */
    function removeTag() {
        $(this).closest(selectors.addedTagClass).remove();
    }

    /*
   * Add page tag in PageProperties dialog.
   */
    function addNewTag(dialog) {
        var template = dialog.container.find(selectors.addTagTemplate),
            templateBlock = $(template.html()),
            removeLink = templateBlock.find('a'),
            hiddenTemplateInput = templateBlock.find(selectors.tagHiddenValue),
            hiddenTemplateIndexer = templateBlock.find(selectors.tagHiddenIndex),
            tagField = dialog.container.find(selectors.addTagField),
            tagName = tagField.val(),
            tagsHolder = dialog.container.find(selectors.tagsContainer),
            tagNames = [];

        if (tagName) {
            tagsHolder.find(selectors.addedTagClass).each(function (index, value) {
                tagNames.push($(value).data('name').toLowerCase());
            });

            if ($.inArray(tagName.toLowerCase(), tagNames) < 0) {
                removeLink.before(tagName);
                removeLink.on('click', removeTag);
                templateBlock.attr('data-name', tagName);
                tagsHolder.append(templateBlock);

                hiddenTemplateInput.attr('id', hiddenTemplateInput.attr('id').replace("0", pagePropertiesTagCounter));
                hiddenTemplateInput.attr('name', hiddenTemplateInput.attr('name').replace("0", pagePropertiesTagCounter));
                hiddenTemplateInput.val(tagName);

                hiddenTemplateIndexer.val(pagePropertiesTagCounter);

                pagePropertiesTagCounter = pagePropertiesTagCounter + 1;
            }

            tagField.val('');
        }
    }

    /*
    * Add new category
    */
    function addNewCategory(dialog) {
        var template = dialog.container.find(selectors.addCategoryTemplate),
            templateBlock = $(template.html()),
            removeLink = templateBlock.find('a'),
            hiddenTemplateInput = templateBlock.find(selectors.categoryHiddenValue),
            hiddenTemplateIndexer = templateBlock.find(selectors.categoryHiddenIndex),
            categoryField = dialog.container.find(selectors.addCategoryField),
            categoryName = categoryField.val(),
            categoriesHolder = dialog.container.find(selectors.categoriesContainer),
            categories = [];

        if (categoryName) {
            categoriesHolder.find(selectors.addedCategoryClass).each(function (index, value) {
                categories.push($(value).data('name').toLowerCase());
            });

            if ($.inArray(categoryName.toLowerCase(), categories) < 0) {
                removeLink.before(categoryName);
                removeLink.on('click', removeTag);
                templateBlock.attr('data-name', categoryName);
                categoriesHolder.append(templateBlock);

                hiddenTemplateInput.attr('id', hiddenTemplateInput.attr('id').replace("0", pagePropertiesCategoryCounter));
                hiddenTemplateInput.attr('name', hiddenTemplateInput.attr('name').replace("0", pagePropertiesCategoryCounter));
                hiddenTemplateInput.val(categoryName);

                hiddenTemplateIndexer.val(pagePropertiesCategoryCounter);

                pagePropertiesCategoryCounter = pagePropertiesCategoryCounter + 1;
            }

            categoryField.val('');
        }
    }
    
    /**
    * Initializes EditPageProperties dialog events.
    */
    page.initEditPagePropertiesDialogEvents = function (dialog) {
        dialog.container.find(selectors.editPermalink).on('click', function () {
            page.showPagePropertiesEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.editPermalinkClose).on('click', function () {
            page.closePagePropertiesEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.editPermalinkSave).on('click', function () {
            page.savePagePropertiesEditPermalinkBox(dialog);
        });

        dialog.container.find(selectors.pagePropertiesForm).on('submit', function () {
            if (!dialog.container.find(selectors.permalinkEditField).valid()) {
                page.showPagePropertiesEditPermalinkBox(dialog);
            }
        });

        var infoMessageClosed = localStorage.getItem(keys.editPagePropertiesInfoMessageClosed);
        if (infoMessageClosed && infoMessageClosed === '1') {
            page.hideEditPagePropertiesInfoMessage(dialog);
        } else {
            dialog.container.find(selectors.editPagePropertiesCloseInfoMessageBox).on('click', function () {
                localStorage.setItem(keys.editPagePropertiesInfoMessageClosed, '1');
                page.hideEditPagePropertiesInfoMessage(dialog);
            });
        }

        dialog.container.find(selectors.pagePropertiesTemplateSelect).on('click', function () {
            page.highlightPagePropertiesActiveTemplate(dialog, this);
        });

        dialog.container.find(selectors.addTagField).on('blur', function () {
            addNewTag(dialog);
        });

        dialog.container.find(selectors.addCategoryField).on('blur', function () {
            addNewCategory(dialog);
        });

        dialog.container.find(selectors.buttonTagExpand).on('click', function () {
            var button = $(this),
                tagsContainer = button.siblings(selectors.tagsInputHolder),
                visible = tagsContainer.is(':visible');

            if (visible) {
                tagsContainer.hide();
                button.html(globalization.tagExpandTitle);
            } else {
                tagsContainer.show();
                button.html(globalization.tagCollapse);
            }
        });
        
        dialog.container.find(selectors.buttonCategoryExpand).on('click', function () {
            var button = $(this),
                tagsContainer = button.siblings(selectors.tagsInputHolder),
                visible = tagsContainer.is(':visible');

            if (visible) {
                tagsContainer.hide();
                button.html(globalization.tagExpandTitle);
            } else {
                tagsContainer.show();
                button.html(globalization.tagCollapse);
            }
        });

        dialog.container.find(selectors.tagRemoveLink).on('click', removeTag);

        bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.permalinkEditField), {
            preventedEnter: function () {
                dialog.container.find(selectors.permalinkEditField).blur();
                page.savePagePropertiesEditPermalinkBox(dialog);
            },
            preventedEsc: function () {
                dialog.container.find(selectors.permalinkEditField).blur();
                page.closePagePropertiesEditPermalinkBox(dialog);
            }
        });

        bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.addTagField), {
            preventedEnter: function () {
                dialog.container.find(selectors.addTagField).blur();
            },
            preventedEsc: function () {
                $(selectors.addTagField).val('');
                dialog.container.find(selectors.addTagField).blur();
            }
        });

        bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.addCategoryField), {
            preventedEnter: function () {
                dialog.container.find(selectors.addCategoryField).blur();
            },
            preventedEsc: function () {
                $(selectors.addCategoryField).val('');
                dialog.container.find(selectors.addCategoryField).blur();
            }
        });

        pagePropertiesTagCounter = $(selectors.tagsContainer).find(selectors.addedTagClass).length;
        pagePropertiesCategoryCounter = $(selectors.categoriesContainer).find(selectors.addedCategoryClass).length;
    };

    /**
    * Shows edit permalink box in PageProperties dialog.
    */
    page.showPagePropertiesEditPermalinkBox = function (dialog) {
        dialog.container.find(selectors.editPermalinkBox).show();
        dialog.container.find(selectors.editPermalink).hide();
        dialog.container.find(selectors.permalinkEditField).focus();
    };

    /**
    * Sets changed permalink value in PageProperties dialog
    */
    page.savePagePropertiesEditPermalinkBox = function (dialog) {
        if ($(selectors.permalinkEditField).valid()) {
            var value = dialog.container.find(selectors.permalinkEditField).val();
            dialog.container.find(selectors.permalinkHiddenField).val(value);
            dialog.container.find(selectors.permalinkInfoField).html(value ? value : "&nbsp;");

            page.hidePagePropertiesEditPermalinkBox(dialog);
        }
    };

    /*
    * Closes edit permalink box in PageProperties dialog.
    */
    page.closePagePropertiesEditPermalinkBox = function (dialog) {
        var value = dialog.container.find(selectors.permalinkHiddenField).val();
        dialog.container.find(selectors.permalinkEditField).val(value);

        page.hidePagePropertiesEditPermalinkBox(dialog);
    };

    /**
    * Hides edit permalink box in PageProperties dialog.
    */
    page.hidePagePropertiesEditPermalinkBox = function (dialog) {
        dialog.container.find(selectors.editPermalinkBox).hide();
        dialog.container.find(selectors.editPermalink).show();
    };

    /**
    * Hides info message box in EditPageProperties dialog.
    */
    page.hideEditPagePropertiesInfoMessage = function (dialog) {
        dialog.container.find(selectors.editPagePropertiesCloseInfoMessageBox).hide();
    };

    /**
    * highlights active template box in PageProperties dialog.
    */
    page.highlightPagePropertiesActiveTemplate = function (dialog, selectButton) {
        var active = dialog.container.find(selectors.pagePropertiesActiveTemplateBox),
            template = $(selectButton).parents(selectors.pagePropertiesTemplateBox);

        active.removeClass(classes.pagePropertiesActiveTemplateBox);
        active.find(selectors.pagePropertiesTemplateSelect).show();
        active.find(selectors.pagePropertiesActiveTemplateMessage).hide();

        if (template) {
            dialog.container.find(selectors.pagePropertiesTemplateId).val($(template).data('id'));
            $(template).addClass(classes.pagePropertiesActiveTemplateBox);
            $(template).find(selectors.pagePropertiesActiveTemplateMessage).show();
        }

        $(selectButton).hide();
    };

    /**
    * Opens modal window for given page with page properties
    */
    page.openEditPageDialog = function (id, postSuccess) {
        modal.open({
            title: globalization.editPagePropertiesModalTitle,
            onLoad: function (dialog) {
                var url = $.format(links.loadEditPropertiesDialogUrl, id);
                dynamicContent.bindDialog(dialog, url, {
                    contentAvailable: page.initEditPagePropertiesDialogEvents,

                    beforePost: function () {
                        if (!dialog.container.find(selectors.permalinkEditField).valid()) {
                            page.showAddNewPageEditPermalinkBox(dialog);
                        }
                    },

                    postSuccess: postSuccess
                });
            }
        });
    };

    /**
    * Opens modal window for current page with page properties
    */
    page.editPageProperties = function () {
        page.openEditPageDialog(bcms.pageId, function (data) {
            // Redirect
            if (data.Data && data.Data.PageUrl) {
                window.location.href = data.Data.PageUrl;
            }
        });
    };

    /**
    * Initializes page module.
    */
    page.init = function () {
        console.log('Initializing bcms.pages.properties module.');
    };

    /**
    * Register initialization
    */
    bcms.registerInit(page.init);

    return page;
});
