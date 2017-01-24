// Globals for page info
// Note: specified in view is global variable pageLimit
thisPage = 0;
totalPages = 1;
anchorUser = null;

// Goes to the specified page by toggling the 'offPage' class
goToPage = function (pageNum) {
    if (!pageNum || pageNum <= 0 || pageNum > totalPages) return;

    $('.userRow').each(function () {
        var ele = $(this);
        if (ele.attr("data-page") == pageNum) {
            ele.toggleClass("off-page", false);
        }
        else {
            ele.toggleClass("off-page", true);
        }
    });

    thisPage = pageNum;
    $('.page-btn').prop('disabled', false);
    $('#page-' + pageNum).prop('disabled', true);
}

// Creates a button element for paging and returns it
makePageButton = function (pageNum) {
    return $('<input/>', {
        id: "page-" + pageNum,
        type: "button",
        "class": "page-btn btn btn-primary btn-xs",
        value: pageNum
    }).click(function () {
        goToPage(pageNum);
        anchorUser = findFirstRow();
    });
}

// Finds the first user row that is shown at the current page with the current filters
findFirstRow = function () {
    var result = null;
    $('.userRow').each(function () {
        if (!$(this).hasClass("off-page") && !$(this).hasClass("no-match")) {
            result = $(this);
            return false;
        }
    });
    return result;
}

// Updates the classes on each row item according to the current filter settings, and
//  assigns page numbers to each row based on their display status.
filterRows = function () {
    // Grab all filter values
    var userFilter = userSearch.val().toLowerCase();
    var orgRole = orgRoleDD.val();
    if(hasTT) {
        ttRole = ttRoleDD.val();
    }

    // Start off the paging process. We start with a full page 0. That way, if there are no rows,
    //  the page count stays at 0, but if there's even 1, we get a page 1. Also, if the last row
    //  perfectly fills up a page, it won't make the next page.
    var currentPage = 0;
    var currentPageTotal = pageLimit - 1;
    var pageContainer = $('.pageContainer');
    pageContainer.empty();

    // Evaluate each row and update classes
    $('.userRow').each(function (index) {
        var ele = $(this);
        var eleCheckBox = ele.find("#checked > input")[0];
        var eleUser = ele.find("#name");
        var eleOrgRole = ele.find("#orgRole");
        var eleTTRole;
        if (hasTT) eleTTRole = ele.find("#ttRole");

        // This array represents the result of each filter check
        // [checked, match name search, match org role, match tt role]
        var display = [false, true, true, true];

        display[0] = (eleCheckBox.checked);

        if (userFilter && userFilter != "") {
            display[1] = (eleUser.html().toLowerCase().search(userFilter) > -1);
        }

        if (orgRole != anyValue) {
            display[2] = (eleOrgRole.html() == orgRole);
        }

        if (hasTT) {
            if (ttRole != anyValue) {
                display[3] = (eleTTRole.html() == ttRole);
            }
        }

        // Anything that matches all 3 filters is displayed normally
        // Anything else that is checked is still displayed, but with alternate style
        // Anything else is hidden
        var isShown = true;
        if (display[1] && display[2] && display[3]) {
            ele.toggleClass("check-no-match", false);
            ele.toggleClass("no-match", false);
        }
        else
        {
            if (display[0]) {
                ele.toggleClass("check-no-match", true);
                ele.toggleClass("no-match", false);
            }
            else
            {
                ele.toggleClass("check-no-match", false);
                ele.toggleClass("no-match", true);
                isShown = false;
            }
        }

        if (isShown) {
            currentPageTotal += 1;
            if (currentPageTotal >= pageLimit) {
                currentPageTotal = 0;
                currentPage += 1;
                pageContainer.append(makePageButton(currentPage));
            }
        }
        ele.attr("data-page", currentPage);
    });

    totalPages = currentPage;
    if (anchorUser) {
        goToPage(anchorUser.attr("data-page"));
    }
    else
    {
        goToPage(1);
        anchorUser = findFirstRow();
    }
}

// Gathers the data to send to the ManagePermissions action. Data must be structered as
// a UserPermissionsAction object (in services).
gatherData = function () {
    var checked_users = [];

    // Go through each row and add it to the data if it's checked
    $('.userRow').each(function () {
        var userEle = $(this);
        if (userEle.find("#checked > input")[0].checked) {
            var userResult = {};
            userResult.userid = userEle.attr("data-id");
            userResult.name = userEle.attr("data-name");
            checked_users.push(userResult);
        }
    });
    if (checked_users.length == 0) return null;
    
    // Add in target action information
    var selectedAction = {};
    var value = orgActions[$('#actionSelect').val()];
    if (!value) {
        value = ttActions[$('#actionSelect').val()];
        selectedAction["TimeTrackerRoleTarget"] = value;
    }
    else {
        if (value == -1) {
            if (!confirm(confirmMessage)) return null;
        }
        selectedAction["OrgRoleTarget"] = value;
    }
    
    // Assemble and return UserPermissionsAction object of data
    result = {};
    result.SelectedUsers = checked_users;
    result.SelectedActions = selectedAction;
    return result;
}

// Creates a form element and submits it using data gathered from the page's state
formSubmit = function() {
    var form = $(document.createElement("form")).attr({ "method": "POST", "action": "ManagePermissions" });
    var data = gatherData();
    if (!data) { // gatherData() will return null if no one is checked or the user declines the confirmation prompt.
        return;
    }
    var datajson = JSON.stringify(data);

    $(document.createElement("input")).attr({ "type": "hidden", "name": "data", "value": datajson }).appendTo(form);
    form.appendTo(document.body).submit();
}

$(document).ready(function () {
    orgRoleDD = $('#orgRoleFilter');
    orgRoleDD.change(function () {
        filterRows();
    });

    if(hasTT) {
        ttRoleDD = $('#ttRoleFilter');
        ttRoleDD.change(function () {
            filterRows();
        });
    }

    userSearch = $("#userSearch");
    userSearch.on('input', _.debounce(function () { // The 'input' actions catches changes AND clicking the X to clear
        filterRows();
    }, 200));

    $('#do-it').on("click", function () {
        formSubmit();
    });

    filterRows();
});