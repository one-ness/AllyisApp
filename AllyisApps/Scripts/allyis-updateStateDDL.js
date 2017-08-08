//$(document).ready(function () {
//	if ($('#Country').val() != "") {
//		updateStateDDL(function () {
//			if (preserveState != "") {
//				var ddl = document.getElementById('State');
//				for (var i = 0; i < ddl.options.length; i++) {
//					if (ddl.options[i].text === preserveState) {
//						ddl.selectedIndex = i;
//					}
//				}
//			}
//		});
//	}

//	$('#Country').change(function () {
//		updateStateDDL(null);
//	});
//});

//function updateStateDDL(successCallback) {
//	if ($('#Country').val() != "") {
//		$.ajax({
//			type: "POST",
//			url: "/Home/GetStates",
//			data: JSON.stringify({ country: $('#Country').val() }),
//			datatype: "json",
//			contentType: "application/json",
//			success: function (states) {
//				$('#State').empty();
//				$('#State').append("<option value=\"\">" + dropdownempty + "</option>");
//				Object.keys(states).forEach(function (key) {
//					$('#State').append("<option value=\"" + key + "\">" + states[key] + "</option>");
//				});
//				$('#State').prop("disabled", false);

//				if (successCallback != null) successCallback();
//			},
//			error: function (XMLHttpRequest, textStatus, errorThrown) {
//				alert("Error: " + errorThrown)
//			}
//		});
//	} else {
//		$('#State').empty();
//		$('#State').append("<option value=\"\">" + dropdownempty + "</option>");
//	}
//}

// NOTE: for the update to work, ddlCountriesId, ddlStatesId, ddlStatesOptionsId should already defined in the containing file
$(document).ready(function () {
	var ddlCountriesObj = $("#" + ddlCountriesId);
	ddlCountriesObj.change(function () {
		updateStateDdl();
	});

	// populate states also on load
	updateStateDdl();
});

function updateStateDdl() {
	var ddlCountriesObj = $("#" + ddlCountriesId);
	var ddlStatesObj = $("#" + ddlStatesId);
	var countryCode = ddlCountriesObj.val();
	if (countryCode !== "") {
		var jsData = "{\"countryCode\":\"" + countryCode + "\"}";
		$.ajax({
			type: "POST",
			url: getStatesUrl,
			data: jsData,
			datatype: "json",
			contentType: "application/json",
			success: function (states) {
				// empty out the states dropdown, append all options from json, then sort alphabetically
				ddlStatesObj.empty();
				Object.keys(states).forEach(function (key) {
					ddlStatesObj.append("<option value=\"" + key + "\">" + states[key] + "</option>");
				});

				ddlStatesObj.html($("#" + ddlStatesOptionsId).sort(function (x, y) {
					return $(x).text() < $(y).text() ? -1 : 1;
				}));

				// prepend the first option
				ddlStatesObj.eq(0).prepend($("<option></option>").val("").text(dropdownempty));
				ddlStatesObj.prop("disabled", false);
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				alert("Error: " + errorThrown + "," + textStatus);
			}
		});
	} else {
		ddlStatesObj.empty();
		ddlStatesObj.append("<option value=\"\">" + dropdownempty + "</option>");
	}
}

