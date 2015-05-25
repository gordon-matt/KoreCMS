'use strict'

var currentSection = $("#main-section");

function switchSection(section) {
    if (section.attr("id") == currentSection.attr("id")) {
        return;
    }
    currentSection.hide("fast");
    section.show("fast");
    currentSection = section;
};

var CountryModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.name = ko.observable('');
    self.countryCode = ko.observable('');
    self.hasStates = ko.observable(false);
    self.parentId = ko.observable(null);

    self.create = function () {
        self.id(0);
        self.name('');
        self.countryCode('');
        self.hasStates(false);
        self.parentId(viewModel.selectedContinentId());

        self.validator.resetForm();
        switchSection($("#country-form-section"));
        $("#country-form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/common/RegionApi(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.countryCode(json.CountryCode);
            self.hasStates(json.HasStates);
            self.parentId(json.ParentId);

            self.validator.resetForm();
            switchSection($("#country-form-section"));
            $("#country-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/common/RegionApi(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#CountryGrid').data('kendoGrid').dataSource.read();
                $('#CountryGrid').data('kendoGrid').refresh();

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

        var record = {
            Id: self.id(),
            Name: self.name(),
            RegionType: 'Country',
            CountryCode: self.countryCode(),
            HasStates: self.hasStates(),
            ParentId: self.parentId()
        };

        if (isNew) {
            $.ajax({
                url: "/odata/kore/common/RegionApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#CountryGrid').data('kendoGrid').dataSource.read();
                $('#CountryGrid').data('kendoGrid').refresh();

                switchSection($("#country-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: "/odata/kore/common/RegionApi(" + self.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#CountryGrid').data('kendoGrid').dataSource.read();
                $('#CountryGrid').data('kendoGrid').refresh();

                switchSection($("#country-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#country-grid-section"));
    };

    self.goBack = function () {
        viewModel.selectedCountryId(0);
        switchSection($("#main-section"));
    };

    self.showStates = function (countryId) {
        //TODO: Filter states grid
        viewModel.selectedCountryId(countryId);
        viewModel.selectedStateId(0);
        switchSection($("#state-grid-section"));
    };

    self.showCities = function (countryId) {
        //TODO: Filter states grid
        viewModel.selectedCountryId(countryId);
        viewModel.selectedStateId(0);
        switchSection($("#city-grid-section"));
    };

    self.validator = $("#country-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 },
            CountryCode: { maxlength: 10 }
        }
    });
};

var StateModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.name = ko.observable('');
    self.stateCode = ko.observable('');
    self.parentId = ko.observable(null);

    self.create = function () {
        self.state.id(0);
        self.state.name('');
        self.state.stateCode('');
        self.state.parentId(viewModel.selectedCountryId());

        self.validator.resetForm();
        switchSection($("#state-form-section"));
        $("#state-form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/common/RegionApi(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.stateCode(json.StateCode);
            self.parentId(json.ParentId);

            self.validator.resetForm();
            switchSection($("#state-form-section"));
            $("#state-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/common/RegionApi(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#StateGrid').data('kendoGrid').dataSource.read();
                $('#StateGrid').data('kendoGrid').refresh();

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

        var record = {
            Id: self.id(),
            Name: self.name(),
            RegionType: 'State',
            StateCode: self.stateCode(),
            ParentId: self.parentId()
        };

        if (isNew) {
            $.ajax({
                url: "/odata/kore/common/RegionApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#StateGrid').data('kendoGrid').dataSource.read();
                $('#StateGrid').data('kendoGrid').refresh();

                switchSection($("#state-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: "/odata/kore/common/RegionApi(" + self.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#StateGrid').data('kendoGrid').dataSource.read();
                $('#StateGrid').data('kendoGrid').refresh();

                switchSection($("#state-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#state-grid-section"));
    };

    self.goBack = function () {
        viewModel.selectedStateId(0);
        switchSection($("#country-grid-section"));
    };

    self.showCities = function (stateId) {
        //TODO: Filter states grid
        viewModel.selectedStateId(stateId);
        switchSection($("#city-grid-section"));
    };

    self.validator = $("#state-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 },
            StateCode: { maxlength: 10 }
        }
    });
};

var CityModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.name = ko.observable('');
    self.parentId = ko.observable(null);

    self.create = function () {
        self.city.id(0);
        self.city.name('');

        if (viewModel.selectedStateId()) {
            self.city.parentId(viewModel.selectedStateId());
        }
        else {
            self.city.parentId(viewModel.selectedCountryId());
        }

        self.validator.resetForm();
        switchSection($("#city-form-section"));
        $("#city-form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/common/RegionApi(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.parentId(json.ParentId);

            self.validator.resetForm();
            switchSection($("#city-form-section"));
            $("#city-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/common/RegionApi(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#CityGrid').data('kendoGrid').dataSource.read();
                $('#CityGrid').data('kendoGrid').refresh();

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

        var record = {
            Id: self.id(),
            Name: self.name(),
            RegionType: 'City',
            ParentId: self.parentId()
        };

        if (isNew) {
            $.ajax({
                url: "/odata/kore/common/RegionApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#CityGrid').data('kendoGrid').dataSource.read();
                $('#CityGrid').data('kendoGrid').refresh();

                switchSection($("#city-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: "/odata/kore/common/RegionApi(" + self.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#CityGrid').data('kendoGrid').dataSource.read();
                $('#CityGrid').data('kendoGrid').refresh();

                switchSection($("#city-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#city-grid-section"));
    };

    self.goBack = function () {
        if (viewModel.selectedStateId()) {
            switchSection($("#state-grid-section"));
        }
        else {
            switchSection($("#country-grid-section"));
        }
    };

    self.validator = $("#city-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 }
        }
    });
};

var ViewModel = function () {
    var self = this;

    self.selectedContinentId = ko.observable(0);
    self.selectedCountryId = ko.observable(0);
    self.selectedStateId = ko.observable(0);

    self.country = new CountryModel();
    self.state = new StateModel();
    self.city = new CityModel();

    self.showCountries = function (continentId) {
        self.selectedContinentId(continentId);

        var grid = $('#CountryGrid').data('kendoGrid');
        grid.dataSource.transport.options.read.url = "/odata/kore/common/RegionApi?$filter=ParentId eq " + continentId;
        grid.dataSource.read();
        grid.refresh();

        switchSection($("#country-grid-section"));
    };
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();

    switchSection($("#main-section"));

    $("#CountryGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/common/RegionApi",
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
            title: translations.Columns.Name,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group">' +
                '<a onclick="viewModel.country.showStates(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.States + '</a>' +
                '<a onclick="viewModel.country.showCities(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Cities + '</a>' +
                '<a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 180
        }]
    });

    ko.applyBindings(viewModel);
});