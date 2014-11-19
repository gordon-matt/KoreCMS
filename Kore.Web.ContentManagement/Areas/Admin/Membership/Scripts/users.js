'use strict'

var ChangePasswordVM = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.userName = ko.observable('');
    self.password = ko.observable('');
    self.confirmPassword = ko.observable('');

    self.cancel = function () {
        switchSection($("#users-grid-section"));
    };

    self.save = function () {
        if (!$("#change-password-form-section-form").valid()) {
            return false;
        }

        var record = {
            userId: self.id(),
            password: self.password()
        };

        $.ajax({
            url: "/odata/kore/cms/Users/ChangePassword",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(record),
            dataType: "json",
            async: false
        })
        .done(function (json) {
            switchSection($("#users-grid-section"));
            $.notify("Successfully changed password.", "success");
        })
        .fail(function () {
            $.notify("There was an error when changing the password.", "error");
        });
    };

    self.validator = $("#change-password-form-section-form").validate({
        rules: {
            Change_Password: { required: true, maxlength: 128 },
            Change_ConfirmPassword: { required: true, maxlength: 128, equalTo: "#Change_Password" },
        },
        messages: {
            Change_Password: {
                required: "Password is required.",
                maxlength: "Password cannot consist of more than 128 characters."
            },
            Change_ConfirmPassword: {
                required: "Confirm Password is required.",
                maxlength: "Confirm Password cannot consist of more than 128 characters."
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
};

var UserVM = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.userName = ko.observable('');
    self.email = ko.observable('');
    self.isLockedOut = ko.observable(false);

    self.roles = ko.observableArray([]);
    self.filterRoleId = ko.observable(emptyGuid);

    self.create = function () {
        self.id(emptyGuid);
        self.userName('');
        self.email('');
        self.isLockedOut(false);

        self.roles([]);
        self.filterRoleId(emptyGuid);

        self.validator.resetForm();
        switchSection($("#users-edit-form-section"));
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/Users('" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.userName(json.UserName);
            self.email(json.Email);
            self.isLockedOut(json.IsLockedOut);

            self.roles([]);
            self.filterRoleId(emptyGuid);

            self.validator.resetForm();
            switchSection($("#users-edit-form-section"));
        })
        .fail(function () {
            $.notify("There was an error when retrieving the record.", "error");
        });
    };

    self.delete = function (id) {
        if (confirm("Are you sure you want to delete this record?")) {
            $.ajax({
                url: "/odata/kore/cms/Users('" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#UsersGrid').data('kendoGrid').dataSource.read();
                $('#UsersGrid').data('kendoGrid').refresh();

                $.notify("Successfully deleted record.", "success");
            })
            .fail(function () {
                $.notify("There was an error when deleting the record.", "error");
            });
        }
    };

    self.save = function () {

        var isNew = (self.id() == emptyGuid);

        if (!$("#users-edit-form-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            UserName: self.userName(),
            Email: self.email(),
            IsLockedOut: self.isLockedOut()
        };

        if (isNew) {
            // INSERT
            $.ajax({
                url: "/odata/kore/cms/Users",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#UsersGrid').data('kendoGrid').dataSource.read();
                $('#UsersGrid').data('kendoGrid').refresh();

                switchSection($("#users-grid-section"));

                $.notify("Successfully inserted record.", "success");
            })
            .fail(function () {
                $.notify("There was an error when inserting the record.", "error");
            });
        }
        else {
            // UPDATE
            $.ajax({
                url: "/odata/kore/cms/Users('" + self.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#UsersGrid').data('kendoGrid').dataSource.read();
                $('#UsersGrid').data('kendoGrid').refresh();

                switchSection($("#users-grid-section"));

                $.notify("Successfully updated record.", "success");
            })
            .fail(function () {
                $.notify("There was an error when updating the record.", "error");
            });
        }
    };

    self.cancel = function () {
        switchSection($("#users-grid-section"));
    };

    self.editRoles = function (id) {
        self.id(id);
        self.roles([]);
        self.filterRoleId(emptyGuid);

        $.ajax({
            url: "/odata/kore/cms/Roles/GetRolesForUser",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ userId: id }),
            dataType: "json",
            async: false
        })
        .done(function (json) {
            if (json.value && json.value.length > 0) {
                $.each(json.value, function () {
                    self.roles.push(this.Id);
                });
            }
        })
        .fail(function () {
            $.notify("There was an error when retrieving the record.", "error");
        });

        switchSection($("#user-roles-form-section"));
    };

    self.editRoles_cancel = function () {
        switchSection($("#users-grid-section"));
    };

    self.editRoles_save = function () {
        var data = {
            userId: self.id(),
            roles: self.roles()
        };

        $.ajax({
            url: "/odata/kore/cms/Users/AssignUserToRoles",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "json",
            async: false
        })
        .done(function (json) {
            switchSection($("#users-grid-section"));
            $.notify("Successfully saved roles.", "success");
        })
        .fail(function () {
            $.notify("There was an error when saving roles.", "error");
        });
    };

    self.changePassword = function (id, userName) {
        viewModel.changePasswordModel.id(id);
        viewModel.changePasswordModel.userName(userName);
        viewModel.changePasswordModel.validator.resetForm();
        switchSection($("#change-password-form-section"));
    };

    self.filterRole = function () {
        var grid = $('#UsersGrid').data('kendoGrid');

        if (self.filterRoleId() == "") {
            grid.dataSource.transport.options.read.url = "/odata/kore/cms/Users";
            grid.dataSource.transport.options.read.type = "GET";
            delete grid.dataSource.transport.options.read.contentType;
            grid.dataSource.transport.parameterMap = function (options, operation) {
                return kendo.data.transports.odata.parameterMap(options, operation);
            };
        }
        else {
            //grid.dataSource.transport.options.read.url = "/odata/kore/cms/Users/GetUsersInRole";

            // For some reason, the OData query string doesn't get populated by Kendo Grid when we're using POST,
            // so we need to build it ourselves manually
            grid.dataSource.transport.options.read.url = function (data) {
                var params = {
                    page: grid.dataSource.page(),
                    sort: grid.dataSource.sort(),
                    filter: grid.dataSource.filter()
                };

                var queryString = "page=" + (params.page || "~");

                if (params.sort) {
                    queryString += "&$orderby=";
                    var isFirst = true;
                    $.each(params.sort, function () {
                        if (!isFirst) {
                            queryString += ",";
                        }
                        else {
                            isFirst = false;
                        }
                        queryString += this.field + " " + this.dir;
                    });
                }

                if (params.filter) {
                    queryString += "&$filter=";
                    var isFirst = true;
                    $.each(params.filter, function () {
                        if (!isFirst) {
                            queryString += " and ";
                        }
                        else {
                            isFirst = false;
                        }
                        queryString += this.field + " " + this.operator + " '" + this.value + "'";
                    });
                }

                return "/odata/kore/cms/Users/GetUsersInRole?" + queryString;
            };
            grid.dataSource.transport.options.read.type = "POST";
            grid.dataSource.transport.options.read.contentType = "application/json";
            grid.dataSource.transport.parameterMap = function (options, operation) {
                if (operation === "read") {
                    return kendo.stringify({
                        roleId: self.filterRoleId()
                    });
                }
            };
        }
        grid.dataSource.read();
        grid.refresh();
    }

    self.validator = $("#users-edit-form-section-form").validate({
        rules: {
            UserName: { required: true, maxlength: 128 },
            Email: { required: true, maxlength: 255 },
        },
        messages: {
            UserName: {
                required: "UserName is required.",
                maxlength: "UserName cannot consist of more than 128 characters."
            },
            Email: {
                required: "Email is required.",
                maxlength: "Email cannot consist of more than 255 characters."
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
                    url: "/odata/kore/cms/Users",
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
                        UserName: { type: "string" },
                        Email: { type: "string" },
                        IsLockedOut: { type: "boolean" }
                    }
                }
            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "UserName", dir: "asc" }
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
            field: "UserName",
            title: "User Name",
            filterable: true
        }, {
            field: "Email",
            title: "E-mail",
            filterable: true
        }, {
            field: "IsLockedOut",
            title: "Is Active",
            template: '<i class="fa #=!IsLockedOut ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
            attributes: { "class": "text-center" },
            filterable: true,
            width: 70
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.userModel.editRoles(\'#=Id#\')" class="btn btn-default btn-xs">Roles</a>' +
                '<a onclick="viewModel.userModel.changePassword(\'#=Id#\',\'#=UserName#\')" class="btn btn-default btn-xs">Password</a>' +
                '<a onclick="viewModel.userModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">Edit</a>' +
                '<a onclick="viewModel.userModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">Delete</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 230
        }]
    };
};