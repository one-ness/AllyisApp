document.getElementById("title-row").style.marginBottom = "1px"; //Don't want to mess with any other tables but this one

// permission check and grab data
//function ajaxUpdateUserSummary() {
//    var data = {
//        userId: "@(Model.EntryRange.UserId)",
//        startingDate: "@(Model.EntryRange.StartDate)",
//        endingDate: "@(Model.EntryRange.EndDate)"};

//    $.ajax({
//        type: "post",
//        url: '@(Url.Action(ActionConstants.GetDetailsDataJson))',
//        data: JSON.stringify(data),
//        contentType: "application/json"})
//    .done(function (res)
//    {
//        ajaxHandleOverridingResponses(res);
//        if (res.status == "success")
//        {
//            ajaxUpdateUserSummary_populate(res.projects);
//        }
//    });
//    return false;
//}

// build Summary box
//function ajaxUpdateUserSummary_populate(data) {
//    data = $(data);
//    var the_table = $("#timesheet-summary table:first");
//    var string_builder = []
//    data.each(function(index, element) {
//        var strong = (index == data.length - 1);
//        var strong_pre = strong ? "<strong>" : "";
//        var strong_post = strong ? "</strong>" : "";
//        string_builder.push(
//            "<tr>",
//                "<td>", strong_pre, this.projectName, strong_post, "</td>",
//                "<td>", strong_pre, this.hours, strong_post, "</td>",
//            "</tr>")
//    });
//    the_table.html(string_builder.join(""));
//}

function ajaxDelete(form_child, delete_action_url) {
    var form_element = $(form_child).parents('form:first');
    if (form_element.is(".deleting, .locked")) return false; // do not resubmit the request until it returns, do not submit locked forms
    var form_wrap = form_element.parents('.inline-form-anim-wrap:first');
    var data = form_element.serialize();
    form_element.addClass("deleting");
    $.ajax({
        type: "post",
        url: delete_action_url,
        data: data,
        timeout: 5000,
        dataType: "json"})
    .done(function(res) {
        if (res.status == 'success')
        {
            form_wrap.css({ 'z-index': -1000 }).transition({ 'max-height': 0, duration: 500, easing: 'linear' },
                    function() { form_wrap.remove(); });
            //ajaxApproveReject_markPendingIfSet(form_element);
            //ajaxUpdateUserSummary();
        }
        form_element.removeClass("deleting");
    })
    .fail(function(res) {
        form_element.removeClass("deleting").addClass("error error-delete");
    });
    return false;
}

function ajaxCreate(form_element, create_action_url) {
    form_element = $(form_element);
    if (form_element.is(".submitting, .locked")) return false; // do not resubmit the request until it returns, do not submit locked forms
    form_element.addClass("submitting");
    var container = form_element.parents(".table-col:first");
    var data = form_element.serialize();
    $.ajax({
        type: "post",
        url: create_action_url,
        data: data,
        timeout: 5000,
        dataType: "json"})
    .done(function(res) {
        ajaxHandleOverridingResponses(res, { form_element: form_element});
        if (res.status == 'success')
        {
            //alert('success. id=' + res.id + "\r\n" + res.duration + "\r\n" + res.message);
            ajaxUpdateValues(form_element, res.values);
            changeRemove(form_element);
            form_element.removeClass('create');
            appendNewEntryForm(container);
            //ajaxApproveReject_markPendingIfSet(form_element);
            //ajaxUpdateUserSummary();
        }
        form_element.removeClass("submitting");
    })
    .fail(function(res) {
        form_element.removeClass("submitting").addClass("error error-submit");
    });
    return false;
}

function ajaxUpdateValues(form_element, values) {
    form_element = $(form_element);
    if (values.duration) {
        form_element.find("[name='Duration']").val(values.duration);
    }
    if (values.id) {
        form_element.find("[name='TimeEntryId']").val(values.id);
        form_element.parents(".inline-form-anim-wrap:first").attr("id", "timeentry_" + values.id);
    }
    if (values.description) {
        form_element.find("[name='Description']").val(values.description);
    }
    if (values.PayClassName) {
        form_element.find("[name='PayClassName']").val(values.PayClassName);
    }
}

function appendNewEntryForm(container_element) {
    //var container = form_element.parents(".table-col:first");
    var sample_element = container_element.find(".hidden-sample:first");
    var new_entry = sample_element.clone().appendTo(container_element);
    new_entry.css('height'); //read a css attribute so we can be sure it was rendered before triggering the animation
    new_entry.removeClass("hidden-sample");
    var delay = 300; //delay in milliseconds
    setTimeout(function(){  //Need to delay focus while ajax makes the new row
        new_entry.find("[name=Duration]").focus();
    }, delay);
}

