/* 
allyis-pages-with-filter.js
Utility for providing a table of rows that can be filtered and paginated in tandem.

To use:
    In your html, provide each of the filterable rows with some class (e.g. class="filter-row"). Give an identifying id or class
to each item within the row that you will use to match to some filter. If you would like to include check boxes in the filtering
(i.e. any checked rows will remain visible even when they don't match the filters), then you should also have an id or class for
them.
    At the bottom (or top, or wherever), include a div with class="pageContainer" - this will hold the page buttons (they will
auto-populate).
    Filters can be text boxes or select inputs (drop-down menus). Make sure each filter you will use has an id or unique class to
identify it by.
    Put a script tag in your view after this script loads. In it, you'll need to define the following variables:
rowSelector - set this equal to a css selector for your filterable rows (e.g. '.filter-row')
checkBoxSelector - if you are including check boxes, set this to a css selector for their id/class within a row (e.g. '#check')
    Then, use the addFilter method to define what filters you have and what they act on. This is the syntax:
addFilter(filterElementSelector, filterType, rowFilteredPropertySelector, noneValue);
filterElementSelector - a css selector for the id/class of the filter (e.g. '#nameFilter')
filterType - this must be either "search" for a text box, or "dropDown" for a select element
rowFilteredPropertySelector - a css selector for the id/class of the item within each row that this filter is applied to (e.g. '#rowUserName')
noneValue - (optional) for dropDown types, this is the text of the 'neutral' value (e.g. "Select...", "Any", "None", etc.)
    That should set up your filters. If you want to include an 'all-check' check-box, simply make sure the checkbox input element
has class="all-check". This checkbox will have a darker outline, and checking/unchecking it will check/uncheck all visible rows.
    The last step is to make sure you import the 'allyis-pages-with-filter.less' LESS file in your page's LESS file. (Note:
remember to add an import reference to 'allyis-pages-with-filter.less' when you do).
*/




// Globals for page info
pageLimit = 16;
pageContainer = $('.pageContainer'); // Place a div on your page with the class pageContainer - it will auto-populate with page buttons
thisPage = 0;
totalPages = 1;
anchorRow = null; // This is the row element that is used to decide what page to show when pagination changes

// Globals for filtering
// Note: specified in view are global variables:
//  rowSelector - css selector to grab all row elements
//  checkBoxSelector - css selector to find check boxes within rows. Leave null if you are not using checkboxes.
filters = []; // Data sructure to store filters in use and their types

// Goes to the specified page by toggling the 'offPage' class
goToPage = function (pageNum) {
    if (!pageNum || pageNum <= 0 || pageNum > totalPages) return;

    $(rowSelector).each(function () {
        var ele = $(this);
        if (ele.attr("data-page") == pageNum) {
            ele.toggleClass("off-page", false);
        }
        else {
            ele.toggleClass("off-page", true);
        }
    });

    // Enable/disable page buttons
    thisPage = pageNum;
    $('.page-btn').prop('disabled', false);
    $('#page-' + pageNum).prop('disabled', true);
}

// Changes to the specified page. Same as 'goToPage()' but also updates anchor user
changePage = function (pageNum) {
    goToPage(pageNum);
    anchorRow = findFirstRow(); // Anchor row updated on page change
}

// Finds the first user row that is shown at the current page with the current filters
findFirstRow = function () {
    var result = null;
    $(rowSelector).each(function () {
        if (!$(this).hasClass("off-page") && !$(this).hasClass("no-match")) {
            result = $(this);
            return false;
        }
    });
    return result;
}

// Creates a button element for paging and returns it
makePageButton = function (pageNum) {
    return $('<input/>', {
        id: "page-" + pageNum,
        type: "button",
        "class": "page-btn btn btn-primary btn-xs",
        value: pageNum
    }).click(function () {
        changePage(pageNum);
    });
}

