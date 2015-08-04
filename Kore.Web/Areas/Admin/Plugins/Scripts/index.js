var viewModel;

define(['jquery', 'knockout', 'kendo', 'kore-common', 'notify'],
function ($, ko, kendo, kore_common, notify) {
    'use strict'

    //var $ = require('jquery');
    //var ko = require('knockout');

    //require('kendo');

    //require('kore-common');

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.attached = function () {
            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/plugins/get-translations",
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
                    pageSize: self.gridPageSize,
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
                    title: self.translations.Columns.Group,
                    filterable: true
                }, {
                    field: "FriendlyName",
                    title: self.translations.Columns.PluginInfo,
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
                        '# if(Installed) {# <a onclick="viewModel.uninstall(\'#=SystemName#\')" class="btn btn-default btn-sm">#=viewModel.translations.Uninstall#</a> #} ' +
                        'else {# <a onclick="viewModel.install(\'#=SystemName#\')" class="btn btn-success btn-sm">#=viewModel.translations.Install#</a> #} #',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 130
                }]
            });
        };
        self.install = function (systemName) {
            systemName = replaceAll(systemName, ".", "-");

            $.ajax({
                url: "/admin/plugins/install/" + systemName,
                type: "POST"
            })
            .done(function (json) {
                if (json.Success) {
                    $.notify(json.Message, "success");
                }
                else {
                    $.notify(json.Message, "error");
                }

                setTimeout(function () {
                    window.location.reload();
                }, 1000);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.ClearError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        self.uninstall = function (systemName) {
            systemName = replaceAll(systemName, ".", "-");

            $.ajax({
                url: "/admin/plugins/uninstall/" + systemName,
                type: "POST"
            })
            .done(function (json) {
                if (json.Success) {
                    $.notify(json.Message, "success");
                }
                else {
                    $.notify(json.Message, "error");
                }

                setTimeout(function () {
                    window.location.reload();
                }, 1000);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.ClearError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    }

    viewModel = new ViewModel();
    return viewModel;
});