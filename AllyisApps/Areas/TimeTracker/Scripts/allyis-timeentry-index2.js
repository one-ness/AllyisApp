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

function nonajaxDelete(form_child, delete_action_url) {
    var form_element = $(form_child).parents('form:first');
    if (form_element.is(".deleting, .locked")) return false; // do not resubmit the request until it returns, do not submit locked forms
    var form_wrap = form_element.parents('.inline-form-anim-wrap:first');
    //var data = form_element.serialize();
    form_element.addClass("deleting");
    if (form_element[0].TimeEntryId.value === '' || form_element[0].TimeEntryId.value === 0) {
        console.log("Delete of previous");
        addRow(form_child);
        //remove form as we no longer care about data
        form_wrap.css({ 'z-index': -1000 }).transition({ 'max-height': 0, duration: 500, easing: 'linear' },
            function () { form_wrap.remove(); });
    }
    else {
        var del = form_element.find("[name='IsDeleted']")[0]
        del.setAttribute('value', 'True');
        //hide form so that delete message is passed on
        form_wrap.css({ 'z-index': -1000 }).transition({ 'max-height': 0, duration: 500, easing: 'linear' },
            function () { form_wrap.hide(); });
    }

    form_element.removeClass("deleting");
    //.fail(function (res) {
    //	form_element.removeClass("deleting").addClass("error error-delete");
    //});
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
        dataType: "json"
    })
        .done(function (res) {
            if (res.status === 'error') {
                form_element.addClass("error error-submit");
            }
            ajaxHandleOverridingResponses(res, { form_element: form_element });
            if (res.status === 'success') {
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
        .fail(function (res) {
            form_element.removeClass("submitting").addClass("error error-submit");
        });
    return false;
}

function ajaxUpdateValues(form_element, values) {
    form_element = $(form_element);

    form_element.hasClass("create") ? updateTimes(values) : editTimes(values);
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

function editTimes(values) {
    var elements = document.getElementsByName("entry-" + values.projectId);

	/* BUG: If the user makes a new entry for a project not currently listed and
	/* then edits the entry, the Total Time will be 0. This if will stop it but
	/* not update the times in this case.
	*/
    if (elements.length !== 0) {
        var totalTime = $('#totalHours').text().split(":"),
            totalHours = parseInt(totalTime[0]),
            totalMinutes = parseInt(totalTime[1]),
            projectCurrentTotal = $('#' + values.projectId).text().split(":"),
            projectCurrentHours = parseInt(projectCurrentTotal[0]),
            projectCurrentMinutes = parseInt(projectCurrentTotal[1]),
            updatedHours = 0,
            updatedMinutes = 0;

        for (var i = 0; i < elements.length; i++) {
            var duration = elements[i].firstElementChild.value.split(":");
            updatedHours += parseInt(duration[0]);
            updatedMinutes += parseInt(duration[1]);
        }
        if (updatedMinutes >= 60) {
            updatedHours += updatedMinutes / 60;
            updatedMinutes = updatedMinutes % 60;
        }

        $('#' + values.projectId).fadeOut(500, function () {
            $('#' + values.projectId).text(Math.floor(updatedHours) + ":" + (updatedMinutes < 10 ? "0" + updatedMinutes : updatedMinutes)).fadeIn(500);
        });

        totalHours = totalHours + (updatedHours - projectCurrentHours);
        totalMinutes = totalMinutes + (updatedMinutes - projectCurrentMinutes);
        if (totalMinutes >= 60) {
            totalHours += totalMinutes / 60;
            totalMinutes = totalMinutes % 60;
        }
        $('#totalHours').fadeOut(500, function () {
            $('#totalHours').text(Math.floor(totalHours) + ":" + (Math.abs(totalMinutes) < 10 ? "0" + Math.abs(totalMinutes) : Math.abs(totalMinutes))).fadeIn(500);
        })
    }
}

function updateTimes(values) {
    var currentVal = $('#totalHours').text().split(":"),
        updateVal = values.duration.split(":"),
        projectVal = $('#' + values.projectId).text().split(":");

    if (updateVal[0].lastIndexOf('-', 0) === 0) {
        updateVal[1] = '-' + updateVal[1];
    }

    var projectHours = parseInt(projectVal[0]),
        projectMinutes = parseInt(projectVal[1]),
        totalHours = parseInt(currentVal[0]),
        totalMinutes = parseInt(currentVal[1]),
        hourUpdate = parseInt(updateVal[0]),
        minuteUpdate = parseInt(updateVal[1]);

    var projHour = projectHours + hourUpdate;
    var projMinute = projectMinutes + (hourUpdate < 0 ? minuteUpdate * -1 : minuteUpdate);//What do do if negative minites more then current.
    if (projMinute >= 60) {
        projHour += projMinute / 60;
        projMinute = projMinute % 60;
    }

    var totalHour = totalHours + hourUpdate;
    var totalMinute = totalMinutes + minuteUpdate;
    if (totalMinute >= 60) {
        totalHour += totalMinute / 60;
        totalMinute = totalMinute % 60;
    }

    $('#' + values.projectId).fadeOut(500, function () {
        $('#' + values.projectId).text(projHour + ":" + (Math.abs(projMinute) < 10 ? "0" + Math.abs(projMinute) : Math.abs(projMinute))).fadeIn(500);
    });

    $('#totalHours').fadeOut(500, function () {
        $('#totalHours').text(Math.floor(totalHour) + ":" + (Math.abs(totalMinute) < 10 ? "0" + Math.abs(totalMinute) : Math.abs(totalMinute))).fadeIn(500);
    });
}

function addRow(form_chlid) {
    //TODO:Validate
    var form_element = $(form_chlid).parents('form:first');
    var container = form_element.parents(".table-col:first");
    appendNewEntryForm(container);
}

function appendNewEntryForm(container_element) {
    //var container = form_element.parents(".table-col:first");
    var sample_element = container_element.find(".hidden-sample:first");
    var new_entry = sample_element.clone().appendTo(container_element);
    new_entry.css('height'); //read a css attribute so we can be sure it was rendered before triggering the animation
    new_entry.removeClass("hidden-sample");
    setTimeout(function () {  //Need to delay focus while ajax makes the new row
        new_entry.find("[name='Duration']").focus();
    }, 300);
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
        dataType: "json"
    })
        .done(function (res) {
            if (res.status === 'error') {
                form_element.addClass("error error-submit");
            }
            ajaxHandleOverridingResponses(res, { form_element: form_element });
            if (res.status === 'success') {
                changeRemove(form_element);
                ajaxUpdateValues(form_element, res.values);
                //ajaxApproveReject_markPendingIfSet(form_element);
                //ajaxUpdateUserSummary();
            }
            form_element.removeClass("submitting");
        })
        .fail(function (res) {
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
    if (response.status === "error") {
        alert("error: " + response.message);
        if (response.errors) {
            $(errors).each(function () {
                alert("error: " + this.message);
            })
        }
    }
    if (response.action === "REFRESH") {
        location.reload();
    }
    if (response.action === "REVERT" && relevant_objects.form_element) {
        changeRemove(relevant_objects.form_element);
        ajaxUpdateValues(relevant_objects.form_element, response.values);
    }
}
function setDefalutProject(form_child) {
	var form_element = $(form_child).parents("form:first");
	if (form_element[0].ProjectIdDdl.value == -1 && form_element[0].ProjectIdDdl.options.length == 2) {
		form_element[0].ProjectIdDdl.selectedIndex = 0;
		form_element[0].ProjectIdDdl.value = form_element[0].ProjectIdDdl.options[0].value;
	}
}


function changeOccur(form_child) {
    var form_element = $(form_child).parents("form:first");
    form_element.addClass("changed");

    if ($(form_element).hasClass("create")) {
        var created = form_element.find("[name='IsCreated']")[0]
        created.setAttribute("value", "True");
    }

    form_element.removeClass("create");
    var edited = form_element.find("[name='IsEdited']")[0]
    edited.setAttribute("value", "True");
	
    form_element[0].ProjectId.value = form_element[0].ProjectIdDdl.value;

    form_element[0].PayClassId.value = form_element[0].PayClassNameDdl.value;
	setDefalutProject(form_child)
    //form_submit_element = form_element.find("button");
    //form_submit_element.removeAttr("disabled");

    form_element.removeClass("error");
    form_element.removeClass("error-submit");
}

function fillDLL(form_child) {
    var form_element = $(form_child).parents("form:first");
    form_element[0].ProjectId.value = form_element[0].ProjectIdDdl.value;
    form_element[0].PayClassId.value = form_element[0].PayClassNameDdl.value;
}

function changeRemove(form_element) {
    form_element.removeClass("changed");

    form_submit_element = form_element.find("button");
    form_submit_element.attr("disabled", true);
}

function keyDown(e, form_child) {
    changeOccur(form_child);
    var unicode = e.keyCode ? e.keyCode : e.charCode;
    if (!e.shiftKey && unicode === 40) {
        focusNextDuration(form_child);
    }
    else if (!e.shiftKey && unicode === 38) {
        focusPreviousDuration(form_child);
    }
}

$(document).ready(function () {
    var MODULE = new ListGroupSearch();
    MODULE.init({
        $target: $("#viewasuser-list")
    });
    $("#viewasuser-search").keyup(_.debounce(
        function (key) {
            //if (key.which === 13 || key.keyCode === 13) {
                //alert("keypress val:" + this.value);
                if (this.value === "")
                    MODULE.search(" "); //search on a value that is present in all entries
                else
                    MODULE.search(this.value);
                //after search, set new page numbers
                paginate();
            //}
        },
        200
    ));
    //$('ProjectId').each(function (e) {     //Used for model binding with custom ddl option selection
    //	eval($(this).data('onload'));
    //});

    $("[name='ProjectId']").each(function (e) {
        fillDLL(this);
    })

    focusFirstDuration();
});

function focusFirstDuration() {
    $("[name='Duration'][value='']:first").focus();
}

function focusNextDuration(form_child) {
    var tb = $("[name='Duration']");
    for (var i = 0; i < tb.length; i++) {
        if (tb[i] === form_child) {
            if (tb[i + 1].parentNode.parentNode.parentNode.className.indexOf('hidden-sample') > -1) {
                if (tb.length >= i + 2) {
                    tb[i + 2].focus();
                }
            }
            else {
                if (tb.length >= i + 1) {
                    tb[i + 1].focus();
                }
            }
            break;
        }
    }
}

function focusPreviousDuration(form_child) {
    var tb = $("[name='Duration']");
    for (var i = 0; i < tb.length; i++) {
        if (tb[i] === form_child) {
            if (tb[i - 1].parentNode.parentNode.parentNode.className.indexOf('hidden-sample') > -1) {
                if (i - 2 >= 0) {
                    tb[i - 2].focus();
                }
            }
            else {
                if (i - 1 >= 0) {
                    tb[i - 1].focus();
                }
            }
            break;
        }
    }
}

// Date range picker for main page
//init_drp(
//    "daterange",
//    [{
//        text: "Current Pay Period",
//        dateStart: function () { return moment().startOf('week') },
//        dateEnd: function () { return moment().endOf('week') }
//    }, {
//        text: "Next Pay Period",
//        dateStart: function () { return moment().subtract(7, 'days').startOf('week') },
//        dateEnd: function () { return moment().subtract(7, 'days').endOf('week') }
//    }, {
//        text: "Previous Pay Period",
//        dateStart: function () { return moment().startOf('month') },
//        dateEnd: function () { return moment().endOf('month') }
//    }, {
//        text: "Custom Pay Period",
//        dateStart: function () { return moment().subtract(1, 'month').startOf('month') },
//        dateEnd: function () { return moment().subtract(1, 'month').endOf('month') }
//    }],
//    "StartDate",
//    "EndDate",
//    2,
//    function () {
//        if (this.notFirstTime) {
//            $('#daterange').parents("form:first").submit();
//        }
//        this.notFirstTime = true;
//    },
//    model_startdate,
//    model_enddate
//);

// Date range picker for lock date
//init_drp(
//    "LockDate",
//    [{
//        text: "Today",
//        dateStart: function() { return moment() },
//        dateEnd: function() { return moment() }
//    }, {
//        text: "Two Weeks Ago",
//        dateStart: function() { return moment().subtract(6, 'days') },
//        dateEnd: function() { return moment().subtract(6, 'days') }
//    }],
//    "LockDate",
//    null,
//    1,
//    function () {
//        if (this.notFirstTimeLockDate) {
//            $('#LockDate').parents("form:first").submit();
//        }
//        this.notFirstTimeLockDate = true;
//    },
//    model_lockdate,
//    null
//);

function drp_nextWeek() {
    var drp = $('#daterange');
    var rangeObj = drp.daterangepicker("getRange");
    var newStart = new Date();
    var newEnd = new Date();
    var oldStart = new Date();
    var oldEnd = new Date();

    oldStart.setTime(rangeObj.start.getTime());
    oldEnd.setTime(rangeObj.end.getTime());
    newStart.setTime(rangeObj.start.getTime() + (7 * 86400000));
    newEnd.setTime(rangeObj.end.getTime() + (7 * 86400000));

    // get offset hours from standard time (changes during DST)
    var oldStartOffset = oldStart.getTimezoneOffset() / 60;
    var oldEndOffset = oldEnd.getTimezoneOffset() / 60;
    var newStartOffset = newStart.getTimezoneOffset() / 60;
    var newEndOffset = newEnd.getTimezoneOffset() / 60;
    var startOffsetChange = newStartOffset - oldStartOffset; // = 0, -1, or 1
    var endOffsetChange = newEndOffset - oldEndOffset; // = 0, -1, or 1

    newStart.setTime(newStart.getTime() + (startOffsetChange * 3600000));
    newEnd.setTime(newEnd.getTime() + (endOffsetChange * 3600000));

    drp.daterangepicker("setRange", { start: newStart, end: newEnd });
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
    newStart.setTime(rangeObj.start.getTime() - (7 * 86400000));
    newEnd.setTime(rangeObj.end.getTime() - (7 * 86400000));

    // get offset hours from standard time (changes during DST)
    var oldStartOffset = oldStart.getTimezoneOffset() / 60;
    var oldEndOffset = oldEnd.getTimezoneOffset() / 60;
    var newStartOffset = newStart.getTimezoneOffset() / 60;
    var newEndOffset = newEnd.getTimezoneOffset() / 60;
    var startOffsetChange = newStartOffset - oldStartOffset; // = 0, -1, or 1
    var endOffsetChange = newEndOffset - oldEndOffset; // = 0, -1, or 1

    newStart.setTime(newStart.getTime() + (startOffsetChange * 3600000));
    newEnd.setTime(newEnd.getTime() + (endOffsetChange * 3600000));
    drp.daterangepicker("setRange", { start: newStart, end: newEnd });
}

(function set_copier_confirmation() {
    $('.weekcopier').click(function () {
        return confirm('Copying the previous week will overwrite the current entries for this week. Are you sure you want to continue?');
    });
    $('.daycopier').click(function () {
        return confirm('Copying the previous day will overwrite the current entries for this day. Are you sure you want to continue?')
    })
})()