function clearMessagesAndEnableAdd() {
	$("#addMember").prop("disabled", false);
	$("#cannotAddMessage").hide();
	$("#noProjectMessage").hide();
	$("#roleRequiredMessage").hide();
	$("#projectRequiredMessage").hide();
}

function showNoProjectMessage(type) {
	$("#noProjectMessage").show();
	$("#cannotAddMessage").hide();
	$("#addMember").prop("disabled", true);
	if (type) { $("#roleRequiredMessage").show(); }
	if (!type) { $("#projectRequiredMessage").show(); }
}

function showMustBeOwnerOrUserMessage() {
	$("#noProjectMessage").hide();
	$("#cannotAddMessage").show();
	$("#addMember").prop("disabled", true);
}

function checkAddConditions() {
	//if ($('#AddAsOwner').is(':checked')) { // Adding as owner;
	if ($(".role-selector option:selected").val() === 1) { // Time Tracker role is User: must check for project
		if ($(".project-selector option:selected").val() === "") { // No project selected
			showNoProjectMessage(false);
		} else { // Project selected
			clearMessagesAndEnableAdd();
		}
	}
	else if ($(".project-selector option:selected").val() !== "") { //Project is selected
		if ($(".role-selector option:selected").val() === 0) {  // No role is selected
			showNoProjectMessage(true);
		}
		else { // Project selected
			clearMessagesAndEnableAdd();
		}
	}
	else { // Time Tracker role is None, just adding as owner
		clearMessagesAndEnableAdd();
	}
	//} else { // Not an owner
	//    if ($('.role-selector option:selected').val() == 0) { // Selected role is None
	//        showMustBeOwnerOrUserMessage();
	//    } else { // Role selected
	//        if ($('.project-selector option:selected').val() == "") { // No project selected
	//            showNoProjectMessage();
	//        } else { // Role and project selected
	//            clearMessagesAndEnableAdd();
	//        }
	//    }
	//}
}

$(document).ready(function () {
	$("#addMember").prop("disabled", true);
	$("#noProjectMessage").hide();

	$(".project-selector").change(function () {
		checkAddConditions();
	});

	$("#AddAsOwner").change(function () {
		checkAddConditions();
	});

	$(".role-selector").change(function () {
		checkAddConditions();
	});

	checkAddConditions();
});