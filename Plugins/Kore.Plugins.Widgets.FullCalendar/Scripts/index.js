'use strict'

var calendarApiUrl = "/odata/kore/plugins/FullCalendar/CalendarApi";
var eventApiUrl = "/odata/kore/plugins/FullCalendar/CalendarEventApi";

var EventModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.calendarId = ko.observable(0);
    self.name = ko.observable(null);
    self.startDateTime = ko.observable('');
    self.endDateTime = ko.observable('');

    self.create = function () {
        self.id(0);
        self.calendarId(viewModel.selectedCalendarId());
        self.name(null);
        self.startDateTime('');
        self.endDateTime('');

        self.validator.resetForm();
        switchSection($("#events-edit-section"));
        $("#events-edit-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: eventApiUrl + "(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.calendarId(json.CalendarId);
            self.name(json.Name);
            self.startDateTime(json.StartDateTime);
            self.endDateTime(json.EndDateTime);

            self.validator.resetForm();
            switchSection($("#events-edit-section"));
            $("#events-edit-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: eventApiUrl + "(" + id + ")",
                type: "DELETE",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#EventGrid').data('kendoGrid').dataSource.read();
                $('#EventGrid').data('kendoGrid').refresh();
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

        if (!$("#events-edit-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            CalendarId: self.calendarId(),
            Name: self.name(),
            StartDateTime: self.startDateTime(),
            EndDateTime: self.endDateTime()
        };

        if (isNew) {
            $.ajax({
                url: eventApiUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#EventGrid').data('kendoGrid').dataSource.read();
                $('#EventGrid').data('kendoGrid').refresh();

                switchSection($("#events-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: eventApiUrl + "(" + self.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#EventGrid').data('kendoGrid').dataSource.read();
                $('#EventGrid').data('kendoGrid').refresh();

                switchSection($("#events-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.goBack = function () {
        switchSection($("#grid-section"));
    };

    self.cancel = function () {
        switchSection($("#events-grid-section"));
    };

    self.validator = $("#events-edit-section-form").validate({
        rules: {
            Event_Name: { required: true, maxlength: 255 },
            Event_StartDateTime: { required: true, date: true },
            Event_EndDateTime: { required: true, date: true }
        }
    });
};

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.name = ko.observable(null);

    self.selectedCalendarId = ko.observable(0);

    self.eventModel = new EventModel();

    self.create = function () {
        self.id(0);
        self.name(null);

        self.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: calendarApiUrl + "(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);

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
                url: calendarApiUrl + "(" + id + ")",
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

        var record = {
            Id: self.id(),
            Name: self.name()
        };

        if (self.id() == emptyGuid) {
            // INSERT
            $.ajax({
                url: calendarApiUrl,
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
                url: calendarApiUrl + "(" + self.id() + ")",
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

    self.showEvents = function (calendarId) {
        self.selectedCalendarId(calendarId);

        var grid = $('#EventGrid').data('kendoGrid');
        grid.dataSource.transport.options.read.url = eventApiUrl + "?$filter=CalendarId eq " + continentId;
        grid.dataSource.page(1);

        switchSection($("#events-grid-section"));
    };

    self.validator = $("#form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 }
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
                    url: calendarApiUrl,
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
                '<a onclick="viewModel.showEvents(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Events + '</a>' +
                '<a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 180
        }]
    });

    $("#EventGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: eventApiUrl,
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
                        StartDateTime: { type: "date" },
                        EndDateTime: { type: "date" }
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
            field: "StartDateTime",
            title: translations.Columns.StartDateTime,
            filterable: true
        }, {
            field: "EndDateTime",
            title: translations.Columns.EndDateTime,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group">' +
                '<a onclick="viewModel.eventModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.eventModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 180
        }]
    });
});
