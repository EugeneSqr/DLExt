ko.bindingHandlers.dialog = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $(element).dialog({
            modal: true,
            autoOpen: false,
            minHeight: 200,
            close: function () { viewModel.isOpen(false); }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        if (viewModel.isOpen())
            $(element).dialog("open");
    }
};