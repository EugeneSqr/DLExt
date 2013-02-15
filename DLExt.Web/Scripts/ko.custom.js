ko.bindingHandlers.dialog = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $(element).dialog({
            modal: true,
            resizable: false,
            autoOpen: false,
            minHeight: 350,
            minWidth: 500,
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
            el.find('textarea').focus().select();
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

ko.bindingHandlers.autoScroll = {
    update: function (element, valueAccessor, allBindingAccessor, viewModel) {
        if (ko.utils.unwrapObservable(valueAccessor())) {
            $(element).scrollTop(0);
            viewModel.scrollTop(false);
        }
    }
}