'use strict'

var odataBaseUrl = "/odata/kore/cms/LocalizableStringApi/";

$(document).ready(function () {
    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: odataBaseUrl + "GetComparitiveTable",

                    // Refer to note in LocalizableStringsController (may need to implement below code in future version of OData for Web API)
                    //  if we want to support filtering & sorting on the server side (which doesn't work for this kind of OData Action)
                    //url: function (data) {
                    //    var grid = $('#Grid').data('kendoGrid');

                    //    var params = {
                    //        page: grid.dataSource.page(),
                    //        sort: grid.dataSource.sort(),
                    //        filter: grid.dataSource.filter()
                    //    };

                    //    var queryString = "page=" + (params.page || "~");

                    //    if (params.sort) {
                    //        queryString += "&$orderby=";
                    //        var isFirst = true;
                    //        $.each(params.sort, function () {
                    //            alert(JSON.stringify(this));
                    //            if (!isFirst) {
                    //                queryString += ",";
                    //            }
                    //            else {
                    //                isFirst = false;
                    //            }
                    //            queryString += this.field + " " + this.dir;
                    //        });
                    //    }

                    //    if (params.filter) {
                    //        queryString += "&$filter=";
                    //        var isFirst = true;
                    //        $.each(params.filter, function () {
                    //            if (!isFirst) {
                    //                queryString += " and ";
                    //            }
                    //            else {
                    //                isFirst = false;
                    //            }
                    //            queryString += this.field + " " + this.operator + " '" + this.value + "'";
                    //        });
                    //    }

                    //    // odataBaseUrl was: "/odata/kore/cms/ComparitiveLocalizableStrings/" when I was testing this...
                    //    return odataBaseUrl + "GetComparitiveTable?" + queryString;
                    //},
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST"
                },
                update: {
                    url: odataBaseUrl + "PutComparitive",
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST"
                },
                destroy: {
                    url: odataBaseUrl + "DeleteComparitive",
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return kendo.stringify({
                            cultureCode: cultureCode
                        });
                    }
                    else if (operation === "update") {
                        return kendo.stringify({
                            cultureCode: cultureCode,
                            key: options.Key,
                            entity: options
                        });
                    }
                    else if (operation === "destroy") {
                        return kendo.stringify({
                            cultureCode: cultureCode,
                            key: options.Key
                        });
                    }
                }
            },
            schema: {
                data: function (data) {
                    return data.value;
                },
                total: function (data) {
                    return data.value.length; // Special case (refer to note in LocalizableStringApiController)
                    //return data["odata.count"];
                },
                model: {
                    id: "Key",
                    fields: {
                        Key: { type: "string", editable: false },
                        InvariantValue: { type: "string", editable: false },
                        LocalizedValue: { type: "string" }
                    }
                }
            },
            batch: false,
            pageSize: gridPageSize,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            //serverPaging: true,  // Special case (refer to note in LocalizableStringsController)
            //serverFiltering: true,
            //serverSorting: true,
            sort: { field: "Name", dir: "asc" }
        },
        dataBound: function (e) {
            $(".k-grid-edit").html("Edit");
            $(".k-grid-delete").html("Delete");
            $(".k-grid-edit").addClass("btn btn-default btn-sm");
            $(".k-grid-delete").addClass("btn btn-danger btn-sm");
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
                $(".k-grid-delete").html("Delete");
                $(".k-grid-edit").addClass("btn btn-default btn-sm");
                $(".k-grid-delete").addClass("btn btn-danger btn-sm");
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
            field: "Key",
            title: translations.Columns.Key,
            filterable: true
        }, {
            field: "InvariantValue",
            title: translations.Columns.InvariantValue,
            filterable: true
        }, {
            field: "LocalizedValue",
            title: translations.Columns.LocalizedValue,
            filterable: true
        }, {
            command: ["edit", "destroy"],
            title: "&nbsp;",
            attributes: { "class": "text-center" },
            filterable: false,
            width: 200
        }],
        editable: "inline"
    });
});

function exportFile() {
    var downloadForm = $("<form>")
        .attr("method", "GET")
        .attr("action", "/admin/localization/localizable-strings/export/" + cultureCode);
    $("body").append(downloadForm);
    downloadForm.submit();
    downloadForm.remove();
};