// Gathers the data to send to the ManagePermissions action. Data must be structered as
// a UserPermissionsAction object (in services).
gatherData = function () {
	var checked_users = [];

	// Go through each row and add it to the data if it's checked
	$('.userRow').each(function () {
		var userEle = $(this);
		if (userEle.find("#check")[0].checked) {
			var userResult = {};
			userResult.userid = userEle.attr("data-id");
			userResult.name = userEle.attr("data-name");
			checked_users.push(userResult);
		}
	});
	if (checked_users.length === 0) return null;

	// Add in target action information
	var selectedAction = {};
	var value = -2;
	if (currentTabTitle === "OrganizationTab") {
		value = orgActions[$('#orgActionSelect').val()];
		if (value === -1) {
			if (!confirm(confirmMessage)) return null;
		}
		selectedAction["OrganizationRoleTarget"] = value;
	} else if (currentTabTitle === "TimeTrackerTab") {
		if (currentTabTitle === "TimeTrackerTab") {
			value = ttActions[$('#ttActionSelect').val()];
			selectedAction["TimeTrackerRoleTarget"] = value;
		}
	} else {
		if (currentTabTitle === "ExpenseTrackerTab") {
			value = etActions[$('#etActionSelect').val()];
			selectedAction["ExpenseTrackerRoleTarget"] = value;
		}
	}

	// Assemble and return UserPermissionsAction object of data
	result = {};
	result.SelectedUsers = checked_users;
	result.SelectedActions = selectedAction;
	result.OrganizationId = $("#OrganizationId").val();

	result.isPermissions2 = true; // Delete this once there's only one permissions management page

	return result;
}

// Creates a form element and submits it using data gathered from the page's state
formSubmit = function () {
	var form = $(document.createElement("form")).attr({ "method": "POST", "action": "ManagePermissions" });
	var data = gatherData();
	if (!data) { // gatherData() will return null if no one is checked or the user declines the confirmation prompt.
		return;
	}
	var datajson = JSON.stringify(data);

	$(document.createElement("input")).attr({ "type": "hidden", "name": "data", "value": datajson }).appendTo(form);
	form.appendTo(document.body).submit();
}

// Tabs
var currentTabTitle = "OrganizationTab";
function goToTab(tabTitle) {
	$('#' + currentTabTitle).toggleClass("selected", false);
	$('.tab-' + currentTabTitle).hide();
	currentTabTitle = tabTitle;
	$('#' + currentTabTitle).toggleClass("selected", true);
	$('.tab-' + currentTabTitle).show();
	// update session storage with currently selected tab
	sessionStorage.setItem("Tab", tabTitle);
}
$(document).ready(function () {
	$('#do-it').on("click", function () {
		formSubmit();
	});

	// Tabs
	$('.allyis-tabs > li').on("click", function () {
		var tabTitle = $(this).attr("id");
		if (currentTabTitle !== tabTitle) {
			goToTab(tabTitle);
		}
	})

	// If a tab was selected previously, switch to that tab
	if (typeof ($("#TimeTrackerTab")[0]) != "undefined") {
		if (sessionStorage.getItem("Tab") != null) {
			goToTab(sessionStorage.getItem("Tab"));
		}
	}
});