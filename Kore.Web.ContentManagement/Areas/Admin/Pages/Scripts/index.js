var viewModel;

define(function (require) {
    'use strict'

    viewModel = null;

    var $ = require('jquery');
    var ko = require('knockout');
    var koMap = require('knockout-mapping');

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

    ko.mapping = koMap;

    var PageTypeModel = function () {
        var self = this;

        self.id = ko.observable(emptyGuid);
        self.name = ko.observable(null);
        self.layoutPath = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#page-type-form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 },
                    LayoutPath: { required: true, maxlength: 255 }
                }
            });

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
                    title: viewModel.translations.Columns.PageType.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<a onclick="viewModel.pageTypeModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 130
                }]
            });
        };
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

                if (json.LayoutPath) {
                    self.layoutPath(json.LayoutPath);
                }
                else {
                    self.layoutPath(viewModel.defaultFrontendLayoutPath);
                }

                self.validator.resetForm();
                switchSection($("#page-type-form-section"));
                viewModel.displayMode('PageTypeEdit');
                $("#page-type-form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.save = function () {
            var isNew = (self.id() == emptyGuid);

            if (!$("#page-type-form-section-form").valid()) {
                return false;
            }

            var record = {
                Id: self.id(),
                Name: self.name(),
                LayoutPath: self.layoutPath()
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
                    viewModel.displayMode('PageTypeGrid');

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
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
                    viewModel.displayMode('PageTypeGrid');

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#page-type-grid-section"));
            viewModel.displayMode('PageTypeGrid');
        };
    };

    var PageVersionModel = function () {
        var self = this;

        self.id = ko.observable(emptyGuid);
        self.pageId = ko.observable(emptyGuid);
        self.cultureCode = ko.observable(viewModel.currentCulture);
        self.status = ko.observable('Draft');
        self.title = ko.observable(null);
        self.slug = ko.observable(null);
        self.fields = ko.observable(null);

        self.isDraft = ko.observable(true);

        self.pageModelStub = null;

        self.init = function () {
            $("#PageVersionGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/cms/PageVersionApi?$filter=CultureCode eq null",
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
                                DateModifiedUtc: { type: "date" },
                                IsEnabled: { type: "boolean" }
                            }
                        }
                    },
                    pageSize: viewModel.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "DateModifiedUtc", dir: "desc" }
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
                    title: viewModel.translations.Columns.PageVersion.Title,
                    filterable: true
                }, {
                    field: "Slug",
                    title: viewModel.translations.Columns.PageVersion.Slug,
                    filterable: true
                }, {
                    field: "DateModifiedUtc",
                    title: viewModel.translations.Columns.PageVersion.DateModifiedUtc,
                    filterable: true,
                    width: 180,
                    type: "date",
                    format: "{0:G}"
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a onclick="viewModel.pageVersionModel.restore(\'#=Id#\')" class="btn btn-warning btn-xs">' + viewModel.translations.Restore + '</a>' +
                        '<a onclick="viewModel.pageModel.preview(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Preview + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 130
                }]
            });
        };
        self.restore = function (id) {
            if (confirm(viewModel.translations.PageHistoryRestoreConfirm)) {
                $.ajax({
                    url: "/odata/kore/cms/PageVersionApi(guid'" + id + "')/RestoreVersion",
                    type: "POST"
                })
                .done(function (json) {
                    $('#PageVersionGrid').data('kendoGrid').dataSource.read();
                    $('#PageVersionGrid').data('kendoGrid').refresh();
                    switchSection($("#blank-section"));
                    viewModel.displayMode('Blank');
                    $.notify(viewModel.translations.PageHistoryRestoreSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.PageHistoryRestoreError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            };
        };
        self.goBack = function () {
            switchSection($("#form-section"));
            viewModel.displayMode('EditPage');
        };
    };

    var PageModel = function () {
        var self = this;

        self.id = ko.observable(emptyGuid);
        self.parentId = ko.observable(null);
        self.pageTypeId = ko.observable(emptyGuid);
        self.name = ko.observable(null);
        self.isEnabled = ko.observable(false);
        self.order = ko.observable(0);
        self.showOnMenus = ko.observable(true);

        self.accessRestrictions = null;
        self.roles = ko.observableArray([]);

        self.pageVersionGrid = null;

        self.validator = false;
        self.versionValidator = false;

        self.init = function () {
            self.validator = $("#form-section-form").validate({
                rules: {
                    Title: { required: true, maxlength: 255 },
                    Order: { required: true, digits: true }
                }
            });

            self.versionValidator = $("#form-section-version-form").validate({
                rules: {
                    Version_Title: { required: true, maxlength: 255 },
                    Version_Slug: { required: true, maxlength: 255 }
                }
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
                dataTextField: ["Title"],
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

                    if (viewModel.pageModel.id() == destinationId) {
                        destinationPage = {
                            Id: viewModel.pageModel.id(),
                            ParentId: viewModel.pageModel.parentId()
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
                            $.notify(viewModel.translations.GetRecordError, "error");
                            console.log(textStatus + ': ' + errorThrown);
                            return;
                        });
                    }

                    if (destinationPage.ParentId == sourceId) {
                        $.notify(viewModel.translations.CircularRelationshipError, "error");
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
                        $.notify(viewModel.translations.UpdateRecordError, "error");
                        console.log(textStatus + ': ' + errorThrown);
                    });
                }
            });

            self.pageVersionGrid = $('#PageVersionGrid').data('kendoGrid');
        };
        self.create = function () {
            self.id(emptyGuid);
            self.parentId(null);
            self.pageTypeId(emptyGuid);
            self.name(null);
            self.isEnabled(false);
            self.order(0);
            self.showOnMenus(true);
            self.accessRestrictions = null;

            self.roles([]);

            self.setupVersionCreateSection();

            viewModel.displayMode('CreatePage');

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(viewModel.translations.Create);
        };
        self.setupVersionCreateSection = function () {
            viewModel.pageVersionModel.id(emptyGuid);
            viewModel.pageVersionModel.pageId(emptyGuid);
            viewModel.pageVersionModel.cultureCode(viewModel.currentCulture);
            viewModel.pageVersionModel.status(0);
            viewModel.pageVersionModel.title(null);
            viewModel.pageVersionModel.slug(null);
            viewModel.pageVersionModel.fields(null);

            // Clean up from previously injected html/scripts
            if (viewModel.pageVersionModel.pageModelStub != null && typeof viewModel.pageVersionModel.pageModelStub.cleanUp === 'function') {
                viewModel.pageVersionModel.pageModelStub.cleanUp();
            }
            viewModel.pageVersionModel.pageModelStub = null;

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

            self.versionValidator.resetForm();
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
                self.isEnabled(json.IsEnabled);
                self.order(json.Order);
                self.showOnMenus(json.ShowOnMenus);
                self.accessRestrictions = ko.mapping.fromJSON(json.AccessRestrictions);

                if (self.accessRestrictions.Roles != null) {
                    var split = self.accessRestrictions.Roles().split(',');
                    self.roles(split);
                }
                else {
                    self.roles([]);
                }

                $.ajax({
                    url: "/odata/kore/cms/PageVersionApi/GetCurrentVersion",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        pageId: self.id(),
                        cultureCode: viewModel.currentCulture
                    }),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    self.setupVersionEditSection(json);
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.GetRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });

                viewModel.displayMode('EditPage');

                self.validator.resetForm();
                switchSection($("#form-section"));
                $("#form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.setupVersionEditSection = function (json) {
            viewModel.pageVersionModel.id(json.Id);
            viewModel.pageVersionModel.pageId(json.PageId);
            viewModel.pageVersionModel.cultureCode(json.CultureCode);
            viewModel.pageVersionModel.status(json.Status);
            viewModel.pageVersionModel.title(json.Title);
            viewModel.pageVersionModel.slug(json.Slug);
            viewModel.pageVersionModel.fields(json.Fields);

            if (json.Status == 'Draft') {
                viewModel.pageVersionModel.isDraft(true);
            }
            else {
                viewModel.pageVersionModel.isDraft(false);
            }

            $.ajax({
                url: "/admin/pages/get-editor-ui/" + viewModel.pageVersionModel.id(),
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                // Clean up from previously injected html/scripts
                if (viewModel.pageVersionModel.pageModelStub != null && typeof viewModel.pageVersionModel.pageModelStub.cleanUp === 'function') {
                    viewModel.pageVersionModel.pageModelStub.cleanUp();
                }
                viewModel.pageVersionModel.pageModelStub = null;

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
                    viewModel.pageVersionModel.pageModelStub = pageModel;
                    if (typeof viewModel.pageVersionModel.pageModelStub.updateModel === 'function') {
                        viewModel.pageVersionModel.pageModelStub.updateModel();
                    }
                    ko.applyBindings(viewModel, elementToBind);
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });

            self.versionValidator.resetForm();
        };
        self.remove = function () {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/cms/PageApi(guid'" + self.id() + "')",
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
            var isNew = (self.id() == emptyGuid);

            if (!$("#form-section-form").valid()) {
                return false;
            }

            if (!isNew) {
                if (!$("#form-section-version-form").valid()) {
                    return false;
                }
            }

            var record = {
                Id: self.id(),
                ParentId: self.parentId(),
                PageTypeId: self.pageTypeId(),
                Name: self.name(),
                IsEnabled: self.isEnabled(),
                Order: self.order(),
                ShowOnMenus: self.showOnMenus(),
                AccessRestrictions: JSON.stringify({
                    Roles: self.roles().join()
                }),
            };

            if (isNew) {
                $.ajax({
                    url: "/odata/kore/cms/PageApi",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                $.ajax({
                    url: "/odata/kore/cms/PageApi(guid'" + self.id() + "')",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });

                self.saveVersion();
            }

            self.refresh();
        };
        self.saveVersion = function () {

            // ensure the function exists before calling it...
            if (viewModel.pageVersionModel.pageModelStub != null && typeof viewModel.pageVersionModel.pageModelStub.onBeforeSave === 'function') {
                viewModel.pageVersionModel.pageModelStub.onBeforeSave();
            }

            var cultureCode = viewModel.pageVersionModel.cultureCode();
            if (cultureCode == '') {
                cultureCode = null;
            }

            var status = 'Draft';

            // if not preset to 'Archived' status...
            if (viewModel.pageVersionModel.status() != 'Archived') {
                // and checkbox for Draft has been set,
                if (viewModel.pageVersionModel.isDraft()) {
                    // then change status to 'Draft'
                    status = 'Draft';
                }
                else {
                    // else change status to 'Published'
                    status = 'Published';
                }
            }

            var record = {
                Id: viewModel.pageVersionModel.id(),
                PageId: viewModel.pageVersionModel.pageId(),
                CultureCode: cultureCode,
                Status: status,
                Title: viewModel.pageVersionModel.title(),
                Slug: viewModel.pageVersionModel.slug(),
                Fields: viewModel.pageVersionModel.fields(),
            };

            // UPDATE only (no option for insert here)
            $.ajax({
                url: "/odata/kore/cms/PageVersionApi(guid'" + viewModel.pageVersionModel.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $.notify(viewModel.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.cancel = function () {

            // Clean up from previously injected html/scripts
            if (viewModel.pageVersionModel.pageModelStub != null && typeof viewModel.pageVersionModel.pageModelStub.cleanUp === 'function') {
                viewModel.pageVersionModel.pageModelStub.cleanUp();
            }
            viewModel.pageVersionModel.pageModelStub = null;

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

            viewModel.displayMode('Blank');

            switchSection($("#blank-section"));
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
                $.notify(viewModel.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.showPageHistory = function () {
            viewModel.displayMode('PageHistoryGrid');

            if (viewModel.currentCulture == null || viewModel.currentCulture == "") {
                self.pageVersionGrid.dataSource.transport.options.read.url = "/odata/kore/cms/PageVersionApi?$filter=CultureCode eq null and PageId eq guid'" + self.id() + "'";
            }
            else {
                self.pageVersionGrid.dataSource.transport.options.read.url = "/odata/kore/cms/PageVersionApi?$filter=CultureCode eq '" + viewModel.currentCulture + "' and PageId eq guid'" + self.id() + "'";
            }
            self.pageVersionGrid.dataSource.page(1);

            switchSection($("#version-grid-section"));
        };
        self.showPageTypes = function () {
            viewModel.displayMode('PageTypeGrid');
            switchSection($("#page-type-grid-section"));
        };
        self.refresh = function () {
            switchSection($("#blank-section"));
            viewModel.displayMode('Blank');
            $("#treeview").data("kendoTreeView").dataSource.read();
        };
        self.previewCurrent = function () {
            self.preview(viewModel.pageVersionModel.id());
        };
        self.preview = function (id) {
            var win = window.open('/admin/pages/preview/' + id, '_blank');
            if (win) {
                win.focus();
            } else {
                alert('Please allow popups for this site');
            }
            return false;
        };
    };

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;
        self.currentCulture = null;
        self.defaultFrontendLayoutPath = null;

        self.pageModel = false;
        self.pageVersionModel = false;
        self.pageTypeModel = false;

        self.displayMode = ko.observable('Blank');

        self.activate = function () {
            self.pageModel = new PageModel();
            self.pageVersionModel = new PageVersionModel();
            self.pageTypeModel = new PageTypeModel();
        };
        self.attached = function () {
            currentSection = $("#blank-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/pages/get-translations",
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
            self.currentCulture = $("#CurrentCulture").val();
            self.defaultFrontendLayoutPath = $("#DefaultFrontendLayoutPath").val();

            if (!self.currentCulture) {
                self.currentCulture = null;
            }
            if (!self.defaultFrontendLayoutPath) {
                self.defaultFrontendLayoutPath = null;
            }

            self.pageTypeModel.init();
            self.pageVersionModel.init();
            self.pageModel.init(); // initialize this last, so that pageVersionGrid is not undefined
        };
    };

    viewModel = new ViewModel();
    return viewModel;
});