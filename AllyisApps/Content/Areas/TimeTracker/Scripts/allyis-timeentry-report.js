//// Date range picker
//init_drp(
//    "daterange",
//    [{
//        text: "Today",
//        dateStart: function () { return moment() },
//        dateEnd: function () { return moment() }
//    }, {
//        text: "Yesterday",
//        dateStart: function () { return moment().subtract(1, 'days') },
//        dateEnd: function () { return moment().subtract(1, 'days') }
//    }, {
//        text: "Last Week",
//        dateStart: function () {
//            weekStart = 0 + model_startofweek;
//            if (weekStart <= moment().day())
//                return (moment().day(weekStart - 7));
//            else
//                return (moment().day(weekStart - 14));
//        },
//        dateEnd: function () {
//            weekStart = 0 + model_startofweek;
//            if (weekStart <= moment().day())
//                return (moment().day(weekStart - 1));
//            else
//                return (moment().day(weekStart - 8));
//        }
//    }, {
//        text: "This Month",
//        dateStart: function () { return moment().startOf('month') },
//        dateEnd: function () { return moment().endOf('month') }
//    }, {
//        text: "Last Month",
//        dateStart: function () { return moment().subtract(1, 'month').startOf('month') },
//        dateEnd: function () { return moment().subtract(1, 'month').endOf('month') }
//    }, {
//        text: "This quarter",
//        dateStart: function () { return moment().startOf('quarter') },
//        dateEnd: function () { return moment().endOf('quarter') }
//    }, {
//        text: "Last quarter",
//        dateStart: function () { return moment().subtract(1, 'quarter').startOf('quarter') },
//        dateEnd: function () { return moment().subtract(1, 'quarter').endOf('quarter') }
//    }],
//    "DateRangeStart",
//    "DateRangeEnd",
//    2,
//    null,
//    model_startdate,
//    model_enddate
//);

function submitFormForCSV() {
    $("#reportForm").attr("action", "/TimeTracker/TimeEntry/ExportReport");
    $("#reportForm").submit();
}

$('#datePickerStart').datepicker({
    showOn: "button",
    buttonImageOnly: true,
    onSelect: function () {
        $(this).change();
    }
});
$('#datePickerStart').on("change", function () {
    var enteredDate = moment($(this).val());
    if (enteredDate.year() < 2000) {
        enteredDate.add(100, 'years');
    }
    $('#DateRangeStart').val(DateConvert.GetDaysFromMoment(enteredDate));
});
$('#datePickerEnd').datepicker({
    showOn: "button",
    buttonImageOnly: true,
    onSelect: function () {
        $(this).change();
    }
});
$('#datePickerEnd').on("change", function () {
    var enteredDate = moment($(this).val());
    if (enteredDate.year() < 2000) {
        enteredDate.add(100, 'years');
    }
    $('#DateRangeEnd').val(DateConvert.GetDaysFromMoment(enteredDate));
});

// $( document ).ready()
$(function () {
	$("#ddl-customers").change(function () {
		var ddl_ele = $('#ddl-projects');
		var newCustId = this.value;
		if (newCustId == 0) {
			ddl_ele.prop('disabled', true);
		} else {
			ddl_ele.prop('disabled', false);
		}
		ddl_ele.empty();
		$('.projectSelectOption').each(function () {
			var ele = $(this);
			var optionCust = ele.attr("data-cust");
			if (optionCust == 0 || optionCust == newCustId) {
				ddl_ele.append(ele.clone());
			}
		});

        //console.log("Sending: " + this.value);
        //$.ajax({
        //    url: "/TimeTracker/Project/GetProjects?customerId=" + this.value,
        //    method: "POST",
        //    datatype: JSON,
        //    success: function (data) {
        //        console.log(data);
        //        var ddl = $("#ddl-projects");
        //        ddl.empty();
        //        ddl.prop('disabled', true);
        //        if (!$.isEmptyObject(data)) {
        //            ddl.append($("<option />").val(0).text("No Filter"));
        //            $.each(data, function () {
        //                ddl.append($("<option />").val(this.ProjectId).text(this.Name));
        //            });
        //            ddl.prop('disabled', false);
        //        }
        //    },
        //    failure: $(function () {
        //        console.log("fail");
        //        $("#ddl-projects").empty();
        //        $("#ddl-projects").prop('disabled', true);
        //    })
        //});
    });

    $('#datePickerStart').datepicker("setDate", DateConvert.GetMomentFromDays(model_startdate).toDate());
    $('#datePickerEnd').datepicker("setDate", DateConvert.GetMomentFromDays(model_enddate).toDate());
    $('.ui-datepicker-trigger').remove();
    $('#datePickerStartButton').on("click", function () {
        $('#datePickerStart').datepicker("show");
    });
    $('#datePickerEndButton').on("click", function () {
        $('#datePickerEnd').datepicker("show");
    });
});
