var viewModel;

define(function (require) {
    'use strict'

    viewModel = null;

    var $ = require('jquery');
    var ko = require('knockout');

    require('kendo');
    //require('notify');

    //require('kore-section-switching');

    var orderApiUrl = "/odata/kore/plugins/simple-commerce/SimpleCommerceOrderApi";

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.attached = function () {
            //currentSection = $("#grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/plugins/ecommerce/simple/orders/get-translations",
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
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: orderApiUrl,
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
                                Id: { type: "number" },
                                Status: { type: "number" },
                                PaymentStatus: { type: "number" },
                                OrderDateUtc: { type: "date" },
                                OrderTotal: { type: "number" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
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
                    title: self.translations.Columns.Id,
                    filterable: false
                }, {
                    field: "Status",
                    title: self.translations.Columns.Status,
                    filterable: true
                }, {
                    field: "PaymentStatus",
                    title: self.translations.Columns.PaymentStatus,
                    filterable: true
                }, {
                    field: "OrderDateUtc",
                    title: self.translations.Columns.OrderDateUtc,
                    format: "{0:G}",
                    filterable: true
                }, {
                    field: "OrderTotal",
                    title: self.translations.Columns.OrderTotal,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewDetails(\'#=Id#\')" class="btn btn-default btn-xs">' + self.translations.View + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 120
                }]
            });
        };
        self.viewDetails = function (id) {
        };
    };

    viewModel = new ViewModel();
    return viewModel;
});