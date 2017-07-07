﻿//------------------------------------------------------------------------------
// <copyright file="ViewExtensions.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace AllyisApps.Extensions.ViewExtensions
{
	/// <summary>
	/// View extension class.
	/// </summary>
	public static class ViewExtensions
	{
		/// <summary>
		/// Creates a confirm Dialog from the given string.
		/// <para>
		///   Embed it as the text for the onclick event of links and onsubmit event of forms and it will block their submission without confirmation.
		/// </para>
		/// </summary>
		/// <param name="htmlHelper">The class this is attached to.</param>
		/// <param name="alertText">The string to show.</param>
		/// <returns>Javascript of the describing the confirmation string.</returns>
		public static string ConfirmDialog(this HtmlHelper htmlHelper, string alertText)
		{
			return string.Format("return confirm('{0}');", alertText);
		}

        /// <summary>
		/// Creates a confirm Dialog from the given string.
		/// <para>
		///   Embed it as the text for the onclick event of links and onsubmit event of forms and it will block their submission without confirmation.
		/// </para>
		/// </summary>
		/// <param name="alertText">The string to show.</param>
		/// <returns>Javascript of the describing the confirmation string.</returns>
		public static string ConfirmDialog(string alertText)
        {
            return string.Format("return confirm('{0}');", alertText);
        }

        /// <summary>
        /// Creates a confirm Dialog from the given resource string, after replacing all variables.
        /// <para>
        ///   Embed it as the text for the onclick event of links and onsubmit event of forms and it will block their submission without confirmation.
        /// </para>
        /// <para>
        ///   The Format of the variables construct should be "variable" => "value", where
        ///   all instances of "%variable%" within the string will be replaced with "value"
        /// </para>
        /// </summary>
        /// <param name="htmlHelper">The class this is attached to.</param>
        /// <param name="resourceString">The resource string to process.</param>
        /// <param name="variables">The variables to replace. Should be specified as "variable" => "value" where "%variable%" will be replaced with "value".</param>
        /// <returns>A javascript function.</returns>
        public static string ConfirmDialog(this HtmlHelper htmlHelper, string resourceString, Dictionary<string, string> variables)
		{
			return ViewExtensions.ConfirmDialog(htmlHelper, Helpers.ResourceReplace(resourceString, variables));
		}

		/// <summary>
		/// Creates a confirm Dialog from the given resource string, after replacing all instances of the varabie.
		/// <para>
		///   Embed it as the text for the onclick event of links and onsubmit event of forms and it will block their submission without confirmation.
		/// </para>
		/// </summary>
		/// <param name="htmlHelper">The class this is attached to.</param>
		/// <param name="resourceString">The resource string to process.</param>
		/// <param name="variable">The variable to replace. Should be "variable" where "%variable%" will be replaced.</param>
		/// <param name="value">The value to replace the variable with.</param>
		/// <returns>Javascript describing the confirmation message with a string replacement.</returns>
		public static string ConfirmDialog(this HtmlHelper htmlHelper, string resourceString, string variable, string value)
		{
			return ViewExtensions.ConfirmDialog(htmlHelper, Helpers.ResourceReplace(resourceString, variable, value));
		}

		/// <summary>
		/// Extension method for creating a disabled textbox.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <typeparam name="TProperty">The type of the propery being displayed.</typeparam>
		/// <param name="htmlHelper">The HtmlHelper object being extended.</param>
		/// <param name="expression">The expression for which value to display.</param>
		/// <param name="disabled">Whether or not the textbox is disabled.</param>
		/// <param name="htmlAttributes">Any other Html attributes.</param>
		/// <returns>Html creating an enabled or disabled textbox.</returns>
		public static MvcHtmlString TextBoxDisabledFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			bool disabled,
			object htmlAttributes = null)
		{
			return TextBoxDisabledFor(htmlHelper, expression, disabled, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		/// <summary>
		/// Helper for creating an HTML textbox that is disabled based on the disabled param.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <typeparam name="TProperty">The type of the propery being displayed.</typeparam>
		/// <param name="htmlHelper">The HtmlHelper object being extended.</param>
		/// <param name="expression">The expression for which value to display.</param>
		/// <param name="disabled">Whether or not the textbox is disabled.</param>
		/// <param name="htmlAttributes">Any other Html attributes.</param>
		/// <returns>Html creating an enabled or disabled textbox.</returns>
		public static MvcHtmlString TextBoxDisabledFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool disabled, IDictionary<string, object> htmlAttributes)
		{
			if (disabled)
			{
				htmlAttributes["disabled"] = "disabled";
			}

			return htmlHelper.TextBoxFor(expression, htmlAttributes);
		}

		/// <summary>
		/// Helper for creating an HTML dropdown list that is disabled based on the disabled param.
		/// </summary>
		/// <typeparam name="TModel">Type of the Model.</typeparam>
		/// <param name="htmlHelper">The HtmlHelper object being extended.</param>
		/// <param name="name">The name of the dropdown.</param>
		/// <param name="items">The list of items in the dropdown.</param>
		/// <param name="disabled">Whether or not the dropdown is disabled.</param>
		/// <param name="htmlAttributes">Any other Html attributes.</param>
		/// <returns>Html creating an enabled or disabled dropdown list.</returns>
		public static MvcHtmlString DropDownListDisabled<TModel>(
			this HtmlHelper<TModel> htmlHelper,
			string name,
			IEnumerable<SelectListItem> items,
			bool disabled,
			object htmlAttributes = null)
		{
			return DropDownListDisabled(htmlHelper, name, items, disabled, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		/// <summary>
		/// Helper for creating an HTML dropdown list that is disabled based on the disabled param.
		/// </summary>
		/// <typeparam name="TModel">Type of the Model.</typeparam>
		/// <param name="htmlHelper">The HtmlHelper object being extended.</param>
		/// <param name="name">The name of the dropdown.</param>
		/// <param name="items">The list of items in the dropdown.</param>
		/// <param name="disabled">Whether or not the dropdown is disabled.</param>
		/// <param name="htmlAttributes">Any other Html attributes.</param>
		/// <returns>Html creating an enabled or disabled dropdown list.</returns>
		public static MvcHtmlString DropDownListDisabled<TModel>(
			this HtmlHelper<TModel> htmlHelper,
			string name,
			IEnumerable<SelectListItem> items,
			bool disabled,
			IDictionary<string, object> htmlAttributes)
		{
			if (disabled)
			{
				htmlAttributes["disabled"] = "disabled";
			}

			return htmlHelper.DropDownList(name, items, htmlAttributes);
		}

		/// <summary>
		/// Extension for generating Htmlhelper strings without ids.
		/// </summary>
		/// <param name="helper">The MvcHtmlString generated by the htmlhelper.</param>
		/// <returns>An MvcHtmlString without the id attribute.</returns>
		public static MvcHtmlString RemoveIdAttribute(this MvcHtmlString helper)
		{
			if (helper == null)
			{
				throw new ArgumentNullException();
			}

			var element = helper.ToString();
			var regex = new Regex("(id=)['\"][^'\"]*['\"]", RegexOptions.IgnoreCase | RegexOptions.Compiled); // id="things" and id='things'
			return MvcHtmlString.Create(regex.Replace(element, string.Empty));
		}
	}

	/// <summary>
	/// Required Label Extensions. Based on code found here http://stackoverflow.com/questions/5940506/how-can-i-modify-labelfor-to-display-an-asterisk-on-required-fields.
	/// </summary>
	public static class LabelExtensions
	{
		/// /// <summary>
		/// Create a label with the "required" class, if necessary.
		/// </summary>
		/// <typeparam name="TModel">The type of the model passed to the view.</typeparam>
		/// <typeparam name="TValue">The type of the value to search for.</typeparam>
		/// <param name="html">The Extension target.</param>
		/// <param name="expression">The field this label represents.</param>
		/// <param name="specifiedLabelText">Specific text input.</param>
		/// <returns>A Label string.</returns>
		public static MvcHtmlString ReqLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string specifiedLabelText = "")
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
			string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
			string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

			if (string.IsNullOrEmpty(labelText) && string.IsNullOrEmpty(specifiedLabelText))
			{
				return MvcHtmlString.Empty;
			}

			var sb = new StringBuilder();
			if (!string.IsNullOrEmpty(specifiedLabelText))
			{
				sb.Append(specifiedLabelText);
			}
			else
			{
				sb.Append(labelText);
			}

			var tag = new TagBuilder("label");

			if (metadata.IsRequired)
			{
				tag.AddCssClass("required");
			}

			tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
			tag.SetInnerText(sb.ToString());

			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
	}
}
