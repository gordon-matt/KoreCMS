'use strict'

var odataBaseUrl = "/odata/kore/plugins/google/GoogleXmlSitemap/";

var changeFrequencies = [
    {
        "Id": 0,
        "Name": "Always"
    },
    {
        "Id": 1,
        "Name": "Hourly"
    },
    {
        "Id": 2,
        "Name": "Daily"
    },
    {
        "Id": 3,
        "Name": "Weekly"
    },
    {
        "Id": 4,
        "Name": "Monthly"
    },
    {
        "Id": 5,
        "Name": "Yearly"
    },
    {
        "Id": 6,
        "Name": "Never"
    },
];

function changeFrequenciesDropDownEditor(container, options) {
    $('<input required data-text-field="Name" data-value-field="Id" data-bind="value:' + options.field + '"/>')
		.appendTo(container)
		.kendoDropDownList({
		    autoBind: false,
		    dataSource: new kendo.data.DataSource({
		        data: changeFrequencies
		    }),
		    template: "#=data.Name#"
		});
}

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
                            changeFrequency: options.ChangeFrequency,
                            priority: options.Priority
                            //entity: options
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
                        ChangeFrequency: { defaultValue: { Id: "3", Name: "Weekly" } },
                        Priority: { type: "number" }
                    }
                }
            },
            sync: function (e) {
                // Refresh grid after save (not ideal, but if we don't, then the enum column (ChangeFrequency) shows
                //  a number instead of the name). Haven't found a better solution yet.
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();
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
            filterable: false,
            editor: changeFrequenciesDropDownEditor,
            template: "#=ChangeFrequency#"
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