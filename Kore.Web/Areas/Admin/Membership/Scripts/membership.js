'use strict'

var ViewModel = function () {
    var self = this;

    self.userModel = new UserVM();
    self.roleModel = new RoleVM();
    self.changePasswordModel = new ChangePasswordVM();

    self.viewUsers = function () {
        switchSection($("#users-grid-section"));
    };

    self.viewRoles = function () {
        switchSection($("#roles-grid-section"));
    };
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    $("#UsersGrid").kendoGrid(viewModel.userModel.gridConfig);
    $("#RolesGrid").kendoGrid(viewModel.roleModel.gridConfig);
});