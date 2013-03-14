/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.media.imageeditor', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.jquery.Jcrop', 'bcms.ko.extenders'],
    function($, bcms, modal, siteSettings, forms, dynamicContent, jcrop, ko) {
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

                imageToCrop: ".bcms-croped-block img"
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
                imageEditorUpdateFailureMessageMessage: null
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
                        },
                        postError: function () {
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

            function ImageEditorViewModel(dialog, json) {
                _super.call(this);

                var self = this;

                self.dialog = dialog;

                self.widthInput = dialog.container.find(selectors.imageSizeEditBoxWidth);
                self.heightInput = dialog.container.find(selectors.imageSizeEditBoxHeight);
                self.image = dialog.container.find(selectors.imageToCrop);

                self.originalWidth = json.OriginalImageWidth;
                self.originalHeight = json.OriginalImageHeight;
                self.width = ko.observable(json.ImageWidth);
                self.height = ko.observable(json.ImageHeight);
                self.oldWidth = ko.observable(json.ImageWidth);
                self.oldHeight = ko.observable(json.ImageHeight);
                self.fit = ko.observable(false);
                self.calculatedWidth = ko.observable(json.ImageWidth);
                self.calculatedHeight = ko.observable(json.ImageHeight);
                self.cropCoordX1 = ko.observable(json.CropCoordX1);
                self.cropCoordX2 = ko.observable(json.CropCoordX2);
                self.cropCoordY1 = ko.observable(json.CropCoordY1);
                self.cropCoordY2 = ko.observable(json.CropCoordY2);
                self.keepAspectRatio = ko.observable(false);
                self.url = json.OriginalImageUrl + '?version=' + json.Version;

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
                self.keepAspectRatio.subscribe(function () {
                    self.changeHeight();
                });

                self.widthAndHeight = ko.computed(function () {
                    return self.oldWidth() + ' x ' + self.oldHeight();
                });

                self.changeHeight = function() {
                    if (self.keepAspectRatio() && self.widthInput.valid() && self.oldWidth != self.width()) {
                        var ratio = self.width() / (self.originalWidth || 1);

                        self.height(Math.round(self.originalHeight * ratio));
                    }
                    
                    return true;
                };
                
                self.changeWidth = function () {
                    if (self.keepAspectRatio() && self.heightInput.valid() && self.oldHeight != self.height()) {
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
                    self.save();
                };

                self.onCropCoordsUpdated = function (coords) {
                    if (coords != null) {
                        self.cropCoordX1(coords.x);
                        self.cropCoordY1(coords.y);
                        self.cropCoordX2(coords.x2);
                        self.cropCoordY2(coords.y2);
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

                self.removeCrop = function () {
                    self.cropCoordX1(0);
                    self.cropCoordY1(0);
                    self.cropCoordX2(self.originalWidth);
                    self.cropCoordY2(self.originalHeight);

                    addCropper();
                };

                self.changeFit = function() {
                    self.fit(!self.fit());
                };
                
                self.changeAspectRatio = function () {
                    self.keepAspectRatio(!self.keepAspectRatio());
                };

                function recalculate() {
                    var calcWidth = self.oldWidth(),
                        calcHeight = self.oldHeight(),
                        scaleX = constants.maxWidthToFit > 0 ? calcWidth / constants.maxWidthToFit : 0,
                        scaleY = constants.maxHeightToFit > 0 ? calcHeight / constants.maxHeightToFit : 0;

                    if (self.fit() && (calcHeight > constants.maxHeightToFit || calcWidth > constants.maxWidthToFit)) {
                        
                        if (scaleX > scaleY) {
                            calcWidth = constants.maxWidthToFit;
                            calcHeight = scaleX > 0 ? calcHeight / scaleX : 0;
                        } else {
                            calcHeight = constants.maxHeightToFit;
                            calcWidth = scaleY > 0 ? calcWidth / scaleY : 0;
                        }
                    }
                    
                    self.calculatedWidth(calcWidth);
                    self.calculatedHeight(calcHeight);

                    if (jCropApi != null) {
                        addCropper();
                    }
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
                // Call recalculation, if "keep aspect ratio" is checked
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
        function ImageEditViewModel(titleEditorViewModel, imageEditorViewModel) {
            var self = this;

            self.titleEditorViewModel = titleEditorViewModel;
            self.imageEditorViewModel = imageEditorViewModel;
        }

        /**
        * Initializes ImageEditor dialog events.
        */
        function initImageEditorDialogEvents(dialog, content) {

            var data = content.Data ? content.Data : { };

            // Create view models for editor boxes and for form
            var titleEditorViewModel = new TitleEditorViewModel(dialog, data.Title);
            
            var imageEditorViewModel = new ImageEditorViewModel(dialog, data);
            
            var viewModel = new ImageEditViewModel(titleEditorViewModel, imageEditorViewModel);
            ko.applyBindings(viewModel, dialog.container.find(selectors.imageEditorForm).get(0));

            // Image alignment
            dialog.container.find(selectors.imageAlignmentControls).children().each(function () {
                setImageAlignment(this, dialog);
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