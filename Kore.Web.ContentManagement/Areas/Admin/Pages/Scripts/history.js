//'use strict'

//var ViewModel = function () {
//    var self = this;

//    self.id = ko.observable(emptyGuid);
//    self.pageId = ko.observable(emptyGuid);
//    self.parentId = ko.observable(null);
//    self.pageTypeId = ko.observable(emptyGuid);
//    self.archivedDate = ko.observable(null);
//    self.name = ko.observable(null);
//    self.slug = ko.observable(null);
//    self.fields = ko.observable(null);
//    self.isEnabled = ko.observable(false);
//    self.order = ko.observable(0);
//    self.showOnMenus = ko.observable(true);
//    self.dateCreatedUtc = ko.observable();
//    self.dateModifiedUtc = ko.observable();
//    self.cultureCode = ko.observable(null);
//    self.refId = ko.observable(null);

//    self.view = function (id) {
//        $.ajax({
//            url: "/odata/kore/cms/HistoricPageApi(guid'" + id + "')",
//            type: "GET",
//            dataType: "json",
//            async: false
//        })
//        .done(function (json) {
//            self.id(json.Id);
//            self.pageId(json.PageId);
//            self.parentId(json.ParentId);
//            self.pageTypeId(json.PageTypeId);
//            self.archivedDate(json.ArchivedDate);
//            self.name(json.Name);
//            self.slug(json.Slug);
//            self.fields(json.Fields);
//            self.isEnabled(json.IsEnabled);
//            self.order(json.Order);
//            self.showOnMenus(json.ShowOnMenus);
//            self.dateCreatedUtc(json.DateCreatedUtc);
//            self.dateModifiedUtc(json.DateModifiedUtc);
//            self.cultureCode(json.CultureCode);
//            self.refId(json.RefId);

//            $.get("/admin/pages/preview/" + id + '/true', function (data) {
//                $("#page-preview").contents().find('html').html(data);
//            });

//            switchSection($("#details-section"));
//        })
//        .fail(function (jqXHR, textStatus, errorThrown) {
//            $.notify(translations.GetRecordError, "error");
//            console.log(textStatus + ': ' + errorThrown);
//        });
//    };

//    self.restore = function () {
//        if (confirm(translations.PageHistoryRestoreConfirm)) {
//            $.ajax({
//                url: "/odata/kore/cms/HistoricPageApi(guid'" + self.id() + "')/RestoreVersion",
//                type: "POST"
//            })
//            .done(function (json) {
//                $('#Grid').data('kendoGrid').dataSource.read();
//                $('#Grid').data('kendoGrid').refresh();
//                switchSection($("#grid-section"));
//                $.notify(translations.PageHistoryRestoreSuccess, "success");
//            })
//            .fail(function (jqXHR, textStatus, errorThrown) {
//                $.notify(translations.PageHistoryRestoreError, "error");
//                console.log(textStatus + ': ' + errorThrown);
//            });
//        };
//    };

//    self.cancel = function () {
//        $('#page-preview').contents().find('html').html('');
//        switchSection($("#grid-section"));
//    };;
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
//                    url: "/odata/kore/cms/HistoricPageApi?$filter=PageId eq guid'" + pageId + "'",
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
//                        Title: { type: "string" },
//                        ArchivedDate: { type: "date" },
//                        IsEnabled: { type: "boolean" }
//                    }
//                }
//            },
//            pageSize: gridPageSize,
//            serverPaging: true,
//            serverFiltering: true,
//            serverSorting: true,
//            sort: { field: "ArchivedDate", dir: "desc" }
//        },
//        filterable: false,
//        sortable: {
//            allowUnsort: false
//        },
//        pageable: {
//            refresh: true
//        },
//        scrollable: false,
//        columns: [{
//            field: "Name",
//            title: translations.Columns.Name,
//            template: '<a href="/#=Slug#" target="_blank">#=Name#</a>',
//        }, {
//            field: "ArchivedDate",
//            title: translations.Columns.ArchivedDate,
//            format: "{0:yyyy-MM-dd HH:mm:ss}",
//            width: 150
//        }, {
//            field: "Id",
//            title: " ",
//            template:
//                '<div class="btn-group"><a onclick="viewModel.view(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.View + '</a></div>',
//            attributes: { "class": "text-center" },
//            width: 70
//        }]
//    });
//});