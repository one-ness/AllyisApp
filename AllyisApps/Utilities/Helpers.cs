﻿//------------------------------------------------------------------------------
// <copyright file="Helpers.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using AllyisApps.ViewModels.TimeTracker.Project;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Utilities
{
	/// <summary>
	/// Helper class.
	/// </summary>
	public static class Helpers
	{
		/// <summary>
		/// Reads the app setting in the config file for the given key.
		/// </summary>
		/// <param name="key">
		/// They configuration key.
		/// .</param>
		/// <returns>
		/// Returns appliction settings.
		/// .</returns>
		public static string ReadAppSetting(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}

		/// <summary>
		/// Replaces all given variable instances in the string.
		/// <para>
		///   The Format of the variables construct should be "variable" => "value", where
		///   all instances of "%variable%" within the string will be replaced with "value"
		/// </para>.
		/// </summary>
		/// <param name="resourceString">
		/// The resource string to process.
		/// .</param>
		/// <param name="variables">
		/// The variables to replace. Should be specified as "variable" => "value" where "%variable%" will be replaced with "value".
		/// .</param>
		/// <returns>The input resource string with the values replaced.</returns>
		public static string ResourceReplace(string resourceString, Dictionary<string, string> variables)
		{
			string result = resourceString;
			foreach (KeyValuePair<string, string> kv in variables)
			{
				string res = string.Format("%{0}%", kv.Key);
				result = result.Replace(res, kv.Value);
			}

			return result;
		}

		/// <summary>
		/// Proxy for ResourceReplace for the case where there is only one variable to replace.
		/// Replaces all given variable instances in the string.
		/// <para>
		///   The Format of the variables construct should be "variable" => "value", where
		///   all instances of "%variable%" within the string will be replaced with "value"
		/// </para>.
		/// </summary>
		/// <param name="resourceString">The resource string to process.</param>
		/// <param name="variable">The variable string to replace. Should be "variable" where "%variable%" will be replaced.</param>
		/// <param name="value">The value to replace the variable with.</param>
		/// <returns>The input resourceString with the variable replaced with value.</returns>
		public static string ResourceReplace(string resourceString, string variable, string value)
		{
			string res = string.Format("%{0}%", variable);
			return resourceString.Replace(res, value);
		}
	}

	/// <summary>
	/// Render helper for use in Controllers.
	/// </summary>
	public static class RenderHelper
	{
		/// <summary>
		/// Render a partial view to a string.
		/// </summary>
		/// <param name="controller">The controller to render the view for.</param>
		/// <param name="viewName">The view to render.</param>
		/// <param name="model">The model to pass to the partial.</param>
		/// <returns>The rendered partial view.</returns>
		public static string PartialView(Controller controller, string viewName, object model)
		{
			controller.ViewData.Model = model;

			using (var sw = new StringWriter())
			{
				var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
				var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);

				viewResult.View.Render(viewContext, sw);
				viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

				return sw.ToString();
			}
		}
	}

	/// <summary>
	/// Helper for commonly shared 
	/// </summary>
	public static class ViewModelHelper
	{
		/// <summary>
		/// Constuct View Model 
		/// </summary>
		/// <param name="proj"></param>
		/// <returns></returns>
		public static CompleteProjectViewModel ConstuctCompleteProjectViewModel(CompleteProjectInfo proj)
		{
			return new CompleteProjectViewModel()
			{
				CanEditProject = proj.CanEditProject,
				CreatedUtc = proj.CreatedUtc,
				CustomerId = proj.CustomerId,
				CustomerName = proj.CustomerName,
				CustomerOrgId = proj.CustomerOrgId,
				EndDate = proj.EndDate,
				IsActive = proj.IsActive,
				IsCustomerActive = proj.IsCustomerActive,
				IsProjectUser = proj.IsProjectUser,
				IsUserActive = proj.IsUserActive,
				OrganizationId = proj.OrganizationId,
				OrganizationName = proj.OrganizationName,
				OrganizationRoleId = proj.OrganizationRoleId,
				PriceType = proj.PriceType,
				ProjectId = proj.ProjectId,
				ProjectName = proj.ProjectName,
				ProjectOrgId = proj.ProjectOrgId,
				StartDate = proj.StartDate
			};
		}

		
	}
}