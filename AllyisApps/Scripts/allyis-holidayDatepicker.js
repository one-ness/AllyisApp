// This is a small script to format the date picker.
//
// Currently (3/27/2017) the date picker is used on the "Account/Register"
// and "Account/EditProfile" pages on only the Edge browser to replace the
// default date picker for the "Date of Birth" field.
//
// Minimum for Usage:
// in "Scripts" section in .cshtml:
// <script scr="~/Scripts/allyis-birthDatepicker.js" ></script>
//
// in code in .cshtml:
//    The important parts of both use cases is that the "id" attribute is
//    "datePickerBirth" to access the jquery script, and the type is
//    set to "text" for the jquery-ui script to work.
// edit:
//    @Html.EditorFor(m => m.A_Date_Field, new { htmlattributes = new { @id = "datePickerBirth", @type = "text" } })
// Register:
//    <input id="datePickerBirth" type="text">

$("#datePickerHoliday").datepicker({
	showOn: "button",
	buttonImageOnly: true,
	changeMonth: true,
	changeYear: true,
	yearRange: "-120:+0",
	onSelect: function () {
		$(this).change();
	}
});
$('#datePickerHoliday').on("change", function () {
	var enteredDate = moment($(this).val());
	$('#DateOfHoliday').val(DateConvert.GetDaysFromMoment(enteredDate));
});

$(function () {
	var initHoliday = DateConvert.GetMomentFromDays(model_holiday);
    if (!isNaN(initHoliday)) {
        $('#datePickerHoliday').datepicker("setDate", initHoliday.toDate());
	}
	$('.ui-datepicker-trigger').remove();
	$('#datePickerHolidayButton').on("click", function () {
		$('#datePickerHoliday').datepicker("show");
	});
});

//the following came from Areas\TimeTracker\Scripts\allyis-drp-init.js

// Helper for converting between int form of date used for communication with server (see Service.GetDateTimeFromDays/GetDayFromDateTime)
// and moment.js dates
var DateConvert = (function () {
	var minimumDate = moment(-62135568000000) // This corresponds to DateTime.MinValue()

	return {
		GetDaysFromMoment: function (date) {
			if (isNaN(date)) {
				return -1;
			}
			return date.diff(minimumDate, 'days');
		},
		GetMomentFromDays: function (days) {
			if (days < 0) {
				return Number.NaN;
			}

			// Shuffling of value necessary to preserve minimumDate, since the add method changes the moment object it's called from.
			oldval = moment(minimumDate);
			var sum = minimumDate.add(days, 'days');
			minimumDate = moment(oldval);
			return sum;
		}
	};
})();
