function onPageLoad() {
    doAjaxCall('http://dlext.ru/Service.svc/GetLocations', 'GET', function (json) {
        fillLocationsList(json);
    });
    doAjaxCall('http://dlext.ru/Service.svc/GetAllPersons', 'GET', function (json) {
        fillPersonsDropDownList(json);
    });
}

function doAjaxCall(url, method, onSuccess) {
    var xmlhttp;
    if (window.XMLHttpRequest) { // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest;
    } else { // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var response = xmlhttp.response;
            var json = JSON.parse(response);
            onSuccess(json);
        } else {
            if (xmlhttp.readyState == 4) alert('error');
        }
    };
    xmlhttp.open(method, url, true);
    xmlhttp.send();
}

function doAjaxCallPost(url, method, params, onSuccess) {
    var xmlhttp;
    if (window.XMLHttpRequest) { // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest;
    } else { // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var response = xmlhttp.response;
            var json = JSON.parse(response);
            onSuccess(json);
        } else {
            if (xmlhttp.readyState == 4) alert('error');
        }
    };
    xmlhttp.open(method, url, true);
    xmlhttp.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
    xmlhttp.send(params);
}

function fillLocationsList(locations) {
    var div = document.getElementById("checkBoxListLocations");
    for (var i = 0; i < locations.length; i++) {
        var checkbox = document.createElement('input');
        checkbox.id = "checkBoxId" + i;
        checkbox.type = "checkbox";
        checkbox.name = locations[i].Name;
        checkbox.value = JSON.stringify(locations[i]);
        checkbox.onchange = onCheckBoxChange;
        //checkbox.addEventListener('onchange', function () { getJsonSelectedLocation(); }, false);

        var label = document.createElement('label');
        label.htmlFor = checkbox.id;
        label.appendChild(document.createTextNode(locations[i].Name));
        label.appendChild(document.createElement('br'));

        div.appendChild(checkbox);
        div.appendChild(label);
    }
}

function fillPersonsDropDownList(persons) {
    var dropDownList = document.getElementById("personsDropDownList");
    for (var i = 0; i < persons.length; i++) {
        var option = document.createElement('option');
        option.value = persons[i].Email;

        option.appendChild(document.createTextNode(persons[i].DisplayName));
        dropDownList.appendChild(option);
    }
}

function onCheckBoxChange() {
    var request = encodeURIComponent(buildFilterRequestString(getJsonSelectedLocation()));
//    doAjaxCall('http://dlext.ru/Service.svc/GetPersons?Request=' + request, 'GET', function(json) {
//        fillPersonsDropDownList(json);
    doAjaxCallPost('http://dlext.ru/Service.svc/GetPersonsPost', 'POST', request, function (json) {
        fillPersonsDropDownList(json);
    });
}

//function getJsonSelectedLocation() {
//    var div = document.getElementById("checkBoxListLocations");
//    var childs = div.childNodes;
//    var selectedLocations = [];
//    for (var i = 0; i < childs.length; i++) {
//        if (childs[i].type == "checkbox") {
//            if (childs[i].checked)
//                selectedLocations.push(childs[i].value);
//        }
//    }
//    return selectedLocations;
//}

function buildFilterRequestString(locations, persons) {
    var filterRequest = {};
    if (locations == undefined)
        locations = [];
    if (persons == undefined)
        persons = [];
    filterRequest.ExcludedPersons = persons;
    filterRequest.Locations = locations;
    return JSON.stringify(filterRequest);
}
