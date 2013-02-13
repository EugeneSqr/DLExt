ko.bindingHandlers.dialog = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $(element).dialog({
            modal: true,
            autoOpen: false,
            minHeight: 200,
            minWidth: 400,
            close: function () { viewModel.isOpen(false); }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        if (viewModel.isOpen())
            $(element).dialog("open");
    }
};

ko.bindingHandlers.visibility = {
    update: function (element, valueAccessor) {
        $(element).css('visibility', ko.utils.unwrapObservable(valueAccessor()) ? 'visible' : 'hidden');
    }
};