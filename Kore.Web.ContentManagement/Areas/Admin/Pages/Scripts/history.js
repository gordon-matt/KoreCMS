'use strict'

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.pageId = ko.observable(emptyGuid);
    self.archivedDate = ko.observable(null);
    self.title = ko.observable('');
    self.slug = ko.observable('');
    self.metaKeywords = ko.observable('');
    self.metaDescription = ko.observable('');
    self.isEnabled = ko.observable(false);
    self.bodyContent = ko.observable('');
    self.cssClass = ko.observable('');
    self.cultureCode = ko.observable('');
    self.refId = ko.observable(null);

    self.view = function (id) {
        $.ajax({
            url: "/odata/kore/cms/HistoricPages(guid'" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.pageId(json.PageId);
            self.archivedDate(json.ArchivedDate);
            self.id(json.Id);
            self.title(json.Title);
            self.slug(json.Slug);
            self.metaKeywords(json.MetaKeywords);
            self.metaDescription(json.MetaDescription);
            self.isEnabled(json.IsEnabled);
            self.bodyContent(json.BodyContent);
            self.cssClass(json.CssClass);
            self.cultureCode(json.CultureCode);
            self.refId(json.RefId);

            switchSection($("#details-section"));
        })
        .fail(function () {
            $.notify(translations.GetRecordError, "error");
        });
    };

    self.restore = function () {
        $.ajax({
            url: "/odata/kore/cms/HistoricPages(guid'" + self.id() + "')/RestoreVersion",
            type: "POST"
        })
        .done(function (json) {
            $('#Grid').data('kendoGrid').dataSource.read();
            $('#Grid').data('kendoGrid').refresh();
            switchSection($("#grid-section"));
            $.notify(translations.PageHistoryRestoreSuccess, "success");
        })
        .fail(function () {
            $.notify(translations.PageHistoryRestoreError, "error");
        });
    };

    self.cancel = function () {
        switchSection($("#grid-section"));
    };;
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
                    url: "/odata/kore/cms/HistoricPages",
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
                        Title: { type: "string" },
                        ArchivedDate: { type: "date" },
                        IsEnabled: { type: "boolean" }
                    }
                }
            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "ArchivedDate", dir: "desc" }
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
            field: "Title",
            title: "Title",
            template: '<a href="/#=Slug#" target="_blank">#=Title#</a>',
            filterable: true
        }, {
            field: "ArchivedDate",
            title: "Archived Date",
            format: "{0:yyyy-MM-dd HH:mm:ss}",
            filterable: true,
            width: 150
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.view(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.View + '</a></div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 70
        }]
    });
});