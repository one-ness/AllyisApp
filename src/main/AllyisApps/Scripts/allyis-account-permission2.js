filterRows = function() {
    var userFilter = userSearch.val().toLowerCase();
    var orgRole = orgRoleDD.val();
    if(hasTT) {
        ttRole = ttRoleDD.val();
    }

    $('.userRow').each(function (index) {
        var ele = $(this);
        var eleCheckBox = ele.find("#checked > input")[0];
        var eleUser = ele.find("#name");
        var eleOrgRole = ele.find("#orgRole");
        var eleTTRole;
        if (hasTT) eleTTRole = ele.find("#ttRole");

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
            }
        }
    });
}

gatherData = function () {
    var checked_users = [];

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
    
    result = {};
    result.SelectedUsers = checked_users;
    result.SelectedActions = selectedAction;
    return result;
}

formSubmit = function() {
    var form = $(document.createElement("form")).attr({ "method": "POST", "action": "ManagePermissions" });
    var data = gatherData();
    if (!data) {
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
    userSearch.on('input', _.debounce(function () {
        filterRows();
    }, 200));

    $('#do-it').on("click", function () {
        formSubmit();
    });
});