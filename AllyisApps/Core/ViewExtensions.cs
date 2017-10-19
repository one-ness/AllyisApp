//------------------------------------------------------------------------------
// <copyright file="ViewExtensions.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace AllyisApps.Extensions.ViewExtensions
{
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
