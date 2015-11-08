define(function (require) {
    'use strict'

    var $ = require('jquery');
    var ko = require('knockout');

    require('jqueryval');
    require('kendo');
    require('notify');

    require('kore-section-switching');
    require('kore-jqueryval');

    var baseUrl = "/odata/kore/watchdog/WatchdogInstanceApi";

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;
        self.allowAddRemove = true;
        self.onlyShowWatched = false;

        self.url = ko.observable(null);
        self.password = ko.observable(null);

        self.validator = false;

        self.attached = function () {
            currentSection = $("#grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/maintenance/watchdog/get-translations",
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
            self.allowAddRemove = ($("#AllowAddRemove").val() === 'true');
            self.onlyShowWatched = ($("#OnlyShowWatched").val() === 'true');

            self.validator = $("#form-section-form").validate({
                rules: {
                    Url: { required: true, maxlength: 255 },
                    Password: { required: true, maxlength: 255 }
                }
            });
            $("#Grid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: baseUrl,
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
                            id: "Id",
                            fields: {
                                Id: { type: "number" },
                                Url: { type: "string" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Url", dir: "asc" }
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
                    field: "Url",
                    title: self.translations.Columns.Url,
                    filterable: true
                }],
                detailTemplate: kendo.template($("#services-template").html()),
                detailInit: self.detailInit,
                dataBound: function () {
                    this.expandRow(this.tbody.find("tr.k-master-row").first());
                },
            });
        };
        self.create = function () {
            self.url(null);
            self.password(null);

            self.validator.resetForm();
            switchSection($("#form-section"));
        };
        self.delete = function (id) {
            if (confirm(self.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: baseUrl + "(" + id + ")",
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
        self.save = function () {
            if (!$("#form-section-form").valid()) {
                return false;
            }

            var record = {
                Url: self.url(),
                Password: self.password(),
            };

            // INSERT
            $.ajax({
                url: baseUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                switchSection($("#grid-section"));

                $.notify(self.translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.cancel = function () {
            switchSection($("#grid-section"));
        };
        self.stopService = function (instanceId, name) {
            if (confirm(self.translations.ConfirmStopService)) {
                self.executeAction(baseUrl + "/Default.StopService", instanceId, name, self.translations.StopServiceError, self.translations.StopServiceSuccess, false);
            }
        };
        self.startService = function (instanceId, name) {
            self.executeAction(baseUrl + "/Default.StartService", instanceId, name, self.translations.StartServiceError, self.translations.StartServiceSuccess, false);
        };
        self.restartService = function (instanceId, name) {
            self.executeAction(baseUrl + "/Default.RestartService", instanceId, name, self.translations.RestartServiceError, self.translations.RestartServiceSuccess, false);
        };
        self.addService = function (instanceId, name) {
            self.executeAction(baseUrl + "/Default.AddService", instanceId, name, self.translations.AddServiceError, self.translations.AddServiceSuccess, true);
        };
        self.removeService = function (instanceId, name) {
            if (confirm(self.translations.ConfirmRemoveService)) {
                self.executeAction(baseUrl + "/Default.RemoveService", instanceId, name, self.translations.RemoveServiceError, self.translations.RemoveServiceSuccess, true);
            }
        };
        self.executeAction = function (url, instanceId, name, successMsg, errorMsg, returnsVoid) {
            var data = {
                instanceId: instanceId,
                name: name
            };

            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(data),
                dataType: returnsVoid ? "text" : "json",
                async: false
            })
            .done(function (json) {
                $('#services-grid-' + instanceId).data('kendoGrid').dataSource.read();
                $('#services-grid-' + instanceId).data('kendoGrid').refresh();

                switchSection($("#grid-section"));

                $.notify(successMsg, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(errorMsg, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };

        self.detailInit = function (e) {
            var detailRow = e.detailRow;

            detailRow.find(".tabstrip").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });

            var detailGrid = detailRow.find(".services-grid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: function (data) {
                                return baseUrl + "/Default.GetServices(watchdogInstanceId=" + e.data.Id + ")";
                            },
                            dataType: "json"
                        },
                        parameterMap: function (options, operation) {
                            if (operation === "read") {
                                var paramMap = kendo.data.transports.odata.parameterMap(options, operation);

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
                            else {
                                return kendo.data.transports.odata.parameterMap(options, operation);
                            }
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
                            id: "Id",
                            fields: {
                                DisplayName: { type: "string" },
                                ServiceName: { type: "string" },
                                Status: { type: "string" },
                                IsWatched: { type: "boolean" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "DisplayName", dir: "asc" },
                },
                dataBound: function (e) {
                    var body = this.element.find("tbody")[0];
                    if (body) {
                        ko.cleanNode(body);
                        ko.applyBindings(ko.dataFor(body), body);
                    }
                },
                pageable: {
                    refresh: true
                },
                scrollable: false,
                columns: [{
                    field: "DisplayName",
                    title: self.translations.Columns.DisplayName,
                    filterable: true
                }, {
                    field: "ServiceName",
                    title: self.translations.Columns.ServiceName,
                    filterable: true
                }, {
                    field: "Status",
                    title: self.translations.Columns.Status,
                    filterable: true,
                    width: 100
                }, {
                    field: "IsWatched",
                    title: "&nbsp;",
                    template: '# if(IsWatched) {#<img src="/Plugins/Watchdog/Content/img/watchdog_32.png" alt="Watched" /> #} #',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 70
                }, {
                    field: "WatchdogInstanceId",
                    title: " ",
                    template: function(dataItem) {
                        var instanceId = dataItem.WatchdogInstanceId;
                        var name = dataItem.ServiceName;

                        var s = '<div class="btn-group">';

                        if (dataItem.Status == "Stopped") {
                            s+= '<a data-bind="click: startService.bind($data,' + instanceId + ', \'' + name + '\')" class="btn btn-success btn-sm"><i class="kore-icon kore-icon-start"></i></a>';
                        }
                        else if (dataItem.Status == "Running") {
                            s+= '<a data-bind="click: stopService.bind($data,' + instanceId + ', \'' + name + '\')" class="btn btn-danger btn-sm"><i class="kore-icon kore-icon-stop"></i></a>' +
                            '<a data-bind="click: restartService.bind($data,' + instanceId + ', \'' + name + '\')" class="btn btn-success btn-sm"><i class="kore-icon kore-icon-restart"></i></a>';
                        }
                        s += '</div><div class="btn-group">';

                        if (self.allowAddRemove) {
                            if (!dataItem.IsWatched) {
                                s += '<a data-bind="click: addService.bind($data,' + instanceId + ', \'' + name + '\')" class="btn btn-default btn-sm"><i class="kore-icon kore-icon-add"></i></a>';
                            }
                            else {
                                s += '<a data-bind="click: removeService.bind($data,' + instanceId + ', \'' + name + '\')" class="btn btn-warning btn-sm"><i class="kore-icon kore-icon-remove"></i></a>';
                            }
                        }
                        s += '</div>';
                        return s;
                    },
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 180
                }]
            });

            if (self.onlyShowWatched) {
                var grid = detailRow.find(".services-grid").data("kendoGrid");
                grid.hideColumn("IsWatched");
            }
        };
    };

    var viewModel = new ViewModel();
    return viewModel;
});