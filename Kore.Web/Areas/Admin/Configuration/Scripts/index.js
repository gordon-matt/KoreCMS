//'use strict'

//var ViewModel = function () {
//    var self = this;

//    self.id = ko.observable(emptyGuid);
//    self.name = ko.observable("");
//    self.type = ko.observable("");
//    self.value = ko.observable(null);

//    self.edit = function (id) {
//        $.ajax({
//            url: "/odata/kore/web/Settings(guid'" + id + "')",
//            type: "GET",
//            dataType: "json",
//            async: false
//        })
//        .done(function (json) {
//            self.id(json.Id);
//            self.name(json.Name);
//            self.type(json.Type);
//            self.value(json.Value);

//            $.ajax({
//                url: "/admin/configuration/get-editor-ui/" + replaceAll(self.type(), ".", "-"),
//                type: "GET",
//                dataType: "json",
//                async: false
//            })
//            .done(function (json) {

//                // Clean up from previously injected html/scripts
//                if (typeof cleanUp == 'function') {
//                    cleanUp();
//                }

//                // Remove Old Scripts
//                var oldScripts = $('script[data-settings-script="true"]');

//                if (oldScripts.length > 0) {
//                    $.each(oldScripts, function () {
//                        $(this).remove();
//                    });
//                }

//                var elementToBind = $("#form-section")[0];
//                ko.cleanNode(elementToBind);

//                var result = $(json.Content);

//                // Add new HTML
//                var content = $(result.filter('#settings-content')[0]);
//                var details = $('<div>').append(content.clone()).html();
//                $("#settings-details").html(details);

//                // Add new Scripts
//                var scripts = result.filter('script');

//                $.each(scripts, function () {
//                    var script = $(this);
//                    script.attr("data-settings-script", "true");//for some reason, .data("widget-script", "true") doesn't work here
//                    script.appendTo('body');
//                });

//                // Update Bindings
//                // Ensure the function exists before calling it...
//                if (typeof updateModel == 'function') {
//                    var data = ko.toJS(ko.mapping.fromJSON(self.value()));
//                    updateModel(data);
//                    ko.applyBindings(viewModel, elementToBind);
//                }

//                //self.validator.resetForm();
//                switchSection($("#form-section"));
//            })
//            .fail(function () {
//                $.notify(translations.GetRecordError, "error");
//            });
//        })
//        .fail(function () {
//            $.notify(translations.GetRecordError, "error");
//        });
//    };

//    self.save = function () {
//        // ensure the function exists before calling it...
//        if (typeof onBeforeSave == 'function') {
//            onBeforeSave();
//        }

//        var record = {
//            Id: self.id(),
//            Name: self.name(),
//            Type: self.type(),
//            Value: self.value()
//        };

//        $.ajax({
//            url: "/odata/kore/web/Settings(guid'" + self.id() + "')",
//            type: "PUT",
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify(record),
//            dataType: "json",
//            async: false
//        })
//        .done(function (json) {
//            switchSection($("#grid-section"));

//            $.notify(translations.UpdateRecordSuccess, "success");
//        })
//        .fail(function () {
//            $.notify(translations.UpdateRecordError, "error");
//        });
//    };

//    self.cancel = function () {
//        switchSection($("#grid-section"));
//    };
//};

//var viewModel;
//$(document).ready(function () {
//    viewModel = new ViewModel();
//    ko.applyBindings(viewModel);

//    $("#Grid").kendoGrid({
//        data: null,
//        dataSource: {
//            type: "odata",
//            transport: {
//                read: {
//                    url: "/odata/kore/web/Settings",
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
//                        Name: { type: "string" }
//                    }
//                }
//            },
//            pageSize: 10,
//            serverPaging: true,
//            serverFiltering: true,
//            serverSorting: true,
//            sort: { field: "Name", dir: "asc" }
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
//            field: "Name",
//            title: "Name",
//            filterable: true
//        }, {
//            field: "Id",
//            title: " ",
//            template: '<div class="btn-group"><a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a></div>',
//            attributes: { "class": "text-center" },
//            filterable: false,
//            width: 120
//        }]
//    });
//});