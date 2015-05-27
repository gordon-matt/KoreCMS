'use strict'

$(document).ready(function () {
    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/web/PluginApi",
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
                        Group: { type: "string" },
                        FriendlyName: { type: "string" },
                        Installed: { type: "boolean" }
                    }
                }
            },
            pageSize: gridPageSize,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "Group", dir: "asc" }
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
            field: "Group",
            title: translations.Columns.Group,
            filterable: true
        }, {
            field: "FriendlyName",
            title: translations.Columns.PluginInfo,
            template: '<b>#:FriendlyName#</b>' +
                '<br />Version: #:Version#' +
                '<br />Author: #:Author#' +
                '<br />SystemName: #:SystemName#' +
                '<br />DisplayOrder: #:DisplayOrder#' +
                '<br />Installed: <i class="kore-icon #=Installed ? \'kore-icon-ok-circle kore-icon-2x text-success\' : \'kore-icon-no-circle kore-icon-2x text-danger\'#"></i>',
            filterable: false
        }, {
            field: "Installed",
            title: " ",
            template:
                '# if(Installed) {# <a href="/admin/plugins/uninstall/#=replaceAll(SystemName, ".", "-")#" class="btn btn-default btn-sm">#=translations.Uninstall#</a> #} ' +
                'else {# <a href="/admin/plugins/install/#=replaceAll(SystemName, ".", "-")#" class="btn btn-success btn-sm">#=translations.Install#</a> #} #',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 130
        }]
    });
});