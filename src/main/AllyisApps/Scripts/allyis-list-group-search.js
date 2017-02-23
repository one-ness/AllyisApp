//// Clientside searching lib.
//// Searches on text or data-search attribute. Case insensitive.
//// Marks the $target element with "search" class when initialized.
//// Toggles "searching" class when currently filtering. Pass "" or false to Search to disasble.
//// Marks searched elements with "excluded" or "included" class.
//// Optional element with "none-found" class may be included at the end and will be excluded from searching.
//// View the full public interface at the bottom of this module.
//// Example Usage:
////   MODULE = new ListGroupSearch();
////   MODULE.init({$target: $("#yourlist")})
////   MODULE.search("hello wor")
////   $("#thedudes-search").keyup(_.debounce(
////     function() {MODULE.search(this.value);},
////     200
////   ));
//// Demo:
var ListGroupSearch = function () {
    //// conf
    function regex_escape(text) {
        return text.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
    }
    function construct_text_regex(text_partial) {
        return new RegExp(".*" + regex_escape(text_partial) + ".*", "i");
    }
    function construct_search_regex(search_partial) {
        return new RegExp(".*" + regex_escape(search_partial) + ".*", "i");
    }
    function search_mark_ele_found($ele) {
        $ele.addClass("included").removeClass("excluded");
    }
    function search_mark_ele_hidden($ele) {
        $ele.removeClass("included").addClass("excluded");
    }

    //// internal vars
    var $target = null;
    var $thedudes = null;
    var the_list = [];

    //// utils
    function arr_diff(old_array, new_array) {
        // new array must contain a subset of the elements in old_array
        return $(old_array).not(new_array).get();
    }

    //// internal methods
    function init(conf) {
        $target = conf.$target; // jquery target list-group div
        $target.addClass("allyis-search");
        $thedudes = $target.find("> *:not(.none-found)");
        // add all the elements to the existing array
        Array.prototype.push.apply(the_list, scan_elements($thedudes));
    }

    function scan_elements($thelist) {
        var dudes = [];
        $thelist.each(function () {
            var $this = $(this);
            var dude = {
                $ele: $this,
                fulltext: $this.text(),
                search: $this.attr("data-search")
            }
            dudes.push(dude);
        });
        return dudes;
    }

    function filter_on_text_or_search(list, text_regex, search_regex) {
        return list.filter(email_or_text_filter_function,
                           {
                               text_regex: text_regex,
                               search_regex: search_regex
                           })
    }

    function email_or_text_filter_function(obj) {
        // force boolean
        return !!this.text_regex.exec(obj.fulltext)
                  ||
               !!this.search_regex.exec(obj.search)
    }

    function filter(text_or_search_partial) {
        var text_regex = construct_text_regex(text_or_search_partial);
        var search_regex = construct_search_regex(text_or_search_partial);
        return filter_on_text_or_search(the_list, text_regex, search_regex);
    }

    function search(text_or_search_partial) {
        if (text_or_search_partial) {
            yes = filter(text_or_search_partial);
            no = arr_diff(the_list, yes);
            _.each(yes, function (obj) {
                search_mark_ele_found(obj.$ele);
            });
            _.each(no, function (obj) {
                search_mark_ele_hidden(obj.$ele);
            });
            $target.addClass("searching");
        } else {
            $target.removeClass("searching");
        }
    }

    //// public interface
    return {
        init: init, // args: target element
        filter: filter, // args: string text_or_search_partial
        search: search, // args: string text_or_search_partial. replaces contents of $target
        full_list: the_list // the full array of objects. used internally.
    }
}
