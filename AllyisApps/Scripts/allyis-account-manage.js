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
function editEmployeeId(orgId, userId, oldEmployeeId, isMember) {
	var tag  = isMember ? "empIdedit-" : "invIdedit-";
	var icon = isMember ? "empIcon" : "invIcon";
	$((isMember ? "#empId-" : "#invId-") + userId).html(
		'<input id="' + tag + userId + '" type="text" class="employeeIdEdit" data-oldval="' + oldEmployeeId + '" data-org="' + orgId + '" value="' + oldEmployeeId + '" maxlength="16"/>' +
		' <a href="javascript: void(0);" id="empIcon' + userId + '" class="text-muted" title="Save Changes" onclick=\x27saveEmployeeId(' + orgId + ',' + userId + ',' + isMember + ')\x27><span class="fa fa-fw fa-save"></span></a>' +
		'<a href="javascript: void(0);" class="text-muted" title="Cancel Changes" onclick=\x27cancelEditEmployeeId(' + userId + ',' + isMember + ')\x27><span class="fa fa-fw fa-remove text-danger"></span></a>'
	);
}

function editEmployeeType(orgId, userId, oldEmployeeTypeId, isMember, salaried, hourly) {
	var tag = isMember ? "empTypeedit-" : "invTypeedit-";
	var icon = isMember ? "empIcon" : "invIcon";
	$((isMember ? "#empType-" : "#invType-") + userId).html(
		'<div>' +
		'<select id="' + tag + userId + '" class="employeeTypeEdit" data-oldval="' + oldEmployeeTypeId + '" data-org="' + orgId + '">' +
		'<option value=1>' + salaried + '</option>' +
		'<option value=2>' + hourly + '</option></select>' +
		' <a href="javascript: void(0);" id="empIcon' + userId + '" class="text-muted" title="Save Changes" onclick=\x27saveEmployeeType(' + orgId + ',' + userId + ',' + isMember + ', "' + salaried + '", "' + hourly + '") \x27 > <span class="fa fa-fw fa-save"></span></a > ' +
		'<a href="javascript: void(0);" class="text-muted" title="Cancel Changes" onclick=\x27cancelEditEmployeeType(' + userId + ',' + isMember + ', "' + salaried + '", "' + hourly + '")\x27><span class="fa fa-fw fa-remove text-danger"></span></a>' +
		'</div>'
	);
	document.getElementById(tag + userId).value = oldEmployeeTypeId; //sets selected to the employee's current type
}

function saveEmployeeId(orgId, userId, isMember) {
	toggleSaveIcon(isMember, true, userId);
	var inp = $((isMember ? "#empIdedit-" : "#invIdedit-") + userId);
	var newId = inp.val();
	if (newId == inp.attr("data-oldval")) {
		cancelEditEmployeeId(userId, isMember);
		toggleSaveIcon(isMember, false, userId);
		return;
	}
	//Check if id already in use
	var idInUse = false;
	$(".empId").each(function (index, elem) {
		if (elem.textContent === newId) {
			toggleSaveIcon(isMember, false, userId);
			alert('The employee ID "' + newId + '" is already in use.');
			idInUse = true;
			return;
		}
	});
	if (idInUse) {
		$.ajax({
			type: "post",
			url: isMember ? "/Account/SaveEmployeeId/" : "/Account/SaveInvitationEmployeeId/",
			data: { user: userId, org: orgId, employeeId: newId },
			timeout: 5000,
			dataType: "json"
		}).fail(function (res) {
			console.log(res);
			if (res.responseText === "True") {
				stopEditEmployeeId(userId, newId, isMember);
				toggleSaveIcon(isMember, false, userId);
				return;
			}
			if (res.responseText === "False") {
				toggleSaveIcon(isMember, false, userId);
				alert("The employee ID " + newId + " is already in use.");
				return;
			}
			else {
				alert('Failed to update employee ID.');
				cancelEditEmployeeId(userId, isMember);
				toggleSaveIcon(isMember, false, userId);
			}
		});
	}
}

