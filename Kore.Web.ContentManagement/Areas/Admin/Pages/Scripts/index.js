'use strict'

var PageTypeVM = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.name = ko.observable('');
    self.layoutPath = ko.observable('');
    self.displayTemplatePath = ko.observable('');
    self.editorTemplatePath = ko.observable('');

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/PageTypeApi(guid'" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.layoutPath(json.LayoutPath);
            self.displayTemplatePath(json.DisplayTemplatePath);
            self.editorTemplatePath(json.EditorTemplatePath);

            self.validator.resetForm();
            switchSection($("#page-type-form-section"));
            $("#page-type-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/cms/PageTypeApi(guid'" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#PageTypesGrid').data('kendoGrid').dataSource.read();
                $('#PageTypesGrid').data('kendoGrid').refresh();

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.save = function () {
        var isNew = (self.id() == emptyGuid);

        var record = {
            Id: self.id(),
            Name: self.name(),
            LayoutPath: self.layoutPath(),
            DisplayTemplatePath: self.displayTemplatePath(),
            EditorTemplatePath: self.editorTemplatePath()
        };

        if (isNew) {
            $.ajax({
                url: "/odata/kore/cms/PageTypeApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#PageTypesGrid').data('kendoGrid').dataSource.read();
                $('#PageTypesGrid').data('kendoGrid').refresh();

                switchSection($("#page-type-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: "/odata/kore/cms/PageTypeApi(guid'" + self.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#PageTypesGrid').data('kendoGrid').dataSource.read();
                $('#PageTypesGrid').data('kendoGrid').refresh();

                switchSection($("#page-type-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#page-type-grid-section"));
    };

    //self.showPages = function () {
    //    switchSection($("#blank-section"));
    //};

    self.validator = $("#page-type-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 },
            LayoutPath: { required: true, maxlength: 255 },
            DisplayTemplatePath: { maxlength: 255 },
            EditorTemplatePath: { maxlength: 255 }
        }
    });
};

var ViewModel = function () {
    var self = this;
    
    self.id = ko.observable(emptyGuid);
    self.parentId = ko.observable(null);
    self.pageTypeId = ko.observable(emptyGuid);
    self.name = ko.observable('');
    self.slug = ko.observable('');
    self.fields = ko.observable('');
    self.isEnabled = ko.observable(false);
    self.order = ko.observable(0);
    self.showOnMenus = ko.observable(true);
    self.accessRestrictions = null; // doesn't need to be an observable
    self.cultureCode = ko.observable('');
    self.refId = ko.observable(null);
    self.showToolbar = ko.observable(false);

    self.roles = ko.observableArray([]);

    self.pageType = new PageTypeVM();

    self.pageModelStub = null;

    self.create = function () {
        self.id(emptyGuid);
        self.parentId(null);
        self.pageTypeId(emptyGuid);
        self.name('');
        self.slug('');
        self.fields('');
        self.isEnabled(false);
        self.order(0);
        self.showOnMenus(true);
        self.accessRestrictions = null;
        self.cultureCode('');
        self.refId(null);

        self.roles([]);

        self.showToolbar(false);

        // Clean up from previously injected html/scripts
        if (self.pageModelStub != null && typeof self.pageModelStub.cleanUp === 'function') {
            self.pageModelStub.cleanUp();
        }
        self.pageModelStub = null;

        // Remove Old Scripts
        var oldScripts = $('script[data-fields-script="true"]');

        if (oldScripts.length > 0) {
            $.each(oldScripts, function () {
                $(this).remove();
            });
        }

        var elementToBind = $("#fields-definition")[0];
        ko.cleanNode(elementToBind);
        $("#fields-definition").html("");

        self.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/PageApi(guid'" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.parentId(json.ParentId);
            self.pageTypeId(json.PageTypeId);
            self.name(json.Name);
            self.slug(json.Slug);
            self.fields(json.Fields);
            self.isEnabled(json.IsEnabled);
            self.order(json.Order);
            self.showOnMenus(json.ShowOnMenus);
            self.accessRestrictions = ko.mapping.fromJSON(json.AccessRestrictions);
            self.cultureCode(json.CultureCode);
            self.refId(json.RefId);

            if (self.accessRestrictions.Roles != null) {
                var split = self.accessRestrictions.Roles().split(',');
                self.roles(split);
            }
            else {
                self.roles([]);
            }

            self.showToolbar(true);

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(translations.Edit);

            $.ajax({
                url: "/admin/pages/get-editor-ui/" + self.id(),
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                // Clean up from previously injected html/scripts
                if (self.pageModelStub != null && typeof self.pageModelStub.cleanUp === 'function') {
                    self.pageModelStub.cleanUp();
                }
                self.pageModelStub = null;

                // Remove Old Scripts
                var oldScripts = $('script[data-fields-script="true"]');

                if (oldScripts.length > 0) {
                    $.each(oldScripts, function () {
                        $(this).remove();
                    });
                }

                var elementToBind = $("#fields-definition")[0];
                ko.cleanNode(elementToBind);

                var result = $(json.Content);

                // Add new HTML
                var content = $(result.filter('#fields-content')[0]);
                var details = $('<div>').append(content.clone()).html();
                $("#fields-definition").html(details);

                // Add new Scripts
                var scripts = result.filter('script');

                $.each(scripts, function () {
                    var script = $(this);
                    script.attr("data-fields-script", "true");//for some reason, .data("fields-script", "true") doesn't work here
                    script.appendTo('body');
                });

                // Update Bindings
                // Ensure the function exists before calling it...
                if (typeof pageModel != null) {
                    self.pageModelStub = pageModel;
                    if (typeof self.pageModelStub.updateModel === 'function') {
                        self.pageModelStub.updateModel();
                    }
                    ko.applyBindings(viewModel, elementToBind);
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function () {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/cms/PageApi(guid'" + self.id() + "')",
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

        // ensure the function exists before calling it...
        if (self.pageModelStub != null && typeof self.pageModelStub.onBeforeSave === 'function') {
            self.pageModelStub.onBeforeSave();
        }

        var cultureCode = self.cultureCode();
        if (cultureCode == '') {
            cultureCode = null;
        }

        var record = {
            Id: self.id(),
            ParentId: self.parentId(),
            PageTypeId: self.pageTypeId(),
            Name: self.name(),
            Slug: self.slug(),
            Fields: self.fields(),
            IsEnabled: self.isEnabled(),
            Order: self.order(),
            ShowOnMenus: self.showOnMenus(),
            AccessRestrictions: JSON.stringify({
                Roles: self.roles().join()
            }),
            CultureCode: cultureCode,
            RefId: self.refId()
        };

        if (self.id() == emptyGuid) {
            // INSERT
            $.ajax({
                url: "/odata/kore/cms/PageApi",
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
                url: "/odata/kore/cms/PageApi(guid'" + self.id() + "')",
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
        // Clean up from previously injected html/scripts
        if (self.pageModelStub != null && typeof self.pageModelStub.cleanUp === 'function') {
            self.pageModelStub.cleanUp();
        }
        self.pageModelStub = null;

        // Remove Old Scripts
        var oldScripts = $('script[data-fields-script="true"]');

        if (oldScripts.length > 0) {
            $.each(oldScripts, function () {
                $(this).remove();
            });
        }

        var elementToBind = $("#fields-definition")[0];
        ko.cleanNode(elementToBind);
        $("#fields-definition").html("");

        switchSection($("#blank-section"));
        self.showToolbar(false);
    };

    self.toggleEnabled = function () {
        var patch = {
            IsEnabled: !self.isEnabled()
        };

        $.ajax({
            url: "/odata/kore/cms/PageApi(guid'" + self.id() + "')",
            type: "PATCH",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(patch),
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
    };

    self.translate = function (id) {
        $("#CultureSelector_PageId").val(id); //TODO: make this a self variable
        self.showToolbar(false);
        switchSection($("#culture-selector-section"));
    };

    self.cultureSelector_onCancel = function () {
        // Clean up from previously injected html/scripts
        if (self.pageModelStub != null && typeof self.pageModelStub.cleanUp === 'function') {
            self.pageModelStub.cleanUp();
        }
        self.pageModelStub = null;

        // Remove Old Scripts
        var oldScripts = $('script[data-fields-script="true"]');

        if (oldScripts.length > 0) {
            $.each(oldScripts, function () {
                $(this).remove();
            });
        }

        var elementToBind = $("#fields-definition")[0];
        ko.cleanNode(elementToBind);
        $("#fields-definition").html("");

        switchSection($("#blank-section"));
        self.showToolbar(false);
    };

    self.cultureSelector_onSelected = function () {
        var pageId = $("#CultureSelector_PageId").val();

        var data = {
            pageId: pageId,
            cultureCode: $("#CultureSelector_CultureCode").val()
        };

        $.ajax({
            url: "/odata/kore/cms/PageApi/Translate",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.parentId(json.ParentId);
            self.pageTypeId(json.PageTypeId);
            self.name(json.Name);
            self.slug(json.Slug);
            self.fields(json.Fields);
            self.isEnabled(json.IsEnabled);
            self.order(json.Order);
            self.showOnMenus(json.ShowOnMenus);
            self.accessRestrictions = ko.mapping.fromJSON(json.AccessRestrictions);
            self.cultureCode(json.CultureCode);
            self.refId(json.RefId);

            if (self.accessRestrictions.Roles === "function") {
                self.roles(self.accessRestrictions.Roles().split(','));
            }
            else {
                self.roles([]);
            }

            self.showToolbar(false);
            
            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(translations.Edit);

            if (self.id() != emptyGuid) {
                $.ajax({
                    url: "/admin/pages/get-editor-ui/" + self.id(),
                    type: "GET",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    // Clean up from previously injected html/scripts
                    if (self.pageModelStub != null && typeof self.pageModelStub.cleanUp === 'function') {
                        self.pageModelStub.cleanUp();
                    }
                    self.pageModelStub = null;

                    // Remove Old Scripts
                    var oldScripts = $('script[data-fields-script="true"]');

                    if (oldScripts.length > 0) {
                        $.each(oldScripts, function () {
                            $(this).remove();
                        });
                    }

                    var elementToBind = $("#fields-definition")[0];
                    ko.cleanNode(elementToBind);

                    var result = $(json.Content);

                    // Add new HTML
                    var content = $(result.filter('#fields-content')[0]);
                    var details = $('<div>').append(content.clone()).html();
                    $("#fields-definition").html(details);

                    // Add new Scripts
                    var scripts = result.filter('script');

                    $.each(scripts, function () {
                        var script = $(this);
                        script.attr("data-fields-script", "true");//for some reason, .data("fields-script", "true") doesn't work here
                        script.appendTo('body');
                    });

                    // Update Bindings
                    // Ensure the function exists before calling it...
                    if (typeof pageModel != null) {
                        self.pageModelStub = pageModel;
                        if (typeof self.pageModelStub.updateModel === 'function') {
                            self.pageModelStub.updateModel();
                        }
                        ko.applyBindings(viewModel, elementToBind);
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(translations.GetRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetTranslationError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.showPageTypes = function () {
        self.showToolbar(false);
        switchSection($("#page-type-grid-section"));
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
            Order: { required: true, digits: true }
        }
    });
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    switchSection($("#blank-section"));

    $("#PageTypesGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/cms/PageTypeApi",
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
                        Name: { type: "string" }
                    }
                }
            },
            pageSize: gridPageSize,
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
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<a onclick="viewModel.pageType.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 130
        }]
    });

    var treeviewDS = new kendo.data.HierarchicalDataSource({
        type: "odata",
        transport: {
            read: {
                url: "/odata/kore/cms/PageTreeApi?$expand=SubPages/SubPages",
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
                children: "SubPages"
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
                // A page cannot be a parent of itself!
                return;
            }

            var parentId = null;
            var destinationPage = null;

            if (viewModel.id() == destinationId) {
                destinationPage = {
                    Id: viewModel.id(),
                    ParentId: viewModel.parentId()
                };
            }
            else {
                $.ajax({
                    url: "/odata/kore/cms/PageApi(guid'" + destinationId + "')",
                    type: "GET",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    destinationPage = {
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

            if (destinationPage.ParentId == sourceId) {
                $.notify(translations.CircularRelationshipError, "error");
                $("#treeview").data("kendoTreeView").dataSource.read();
                return;
            }

            switch (dropPosition) {
                case 'over':
                    parentId = destinationId;
                    break;
                default:
                    parentId = destinationPage.ParentId;
                    break;
            }

            var patch = {
                ParentId: parentId
            };

            $.ajax({
                url: "/odata/kore/cms/PageApi(guid'" + sourceId + "')",
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