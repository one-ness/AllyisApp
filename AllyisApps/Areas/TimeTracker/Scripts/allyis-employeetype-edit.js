﻿function moveSelectedProject(direction) {
	// Assuming only internally called with "in" or "out"
	var addList = direction === "in" ? document.getElementById("included-payclasses") : document.getElementById("excluded-payclasses");
	var removeList = direction === "in" ? document.getElementById("excluded-payclasses") : document.getElementById("included-payclasses");

	for (var i = 0; i < removeList.length;) {
		if (removeList[i].selected) {
			var foo = removeList[i];
			addList.add(new Option(foo.text, foo.value));
			removeList.remove(i);
		} else {
			i++;
		}
	}
};

var selectAll = function () {
	var items = $("#included-payclasses option");
	$("#included-payclasses option").prop("selected", "true"); // Select all users in CurrentPayClasses listbox for model binding
	return true;
};

var project_assign_module = function () {
	var modal;
	var left;
	var right;
	var leftInput;
	var rightInput;
	var leftSelect;
	var rightSelect;
	var actions;
	var actionMoveIn;
	var actionMoveOut;

	function init() {
		modal = $("#PayClassForm");
		actions = modal.find(".actions");
		actionMoveIn = actions.find(".move-in"); //?
		actionMoveOut = actions.find(".move-out"); //?
		left = modal.find(".swapper-ui .left");
		right = modal.find(".swapper-ui .right");
		leftSelect = left.find("select");
		rightSelect = right.find("select");
		leftInput = left.find("input"); // Left search bar
		rightInput = right.find("input"); // Right search bar

		// Changed parameters for filter and refilter at call to be .find() result
		// each time to ensure the search results reflected changes made in the modal
		leftInput.keyup(_.debounce(function () {
			filter(this.value.toLowerCase(), left.find("select option"));
		}, 250));
		rightInput.keyup(_.debounce(function () {
			filter(this.value.toLowerCase(), right.find("select option"));
		}, 250));
	}

	function refilter() {
		filter(leftInput.value.toLowerCase(), left.find("select option"));
		filter(rightInput.value.toLowerCase(), right.find("select option"));
	}

	// filters an element on a phrase
	function filter(text, what) {
		_.each(what, function (ele) {
			var $ele = $(ele);
			if (!$ele.text_lower) {
				$ele.text_lower = $ele.text().toLowerCase();
				if ($ele.attr("data-search"))
					$ele.text_lower += " " + $ele.attr("data-search").toLowerCase();
			}
			var val = $ele.text_lower;
			if (val.indexOf(text) > -1) {
				$ele.addClass("included").removeClass("excluded");
			} else {
				$ele.addClass("excluded").removeClass("included");
			}
		});
	}

	return {
		init: init,
		filter: filter,
		refilter: refilter
	};
}();

$(document).ready(function () {
	project_assign_module.init();
	$("#StartDate").singleDatePicker();
	$("#EndDate").singleDatePicker();
});

$.fn.singleDatePicker = function () {
	var datepickerElement = this;

	$(datepickerElement).on("apply.daterangepicker", function (e, picker) {
		picker.element.val(picker.startDate.format(picker.locale.format));
	});

	$(datepickerElement).siblings(".input-group-addon").click(function () {
		$(datepickerElement).click();
	});

	return $(datepickerElement).daterangepicker({
		singleDatePicker: true,
		showDropdowns: true,
		autoUpdateInput: false,
		drops: "up"
	});
};