function confirmIfFreeAndChanged() {
	element = $("input[type='radio'][name='SelectedSku']:checked");
	if (element.length <= 0) {
		return false;
	}
	if (!!element.attr('free') && element.val() != model_previousSku && model_previousSku != 0) {
		return confirm(warningWhenSwitchingToFreeSubscription);
	}
	return true;
}

function fixUserCount() {
	var count = $("#user-count").val();
	var result = parseInt(count) + 4 - ((count - 1) % 5);
	var currentUsers = parseInt($("#currentUserCount").val());
	if (result < currentUsers) {
		$("#notice").text(cannotReduceUserCount);
	} else {
		$("#notice").text("");
	}
	if (result > 500) result = 500; // Remove this once billing and pricing is fixed
	$("#user-count").val(result);
	var costOut = result - 500 > 0 ? result - 500 : 0;
	$("#sub-price").text("$" + costOut.toFixed(2));
	$("#SelectedSku").val(costOut == 0 ? 1 : 2);
}
