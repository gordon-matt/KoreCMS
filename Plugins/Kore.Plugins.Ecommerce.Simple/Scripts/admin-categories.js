var viewModel;

define(function (require) {
    'use strict'

    viewModel = null;

    var $ = require('jquery');
    var ko = require('knockout');
    //var koMap = require('knockout-mapping');

    require('jqueryval');
    require('kendo');
    require('notify');
    require('tinymce');
    require('tinymce-jquery');
    require('tinymce-knockout');

    require('kore-common');
    require('kore-section-switching');
    require('kore-jqueryval');
    require('kore-tinymce');

    //ko.mapping = koMap;

    var categoryApiUrl = "/odata/kore/plugins/simple-commerce/SimpleCommerceCategoryApi";
    var categoryTreeApiUrl = "/odata/kore/plugins/simple-commerce/SimpleCommerceCategoryTreeApi";
    var productApiUrl = "/odata/kore/plugins/simple-commerce/SimpleCommerceProductApi";

    var ProductImageModel = function () {
        var self = this;

        self.id = ko.observable(0);
        self.productId = ko.observable(0);
        self.url = ko.observable(null);
        self.thumbnailUrl = ko.observable(null);
        self.caption = ko.observable(null);
        self.order = ko.observable(0);
    };

    var ProductModel = function () {
        var self = this;

        self.id = ko.observable(0);
        self.name = ko.observable(null);
        self.slug = ko.observable(null);
        self.categoryId = ko.observable(0);
        self.price = ko.observable(0);
        self.tax = ko.observable(0);
        self.shippingCost = ko.observable(0);
        self.mainImageUrl = ko.observable(null);
        self.shortDescription = ko.observable(null);
        self.fullDescription = ko.observable(null);
        self.metaKeywords = ko.observable(null);
        self.metaDescription = ko.observable(null);
        self.useExternalLink = ko.observable(false);
        self.externalLink = ko.observable(null);

        self.images = ko.observableArray([]);

        self.tinyMCE_fullDescription = koreDefaultTinyMCEConfig;

        self.showToolbar = ko.observable(false);

        self.validator = false;

        self.init = function () {
            self.validator = $("#product-form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 },
                    Slug: { required: true, maxlength: 255 },
                    CategoryId: { required: true },
                    Price: { required: true, number: true },
                    Tax: { required: true, number: true },
                    ShippingCost: { required: true, number: true },
                    MainImageUrl: { maxlength: 255 },
                    ShortDescription: { required: true },
                    FullDescription: { required: true },
                    MetaKeywords: { maxlength: 255 },
                    MetaDescription: { maxlength: 255 },
                    ExternalLink: { maxlength: 255 }
                }
            });

            $("#ProductGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: function () {
                                return productApiUrl + "?$filter=CategoryId eq " + viewModel.category.id()
                            },
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
                                Name: { type: "string" },
                                Price: { type: "number" }
                            }
                        }
                    },
                    pageSize: viewModel.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Name", dir: "asc" }
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
                    field: "Name",
                    title: viewModel.translations.Columns.Product.Name,
                    filterable: true
                }, {
                    field: "Price",
                    title: viewModel.translations.Columns.Product.Price,
                    filterable: true,
                    width: 200
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.product.edit(#=Id#)" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.product.remove(#=Id#)" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 150
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.name(null);
            self.slug(null);
            //self.categoryId(0);
            self.price(0);
            self.tax(0);
            self.shippingCost(0);
            self.mainImageUrl(null);
            self.shortDescription(null);
            self.fullDescription('');
            self.metaKeywords(null);
            self.metaDescription(null);
            self.useExternalLink(false);
            self.externalLink(null);

            self.validator.resetForm();
            switchSection($("#product-form-section"));
            self.showToolbar(false);
            $("#product-form-section-legend").html(viewModel.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: productApiUrl + "(" + id + ")",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.name(json.Name);
                self.slug(json.Slug);
                self.categoryId(json.CategoryId);
                self.price(json.Price);
                self.tax(json.Tax);
                self.shippingCost(json.ShippingCost);
                self.mainImageUrl(json.MainImageUrl);
                self.shortDescription(json.ShortDescription);
                self.fullDescription(json.FullDescription);
                self.metaKeywords(json.MetaKeywords);
                self.metaDescription(json.MetaDescription);
                self.useExternalLink(json.UseExternalLink);
                self.externalLink(json.ExternalLink);

                self.validator.resetForm();
                switchSection($("#product-form-section"));
                self.showToolbar(true);
                $("#product-form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: productApiUrl + "(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#ProductGrid').data('kendoGrid').dataSource.read();
                    $('#ProductGrid').data('kendoGrid').refresh();

                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            if (!$("#product-form-section-form").valid()) {
                return false;
            }

            var record = {
                Id: self.id(),
                Name: self.name(),
                Slug: self.slug(),
                CategoryId: self.categoryId(),
                Price: self.price(),
                Tax: self.tax(),
                ShippingCost: self.shippingCost(),
                MainImageUrl: self.mainImageUrl(),
                ShortDescription: self.shortDescription(),
                FullDescription: self.fullDescription(),
                MetaKeywords: self.metaKeywords(),
                MetaDescription: self.metaDescription(),
                UseExternalLink: self.useExternalLink(),
                ExternalLink: self.externalLink()
            };

            if (self.id() == 0) {
                // INSERT
                $.ajax({
                    url: productApiUrl,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#ProductGrid').data('kendoGrid').dataSource.read();
                    $('#ProductGrid').data('kendoGrid').refresh();

                    switchSection($("#product-grid-section"));

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                // UPDATE
                $.ajax({
                    url: productApiUrl + "(" + self.id() + ")",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#ProductGrid').data('kendoGrid').dataSource.read();
                    $('#ProductGrid').data('kendoGrid').refresh();

                    switchSection($("#product-grid-section"));

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#product-grid-section"));
        };
    };

    var CategoryModel = function () {
        var self = this;

        self.id = ko.observable(0);
        self.parentId = ko.observable(null);
        self.name = ko.observable(null);
        self.slug = ko.observable(null);
        self.order = ko.observable(0);
        self.imageUrl = ko.observable(null);
        self.description = ko.observable(null);
        self.metaKeywords = ko.observable(null);
        self.metaDescription = ko.observable(null);

        self.showToolbar = ko.observable(false);

        self.validator = false;

        self.init = function () {
            self.validator = $("#form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 },
                    Slug: { required: true, maxlength: 255 },
                    Order: { required: true, number: true },
                    ImageUrl: { maxlength: 255 },
                    Description: { maxlength: 255 },
                    MetaKeywords: { maxlength: 255 },
                    MetaDescription: { maxlength: 255 }
                }
            });

            var treeviewDS = new kendo.data.HierarchicalDataSource({
                type: "odata",
                transport: {
                    read: {
                        url: categoryTreeApiUrl + "?$expand=SubCategories($levels=max)",
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

                    if (viewModel.category.id() == destinationId) {
                        destinationCategory = {
                            Id: viewModel.category.id(),
                            ParentId: viewModel.category.parentId()
                        };
                    }
                    else {
                        $.ajax({
                            url: categoryApiUrl + "(" + destinationId + ")",
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
                            $.notify(viewModel.translations.GetRecordError, "error");
                            console.log(textStatus + ': ' + errorThrown);
                            return;
                        });
                    }

                    if (destinationCategory.ParentId == sourceId) {
                        $.notify(viewModel.translations.CircularRelationshipError, "error");
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
                        url: categoryApiUrl + "(" + sourceId + ")",
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
                        $.notify(viewModel.translations.UpdateRecordError, "error");
                        console.log(textStatus + ': ' + errorThrown);
                    });
                }
            });
        };
        self.create = function () {
            self.id(0);
            self.parentId(null);
            self.name(null);
            self.slug(null);
            self.order(0);
            self.imageUrl(null);
            self.description(null);
            self.metaKeywords(null);
            self.metaDescription(null);

            self.showToolbar(false);

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(viewModel.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: categoryApiUrl + "(" + id + ")",
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
                self.imageUrl(json.ImageUrl);
                self.description(json.Description);
                self.metaKeywords(json.MetaKeywords);
                self.metaDescription(json.MetaDescription);

                self.showToolbar(true);

                self.validator.resetForm();
                switchSection($("#form-section"));
                $("#form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function () {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: categoryApiUrl + "(" + self.id() + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    self.refresh();
                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
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
                Order: self.order(),
                ImageUrl: self.imageUrl(),
                Description: self.description(),
                MetaKeywords: self.metaKeywords(),
                MetaDescription: self.metaDescription()
            };

            if (self.id() == 0) {
                // INSERT
                $.ajax({
                    url: categoryApiUrl,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    self.refresh();
                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                // UPDATE
                $.ajax({
                    url: categoryApiUrl + "(" + self.id() + ")",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    self.refresh();
                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
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
        };
        self.showProducts = function () {
            viewModel.product.categoryId(self.id());
            self.showToolbar(false);
            $('#ProductGrid').data('kendoGrid').dataSource.read();
            $('#ProductGrid').data('kendoGrid').refresh();
            switchSection($("#product-grid-section"));
        };
    };

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.category = false;
        self.product = false;

        self.modalDismissed = false;

        self.activate = function () {
            self.category = new CategoryModel();
            self.product = new ProductModel();
        };
        self.attached = function () {
            currentSection = $("#main-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/plugins/ecommerce/simple/categories/get-translations",
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

            self.category.init();
            self.product.init();

            $('#categoryImageModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#ImageUrl').val();
                    url = "/Media/Uploads/" + url;
                    viewModel.category.imageUrl(url);
                }
                self.modalDismissed = false;
            });

            $('#productImageModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#MainImageUrl').val();
                    url = "/Media/Uploads/" + url;
                    viewModel.product.mainImageUrl(url);
                }
                self.modalDismissed = false;
            });
        };

        self.dismissModal = function(modalId) {
            modalDismissed = true;
            $('#' + modalId).modal('hide');
        };
    };

    viewModel = new ViewModel();
    return viewModel;
});