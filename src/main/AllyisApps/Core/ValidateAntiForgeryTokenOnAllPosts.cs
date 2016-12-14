//------------------------------------------------------------------------------
// <copyright file="ValidateAntiForgeryTokenOnAllPosts.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------
using System;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AllyisApps.Core
{
	/// <summary>
	/// Validate Anti Forgery Token On All Posts.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
	{
		/// <summary>
		/// On authorization.
		/// </summary>
		/// <param name="filterContext">Filter context.</param>
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}

			HttpRequestBase request = filterContext.HttpContext.Request;

			// Only validate POSTs
			if (request.HttpMethod == WebRequestMethods.Http.Post)
			{
				// Ajax POSTs and normal form posts have to be treated differently when it comes
				// to validating the AntiForgeryToken
				if (request.IsAjaxRequest())
				{
					HttpCookie antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

					string cookieValue = antiForgeryCookie != null
						? antiForgeryCookie.Value
						: null;

					AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
				}
				else
				{
					new ValidateAntiForgeryTokenAttribute()
						.OnAuthorization(filterContext);
				}
			}
		}
	}
}