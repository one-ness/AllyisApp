//Clears validation error messages
$.fn.clearErrors = function () {
	$(this).each(function () {
		$(this).find(".field-validation-error").empty();
		$(this).trigger("reset.unobtrusiveValidation");
	});
};

function removeUser(orgId, userId, fullName, isInvite) {
	if (!confirm(removeFromOrg.replace("{0}", fullName))) return;

	var url = (isInvite ? removeInvitationAction : removeMemberAction) + userId + "&id=" + orgId;
	var token = $('[name="__RequestVerificationToken"]').val();
	var form = document.createElement("form");
	var input = document.createElement("input");

	form.setAttribute("method", "post");
	form.setAttribute("action", url);
	form.style.display = "none";
	input.type = "hidden";
	input.name = "__RequestVerificationToken";
	input.value = token;

	form.appendChild(input);
	document.body.appendChild(form);
	form.submit();
}