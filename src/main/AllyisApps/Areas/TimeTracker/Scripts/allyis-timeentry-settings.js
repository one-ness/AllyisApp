function deletePayClass(payClassId, payClassName) {
	if (confirm(deletePayClassWarning.replace('{0}', payClassName))) {
		window.location = deletePayClassUrl.replace('-1', payClassId);
	}
}

function deleteHoliday(holidayId, holidayName) {
	if (confirm(deleteHolidayWarning.replace('{0}', holidayName))) {
		window.location = deleteHolidayUrl.replace('-1', holidayId);
	}
}

function hideOT(hideIt) {
	$('#OTSettings').css('visibility', hideIt ? 'hidden' : 'visible');
}
function disableLD(isDisabling) {
}

$(document).ready(function () {
	console.log("script loaded");
});
