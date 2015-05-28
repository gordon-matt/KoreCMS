'use strict'

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.eventDateTime = ko.observable(null);
    self.eventLevel = ko.observable(null);
    self.userName = ko.observable(null);
    self.machineName = ko.observable(null);
    self.eventMessage = ko.observable(null);
    self.errorSource = ko.observable(null);
    self.errorClass = ko.observable(null);
    self.errorMethod = ko.observable(null);
    self.errorMessage = ko.observable(null);
    self.innerErrorMessage = ko.observable(null);

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/web/LogApi(guid'" + id + "')",
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

    self.view = function (id) {
        $.ajax({
            url: "/odata/kore/web/LogApi(guid'" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.eventDateTime(json.EventDateTime);
            self.eventLevel(json.EventLevel);
            self.userName(json.UserName);
            self.machineName(json.MachineName);
            self.eventMessage(json.EventMessage);
            self.errorSource(json.ErrorSource);
            self.errorClass(json.ErrorClass);
            self.errorMethod(json.ErrorMethod);
            self.errorMessage(json.ErrorMessage);
            self.innerErrorMessage(json.InnerErrorMessage);

            switchSection($("#details-section"));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.cancel = function () {
        switchSection($("#grid-section"));
    };

    self.clear = function () {
        if (confirm(translations.ClearConfirm)) {
            $.ajax({
                url: "/odata/kore/web/LogApi/Clear",
                type: "POST"
            })
            .done(function (json) {
                $.notify(translations.ClearSuccess, "success");
                setTimeout(function () {
                    window.location.reload();
                }, 500);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.ClearError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    switchSection($("#grid-section"));

    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/web/LogApi",
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
                        EventLevel: { type: "string" },
                        EventMessage: { type: "string" },
                        EventDateTime: { type: "date" }
                    }
                }
            },
            pageSize: gridPageSize,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "EventDateTime", dir: "desc" }
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
            field: "EventLevel",
            title: translations.Columns.EventLevel,
            filterable: true
        }, {
            field: "EventMessage",
            title: translations.Columns.EventMessage,
            filterable: true
        }, {
            field: "EventDateTime",
            title: translations.Columns.EventDateTime,
            format: "{0:G}",
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group">' +
                '<a onclick="viewModel.view(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.View + '</a>' +
                '<a onclick="viewModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 120
        }]
    });
});