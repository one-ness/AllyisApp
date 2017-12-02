/*
allyis-pages-with-filter.js
Utility for providing a table of rows that can be filtered and paginated in tandem.

To use:
    In your html, provide each of the filterable rows with some class (e.g. class="filter-row"). Give an identifying id or class
to each item within the row that you will use to match to some filter. By default, this module uses the class "pwfRow". If you would
like to include check boxes in the filtering (i.e. any checked rows will remain visible even when they don't match the filters), then
you should also have an id or class for them (the default is the class "pwfCheck").
    You should also include a final row that simply has the id "tableEnd". This will produce a bottom border under the last row of
data, and be used as a reference for adding filler rows.
    At the bottom (or top, or wherever), include a div with class="pageContainer" - this will hold the page buttons (they will
auto-populate; all you need is the empty div).
    Filters can be text boxes or select inputs (drop-down menus). Make sure each filter you will use has an id or unique class to
identify it by.
    Make sure you import the 'allyis-pages-with-filter.less' LESS file in your page's LESS file. (Note: remember to add an import
reference to your page's LESS at the top of 'allyis-pages-with-filter.less' when you do).

    Put a script tag in your view after this script. Then, use the pwf.addFilter method to define what filters you have and what they act
on. This is the syntax:
    pwf.addFilter(filterElementSelector, filterType, rowFilteredPropertySelector, noneValue);
    - filterElementSelector - a css selector for the id/class of the filter (e.g. '#nameFilter')
    - filterType - this must be either "search" for a text box, or "dropDown" for a select element
    - rowFilteredPropertySelector - a css selector for the id/class of the item within each row that this filter is applied to (e.g. '#rowUserName')
    - noneValue - (optional) for dropDown types, this is the text of the 'neutral' value (e.g. "Select...", "Any", "None", etc.)
    That should set up your filters. If you want to include an 'all-check' check-box, simply make sure the checkbox input element
has class="all-check". This checkbox will have a darker outline, and checking/unchecking it will check/uncheck all visible rows.
    Use these methods to tell the module what class/id/other selector to use for the rows and checkboxes, if they are different than
the defaults:
    pwf.setRowClass(newRowClass);
    pwf.setCheckBoxSelector(newCheckBoxSelector); - If you aren't using check boxes, there's no need to set this to anything.
Also, you can change the internal page limit. The default is 16 rows per page. The set value can be any postive number.
    pwf.setPageLimit(newPageLimit);

A few notes on filler rows:
    One issue faced by paging/filtering is that whenever the number of displayed rows jumps from a higher number to a lower
number, the height of the table changes. If it's at the bottom of your page, then your scroll position will also jump. This can
be very annoying when you switch between the last page and previous pages, or when you filter down the list to one or two rows and then
clear the filter.
    To keep the table the same constant height, filler rows are added at the end of the list to fill up the last page. On each call
of the filter rows event (when the pages are recalculated), the method clears all filler rows, figures out how many are now needed,
and adds them.
    This isn't enough, though. The number of rows still jumps for an instant down before filling back up, so your scroll position
still gets jumped upward. So, in addition to the filler rows, there is 1 full page of 'scrollBuffer' rows. They are just like filler
rows, except they remain in place always. They are usually not displayed. When a filter event occurs, they're display is turned on, extending
the table to double its height. Then the filtering/paging is done, and the filler rows created. Then, before you have time
to notice them, the buffer rows' display is turned off again. And your scroll position is unaffected.
*/
(function (exports) {
	// Paging variables
	var pageLimit = 16; // Rows per page
	var pageButtonLimit = 5; // Max page buttons to display at once
	var currentPageButtonStart = 1; // The first displayed page button
	var pageContainer = $('.pageContainer'); // Place a div on your page with the class pageContainer - it will auto-populate with page buttons
	var totalPages = 1; // Page count, recalculated on each filter
	var anchorRow = null; // This is the row element that is used to decide what page to show when pagination changes
	var maxPageButtonStart = 0;
	var previouslySelectedPage = 0;

	// Filtering variables
	var rowClass = "pwfRow"; // class for all row elements
	var fillerClass = "pwfFillerRow"; // class for the blank filler rows at the end
	var checkBoxSelector = ".pwfCheck"; // css selector to find check boxes within rows. Leave null if you are not using checkboxes.
	var hasCheckBoxes = false; // Whether the check box selector actually was found on the page on load.
	var filters = []; // Data sructure to store filters in use and their types
	var scrollBufferCreated = false;

	// Mutators for variables
	exports.setPageLimit = function (newPageLimit) {
		if (newPageLimit > 0) {
			pageLimit = newPageLimit;
		} else {
			console.log("pwf - Error setting page limit: Page limit must be greater than 0.");
		}
	}
	exports.setPageButtonLimit = function (newPageButtonLimit) {
		if (newPageButtonLimit > 0) {
			pageButtonLimit = newPageButtonLimit;
		} else {
			console.log("pwf - Error setting page button limit: Page button limit must be greater than 0.");
		}
	}
	exports.setRowClass = function (newRowClass) {
		rowClass = newRowClass;
	}
	exports.setCheckBoxSelector = function (newCheckBoxSelector) {
		checkBoxSelector = newCheckBoxSelector;
	}
	exports.setPreviouslySelectedPage = function (page) {
		previouslySelectedPage = parseInt(page);
	}

	// Goes to the specified page by toggling the 'offPage' class
	_goToPage = function (pageNum) {
		if (!pageNum || pageNum <= 0 || pageNum > totalPages) return;

		$('.' + rowClass).each(function () {
			var ele = $(this);
			if (ele.attr("data-page") == pageNum) {
				ele.toggleClass("off-page", false);
			}
			else {
				ele.toggleClass("off-page", true);
			}
		});
		$('.' + fillerClass).each(function () {
			var ele = $(this);
			if (ele.attr("data-page") == pageNum) {
				ele.toggleClass("off-page", false);
			}
			else {
				ele.toggleClass("off-page", true);
			}
		});

		// Enable/disable page buttons
		$('.page-btn').prop('disabled', false);
		$('#page-' + pageNum).prop('disabled', true);
	}

	// Changes to the specified page. Same as 'goToPage()' but also updates anchor user
	exports.goToPage = function (pageNum) {
		_goToPage(pageNum);
		anchorRow = findFirstRow(); // Anchor row updated on page change
		//current Url
		var _path = document.location.href;
		//set session storage page key to view path (ie: /Manage) and value as selected page
		sessionStorage.setItem(_path.slice(_path.lastIndexOf('/')) + "Page", pageNum);
	}

	// Finds the first user row that is shown at the current page with the current filters
	findFirstRow = function () {
		var result = null;
		$('.' + rowClass).each(function () {
			if (!$(this).hasClass("off-page") && !$(this).hasClass("no-match")) {
				result = $(this);
				return false;
			}
		});
		return result;
	}

	// Creates a button element for paging and returns it
	makePageButton = function (pageNum) {
		var fillerClass = "";
		if (pageNum == 0) {
			fillerClass = " pageButtonFiller";
		}
		return $('<input/>', {
			id: "page-" + pageNum,
			type: "button",
			"class": "page-btn btn btn-primary btn-xs" + fillerClass,
			value: pageNum
		}).click(function () {
			if (pageNum > 0) exports.goToPage(pageNum);
		});
	}

	// Creates a button element for scrolling through page buttons
	// -2 = <<, -1 = <, 1 = >, 2 = >>
	var chevrons = ["<<", "<", "", ">", ">>"];
	makeChevronButton = function (type) {
		if (type < -2 || type > 2) return null;
		return $('<input/>', {
			id: "chevron_" + type,
			type: "button",
			"class": "chevron-btn btn btn-primary btn-xs",
			value: chevrons[type + 2]
		}).click(function () {
			scrollPageButtons(type);
		})
	}
	scrollPageButtons = function (how) {
		switch (how) {
			case -2:
				setPageButtonPosition(1);
				break;
			case -1:
				setPageButtonPosition(Math.max(currentPageButtonStart - pageButtonLimit, 1));
				break;
			case 1:
				setPageButtonPosition(Math.min(currentPageButtonStart + pageButtonLimit, maxPageButtonStart));
				break;
			case 2:
				setPageButtonPosition(maxPageButtonStart);
				break;
		}
	}
	setPageButtonPosition = function (newPageButtonStart) {
		var newPageButtonEnd = newPageButtonStart + pageButtonLimit;
		$('.page-btn').each(function (index, elem) {
			var ele = $(this);
			var pageNum = ele.val();
			if (pageNum >= newPageButtonStart && pageNum < newPageButtonEnd) {
				ele.show();
			}
			else {
				ele.hide();
			}
		});
		if (newPageButtonStart == 1) {
			$("#chevron_-2").hide();
			$("#chevron_-1").hide();
		} else {
			$("#chevron_-2").show();
			$("#chevron_-1").show();
		}
		console.log(maxPageButtonStart);
		if (newPageButtonStart == maxPageButtonStart) {
			$("#chevron_2").hide();
			$("#chevron_1").hide();
		} else {
			$("#chevron_2").show();
			$("#chevron_1").show();
		}
		currentPageButtonStart = newPageButtonStart;
	}

	// Registers an element to act as a search filter.
	//  filterElementSelector - a css selector to find the input element that controls this filter
	//  filterType - either "search" or "dropDown", indicating what kind of filter it is
	//  rowFilteredPropertySelector - a css selector to find the element within a row that this filter acts on
	//  noneValue - for drop down filters, this is the text of the 'none' option (e.g. "Any", "None", "Select...", etc.)
	exports.addFilter = function (filterElementSelector, filterType, rowFilteredPropertySelector, noneValue) {
		// validate type
		if (["search", "dropDown"].indexOf(filterType) == -1) {
			console.log("Error registering filter for " + filterElementSelector + "; '" + filterType + "' is not a valid filter type. Use 'search' or 'dropDown'");
			return;
		}

		// create filter (no validation of selectors)
		var newFilter = {};
		newFilter.selector = filterElementSelector;
		newFilter.type = filterType;
		newFilter.find = rowFilteredPropertySelector;
		if (noneValue) newFilter.noneValue = noneValue;
		filters.push(newFilter);

		// add event listener
		if (filterType == "search") {
			$(filterElementSelector).on('input', _.debounce(function () { // The 'input' actions catches changes AND clicking the X to clear
				filterRows();
			}, 400));
		}
		else {
			$(filterElementSelector).change(function () {
				filterRows();
			});
		}

		console.log("pwf - Added filter:");
		console.log(newFilter);
	}

	// Updates the classes on each row item according to the current filter settings, and
	//  assigns page numbers to each row based on their display status.
	filterRows = function () {
		var fillerRowHeight = $('.' + rowClass).eq(1).height(); //using the 2nd item, because the 1st sometimes has a different top border thickness
		console.log(fillerRowHeight);
		if (!scrollBufferCreated) {
			for (var j = 0; j < pageLimit; j++) {
				//$('#tableEnd').after(generateFillerRow(fillerRowHeight, 1000000, true));
			}
			scrollBufferCreated = true;
		}

		$('.scrollBuffer').toggleClass('off-page', false);

		// Grab all filter values
		var filterValues = [];
		for (var h = 0; h < filters.length; h++) {
			filterValues[h] = $(filters[h].selector).val();
			if (filters[h].type == "search") filterValues[h] = filterValues[h].toLowerCase();
		}

		// Start off the paging process.
		var currentPage = 0;
		var currentPageTotal = pageLimit - 1;
		pageContainer.empty();
		pageContainer.append(makeChevronButton(-2));
		pageContainer.append(makeChevronButton(-1));

		// Evaluate each row and update classes
		$('.' + rowClass).each(function (index) {
			var ele = $(this);

			var checked = false;
			if (hasCheckBoxes) {
				checked = ele.find(checkBoxSelector)[0].checked;
			}

			// Go through registered filters
			var display = [];
			for (var i = 0; i < filters.length; i++) {
				if (filters[i].type == "search") {
					if (!filterValues[i] || filterValues[i] == "") {
						display[i] = true; // Display all when no search text entered
					}
					else {
						var thisItemText = ele.find(filters[i].find).html();
						ele.find(filters[i].find).each(function (index) {
							thisItemText += $(this).html();
						});
						display[i] = thisItemText.toLowerCase().search(filterValues[i]) > -1;
					}
				}
				else {
					if (filters[i].noneValue && filterValues[i] == filters[i].noneValue) {
						display[i] = true; // Display all when no filter option selected
					}
					else {
						display[i] = ele.find(filters[i].find).html() == filterValues[i];
					}
				}
			}

			// Anything that matches all filters is displayed normally
			// Anything else that is checked is still displayed, but with alternate style
			// Anything else is hidden
			var isShown = true;
			var matchesAllFilters = true;
			for (var i = 0; i < display.length; i++) {
				if (!display[i]) matchesAllFilters = false;
			}

			if (matchesAllFilters) {
				ele.toggleClass("check-no-match", false);
				ele.toggleClass("no-match", false);
			}
			else {
				if (checked) {
					ele.toggleClass("check-no-match", true);
					ele.toggleClass("no-match", false);
				}
				else {
					ele.toggleClass("check-no-match", false);
					ele.toggleClass("no-match", true);
					isShown = false;
				}
			}

			// Assign page. Paging starts at end of "page 0" with incrementation before
			// page assignment. So, the first element becomes the first item on page 1. This way,
			// if the last element perfectly fills the last page, it doesn't make another page
			// yet.
			if (isShown) {
				currentPageTotal += 1;
				if (currentPageTotal >= pageLimit) { // New page
					currentPageTotal = 0;
					currentPage += 1;
					pageContainer.append(makePageButton(currentPage));
				}
			}
			ele.attr("data-page", Math.max(currentPage, 1)); // We don't want hidden elements with page 0
		});

		if (currentPage == 0) {
			pageContainer.append(makePageButton(0)); // Generates filler page button for keeping table height constant
		}

		totalPages = currentPage;
		maxPageButtonStart = (Math.ceil(totalPages / pageButtonLimit) - 1) * pageButtonLimit + 1;
		pageContainer.append(makeChevronButton(1));
		pageContainer.append(makeChevronButton(2));
		setPageButtonPosition(1);

		// Removal/addition of filler rows for keeping constant table height.
		var fillerRowCount = currentPage == 0 ? pageLimit : pageLimit - currentPageTotal - 1;
		$('.' + fillerClass).remove();
		for (var k = 0; k < fillerRowCount; k++) {
			$('#tableEnd').after(generateFillerRow(fillerRowHeight, currentPage));
		}

		// After pagination is recalculated, the anchor row is used to decide which page to show. This way,
		// if a user is changing the filters a lot, their place in the list is roughly constant.
		if (anchorRow) {
			_goToPage(anchorRow.attr("data-page"));
		}
		else {
			_goToPage(1);
			anchorRow = findFirstRow();
		}

		$('.scrollBuffer').toggleClass('off-page', true);
		if (!findFirstRow()) {
			pageContainer.hide();
			console.log("Test");
			
		} else {
			pageContainer.show();
		}
	}

	generateFillerRow = function (height, page, isScrollBuffer) {
		if (typeof isScrollBuffer === 'undefined') {
			isScrollBuffer = false;
		}
		return '<tr class="nohover ' + (isScrollBuffer ? 'scrollBuffer' : fillerClass) + '" data-page="' + page + '" style="height: ' + height + 'px"><td class="whitetext">|</td></tr>'
	}

	$(document).ready(function () {
		$('#loading').hide();
		if ($(checkBoxSelector).length > 0) {
			hasCheckBoxes = true;
		}

		if ($('.all-check').length == 1) {
			if (hasCheckBoxes) {
				$('.all-check').change(function () {
					$('.' + rowClass).each(function () {
						var ele = $(this);
						if (!ele.hasClass("no-match") && !ele.hasClass("currUser")) {
							ele.find(checkBoxSelector).prop('checked', $('.all-check')[0].checked);
						}
					});
				});
			}
		}

		filterRows();
		if (previouslySelectedPage != 0) {
			_goToPage(previouslySelectedPage);
		}
		$('#loading').show();
	});
})(this.pwf = {});