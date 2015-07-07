'use strict'

var sliderApiUrl = "/odata/kore/revolution-slider/SliderApi";
var slideApiUrl = "/odata/kore/revolution-slider/SlideApi";

var SliderModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.name = ko.observable(null);

    self.create = function () {
        self.id(0);
        self.name(null);

        self.validator.resetForm();
        switchSection($("#sliders-form-section"));
        $("#sliders-form-section-legend").html(translations.Create);
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
            self.title(json.Name);

            self.validator.resetForm();
            switchSection($("#sliders-form-section"));
            $("#sliders-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: sliderApiUrl + "(" + id + ")",
                type: "DELETE",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#SliderGrid').data('kendoGrid').dataSource.read();
                $('#SliderGrid').data('kendoGrid').refresh();
                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
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

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
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

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#sliders-grid-section"));
    };

    self.showSlides = function () {
        switchSection($("#slides-grid-section"));
    };

    self.validator = $("#sliders-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 },
        }
    });
};

var SlideModel = function () {
    var self = this;

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

    self.create = function () {
        self.id(0);
        self.sliderId(0);
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
        $("#slides-form-section-legend").html(translations.Create);
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
            $("#slides-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: slideApiUrl + "(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#SlideGrid').data('kendoGrid').dataSource.read();
                $('#SlideGrid').data('kendoGrid').refresh();

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
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
            Name: self.name(),
            SliderId: self.sliderId(),
            Order: self.order(),
            Transition: self.transition(),
            RandomTransition: self.randomTransition(),
            SlotAmount: self.slotAmount(),
            MasterSpeed: self.masterSpeed(),
            Delay: self.delay(),
            Link: self.link(),
            Target: self.target(),
            SlideIndex: self.slideIndex(),
            Thumb: self.thumb(),
            Title: self.title(),
            LazyLoad: self.lazyLoad(),
            BackgroundRepeat: self.backgroundRepeat(),
            BackgroundFit: self.backgroundFit(),
            BackgroundFitCustomValue: self.backgroundFitCustomValue(),
            BackgroundFitEnd: self.backgroundFitEnd(),
            BackgroundPosition: self.backgroundPosition(),
            BackgroundPositionEnd: self.backgroundPositionEnd(),
            KenBurnsEffect: self.kenBurnsEffect(),
            Duration: self.duration(),
            Easing: self.easing(),
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
                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
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

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#slides-grid-section"));
    };

    self.showSliders = function () {
    };

    self.validator = $("#slides-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 },
            Order: { required: true },
        }
    });
};

var ViewModel = function () {
    var self = this;
    self.slider = new SliderModel();
    self.slide = new SlideModel();

    self.showSliders = function () {
        switchSection($("#sliders-grid-section"));
    };

    self.showSlides = function () {
        switchSection($("#slides-grid-section"));
    };
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    switchSection($("#slides-grid-section"));

    $("#SliderGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: sliderApiUrl,
                    dataType: "json"
                }
            },
            schema: {
                data: function (data) {
                    return data.value;
                },
                total: function (data) {
                    return data["odata.count"];
                },
                model: {
                    fields: {
                        Name: { type: "string" }
                    }
                }
            },
            pageSize: gridPageSize,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "Name", dir: "asc" }
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
            title: translations.Columns.Slider.Name,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group">' +
                '<a onclick="viewModel.slider.edit(#=Id#)" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.slider.delete(#=Id#)" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '<a onclick="viewModel.slider.showSlides(#=Id#)" class="btn btn-danger btn-xs">' + translations.Slides + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 180
        }]
    });

    $("#SlideGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: slideApiUrl,
                    dataType: "json"
                }
            },
            schema: {
                data: function (data) {
                    return data.value;
                },
                total: function (data) {
                    return data["odata.count"];
                },
                model: {
                    fields: {
                        Order: { type: "number" },
                        Title: { type: "string" },
                        Link: { type: "string" },
                    }
                }
            },
            pageSize: gridPageSize,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "Order", dir: "asc" }
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
            title: translations.Columns.Slide.Order,
            filterable: true
        }, {
            field: "Title",
            title: translations.Columns.Slide.Title,
            filterable: true
        }, {
            field: "Link",
            title: translations.Columns.Slide.Link,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group">' +
                '<a onclick="viewModel.slide.edit(#=Id#)" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.slide.delete(#=Id#)" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 210
        }]
    });
});