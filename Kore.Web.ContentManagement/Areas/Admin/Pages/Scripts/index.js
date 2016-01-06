define(function (require) {
    'use strict'

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

    var PageTypeModel = function (parent) {
        var self = this;

        self.parent = parent;
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
                                Name: { type: "string" }
                            }
                        }
                    },
                    pageSize: self.parent.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Name", dir: "asc" }
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
                    field: "Name",
                    title: self.parent.translations.Columns.PageType.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<a data-bind="click: pageTypeModel.edit.bind($data,\'#=Id#\')" class="btn btn-default btn-xs">' + self.parent.translations.Edit + '</a>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 130
                }]
            });
        };
        self.edit = function (id) {
            $.ajax({
                url: "/odata/kore/cms/PageTypeApi(" + id + ")",
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
                    self.layoutPath(self.parent.defaultFrontendLayoutPath);
                }

                self.validator.resetForm();
                switchSection($("#page-type-form-section"));
                self.parent.displayMode('PageTypeEdit');
                $("#page-type-form-section-legend").html(self.parent.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
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
                    self.parent.displayMode('PageTypeGrid');

                    $.notify(self.parent.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                $.ajax({
                    url: "/odata/kore/cms/PageTypeApi(" + self.id() + ")",
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
                    self.parent.displayMode('PageTypeGrid');

                    $.notify(self.parent.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#page-type-grid-section"));
            self.parent.displayMode('PageTypeGrid');
        };
    };

    var PageVersionModel = function (parent) {
        var self = this;

        self.parent = parent;
        self.id = ko.observable(emptyGuid);
        self.pageId = ko.observable(emptyGuid);
        self.cultureCode = ko.observable(null);
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
                                Title: { type: "string" },
                                DateModifiedUtc: { type: "date" },
                                IsEnabled: { type: "boolean" }
                            }
                        }
                    },
                    pageSize: self.parent.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "DateModifiedUtc", dir: "desc" }
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
                    field: "Title",
                    title: self.parent.translations.Columns.PageVersion.Title,
                    filterable: true
                }, {
                    field: "Slug",
                    title: self.parent.translations.Columns.PageVersion.Slug,
                    filterable: true
                }, {
                    field: "DateModifiedUtc",
                    title: self.parent.translations.Columns.PageVersion.DateModifiedUtc,
                    filterable: true,
                    width: 180,
                    type: "date",
                    format: "{0:G}"
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a data-bind="click: pageVersionModel.restore.bind($data,\'#=Id#\')" class="btn btn-warning btn-xs">' + self.parent.translations.Restore + '</a>' +
                        '<a data-bind="click: pageModel.preview.bind($data,\'#=Id#\')" class="btn btn-default btn-xs">' + self.parent.translations.Preview + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 130
                }]
            });
        };
        self.restore = function (id) {
            if (confirm(self.parent.translations.PageHistoryRestoreConfirm)) {
                $.ajax({
                    url: "/odata/kore/cms/PageVersionApi(" + id + ")/Default.RestoreVersion",
                    type: "POST"
                })
                .done(function (json) {
                    $('#PageVersionGrid').data('kendoGrid').dataSource.read();
                    $('#PageVersionGrid').data('kendoGrid').refresh();
                    switchSection($("#blank-section"));
                    self.parent.displayMode('Blank');
                    $.notify(self.parent.translations.PageHistoryRestoreSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.PageHistoryRestoreError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            };
        };
        self.goBack = function () {
            switchSection($("#form-section"));
            self.parent.displayMode('EditPage');
        };
    };

    var PageModel = function (parent) {
        var self = this;

        self.parent = parent;
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

        self.selectedTopLevelPageId = null;

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

            self.reloadTopLevelPages();

            var treeviewDS = new kendo.data.HierarchicalDataSource({
                type: "odata",
                transport: {
                    read: {
                        url: "/odata/kore/cms/PageTreeApi?$expand=SubPages($levels=max)",
                        dataType: "json"
                    },
                    parameterMap: function (options, operation) {
                        var paramMap = kendo.data.transports.odata.parameterMap(options);
                        if (paramMap.$inlinecount) {
                            //if (paramMap.$inlinecount == "allpages") {
                            //    paramMap.$count = true;
                            //}
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
                        if (response.value) {
                            return response.value;
                        }

                        var dummyArray = [];
                        dummyArray.push(response);
                        return dummyArray;
                    },
                    total: function (response) {
                        if (response.value) {
                            return response.value.length;
                        }
                        return 1;
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
                    var treeview = this.element.find("ul")[0];
                    if (treeview) {
                        ko.cleanNode(treeview);
                        ko.applyBindings(ko.dataFor(treeview), treeview);
                    }
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

                    if (self.parent.pageModel.id() == destinationId) {
                        destinationPage = {
                            Id: self.parent.pageModel.id(),
                            ParentId: self.parent.pageModel.parentId()
                        };
                    }
                    else {
                        $.ajax({
                            url: "/odata/kore/cms/PageApi(" + destinationId + ")",
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
                            $.notify(self.parent.translations.GetRecordError, "error");
                            console.log(textStatus + ': ' + errorThrown);
                            return;
                        });
                    }

                    if (destinationPage.ParentId == sourceId) {
                        $.notify(self.parent.translations.CircularRelationshipError, "error");
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
                        url: "/odata/kore/cms/PageApi(" + sourceId + ")",
                        type: "PATCH",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(patch),
                        dataType: "json",
                        async: false
                    })
                    .done(function (json) {
                        $("#treeview").data("kendoTreeView").dataSource.read();
                        self.reloadTopLevelPages();
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        $.notify(self.parent.translations.UpdateRecordError, "error");
                        console.log(textStatus + ': ' + errorThrown);
                    });
                }
            });

            self.pageVersionGrid = $('#PageVersionGrid').data('kendoGrid');
        };
        self.create = function () {
            self.parent.currentCulture = null;

            self.id(emptyGuid);
            self.parentId(self.selectedTopLevelPageId);
            self.pageTypeId(emptyGuid);
            self.name(null);
            self.isEnabled(false);
            self.order(0);
            self.showOnMenus(true);
            self.accessRestrictions = null;

            self.roles([]);

            self.setupVersionCreateSection();

            self.parent.displayMode('CreatePage');

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(self.parent.translations.Create);
        };
        self.setupVersionCreateSection = function () {
            self.parent.pageVersionModel.id(emptyGuid);
            self.parent.pageVersionModel.pageId(emptyGuid);
            self.parent.pageVersionModel.cultureCode(self.parent.currentCulture);
            self.parent.pageVersionModel.status(0);
            self.parent.pageVersionModel.title(null);
            self.parent.pageVersionModel.slug(null);
            self.parent.pageVersionModel.fields(null);

            // Clean up from previously injected html/scripts
            if (self.parent.pageVersionModel.pageModelStub != null && typeof self.parent.pageVersionModel.pageModelStub.cleanUp === 'function') {
                self.parent.pageVersionModel.pageModelStub.cleanUp(self.parent.pageVersionModel);
            }
            self.parent.pageVersionModel.pageModelStub = null;

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
        self.edit = function (id, cultureCode) {
            if (cultureCode) {
                self.parent.currentCulture = cultureCode;
            }
            else {
                self.parent.currentCulture = null;
            }

            $.ajax({
                url: "/odata/kore/cms/PageApi(" + id + ")",
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
                    url: "/odata/kore/cms/PageVersionApi/Default.GetCurrentVersion(pageId=" + self.id() + ",cultureCode='" + self.parent.currentCulture + "')",
                    type: "GET",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    self.setupVersionEditSection(json);
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.GetRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });

                self.parent.displayMode('EditPage');

                self.validator.resetForm();
                switchSection($("#form-section"));
                $("#form-section-legend").html(self.parent.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.setupVersionEditSection = function (json) {
            self.parent.pageVersionModel.id(json.Id);
            self.parent.pageVersionModel.pageId(json.PageId);

            // Don't do this, since API may return invariant version if localized does not exist yet...
            //self.parent.pageVersionModel.cultureCode(json.CultureCode);

            // So do this instead...
            self.parent.pageVersionModel.cultureCode(self.parent.currentCulture);

            self.parent.pageVersionModel.status(json.Status);
            self.parent.pageVersionModel.title(json.Title);
            self.parent.pageVersionModel.slug(json.Slug);
            self.parent.pageVersionModel.fields(json.Fields);

            if (json.Status == 'Draft') {
                self.parent.pageVersionModel.isDraft(true);
            }
            else {
                self.parent.pageVersionModel.isDraft(false);
            }

            $.ajax({
                url: "/admin/pages/get-editor-ui/" + self.parent.pageVersionModel.id(),
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                // Clean up from previously injected html/scripts
                if (self.parent.pageVersionModel.pageModelStub != null && typeof self.parent.pageVersionModel.pageModelStub.cleanUp === 'function') {
                    self.parent.pageVersionModel.pageModelStub.cleanUp(self.parent.pageVersionModel);
                }
                self.parent.pageVersionModel.pageModelStub = null;

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
                    self.parent.pageVersionModel.pageModelStub = pageModel;
                    if (typeof self.parent.pageVersionModel.pageModelStub.updateModel === 'function') {
                        self.parent.pageVersionModel.pageModelStub.updateModel(self.parent.pageVersionModel);
                    }
                    ko.applyBindings(self.parent, elementToBind);
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });

            self.versionValidator.resetForm();
        };
        self.remove = function () {
            if (confirm(self.parent.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/cms/PageApi(" + self.id() + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    self.refresh();
                    $.notify(self.parent.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.DeleteRecordError, "error");
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
                    $.notify(self.parent.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                $.ajax({
                    url: "/odata/kore/cms/PageApi(" + self.id() + ")",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $.notify(self.parent.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });

                self.saveVersion();
            }

            self.refresh();
        };
        self.saveVersion = function () {

            // ensure the function exists before calling it...
            if (self.parent.pageVersionModel.pageModelStub != null && typeof self.parent.pageVersionModel.pageModelStub.onBeforeSave === 'function') {
                self.parent.pageVersionModel.pageModelStub.onBeforeSave(self.parent.pageVersionModel);
            }

            var cultureCode = self.parent.pageVersionModel.cultureCode();
            if (cultureCode == '') {
                cultureCode = null;
            }

            var status = 'Draft';

            // if not preset to 'Archived' status...
            if (self.parent.pageVersionModel.status() != 'Archived') {
                // and checkbox for Draft has been set,
                if (self.parent.pageVersionModel.isDraft()) {
                    // then change status to 'Draft'
                    status = 'Draft';
                }
                else {
                    // else change status to 'Published'
                    status = 'Published';
                }
            }

            var record = {
                Id: self.parent.pageVersionModel.id(), // Should always create a new one, so don't send Id!
                PageId: self.parent.pageVersionModel.pageId(),
                CultureCode: cultureCode,
                Status: status,
                Title: self.parent.pageVersionModel.title(),
                Slug: self.parent.pageVersionModel.slug(),
                Fields: self.parent.pageVersionModel.fields(),
            };

            // UPDATE only (no option for insert here)
            $.ajax({
                url: "/odata/kore/cms/PageVersionApi(" + self.parent.pageVersionModel.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $.notify(self.parent.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.cancel = function () {

            // Clean up from previously injected html/scripts
            if (self.parent.pageVersionModel.pageModelStub != null && typeof self.parent.pageVersionModel.pageModelStub.cleanUp === 'function') {
                self.parent.pageVersionModel.pageModelStub.cleanUp(self.parent.pageVersionModel);
            }
            self.parent.pageVersionModel.pageModelStub = null;

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

            self.parent.displayMode('Blank');

            switchSection($("#blank-section"));
        };
        self.toggleEnabled = function () {
            var patch = {
                IsEnabled: !self.isEnabled()
            };

            $.ajax({
                url: "/odata/kore/cms/PageApi(" + self.id() + ")",
                type: "PATCH",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(patch),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.refresh();
                $.notify(self.parent.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.showPageHistory = function () {
            self.parent.displayMode('PageHistoryGrid');

            if (self.parent.currentCulture == null || self.parent.currentCulture == "") {
                self.pageVersionGrid.dataSource.transport.options.read.url = "/odata/kore/cms/PageVersionApi?$filter=CultureCode eq null and PageId eq " + self.id();
            }
            else {
                self.pageVersionGrid.dataSource.transport.options.read.url = "/odata/kore/cms/PageVersionApi?$filter=CultureCode eq '" + self.parent.currentCulture + "' and PageId eq " + self.id();
            }
            self.pageVersionGrid.dataSource.page(1);

            switchSection($("#version-grid-section"));
        };
        self.showPageTypes = function () {
            self.parent.displayMode('PageTypeGrid');
            switchSection($("#page-type-grid-section"));
        };
        self.refresh = function () {
            switchSection($("#blank-section"));
            self.parent.displayMode('Blank');
            self.reloadTopLevelPages();
            self.filter(null);
        };
        self.previewCurrent = function () {
            self.preview(self.parent.pageVersionModel.id());
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
        self.filter = function () {
            var treeview = $('#treeview').data('kendoTreeView');

            if (!treeview) {
                return;
            }

            var id = $('#TopLevelPages').val();

            if (!id) {
                self.selectedTopLevelPageId = null;
                treeview.dataSource.transport.options.read.url = "/odata/kore/cms/PageTreeApi?$expand=SubPages($levels=max)",
                treeview.dataSource.read();
            }
            else {
                self.selectedTopLevelPageId = id;
                treeview.dataSource.transport.options.read.url = "/odata/kore/cms/PageTreeApi(" + id + ")?$expand=SubPages($levels=max)",
                treeview.dataSource.read();
            }
        };
        self.reloadTopLevelPages = function () {
            $.ajax({
                url: "/odata/kore/cms/PageApi/Default.GetTopLevelPages()",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#TopLevelPages,#ParentId').html('');
                $('#TopLevelPages,#ParentId').append($('<option>', {
                    value: '',
                    text: '[Root]'
                }));
                $.each(json.value, function () {
                    var item = this;
                    $('#TopLevelPages,#ParentId').append($('<option>', {
                        value: item.Id,
                        text: item.Name
                    }));
                });

                var elementToBind = $("#TopLevelPages,#ParentId")[0];
                ko.cleanNode(elementToBind);
                ko.applyBindings(self.parent, elementToBind);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.move = function () {
            $("#parentPageModal").modal("show");
        };
        self.onParentSelected = function () {
            var parentId = $("#ParentId").val();

            if (parentId == self.id()) {
                $("#parentPageModal").modal("hide");
                return;
            }
            if (parentId == '') {
                parentId = null;
            }

            var patch = {
                ParentId: parentId
            };

            $.ajax({
                url: "/odata/kore/cms/PageApi(" + self.id() + ")",
                type: "PATCH",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(patch),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $("#parentPageModal").modal("hide");
                self.refresh();
                $.notify(self.parent.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.localize = function () {
            $("#cultureModal").modal("show");
        };
        self.onCultureSelected = function () {
            var id = self.id();
            var cultureCode = $("#CultureCode").val();
            self.edit(id, cultureCode);
            $("#cultureModal").modal("hide");
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
            self.pageModel = new PageModel(self);
            self.pageVersionModel = new PageVersionModel(self);
            self.pageTypeModel = new PageTypeModel(self);
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
            self.defaultFrontendLayoutPath = $("#DefaultFrontendLayoutPath").val();

            if (!self.defaultFrontendLayoutPath) {
                self.defaultFrontendLayoutPath = null;
            }

            self.pageTypeModel.init();
            self.pageVersionModel.init();
            self.pageModel.init(); // initialize this last, so that pageVersionGrid is not undefined
        };
    };

    var viewModel = new ViewModel();
    return viewModel;
});