// Gathers the data to send to the ManagePermissions action. Data must be structered as
// a UserPermissionsAction object (in services).
gatherData = function () {
	var checked_users = [];

	// Go through each row and add it to the data if it's checked
	$('.userRow').each(function () {
		var userEle = $(this);
		if (userEle.find("#check")[0].checked && !userEle.find("#check")[0].disabled) {
			var userResult = {};
			userResult.userid = userEle.attr("data-id");
			userResult.name = userEle.attr("data-name");
			checked_users.push(userResult);
		}
	});
	if (checked_users.length === 0) return null;

	var value = -2;

	value = $('#ActionSelect').val();
	if (value === -1) {
		if (!confirm(confirmMessage)) return null;
	}
	var selectedAction = value;

	// Assemble and return UserPermissionsAction object of data
	result = {};
	result.SelectedUsers = checked_users;
	result.SelectedAction = selectedAction;
	result.OrganizationId = $("#OrganizationId").val();
	result.SubscriptionId = $("#SubscriptionId").val();
	result.FromUrl = FromUrl
	result.ProductId = $("#ProductId").val();
	result.isPermissions2 = true; // Delete this once there's only one permissions management page
	return result;
}

// Creates a form element and submits it using data gathered from the page's state
formSubmit = function () {
	var form = $(document.createElement("form")).attr({ "method": "POST", "action": formUrl });
	var data = gatherData();
	if (!data) { // gatherData() will return null if no one is checked or the user declines the confirmation prompt.
		return;
	}
	var datajson = JSON.stringify(data);

	$(document.createElement("input")).attr({ "type": "hidden", "name": "data", "value": datajson }).appendTo(form);
	form.appendTo(document.body).submit();
}

$(document).ready(function () {
	$('#do-it').on("click", function () {
		formSubmit();
	});
});