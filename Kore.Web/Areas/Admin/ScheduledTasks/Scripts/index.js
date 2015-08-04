var viewModel;

define(['jquery', 'knockout', 'kendo', 'notify'],
function ($, ko, kendo, notify) {
    'use strict'

    var odataBaseUrl = "/odata/kore/web/ScheduledTaskApi";

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.attached = function () {
            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/scheduledtasks/get-translations",
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

            $("#Grid").kendoGrid({
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: odataBaseUrl,
                            type: "GET",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                        },
                        update: {
                            url: function (data) {
                                return odataBaseUrl + '(' + data.Id + ')'
                            },
                            type: "PUT",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                        }
                    },
                    schema: {
                        data: function (data) {
                            return data.value;
                        },
                        total: function (data) {
                            return data.value.length; // Special case
                            //return data["odata.count"];
                        },
                        model: {
                            id: "Id",
                            fields: {
                                Name: { type: "string", editable: true },
                                Seconds: { type: "number", editable: true },
                                Enabled: { type: "boolean", editable: true },
                                StopOnError: { type: "boolean", editable: true },
                                LastStartUtc: { type: "date", editable: false },
                                LastEndUtc: { type: "date", editable: false },
                                LastSuccessUtc: { type: "date", editable: false },
                                Id: { type: "number", editable: false }
                            }
                        }
                    },
                    batch: false,
                    pageSize: self.gridPageSize,
                    serverPaging: false,
                    serverFiltering: false,
                    serverSorting: false,
                    //serverPaging: true,  // Special case
                    //serverFiltering: true,
                    //serverSorting: true,
                    sort: { field: "Name", dir: "asc" }
                },
                dataBound: function (e) {
                    $(".k-grid-edit").html("Edit");
                    $(".k-grid-edit").addClass("btn btn-default btn-sm");
                },
                edit: function (e) {
                    $(".k-grid-update").html("Update");
                    $(".k-grid-cancel").html("Cancel");
                    $(".k-grid-update").addClass("btn btn-success btn-sm");
                    $(".k-grid-cancel").addClass("btn btn-default btn-sm");
                },
                cancel: function (e) {
                    setTimeout(function () {
                        $(".k-grid-edit").html("Edit");
                        $(".k-grid-edit").addClass("btn btn-default btn-sm");
                    }, 0);
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
                    title: self.translations.Columns.Name,
                    filterable: true
                }, {
                    field: "Seconds",
                    title: self.translations.Columns.Seconds,
                    width: 70,
                    filterable: false
                }, {
                    field: "Enabled",
                    title: self.translations.Columns.Enabled,
                    template: '<i class="kore-icon #=Enabled ? \'kore-icon-ok text-success\' : \'kore-icon-no text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "StopOnError",
                    title: self.translations.Columns.StopOnError,
                    template: '<i class="kore-icon #=StopOnError ? \'kore-icon-ok text-success\' : \'kore-icon-no text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "LastStartUtc",
                    title: self.translations.Columns.LastStartUtc,
                    width: 200,
                    type: "date",
                    format: "{0:G}",
                    filterable: false
                }, {
                    field: "LastEndUtc",
                    title: self.translations.Columns.LastEndUtc,
                    width: 200,
                    type: "date",
                    format: "{0:G}",
                    filterable: false
                }, {
                    field: "LastSuccessUtc",
                    title: self.translations.Columns.LastSuccessUtc,
                    width: 200,
                    type: "date",
                    format: "{0:G}",
                    filterable: false
                }, {
                    field: "Id",
                    title: " ",
                    width: 80,
                    filterable: false,
                    template: '<a onclick="viewModel.runNow(#=Id#)" class="btn btn-primary btn-sm">Run Now</a>'
                }, {
                    command: ["edit"],
                    title: "&nbsp;",
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 200
                }],
                editable: "inline"
            });
        };
        self.runNow = function (id) {
            $.ajax({
                url: "/odata/kore/web/ScheduledTaskApi/RunNow",
                type: "POST",
                data: JSON.stringify({
                    taskId: id
                }),
                contentType: "application/json; charset=utf-8"
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();
                $.notify(self.translations.ExecutedTaskSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.ExecutedTaskError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
    }

    viewModel = new ViewModel();
    return viewModel;
});