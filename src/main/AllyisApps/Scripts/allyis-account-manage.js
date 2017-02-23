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
function editEmployeeId(orgId, userId, oldEmployeeId) {
	var td = $('#empId-' + userId);
	td.empty();
	td.html(
		'<input id="empIdedit-' + userId + '" type="text" class="employeeIdEdit" data-oldval="' + oldEmployeeId + '" data-org="' + orgId + '" value="' + oldEmployeeId + '"/>' +
		' <a href="javascript: void(0);" class="text-muted" title="Save Changes" onclick=\x27saveEmployeeId("' + orgId + '", "' + userId + '")\x27><span class="fa fa-fw fa-save"></span></a>' +
		'<a href="javascript: void(0);" class="text-muted" title="Cancel Changes" onclick=\x27cancelEditEmployeeId("' + userId + '")\x27><span class="fa fa-fw fa-remove text-danger"></span></a>'
	);
}

function saveEmployeeId(orgId, userId) {
	var inp = $('#empIdedit-' + userId);
	var newId = inp.val();
	if (newId == inp.attr("data-oldval")) {
		cancelEditEmployeeId(userId);
		return;
	}
	//Check if id already in use
	var goAhead = true;
	$('.empId').each(function (index, elem) {
		if (elem.textContent == newId) {
			alert('The employee ID "' + newId + '" is alreay in use.');
			goAhead = false;
			return;
		}
	});
	if (goAhead) {
		var data = {
			user: userId,
			org: orgId,
			employeeId: newId
		};
		$.ajax({
			type: "post",
			url: "/Account/SaveEmployeeId/",
			data: data,
			timeout: 5000,
			dataType: "json"
		})
		.fail(function (res) {
			if (res.responseText == "True") {
				stopEditEmployeeId(userId, newId);
			} else {
				alert('Failed to update employee ID.');
				cancelEditEmployeeId(userId);
			}
		});
	}
}

function cancelEditEmployeeId(userId) {
	//var inp = $('#empIdedit-' + userId);
	//var oldEmployeeId = inp.attr("data-oldval");
	//var orgId = inp.attr("data-org");
	//var td = $('#empId-' + userId);
	//td.empty();
	//td.html(
	//	'<span class="empId">' + oldEmployeeId + '</span>' +
	//	' <a href="javascript: void(0);" class="text-muted" title="Edit Employee Id"	onclick=\x27editEmployeeId("' + orgId + '", "' + userId + '", "' + oldEmployeeId + '")\x27><span class="fa fa-fw fa-edit"></span></a>'
	//);
	var oldEmployeeId = $('#empIdedit-' + userId).attr("data-oldval");
	stopEditEmployeeId(userId, oldEmployeeId);
}

function stopEditEmployeeId(userId, newValue) {
	var inp = $('#empIdedit-' + userId);
	var orgId = inp.attr("data-org");
	var td = $('#empId-' + userId);
	td.empty();
	td.html(
		'<span class="empId">' + newValue + '</span>' +
		' <a href="javascript: void(0);" class="text-muted" title="Edit Employee Id"	onclick=\x27editEmployeeId("' + orgId + '", "' + userId + '", "' + newValue + '")\x27><span class="fa fa-fw fa-edit"></span></a>'
	);
}

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
