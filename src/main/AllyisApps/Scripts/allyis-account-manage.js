$(document).ready(function () {
    //var MODULE = new ListGroupSearch();
    //MODULE.init({
    //    $target: $("#org-members-list")
    //});

    //$("#org-members-search").keyup(_.debounce(
    //    function (key) {
    //        if (key.which == 13 || key.keyCode == 13) {
    //            if (this.value == "")
    //                MODULE.search(" "); //search on a value that is present in all entries
    //            else
    //                MODULE.search(this.value);
    //            //after search, set new page numbers
    //            paginate();
    //        }
    //    },
    //    200
    //));

    //$("#org-members-search").keyup(_.debounce(
    //    function () { MODULE.search(this.value); },
    //    200
    //));
});

//Clears validation error messages
$.fn.clearErrors = function () {
    $(this).each(function () {
        $(this).find(".field-validation-error").empty();
        $(this).trigger('reset.unobtrusiveValidation');
    });
};

// Members
function removeUser(orgId, userId, fullName) {
    var result = confirm(removeFromOrg + " " + fullName + " " + removeFromOrgEnd);
    if (result == true) {
        var url = removeMemberAction + userId;

        var form = document.createElement('form');
        form.setAttribute('method', 'post');
        form.setAttribute('action', url);
        form.style.display = 'hidden';

        var token = $('[name="__RequestVerificationToken"]').val();

        var input = document.createElement('input');
        input.type = 'hidden';
        input.name = '__RequestVerificationToken';
        input.value = token;
        form.appendChild(input);

        document.body.appendChild(form)
        form.submit();
    }
}

function deleteInvitation(orgId, invId) {
    var result = confirm(removeInvitation);
    if (result == true) {
        var url = removeInviationAction + invId;

        var token = $('[name="__RequestVerificationToken"]').val();

        var form = document.createElement('form');
        form.setAttribute('method', 'post');
        form.setAttribute('action', url);

        var input = document.createElement('input');
        input.type = 'hidden';
        input.name = '__RequestVerificationToken';
        input.value = token;
        form.appendChild(input);

        form.style.display = 'hidden';
        document.body.appendChild(form)
        form.submit();
    }
}