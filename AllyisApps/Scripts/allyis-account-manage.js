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
    var type = "";
    var tag = "";
    var icon = "";
    isMember == true ? type = "#empId-" : type = "#invId-";
    isMember == true ? tag = "empIdedit-" : tag = "invIdedit-";
    isMember == true ? icon = "empIcon" : icon = "invIcon";
    var td = $(type + userId);
	td.empty();
	td.html(
		'<input id="' + tag + userId + '" type="text" class="employeeIdEdit" data-oldval="' + oldEmployeeId + '" data-org="' + orgId + '" value="' + oldEmployeeId + '" maxlength="16"/>' +
		' <a href="javascript: void(0);" id="empIcon'+ userId +'" class="text-muted" title="Save Changes" onclick=\x27saveEmployeeId(' + orgId + ',' + userId + ','+isMember+')\x27><span class="fa fa-fw fa-save"></span></a>' +
		'<a href="javascript: void(0);" class="text-muted" title="Cancel Changes" onclick=\x27cancelEditEmployeeId(' + userId + ',' + isMember +')\x27><span class="fa fa-fw fa-remove text-danger"></span></a>'
	);
}

function editEmployeeType(orgId, userId, oldEmployeeTypeId, isMember) {
    var type = "";
    var tag = "";
    var icon = "";
    isMember == true ? type = "#empType-" : type = "#invType-";
    isMember == true ? tag = "empTypeedit-" : tag = "invTypeedit-";
    isMember == true ? icon = "empIcon" : icon = "invIcon";
    var td = $(type + userId);
    td.empty();
    td.html(
		'<div>' +
        '<select id="' + tag + userId + '" class="employeeTypeEdit" data-oldval="' + oldEmployeeTypeId + '" data-org="' + orgId + '">' +
        '<option value=1>Salaried</option>' + 
        '<option value=2>Hourly</option></select>' +
		' <a href="javascript: void(0);" id="empIcon' + userId + '" class="text-muted" title="Save Changes" onclick=\x27saveEmployeeType(' + orgId + ',' + userId + ',' + isMember + ')\x27><span class="fa fa-fw fa-save"></span></a>' +
		'<a href="javascript: void(0);" class="text-muted" title="Cancel Changes" onclick=\x27cancelEditEmployeeType(' + userId + ',' + isMember + ')\x27><span class="fa fa-fw fa-remove text-danger"></span></a>' +
        '</div>'
	);
    document.getElementById(tag + userId).value = oldEmployeeTypeId; //sets selected to the employee's current type
}

