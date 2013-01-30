/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.media.imageeditor', ['jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'jquery.Jcrop', 'knockout'],
    function($, bcms, modal, siteSettings, forms, dynamicContent, jcrop, ko) {
        'use strict';

        var imageEditor = {},
            selectors = {
                // imageEditLink: ".bcms-btn-main",
                imageToEdit: ".bcms-croped-block img",
                imageVersionField: "#image-version-field",
                imageCaption: "#Caption",
                imageFileName: "#image-file-name",
                imageFileSize: "#image-file-size",
                imageDimensions: "#image-dimensions",
                imageAlignment: "input[name=ImageAlign]:checked",
                imageAlignmentControls: ".bcms-alignment-controls",

                imageEditorForm: 'form:first',

                imageSizeEditBoxWidth: "#image-width",
                imageSizeEditBoxHeight: "#image-height",
                
                imageTitleEditInput: "#bcms-image-title-editor",

                imageToCrop: ".bcms-crop-image-block img",
                imageToCropCoordX1: ".bcms-crop-image-x1",
                imageToCropCoordX2: ".bcms-crop-image-x2",
                imageToCropCoordY1: ".bcms-crop-image-y1",
                imageToCropCoordY2: ".bcms-crop-image-y2",
            },
            links = {
                imageEditorDialogUrl: null,
                imageEditorInsertDialogUrl: null,
                imageEditorCroppingDialogUrl: null,
                imageResizeUrl: null,
            },
            globalization = {
                imageEditorDialogTitle: null,
                imageEditorInsertDialogTitle: null,
                imageEditorInsertDialogAcceptButton: null,
                imageEditorUpdateFailureMessageTitle: null,
                imageEditorUpdateFailureMessageMessage: null,
                imageEditorResizeFailureMessageTitle: null,
                imageEditorResizeFailureMessageMessage: null,
                imageEditorCroppingDialogTitle: null,
                imageEditorCropFailureMessageTitle: null,
                imageEditorCropFailureMessageMessage: null,
            };

        /**
        * Assign objects to module.
        */
        imageEditor.links = links;
        imageEditor.globalization = globalization;

        /**
        * Called when image editing needed.
        */
        imageEditor.onEditImage = function (imageId, callback) {
            imageEditor.showImageEditorDialog(imageId, callback);
        };

        /**
        * Called when image insert needed.
        */
        imageEditor.onInsertImage = function (image, callback) {
            imageEditor.showImageEditorInsertDialog(image, callback);
        };

        /**
        * Show image editor dialog.
        */
        imageEditor.showImageEditorDialog = function (imageId, callback) {
            modal.open({
                title: globalization.imageEditorDialogTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.imageEditorDialogUrl, imageId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: imageEditor.initImageEditorDialogEvents,
                        beforePost: function () {
                            var newVersion = $(selectors.imageToEdit).data('version');
                            if (newVersion > 0) {
                                $(selectors.imageVersionField).val(newVersion);
                            }
                        },
                        postSuccess: function (json) {
                            if (json.Success) {
                                callback(json.Data);
                            }
                            dialog.close();
                        },
                        postError: function() {
                            modal.alert({
                                title: globalization.imageEditorUpdateFailureMessageTitle,
                                content: globalization.imageEditorUpdateFailureMessageMessage,
                            });
                        }
                    });
                }
            });
        };

        /**
        * Show image insert editor dialog.
        */
        imageEditor.showImageEditorInsertDialog = function (image, callback) {
            modal.open({
                title: globalization.imageEditorInsertDialogTitle,
                acceptTitle: globalization.imageEditorInsertDialogAcceptButton,
                onLoad: function (dialog) {
                    var url = $.format(links.imageEditorInsertDialogUrl, image.id());
                    dynamicContent.setContentFromUrl(dialog, url, {
                        done: function (content) {
                            // NOTE: attach events if needed.
                        },
                    });
                },
                onAcceptClick: function (dialog) {
                    var imageUrl = dialog.container.find(selectors.imageToEdit).attr("src"),
                        caption = dialog.container.find(selectors.imageCaption).val(),
                        align = dialog.container.find(selectors.imageAlignment).val();
                    dialog.close();
                    callback(image, imageUrl, caption, align);
                }
            });
        };

        /**
        * Editor base view model
        */
        var EditorBaseViewModel = (function () {
            function EditorBaseViewModel() {
                var self = this;

                self.isOpened = ko.observable(false);
                
                self.open = function () {
                    self.isOpened(true);
                };

                self.close = function () {
                    self.onClose();
                    self.isOpened(false);
                };

                self.save = function () {
                    if (self.onSave()) {
                        self.isOpened(false);
                    }
                };
            }
            
            EditorBaseViewModel.prototype.onSave = function () { };
            
            EditorBaseViewModel.prototype.onClose = function () { };

            return EditorBaseViewModel;
        })();
        
        /**
        * Title editor view model
        */
        var TitleEditorViewModel = (function (_super) {
            bcms.extendsClass(TitleEditorViewModel, _super);

            function TitleEditorViewModel(dialog, title) {
                _super.call(this);

                var self = this;

                self.input = dialog.container.find(selectors.imageTitleEditInput);
                
                self.title = ko.observable(title);
                self.oldTitle = ko.observable(title);
            }

            TitleEditorViewModel.prototype.onSave = function () {
                if (this.input.valid()) {
                    this.oldTitle(this.title());

                    return true;
                }

                return false;
            };

            TitleEditorViewModel.prototype.onClose = function () {
                this.title(this.oldTitle());
            };

            return TitleEditorViewModel;
        })(EditorBaseViewModel);

        /**
        * Dimension editor view model
        */
        var DimensionEditorViewModel = (function (_super) {
            bcms.extendsClass(DimensionEditorViewModel, _super);

            function DimensionEditorViewModel(dialog, widht, height) {
                _super.call(this);

                var self = this;

                self.dialog = dialog;

                self.widthInput = dialog.container.find(selectors.imageSizeEditBoxWidth);
                self.heightInput = dialog.container.find(selectors.imageSizeEditBoxWidth);

                self.width = ko.observable(widht);
                self.height = ko.observable(height);
                self.oldWidth = ko.observable(widht);
                self.oldHeight = ko.observable(height);

                self.widthAndHeight = ko.computed(function () {
                    return self.oldWidth() + ' x ' + self.oldHeight();
                });
            }

            DimensionEditorViewModel.prototype.onSave = function () {
                if (this.widthInput.valid() && this.heightInput.valid() /*&& imageEditor.onImageResize(this.dialog) === true*/) {
                    this.oldWidth(this.width());
                    this.oldHeight(this.height());

                    return true;
                }

                return false;
            };

            DimensionEditorViewModel.prototype.onClose = function () {
                this.width(this.oldWidth());
                this.height(this.oldHeight());
            };
            
            return DimensionEditorViewModel;
        })(EditorBaseViewModel);

        /**
        * Image edit form view model
        */
        function ImageEditViewModel(titleEditor, dimensionEditor) {
            var self = this;

            self.titleEditor = titleEditor;
            self.dimensionEditor = dimensionEditor;
        }

        /**
        * Initializes ImageEditor dialog events.
        */
        imageEditor.initImageEditorDialogEvents = function (dialog) {

            // Create view models for editor boxes and for form
            var titleEditor = new TitleEditorViewModel(dialog, dialog.container.find(selectors.imageTitleEditInput).val());
            
            var dimensionEditor = new DimensionEditorViewModel(dialog,
                dialog.container.find(selectors.imageSizeEditBoxWidth).val(),
                dialog.container.find(selectors.imageSizeEditBoxHeight).val());
            
            var viewModel = new ImageEditViewModel(titleEditor, dimensionEditor);
            ko.applyBindings(viewModel, dialog.container.find(selectors.imageEditorForm).get(0));

            /*dialog.container.find(selectors.imageEditLink).on('click', function () {
                imageEditor.showImageCroppingDialog(dialog);
            });*/

            dialog.container.find(selectors.imageToEdit).on('click', function () {
                var img = $(this),
                    src = img.attr('src');
                if (src) {
                    modal.imagePreview(src, img.attr('alt'));
                }
            });

            // Image alignment
            dialog.container.find(selectors.imageAlignmentControls).children().each(function () {
                var item = this;
                $(item).on('click', function () {
                    dialog.container.find(selectors.imageAlignmentControls).children().each(function () {
                        $(this).attr('class', $(this).attr('class').replace('-active', ''));
                        $('input', this).removeAttr('checked');
                    });
                    $(item).attr('class', $(item).attr('class') + '-active');
                    $('input', item).attr('checked', 'true');
                });
            });
        };

        /**
        * Resize image.
        */
        imageEditor.onImageResize = function (editorDialog) {
            var id = $(selectors.imageToEdit).data('id'),
                width = $(selectors.imageSizeEditBoxWidth).val(),
                height = $(selectors.imageSizeEditBoxHeight).val(),
                version = $(selectors.imageToEdit).data('version'),
                url = $.format(links.imageResizeUrl, id, width, height, version),
                onComplete = function(json) {
                    if (json.Success) {
                        imageEditor.updateImage(editorDialog, json.Data);
                    } else {
                        modal.alert({
                            title: globalization.imageEditorResizeFailureMessageTitle,
                            content: globalization.imageEditorResizeFailureMessageMessage,
                        });
                    }
                };

            $.ajax({
                type: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            })
            .done(function (result) {
                onComplete(result);
            })
            .fail(function (response) {
                onComplete(bcms.parseFailedResponse(response));
            });
            return true;
        };

        /**
        * Show image cropping dialog.
        */
        /*imageEditor.showImageCroppingDialog = function (editorDialog) {
            var imageId = editorDialog.container.find(selectors.imageToEdit).data('id');
            modal.open({
                title: globalization.imageEditorCroppingDialogTitle,
                onLoad: function (cropperDialog) {
                    var url = $.format(links.imageEditorCroppingDialogUrl, imageId),
                        onFail = function() {
                            modal.alert({
                                title: globalization.imageEditorCropFailureMessageTitle,
                                content: globalization.imageEditorCropFailureMessageMessage,
                                onClose: function () {
                                    cropperDialog.close();
                                }
                            });
                        };

                    dynamicContent.bindDialog(cropperDialog, url, {
                        contentAvailable: imageEditor.initImageCroppingDialogEvents,
                        postSuccess: function (json) {
                            if (json.Success) {
                                imageEditor.updateImage(editorDialog, json.Data);
                            } else {
                                onFail();
                            }
                        },
                        postError: onFail
                    });
                }
            });
        };*/

        /**
        * Update image and properties.
        */
        imageEditor.updateImage = function(editorDialog, data) {
            editorDialog.container.find(selectors.imageToEdit).attr('src', data.Url + '?version=' + Math.random());
            editorDialog.container.find(selectors.imageToEdit).data('version', data.Version);
            editorDialog.container.find(selectors.imageFileName).text(data.FileName + '.' + data.FileExtension);
            editorDialog.container.find(selectors.imageFileSize).text(data.FileSize);
        };

        /**
        * Initializes ImageCropping dialog events.
        */
        imageEditor.initImageCroppingDialogEvents = function (dialog) {
            var x1Input = dialog.container.find(selectors.imageToCropCoordX1),
                y1Input = dialog.container.find(selectors.imageToCropCoordY1),
                x2Input = dialog.container.find(selectors.imageToCropCoordX2),
                y2Input = dialog.container.find(selectors.imageToCropCoordY2),
                onCropCoordsUpdated = function(coords) {
                    x1Input.val(Math.round(coords.x));
                    y1Input.val(Math.round(coords.y));
                    x2Input.val(Math.round(coords.x2));
                    y2Input.val(Math.round(coords.y2));
                };

            var image = dialog.container.find(selectors.imageToCrop);
            image.load(function() {
                image.Jcrop({
                    onChange: onCropCoordsUpdated,
                    onSelect: onCropCoordsUpdated,
                    trueSize: [image[0].naturalWidth, image[0].naturalHeight],
                    setSelect: [x1Input.val(), y1Input.val(), x2Input.val(), y2Input.val()],
                });
            });
        };

        /**
        * Initializes page module.
        */
        imageEditor.init = function() {
            console.log('Initializing bcms.media.imageeditor module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(imageEditor.init);

        return imageEditor;
    });