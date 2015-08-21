//define(function (require) {
define(['jquery', 'knockout', 'kendo', 'notify'], function ($, ko, kendo, notify) {
    'use strict'

    //var $ = require('jquery');
    //var ko = require('knockout');

    //require('kendo');
    //require('notify');

    var odataBaseUrl = "/odata/kore/cms/XmlSitemapApi/";

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.changeFrequencies = [];

        self.attached = function () {
            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/sitemap/xml-sitemap/get-translations",
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

            self.changeFrequencies = [
                { "Id": 0, "Name": self.translations.ChangeFrequencies.Always },
                { "Id": 1, "Name": self.translations.ChangeFrequencies.Hourly },
                { "Id": 2, "Name": self.translations.ChangeFrequencies.Daily },
                { "Id": 3, "Name": self.translations.ChangeFrequencies.Weekly },
                { "Id": 4, "Name": self.translations.ChangeFrequencies.Monthly },
                { "Id": 5, "Name": self.translations.ChangeFrequencies.Yearly },
                { "Id": 6, "Name": self.translations.ChangeFrequencies.Never }
            ];

            $("#Grid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: odataBaseUrl + "Default.GetConfig",
                            dataType: "json",
                            contentType: "application/json",
                            type: "POST"
                        },
                        update: {
                            url: odataBaseUrl + "Default.SetConfig",
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
                    // Don't change this to self.gridPageSize. There's a bug with client-side paging. If we set self.gridPageSize here, then paging gets messed up.
                    //  For more info, see: http://stackoverflow.com/questions/31810484/kendo-grid-misbehaving-in-certain-situations-with-durandal-requirejs?noredirect=1#comment52004513_31810484
                    pageSize: 15,
                    serverPaging: false,
                    serverFiltering: false,
                    serverSorting: false,
                    sort: { field: "Location", dir: "asc" }
                },
                dataBound: function (e) {
                    var body = this.element.find("tbody")[0];
                    if (body) {
                        ko.cleanNode(body);
                        ko.applyBindings(ko.dataFor(body), body);
                    }

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
                    title: self.translations.Columns.Id,
                    filterable: false
                }, {
                    field: "Location",
                    title: self.translations.Columns.Location,
                    filterable: true
                }, {
                    field: "ChangeFrequency",
                    title: self.translations.Columns.ChangeFrequency,
                    filterable: false,
                    editor: self.changeFrequenciesDropDownEditor
                }, {
                    field: "Priority",
                    title: self.translations.Columns.Priority,
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
        };
        self.getChangeFrequencyIndex = function (name) {
            for (var i = 0; i < self.changeFrequencies.length; i++) {
                var item = self.changeFrequencies[i];
                if (item.Name == name) {
                    return i;
                }
            }
            return 3;
        };
        self.changeFrequenciesDropDownEditor = function (container, options) {
            $('<input id="test" required data-text-field="Name" data-value-field="Id" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    autoBind: false,
                    dataSource: new kendo.data.DataSource({
                        data: self.changeFrequencies
                    }),
                    template: "#=data.Name#"
                });

            var selectedIndex = self.getChangeFrequencyIndex(options.model.ChangeFrequency);
            setTimeout(function () {
                var dropdownlist = $("#test").data("kendoDropDownList");
                dropdownlist.select(selectedIndex);
            }, 200);
        }
        self.generateFile = function () {
            if (confirm(self.translations.ConfirmGenerateFile)) {
                $.ajax({
                    url: odataBaseUrl + "Default.Generate",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $.notify(self.translations.GenerateFileSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.translations.GenerateFileError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        }
    };

    var viewModel = new ViewModel();
    return viewModel;
});