// Registers an element to act as a search filter.
//  filterElementSelector - a css selector to find the input element that controls this filter
//  filterType - either "search" or "dropDown", indicating what kind of filter it is
//  rowFilteredPropertySelector - a css selector to find the element within a row that this filter acts on
//  noneValue - for drop down filters, this is the text of the 'none' option (e.g. "Any", "None", "Select...", etc.)
addFilter = function (filterElementSelector, filterType, rowFilteredPropertySelector, noneValue) {
    // validate type
    if (["search", "dropDown"].indexOf(filterType) == -1) {
        console.log("Error registering filter for " + filterElementSelector + "; '" + filterType + "' is not a valid filter type. Use 'search' or 'dropDown'");
        return;
    }

    // create filter (no validation of selectors)
    var newFilter = {};
    newFilter.selector = filterElementSelector;
    newFilter.type = filterType;
    newFilter.find = rowFilteredPropertySelector;
    if (noneValue) newFilter.noneValue = noneValue;
    filters.push(newFilter);

    // add event listener
    if (filterType == "search") {
        $(filterElementSelector).on('input', _.debounce(function () { // The 'input' actions catches changes AND clicking the X to clear
            filterRows();
        }, 400));
    }
    else {
        $(filterElementSelector).change(function () {
            filterRows();
        });
    }

    console.log("Added filter:");
    console.log(newFilter);
}

// Updates the classes on each row item according to the current filter settings, and
//  assigns page numbers to each row based on their display status.
filterRows = function () {
    console.log('filtering rows');

    // Grab all filter values
    var filterValues = [];
    for (var i = 0; i < filters.length; i++) {
        filterValues[i] = $(filters[i].selector).val();
        if (filters[i].type == "search") filterValues[i] = filterValues[i].toLowerCase();
    }

    // Start off the paging process.
    var currentPage = 0;
    var currentPageTotal = pageLimit - 1;
    pageContainer.empty();

    // Evaluate each row and update classes
    $(rowSelector).each(function (index) {
        var ele = $(this);

        var checked = true;
        if (checkBoxSelector) {
            checked = ele.find(checkBoxSelector)[0].checked;
        }

        // Go through registered filters
        var display = [];
        for (var i = 0; i < filters.length; i++) {
            var thisItemText = ele.find(filters[i].find).html();
            if (filters[i].type == "search") {
                if (!filterValues[i] || filterValues[i] == "") {
                    display[i] = true; // Display all when no search text entered
                }
                else {
                    display[i] = thisItemText.toLowerCase().search(filterValues[i]) > -1;
                }
            }
            else {
                if (filters[i].noneValue && filterValues[i] == filters[i].noneValue) {
                    display[i] = true; // Display all when no filter option selected
                }
                else {
                    display[i] = thisItemText == filterValues[i];
                }
            }
        }

        // Anything that matches all filters is displayed normally
        // Anything else that is checked is still displayed, but with alternate style
        // Anything else is hidden
        var isShown = true;
        var matchesAllFilters = true;
        for (var i = 0; i < display.length; i++) {
            if (!display[i]) matchesAllFilters = false;
        }

        if (matchesAllFilters) {
            ele.toggleClass("check-no-match", false);
            ele.toggleClass("no-match", false);
        }
        else {
            if (checked) {
                ele.toggleClass("check-no-match", true);
                ele.toggleClass("no-match", false);
            }
            else {
                ele.toggleClass("check-no-match", false);
                ele.toggleClass("no-match", true);
                isShown = false;
            }
        }

        // Assign page. Paging starts at end of "page 0" with incrementation before
        // page assignment. So, the first element becomes the first item on page 1. This way,
        // if the last element perfectly fills the last page, it doesn't make another page
        // yet.
        if (isShown) {
            currentPageTotal += 1;
            if (currentPageTotal >= pageLimit) { // New page
                currentPageTotal = 0;
                currentPage += 1;
                pageContainer.append(makePageButton(currentPage));
            }
        }
        ele.attr("data-page", Math.max(currentPage, 1)); // We don't want hidden elements with page 0
    });

    totalPages = currentPage;

    // After pagination is recalculated, the anchor row is used to decide which page to show. This way,
    // if a user is changing the filters a lot, their place in the list is roughly constant.
    if (anchorRow) {
        goToPage(anchorRow.attr("data-page"));
    }
    else {
        goToPage(1);
        anchorRow = findFirstRow();
    }
}

$(document).ready(function () {
    if ($('.all-check').length == 1) {
        if (checkBoxSelector) {
            $('.all-check').change(function () {
                $(rowSelector).each(function () {
                    var ele = $(this);
                    if (!ele.hasClass("no-match")) {
                        ele.find(checkBoxSelector).prop('checked', $('.all-check')[0].checked);
                    }
                });
            });
        }
    }

    filterRows();
});