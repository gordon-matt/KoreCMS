'use strict'

function viewDetails(id) {
}

$(document).ready(function () {
    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/plugins/simple-commerce/OrderApi",
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
                        Id: { type: "number" },
                        Status: { type: "number" },
                        PaymentStatus: { type: "number" },
                        OrderDateUtc: { type: "date" },
                        OrderTotal: { type: "number" }
                    }
                }
            },
            pageSize: gridPageSize,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "OrderDateUtc", dir: "desc" }
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
            title: "Order ID",
            filterable: false
        }, {
            field: "Status",
            title: "Status",
            filterable: true
        }, {
            field: "PaymentStatus",
            title: "Payment Status",
            filterable: true
        }, {
            field: "OrderDateUtc",
            title: "Date",
            format: "{0:yyyy-MM-dd}",
            filterable: true
        }, {
            field: "OrderTotal",
            title: "Total",
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewDetails(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.View + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 120
        }]
    });
});