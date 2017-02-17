//Clears validation error messages
$.fn.clearErrors = function () {
    $(this).each(function () {
        $(this).find(".field-validation-error").empty();
        $(this).trigger('reset.unobtrusiveValidation');
    });
};


var ArbitrarySearcher = function () {
    var my = {};
    var all_targets = null;
    var all_elements = null;
    my.all = function () {
        return all_elements;
    }
    // defaults for the conf.targets objects
    var target_defaults = {
        target: null,
        elements: function ($target) { return $target.find('> *'); },
        searchtext: function ($ele) {
            var text = $ele.text()
            var searchdata = $ele.attr("data-search")
            if (searchdata)
                return text + " " + searchdata
            else
                return text
        },
        style: function ($ele, included) {
            if (included) {
                $ele.addClass("included").removeClass("excluded")
            } else {
                $ele.addClass("excluded").removeClass("included")
            }
        },
        clear: function ($ele) {
            $ele.removeClass("excluded").removeClass("included")
        }
    }

    // initialize the module
    my.init = function (conf) {
        init_process_targets(conf.targets)
        //collapsibles = init_process_collapsibles();
        // debounced auto search
        conf.input && conf.input.keyup(_.debounce(
            function () {
                my.search(this.value);
            },
            200
            ));
    }

    // process the conf.targets argument to init.
    // initalizes all_targets, all_elements
    function init_process_targets(targets) {
        all_targets = [];
        all_elements = [];
        var TDEF = target_defaults;
        _.each(targets, function (obj) {
            var res = {};
            all_targets.push(res);
            res.target = obj.target;
            var elements = obj.elements ? obj.elements(res.target) :
                                            TDEF.elements(res.target);
            res.elements = _.map(elements, function ($ele) {
                $ele = $($ele);
                return {
                    // jquery element
                    $ele: $ele,
                    // raw string
                    searchtext: (obj.searchtext ? obj.searchtext($ele) :
                                                    TDEF.searchtext($ele)).toLowerCase(),
                    data: obj.process && obj.process($ele),
                    before: obj.before,
                    after: obj.after,
                    included: obj.included,
                    excluded: obj.excluded,
                    style: obj.style || TDEF.style,
                    clear: obj.clear || TDEF.clear
                }
            });
            Array.prototype.push.apply(all_elements, res.elements);
        });
    }

    // process the search_targets to collect their unique collapsibles
    function init_process_collapsibles() {
        var res = [];
        // map elements to collapsibles and add to res
        _.each(search_targets, function (target) {
            Array.prototype.push.apply(res,
                _.map(target.elements, function (obj) {
                    return {
                        $collapsible: obj.$collapsible,
                        $collapsible_parent: obj.$collapsible_parent,
                        $collapsible_parent_title: obj.$collapsible_parent_title
                    }
                })
            )
        })
    }
    // filter helper
    function filter_function(obj) /*this object is the passed value*/ {
        return obj.searchtext.indexOf(this) > -1
    }
    // search helper
    function filter(elements_list, text) {
        return elements_list.filter(filter_function, text.toLowerCase())
    }

    my.clear = function () {
        _.each(all_elements, function (obj) {
            obj.before && obj.before(obj.$ele, obj.data);
        })
        _.each(all_elements, function (obj) {
            obj.clear && obj.clear(obj.$ele, obj.data);
        })
        _.each(all_elements, function (obj) {
            obj.after && obj.after(obj.$ele, obj.data);
        })
    }

    my.search = function (text) {
        if (!text) {
            my.clear()
            return
        }
        var yes = filter(all_elements, text)
        var no = _.difference(all_elements, yes)

        // before
        _.each(yes, function (obj) {
            obj.before && obj.before(obj.$ele, obj.data)
        })
        _.each(no, function (obj) {
            obj.before && obj.before(obj.$ele, obj.data)
        })

        // main
        _.each(yes, function (obj) {
            obj.included && obj.included(obj.$ele, obj.data)
            obj.style && obj.style(obj.$ele, true)
        })
        _.each(no, function (obj) {
            obj.excluded && obj.excluded(obj.$ele, obj.data)
            obj.style && obj.style(obj.$ele, false)
        })

        // after
        _.each(yes, function (obj) {
            obj.after && obj.after(obj.$ele, obj.data)
        })
        _.each(no, function (obj) {
            obj.after && obj.after(obj.$ele, obj.data)
        })
    }
    return my;
};

var MODULE = null;

MODULE = new ArbitrarySearcher();

function process($ele) {
    var $collapsible = $ele.closest(".search-collapsible");
    return {
        $collapsible: $collapsible,
        $collapsible_parent: $collapsible.closest(".search-collapsible-parent-title"),
        $collapsible_title: $collapsible.closest(".search-collapsible-parent-title")
    }
}

//// all this is for expanding
function getDomElement(element) {
    if (element instanceof jQuery)
        return element.get()
    return element
}
getPrivateId = function () {
    var expando_callback_id_counter = 0;
    var getPrivateId = function (dom_element) {
        dom_element = getDomElement(dom_element);
        if (!dom_element.expando_callback_id) {
            dom_element.expando_callback_id = expando_callback_id_counter;
            expando_callback_id_counter += 1;
        }
        return dom_element.expando_callback_id
    }
    return getPrivateId;
}()
var expand_list = null;
function expand_list_callback() {
    if (expand_list) {
        _.each(expand_list, function (value, key) {
            // key is irrelevant, the value is our dom object
            if (value.search_expand) {
                $(value).collapse("show");
            } else {
                $(value).collapse("hide");
            }
        })
        expand_list = null;
    }
}

var expando_callbacks = {
    before: function ($ele, data) {
        //initialize if it hasn't been already
        if (!expand_list)
            expand_list = {}
        $ele.parents(".search-expand").each(function () {
            getDomElement(this).search_expand = false; // mark it to collapse by default
            expand_list[getPrivateId(this)] = this; //just to keep it around
        })
    },
    included: function ($ele, data) {
        $ele.parents(".search-expand").each(function () {
            getDomElement(this).search_expand = this; // mark it to expand
        })
    },
    after: expand_list_callback
}

MODULE.init({
    input: $("#SearchInput"),
    targets: [
        { // customers
            target: $("#customer-panel"),
            elements: function ($target) {
                return $target.find("> .panel > .panel-heading > a")
            },
            before: expando_callbacks.before,
            included: expando_callbacks.included,
            after: expando_callbacks.after
        },
        { // unexpandable projects list
            target: $("#customer-panel"),
            elements: function ($target) {
                return $target.find("> .panel > .panel-collapse > div > .list-group > .list-group-item > .accordion-toggle")
            },
            before: expando_callbacks.before,
            included: expando_callbacks.included,
            after: expando_callbacks.after
        }
    ]
})

$(document).ready(function () {

	if (location.hash) {
		$(location.hash).collapse('show');
		$(location.hash).parents('.accordion-body').collapse('show');
	}
});

//$('#Country').change(updateStateDDL);