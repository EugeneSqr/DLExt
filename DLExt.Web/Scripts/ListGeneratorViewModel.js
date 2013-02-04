function Person(data) {
    var self = this;

    self.id = data.Id;
    self.name = data.Name;
    self.email = data.Email;
    self.location = data.Location;
}

function ListGeneratorViewModel() {
    var self = this;

    self.persons = [];
    self.filteredPersons = ko.observableArray();
    self.excludedPersons = ko.observableArray();
    self.personToExclude = ko.observable();
    self.personToInclude = ko.observable();

    self.locations = ko.observableArray();
    self.checkedLocations = ko.observableArray();
    self.checkedLocations.subscribe(function (newLocations) {
        self.filterByLocation(newLocations);
    });

    self.locationsLoading = ko.observable(true);
    self.locationsLoadingCompleted = ko.computed(function () {
        return !self.locationsLoading();
    });
    self.personsLoading = ko.observable(true);
    self.personsLoadingCompleted = ko.computed(function () {
        return !self.personsLoading();
    });

    self.addressList = ko.observable();
    self.addressListBuilt = ko.observable(false);

    self.filterByLocation = function (locations) {
        self.filteredPersons.removeAll();
        for (i in locations) {
            var location = locations[i];
            for (j in self.persons) {
                var person = self.persons[j];
                if (person.location == location) {
                    if (!self.containsPerson(self.excludedPersons(), person)) {
                        self.filteredPersons.push(person);
                    }
                }
            }
        }
    };

    self.excludePerson = function () {
        var personToExclude = self.personToExclude();
        if (typeof personToExclude != 'undefined') {
            if (!self.containsPerson(self.excludedPersons(), self.personToExclude())) {
                self.excludedPersons.push(personToExclude);
                self.filteredPersons.remove(function(item) {
                    return item.id == personToExclude.id;
                });
            }
        }
    };

    self.includePerson = function () {
        var personToInclude = self.personToInclude();
        if (typeof personToInclude != 'undefined') {
            self.excludedPersons.remove(function (item) {
                return item.id == personToInclude.id;
            });

            self.filterByLocation(self.checkedLocations());
        }
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

    self.createAddress = function () {
        self.addressListBuilt(false);
        var filteredPersons = self.filteredPersons();
        var address = "";
        for (index in filteredPersons) {
            var person = filteredPersons[index];
            address = person.email + ';' + address;
        }
        self.addressList("mailto:" + address);
        self.addressListBuilt(true);
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
                self.persons.push(new Person(data[index]));
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