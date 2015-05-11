'use strict'

var PermissionVM = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.name = ko.observable('');
    self.category = ko.observable('');
    self.description = ko.observable('');

    self.create = function () {
        self.id(emptyGuid);
        self.name('');
        self.category('');
        self.description('');

        self.validator.resetForm();
        switchSection($("#permissions-form-section"));
        $("#permissions-form-section-legend").html("Create New");
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/PermissionApi('" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.category(json.Category);
            self.description(json.Description);

            self.validator.resetForm();
            switchSection($("#permissions-form-section"));
            $("#permissions-form-section-legend").html("Edit");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify("There was an error when retrieving the record.", "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm("Are you sure you want to delete this record?")) {
            $.ajax({
                url: "/odata/kore/cms/PermissionApi('" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#PermissionsGrid').data('kendoGrid').dataSource.read();
                $('#PermissionsGrid').data('kendoGrid').refresh();

                $.notify("Successfully deleted record.", "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify("There was an error when deleting the record.", "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.save = function () {

        if (!$("#permissions-form-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            Name: self.name(),
            Category: self.category(),
            Description: seld.description()
        };

        if (self.id() == emptyGuid) {
            // INSERT
            $.ajax({
                url: "/odata/kore/cms/PermissionApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#PermissionsGrid').data('kendoGrid').dataSource.read();
                $('#PermissionsGrid').data('kendoGrid').refresh();

                switchSection($("#permissions-grid-section"));

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
                url: "/odata/kore/cms/PermissionApi('" + self.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#PermissionsGrid').data('kendoGrid').dataSource.read();
                $('#PermissionsGrid').data('kendoGrid').refresh();

                switchSection($("#permissions-grid-section"));

                $.notify("Successfully updated record.", "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify("There was an error when updating the record.", "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#permissions-grid-section"));
    };

    self.validator = $("#permissions-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 },
            Category: { maxlength: 255 }
        },
        messages: {
            Name: {
                required: "Name is required.",
                maxlength: "Name cannot consist of more than 255 characters."
            },
            Category: {
                maxlength: "Category cannot consist of more than 255 characters."
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
                    url: "/odata/kore/cms/PermissionApi",
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
                        Name: { type: "string" },
                        Category: { type: "string" }
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
            field: "Category",
            title: "Category",
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.permissionModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">Edit</a>' +
                '<a onclick="viewModel.permissionModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">Delete</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 170
        }]
    };
};