(function ($, undefined) {
    $.fn.serializeData = function () {
        var obj = {}
        this.find('input').each(function (index, element) {
            var $element = $(element);
            var name = $element.attr('name');
            if (!name) {
                return;
            }
            var type = $element.attr('type');
            var value = obj[name];
            if (type == 'checkbox') {
                value = value ? value : []
                if (element.checked)
                    value.push($element.val())
            } else if (type == 'radio') {
                if (element.checked)
                    value = $element.val()
            } else {
                value = $element.val()
            }
            obj[name] = value === undefined ? null : value;
        })
        this.find('select').each(function (index, element) {
            var $element = $(element);
            var name = $element.attr('name');
            if (!name) {
                return;
            }
            obj[name] = $element.val();
        })
        this.find('textarea').each(function (index, element) {
            var $element = $(element);
            var name = $element.attr('name');
            if (!name) {
                return;
            }
            obj[name] = $element.val();
        })
        return obj
    }

    $.fn.fillData = function (data) {
        for (var name in data) {
            var value = data[name];
            var $element = this.find("[name=" + name + "]");
            if ($element.length == 0) {

            } else if ($element.is("input")) {
                var type = $element.attr("type")
                if (type == "checkbox") {

                } else if (type == "radio") {

                } else {
                    $element.val(value)
                }
            } else if ($element.is("select")) {

            } else if ($element.is("textarea")) {
                $element.val(value)
            }
        }
    }
})(jQuery)