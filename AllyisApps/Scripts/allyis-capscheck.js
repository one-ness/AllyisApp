// A utility for attaching a caps-lock check to a text field (e.g. for password).

// To use, put this script in the page
// Also, include a hidden <strong> tag below it with id="capsWarningDiv". It's also good to use the capscheck text resource. Or more simply, copy the following line:
// <strong id="capsWarningDiv" style="visibility:hidden">@AllyisApps.Resources.Views.Account.Strings.CapsLock</strong>

document.addEventListener("keydown", function (event) {
	document.msCapsLockWarningOff = true; //ie tries to give its own caps lock warning
	var caps = event.getModifierState && event.getModifierState("CapsLock");
	document.getElementById("capsWarningDiv").style.visibility = caps ? "visible" : "hidden";
});