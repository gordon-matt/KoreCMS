'use strict'

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.name = ko.observable(null);
    self.cultureCode = ko.observable(null);
    self.isRTL = ko.observable(false);
    self.isEnabled = ko.observable(false);
    self.sortOrder = ko.observable(0);

    self.create = function () {
        self.id(emptyGuid);
        self.name('');
        self.cultureCode('');
        self.isRTL(false);
        self.isEnabled(false);
        self.sortOrder(0);

        self.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/LanguageApi(guid'" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.cultureCode(json.CultureCode);
            self.isRTL(json.IsRTL);
            self.isEnabled(json.IsEnabled);
            self.sortOrder(json.SortOrder);

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/cms/LanguageApi(guid'" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.save = function () {

        if (!$("#form-section-form").valid()) {
            return false;
        }

        var cultureCode = self.cultureCode();
        if (cultureCode == '') {
            cultureCode = null;
        }

        var record = {
            Id: self.id(),
            Name: self.name(),
            CultureCode: cultureCode,
            IsRTL: self.isRTL(),
            IsEnabled: self.isEnabled(),
            SortOrder: self.sortOrder()
        };

        if (self.id() == emptyGuid) {
            // INSERT
            $.ajax({
                url: "/odata/kore/cms/LanguageApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                switchSection($("#grid-section"));

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
                url: "/odata/kore/cms/LanguageApi(guid'" + self.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                switchSection($("#grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#grid-section"));
    };

    self.onCultureCodeChanged = function () {
        var cultureName = $('#CultureCode option:selected').text();
        self.name(cultureName);
    };

    self.clear = function () {
        $.ajax({
            url: "/odata/kore/cms/LanguageApi/ResetLocalizableStrings",
            type: "POST"
        })
        .done(function (json) {
            $.notify(translations.ResetLocalizableStringsSuccess, "success");
            setTimeout(function () {
                window.location.reload();
            }, 500);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.ResetLocalizableStringsError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.validator = $("#form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 },
            CultureCode: { required: true, maxlength: 10 },
            SortOrder: { required: true }
        }
    });
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/cms/LanguageApi",
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
                        Name: { type: "string" },
                        CultureCode: { type: "string" },
                        IsEnabled: { type: "boolean" },
                        SortOrder: { type: "number" }
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
            field: "CultureCode",
            title: translations.Columns.CultureCode,
            filterable: true,
            width: 70
        }, {
            field: "IsEnabled",
            title: translations.Columns.IsEnabled,
            template: '<i class="fa #=IsEnabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
            attributes: { "class": "text-center" },
            filterable: true,
            width: 70
        }, {
            field: "SortOrder",
            title: translations.Columns.SortOrder,
            filterable: true,
            width: 70
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '<a href="/admin/localization/localizable-strings/#=Id#" class="btn btn-primary btn-xs">' + translations.Localize + '</a>' +
                '</div>',
                //TODO: '<a onclick="viewModel.setDefault(\'#=Id#\', #=IsEnabled#)" class="btn btn-default btn-xs">Set Default</a></div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 170
        }]
    });
});