var viewModel;

define(function (require) {
    'use strict'

    viewModel = null;

    var $ = require('jquery');
    var ko = require('knockout');
    var koMap = require('knockout-mapping');

    require('jqueryval');
    require('kendo');
    require('notify');

    require('kore-common');
    require('kore-section-switching');
    require('kore-jqueryval');

    require('jquery-maphilight');

    ko.mapping = koMap;

    var SettingsModel = function () {
        var self = this;
        self.regionId = ko.observable(0);
        self.settingsId = ko.observable('');
        self.fields = ko.observable('');

        self.init = function () {
            $("#SettingsGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/common/RegionSettingsApi",
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
                                Name: { type: "string" }
                            }
                        }
                    },
                    pageSize: viewModel.gridPageSize,
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
                    title: viewModel.translations.Columns.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template: '<div class="btn-group"><a onclick="viewModel.settings.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a></div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 120
                }]
            });
        };
        self.edit = function (id) {
            self.settingsId(id);

            $.ajax({
                url: "/odata/kore/common/RegionSettingsApi/Default.GetSettings",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    settingsId: id,
                    regionId: self.regionId()
                }),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.fields(json.Fields);

                $.ajax({
                    url: "/admin/regions/get-editor-ui/" + id,
                    type: "GET",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {

                    // Clean up from previously injected html/scripts
                    if (typeof cleanUp == 'function') {
                        cleanUp();
                    }

                    // Remove Old Scripts
                    var oldScripts = $('script[data-settings-script="true"]');

                    if (oldScripts.length > 0) {
                        $.each(oldScripts, function () {
                            $(this).remove();
                        });
                    }

                    var elementToBind = $("#settings-form-section")[0];
                    ko.cleanNode(elementToBind);

                    var result = $(json.Content);

                    // Add new HTML
                    var content = $(result.filter('#region-settings')[0]);
                    var details = $('<div>').append(content.clone()).html();
                    $("#settings-details").html(details);

                    // Add new Scripts
                    var scripts = result.filter('script');

                    $.each(scripts, function () {
                        var script = $(this);
                        script.attr("data-settings-script", "true");//for some reason, .data("block-script", "true") doesn't work here
                        script.appendTo('body');
                    });

                    // Update Bindings
                    // Ensure the function exists before calling it...
                    if (typeof updateModel == 'function') {
                        var data = ko.toJS(ko.mapping.fromJSON(self.fields()));
                        updateModel(data);
                        ko.applyBindings(viewModel, elementToBind);
                    }

                    //self.validator.resetForm();
                    switchSection($("#settings-form-section"));
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.GetRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.save = function () {
            // ensure the function exists before calling it...
            if (typeof onBeforeSave == 'function') {
                onBeforeSave();
            }

            var record = {
                settingsId: self.settingsId(),
                regionId: self.regionId(),
                fields: self.fields()
            };

            $.ajax({
                url: "/odata/kore/common/RegionSettingsApi/Default.SaveSettings",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false,
            })
            .done(function (json) {
                switchSection($("#settings-grid-section"));

                $.notify(viewModel.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.UpdateRecordError + ": " + jqXHR.responseText || textStatus, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.cancel = function () {
            switchSection($("#settings-grid-section"));
        };
        self.goBack = function () {
            switchSection($("#main-section"));
        };
    };

    var CountryModel = function () {
        var self = this;

        self.id = ko.observable(0);
        self.name = ko.observable(null);
        self.countryCode = ko.observable(null);
        self.hasStates = ko.observable(false);
        self.parentId = ko.observable(null);
        self.order = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#country-form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 },
                    CountryCode: { maxlength: 10 }
                }
            });

            $("#CountryGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/common/RegionApi?$filter=RegionType eq Kore.Web.Common.Areas.Admin.Regions.Domain.RegionType'Country'",
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
                                Name: { type: "string" }
                            }
                        }
                    },
                    pageSize: viewModel.gridPageSize,
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
                    title: viewModel.translations.Columns.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '# if(HasStates) {# <a onclick="viewModel.country.showStates(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.States + '</a> #} ' +
                        'else {# <a onclick="viewModel.country.showCities(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Cities + '</a> #} # ' +
                        '<a onclick="viewModel.country.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.country.remove(\'#=Id#\')" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '<a onclick="viewModel.showSettings(#=Id#)" class="btn btn-info btn-xs">' + viewModel.translations.Settings + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 230
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.name(null);
            self.countryCode(null);
            self.hasStates(false);
            self.parentId(viewModel.selectedContinentId());
            self.order(null);

            self.validator.resetForm();
            switchSection($("#country-form-section"));
            $("#country-form-section-legend").html(viewModel.translations.Create);
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
                self.order(json.Order);

                self.validator.resetForm();
                switchSection($("#country-form-section"));
                $("#country-form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/common/RegionApi(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#CountryGrid').data('kendoGrid').dataSource.read();
                    $('#CountryGrid').data('kendoGrid').refresh();

                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            var isNew = (self.id() == 0);

            if (!$("#country-form-section-form").valid()) {
                return false;
            }

            var order = self.order();
            if (!order) {
                order = null;
            }

            var record = {
                Id: self.id(),
                Name: self.name(),
                RegionType: 'Country',
                CountryCode: self.countryCode(),
                HasStates: self.hasStates(),
                ParentId: self.parentId(),
                Order: order,
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

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
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

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
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

            var grid = $('#StateGrid').data('kendoGrid');
            grid.dataSource.transport.options.read.url = "/odata/kore/common/RegionApi?$filter=RegionType eq Kore.Web.Common.Areas.Admin.Regions.Domain.RegionType'State' and ParentId eq " + countryId;
            grid.dataSource.page(1);
            //grid.dataSource.read();
            //grid.refresh();

            switchSection($("#state-grid-section"));
        };
        self.showCities = function (countryId) {
            //TODO: Filter states grid
            viewModel.selectedCountryId(countryId);
            viewModel.selectedStateId(0);

            var grid = $('#CityGrid').data('kendoGrid');
            grid.dataSource.transport.options.read.url = "/odata/kore/common/RegionApi?$filter=RegionType eq Kore.Web.Common.Areas.Admin.Regions.Domain.RegionType'City' and ParentId eq " + countryId;
            grid.dataSource.page(1);
            //grid.dataSource.read();
            //grid.refresh();

            switchSection($("#city-grid-section"));
        };
    };

    var StateModel = function () {
        var self = this;

        self.id = ko.observable(0);
        self.name = ko.observable(null);
        self.stateCode = ko.observable(null);
        self.parentId = ko.observable(null);
        self.order = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#state-form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 },
                    StateCode: { maxlength: 10 }
                }
            });

            $("#StateGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/common/RegionApi?$filter=RegionType eq Kore.Web.Common.Areas.Admin.Regions.Domain.RegionType'State'",
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
                                Name: { type: "string" }
                            }
                        }
                    },
                    pageSize: viewModel.gridPageSize,
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
                    title: viewModel.translations.Columns.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a onclick="viewModel.state.showCities(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Cities + '</a>' +
                        '<a onclick="viewModel.state.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.state.remove(\'#=Id#\')" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '<a onclick="viewModel.showSettings(#=Id#)" class="btn btn-info btn-xs">' + viewModel.translations.Settings + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 230
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.name(null);
            self.stateCode(null);
            self.parentId(viewModel.selectedCountryId());
            self.order(null);

            self.validator.resetForm();
            switchSection($("#state-form-section"));
            $("#state-form-section-legend").html(viewModel.translations.Create);
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
                self.order(json.Order);

                self.validator.resetForm();
                switchSection($("#state-form-section"));
                $("#state-form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/common/RegionApi(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#StateGrid').data('kendoGrid').dataSource.read();
                    $('#StateGrid').data('kendoGrid').refresh();

                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            var isNew = (self.id() == 0);

            if (!$("#state-form-section-form").valid()) {
                return false;
            }

            var order = self.order();
            if (!order) {
                order = null;
            }

            var record = {
                Id: self.id(),
                Name: self.name(),
                RegionType: 'State',
                StateCode: self.stateCode(),
                ParentId: self.parentId(),
                Order: order,
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

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
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

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
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

            var grid = $('#CityGrid').data('kendoGrid');
            grid.dataSource.transport.options.read.url = "/odata/kore/common/RegionApi?$filter=RegionType eq Kore.Web.Common.Areas.Admin.Regions.Domain.RegionType'City' and ParentId eq " + stateId;
            grid.dataSource.page(1);
            //grid.dataSource.read();
            //grid.refresh();

            switchSection($("#city-grid-section"));
        };
    };

    var CityModel = function () {
        var self = this;

        self.id = ko.observable(0);
        self.name = ko.observable(null);
        self.parentId = ko.observable(null);
        self.order = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#city-form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 }
                }
            });

            $("#CityGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/common/RegionApi?$filter=RegionType eq Kore.Web.Common.Areas.Admin.Regions.Domain.RegionType'City'",
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
                                Name: { type: "string" }
                            }
                        }
                    },
                    pageSize: viewModel.gridPageSize,
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
                    title: viewModel.translations.Columns.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a onclick="viewModel.city.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.city.remove(\'#=Id#\')" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '<a onclick="viewModel.showSettings(#=Id#)" class="btn btn-info btn-xs">' + viewModel.translations.Settings + '</a>' +
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
            self.order(null);

            if (viewModel.selectedStateId()) {
                self.parentId(viewModel.selectedStateId());
            }
            else {
                self.parentId(viewModel.selectedCountryId());
            }

            self.validator.resetForm();
            switchSection($("#city-form-section"));
            $("#city-form-section-legend").html(viewModel.translations.Create);
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
                self.order(json.Order);

                self.validator.resetForm();
                switchSection($("#city-form-section"));
                $("#city-form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/common/RegionApi(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#CityGrid').data('kendoGrid').dataSource.read();
                    $('#CityGrid').data('kendoGrid').refresh();

                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            var isNew = (self.id() == 0);

            if (!$("#city-form-section-form").valid()) {
                return false;
            }

            var order = self.order();
            if (!order) {
                order = null;
            }

            var record = {
                Id: self.id(),
                Name: self.name(),
                RegionType: 'City',
                ParentId: self.parentId(),
                Order: order,
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

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
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

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
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
    };

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.selectedContinentId = ko.observable(0);
        self.selectedCountryId = ko.observable(0);
        self.selectedStateId = ko.observable(0);

        self.country = false;
        self.state = false;
        self.city = false;
        self.settings = false;

        self.activate = function () {
            self.country = new CountryModel();
            self.state = new StateModel();
            self.city = new CityModel();
            self.settings = new SettingsModel();
        };
        self.attached = function () {
            currentSection = $("#main-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/regions/get-translations",
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

            $('#map').maphilight({
                fade: false
            });

            self.country.init();
            self.state.init();
            self.city.init();
            self.settings.init();
        };
        self.showCountries = function (continentId) {
            self.selectedContinentId(continentId);

            var grid = $('#CountryGrid').data('kendoGrid');
            grid.dataSource.transport.options.read.url = "/odata/kore/common/RegionApi?$filter=RegionType eq Kore.Web.Common.Areas.Admin.Regions.Domain.RegionType'Country' and ParentId eq " + continentId;
            grid.dataSource.page(1);
            //grid.dataSource.read();
            //grid.refresh();

            switchSection($("#country-grid-section"));
        };
        self.showSettings = function (regionId) {
            self.settings.regionId(regionId);
            switchSection($("#settings-grid-section"));
        };
    };

    viewModel = new ViewModel();
    return viewModel;
});