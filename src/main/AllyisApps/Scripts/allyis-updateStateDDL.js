$(document).ready(function () {
    if ($('#Country').val() != "") {
        updateStateDDL(function () {
            if (preserveState != "") {
                var ddl = document.getElementById('State');
                for (var i = 0; i < ddl.options.length; i++) {
                    if (ddl.options[i].text === preserveState) {
                        ddl.selectedIndex = i;
                    }
                }
            }
        });
    }

    $('#Country').change(updateStateDDL);
});

function updateStateDDL(successCallback) {
    if ($('#Country').val() != "") {
        $.ajax({
            type: "POST",
            url: "/Home/GetStates",
            data: JSON.stringify({ country: $('#Country').val() }),
            datatype: "json",
            contentType: "application/json",
            success: function (states) {
                debugger
                $('#State').empty();
                $('#State').append("<option value=\"\">" + dropdownempty + "</option>");
                Object.keys(states).forEach(function (key) {
                    $('#State').append("<option value=\"" + key + "\">" + states[key] + "</option>");
                });
                $('#State').prop("disabled", false);

                successCallback();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Error: " + errorThrown)
            }
        });
    } else {
        $('#State').empty();
        $('#State').append("<option value=\"\">" + dropdownempty + "</option>");
    }
}