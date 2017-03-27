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
		'<input id="empIdedit-' + userId + '" type="text" class="employeeIdEdit" data-oldval="' + oldEmployeeId + '" data-org="' + orgId + '" value="' + oldEmployeeId + '" maxlength="16"/>' +
		' <a href="javascript: void(0);" id="empIcon'+ userId +'" class="text-muted" title="Save Changes" onclick=\x27saveEmployeeId("' + orgId + '", "' + userId + '")\x27><span class="fa fa-fw fa-save"></span></a>' +
		'<a href="javascript: void(0);" class="text-muted" title="Cancel Changes" onclick=\x27cancelEditEmployeeId("' + userId + '")\x27><span class="fa fa-fw fa-remove text-danger"></span></a>'
	);
}

function editInvitationId(orgId, userId, oldEmployeeId) {
    var td = $('#invId-' + userId);
    td.empty();
    td.html(
		'<input id="invIdedit-' + userId + '" type="text" class="employeeIdEdit" data-oldval="' + oldEmployeeId + '" data-org="' + orgId + '" value="' + oldEmployeeId + '" maxlength="16"/>' +
		' <a href="javascript: void(0);" id="invIcon'+ userId +'" class="text-muted" title="Save Changes" onclick=\x27saveInvitationId("' + orgId + '", "' + userId + '")\x27><span class="fa fa-fw fa-save"></span></a>' +
		'<a href="javascript: void(0);" class="text-muted" title="Cancel Changes" onclick=\x27cancelEditInvitationId("' + userId + '")\x27><span class="fa fa-fw fa-remove text-danger"></span></a>'
	);
}

function saveEmployeeId(orgId, userId) {
    toggleSaveIcon(true, true, userId);
	var inp = $('#empIdedit-' + userId);
	var newId = inp.val();
	if (newId == inp.attr("data-oldval")) {
	    cancelEditEmployeeId(userId);
	    toggleSaveIcon(true, false, userId);
		return;
	}
	//Check if id already in use
	var goAhead = true;
	//$('.empId').each(function (index, elem) {
	//	if (elem.textContent == newId) {
	//		toggleSaveIcon(true, false, userId);
	//		alert('The employee ID "' + newId + '" is alreay in use.');
	//		goAhead = false;
	//		return;
	//	}
	//});
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
            toggleSaveIcon(true, false, userId);
		});
	}
}

function saveInvitationId(orgId, userId) {
    toggleSaveIcon(false, true, userId);
    var inp = $('#invIdedit-' + userId);
    var newId = inp.val();
    if (newId == inp.attr("data-oldval")) {
        cancelEditInvitationId(userId);
        toggleSaveIcon(false, false, userId);
        return;
    }
    //Check if id is already in use
    var goAhead = true;
    //$('.empId').each(function (index, elem) {
    //    if (elem.innerText == newId) {
    //        alert('The employee ID "' + newId + '" is alreay in use.');
    //        goAhead = false;
    //        toggleSaveIcon(false, false, userId);
    //        return;
    //    }
    //});
    if (goAhead) {
        var data = {
            user: userId,
            org: orgId,
            employeeId: newId
        };
        $.ajax({
            type: "post",
            url: "/Account/SaveInvitationEmployeeId/",
            data: data,
            timeout: 5000,
            dataType: "json"
        })
		.fail(function (res) {
		    if (res.responseText == "True") {
		        stopEditInvitationId(userId, newId);
		    } else {
		        alert('Failed to update employee ID.');
		        cancelEditInvitationId(userId);
		    }
		    toggleSaveIcon(false, false, userId);
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

function cancelEditInvitationId(userId) {
    var oldInvitationId = $('#invIdedit-' + userId).attr("data-oldval");
    stopEditInvitationId(userId, oldInvitationId)
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

function stopEditInvitationId(userId, newValue) {
    var inp = $('#invIdedit-' + userId);
    var orgId = inp.attr("data-org");
    var td = $('#invId-' + userId);
    td.empty();
    td.html(
		'<span class="empId">' + newValue + '</span>' +
		' <a href="javascript: void(0);" class="text-muted" title="Edit Employee Id"	onclick=\x27editInvitationId("' + orgId + '", "' + userId + '", "' + newValue + '")\x27><span class="fa fa-fw fa-edit"></span></a>'
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

function toggleSaveIcon(type, state, id){
    var _type;
    var _state;
    type == true ? _type = 'empIcon' : _type = 'invIcon';
    var node = $('#' + _type + id);
    if (state) {
        node.html("<span class='fa fa-refresh fa-spin'></span>");
    }
    else {
        node.html("<span class='fa fa-fw fa-save'></span>");
    }
}
