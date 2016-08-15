//------------------------------------------------------------------------------
// <copyright file="UploadCsvFileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Utilities;
using AllyisApps.ViewModels;
using CsvHelper;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Process an uploaded CSVFile and send invitations for those users.
		/// </summary>
		/// <param name="upload">Caught file upload.</param>
		/// <returns>An ActionResult.</returns>
		[HttpPost]
		public async Task<ActionResult> UploadCsvFile(HttpPostedFileBase upload)
		{
			string[] validExtentions = { "CSV", "TXT" };
			List<string> extentionList = new List<string>(validExtentions);

			List<string> result = new List<string>();
			if (upload != null && upload.ContentLength > 0)
			{
				string fileExt = upload.FileName.Split('.').Last().ToUpper();
				bool extValid = false;
				foreach (string s in extentionList)
				{
					if (s == fileExt)
					{
						extValid = true;
						break;
					}
				}

				if (!extValid)
				{
					string notification = string.Format("File type {0} not valid", fileExt);
					Notifications.Add(new BootstrapAlert(notification, Variety.Warning));
					return this.RedirectToAction(ActionConstants.Add);
				}

				StreamReader reader = new StreamReader(upload.InputStream);
				CsvReader csvread = new CsvReader(reader);
				csvread.Configuration.RegisterClassMap<InviteMap>();
				var invites = csvread.GetRecords<OrganizationAddMembersViewModel>().ToList();
				List<string> emails = new List<string>();
				List<OrganizationAddMembersViewModel> rvites = new List<OrganizationAddMembersViewModel>();
				foreach (OrganizationAddMembersViewModel invite in invites)
				{
					if (emails.Contains(invite.Email) || OrgService.GetOrgUserFirstName(invite.Email) != null)
					{
						emails.Add(invite.Email);
						rvites.Add(invite);
					}
					else
					{
						invite.OrganizationId = UserContext.ChosenOrganizationId;
						emails.Add(invite.Email);
					}
				}

				foreach (OrganizationAddMembersViewModel invite in rvites)
				{
					invites.Remove(invite);
				}

				foreach (OrganizationAddMembersViewModel invite in invites)
				{
					try
					{
						invite.Organization = OrgService.GetOrganization(UserContext.ChosenOrganizationId);
						await this.ProcessUserInput(invite);
					}
					catch
					{
						string res = string.Format("{0} {1}", invite.FirstName, invite.LastName);
						result.Add(res);
					}
				}
			}

			return this.RedirectToAction(ActionConstants.Manage);
		}
	}
}
