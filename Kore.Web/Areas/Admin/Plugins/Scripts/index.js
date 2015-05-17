//'use strict'

//function install(systemName) {
//    $.ajax({
//        url: "/odata/kore/web/PluginApi/Install",
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        data: JSON.stringify({
//            systemName: systemName
//        }),
//        dataType: "json",
//        async: false
//    })
//    .done(function (json) {
//        $('#Grid').data('kendoGrid').dataSource.read();
//        $('#Grid').data('kendoGrid').refresh();

//        $.notify(translations.InstallPluginSuccess, "success");
//    })
//    .fail(function (jqXHR, textStatus, errorThrown) {
//        $.notify(translations.InstallPluginError, "error");
//          console.log(textStatus + ': ' + errorThrown);
//    });
//}

//function uninstall(systemName) {
//    $.ajax({
//        url: "/odata/kore/web/PluginApi/Uninstall",
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        data: JSON.stringify({
//            systemName: systemName
//        }),
//        dataType: "json",
//        async: false
//    })
//    .done(function (json) {
//        $('#Grid').data('kendoGrid').dataSource.read();
//        $('#Grid').data('kendoGrid').refresh();

//        $.notify(translations.UninstallPluginSuccess, "success");
//    })
//    .fail(function (jqXHR, textStatus, errorThrown) {
//        $.notify(translations.UninstallPluginError, "error");
//        console.log(textStatus + ': ' + errorThrown);
//    });
//}

//$(document).ready(function () {

//    $("#Grid").kendoGrid({
//        data: null,
//        dataSource: {
//            type: "odata",
//            transport: {
//                read: {
//                    url: "/odata/kore/web/PluginApi",
//                    dataType: "json"
//                }
//            },
//            schema: {
//                data: function (data) {
//                    return data.value;
//                },
//                total: function (data) {
//                    return data["odata.count"];
//                },
//                model: {
//                    fields: {
//                        Group: { type: "string" },
//                        FriendlyName: { type: "string" },
//                        Installed: { type: "boolean" }
//                    }
//                }
//            },
//            pageSize: gridPageSize,
//            serverPaging: true,
//            serverFiltering: true,
//            serverSorting: true,
//            sort: { field: "Group", dir: "asc" }
//        },
//        filterable: true,
//        sortable: {
//            allowUnsort: false
//        },
//        pageable: {
//            refresh: true
//        },
//        scrollable: false,
//        columns: [{
//            field: "Group",
//            title: "Group",
//            filterable: true
//        }, {
//            field: "FriendlyName",
//            title: "Plugin Info",
//            template: '<b>#:FriendlyName#</b>' +
//                '<br />Version: #:Version#' +
//                '<br />Author: #:Author#' +
//                '<br />SystemName: #:SystemName#' +
//                '<br />DisplayOrder: #:DisplayOrder#' +
//                '<br />Installed: <i class="fa #=Installed ? \'fa-check-circle fa-2x text-success\' : \'fa-times-circle fa-2x text-danger\'#"></i>',
//            filterable: false
//        }, {
//            field: "Installed",
//            title: " ",
//            template:
//                '# if(Installed) {# <a href="/admin/plugins/uninstall/#=replaceAll(SystemName, ".", "-")#" class="btn btn-default btn-sm">Uninstall</a> #} ' +
//                'else {# <a href="/admin/plugins/install/#=replaceAll(SystemName, ".", "-")#" class="btn btn-success btn-sm">Install</a> #} #',
//            attributes: { "class": "text-center" },
//            filterable: false,
//            width: 130
//        }]
//    });
//});