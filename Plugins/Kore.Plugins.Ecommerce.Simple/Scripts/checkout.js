'use strict'

var AddressModel = function () {
    var self = this;

    self.familyName = ko.observable(null);
    self.givenNames = ko.observable(null);
    self.email = ko.observable(null);
    self.addressLine1 = ko.observable(null);
    self.addressLine2 = ko.observable(null);
    self.addressLine3 = ko.observable(null);
    self.city = ko.observable(null);
    self.postalCode = ko.observable(null);
    self.countryId = ko.observable(0);
    self.phoneNumber = ko.observable(null);
};

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
    self.billingAddress = new AddressModel();
    self.shippingAddress = new AddressModel();
    self.shippingAddressIsSameAsBillingAddress = ko.observable(true);

    self.currentStep = ko.observable(1);
    self.maxSteps = 1;

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

    self.canShowPrevious = function () {
        if (self.currentStep() == 1) {
            return false;
        }
        return true;
    };

    self.canShowNext = function () {
        if (self.items().length == 0) {
            return false;
        }
        if (self.currentStep() == 1) {
            // we show "Checkout" button on first page instead (same thing as "Next")
            return false;
        }
        if (self.currentStep() == self.maxSteps) {
            return false;
        }
        return true;
    };

    self.previous = function () {
        var step = self.currentStep() - 1;
        var section = $('div[data-step=' + step + ']');
        switchSection(section);
        self.currentStep(step);
    };

    self.next = function () {
        if (self.currentStep() > 1) {
            // find form to validate, if any...
            var form = currentSection.find('form');
            if (form) {
                form.validate();
                if (!form.valid()) {
                    return;
                }
            }
        }

        var step = self.currentStep() + 1;
        var section = $('div[data-step=' + step + ']');
        switchSection(section);
        self.currentStep(step);
    };

    self.confirm = function () {
        var data = {};
        if (self.shippingAddressIsSameAsBillingAddress()) {
            data = {
                billingAddress: self.billingAddress,
                shippingAddress: self.billingAddress
            };
        }
        else {
            data = {
                billingAddress: self.billingAddress,
                shippingAddress: self.shippingAddress
            };
        }

        // first create order...
        $.ajax({
            url: "/store/checkout/create-order",
            type: "POST",
            dataType: "json",
            data: ko.toJSON(data),
            contentType: 'application/json; charset=utf-8',
            async: false
        })
        .done(function (json) {
            if (json.Success) {
                // ...then, go to PayPal...
                window.location.replace('/store/paypal/buy-now');
            }
            else {
                alert(json.Message);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
            console.log(textStatus + ': ' + errorThrown);
        });

    };

    self.billingAddressValidator = $("#billing-address-section-form").validate({
        rules: {
            Billing_FamilyName: { required: true, maxlength: 128 },
            Billing_GivenNames: { required: true, maxlength: 128 },
            Billing_Email: { required: true, maxlength: 255, email: true },
            Billing_AddressLine1: { required: true, maxlength: 128 },
            Billing_AddressLine2: { maxlength: 128 },
            Billing_AddressLine3: { maxlength: 128 },
            Billing_City: { required: true, maxlength: 128 },
            Billing_PostalCode: { required: true, maxlength: 10 },
            Billing_Country: { required: true, maxlength: 50 },
            Billing_PhoneNumber: { required: true, maxlength: 25 },
        }
    });

    self.shippingAddressValidator = $("#shipping-address-section-form").validate({
        rules: {
            Shipping_FamilyName: { required: true, maxlength: 128 },
            Shipping_GivenNames: { required: true, maxlength: 128 },
            Shipping_Email: { required: true, maxlength: 255, email: true },
            Shipping_AddressLine1: { required: true, maxlength: 128 },
            Shipping_AddressLine2: { maxlength: 128 },
            Shipping_AddressLine3: { maxlength: 128 },
            Shipping_City: { required: true, maxlength: 128 },
            Shipping_PostalCode: { required: true, maxlength: 10 },
            Shipping_Country: { required: true, maxlength: 50 },
            Shipping_PhoneNumber: { required: true, maxlength: 25 },
        }
    });
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();

    var max = 1;
    $('div[data-step]').each(function () {
        var step = parseInt($(this).data('step'));
        if (step > max) { max = step; }
    });
    viewModel.maxSteps = max;

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