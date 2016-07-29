using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps
{
	/// <summary>
	/// Constants for action strings.
	/// </summary>
	public static class ActionConstants
	{
		/// <summary>
		/// For when you need a key for storing your action value or some such thing.
		/// </summary>
		public const string Action = "Action";

		#region account
		/// <summary>
		/// Action-Name: Add.
		/// </summary>
		public const string Add = "Add";

		/// <summary>
		/// Action-Name: ChangePassword.
		/// </summary>
		public const string ChangePassword = "ChangePassword";

		/// <summary>
		/// Action-Name: Charge.
		/// </summary>
		public const string Charge = "Charge";

		/// <summary>
		/// Action-Name: ConfirmEmail.
		/// </summary>
		public const string ConfirmEmail = "ConfirmEmail";

		/// <summary>
		/// Action-Name: CreateOrg.
		/// </summary>
		public const string CreateOrg = "CreateOrg";

		/// <summary>
		/// Action-Name: DeleteOrg.
		/// </summary>
		public const string DeleteOrg = "DeleteOrg";

		/// <summary>
		/// Action-Name: DownloadImportUsersTemplate.
		/// </summary>
		public const string DownloadImportUsersTemplate = "DownloadImportUsersTemplate";

		/// <summary>
		/// Action-Name: Edit.
		/// </summary>
		public const string Edit = "Edit";

		/// <summary>
		/// Action-Name: EditProfile.
		/// </summary>
		public const string EditProfile = "EditProfile";

		/// <summary>
		/// Action-Name: ForgotPassword.
		/// </summary>
		public const string ForgotPassword = "ForgotPassword";

		/// <summary>
		/// Action-Name: ForgotPasswordConfirmation.
		/// </summary>
		public const string ForgotPasswordConfirmation = "ForgotPasswordConfirmation";

		/// <summary>
		/// Action-Name: GetBillingSummary.
		/// </summary>
		public const string GetBillingSummary = "GetBillingSummary";

		/// <summary>
		/// Action-Name: Index.
		/// </summary>
		public const string Index = "Index";

		/// <summary>
		/// Action-Name: Invite.
		/// </summary>
		public const string Invite = "Invite";

		/// <summary>
		/// Action-Name: LogOff.
		/// </summary>
		public const string LogOff = "LogOff";

		/// <summary>
		/// Action-Name: LogOn.
		/// </summary>
		public const string LogOn = "LogOn";

		/// <summary>
		/// Action-Name: Manage.
		/// </summary>
		public const string Manage = "Manage";

		/// <summary>
		/// Action-Name: ManagePermissions.
		/// </summary>
		public const string ManagePermissions = "ManagePermissions";

		/// <summary>
		/// Action-Name: Organizations.
		/// </summary>
		public const string Organizations = "Organizations";

		/// <summary>
		/// Action-Name: OrgIndex.
		/// </summary>
		public const string OrgIndex = "OrgIndex";

		/// <summary>
		/// Action-Name: PrivacyPolicy.
		/// </summary>
		public const string PrivacyPolicy = "PrivacyPolicy";

		/// <summary>
		/// Action-Name: Register.
		/// </summary>
		public const string Register = "Register";

		/// <summary>
		/// Action-Name: RemoveBilling.
		/// </summary>
		public const string RemoveBilling = "RemoveBilling";

		/// <summary>
		/// Action-Name: RemoveInvitation.
		/// </summary>
		public const string RemoveInvitation = "RemoveInvitation";

		/// <summary>
		/// Action-Name: RemoveMember.
		/// </summary>
		public const string RemoveMember = "RemoveMember";

		/// <summary>
		/// Action-Name: RemoveMember.
		/// </summary>
		public const string RemoveUser = "RemoveUser";

		/// <summary>
		/// Action-Name: ResetPassword.
		/// </summary>
		public const string ResetPassword = "ResetPassword";

		/// <summary>
		/// Action-Name: ResetPasswordConfirmation.
		/// </summary>
		public const string ResetPasswordConfirmation = "ResetPasswordConfirmation";

		/// <summary>
		/// Action-Name: RouteHome.
		/// </summary>
		public const string RouteHome = "RouteHome";

		/// <summary>
		/// Action-Name: Subscribe.
		/// </summary>
		public const string Subscribe = "Subscribe";

		/// <summary>
		/// Action-Name: Unsubscribe.
		/// </summary>
		public const string Unsubscribe = "Unsubscribe";
		#endregion

		#region home
		/// <summary>
		/// Action-Name: ApplicationRedirect.
		/// </summary>
		public const string ApplicationRedirect = "ApplicationRedirect";

		/// <summary>
		/// Action-Name: Help.
		/// </summary>
		public const string Help = "Help";

		/// <summary>
		/// Action-Name: RouteProduct.
		/// </summary>
		public const string RouteProduct = "RouteProduct";
		#endregion

		#region shared
		/// <summary>
		/// Action-Name: FooterPartial.
		/// </summary>
		public const string FooterPartial = "FooterPartial";

		/// <summary>
		/// Action-Name: LogOnPartial.
		/// </summary>
		public const string LogOnPartial = "LogOnPartial";

		/// <summary>
		/// Action-Name: RouteProduct.
		/// </summary>
		public const string UpdateLanguage = "UpdateLanguage";
        #endregion

        #region TimeEntry
        /// <summary>
        /// Action-Name: CopyEntries.
        /// </summary>
        public const string CopyEntries = "CopyEntries";

        #endregion
    }

    /// <summary>
    /// Constants for controller Strings.
    /// </summary>
    public static class ControllerConstants
	{
		/// <summary>
		/// Controller-Name: Account.
		/// </summary>
		public const string Account = "Account";

		/// <summary>
		/// For when you need a key for storing your Controller value or some such thing.
		/// </summary>
		public const string Controller = "Controller";

		/// <summary>
		/// Controller-Name: Home.
		/// </summary>
		public const string Home = "Home";

		/// <summary>
		/// Controller-Name: Organization.
		/// </summary>
		public const string Organization = "Organization";

		/// <summary>
		/// Controller-Name: Shared.
		/// </summary>
		public const string Shared = "Shared";

		/// <summary>
		/// Controller-Name: Subscription.
		/// </summary>
		public const string Subscription = "Subscription";

        /// <summary>
        /// Controller-Name: TimeEntry.
        /// </summary>
        public const string TimeEntry = "TimeEntry";
	}

	/// <summary>
	/// Constants for view Strings.
	/// </summary>
	public static class ViewConstants
	{
		/// <summary>
		/// View-Name: AddBillingToSubscribe.
		/// </summary>
		public const string AddBillingToSubscribe = "AddBillingToSubscribe";

		/// <summary>
		/// View-Name: ConfirmRemoveBillingInformation.
		/// </summary>
		public const string ConfirmRemoveBillingInformation = "ConfirmRemoveBillingInformation";

		/// <summary>
		/// View-Name: Details.
		/// </summary>
		public const string Details = "Details";

		/// <summary>
		/// View-Name: Error.
		/// </summary>
		public const string Error = "Error";

		/// <summary>
		/// View-Name: Footer.
		/// </summary>
		public const string Footer = "_Footer";

		/// <summary>
		/// View-Name: ForgotPasswordConfirmation.
		/// </summary>
		public const string ForgotPasswordConfirmation = "ForgotPasswordConfirmation";

		/// <summary>
		/// View-Name: LogOnPartial.
		/// </summary>
		public const string LogOnPartial = "_LogOnPartial";

		/// <summary>
		/// View-Name: Subscribe.
		/// </summary>
		public const string Subscribe = "Subscribe";
	}
}