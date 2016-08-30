//Validate subdomain both when it autofills and when user edits it themself
$("#SubdomainName").on("paste keyup", _.debounce(validateSubdomain, 250));

//Generate subdomain name as user types in company name
$("#Name").on("paste keyup", _.debounce(function () {
    var orgName = $(this).val();
    orgName = orgName.replace(/[^a-zA-Z0-9]/g, ""); //Remove anything not alphanumeric
    orgName = orgName.toLowerCase(); //So users can see if subdomain would be ugly in url
    $("#SubdomainName").val(orgName);
    validateSubdomain();
}, 250));

function validateSubdomain() {
    var subdomain = $("#SubdomainName").val();
    subdomain = subdomain.trim();
    var legalPatt = /^[a-zA-Z0-9]+(-[a-zA-Z0-9]+)*$/;
    var isLegal = legalPatt.test(subdomain);
    //console.log(isLegal);
    if (isLegal) {
        $.ajax({
            url: isSubdomainNameUniqueAction + subdomain,
            success: function (data) {
                if ("False" === data) {
                    $('#subdomainTaken').show();
                    $("#submit").prop('disabled', true);
                } else {
                    $('#subdomainTaken').hide();
                    $("#submit").prop('disabled', false);
                }
            },
            error: function (xhr) {
                console.log("Unexpected error occured.")
            }
        })
    } else {  //Not a valid subdomain
        $('#subdomainTaken').hide();
        $("#submit").prop('disabled', true);
    }
}