'use strict'

var RoleVM = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.name = ko.observable('');

    self.permissions = ko.observableArray([]);

    self.create = function () {
        self.id(emptyGuid);
        self.name('');

        self.permissions([]);

        self.validator.resetForm();
        switchSection($("#roles-form-section"));
        $("#roles-form-section-legend").html("Create New");
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/RoleApi('" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);

            self.permissions([]);

            self.validator.resetForm();
            switchSection($("#roles-form-section"));
            $("#roles-form-section-legend").html("Edit");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify("There was an error when retrieving the record.", "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm("Are you sure you want to delete this record?")) {
            $.ajax({
                url: "/odata/kore/cms/RoleApi('" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#RolesGrid').data('kendoGrid').dataSource.read();
                $('#RolesGrid').data('kendoGrid').refresh();

                $.notify("Successfully deleted record.", "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify("There was an error when deleting the record.", "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.save = function () {

        if (!$("#roles-form-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            Name: self.name()
        };

        if (self.id() == emptyGuid) {
            // INSERT
            $.ajax({
                url: "/odata/kore/cms/RoleApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#RolesGrid').data('kendoGrid').dataSource.read();
                $('#RolesGrid').data('kendoGrid').refresh();

                switchSection($("#roles-grid-section"));

                $.notify("Successfully inserted record.", "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify("There was an error when inserting the record.", "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            // UPDATE
            $.ajax({
                url: "/odata/kore/cms/RoleApi('" + self.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#RolesGrid').data('kendoGrid').dataSource.read();
                $('#RolesGrid').data('kendoGrid').refresh();

                switchSection($("#roles-grid-section"));

                $.notify("Successfully updated record.", "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify("There was an error when updating the record.", "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#roles-grid-section"));
    };

    self.editPermissions = function (id) {
        self.id(id);
        self.permissions([]);

        $.ajax({
            url: "/odata/kore/cms/PermissionApi/GetPermissionsForRole",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ roleId: id }),
            dataType: "json",
            async: false
        })
        .done(function (json) {
            if (json.value && json.value.length > 0) {
                $.each(json.value, function () {
                    self.permissions.push(this.Id);
                });
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify("There was an error when retrieving the record.", "error");
            console.log(textStatus + ': ' + errorThrown);
        });

        switchSection($("#role-permissions-form-section"));
    };

    self.editPermissions_cancel = function (id) {
        switchSection($("#roles-grid-section"));
    };

    self.editPermissions_save = function () {
        var data = {
            roleId: self.id(),
            permissions: self.permissions()
        };

        $.ajax({
            url: "/odata/kore/cms/RoleApi/AssignPermissionsToRole",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            async: false
        })
        .done(function (json) {
            switchSection($("#roles-grid-section"));
            $.notify("Successfully saved permissions.", "success");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify("There was an error when saving permissions.", "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.validator = $("#roles-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 }
        },
        messages: {
            Name: {
                required: "Name is required.",
                maxlength: "Name cannot consist of more than 255 characters."
            }
        },
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

    self.gridConfig = {
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/cms/RoleApi",
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
            pageSize: 10,
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
            title: "Name",
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.roleModel.editPermissions(\'#=Id#\')" class="btn btn-default btn-xs">Permissions</a>' +
                '<a onclick="viewModel.roleModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">Edit</a>' +
                '<a onclick="viewModel.roleModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">Delete</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 200
        }]
    };
};