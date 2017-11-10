function submitFormForCSV() {
	$("#reportForm").attr("action", "/TimeTracker/TimeEntry/ExportReport");
	$("#reportForm").submit();
}

$(document).ready(function() {
	$("#ddl-customers").change(function () {
		var ddl_ele = $("#ddl-projects");
		var newCustId = this.value;
		if (newCustId == 0) {
			ddl_ele.prop("disabled", true);
		} else {
			ddl_ele.prop("disabled", false);
		}
		ddl_ele.empty();
		$(".projectSelectOption").each(function () {
			var ele = $(this);
			var optionCust = ele.attr("data-cust");
			if (optionCust == 0 || optionCust == newCustId) {
				ddl_ele.append(ele.clone());
			}
		});
	});

	//initialize date range picker
	$(function () {
		var start = moment(model_startdate);
		var end = moment(model_enddate);

		function cb(start, end) {
			$("#selection-range span").html(start.format("MMMM D, YYYY") + " - " + end.format("MMMM D, YYYY"));
			$("input[name=DateRangeStart]").val(start.format().slice(0, 10));
			$("input[name=DateRangeEnd]").val(end.format().slice(0, 10));
		}

		$("#selection-range").daterangepicker({
			locale: "us",
			showDropdowns: true,
			linkedCalendars: false,
			startDate: start,
			endDate: end
		}, cb);

		cb(start, end);
	});
});