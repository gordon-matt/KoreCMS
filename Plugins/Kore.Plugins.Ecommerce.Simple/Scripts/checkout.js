'use strict'

var currentSection = $("#main-section");

function switchSection(section) {
    if (section.attr("id") == currentSection.attr("id")) {
        return;
    }
    currentSection.hide("fast");
    section.show("fast");
    currentSection = section;
};

var CartItemModel = function () {
    var self = this;

    self.productId = ko.observable(0);
    self.productName = ko.observable('');
    self.imageUrl = ko.observable('');
    self.description = ko.observable('');
    self.quantity = ko.observable(0);
    self.price = ko.observable(0);
    self.tax = ko.observable(0);
    self.shippingCost = ko.observable(0);
};

var ViewModel = function () {
    var self = this;
    self.items = ko.observableArray([]);

    self.totalPrice = ko.computed(function () {
        var total = 0;
        for (var i = 0; i < self.items().length; i++) {
            var item = self.items()[i];
            total += ((item.price() + item.tax() + item.shippingCost()) * item.quantity());
        }
        return total.toFixed(2);
    });

    self.removeItem = function (item) {
        self.items.remove(item);
    };

    self.updateCart = function () {
        $.ajax({
            url: '/store/cart/update-cart',
            type: 'POST',
            dataType: 'json',
            data: ko.toJSON(self.items),
            contentType: 'application/json; charset=utf-8',
            async: false
        })
        .done(function (json) {
            alert(json.Message);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
            console.log(textStatus + ': ' + errorThrown);
        });
    };
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    //if (localStorage) {
    //}
    //else {
    //}

    $.ajax({
        url: "/store/cart/get-cart",
        type: "GET",
        dataType: "json",
        async: false
    })
    .done(function (json) {
        $(json.Items).each(function (index, item) {
            var cartItem = new CartItemModel();
            cartItem.productId(item.ProductId);
            cartItem.productName(item.ProductName);
            cartItem.imageUrl(item.ImageUrl);
            cartItem.description(item.Description);
            cartItem.quantity(item.Quantity);
            cartItem.price(item.Price);
            cartItem.tax(item.Tax);
            cartItem.shippingCost(item.ShippingCost);
            viewModel.items.push(cartItem);
        });
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert(errorThrown);
        console.log(textStatus + ': ' + errorThrown);
    });
});