function saveEmployeeType(orgId, userId, isMember, salaried, hourly) {
	toggleSaveIcon(isMember, true, userId);
	var inp = $((isMember ? "#empTypeedit-" : "#invTypeedit-") + userId);
	var newId = inp.val();
	if (newId == inp.attr("data-oldval")) {
		cancelEditEmployeeType(userId, isMember);
		toggleSaveIcon(isMember, false, userId);
		return;
	}
	
	$.ajax({
		type: "post",
		url: isMember ? "/Account/SaveEmployeeTypeId/" : "/Account/SaveInvitationEmployeeTypeId/",
		data: {	user: userId, org: orgId, employeeTypeId: newId	},
		timeout: 5000,
		dataType: "json"
	}).fail(function (res) {
		console.log(res);
		if (res.responseText === "True") {
			stopEditEmployeeType(userId, newId, isMember, salaried, hourly);
			toggleSaveIcon(isMember, false, userId);
			return;
		}
		else {
			alert('Failed to update employee type.');
			cancelEditEmployeeType(userId, isMember, salaried, hourly);
			toggleSaveIcon(isMember, false, userId);
		}
	});
}

function cancelEditEmployeeId(userId, isMember) {
	var oldEmployeeId = $((isMember ? "#empIdedit-" : "#invIdedit-") + userId).attr("data-oldval");
	stopEditEmployeeId(userId, oldEmployeeId, isMember);
}

function cancelEditEmployeeType(userId, isMember, salaried, hourly) {
	var oldEmployeeType = $((isMember ? "#empTypeedit-" : "#invTypeedit-") + userId).attr("data-oldval");
	stopEditEmployeeType(userId, oldEmployeeType, isMember, salaried, hourly);
}

function stopEditEmployeeId(userId, newValue, isMember) {
	var orgId = $((isMember ? "#empIdedit-" : "#invIdedit-") + userId).attr("data-org");
	$((isMember ? "#empId-" : "#invId-") + userId).html(
		'<span class="empId">' + newValue + '</span>' +
		' <a href="javascript: void(0);" class="text-muted" title="Edit Employee Id"	onclick=\x27editEmployeeId(' + orgId + ',' + userId + ',"' + newValue + '",' + isMember + ')\x27><span class="fa fa-fw fa-edit"></span></a>'
	);
}

function stopEditEmployeeType(userId, newTypeId, isMember, salaried, hourly) {
	var orgId = $((isMember ? "#empTypeedit-" : "#invTypeedit-") + userId).attr("data-org");
	var newType = (newTypeId == 1 ? salaried : newTypeId == 2 ? hourly : "N/A");
	$((isMember ? "#empType-" : "#invType-") + userId).html(
		'<span class="empType">' + newType + '</span>' +
		' <a href="javascript: void(0);" class="text-muted" title="Edit Employee Type"	onclick=\x27editEmployeeType(' + orgId + ',' + userId + ',"' + newTypeId + '",' + isMember + ', "' + salaried + '", "' + hourly + '")\x27><span class="fa fa-fw fa-edit"></span></a > '
	);
}

function removeUser(orgId, userId, fullName) {
	var result = confirm(removeFromOrg.replace('{0}', fullName));
	if (result == true) {
		var url = removeMemberAction + userId + "&id=" + orgId;

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

		document.body.appendChild(form);
		form.submit();
	}
}

function deleteInvitation(orgId, invId) {
	var result = confirm(removeInvitation);
	if (result) {
		var url = removeInvitationAction + invId + "&Id=" + orgId;

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
		document.body.appendChild(form);
		form.submit();
	}
}

function toggleSaveIcon(isMember, state, id) {
	var node = $('#' + (isMember ? 'empIcon' : 'invIcon') + id);
	if (state) {
		node.html("<span class='fa fa-refresh fa-spin'></span>");
	}
	else {
		node.html("<span class='fa fa-fw fa-save'></span>");
	}
}
