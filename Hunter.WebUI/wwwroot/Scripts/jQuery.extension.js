

(function ($, undefined) {
    window.tryParseFloat = function (num, _default) {
        try {
            var result = window.parseFloat(num)
            if (window.isNaN(result))
                return _default;
            return num;
        } catch (e) {
            return _default;
        }
    }

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
            var element = $element.get(0)
            if ($element.length == 0) {

            } else if ($element.is("input")) {
                var type = $element.attr("type")
                if (type == "checkbox") {
                    $element.each(function (index, element) {
                        element.checked = false
                        if ($.isArray(value) || $.isPlainObject(value)) {
                            for (var key in value) {
                                if (value[key] == element.value) {
                                    element.checked = true
                                    break;
                                }
                            }
                        } else {
                            if (value == element.value) {
                                element.checked = true
                            }
                        }
                    })
                } else if (type == "radio") {
                    $element.each(function (index, element) {
                        element.checked = false
                        if (value == element.value) {
                            element.checked = true
                        }
                    })
                } else {
                    $element.val(value)
                }
            } else if ($element.is("select")) {
                $element.val(value)
            } else if ($element.is("textarea")) {
                $element.val(value)
            }
        }
    }

    $(document).ajaxError(function (event, xhr, options, exc) {
        if (xhr.responseJSON && xhr.responseJSON.Message) {
            window.top.layer.msg.error(xhr.responseJSON.Message)
        }
    })
    $(document).ajaxSend(function (event, jqxhr, settings) {
        jqxhr.index = window.top.layer.load(0, { shade: [0.3, '#000'] })
    });
    $(document).ajaxComplete(function (event, xhr, settings) {
        window.top.layer.close(xhr.index)
    });

    // jquery Validate
    if (!$.Validate)
        $.Validate = {}
    $.Validate.DEFAULT = {
        tipPosition: 3,
        errorPlacement: function ($label, input) {
            console.log(this)
            var $input = $(input)
            var index = $input.attr('data-layer-index')
            window.top.layer.close(index)
            var msg = $label.text()
            if (msg)
                $input.attr('data-layer-index', window.top.layer.tips(msg, $input, { tipsMore: true, tips: [3, '#78BA32'] }))
        },
        success: function ($label, input) {
            var $input = $(input)
            var index = $input.attr('data-layer-index')
            window.top.layer.close(index)
        }
    }

    // bootstrap table
    $.BootstrapTable = {}
    $.BootstrapTable.DEFAULT = {
        url: '/Form/Query',                 //请求后台的URL（*）
        method: 'post',                     //请求方式（*）
        dataField: 'Data',                  //服务端返回数据键值 就是说记录放的键值是rows，分页时使用总记录数的键值为total
        totalField: 'Total',
        toolbar: '#toolbar',                //工具按钮用哪个容器
        striped: true,                      //是否显示行间隔色
        cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                   //是否显示分页（*）
        sortable: true,                     //是否启用排序
        sortName: 'ID',
        sortOrder: "desc",                  //排序方式
        sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                      //初始化加载第一页，默认第一页
        pageSize: 10,                       //每页的记录行数（*）
        pageList: [10, 20, 30, 40, 50],     //可供选择的每页的行数（*）
        search: false,                      //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
        strictSearch: true,
        showColumns: true,                  //是否显示所有的列
        showRefresh: true,                  //是否显示刷新按钮
        minimumCountColumns: 2,             //最少允许的列数
        clickToSelect: true,                //是否启用点击选中行
        singleSelect: true,                 //设为true则允许复选框仅选择一行
        height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
        showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
        cardView: false,                    //是否显示详细视图
        detailView: false,                  //是否显示父子表
        searchText: 'GetSearchCondition',
        locale: 'zh-CN',
        queryParams: function (params) { //传递参数（*）
            var condition = window[this.searchText];
            if (condition && $.isFunction(condition))
                condition = condition()
            var temp = {
                Size: params.limit,   //页面大小
                Index: Math.ceil(params.offset / params.limit) + 1,  //页码
                Condition: condition,
                Sort: {
                    Field: this.sortName,
                    Order: this.sortOrder == 'asc' ? 0 : 1
                }
            }
            return temp;
        },
        onSort: function (name, sort) {

        }
    }


    $(function () {
        if (!window.layer)
            return
        layer.msg.warning = function (msg) {
            return layer.msg(msg, { icon: 0 })
        }
        layer.msg.success = function (msg) {
            return layer.msg(msg, { icon: 1 })
        }
        layer.msg.error = function (msg) {
            return layer.msg(msg, { icon: 2 })
        }
    })
})(jQuery);

