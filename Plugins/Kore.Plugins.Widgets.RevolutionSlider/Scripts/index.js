define(function (require) {
    'use strict'

    var $ = require('jquery');
    var ko = require('knockout');

    require('jqueryval');
    require('kendo');
    require('notify');

    require('kore-common');
    require('kore-section-switching');
    require('kore-jqueryval');

    var sliderApiUrl = "/odata/kore/revolution-slider/RevolutionSliderApi";
    var slideApiUrl = "/odata/kore/revolution-slider/RevolutionSlideApi";
    var layerApiUrl = "/odata/kore/revolution-slider/RevolutionLayerApi";

    var SliderModel = function (parent) {
        var self = this;

        self.parent = parent;
        self.id = ko.observable(0);
        self.name = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#sliders-form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 },
                }
            });

            $("#SliderGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: sliderApiUrl,
                            dataType: "json"
                        },
                        parameterMap: function (data) {
                            var paramMap = kendo.data.transports.odata.parameterMap(data);
                            if (paramMap.$inlinecount) {
                                if (paramMap.$inlinecount == "allpages") {
                                    paramMap.$count = true;
                                }
                                delete paramMap.$inlinecount;
                            }
                            if (paramMap.$filter) {
                                paramMap.$filter = paramMap.$filter.replace(/substringof\((.+),(.*?)\)/, "contains($2,$1)");
                            }
                            return paramMap;
                        }
                    },
                    schema: {
                        data: function (data) {
                            return data.value;
                        },
                        total: function (data) {
                            return data["@odata.count"];
                        },
                        model: {
                            fields: {
                                Name: { type: "string" }
                            }
                        }
                    },
                    pageSize: self.parent.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Name", dir: "asc" }
                },
                dataBound: function (e) {
                    var body = this.element.find("tbody")[0];
                    if (body) {
                        ko.cleanNode(body);
                        ko.applyBindings(ko.dataFor(body), body);
                    }
                },
                filterable: true,
                sortable: {
                    allowUnsort: false
                },
                pageable: {
                    refresh: true
                },
                scrollable: false,
                columns: [{
                    field: "Name",
                    title: self.parent.translations.Columns.Slider.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a data-bind="click: slider.edit.bind($data,#=Id#)" class="btn btn-default btn-xs">' + self.parent.translations.Edit + '</a>' +
                        '<a data-bind="click: slider.remove.bind($data,#=Id#)" class="btn btn-danger btn-xs">' + self.parent.translations.Delete + '</a>' +
                        '<a data-bind="click: showSlides.bind($data,#=Id#)" class="btn btn-info btn-xs">' + self.parent.translations.Slides + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 180
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.name(null);

            self.validator.resetForm();
            switchSection($("#sliders-form-section"));
            $("#sliders-form-section-legend").html(self.parent.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: sliderApiUrl + "(" + id + ")",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.name(json.Name);

                self.validator.resetForm();
                switchSection($("#sliders-form-section"));
                $("#sliders-form-section-legend").html(self.parent.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(self.parent.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: sliderApiUrl + "(" + id + ")",
                    type: "DELETE",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#SliderGrid').data('kendoGrid').dataSource.read();
                    $('#SliderGrid').data('kendoGrid').refresh();
                    $.notify(self.parent.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            var isNew = (self.id() == 0);

            if (!$("#sliders-form-section-form").valid()) {
                return false;
            }

            var record = {
                Id: self.id(),
                Name: self.name(),
            };

            if (isNew) {
                $.ajax({
                    url: sliderApiUrl,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#SliderGrid').data('kendoGrid').dataSource.read();
                    $('#SliderGrid').data('kendoGrid').refresh();

                    switchSection($("#sliders-grid-section"));

                    $.notify(self.parent.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                $.ajax({
                    url: sliderApiUrl + "(" + self.id() + ")",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#SliderGrid').data('kendoGrid').dataSource.read();
                    $('#SliderGrid').data('kendoGrid').refresh();

                    switchSection($("#sliders-grid-section"));

                    $.notify(self.parent.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#sliders-grid-section"));
        };
    };

    var SlideModel = function (parent) {
        var self = this;

        self.parent = parent;
        self.id = ko.observable(0);
        self.sliderId = ko.observable(0);
        self.order = ko.observable(0);
        self.transition = ko.observable(null);
        self.randomTransition = ko.observable(false);
        self.slotAmount = ko.observable(null);
        self.masterSpeed = ko.observable(null);
        self.delay = ko.observable(null);
        self.link = ko.observable(null);
        self.target = ko.observable(null);
        self.slideIndex = ko.observable(null);
        self.thumb = ko.observable(null);
        self.title = ko.observable(null);
        self.imageUrl = ko.observable(null)
        self.lazyLoad = ko.observable(true);
        self.backgroundRepeat = ko.observable(null);
        self.backgroundFit = ko.observable(null);
        self.backgroundFitCustomValue = ko.observable(null);
        self.backgroundFitEnd = ko.observable(null);
        self.backgroundPosition = ko.observable(null);
        self.backgroundPositionEnd = ko.observable(null);
        self.kenBurnsEffect = ko.observable(false);
        self.duration = ko.observable(null);
        self.easing = ko.observable(null);

        self.isEditMode = ko.observable(false);

        self.validator = false;

        self.init = function () {
            self.validator = $("#slides-form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 },
                    Order: { required: true },
                }
            });

            $("#SlideGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: slideApiUrl,
                            dataType: "json"
                        },
                        parameterMap: function (options, operation) {
                            var paramMap = kendo.data.transports.odata.parameterMap(options);
                            if (paramMap.$inlinecount) {
                                if (paramMap.$inlinecount == "allpages") {
                                    paramMap.$count = true;
                                }
                                delete paramMap.$inlinecount;
                            }
                            if (paramMap.$filter) {
                                paramMap.$filter = paramMap.$filter.replace(/substringof\((.+),(.*?)\)/, "contains($2,$1)");
                            }
                            return paramMap;
                        }
                    },
                    schema: {
                        data: function (data) {
                            return data.value;
                        },
                        total: function (data) {
                            return data["@odata.count"];
                        },
                        model: {
                            fields: {
                                Order: { type: "number" },
                                Title: { type: "string" },
                                Link: { type: "string" },
                            }
                        }
                    },
                    pageSize: self.parent.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Order", dir: "asc" }
                },
                dataBound: function (e) {
                    var body = this.element.find("tbody")[0];
                    if (body) {
                        ko.cleanNode(body);
                        ko.applyBindings(ko.dataFor(body), body);
                    }
                },
                filterable: true,
                sortable: {
                    allowUnsort: false
                },
                pageable: {
                    refresh: true
                },
                scrollable: false,
                columns: [{
                    field: "Order",
                    title: self.parent.translations.Columns.Slide.Order,
                    filterable: true,
                    width: 50
                }, {
                    field: "Title",
                    title: self.parent.translations.Columns.Slide.Title,
                    filterable: true
                }, {
                    field: "Link",
                    title: self.parent.translations.Columns.Slide.Link,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a data-bind="click: slide.edit.bind($data,#=Id#)" class="btn btn-default btn-xs">' + self.parent.translations.Edit + '</a>' +
                        '<a data-bind="click: slide.remove.bind($data,#=Id#)" class="btn btn-danger btn-xs">' + self.parent.translations.Delete + '</a>' +
                        '<a data-bind="click: showLayers.bind($data,#=Id#)" class="btn btn-info btn-xs">' + self.parent.translations.Layers + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 180
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.sliderId(self.parent.selectedSliderId());
            self.order(0);
            self.transition(null);
            self.randomTransition(false);
            self.slotAmount(null);
            self.masterSpeed(null);
            self.delay(null);
            self.link(null);
            self.target(null);
            self.slideIndex(null);
            self.thumb(null);
            self.title(null);
            self.imageUrl(null);
            self.lazyLoad(true);
            self.backgroundRepeat(null);
            self.backgroundFit(null);
            self.backgroundFitCustomValue(null);
            self.backgroundFitEnd(null);
            self.backgroundPosition(null);
            self.backgroundPositionEnd(null);
            self.kenBurnsEffect(false);
            self.duration(null);
            self.easing(null);

            self.isEditMode(false);
            self.validator.resetForm();
            switchSection($("#slides-form-section"));
            $("#slides-form-section-legend").html(self.parent.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: slideApiUrl + "(" + id + ")",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.sliderId(json.SliderId);
                self.order(json.Order);
                self.transition(json.Transition);
                self.randomTransition(json.RandomTransition);
                self.slotAmount(json.SlotAmount);
                self.masterSpeed(json.MasterSpeed);
                self.delay(json.Delay);
                self.link(json.Link);
                self.target(json.Target);
                self.slideIndex(json.SlideIndex);
                self.thumb(json.Thumb);
                self.title(json.Title);
                self.imageUrl(json.ImageUrl);
                self.lazyLoad(json.LazyLoad);
                self.backgroundRepeat(json.BackgroundRepeat);
                self.backgroundFit(json.BackgroundFit);
                self.backgroundFitCustomValue(json.BackgroundFitCustomValue);
                self.backgroundFitEnd(json.BackgroundFitEnd);
                self.backgroundPosition(json.BackgroundPosition);
                self.backgroundPositionEnd(json.BackgroundPositionEnd);
                self.kenBurnsEffect(json.KenBurnsEffect);
                self.duration(json.Duration);
                self.easing(json.Easing);

                self.isEditMode(true);

                self.validator.resetForm();
                switchSection($("#slides-form-section"));
                $("#slides-form-section-legend").html(self.parent.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(self.parent.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: slideApiUrl + "(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#SlideGrid').data('kendoGrid').dataSource.read();
                    $('#SlideGrid').data('kendoGrid').refresh();

                    $.notify(self.parent.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            if (!$("#slides-form-section-form").valid()) {
                return false;
            }

            var record = {
                Id: self.id(),
                SliderId: self.sliderId(),
                Order: self.order(),
                Transition: stringToNullIfEmpty(self.transition()),
                RandomTransition: self.randomTransition(),
                SlotAmount: self.slotAmount(),
                MasterSpeed: self.masterSpeed(),
                Delay: self.delay(),
                Link: self.link(),
                Target: stringToNullIfEmpty(self.target()),
                SlideIndex: self.slideIndex(),
                Thumb: self.thumb(),
                Title: self.title(),
                ImageUrl: self.imageUrl(),
                LazyLoad: self.lazyLoad(),
                BackgroundRepeat: stringToNullIfEmpty(self.backgroundRepeat()),
                BackgroundFit: stringToNullIfEmpty(self.backgroundFit()),
                BackgroundFitCustomValue: self.backgroundFitCustomValue(),
                BackgroundFitEnd: self.backgroundFitEnd(),
                BackgroundPosition: stringToNullIfEmpty(self.backgroundPosition()),
                BackgroundPositionEnd: stringToNullIfEmpty(self.backgroundPositionEnd()),
                KenBurnsEffect: self.kenBurnsEffect(),
                Duration: self.duration(),
                Easing: stringToNullIfEmpty(self.easing()),
            };

            if (self.id() == 0) {
                // INSERT
                $.ajax({
                    url: slideApiUrl,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#SlideGrid').data('kendoGrid').dataSource.read();
                    $('#SlideGrid').data('kendoGrid').refresh();

                    switchSection($("#slides-grid-section"));
                    $.notify(self.parent.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                // UPDATE
                $.ajax({
                    url: slideApiUrl + "(" + self.id() + ")",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#SlideGrid').data('kendoGrid').dataSource.read();
                    $('#SlideGrid').data('kendoGrid').refresh();

                    switchSection($("#slides-grid-section"));

                    $.notify(self.parent.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#slides-grid-section"));
        };
        self.goBack = function () {
            switchSection($("#sliders-grid-section"));
        };
    };

    var LayerModel = function (parent) {
        var self = this;

        self.parent = parent;
        self.id = ko.observable(0);
        self.slideId = ko.observable(0);
        self.captionText = ko.observable(null);
        self.styleClass = ko.observable(null);
        self.incomingAnimation = ko.observable(null);
        self.outgoingAnimation = ko.observable(null);
        self.x = ko.observable(null);
        self.y = ko.observable(null);
        self.horizontalOffset = ko.observable(null);
        self.verticalOffset = ko.observable(null);
        self.speed = ko.observable(null);
        self.splitIn = ko.observable('None');
        self.elementDelay = ko.observable(null);
        self.splitOut = ko.observable('None');
        self.endElementDelay = ko.observable(null);
        self.start = ko.observable(null);
        self.easing = ko.observable(null);
        self.endSpeed = ko.observable(null);
        self.end = ko.observable(null);
        self.endEasing = ko.observable(null);

        self.autoPlay = ko.observable(false);
        self.autoPlayOnlyFirstTime = ko.observable(false);
        self.nextSlideAtEnd = ko.observable(true);
        self.videoPoster = ko.observable(null);
        self.forceCover = ko.observable(false);
        self.forceRewind = ko.observable(false);
        self.mute = ko.observable(false);
        self.videoWidth = ko.observable(null);
        self.videoWidthUnit = ko.observable('Pixels');
        self.videoHeight = ko.observable(null);
        self.videoHeightUnit = ko.observable('Pixels');
        self.aspectRatio = ko.observable(null);
        self.videoPreload = ko.observable('None');
        self.videoType = ko.observable(null);
        self.videoMp4 = ko.observable(null);
        self.videoWebM = ko.observable(null);
        self.videoOgv = ko.observable(null);
        self.youTubeId = ko.observable(null);
        self.vimeoId = ko.observable(null);
        self.showVideoControls = ko.observable(true);
        self.videoAttributes = ko.observable(null);
        self.videoLoop = ko.observable('None');

        self.validator = false;

        self.init = function () {
            self.validator = $("#layers-form-section-form").validate({
                rules: {
                    Layer_SlideId: { required: true },
                    Layer_CaptionText: { maxlength: 255 },
                    Layer_X: { required: true },
                    Layer_Y: { required: true },
                    Layer_Speed: { required: true },
                    Layer_Start: { required: true },
                    Layer_StyleClass: { maxlength: 50 },
                    Layer_SplitIn: { required: true },
                    Layer_SplitOut: { required: true },
                    Layer_VideoPoster: { maxlength: 255 },
                    Layer_VideoWidthUnit: { required: true },
                    Layer_VideoHeightUnit: { required: true },
                    Layer_VideoPreload: { required: true },
                    Layer_VideoMp4: { maxlength: 255 },
                    Layer_VideoWebM: { maxlength: 255 },
                    Layer_VideoOgv: { maxlength: 255 },
                    Layer_YouTubeId: { maxlength: 255 },
                    Layer_VimeoId: { maxlength: 255 },
                    Layer_VideoAttributes: { maxlength: 128 },
                    Layer_VideoLoop: { required: true },
                }
            });

            $("#LayerGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: layerApiUrl,
                            dataType: "json"
                        },
                        parameterMap: function (options, operation) {
                            var paramMap = kendo.data.transports.odata.parameterMap(options);
                            if (paramMap.$inlinecount) {
                                if (paramMap.$inlinecount == "allpages") {
                                    paramMap.$count = true;
                                }
                                delete paramMap.$inlinecount;
                            }
                            if (paramMap.$filter) {
                                paramMap.$filter = paramMap.$filter.replace(/substringof\((.+),(.*?)\)/, "contains($2,$1)");
                            }
                            return paramMap;
                        }
                    },
                    schema: {
                        data: function (data) {
                            return data.value;
                        },
                        total: function (data) {
                            return data["@odata.count"];
                        },
                        model: {
                            fields: {
                                Start: { type: "number" },
                                CaptionText: { type: "string" },
                                Speed: { type: "number" },
                                X: { type: "string" },
                                Y: { type: "string" },
                            }
                        }
                    },
                    pageSize: self.parent.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Start", dir: "asc" }
                },
                dataBound: function (e) {
                    var body = this.element.find("tbody")[0];
                    if (body) {
                        ko.cleanNode(body);
                        ko.applyBindings(ko.dataFor(body), body);
                    }
                },
                filterable: true,
                sortable: {
                    allowUnsort: false
                },
                pageable: {
                    refresh: true
                },
                scrollable: false,
                columns: [{
                    field: "Start",
                    title: self.parent.translations.Columns.Layer.Start,
                    filterable: true
                }, {
                    field: "Speed",
                    title: self.parent.translations.Columns.Layer.Speed,
                    filterable: true
                }, {
                    field: "CaptionText",
                    title: self.parent.translations.Columns.Layer.CaptionText,
                    filterable: true
                }, {
                    field: "X",
                    title: self.parent.translations.Columns.Layer.X,
                    filterable: true
                }, {
                    field: "Y",
                    title: self.parent.translations.Columns.Layer.Y,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a data-bind="click: layer.edit.bind($data,#=Id#)" class="btn btn-default btn-xs">' + self.parent.translations.Edit + '</a>' +
                        '<a data-bind="click: layer.remove.bind($data,#=Id#)" class="btn btn-danger btn-xs">' + self.parent.translations.Delete + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 180
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.slideId(self.parent.selectedSlideId());
            self.captionText(null);
            self.styleClass(null);
            self.incomingAnimation(null);
            self.outgoingAnimation(null);
            self.x(null);
            self.y(null);
            self.horizontalOffset(null);
            self.verticalOffset(null);
            self.speed(null);
            self.splitIn('None');
            self.elementDelay(null);
            self.splitOut('None');
            self.endElementDelay(null);
            self.start(null);
            self.easing(null);
            self.endSpeed(null);
            self.end(null);
            self.endEasing(null);

            self.autoPlay(false);
            self.autoPlayOnlyFirstTime(false);
            self.nextSlideAtEnd(true);
            self.videoPoster(null);
            self.forceCover(false);
            self.forceRewind(false);
            self.mute(false);
            self.videoWidth(null);
            self.videoWidthUnit('Pixels');
            self.videoHeight(null);
            self.videoHeightUnit('Pixels');
            self.aspectRatio(null);
            self.videoPreload('None');
            self.videoType(null);
            self.videoMp4(null);
            self.videoWebM(null);
            self.videoOgv(null);
            self.youTubeId(null);
            self.vimeoId(null);
            self.showVideoControls(true);
            self.videoAttributes(null);
            self.videoLoop('None');

            self.validator.resetForm();
            switchSection($("#layers-form-section"));
            $("#layers-form-section-legend").html(self.parent.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: layerApiUrl + "(" + id + ")",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.slideId(json.SlideId);
                self.captionText(json.CaptionText);
                self.styleClass(json.StyleClass);
                self.incomingAnimation(json.IncomingAnimation);
                self.outgoingAnimation(json.OutgoingAnimation);
                self.x(json.X);
                self.y(json.Y);
                self.horizontalOffset(json.HorizontalOffset);
                self.verticalOffset(json.VerticalOffset);
                self.speed(json.Speed);
                self.splitIn(json.SplitIn);
                self.elementDelay(json.ElementDelay);
                self.splitOut(json.SplitOut);
                self.endElementDelay(json.EndElementDelay);
                self.start(json.Start);
                self.easing(json.Easing);
                self.endSpeed(json.EndSpeed);
                self.end(json.End);
                self.endEasing(json.EndEasing);

                self.autoPlay(json.AutoPlay);
                self.autoPlayOnlyFirstTime(json.AutoPlayOnlyFirstTime);
                self.nextSlideAtEnd(json.NextSlideAtEnd);
                self.videoPoster(json.VideoPoster);
                self.forceCover(json.ForceCover);
                self.forceRewind(json.ForceRewind);
                self.mute(json.Mute);
                self.videoWidth(json.VideoWidth);
                self.videoWidthUnit(json.VideoWidthUnit);
                self.videoHeight(json.VideoHeight);
                self.videoHeightUnit(json.VideoHeightUnit);
                self.aspectRatio(json.AspectRatio);
                self.videoPreload(json.VideoPreload);
                self.videoType(json.VideoType);
                self.videoMp4(json.VideoMp4);
                self.videoWebM(json.VideoWebM);
                self.videoOgv(json.VideoOgv);
                self.youTubeId(json.YouTubeId);
                self.vimeoId(json.VimeoId);
                self.showVideoControls(json.ShowVideoControls);
                self.videoAttributes(json.VideoAttributes);
                self.videoLoop(json.VideoLoop);

                self.validator.resetForm();
                switchSection($("#layers-form-section"));
                $("#layers-form-section-legend").html(self.parent.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(self.parent.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: layerApiUrl + "(" + id + ")",
                    type: "DELETE",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#LayerGrid').data('kendoGrid').dataSource.read();
                    $('#LayerGrid').data('kendoGrid').refresh();
                    $.notify(self.parent.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            var isNew = (self.id() == 0);

            if (!$("#layers-form-section-form").valid()) {
                return false;
            }

            var aspectRatio = null;
            var easing = null;
            var endEasing = null;
            var incomingAnimation = null;
            var outgoingAnimation = null;
            var videoType = null;

            if (self.aspectRatio()) {
                aspectRatio = self.aspectRatio();
            }
            if (self.easing()) {
                easing = self.easing();
            }
            if (self.endEasing()) {
                endEasing = self.endEasing();
            }
            if (self.incomingAnimation()) {
                incomingAnimation = self.incomingAnimation();
            }
            if (self.outgoingAnimation()) {
                outgoingAnimation = self.outgoingAnimation();
            }
            if (self.videoType()) {
                videoType = self.videoType();
            }

            var record = {
                Id: self.id(),
                SlideId: self.slideId(),
                CaptionText: self.captionText(),
                StyleClass: self.styleClass(),
                IncomingAnimation: incomingAnimation,
                OutgoingAnimation: outgoingAnimation,
                X: self.x(),
                Y: self.y(),
                HorizontalOffset: self.horizontalOffset(),
                VerticalOffset: self.verticalOffset(),
                Speed: self.speed(),
                SplitIn: self.splitIn(),
                ElementDelay: self.elementDelay(),
                SplitOut: self.splitOut(),
                EndElementDelay: self.endElementDelay(),
                Start: self.start(),
                Easing: easing,
                EndSpeed: self.endSpeed(),
                End: self.end(),
                EndEasing: endEasing,

                AutoPlay: self.autoPlay(),
                AutoPlayOnlyFirstTime: self.autoPlayOnlyFirstTime(),
                NextSlideAtEnd: self.nextSlideAtEnd(),
                VideoPoster: self.videoPoster(),
                ForceCover: self.forceCover(),
                ForceRewind: self.forceRewind(),
                Mute: self.mute(),
                VideoWidth: self.videoWidth(),
                VideoWidthUnit: self.videoWidthUnit(),
                VideoHeight: self.videoHeight(),
                VideoHeightUnit: self.videoHeightUnit(),
                AspectRatio: aspectRatio,
                VideoPreload: self.videoPreload(),
                VideoType: videoType,
                VideoMp4: self.videoMp4(),
                VideoWebM: self.videoWebM(),
                VideoOgv: self.videoOgv(),
                YouTubeId: self.youTubeId(),
                VimeoId: self.vimeoId(),
                ShowVideoControls: self.showVideoControls(),
                VideoAttributes: self.videoAttributes(),
                VideoLoop: self.videoLoop(),
            };

            if (isNew) {
                $.ajax({
                    url: layerApiUrl,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#LayerGrid').data('kendoGrid').dataSource.read();
                    $('#LayerGrid').data('kendoGrid').refresh();

                    switchSection($("#layers-grid-section"));

                    $.notify(self.parent.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                $.ajax({
                    url: layerApiUrl + "(" + self.id() + ")",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#LayerGrid').data('kendoGrid').dataSource.read();
                    $('#LayerGrid').data('kendoGrid').refresh();

                    switchSection($("#layers-grid-section"));

                    $.notify(self.parent.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#layers-grid-section"));
        };
        self.goBack = function () {
            switchSection($("#slides-grid-section"));
        };
    };

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.slider = false;
        self.slide = false;
        self.layer = false;

        self.selectedSliderId = ko.observable(0);
        self.selectedSlideId = ko.observable(0);

        self.modalDismissed = false;

        self.activate = function () {
            self.slider = new SliderModel(self);
            self.slide = new SlideModel(self);
            self.layer = new LayerModel(self);
        };
        self.attached = function () {
            currentSection = $("#sliders-grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/plugins/widgets/revolutionslider/get-translations",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.translations = json;
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            });

            self.gridPageSize = $("#GridPageSize").val();

            self.slider.init();
            self.slide.init();
            self.layer.init();

            $('#imageUrlModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#ImageUrl').val();
                    url = "/Media/Uploads/" + url;
                    self.slide.imageUrl(url);
                }
                self.modalDismissed = false;
            });
            $('#videoPosterModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#VideoPoster').val();
                    url = "/Media/Uploads/" + url;
                    self.layer.videoPoster(url);
                }
                self.modalDismissed = false;
            });
            $('#videoMp4Modal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#VideoMp4').val();
                    url = "/Media/Uploads/" + url;
                    self.layer.videoMp4(url);
                }
                self.modalDismissed = false;
            });
            $('#videoWebMModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#VideoWebM').val();
                    url = "/Media/Uploads/" + url;
                    self.layer.videoWebM(url);
                }
                self.modalDismissed = false;
            });
            $('#videoOgvModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#VideoOgv').val();
                    url = "/Media/Uploads/" + url;
                    self.layer.videoOgv(url);
                }
                self.modalDismissed = false;
            });
        };
        self.showSlides = function (id) {
            self.selectedSliderId(id);
            var grid = $('#SlideGrid').data('kendoGrid');
            grid.dataSource.transport.options.read.url = slideApiUrl + "?$filter=SliderId eq " + id;
            grid.dataSource.page(1);

            switchSection($("#slides-grid-section"));
        };
        self.showLayers = function (id) {
            self.selectedSlideId(id);
            var grid = $('#LayerGrid').data('kendoGrid');
            grid.dataSource.transport.options.read.url = layerApiUrl + "?$filter=SlideId eq " + id;
            grid.dataSource.page(1);

            switchSection($("#layers-grid-section"));
        };
        self.dismissModal = function (modalId) {
            self.modalDismissed = true;
            $('#' + modalId).modal('hide');
        };
    };

    var viewModel = new ViewModel();
    return viewModel;
});