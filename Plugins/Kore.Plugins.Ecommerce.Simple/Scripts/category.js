'use strict'

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.parentId = ko.observable(null);
    self.name = ko.observable('');
    self.slug = ko.observable('');
    self.order = ko.observable(0);
    self.showToolbar = ko.observable(false);

    self.create = function () {
        self.id(0);
        self.parentId(null);
        self.name('');
        self.slug('');
        self.order(0);
        self.showToolbar(false);

        self.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/plugins/simple-commerce/CategoryApi(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.parentId(json.ParentId);
            self.name(json.Name);
            self.slug(json.Slug);
            self.order(json.Order);

            self.showToolbar(true);

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/plugins/simple-commerce/CategoryApi(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                self.refresh();
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
            Id: self.id(),
            ParentId: self.parentId(),
            Name: self.name(),
            Slug: self.slug(),
            Order: self.order()
        };

        if (self.id() == 0) {
            // INSERT
            $.ajax({
                url: "/odata/kore/plugins/simple-commerce/CategoryApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.refresh();
                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            // UPDATE
            $.ajax({
                url: "/odata/kore/plugins/simple-commerce/CategoryApi(" + self.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.refresh();
                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#blank-section"));
        self.showToolbar(false);
    };

    self.refresh = function () {
        switchSection($("#blank-section"));
        self.showToolbar(false);
        $("#treeview").data("kendoTreeView").dataSource.read();
    }

    self.validator = $("#form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 },
            Slug: { required: true, maxlength: 255 },
            Order: { required: true, number: true }
        }
    });
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    //$("#Grid").kendoGrid({
    //    data: null,
    //    dataSource: {
    //        type: "odata",
    //        transport: {
    //            read: {
    //                url: "/odata/kore/plugins/simple-commerce/CategoryApi",
    //                dataType: "json"
    //            }
    //        },
    //        schema: {
    //            data: function (data) {
    //                return data.value;
    //            },
    //            total: function (data) {
    //                return data["odata.count"];
    //            },
    //            model: {
    //                fields: {
    //                    Name: { type: "string" }
    //                }
    //            }
    //        },
    //        pageSize: gridPageSize,
    //        serverPaging: true,
    //        serverFiltering: true,
    //        serverSorting: true,
    //        sort: { field: "Name", dir: "asc" }
    //    },
    //    filterable: true,
    //    sortable: {
    //        allowUnsort: false
    //    },
    //    pageable: {
    //        refresh: true
    //    },
    //    scrollable: false,
    //    columns: [{
    //        field: "Name",
    //        title: "Name",
    //        filterable: true
    //    }, {
    //        field: "Id",
    //        title: " ",
    //        template:
    //            '<div class="btn-group"><a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
    //            '<a onclick="viewModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
    //            '</div>',
    //        attributes: { "class": "text-center" },
    //        filterable: false,
    //        width: 150
    //    }]
    //});




    var treeviewDS = new kendo.data.HierarchicalDataSource({
        type: "odata",
        transport: {
            read: {
                url: "/odata/kore/plugins/simple-commerce/CategoryTreeApi?$expand=SubCategories/SubCategories",
                dataType: "json"
            }
        },
        schema: {
            data: function (response) {
                return response.value;
            },
            total: function (response) {
                return response.value.length;
            },
            model: {
                id: "Id",
                children: "SubCategories"
            }
        }
    });

    $("#treeview").kendoTreeView({
        template: kendo.template($("#treeview-template").html()),
        dragAndDrop: true,
        dataSource: treeviewDS,
        dataTextField: ["Name"],
        loadOnDemand: false,
        dataBound: function (e) {
            setTimeout(function () {
                $("#treeview").data("kendoTreeView").expand(".k-item");
            }, 20);
        },
        drop: function (e) {
            var sourceDataItem = this.dataItem(e.sourceNode);
            var sourceId = sourceDataItem.id;
            var destinationDataItem = this.dataItem(e.destinationNode);
            var destinationId = destinationDataItem.id;
            var dropPosition = e.dropPosition;

            if (destinationId == sourceId) {
                // A category cannot be a parent of itself!
                return;
            }

            var parentId = null;
            var destinationCategory = null;

            if (viewModel.id() == destinationId) {
                destinationCategory = {
                    Id: viewModel.id(),
                    ParentId: viewModel.parentId()
                };
            }
            else {
                $.ajax({
                    url: "/odata/kore/plugins/simple-commerce/CategoryApi(" + destinationId + ")",
                    type: "GET",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    destinationCategory = {
                        Id: json.Id,
                        ParentId: json.ParentId
                    };
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(translations.GetRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                    return;
                });
            }

            if (destinationCategory.ParentId == sourceId) {
                $.notify(translations.CircularRelationshipError, "error");
                $("#treeview").data("kendoTreeView").dataSource.read();
                return;
            }

            switch (dropPosition) {
                case 'over':
                    parentId = destinationId;
                    break;
                default:
                    parentId = destinationCategory.ParentId;
                    break;
            }

            var patch = {
                ParentId: parentId
            };

            $.ajax({
                url: "/odata/kore/plugins/simple-commerce/CategoryApi(" + sourceId + ")",
                type: "PATCH",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(patch),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $("#treeview").data("kendoTreeView").dataSource.read();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    });
});