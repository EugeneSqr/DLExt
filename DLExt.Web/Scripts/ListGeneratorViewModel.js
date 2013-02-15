function ListGeneratorViewModel() {
    var self = this;

    self.maxMailToLength = 2083;
    self.mailToText = "mailto:";
    self.keyCodeDelete = 46;

    self.persons = [];
    self.filteredPersons = ko.observableArray();
    self.filteredPersonsCount = ko.computed(function () {
        return self.filteredPersons().length;
    });

    self.excludedPersons = ko.observableArray();
    self.personsToExclude = ko.observableArray();
    self.personsToInclude = ko.observableArray();

    self.scrollTop = ko.observable(false);
    self.locations = ko.observableArray();
    self.checkedLocations = ko.observableArray();
    self.checkedLocationCount = ko.observable(0);

    self.checkedLocations.subscribe(function (newLocations) {
        self.errorNoPersonsSelected(false);
        self.excludedPersons.remove(function (person) {
            return newLocations.indexOf(person.location) == -1;
        });
        self.filterByLocation(newLocations);

        if (self.checkedLocationCount() > newLocations.length)
            self.scrollTop(true);
        self.checkedLocationCount(newLocations.length);
    });

    self.locationsLoading = ko.observable(true);
    self.locationsLoadingCompleted = ko.computed(function () {
        return !self.locationsLoading();
    });
    self.personsLoading = ko.observable(true);
    self.personsLoadingCompleted = ko.computed(function () {
        return !self.personsLoading();
    });

    self.dialogWindowAddressList = ko.observable();

    self.personsExcluded = ko.observable(false);
    self.errorNoPersonsSelected = ko.observable(false);
    self.isOpen = ko.observable(false);

    self.isMailToPossible = ko.observable(false);
    self.mailToAddressList = ko.observable();

    self.filterByLocation = function (locations) {
        var filteredPersons = [];
        for (i in locations) {
            var location = locations[i];
            for (j in self.persons) {
                var person = self.persons[j];
                if (person.location == location) {
                    if (!self.containsPerson(self.excludedPersons(), person)) {
                        filteredPersons.push(person);
                    }
                }
            }
        }

        self.filteredPersons(filteredPersons);
    };

    self.excludePersons = function () {
        var personsToExclude = self.personsToExclude();
        for (i in personsToExclude) {
            var person = personsToExclude[i];
            if (!self.containsPerson(self.excludedPersons(), person)) {
                self.excludedPersons.push(person);
                self.filteredPersons.remove(function (item) {
                    return item.id == person.id;
                });
                self.personsExcluded(true);
            }
        }
        self.clearSelectedPersons();
    };

    self.excludePersonsByKeypress = function (data, event) {
        if (event.keyCode == self.keyCodeDelete) {
            self.excludePersons();
        }
    };

    self.includePersons = function () {
        var personsToInclude = self.personsToInclude();
        for (i in personsToInclude) {
            var person = personsToInclude[i];
            self.excludedPersons.remove(function (item) {
                return item.id == person.id;
            });
            if (self.excludedPersons().length == 0)
                self.personsExcluded(false);

            self.filterByLocation(self.checkedLocations());
        }
        self.clearSelectedPersons();
    };

    self.includePersonsByKeypress = function (data, event) {
        if (event.keyCode == self.keyCodeDelete) {
            self.includePersons();
        }
    };

    // preventing selection side-effects after manipulations with person lists
    self.clearSelectedPersons = function () {
        self.personsToExclude([]);
        self.personsToInclude([]);
    };

    self.containsPerson = function (collection, item) {
        for (index in collection) {
            var collectionItem = collection[index];
            if (collectionItem.id == item.id) {
                return true;
            }
        }

        return false;
    };

    self.generateList = function () {
        var filteredPersons = self.filteredPersons();
        if (self.filteredPersons().length == 0) {
            self.errorNoPersonsSelected(true);
            return;
        }
        self.errorNoPersonsSelected(false);

        var address = "";
        for (index in filteredPersons) {
            var person = filteredPersons[index];
            address = person.email + ';' + address;
        }
        self.isMailToPossible(self.mailToText.length + address.length > self.maxMailToLength);
        self.mailToAddressList(self.mailToText + address);
        self.dialogWindowAddressList(address);
        self.isOpen(true);
    };

    $.ajax({
        url: 'http://localhost:8888/DistributionList.svc/rest/GetLocations',
        type: 'GET',
        dataType: 'jsonp',
        success: function (data) {
            self.locations(data);
            self.locationsLoading(false);
        },
        error: function (data, textStatus, errorThrown) {
            console.log("Error getting locations");
            console.log(data);
            console.log(textStatus);
            console.log(errorThrown);
        }
    });

    $.ajax({
        url: 'http://localhost:8888/DistributionList.svc/rest/GetPersons',
        type: 'GET',
        dataType: 'jsonp',
        success: function (data) {
            for (index in data) {
                self.persons.push(data[index]);
            }

            self.personsLoading(false);
        },
        error: function (data, textStatus, errorThrown) {
            console.log("Error getting persons");
            console.log(data);
            console.log(textStatus);
            console.log(errorThrown);
        }
    });
}