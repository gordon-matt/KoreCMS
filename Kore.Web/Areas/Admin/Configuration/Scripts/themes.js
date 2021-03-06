﻿//define(function (require) {
define(['jquery', 'knockout', 'kendo', 'notify'], function ($, ko, kendo, notify) {
    'use strict'

    //var $ = require('jquery');
    //var ko = require('knockout');

    //require('kendo');
    //require('notify');

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.attached = function () {
            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/configuration/themes/get-translations",
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
                            url: "/odata/kore/web/ThemeApi",
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
                                PreviewImageUrl: { type: "string" },
                                Title: { type: "string" },
                                PreviewText: { type: "string" },
                                SupportRtl: { type: "boolean" },
                                MobileTheme: { type: "boolean" },
                                IsDefaultDesktopTheme: { type: "boolean" },
                                IsDefaultMobileTheme: { type: "boolean" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Title", dir: "asc" }
                },
                dataBound: function (e) {
                    var body = this.element.find("tbody")[0];
                    if (body) {
                        ko.cleanNode(body);
                        ko.applyBindings(ko.dataFor(body), body);
                    }
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
                    field: "PreviewImageUrl",
                    title: self.translations.Columns.PreviewImageUrl,
                    template: '<img src="#=PreviewImageUrl#" alt="#=Title#" class="thumbnail" style="max-width:200px;" />',
                    filterable: false,
                    width: 200
                }, {
                    field: "Title",
                    title: self.translations.Columns.Title,
                    filterable: true
                }, {
                    field: "SupportRtl",
                    title: self.translations.Columns.SupportRtl,
                    template: '<i class="fa #=SupportRtl ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "MobileTheme",
                    title: self.translations.Columns.MobileTheme,
                    template: '<i class="fa #=MobileTheme ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "IsDefaultDesktopTheme",
                    title: self.translations.Columns.IsDefaultDesktopTheme,
                    template:
                        '# if(IsDefaultDesktopTheme) {# <i class="fa fa-check-circle fa-2x text-success"></i> #} ' +
                        'else {# <button type="button" data-bind="click: setDesktopTheme.bind($data,\'#=Title#\')" class="btn btn-default btn-sm">' + self.translations.Set + '</button> #} #',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 130
                }, {
                    field: "IsDefaultMobileTheme",
                    title: self.translations.Columns.IsDefaultMobileTheme,
                    template:
                        '# if(IsDefaultMobileTheme) {# <i class="fa fa-check-circle fa-2x text-success"></i> #} ' +
                        'else {# <button type="button" data-bind="click: setMobileTheme.bind($data,\'#=Title#\')" class="btn btn-default btn-sm">' + self.translations.Set + '</button> #} #',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 150
                }]
            });
        };
        self.setDesktopTheme = function (name) {
            $.ajax({
                url: "/odata/kore/web/ThemeApi/Default.SetDesktopTheme",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ themeName: name }),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();
                $.notify(self.translations.SetDesktopThemeSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.SetDesktopThemeError + ": " + jqXHR.responseText || textStatus, "error");
            });
        };
        self.setMobileTheme = function (name) {
            $.ajax({
                url: "/odata/kore/web/ThemeApi/Default.SetMobileTheme",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ themeName: name }),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();
                $.notify(self.translations.SetMobileThemeSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.SetMobileThemeError + ": " + jqXHR.responseText || textStatus, "error");
            });
        };
    };

    var viewModel = new ViewModel();
    return viewModel;
});