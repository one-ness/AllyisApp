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
});