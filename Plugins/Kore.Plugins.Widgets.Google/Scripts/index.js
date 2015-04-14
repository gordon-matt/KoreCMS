'use strict'

var odataBaseUrl = "/odata/kore/plugins/google/GoogleXmlSitemap/";

$(document).ready(function () {
    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: odataBaseUrl + "GetConfig",
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST"
                },
                update: {
                    url: odataBaseUrl + "SetConfig",
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return kendo.stringify({
                            id: options.Id
                        });
                    }
                    else if (operation === "update") {
                        return kendo.stringify({
                            id: options.Id,
                            entity: options
                        });
                    }
                }
            },
            schema: {
                data: function (data) {
                    return data.value;
                },
                total: function (data) {
                    return data.value.length;
                },
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", editable: false },
                        Location: { type: "string", editable: false },
                        ChangeFrequency: { type: "number" },
                        Priority: { type: "number" }
                    }
                }
            },
            batch: false,
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            sort: { field: "Location", dir: "asc" }
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
            field: "Id",
            title: "Id",
            filterable: false
        }, {
            field: "Location",
            title: "Location",
            filterable: true
        }, {
            field: "ChangeFrequency",
            title: "ChangeFrequency",
            filterable: false
        }, {
            field: "Priority",
            title: "Priority",
            filterable: true
        }, {
            command: ["edit"],
            title: "&nbsp;",
            attributes: { "class": "text-center" },
            filterable: false,
            width: 200
        }],
        editable: "inline"
    });
});