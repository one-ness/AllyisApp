// A tool for pagination of a list of items on a view.
// To use, define the following three variables in a javascript tag on the page:
//   - pageSize (as an int)
//   - page_container_id - the ID of the element containing all the listed elements
//   - page_item_tag - the html tag type of the listed elements (e.g. div, tr, etc...)
// This works in conjunction with some set-up in the view model and view. For a working example, see _OrgMembers.cshtml

function paginate() {
    var pcount = 0;
    var page = 1;
    var excludeval = -1;
    var buttonCount = 1;

    $('#' + page_container_id).find(page_item_tag).each(function () {
        if (pcount >= pageSize) {
            page++;
            pcount = 0;
        }

        if ($(this).hasClass('excluded')) {
            $(this).data('page', excludeval);
        }
        else {
            $(this).data('page', page);
            pcount++;
        }
    });

    //hide extra page buttons
    $("input[name='pageButton']").each(function () {
        if (buttonCount > page) {
            $(this).hide();
        }
        else {
            $(this).show();
        }
        buttonCount++;
    });

    //build list based on page
    showPage(1);
}

//show members for page: pageNum
function showPage(pageNum) {
    var pageStr = "" + pageNum;
    var dPage = "";

    //hide other pages and show selected
    $('#' + page_container_id).find(page_item_tag).hide();
    $('#' + page_container_id).find(page_item_tag).each(function () {
        dPage = $(this).data('page');
        if (dPage == pageStr) {
            if ($(this).not('.excluded')) {
                $(this).show();
            }
            else {
                $(this).data('page', excludeval);
            }
        }
    });
    select_page(pageNum);
}

//disable selected page button
function select_page(pageNum) {
    var pcount = 1;
    $("input[name='pageButton']").prop('disabled', false);
    $("input[name='pageButton']").each(function () {
        if (pcount == pageNum) {
            $(this).prop('disabled', true);
        }
        pcount++;
    });
}