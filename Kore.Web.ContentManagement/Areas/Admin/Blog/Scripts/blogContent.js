'use strict'

var currentSection = $("#main-section");

var localStorageRootKey = "BlogContent_";
var localStorageUsersKey = localStorageRootKey + "Users_";

function switchSection(section) {
    if (section.attr("id") == currentSection.attr("id")) {
        return;
    }
    currentSection.hide("fast");
    section.show("fast");
    currentSection = section;
};

var BlogEntryModel = function () {
    var self = this;

    self.headline = ko.observable('');
    self.userId = ko.observable('');
    self.userName = ko.observable('');
    self.dateCreated = ko.observable('');
    self.shortDescription = ko.observable('');
    self.fullDescription = ko.observable('');

    self.showDetails = function () {
        viewModel.selected(self);
        switchSection($('#details-section'));
    };
};

var ViewModel = function () {
    var self = this;
    self.entries = ko.observableArray([]);
    self.selected = ko.observable(new BlogEntryModel());

    self.total = ko.observable(0);
    self.pageIndex = ko.observable(1);
    self.pageSize = 5;

    self.pageCount = function () {
        return Math.ceil(self.total() / self.pageSize);
    };

    self.showMain = function () {
        switchSection($('#main-section'));
    };
};

breeze.config.initializeAdapterInstances({ dataService: "OData" });
var manager = new breeze.EntityManager('/odata/kore/cms');

var pagerInitialized = false;
var userIds = [];

var viewModel;
$(document).ready(function () {
    //TODO: clear local storage from before... for this page only..
    // See: http://stackoverflow.com/questions/8419354/get-html5-localstorage-keys

    viewModel = new ViewModel();
    getBlogs();
    ko.applyBindings(viewModel);
});

function getBlogs() {
    userIds = [];

    var query = new breeze.EntityQuery()
        .from("Blogs")
        .orderBy("DateCreated desc")
        .skip((viewModel.pageIndex() - 1) * viewModel.pageSize)
        .take(viewModel.pageSize)
        .inlineCount();

    manager.executeQuery(query).then(function (data) {
        viewModel.entries([]);
        viewModel.selected(new BlogEntryModel());
        viewModel.total(data.inlineCount);

        $(data.httpResponse.data.results).each(function () {
            var current = this;
            var entry = new BlogEntryModel();
            entry.headline(current.Headline);
            entry.userId(current.UserId);
            entry.dateCreated(current.DateCreated);
            entry.shortDescription(current.ShortDescription);
            entry.fullDescription(current.FullDescription);
            viewModel.entries.push(entry);
            userIds.push(current.UserId);
        });

        if (!pagerInitialized) {
            $('#pager').bootpag({
                total: viewModel.pageCount(),
                page: viewModel.pageIndex(),
                maxVisible: 5,
                leaps: true,
                firstLastUse: true,
            }).on("page", function (event, num) {
                viewModel.pageIndex(num);
                getBlogs();
            });
            pagerInitialized = true;
        }
        getUserNames();
    }).fail(function (e) {
        alert(e);
    });
};

function getUserNames() {
    var query = new breeze.EntityQuery().from("PublicUsers");

    var predicate = null;
    $(userIds).each(function (index, userId) {
        if (!localStorage.getItem(localStorageUsersKey + userId)) {
            if (predicate == null) {
                predicate = breeze.Predicate.create('UserId', '==', userId);
            }
            else {
                predicate = predicate.or(breeze.Predicate.create('UserId', '==', userId));
            }
        }
    });

    query = query
        .where(predicate)
        .select("Id, UserName");

    manager.executeQuery(query).then(function (data) {
        $(data.httpResponse.data.results).each(function (index, item) {
            localStorage.setItem(localStorageUsersKey + item.Id, item.UserName);
        });

        $(viewModel.entries()).each(function (index, item) {
            item.userName(localStorage.getItem(localStorageUsersKey + item.userId()));
        });
    }).fail(function (e) {
        alert(e);
    });
};