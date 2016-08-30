//utility from http://stackoverflow.com/questions/18405736/is-there-a-c-sharp-string-format-equivalent-in-javascript
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

var cur_filter = []
var cur_search = ""

var all_user_$ele = []

// set the data for a dropdown filter (1,3,4,5,7,9)
function makeFilter(option, list) {
    var $option = $(option);
    $option.attr("value", list);
}

// generate the filters dropdown from filter-list data
function generateFilters($dropdown, filter_list) {
    $dropdown.html("");
    _.each(filter_list, function (it) {
        if (it.optgroup) {
            var element = document.createElement("optgroup")
            element.setAttribute("label", it.optgroup)
            $dropdown.append(element);
            var $parent = $(element);
            _.each(it.filters, function (it2) {
                var element = document.createElement("option")
                element.text = it2.name
                $parent.append(element);
                makeFilter(element, it2.list)
            })
        } else {
            var element = document.createElement("option")
            element.text = it.name
            $dropdown.append(element);
            makeFilter(element, it.list)
        }
    })
}

// set the checked class on the given element
function set_checked_class($ele) {
    if ($ele.find("input[type='checkbox']:checked").length > 0) {
        $ele.addClass("checked");
    } else {
        $ele.removeClass("checked");
    }
}

// clear the included/excluded classes on all_user_$ele
// ensure checked classes are up to date on all_user_$ele
function clear_filter() {
    _.each(all_user_$ele, function ($ele) {
        $ele.removeClass("included").addClass("excluded");
        set_checked_class($ele);
    })
}

// set the included/excluded classes on all_user_$ele according to cur_filter and cur_search
// ensure checked classes are up to date on all_user_$ele
function set_filter() {
    if (_.isEmpty(cur_filter[0])) return;
    _.each(cur_filter, function (val) {
        var meta = user_meta[val];

        var metaVal = _.values(meta)[1].toLowerCase();
        var searchVal = cur_search.toLowerCase();

        if (metaVal.search(searchVal) > -1) {
            meta.$ele.addClass("included").removeClass("excluded");
        }
        set_checked_class(meta.$ele);
    })
}

// perform filtering
function do_filter() {
    clear_filter();
    set_filter();
}

// collect the list of elements into all_user_$ele and user_meta
function collect_elements() {
    $("#user-list div").each(function () {
        var $ele = $(this);
        all_user_$ele.push($ele);
        var id = $ele.find("input[type='checkbox']").attr("id");
        user_meta[id] = user_meta[id] || {};
        user_meta[id].$ele = $ele;
        $ele.addClass("included");
    })
}

function do_it_form_submit() {
    var form = $(document.createElement("form")).attr({ "method": "POST", "action": "ManagePermissions" });
    var data = JSON.stringify(do_it_gather_data());
    $(document.createElement("input")).attr({ "type": "hidden", "name": "data", "value": data }).appendTo(form);
    form.appendTo(document.body).submit();
}

function do_it_gather_data() {
    var checked_actions = _.map(
      $("#actions input[type='checkbox']:checked"),
      function (e) {
          return $(e).attr("data-value")
      })
    var checked_users = _.map(
      $("#user-list input[type='checkbox']:checked"),
      function (e) {
          var $e = $(e);
          return { userid: $e.attr("id"), name: $e.attr("data-name") }
      })
    var result = {}
    result.SelectedUsers = checked_users;
    result.SelectedActions = {}
    _.each(checked_actions, function (e) {
        var split = e.split("_");
        var category = split[0];
        var value = split[1];
        result.SelectedActions[category] = value;
    })
    return result;
}

//// prep
$(document).ready(function () {
    //// prep for filtering
    collect_elements();

    //// for dropdown filtering
    var $dropdown = $("#filter-dropdown");
    //generateFilters($dropdown, filters);
    $dropdown.change(function () {
        var value = $dropdown.val().split(",");
        cur_filter = value;
        do_filter();
    })
    $dropdown.change();

    //// for user search filtering
    var $usersearch = $("#user-search");
    $usersearch.keyup(_.debounce(function () {
        cur_search = $usersearch.val();
        do_filter();
    }, 200))

    //// for the actions
    // disable others in group when checked
    var action_checks = $("#actions > .list-group-swap > .list-group-item > input[type='checkbox']:not(.master-check)")
    action_checks.change(function (e) {
        var $this = $(this);
        if ($this.is(':checked')) {
            $this.closest(".list-group-swap").find(".list-group-item > input[type='checkbox']:not(:checked)").attr("disabled", true);
        } else {
            $this.closest(".list-group-swap").find(".list-group-item > input[type='checkbox']:not(:checked)").attr("disabled", false);
        }
    })

    // disable all others when checked
    var master_checks = $("#actions > .list-group-swap > .list-group-item > input[type='checkbox'].master-check")
    master_checks.change(function (e) {
        var $this = $(this);
        if ($this.is(':checked')) {
            var checks = $this.closest(".list-group").find(".list-group-item > input[type='checkbox']");
            checks.each(function (e) {
                var $cur_this = $(this);
                if ($cur_this.context != $this.context) {
                    $cur_this.attr("disabled", true).attr("checked", false);
                }
            })
        } else {
            $this.closest(".list-group").find(".list-group-item > input[type='checkbox']").attr("disabled", false);
        }
    })

    // gather data and submit
    $("#do-it-button input#actually-do-it").click(do_it_form_submit);
});