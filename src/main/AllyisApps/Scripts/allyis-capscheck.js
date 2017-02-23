// A utility for attaching a caps-lock check to a text field (e.g. for password).

// To use, set the field's @onkeypress/@onkeydown html attributes to this function (capsCheck(event)).
// Also, include a hidden <strong> tag below it with id="capsWarningDiv". It's also good to use the capscheck text resource. Or more simply, copy the following line:
// <strong id="capsWarningDiv" style="visibility:hidden">@AllyisApps.Resources.Views.Account.Strings.CapsLock</strong>

function capsCheck(e) {
    kc = e.keyCode ? e.keyCode : e.which;
    sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if ((kc != 20 && kc < 65) || kc > 122) { return; }
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk)) {
        document.getElementById('capsWarningDiv').style.visibility = 'visible';
    }
    else document.getElementById('capsWarningDiv').style.visibility = 'hidden';
}