function ajaxEdit(form_element, edit_action_url) {
    form_element = $(form_element);
    if (form_element.is(".submitting, .locked")) return false; // do not resubmit the request until it returns, do not submit locked forms
    form_element.addClass("submitting");
    var data = form_element.serialize();
    $.ajax({
        type: "post",
        url: edit_action_url,
        data: data,
        timeout: 5000,
        dataType: "json"})
    .done(function(res) {
        ajaxHandleOverridingResponses(res, { form_element: form_element});
        if (res.status == 'success')
        {
            changeRemove(form_element);
            ajaxUpdateValues(form_element, res.values);
            //ajaxApproveReject_markPendingIfSet(form_element);
            //ajaxUpdateUserSummary();
        }
        form_element.removeClass("submitting");
    })
    .fail(function(res) {
        form_element.removeClass("submitting").addClass("error error-submit");
    });
    return false;
}

//function ajaxApproveReject_markPendingIfSet(child_element) {
//    var container_element = ajaxApproveRejectDay_container($(child_element));
//    if (container_element.hasClass("approved") || container_element.hasClass("rejected"))
//    {
//        ajaxApproveRejectDay_switchDayClasses(container_element, "rejected approved", "pending", "");
//    }
//}

function ajaxEditOrCreate(form_element, create_action_url, edit_action_url) {
    form_element = $(form_element);
    if (!form_element.hasClass("submitting")) {
        if (form_element.hasClass("create")) {
            return ajaxCreate(form_element, create_action_url);
        }
        else {
            return ajaxEdit(form_element, edit_action_url);
        }
    }
}

//function ajaxApproveRejectDay_switchDayClasses(container_element, classes_to_remove, classes_to_add, classes_to_toggle) {
//    var header_element = ajaxApproveRejectDay_header(container_element);
//    var header_approval_element = ajaxApproveRejectDay_header_approval(header_element);
//    container_element.removeClass(classes_to_remove).addClass(classes_to_add).toggleClass(classes_to_toggle);
//    header_element.removeClass(classes_to_remove).addClass(classes_to_add).toggleClass(classes_to_toggle);
//}

//function ajaxApproveRejectDay_constructFormData(all_forms, approval_state) {
//    var data = []
//    $(all_forms).each(function() {
//        var timeEntryId = $(this).find("[name='TimeEntryId']").val();
//        var organizationId = $(this).find("[name='OrganizationId']").val();
//        data.push( {
//            TimeEntryId: timeEntryId,
//            OrganizationId: organizationId,
//            ApprovalState: approval_state});
//    });
//    return data;
//}

//function ajaxApproveRejectDay_container(child_button_element) {
//    return child_button_element.parents(".table-container:first");
//}

//function ajaxApproveRejectDay_approval(container_element) {
//    return container_element.find(".approval:first");
//}

//function ajaxApproveRejectDay_header(container_element) {
//    return container_element.prev();
//}

//function ajaxApproveRejectDay_header_approval(header_element) {
//    return header_element.parents(".approval:first");
//}

//function ajaxApproveRejectDay_allForms(container_element) {
//    return container_element.find("div.inline-form-anim-wrap:not([id='timeentry_0']) form");
//}

function ajaxHandleOverridingResponses(response, relevant_objects) {
    if (response.status == "error") {
        alert("error: " + response.message);
        if (response.errors) {
            $(errors).each(function() {
                alert("error: " + this.message);
            })
        }
    }
    if (response.action == "REFRESH") {
        location.reload();
    }
    if (response.action == "REVERT" && relevant_objects.form_element) {
        changeRemove(relevant_objects.form_element);
        ajaxUpdateValues(relevant_objects.form_element, response.values);
    }
}

function changeOccur(form_child) {
    form_element = $(form_child).parents("form:first");
    form_element.addClass("changed");

    form_submit_element = form_element.find("button");
    form_submit_element.removeAttr("disabled");
}

function changeRemove(form_element) {
    form_element.removeClass("changed");

    form_submit_element = form_element.find("button");
    form_submit_element.attr("disabled", true);
}

function keyDown(e, form_child) {
    changeOccur(form_child);
    var unicode = e.keyCode ? e.keyCode : e.charCode;
    if (!e.shiftKey && unicode == 40) {
        focusNextDuration(form_child);
    }
    else if (!e.shiftKey && unicode == 38) {
        focusPreviousDuration(form_child);
    }
}

$( document ).ready(function() {
    var MODULE = new ListGroupSearch();
    MODULE.init({
        $target: $("#viewasuser-list")
    });
    $("#viewasuser-search").keyup(_.debounce(
        function(key) {
            if (key.which == 13 || key.keyCode == 13) {
                //alert("keypress val:" + this.value);
                if (this.value== "")
                    MODULE.search(" "); //search on a value that is present in all entries
                else
                    MODULE.search(this.value);
                //after search, set new page numbers
                paginate();
            }
        },
        200
    ));
    $('ProjectId').each(function(e){     //Used for model binding with custom ddl option selection
        eval($(this).data('onload'));
    });
    focusFirstDuration();
});

function focusFirstDuration(){
    $("[name='Duration'][value='']:first").focus();
}

function focusNextDuration(form_child){
    var tb = $("[name='Duration']");
    for (var i = 0; i < tb.length; i++) {
        if(tb[i] == form_child) {
            if(tb[i+1].parentNode.parentNode.parentNode.className.indexOf('hidden-sample') > -1)
            {
                if(tb.length >= i+2){
                    tb[i+2].focus();
                }
            }
            else
            {
                if(tb.length >= i+1){
                    tb[i+1].focus();
                }
            }
            break;
        }
    }
}

