define(function (require) {
    'use strict'

    var $ = require('jquery');
    var ko = require('knockout');

    require('kendo');
    require('notify');

    require('kore-common');
    require('kore-section-switching');

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.id = ko.observable(emptyGuid);
        self.eventDateTime = ko.observable(null);
        self.eventLevel = ko.observable(null);
        self.userName = ko.observable(null);
        self.machineName = ko.observable(null);
        self.eventMessage = ko.observable(null);
        self.errorSource = ko.observable(null);
        self.errorClass = ko.observable(null);
        self.errorMethod = ko.observable(null);
        self.errorMessage = ko.observable(null);
        self.innerErrorMessage = ko.observable(null);

        self.attached = function () {
            currentSection = $("#grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/log/get-translations",
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
                            url: "/odata/kore/web/LogApi",
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
                                EventLevel: { type: "string" },
                                EventMessage: { type: "string" },
                                EventDateTime: { type: "date" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "EventDateTime", dir: "desc" }
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
                    field: "EventLevel",
                    title: self.translations.Columns.EventLevel,
                    filterable: true
                }, {
                    field: "EventMessage",
                    title: self.translations.Columns.EventMessage,
                    filterable: true
                }, {
                    field: "EventDateTime",
                    title: self.translations.Columns.EventDateTime,
                    format: "{0:G}",
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a data-bind="click: view.bind($data,\'#=Id#\')" class="btn btn-default btn-xs">' + self.translations.View + '</a>' +
                        '<a data-bind="click: remove.bind($data,\'#=Id#\')" class="btn btn-danger btn-xs">' + self.translations.Delete + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 120
                }]
            });
        };
        self.remove = function (id) {
            if (confirm(self.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/web/LogApi(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#Grid').data('kendoGrid').dataSource.read();
                    $('#Grid').data('kendoGrid').refresh();

                    $.notify(self.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.view = function (id) {
            $.ajax({
                url: "/odata/kore/web/LogApi(" + id + ")",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.eventDateTime(json.EventDateTime);
                self.eventLevel(json.EventLevel);
                self.userName(json.UserName);
                self.machineName(json.MachineName);
                self.eventMessage(json.EventMessage);
                self.errorSource(json.ErrorSource);
                self.errorClass(json.ErrorClass);
                self.errorMethod(json.ErrorMethod);
                self.errorMessage(json.ErrorMessage);
                self.innerErrorMessage(json.InnerErrorMessage);

                switchSection($("#details-section"));
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.cancel = function () {
            switchSection($("#grid-section"));
        };
        self.clear = function () {
            if (confirm(self.translations.ClearConfirm)) {
                $.ajax({
                    url: "/odata/kore/web/LogApi/Default.Clear",
                    type: "POST"
                })
                .done(function (json) {
                    $('#Grid').data('kendoGrid').dataSource.read();
                    $('#Grid').data('kendoGrid').refresh();

                    $.notify(self.translations.ClearSuccess, "success");
                    //setTimeout(function () {
                    //    window.location.reload();
                    //}, 500);
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.translations.ClearError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
    };

    var viewModel = new ViewModel();
    return viewModel;
});