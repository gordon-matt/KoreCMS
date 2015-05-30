'use strict'

var odataBaseUrl = "/odata/kore/cms/XmlSitemapApi/";

function getChangeFrequencyIndex(name) {
    for (var i = 0; i < changeFrequencies.length; i++) {
        var item = changeFrequencies[i];
        if (item.Name == name) {
            return i;
        }
    }
    return 3;
};

function changeFrequenciesDropDownEditor(container, options) {
    $('<input id="test" required data-text-field="Name" data-value-field="Id" data-bind="value:' + options.field + '"/>')
		.appendTo(container)
		.kendoDropDownList({
		    autoBind: false,
		    dataSource: new kendo.data.DataSource({
		        data: changeFrequencies
		    }),
		    template: "#=data.Name#"
		});

    var selectedIndex = getChangeFrequencyIndex(options.model.ChangeFrequency);
    setTimeout(function () {
        var dropdownlist = $("#test").data("kendoDropDownList");
        dropdownlist.select(selectedIndex);
    }, 200);
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
                        Priority: { type: "number", validation: { min: 0.0, max: 1.0, step: 0.1 } }
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
            pageSize: gridPageSize,
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
            title: translations.Columns.Id,
            filterable: false
        }, {
            field: "Location",
            title: translations.Columns.Location,
            filterable: true
        }, {
            field: "ChangeFrequency",
            title: translations.Columns.ChangeFrequency,
            filterable: false,
            editor: changeFrequenciesDropDownEditor
        }, {
            field: "Priority",
            title: translations.Columns.Priority,
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

function generateFile() {
    if (confirm(translations.ConfirmGenerateFile)) {
        $.ajax({
            url: odataBaseUrl + "Generate",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            $.notify(translations.GenerateFileSuccess, "success");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GenerateFileError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    }
}