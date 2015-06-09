'use strict'

var CartItemModel = function () {
    var self = this;

    self.productId = ko.observable(0);
    self.productName = ko.observable(null);
    self.imageUrl = ko.observable('http://placehold.it/100x70');
    self.description = ko.observable(null);
    self.quantity = ko.observable(0);
    self.price = ko.observable(0);
    self.tax = ko.observable(0);
    self.shippingCost = ko.observable(0);
};

var ViewModel = function () {
    var self = this;
    self.items = ko.observableArray([]);

    self.subTotal = ko.computed(function () {
        var total = 0;
        for (var i = 0; i < self.items().length; i++) {
            var item = self.items()[i];
            total += (item.price() * item.quantity());
        }
        return total.toFixed(2);
    });

    self.shippingTotal = ko.computed(function () {
        var total = 0;
        for (var i = 0; i < self.items().length; i++) {
            var item = self.items()[i];
            total += (item.shippingCost() * item.quantity());
        }
        total += shippingFlatRate;
        return total.toFixed(2);
    });

    self.taxTotal = ko.computed(function () {
        var total = 0;
        for (var i = 0; i < self.items().length; i++) {
            var item = self.items()[i];
            total += (item.tax() * item.quantity());
        }
        return total.toFixed(2);
    });

    self.totalPrice = ko.computed(function () {
        var total = 0;
        for (var i = 0; i < self.items().length; i++) {
            var item = self.items()[i];
            total += ((item.price() + item.tax() + item.shippingCost()) * item.quantity());
        }
        total += shippingFlatRate;
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
            data: ko.toJSON({ Items: self.items }),
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

            if (item.ImageUrl) {
                cartItem.imageUrl(item.ImageUrl);
            }

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