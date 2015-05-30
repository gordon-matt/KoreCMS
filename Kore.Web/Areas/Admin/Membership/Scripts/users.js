'use strict'

var ChangePasswordVM = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.userName = ko.observable(null);
    self.password = ko.observable(null);
    self.confirmPassword = ko.observable(null);

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
            url: "/odata/kore/web/UserApi/ChangePassword",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(record),
            async: false
        })
        .done(function (json) {
            switchSection($("#users-grid-section"));
            $.notify(translations.ChangePasswordSuccess, "success");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.ChangePasswordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.validator = $("#change-password-form-section-form").validate({
        rules: {
            Change_Password: { required: true, maxlength: 128 },
            Change_ConfirmPassword: { required: true, maxlength: 128, equalTo: "#Change_Password" },
        }
    });
};

var UserVM = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.userName = ko.observable(null);
    self.email = ko.observable(null);
    self.isLockedOut = ko.observable(false);

    self.roles = ko.observableArray([]);
    self.filterRoleId = ko.observable(emptyGuid);

    self.create = function () {
        self.id(emptyGuid);
        self.userName(null);
        self.email(null);
        self.isLockedOut(false);

        self.roles([]);
        self.filterRoleId(emptyGuid);

        self.validator.resetForm();
        switchSection($("#users-edit-form-section"));
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/web/UserApi('" + id + "')",
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
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/web/UserApi('" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#UsersGrid').data('kendoGrid').dataSource.read();
                $('#UsersGrid').data('kendoGrid').refresh();

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
                url: "/odata/kore/web/UserApi",
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
                url: "/odata/kore/web/UserApi('" + self.id() + "')",
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

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
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
            url: "/odata/kore/web/RoleApi/GetRolesForUser",
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
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
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
            url: "/odata/kore/web/UserApi/AssignUserToRoles",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            async: false
        })
        .done(function (json) {
            switchSection($("#users-grid-section"));
            $.notify(translations.SaveRolesSuccess, "success");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.SaveRolesError, "error");
            console.log(textStatus + ': ' + errorThrown);
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
            grid.dataSource.transport.options.read.url = "/odata/kore/web/UserApi";
            grid.dataSource.transport.options.read.type = "GET";
            delete grid.dataSource.transport.options.read.contentType;
            grid.dataSource.transport.parameterMap = function (options, operation) {
                return kendo.data.transports.odata.parameterMap(options, operation);
            };
        }
        else {
            //grid.dataSource.transport.options.read.url = "/odata/kore/web/UserApi/GetUsersInRole";

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

                return "/odata/kore/web/UserApi/GetUsersInRole?" + queryString;
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
        }
    });

    self.gridConfig = {
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/web/UserApi",
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
            pageSize: gridPageSize,
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
            title: translations.Columns.User.UserName,
            filterable: true
        }, {
            field: "Email",
            title: translations.Columns.User.Email,
            filterable: true
        }, {
            field: "IsLockedOut",
            title: translations.Columns.User.IsActive,
            template: '<i class="fa #=!IsLockedOut ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
            attributes: { "class": "text-center" },
            filterable: true,
            width: 70
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.userModel.editRoles(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Roles + '</a>' +
                '<a onclick="viewModel.userModel.changePassword(\'#=Id#\',\'#=UserName#\')" class="btn btn-default btn-xs">' + translations.Password + '</a>' +
                '<a onclick="viewModel.userModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.userModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 230
        }]
    };
};