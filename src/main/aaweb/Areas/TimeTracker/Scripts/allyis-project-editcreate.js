// Moves selected elements from left (subList) to right (projectList) if the direction is in
// or right to left if direction is out
function moveSelectedProjectEditModal(direction) {
    // Assuming only internally called with "in" or "out"
    var addList = direction == "in" ? document.getElementById('included-users') : document.getElementById('excluded-users');
    var removeList = direction == "in" ? document.getElementById('excluded-users') : document.getElementById('included-users');

    for (var i = 0; i < removeList.length;) {
        if (removeList[i].selected) {
            var foo = removeList[i];
            addList.add(new Option(foo.text, foo.value));
            removeList.remove(i);
        } else {
            i++;
        }
    }
}

var selectAll = function () {
    $('#included-users option').prop("selected", "true"); // Select all users in ProjectUsers listbox for model binding
    return true;
};

//var ArbitrarySearcher = function () {
//    var my = {};
//    var all_targets = null;
//    var all_elements = null;
//    my.all = function () {
//        return all_elements;
//    }
//    // defaults for the conf.targets objects
//    var target_defaults = {
//        target: null,
//        elements: function ($target) { return $target.find('> *'); },
//        searchtext: function ($ele) {
//            var text = $ele.text()
//            var searchdata = $ele.attr("data-search")
//            if (searchdata)
//                return text + " " + searchdata
//            else
//                return text
//        },
//        style: function ($ele, included) {
//            if (included) {
//                $ele.addClass("included").removeClass("excluded")
//            } else {
//                $ele.addClass("excluded").removeClass("included")
//            }
//        },
//        clear: function ($ele) {
//            $ele.removeClass("excluded").removeClass("included")
//        }
//    }

//    // initialize the module
//    my.init = function (conf) {
//        init_process_targets(conf.targets)
//        //collapsibles = init_process_collapsibles();
//        // debounced auto search
//        conf.input && conf.input.keyup(_.debounce(
//          function () {
//              my.search(this.value);
//          },
//            200
//          ));
//    }

//    // process the conf.targets argument to init.
//    // initalizes all_targets, all_elements
//    function init_process_targets(targets) {
//        all_targets = [];
//        all_elements = [];
//        var TDEF = target_defaults;
//        _.each(targets, function (obj) {
//            var res = {};
//            all_targets.push(res);
//            res.target = obj.target;
//            var elements = obj.elements ? obj.elements(res.target) :
//                                    TDEF.elements(res.target);
//            res.elements = _.map(elements, function ($ele) {
//                $ele = $($ele);
//                return {
//                    // jquery element
//                    $ele: $ele,
//                    // raw string
//                    searchtext: (obj.searchtext ? obj.searchtext($ele) :
//                                            TDEF.searchtext($ele)).toLowerCase(),
//                    data: obj.process && obj.process($ele),
//                    before: obj.before,
//                    after: obj.after,
//                    included: obj.included,
//                    excluded: obj.excluded,
//                    style: obj.style || TDEF.style,
//                    clear: obj.clear || TDEF.clear
//                }
//            });
//            Array.prototype.push.apply(all_elements, res.elements);
//        });
//    }

//    // process the search_targets to collect their unique collapsibles
//    function init_process_collapsibles() {
//        var res = [];
//        // map elements to collapsibles and add to res
//        _.each(search_targets, function (target) {
//            Array.prototype.push.apply(res,
//              _.map(target.elements, function (obj) {
//                  return {
//                      $collapsible: obj.$collapsible,
//                      $collapsible_parent: obj.$collapsible_parent,
//                      $collapsible_parent_title: obj.$collapsible_parent_title
//                  }
//              })
//            )
//        })
//    }
//    // filter helper
//    function filter_function(obj) /*this object is the passed value*/ {
//        return obj.searchtext.indexOf(this) > -1
//    }
//    // search helper
//    function filter(elements_list, text) {
//        return elements_list.filter(filter_function, text.toLowerCase())
//    }

