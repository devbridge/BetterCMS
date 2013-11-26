/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.media.imageeditor', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.jquery.jcrop', 'bcms.ko.extenders', 'bcms.tags'],
    function($, bcms, modal, siteSettings, forms, dynamicContent, jcrop, ko, tags) {
        'use strict';

        var imageEditor = {},
            selectors = {
                imageToEdit: ".bcms-croped-block img",
                imageVersionField: "#image-version-field",
                imageCaption: "#Caption",
                imageFileName: "#image-file-name",
                imageFileSize: "#image-file-size",
                imageAlignment: "input[name=ImageAlign]:checked",
                imageAlignmentControls: ".bcms-alignment-controls",

                imageEditorForm: 'form:first',

                imageSizeEditBoxWidth: "#image-width",
                imageSizeEditBoxHeight: "#image-height",
                
                imageTitleEditInput: "#bcms-image-title-editor",

                imageToCrop: ".bcms-croped-block img",

                selectableInputs: 'input.bcms-editor-selectable-field-box'
            },
            links = {
                imageEditorDialogUrl: null,
                imageEditorInsertDialogUrl: null
            },
            globalization = {
                imageEditorDialogTitle: null,
                imageEditorInsertDialogTitle: null,
                imageEditorInsertDialogAcceptButton: null,
                imageEditorUpdateFailureMessageTitle: null,
                imageEditorUpdateFailureMessageMessage: null,
                closeButtonTitle: null
            },
            constants = {
                maxHeightToFit: 557,
                maxWidthToFit: 839,
                jcropBackgroundColor: '#F5F5F5'
            },
            jCropApi = null;

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
                        contentAvailable: initImageEditorDialogEvents,
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
                        done: function () {
                            initInsertImageWithOptionsDialogEvents(dialog);
                        }
                    });
                },
                onAccept: function (dialog) {
                    var imageUrl = dialog.container.find(selectors.imageToEdit).attr("src"),
                        caption = dialog.container.find(selectors.imageCaption).val(),
                        align = dialog.container.find(selectors.imageAlignment).val();
                    dialog.close();
                    callback(image, imageUrl, caption, align);
                    return false;
                }
            });
        };

        /**
        * Calculates dimensions for image to fit to maximum allowable size
        */
        imageEditor.calculateImageDimensionsToFit = function (width, height, maxWidth, maxHeight, enlarge) {
            if (enlarge || height > maxHeight || width > maxWidth) {

                var scaleX = maxWidth > 0 ? width / maxWidth : 0,
                scaleY = maxHeight > 0 ? height / maxHeight : 0;

                if (scaleX > scaleY) {
                    width = maxWidth;
                    height = scaleX > 0 ? height / scaleX : 0;
                } else {
                    height = maxHeight;
                    width = scaleY > 0 ? width / scaleY : 0;
                }
            }

            return {
                width: width,
                height: height
            };
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

                self.save = function (element) {
                    if (self.onSave($(element))) {
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
                this.input.blur();
            };

            return TitleEditorViewModel;
        })(EditorBaseViewModel);

        /**
        * Image editor view model
        */
        var ImageEditorViewModel = (function (_super) {
            bcms.extendsClass(ImageEditorViewModel, _super);

            function ImageEditorViewModel(dialog, json, enableCrop) {
                _super.call(this);

                var self = this;

                self.dialog = dialog;
                self.enableCrop = enableCrop;

                self.widthInput = dialog.container.find(selectors.imageSizeEditBoxWidth);
                self.heightInput = dialog.container.find(selectors.imageSizeEditBoxHeight);
                self.image = dialog.container.find(selectors.imageToCrop);

                self.originalWidth = json.OriginalImageWidth;
                self.originalHeight = json.OriginalImageHeight;
                self.width = ko.observable(json.ImageWidth);
                self.height = ko.observable(json.ImageHeight);
                self.oldWidth = ko.observable(json.ImageWidth);
                self.oldHeight = ko.observable(json.ImageHeight);
                self.cropHeight = ko.observable(json.CroppedHeight);
                self.cropWidth = ko.observable(json.CroppedWidth);
                self.fit = ko.observable(false);
                self.calculatedWidth = ko.observable(json.ImageWidth);
                self.calculatedHeight = ko.observable(json.ImageHeight);
                self.cropCoordX1 = ko.observable(json.CropCoordX1);
                self.cropCoordX2 = ko.observable(json.CropCoordX2);
                self.cropCoordY1 = ko.observable(json.CropCoordY1);
                self.cropCoordY2 = ko.observable(json.CropCoordY2);
                self.url = json.OriginalImageUrl;

                // Recalculate image dimensions on image change
                self.fit.subscribe(function () {
                    recalculate();
                });
                self.oldWidth.subscribe(function () {
                    recalculate();
                });
                self.oldHeight.subscribe(function () {
                    recalculate();
                });
                self.widthAndHeight = ko.computed(function () {
                    return self.oldWidth() + ' x ' + self.oldHeight();
                });

                self.changeHeight = function() {
                    if (self.widthInput.valid() && self.oldWidth != self.width()) {
                        var ratio = self.width() / (self.originalWidth || 1);

                        self.height(Math.round(self.originalHeight * ratio));
                    }
                    
                    return true;
                };
                
                self.changeWidth = function () {
                    if (self.heightInput.valid() && self.oldHeight != self.height()) {
                        var ratio = self.height() / (self.originalHeight || 1);

                        self.width(Math.round(self.originalWidth * ratio));
                    }
                    
                    return true;
                };

                self.calculateWidth = ko.computed(function () {
                    if (self.fit() && self.width() > constants.maxWidthToFit) {
                        return constants.maxWidthToFit;
                    }
                    return self.width();
                });

                self.calculateHeight = ko.computed(function () {
                    if (self.fit() && self.height() > constants.maxHeightToFit) {
                        return constants.maxHeightToFit;
                    }
                    return self.height();
                });

                self.restoreOriginalSize = function() {
                    self.width(self.originalWidth);
                    self.height(self.originalHeight);
                };

                self.onCropCoordsUpdated = function (coords) {
                    if (coords != null) {
                        self.cropCoordX1(coords.x);
                        self.cropCoordY1(coords.y);
                        self.cropCoordX2(coords.x2);
                        self.cropCoordY2(coords.y2);

                        recalculateCroppedDimensions();
                    } else {
                        self.removeCrop();
                    }
                };

                self.hasCrop = ko.computed(function () {
                    return self.cropCoordX1() != 0
                        || self.cropCoordY1() != 0
                        || self.cropCoordX2() != self.originalWidth
                        || self.cropCoordY2() != self.originalHeight;
                });

                self.croppedWidthAndHeight = ko.computed(function () {
                    return self.cropWidth() + ' x ' + self.cropHeight();
                });

                self.removeCrop = function () {
                    self.cropCoordX1(0);
                    self.cropCoordY1(0);
                    self.cropCoordX2(self.originalWidth);
                    self.cropCoordY2(self.originalHeight);

                    recalculateCroppedDimensions();

                    addCropper();
                };

                self.changeFit = function() {
                    self.fit(!self.fit());
                };
                
                function recalculateCroppedDimensions() {
                    var width = self.oldWidth(),
                        height = self.oldHeight(),
                        originalWidth = self.originalWidth,
                        originalHeight = self.originalHeight,
                        x1 = self.cropCoordX1(),
                        x2 = self.cropCoordX2(),
                        y1 = self.cropCoordY1(),
                        y2 = self.cropCoordY2(),
                        x, y, ratio;
                    
                    if (self.hasCrop()) {
                        if (width != originalWidth && originalWidth) {
                            ratio = width / originalWidth;

                            x = parseInt(Math.floor(x2 * ratio)) - parseInt(Math.floor(x1 * ratio));
                        } else {
                            x = Math.floor(x2) - Math.floor(x1);
                        }
                        
                        if (height != originalHeight && originalHeight) {
                            ratio = height / originalHeight;

                            y = parseInt(Math.floor(y2 * ratio)) - parseInt(Math.floor(y1 * ratio));
                        } else {
                            y = Math.floor(y2) - Math.floor(y1);
                        }
                    } else {
                        x = self.oldWidth();
                        y = self.oldHeight();
                    }

                    self.cropWidth(x);
                    self.cropHeight(y);
                }

                function recalculate() {
                    var calcWidth = self.oldWidth(),
                        calcHeight = self.oldHeight(),
                        resized;

                    if (self.fit()) {

                        resized = imageEditor.calculateImageDimensionsToFit(calcWidth, calcHeight, constants.maxWidthToFit, constants.maxHeightToFit);
                        calcWidth = resized.width;
                        calcHeight = resized.height;
                    }
                    
                    self.calculatedWidth(calcWidth);
                    self.calculatedHeight(calcHeight);

                    if (jCropApi != null) {
                        addCropper();
                    }
                    
                    recalculateCroppedDimensions();
                }
                
                function initialize() {
                    destroyJCrop();
                    jCropApi = null;

                    self.image.load(function () {
                        self.fit(true);
                        addCropper();
                    });
                    self.image.attr('src', self.url);
                }

                function addCropper() {
                    if (!self.enableCrop) {
                        return;
                    }
                    
                    destroyJCrop();
                    
                    var cropperOptions = {
                        onChange: self.onCropCoordsUpdated,
                        onSelect: self.onCropCoordsUpdated,
                        onRelease: self.onCropCoordsUpdated,
                        trueSize: [self.originalWidth, self.originalHeight],
                        bgColor: constants.jcropBackgroundColor
                    };
                    if (self.hasCrop()) {
                        cropperOptions.setSelect = [self.cropCoordX1(), self.cropCoordY1(), self.cropCoordX2(), self.cropCoordY2()];
                    }

                    setTimeout(function() {
                        self.image.Jcrop(cropperOptions, function() {
                            jCropApi = this;
                        });
                    }, 10);
                }
                
                function destroyJCrop() {
                    if (jCropApi != null) {
                        jCropApi.destroy();
                    }
                }

                initialize();
            }

            ImageEditorViewModel.prototype.onSave = function (element) {
                // Call recalculation
                if (element.get(0) == this.widthInput.get(0)) {
                    this.changeHeight();
                } else if (element.get(0) == this.heightInput.get(0)) {
                    this.changeWidth();
                }
                
                if (this.widthInput.valid() && this.heightInput.valid()) {
                    this.oldWidth(this.width());
                    this.oldHeight(this.height());

                    return true;
                }

                return false;
            };

            ImageEditorViewModel.prototype.onClose = function () {
                this.width(this.oldWidth());
                this.height(this.oldHeight());
                this.heightInput.blur();
                this.widthInput.blur();
            };
            
            return ImageEditorViewModel;
        })(EditorBaseViewModel);

        /**
        * Image edit form view model
        */
        function ImageEditViewModel(titleEditorViewModel, imageEditorViewModel, tagsViewModel) {
            var self = this;

            self.titleEditorViewModel = titleEditorViewModel;
            self.imageEditorViewModel = imageEditorViewModel;
            self.tags = tagsViewModel;
        }

        /**
        * Initializes ImageEditor dialog events.
        */
        function initImageEditorDialogEvents(dialog, content) {

            var data = content.Data ? content.Data : { };

            // Create view models for editor boxes and for form
            var titleEditorViewModel = new TitleEditorViewModel(dialog, data.Title);
            
            var imageEditorViewModel = new ImageEditorViewModel(dialog, data, true);

            var tagsEditorViewModel = new tags.TagsListViewModel(content.Data.Tags);
            
            var viewModel = new ImageEditViewModel(titleEditorViewModel, imageEditorViewModel, tagsEditorViewModel);
            ko.applyBindings(viewModel, dialog.container.find(selectors.imageEditorForm).get(0));

            // Image alignment
            dialog.container.find(selectors.imageAlignmentControls).children().each(function () {
                setImageAlignment(this, dialog);
            });

            dialog.container.find(selectors.selectableInputs).on('click', function () {
                this.select();
            });
        }

        /**
        * Initializes inserting image options dialog events.
        */
        function initInsertImageWithOptionsDialogEvents(dialog) {
            // Image alignment
            dialog.container.find(selectors.imageAlignmentControls).children().each(function() {
                setImageAlignment(this, dialog);
            });
            
            // Select publis URL
            dialog.container.find(selectors.selectableInputs).on('click', function () {
                this.select();
            });
        }

        /**
        * Changes CSS class for selected image alignment type
        */
        function setImageAlignment(item, dialog) {
            $(item).on('click', function() {
                dialog.container.find(selectors.imageAlignmentControls).children().each(function() {
                    $(this).attr('class', $(this).attr('class').replace('-active', ''));
                    $('input', this).removeAttr('checked');
                });
                $(item).attr('class', $(item).attr('class') + '-active');
                $('input', item).attr('checked', 'true');
            });
        }

        return imageEditor;
    });