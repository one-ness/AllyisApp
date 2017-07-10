﻿#pragma warning disable 1591

namespace AllyisApps
{
	/// <summary>
	/// Constants for action strings.
	/// </summary>
	public static class ActionConstants
	{
        public const string Action = "action";
        public const string Add = "add";
		public const string ChangePassword = "changepassword";
		public const string Charge = "charge";
		public const string ConfirmEmail = "confirmemail";
		public const string Create = "create";
		public const string CreateHoliday = "createholiday";
		public const string CreateOrg = "createorg";
		public const string CreatePayClass = "createpayclass";
		public const string CreateTimeEntryJson = "createtimeentryjson";
		public const string Delete = "delete";
		public const string DeleteEntry = "deleteentry";
		public const string DeleteHoliday = "deleteholiday";
		public const string DeleteOrg = "deleteorg";
		public const string DeletePayClass = "deletepayclass";
		public const string DeleteTimeEntry = "deletetimeentry";
		public const string DeleteTimeEntryJson = "deletetimeentryjson";
		public const string Details = "details";
		public const string DownloadImportUsersTemplate = "downloadimportuserstemplate";
		public const string Edit = "edit";
		public const string EditOrg = "editorg";
		public const string EditProfile = "editprofile";
		public const string EditTimeEntryApprovalState = "edittimeentryapprovalstate";
		public const string EditTimeEntryJson = "edittimeentryjson";
		public const string Export = "export";
		public const string ForgotPassword = "forgotpassword";
		public const string ForgotPasswordConfirmation = "forgotpasswordconfirmation";
		public const string GetBillingSummary = "getbillingsummary";
		public const string GetDetailsDataJson = "getdetailsdatajson";
		public const string Import = "import";
		public const string Index = "index";
		public const string Invite = "invite";
		public const string IsSubdomainNameUnique = "issubdomainnameunique";
		public const string LogOff = "logoff";
		public const string LogOn = "logon";
		public const string ManageOrg = "manageorg";
		public const string ManagePermissions = "managepermissions";
		public const string ManagePermissions2 = "managepermissions2";
		public const string OrgIndex = "orgindex";
		public const string PrivacyPolicy = "privacypolicy";
		public const string Reactivate = "reactivate";
		public const string ReactivateCustomer = "reactivatecustomer";
		public const string RedirectToSubdomainAction = "redirecttosubdomainaction";
		public const string Register = "register";
		public const string RemoveBilling = "removebilling";
		public const string RemoveInvitation = "removeinvitation";
		public const string RemoveMember = "removemember";
		public const string RemoveUser = "removeuser";
		public const string ResetPassword = "resetpassword";
		public const string ResetPasswordConfirmation = "resetpasswordconfirmation";
		public const string RouteHome = "routehome";
		public const string Subscribe = "subscribe";
		public const string Unsubscribe = "unsubscribe";
		public const string UpdateOvertime = "updateovertime";
		public const string UpdateStartOfWeek = "updatestartofweek";
		public const string UserEditAJAX = "usereditajax";
		public const string UploadCSVFile = "uploadcsvfile";
		public const string UserEdit = "useredit";
		public const string ViewPage = "viewpage";
		public const string ApplicationRedirect = "applicationredirect";
		public const string Help = "help";
		public const string FooterPartial = "footerpartial";
		public const string LogOnPartial = "logonpartial";
		public const string UpdateLanguage = "updatelanguage";
		public const string CopyEntries = "copyentries";
		public const string Report = "report";
		public const string SetLockDate = "setlockdate";
		public const string Settings = "settings";
		public const string MergePayClass = "mergepayclass";
		public const string ViewReport = "viewreport";
		public const string Template = "template";
		public const string EditUsers = "editusers";
	}

	/// <summary>
	/// Constants for controller Strings.
	/// </summary>
	public static class ControllerConstants
	{
		public const string Account = "account";
        public const string Controller = "controller";
        public const string Customer = "customer";
		public const string Home = "home";
		public const string Organization = "organization";
		public const string Project = "project";
		public const string Shared = "shared";
		public const string Subscription = "subscription";
		public const string TimeEntry = "timeentry";
	}

	/// <summary>
	/// Constants for view Strings.
	/// </summary>
	public static class ViewConstants
	{
		public const string Add = "Add";
		public const string AddBillingToSubscribe = "AddBillingToSubscribe";
		public const string ConfirmRemoveBillingInformation = "ConfirmRemoveBillingInformation";
		public const string Details = "Details";
		public const string Error = "Error";
		public const string Footer = "_Footer";
		public const string ForgotPasswordConfirmation = "ForgotPasswordConfirmation";
		public const string Index = "Index";
		public const string LogOnPartial = "_LogOnPartial";
		public const string Subscribe = "Subscribe";
		public const string MergePayClass = "MergePayClass";
	}

	/// <summary>
	/// Product name constants for use when retreiving product ID by name.
	/// </summary>
	public static class ProductNameKeyConstants
	{
		public const string TimeTracker = "TimeTracker";
	}
}
