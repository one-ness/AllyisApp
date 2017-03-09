function clearMessagesAndEnableAdd() {
	$('#addMember').prop('disabled', false);
	$('#cannotAddMessage').hide();
	$('#noProjectMessage').hide();
}

function showNoProjectMessage() {
	$('#noProjectMessage').show();
	$('#cannotAddMessage').hide();
	$('#addMember').prop('disabled', true);
}

function showMustBeOwnerOrUserMessage() {
	$('#noProjectMessage').hide();
	$('#cannotAddMessage').show();
	$('#addMember').prop('disabled', true);
}

function checkAddConditions() {
	//if ($('#AddAsOwner').is(':checked')) { // Adding as owner;
	if ($('.role-selector option:selected').val() == 1) { // Time Tracker role is User: must check for project
		if ($('.project-selector option:selected').val() == "") { // No project selected
			showNoProjectMessage();
		} else { // Project selected
			clearMessagesAndEnableAdd();
		}
	} else { // Time Tracker role is None, just adding as owner
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
	$('#addMember').prop('disabled', true);
	$('#noProjectMessage').hide();

	$('.project-selector').change(function () {
		checkAddConditions();
	});

	$('#AddAsOwner').change(function () {
		checkAddConditions();
	});

	$('.role-selector').change(function () {
		checkAddConditions();
	});

	checkAddConditions();
});
