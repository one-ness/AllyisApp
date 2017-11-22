$(document).ready(function() {
	$("#customers-dropdown").change(updateProjectsDropdown);
	initializeDaterangePicker();
});

function updateProjectsDropdown() {
	var projectsDropdown = $("#projects-dropdown");
	var newCustId = this.value;
	if (newCustId === "0") {
		projectsDropdown.prop("disabled", true);
	} else {
		projectsDropdown.prop("disabled", false);
	}
	projectsDropdown.empty();
	$(".projectSelectOption").each(function () {
		var ele = $(this);
		var optionCust = ele.attr("data-cust");
		if (optionCust === "0" || optionCust === newCustId) {
			projectsDropdown.append(ele.clone());
		}
	});
};

function initializeDaterangePicker() {
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
		endDate: end,
		autoApply: true,
		ranges: {
			'Previous Pay Period': [prevStart, prevEnd],
			'Current Pay Period': [curStart, curEnd],
			'Next Pay Period': [nextStart, nextEnd],
			'Last 30 Days': [moment().subtract(29, 'days'), moment()],
			'This Month': [moment().startOf('month'), moment().endOf('month')],
			'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
		}
	}, cb);

	cb(start, end);
};

function submitFormForCSV() {
	$("#reportForm").attr("action", "/TimeTracker/TimeEntry/ExportReport");
	$("#reportForm").submit();
};