//    my.clear = function () {
//        _.each(all_elements, function (obj) {
//            obj.before && obj.before(obj.$ele, obj.data);
//        })
//        _.each(all_elements, function (obj) {
//            obj.clear && obj.clear(obj.$ele, obj.data);
//        })
//        _.each(all_elements, function (obj) {
//            obj.after && obj.after(obj.$ele, obj.data);
//        })
//    }

//    my.search = function (text) {
//        if (!text) {
//            my.clear()
//            return
//        }
//        var yes = filter(all_elements, text)
//        var no = _.difference(all_elements, yes)

//        // before
//        _.each(yes, function (obj) {
//            obj.before && obj.before(obj.$ele, obj.data)
//        })
//        _.each(no, function (obj) {
//            obj.before && obj.before(obj.$ele, obj.data)
//        })

//        // main
//        _.each(yes, function (obj) {
//            obj.included && obj.included(obj.$ele, obj.data)
//            obj.style && obj.style(obj.$ele, true)
//        })
//        _.each(no, function (obj) {
//            obj.excluded && obj.excluded(obj.$ele, obj.data)
//            obj.style && obj.style(obj.$ele, false)
//        })

//        // after
//        _.each(yes, function (obj) {
//            obj.after && obj.after(obj.$ele, obj.data)
//        })
//        _.each(no, function (obj) {
//            obj.after && obj.after(obj.$ele, obj.data)
//        })
//    }
//    return my;
//};
//var MODULE = null;
//$(document).ready(function () {
//    MODULE = new ArbitrarySearcher();

//    function process($ele) {
//        var $collapsible = $ele.closest(".search-collapsible");
//        return {
//            $collapsible: $collapsible,
//            $collapsible_parent: $collapsible.closest(".search-collapsible-parent-title"),
//            $collapsible_title: $collapsible.closest(".search-collapsible-parent-title")
//        }
//    }

//    //// all this is for expanding
//    function getDomElement(element) {
//        if (element instanceof jQuery)
//            return element.get()
//        return element
//    }
//    getPrivateId = function () {
//        var expando_callback_id_counter = 0;
//        var getPrivateId = function (dom_element) {
//            dom_element = getDomElement(dom_element);
//            if (!dom_element.expando_callback_id) {
//                dom_element.expando_callback_id = expando_callback_id_counter;
//                expando_callback_id_counter += 1;
//            }
//            return dom_element.expando_callback_id
//        }
//        return getPrivateId;
//    }()
//    var expand_list = null;
//    function expand_list_callback() {
//        if (expand_list) {
//            _.each(expand_list, function (value, key) {
//                // key is irrelevant, the value is our dom object
//                if (value.search_expand) {
//                    $(value).collapse("show");
//                } else {
//                    $(value).collapse("hide");
//                }
//            })
//            expand_list = null;
//        }
//    }

//    var expando_callbacks = {
//        before: function ($ele, data) {
//            //initialize if it hasn't been already
//            if (!expand_list)
//                expand_list = {}
//            $ele.parents(".search-expand").each(function () {
//                getDomElement(this).search_expand = false; // mark it to collapse by default
//                expand_list[getPrivateId(this)] = this; //just to keep it around
//            })
//        },
//        included: function ($ele, data) {
//            $ele.parents(".search-expand").each(function () {
//                getDomElement(this).search_expand = this; // mark it to expand
//            })
//        },
//        after: expand_list_callback
//    }

//    MODULE.init({
//        input: $("#SearchInput"),
//        targets: [
//          { // customers
//              target: $("#customer-panel"),
//              elements: function ($target) {
//                  return $target.find("> .panel > .panel-heading > a")
//              },
//              before: expando_callbacks.before,
//              included: expando_callbacks.included,
//              after: expando_callbacks.after
//          },
//          { // unexpandable projects list
//              target: $("#customer-panel"),
//              elements: function ($target) {
//                  return $target.find("> .panel > .panel-collapse > div > .list-group > .list-group-item > .accordion-toggle")
//              },
//              before: expando_callbacks.before,
//              included: expando_callbacks.included,
//              after: expando_callbacks.after
//          }
//        ]
//    })
//});

