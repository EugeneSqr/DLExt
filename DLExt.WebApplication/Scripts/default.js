var excludedPersonIndexes = [];
var persons = [];
var locations = [];

$(document).ready(function () {

    $('#clearButton').click(function () {
        excludedPersonIndexes = [];

        onPropertyChanged();
    });

    $('#excludeButton').click(function () {
        var personId = parseInt($('#personsDropDownList').val());
        if ($.inArray(personId, excludedPersonIndexes) >= 0)
            return;
        excludedPersonIndexes.push(personId);

        onPropertyChanged();
    });

    $('#sendLetter').click(function () {
        return ($(this).attr('disabled')) ? false : true;
    });

    $.ajax({
        url: 'http://dlext.ru/Service.svc/GetGroupedPersons',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            buildPersonsList(data);

            $(locations).each(function () {
                var checkbox = document.createElement("input");
                checkbox.id = 'checkBoxLocation' + this.Name;
                checkbox.type = "checkbox";
                checkbox.name = this.Name;
                $(checkbox).change(function (e) {
                    var location = getLocationByName(checkbox.name);
                    location.IsSelected = e.target.checked;
                    updatePersonsList();
                    onPropertyChanged();
                });

                $(checkbox).appendTo($('#checkBoxListLocations'));
                $('#checkBoxListLocations').append(this.Name + '<br/>');
            });
            var dropDownList = $('#personsDropDownList');
            $(persons).each(function () {
                var option = document.createElement('option');
                option.value = this.Index;
                option.appendChild(document.createTextNode(this.Name));
                dropDownList.append(option);
            });

            update();
        }
    });
});

function buildPersonsList(rawData) {
    var index = 0;
    persons = [];
    $(rawData).each(function (ind1, keyValuePair) {
        var location = keyValuePair.Key.Name;
        locations.push({
            "Name": location,
            "IsSelected": false
        });
        $(keyValuePair.Value).each(function (ind2, person) {
            persons.push({
                "Index": index,
                "Name": person.DisplayName,
                "Email": person.Email,
                "Location": location,
                "IsSelected": false
            });
            index++;
        });
    });
}

function onPropertyChanged() {
    var email = 'mailto:';

    var personsInList = [];
    $(persons).each(function () {
        if (getLocationByName(this.Location).IsSelected) {
            if ($.inArray(this.Index, excludedPersonIndexes) == -1)
                personsInList.push(this);
        }
    });
    $('#personsCountLabel').html(personsInList.length);

    $(personsInList).each(function () {
        email += this.Email + '; ';
    });

    if (personsInList.length == 0) {
        $('#sendLetter').attr('disabled', true);
        $('#sendLetter').attr('href', '');
    } else {
        $('#sendLetter').attr('disabled', false);
        $('#sendLetter').attr('href', email);
    }

    var personList = $('#excludedPersons');
    personList.empty();
    $(excludedPersonIndexes).each(function (i, value) {
        var person = getPersonByIndex(value);
        var div = $('<div></div>');
        div.attr('id', 'personSelector' + i);
        div.append($('<label>' + person.Name + '</label>'));

        var hyperlink = document.createElement("a");
        hyperlink.href = '';
        hyperlink.textContent = 'удалить';
        $(hyperlink).click(function (e) {
            e.preventDefault();
            excludedPersonIndexes = $.grep(excludedPersonIndexes, function (value1) {
                return value1 != value;
            });
            $('#personSelector' + i).remove();
            
            onPropertyChanged();
        });
        div.append($(hyperlink));
        div.append($('<br/>'));

        div.appendTo(personList);

    });
}

function update() {
    var count = 0;
    $(persons).each(function () {
        if (this.IsSelected)
            count++;
    });
    $('#personsCountLabel').html(count);
}

function updatePersonsList() {
    var dropDownList = $('#personsDropDownList');
    dropDownList.empty();
    $(persons).each(function () {
        if (getLocationByName(this.Location).IsSelected) {
            var option = document.createElement('option');
            option.value = this.Index;
            option.appendChild(document.createTextNode(this.Name));
            dropDownList.append(option);
        }
    });
}

function getLocationByName(name) {
    var result = $.grep(locations, function (item) {
        return item.Name == name;
    });
    if (result.length != 1)
        alert("can't find an element");
    return result[0];
}

function getPersonByIndex(index) {
    var result = $.grep(persons, function (item) {
        return item.Index == index;
    });
    if (result.length != 1)
        alert("can't find an element");
    return result[0];
}

function getLocationPersonsCount(locationName) {
    var count = 0;
    $(persons).each(function () {
        if (this.Location == locationName)
            count++;
    });
    return count;
}