function saveEmployeeId(orgId, userId, isMember) {
    var tag;
    var tag3;
    isMember == true ? tag = "#empIdedit-" : tag = "#invIdedit-";
    toggleSaveIcon(isMember, true, userId);
	var inp = $(tag + userId);
	var newId = inp.val();
	if (newId == inp.attr("data-oldval")) {
	    cancelEditEmployeeId(userId,isMember);
	    toggleSaveIcon(isMember, false, userId);
		return;
	}
	//Check if id already in use
	var goAhead = true;
	$(".empId").each(function (index, elem) {
		if (elem.textContent == newId) {
			toggleSaveIcon(isMember, false, userId);
			alert('The employee ID "' + newId + '" is already in use.');
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
		if (isMember)
        {
		    $.ajax({
		        type: "post",
		        url: "/Account/SaveEmployeeId/",
		        data: data,
		        timeout: 5000,
		        dataType: "json",
		    })
		    .fail(function (res) {
		        console.log(res);
		        if (res.responseText == "True") {
		            stopEditEmployeeId(userId, newId, isMember);
                    toggleSaveIcon(isMember, false, userId);
		            return;
		        }
		        if (res.responseText == "False") {
                    toggleSaveIcon(isMember, false, userId);
		            alert("The employee ID " + newId + " is already in use.");
		            return;
		        }
		        else {
		    		alert('Failed to update employee ID.');
		    		cancelEditEmployeeId(userId,isMember);
		    		toggleSaveIcon(isMember, false, userId);
		        }
		    });
		}
		if (!isMember) {
		    $.ajax({
		        type: "post",
		        url: "/Account/SaveInvitationEmployeeId/",
		        data: data,
		        timeout: 5000,
		        dataType: "json"
		    })
		    .fail(function (res) {
		        console.log(res);
		        if (res.responseText == "True") {
		            stopEditEmployeeId(userId, newId, isMember);
		            toggleSaveIcon(isMember, false, userId);
		            return;
		        }
		        if (res.responseText == "False") {
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
}

function saveEmployeeType(orgId, userId, isMember) {
    var tag;
    var tag3;
    isMember == true ? tag = "#empTypeedit-" : tag = "#invTypeedit-";
    toggleSaveIcon(isMember, true, userId);
    var inp = $(tag + userId);
    var newId = inp.val();
    if (newId == inp.attr("data-oldval")) {
        cancelEditEmployeeType(userId, isMember);
        toggleSaveIcon(isMember, false, userId);
        return;
    }

    var data = {
        user: userId,
        org: orgId,
        employeeTypeId: newId
    };
    if (isMember) {
        $.ajax({
            type: "post",
            url: "/Account/SaveEmployeeTypeId/",
            data: data,
            timeout: 5000,
            dataType: "json",
        })
		.fail(function (res) {
		    console.log(res);
		    if (res.responseText == "True") {
		        stopEditEmployeeType(userId, newId, isMember);
		        toggleSaveIcon(isMember, false, userId);
		        return;
		    }
		    else {
		        alert('Failed to update employee type.');
		        cancelEditEmployeeType(userId, isMember);
		        toggleSaveIcon(isMember, false, userId);
		    }
		});
    }
    if (!isMember) {
        $.ajax({
            type: "post",
            url: "/Account/SaveInvitationEmployeeTypeId/",
            data: data,
            timeout: 5000,
            dataType: "json"
        })
		.fail(function (res) {
		    console.log(res);
		    if (res.responseText == "True") {
		        stopEditEmployeeType(userId, newId, isMember);
		        toggleSaveIcon(isMember, false, userId);
		        return;
		    }
		    else {
		        alert('Failed to update employee type.');
		        cancelEditEmployeeType(userId, isMember);
		        toggleSaveIcon(isMember, false, userId);
		    }
		});
    }
}

function cancelEditEmployeeId(userId, isMember) {
    var tag;
    isMember == true? tag = "#empIdedit-" : tag = "#invIdedit-";
	//var inp = $('#empIdedit-' + userId);
	//var oldEmployeeId = inp.attr("data-oldval");
	//var orgId = inp.attr("data-org");
	//var td = $('#empId-' + userId);
	//td.empty();
	//td.html(
	//	'<span class="empId">' + oldEmployeeId + '</span>' +
	//	' <a href="javascript: void(0);" class="text-muted" title="Edit Employee Id"	onclick=\x27editEmployeeId("' + orgId + '", "' + userId + '", "' + oldEmployeeId + '")\x27><span class="fa fa-fw fa-edit"></span></a>'
	//);
    var oldEmployeeId = $(tag + userId).attr("data-oldval");
    stopEditEmployeeId(userId, oldEmployeeId, isMember);
}

function cancelEditEmployeeType(userId, isMember) {
    var tag;
    isMember == true ? tag = "#empTypeedit-" : tag = "#invTypeedit-";
    var oldEmployeeTypeId = $(tag + userId).attr("data-oldval");
    stopEditEmployeeType(userId, oldEmployeeTypeId, isMember);
}

function stopEditEmployeeId(userId, newValue, isMember) {
    var tag1 = ""
    var tag2 = ""
    isMember == true ? tag1 = "#empIdedit-" : tag1 = "#invIdedit-";
    isMember == true ? tag2 = "#empId-" : tag2 = "#invId-";
	var inp = $(tag1 + userId);
	var orgId = inp.attr("data-org");
	var td = $(tag2 + userId);
	td.empty();
	td.html(
		'<span class="empId">' + newValue + '</span>' +
		' <a href="javascript: void(0);" class="text-muted" title="Edit Employee Id"	onclick=\x27editEmployeeId(' + orgId + ',' + userId + ',"' + newValue + '",'+ isMember + ')\x27><span class="fa fa-fw fa-edit"></span></a>'
	);
}

function stopEditEmployeeType(userId, newTypeId, isMember) {
    var tag1 = ""
    var tag2 = ""
    isMember == true ? tag1 = "#empTypeedit-" : tag1 = "#invTypeedit-";
    isMember == true ? tag2 = "#empType-" : tag2 = "#invType-";
    var inp = $(tag1 + userId);
    var orgId = inp.attr("data-org");
    var td = $(tag2 + userId);
    var newType = (newTypeId == 1 ? "Salaried" : (newTypeId == 2 ? "Hourly" : "N/A"));
    td.empty();
    td.html(
		'<span class="empType">' + newType + '</span>' +
		' <a href="javascript: void(0);" class="text-muted" title="Edit Employee Type"	onclick=\x27editEmployeeType(' + orgId + ',' + userId + ',"' + newTypeId + '",' + isMember + ')\x27><span class="fa fa-fw fa-edit"></span></a>'
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
        var url = removeInvitationAction + invId;

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