var project_assign_module = function () {
    var modal;
    var left;
    var right;
    var left_list;
    var right_list;
    var left_input;
    var right_input;
    var left_select;
    var right_select;
    var actions;
    var action_move_in;
    var action_move_out;

    function init() {
        modal = $("#ProjectForm");
        actions = modal.find(".actions");
        action_move_in = actions.find(".move-in"); //?
        action_move_out = actions.find(".move-out"); //?
        left = modal.find(".swapper-ui .left");
        right = modal.find(".swapper-ui .right");
        left_select = left.find("select")
        right_select = right.find("select")
        left_input = left.find("input"); // Left search bar
        right_input = right.find("input"); // Right search bar

        // Changed parameters for filter and refilter at call to be .find() result
        // each time to ensure the search results reflected changes made in the modal
        left_input.keyup(_.debounce(function () {
            filter(this.value.toLowerCase(), left.find("select option"));
        }, 250));
        right_input.keyup(_.debounce(function () {
            filter(this.value.toLowerCase(), right.find("select option"));
        }, 250));
    }

    function refilter() {
        filter(left_input.value.toLowerCase(), left.find("select option"));
        filter(right_input.value.toLowerCase(), right.find("select option"));
    }

    // filters an element on a phrase
    function filter(text, what) {
        _.each(what, function (ele) {
            var $ele = $(ele);
            if (!$ele.text_lower) {
                $ele.text_lower = $ele.text().toLowerCase();
                if ($ele.attr("data-search"))
                    $ele.text_lower += " " + $ele.attr("data-search").toLowerCase();
            }
            var val = $ele.text_lower;
            if (val.indexOf(text) > -1) {
                $ele.addClass("included").removeClass("excluded");
            } else {
                $ele.addClass("excluded").removeClass("included");
            }
        })
    }

    function get_in() {
        return _.map(right_select.find("option.in"), function (ele) { return ele.value });
    }

    function get_out() {
        return _.map(left_select.find("option.out"), function (ele) { return ele.value })
    }

    return {
        init: init,
        filter: filter,
        refilter: refilter,
        get_in: get_in,
        get_out: get_out
    }
}()

$(document).ready(function () {
    project_assign_module.init()
});

function formChange() {
    var date = $('#StartDate').val();
}

$('form').change(function () { formChange(); });

// Date range picker
init_drp(
    "daterange",
    [{
        text: "All Time",
        dateStart: function () { return moment(mindate_shortstring, 'MM/DD/YYYY') },
        dateEnd: function () { return moment(maxdate_shortstring, 'MM/DD/YYYY') }
    }, {
        text: "Reset Start",
        dateStart: function () { return moment(mindate_shortstring, 'MM/DD/YYYY') },
        dateEnd: function () { return moment($('#daterange').daterangepicker("getRange").end) }
    }, {
        text: "Reset End",
        dateStart: function () { return moment($('#daterange').daterangepicker("getRange").start) },
        dateEnd: function () { return moment(maxdate_shortstring, 'MM/DD/YYYY') }
    },{
        text: "This Month",
        dateStart: function () { return moment().startOf('month') },
        dateEnd: function () { return moment().endOf('month') }
    }, {
        text: "Last Month",
        dateStart: function () { return moment().subtract(1, 'month').startOf('month') },
        dateEnd: function () { return moment().subtract(1, 'month').endOf('month') }
    }, {
        text: "This quarter",
        dateStart: function () { return moment().startOf('quarter') },
        dateEnd: function () { return moment().endOf('quarter') }
    }, {
        text: "Last quarter",
        dateStart: function () { return moment().subtract(1, 'quarter').startOf('quarter') },
        dateEnd: function () { return moment().subtract(1, 'quarter').endOf('quarter') }
    }],
    "StartDate",
    "EndDate",
    2,
    function () {
        setTimeout(
            function () {
                formChange();
            }, 100
        );
    },
    model_startdate,
    model_enddate
);