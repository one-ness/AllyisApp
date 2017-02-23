// A shortcut script resource for setting up a date range picker

// Helper for converting between int form of date used for communication with server (see TimeTrackerService.GetDateTimeFromDays/GetDayFromDateTime)
// and moment.js dates
var DateConvert = (function () {
    var minimumDate = moment(-62135568000000) // This corresponds to DateTime.MinValue()

    return {
        GetDaysFromMoment: function (date) {
            return date.diff(minimumDate, 'days');
        },
        GetMomentFromDays: function (days) {
            // Shuffling of value necessary to preserve minimumDate, since the add method changes the moment object it's called from.
            oldval = moment(minimumDate);
            var sum = minimumDate.add(days, 'days');
            minimumDate = moment(oldval);
            return sum;
        }
    };
})();

function updateDates(control) {
    console.log("updating");
    var stringOutput = control.value.split("-->");
    document.getElementById("DateRangeStart").value = stringOutput[0];
    document.getElementById("DateRangeEnd").value = stringOutput[1];
}

/* Arguments:
 drpElementId - the input element to turn into a daterangepicker
 drpPresets - the presets to include along the lefthand menu
 startDateId - the hidden form input to set as the range start value
 endDateId - the hidden form input to set as the range end value (if applicable)
 numberOfMonths - 1 or 2; number of month panels to display
                - This value also controlls whether to pick one date, or two (for a range) - perhaps not good behavior, but these numbers matched in all use cases
 customChangeFunction - a function that will be executed everytime the date range picker apply button is pressed
 startInit - the initial start of the date range, in days since DateTime.MinValue
 endInit - the initial end of the date range (if applicable), in days since DateTime.MinValue
*/
function init_drp (drpElementId, drpPresets, startDateId, endDateId, numberOfMonths, customChangeFunction, startInit, endInit) {
    if (numberOfMonths == null) {
        numberOfMonths = 2;
    }
    if (customChangeFunction == null) {
        customChangeFunction = function () { };
    }
    var picker = $('#' + drpElementId).daterangepicker({
        initialText: 'Select period...',
        datepickerOptions: {
            minDate: null,
            maxDate: null,
            numberOfMonths: numberOfMonths
        },
        presetRanges: drpPresets,
        applyOnMenuSelect: true,
        onChange: function () {
            var range = picker.daterangepicker("getRange");
            $('#' + startDateId).val(DateConvert.GetDaysFromMoment(moment(range.start)));
            if (numberOfMonths > 1) {
                $('#' + endDateId).val(DateConvert.GetDaysFromMoment(moment(range.end)));
            }
            customChangeFunction();
        }
    });
    if (numberOfMonths == 1) {
        $('#' + drpElementId).daterangepicker("setRange", {
            start: DateConvert.GetMomentFromDays(startInit).toDate()
        });
    } else {
        $('#' + drpElementId).daterangepicker("setRange", {
            start: DateConvert.GetMomentFromDays(startInit).toDate(),
            end: DateConvert.GetMomentFromDays(endInit).toDate()
        });
    }
}
