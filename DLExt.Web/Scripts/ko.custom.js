ko.bindingHandlers.dialog = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $(element).dialog({
            modal: true,
            autoOpen: false,
            minHeight: 200,
            minWidth: 400,
            close: function () {
                viewModel.isOpen(false);
            }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var ctrlDown;
        var ctrlKey = 17, cKey = 67;
        var el = $(element);
        if (viewModel.isOpen()) {
            el.dialog("open");
            el.bind('keydown', function (e) {
                if (e.keyCode == ctrlKey)
                    ctrlDown = true;
                if (ctrlDown && e.keyCode == cKey && el.dialog("isOpen")) {
                    
                    // closing the dialog window initiated by timeout to allow a browser
                    // copy text to a clipboard
                    setTimeout(function () {
                        el.dialog("close");
                    }, 1);
                }
            }).bind('keyup', function () {
                ctrlDown = false;
            });
        } else {
            el.unbind();
        }
    }
};

ko.bindingHandlers.visibility = {
    update: function (element, valueAccessor) {
        $(element).css('visibility', ko.utils.unwrapObservable(valueAccessor()) ? 'visible' : 'hidden');
    }
};