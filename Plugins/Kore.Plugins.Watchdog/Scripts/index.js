'use strict'

var currentSection = $("#grid-section");

function switchSection(section) {
    currentSection.hide("fast");
    section.show("fast");
    currentSection = section;
};

$(document).ready(function () {
    jQuery.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });
});

var baseUrl = "/odata/kore/watchdog/WatchdogInstances";

var ViewModel = function () {
    var self = this;
    self.url = ko.observable('');
    self.password = ko.observable('');

    self.create = function () {
        self.url('');
        self.password('');

        self.validator.resetForm();
        switchSection($("#form-section"));
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: baseUrl + "(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
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

            $.notify(translations.InsertRecordSuccess, "success");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.InsertRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.cancel = function () {
        switchSection($("#grid-section"));
    };

    self.stopService = function (instanceId, name) {
        if (confirm(translations.ConfirmStopService)) {
            self.executeAction(baseUrl + "/StopService", instanceId, name, translations.StopServiceError, translations.StopServiceSuccess, false);
        }
    };

    self.startService = function (instanceId, name) {
        self.executeAction(baseUrl + "/StartService", instanceId, name, translations.StartServiceError, translations.StartServiceSuccess, false);
    };

    self.restartService = function (instanceId, name) {
        self.executeAction(baseUrl + "/RestartService", instanceId, name, translations.RestartServiceError, translations.RestartServiceSuccess, false);
    };

    self.addService = function (instanceId, name) {
        self.executeAction(baseUrl + "/AddService", instanceId, name, translations.AddServiceError, translations.AddServiceSuccess, true);
    };

    self.removeService = function (instanceId, name) {
        if (confirm(translations.ConfirmRemoveService)) {
            self.executeAction(baseUrl + "/RemoveService", instanceId, name, translations.RemoveServiceError, translations.RemoveServiceSuccess, true);
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
    }

    self.validator = $("#form-section-form").validate({
        rules: {
            Url: { required: true, maxlength: 255 },
            Password: { required: true, maxlength: 255 }
        }
    });
};
var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: baseUrl,
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
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Url: { type: "string" }
                    }
                }
            },
            pageSize: 10,
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
            title: "Url",
            filterable: true
        }],
        detailTemplate: kendo.template($("#services-template").html()),
        detailInit: detailInit,
        dataBound: function () {
            this.expandRow(this.tbody.find("tr.k-master-row").first());
        },
    });

    function detailInit(e) {
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
                        url: baseUrl + "/GetServices",

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

                        //    return "/odata/kore/watchdog/WatchdogInstances/GetServices?" + queryString;
                        //},
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json"
                    },
                    parameterMap: function (options, operation) {
                        if (operation === "read") {
                            return kendo.stringify({
                                watchdogInstanceId: e.data.Id
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
                        //return data["odata.count"];
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
                pageSize: 10,
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
                title: "Display Name",
                filterable: true
            }, {
                field: "ServiceName",
                title: "Service Name",
                filterable: true
            }, {
                field: "Status",
                title: "Status",
                filterable: true,
                width: 100
            }, {
                field: "IsWatched",
                title: "&nbsp;",
                template: '# if(IsWatched) {#<img src="/Plugins/Plugins.Watchdog/Content/img/watchdog_32.png" alt="Watched" /> #} #',
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
                    '# if (allowAddRemove) {#' +
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

        if (onlyShowWatched) {
            detailGrid.hideColumn("IsWatched");
        }
    };
});