// This is a small script to format the date picker. 
//
// Currently (3/27/2017) the date picker is used on the "Account/Register" 
// and "Account/EditProfile" pages on only the Edge browser to replace the 
// default date picker for the "Date of Birth" field.
//
// Usage:
// in "Scripts" section in .cshtml:
// <script scr="~/Scripts/allyis-datepicker.js" ></script>
//
// in code in .cshtml:
//    The important parts of both use cases is that there is a class called 
//    "in-cshtml-datepicker" to access the jquery script, and the type is 
//    set to "text" for the jquery-ui script to work.
// edit:
//    @Html.EditorFor(m => m.A_Date_Field, new { htmlattributes = new { @class = "in-cshtml-datepicker", @type = "text" } })
// Register:
//    <input class="in-cshtml-datepicker" type="text">

$( function() {
    $(".in-cshtml-datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-120:+0",
    });
});
