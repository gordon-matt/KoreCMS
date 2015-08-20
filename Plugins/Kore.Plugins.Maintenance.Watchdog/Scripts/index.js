var viewModel;

define(function (require) {
    'use strict'

    viewModel = null;

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
                    //transport: {
                    //    read: {
                    //        url: function (data) {
                    //            return "/odata/kore/watchdog/ServiceInfoResultApi(" + e.data.Id + ")";
                    //        },
                    //        dataType: "json"
                    //    }
                    //},
                    transport: {
                        read: {
                            url: baseUrl + "/Default.GetServices",

                            // The below works fine, but the problem is Microsoft's current OData implementation doesn't seem to return 'odata.count'
                            //  if the endpoint is an ODataAction. So for now we have no choice but to filter on the client side. Will keep an eye
                            //  on changes in future versions and then uncomment the below code if it ever gets fixed.
                            //url: function (data) {
                            //    // In this case, we need to manually build the query string, since the Kendo Grid doesn't seem to do that when
                            //    //  we are using POST
                            //    var grid = $('#services-grid-' + e.data.Id).data('kendoGrid');

                            //    var params = {
                            //        page: grid.dataSource.page(),
                            //        sort: grid.dataSource.sort(),
                            //        filter: grid.dataSource.filter()
                            //    };

                            //    var queryString = "$inlinecount=allpages&$top=10";

                            //    queryString += "&page=" + (params.page || "~");

                            //    if (params.sort) {
                            //        queryString += "&$orderby=";
                            //        var isFirst = true;
                            //        $.each(params.sort, function () {
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

                            //    return "/odata/kore/watchdog/WatchdogInstanceApi/GetServices?" + queryString;
                            //},
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json"
                        },
                        //parameterMap: function (options, operation) {
                        //    if (operation === "read") {
                        //        return kendo.stringify({
                        //            watchdogInstanceId: e.data.Id
                        //        });
                        //    }
                        //},
                        parameterMap: function (options, operation) {
                            if (operation === "read") {
                                var paramMap = kendo.data.transports.odata.parameterMap(options, operation);

                                paramMap.watchdogInstanceId = e.data.Id;

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
                            return data.value.length;
                            //return data["@odata.count"];
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
                    // Don't change this to self.gridPageSize. There's a bug with client-side paging. If we set self.gridPageSize here, then paging gets messed up.
                    //  For more info, see: http://stackoverflow.com/questions/31810484/kendo-grid-misbehaving-in-certain-situations-with-durandal-requirejs?noredirect=1#comment52004513_31810484
                    pageSize: 15,
                    //pageSize: self.gridPageSize,
                    serverPaging: false,
                    serverFiltering: false,
                    serverSorting: false,
                    sort: { field: "DisplayName", dir: "asc" },
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
                    template:
                        '<div class="btn-group">' +
                        '# if(Status == "Stopped") {#' +
                        '<a onclick="viewModel.startService(\'#=WatchdogInstanceId#\', \'#=ServiceName#\')" class="btn btn-success btn-sm"><i class="kore-icon kore-icon-start"></i></a>' +
                        '#} else if (Status == "Running") {#' +
                        '<a onclick="viewModel.stopService(\'#=WatchdogInstanceId#\', \'#=ServiceName#\')" class="btn btn-danger btn-sm"><i class="kore-icon kore-icon-stop"></i></a>' +
                        '<a onclick="viewModel.restartService(\'#=WatchdogInstanceId#\', \'#=ServiceName#\')" class="btn btn-success btn-sm"><i class="kore-icon kore-icon-restart"></i></a>' +
                        "#} #</div>&nbsp;" +
                        '# if (viewModel.allowAddRemove) {#' +
                        '# if (!IsWatched) {#' +
                        '<div class="btn-group"><a onclick="viewModel.addService(\'#=WatchdogInstanceId#\', \'#=ServiceName#\')" class="btn btn-default btn-sm"><i class="kore-icon kore-icon-add"></i></a>' +
                        '#} else {#' +
                        '<a onclick="viewModel.removeService(\'#=WatchdogInstanceId#\', \'#=ServiceName#\')" class="btn btn-warning btn-sm"><i class="kore-icon kore-icon-remove"></i></a>' +
                        '#} #' +
                        '#} #' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 180
                }]
            });

            if (viewModel.onlyShowWatched) {
                var grid = detailRow.find(".services-grid").data("kendoGrid");
                grid.hideColumn("IsWatched");
            }
        };
    };

    viewModel = new ViewModel();
    return viewModel;
});