function focusPreviousDuration(form_child){
    var tb = $("[name='Duration']");
    for(var i = 0; i < tb.length; i++) {
        if(tb[i] == form_child) {
            if(tb[i-1].parentNode.parentNode.parentNode.className.indexOf('hidden-sample') > -1)
            {
                if(i-2 >= 0){
                    tb[i-2].focus();
                }
            }
            else
            {
                if(i-1 >= 0){
                    tb[i-1].focus();
                }
            }
            break;
        }
    }
}

// Date range picker for main page
init_drp(
    "daterange",
    [{
        text: "Today",
        dateStart: function() { return moment() },
        dateEnd: function() { return moment() }
    }, {
        text: "Yesterday",
        dateStart: function() { return moment().subtract(1, 'days') },
        dateEnd: function() { return moment().subtract(1, 'days') }
    }, {
        text: "Last 7 Days",
        dateStart: function() { return moment().subtract(6, 'days') },
        dateEnd: function() { return moment() }
    }, {
        text: "Last 30 Days",
        dateStart: function() { return moment().subtract(29, 'days') },
        dateEnd: function() { return moment() }
    }, {
        text: "This Month",
        dateStart: function() { return moment().startOf('month') },
        dateEnd: function() { return moment().endOf('month') }
    }, {
        text: "Last Month",
        dateStart: function() { return moment().subtract(1, 'month').startOf('month') },
        dateEnd: function() { return moment().subtract(1, 'month').endOf('month') }
    }],
    "StartDate",
    "EndDate",
    2,
    function () {
        if (this.notFirstTime) {
            $('#daterange').parents("form:first").submit();
        }
        this.notFirstTime = true;
    },
    model_startdate,
    model_enddate
);

// Date range picker for lock date
init_drp(
    "LockDate",
    [{
        text: "Today",
        dateStart: function() { return moment() },
        dateEnd: function() { return moment() }
    }, {
        text: "Two Weeks Ago",
        dateStart: function() { return moment().subtract(6, 'days') },
        dateEnd: function() { return moment().subtract(6, 'days') }
    }],
    "LockDate",
    null,
    1,
    function () {
        if (this.notFirstTimeLockDate) {
            $('#LockDate').parents("form:first").submit();
        }
        this.notFirstTimeLockDate = true;
    },
    model_lockdate,
    null
);

function drp_nextWeek() {
    var drp = $('#daterange');
    var rangeObj = drp.daterangepicker("getRange");
    var newStart = new Date();
    var newEnd = new Date();
    var oldStart = new Date();
    var oldEnd = new Date();

    oldStart.setTime(rangeObj.start.getTime());
    oldEnd.setTime(rangeObj.end.getTime());
    newStart.setTime(rangeObj.start.getTime() + (7 * 86400000 ));
    newEnd.setTime(rangeObj.end.getTime() + (7 * 86400000));

    // get offset hours from standard time (changes during DST)
    var oldStartOffset = oldStart.getTimezoneOffset( ) / 60;
    var oldEndOffset = oldEnd.getTimezoneOffset( ) / 60;
    var newStartOffset = newStart.getTimezoneOffset( ) / 60;
    var newEndOffset = newEnd.getTimezoneOffset( ) / 60;
    var startOffsetChange = newStartOffset - oldStartOffset; // = 0, -1, or 1
    var endOffsetChange = newEndOffset - oldEndOffset; // = 0, -1, or 1

    newStart.setTime(newStart.getTime() + (startOffsetChange * 3600000));
    newEnd.setTime(newEnd.getTime() + (endOffsetChange * 3600000));

    drp.daterangepicker("setRange", {start: newStart, end: newEnd });
}
function drp_prevWeek() {
    var drp = $('#daterange');
    var rangeObj = drp.daterangepicker("getRange");
    var newStart = new Date();
    var newEnd = new Date();
    var oldStart = new Date();
    var oldEnd = new Date();

    oldStart.setTime(rangeObj.start.getTime());
    oldEnd.setTime(rangeObj.end.getTime());
    newStart.setTime(rangeObj.start.getTime() - (7 * 86400000 ));
    newEnd.setTime(rangeObj.end.getTime() - (7 * 86400000));

    // get offset hours from standard time (changes during DST)
    var oldStartOffset = oldStart.getTimezoneOffset( ) / 60;
    var oldEndOffset = oldEnd.getTimezoneOffset( ) / 60;
    var newStartOffset = newStart.getTimezoneOffset( ) / 60;
    var newEndOffset = newEnd.getTimezoneOffset( ) / 60;
    var startOffsetChange = newStartOffset - oldStartOffset; // = 0, -1, or 1
    var endOffsetChange = newEndOffset - oldEndOffset; // = 0, -1, or 1

    newStart.setTime(newStart.getTime() + (startOffsetChange * 3600000));
    newEnd.setTime(newEnd.getTime() + (endOffsetChange * 3600000));
    console.log("setrange: " + newStart + ", " + newEnd);
    drp.daterangepicker("setRange", {start: newStart, end: newEnd });
}