//------------------------------------------------------------------------------
// <copyright file="Helpers.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

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

		/// <summary>
		/// Extension method for exception-safe async list iteration
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">List to iterate</param>
		/// <param name="func">Function to execute</param>
		/// <returns></returns>
		public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
		{
			foreach (T value in list)
			{
				await func(value);
			}
		}

		/// <summary>
		/// Replaces the last occurance of a string.
		/// </summary>
		/// <param name="source">The string to perform the operation on.</param>
		/// <param name="find">The string to get replaced.</param>
		/// <param name="replace">The string to replace the "find" string.</param>
		/// <returns></returns>
		public static string ReplaceLastOccurrence(string source, string find, string replace)
		{
			if (string.IsNullOrEmpty(find) || string.IsNullOrEmpty(source)) return source;

			int place = source.LastIndexOf(find, StringComparison.CurrentCulture);

			return place == -1
				? source
				: source.Remove(place, find.Length).Insert(place, replace);
		}

		/// <summary>
		/// Converts a c# DateTime object to js milliseconds, which can be used to initialize a js date or moment date object.
		/// </summary>
		/// <param name="dt">The datetime to convert</param>
		/// <returns></returns>
		public static long ToJavaScriptMilliseconds(this DateTime dt)
		{
			return (dt.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks) / 10000;
		}

		/// <summary>
		/// Returns hiddens for every IEnumerable item, with it's selected properties, if any memberPropsExpression provided.
		/// </summary>
		public static MvcHtmlString HiddenForEnumerable<TModel, TModelProperty>(this HtmlHelper<TModel> helper,
			Expression<Func<TModel, IEnumerable<TModelProperty>>> expression, params Expression<Func<TModelProperty, object>>[] memberPropsExpressions)
		{
			var sb = new StringBuilder();

			var membername = expression.GetMemberName();
			var model = helper.ViewData.Model;
			var list = expression.Compile()(model);

			var memPropsInfo = memberPropsExpressions.Select(x => new
			{
				MemberPropName = x.GetMemberName(),
				ListItemPropGetter = x.Compile()
			}).ToList();

			for (var i = 0; i < list.Count(); i++)
			{
				var listItem = list.ElementAt(i);
				if (memPropsInfo.Any())
				{
					foreach (var q in memPropsInfo)
					{
						sb.Append(helper.Hidden(string.Format("{0}[{1}].{2}", membername, i, q.MemberPropName), q.ListItemPropGetter(listItem)));
					}
				}
				else
				{
					sb.Append(helper.Hidden(string.Format("{0}[{1}]", membername, i), listItem));
				}
			}

			return new MvcHtmlString(sb.ToString());
		}

		/// <summary>
		/// A custom html helper extension for making a hidden input for enumerables in the view model.
		/// </summary>
		/// <typeparam name="TModel">The view model type.</typeparam>
		/// <typeparam name="TModelProperty">The type of the enumerable to make the hidden input for.</typeparam>
		/// <param name="helper">The Html helper class that we're extending.</param>
		/// <param name="expression">Lambda for getting the enumerable from the model. e.g. (model => model.MyList)</param>
		/// <param name="allPublicProps">Bool for whether or not to also make hiddens for all the props of the expression object.</param>
		/// <returns>MvcHtmlString parsable into the view model enumerable.</returns>
		/// <summary>
		/// Returns hiddens for every IEnumerable item, with it's all public writable properties, if allPublicProps set to true.
		/// </summary>
		public static MvcHtmlString HiddenForEnumerable<TModel, TModelProperty>(this HtmlHelper<TModel> helper,
			Expression<Func<TModel, IEnumerable<TModelProperty>>> expression, bool allPublicProps)
		{
			if (!allPublicProps)
				return HiddenForEnumerable(helper, expression);

			var sb = new StringBuilder();

			var membername = expression.GetMemberName();
			var model = helper.ViewData.Model;
			var list = expression.Compile()(model);

			var type = typeof(TModelProperty);
			var memPropsInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.GetSetMethod(false) != null && x.GetGetMethod(false) != null)
				.Select(x => new
				{
					MemberPropName = x.Name,
					ListItemPropGetter = (Func<TModelProperty, object>)(p => x.GetValue(p, null))
				}).ToList();

			if (memPropsInfo.Count == 0)
				return HiddenForEnumerable(helper, expression);

			for (var i = 0; i < list.Count(); i++)
			{
				var listItem = list.ElementAt(i);
				foreach (var q in memPropsInfo)
				{
					sb.Append(helper.Hidden(string.Format("{0}[{1}].{2}", membername, i, q.MemberPropName), q.ListItemPropGetter(listItem)));
				}
			}

			return new MvcHtmlString(sb.ToString());
		}

		/// <summary>
		/// Gets the name of the view model object from the inputted exression.
		/// </summary>
		/// <typeparam name="TModel">View model.</typeparam>
		/// <typeparam name="T">Type of the object in the expression</typeparam>
		/// <param name="input">Expression.</param>
		/// <returns>Name of the view model object from the inputted exression.</returns>
		private static string GetMemberName<TModel, T>(this Expression<Func<TModel, T>> input)
		{
			if (input == null)
				return null;

			MemberExpression memberExp = null;

			if (input.Body.NodeType == ExpressionType.MemberAccess)
				memberExp = input.Body as MemberExpression;
			else if (input.Body.NodeType == ExpressionType.Convert)
				memberExp = ((UnaryExpression)input.Body).Operand as MemberExpression;

			return memberExp?.Member.Name;
		}

		/// <summary>
		/// Returns one date for all datetimes within a week period starting at startOfWeek
		/// </summary>
		/// <param name="dt">DateTime that we're extending</param>
		/// <param name="startOfWeek">The start of the week period to group by</param>
		/// <returns></returns>
		public static DateTime GroupByStartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			int diff = dt.DayOfWeek - startOfWeek;
			if (diff < 0)
			{
				diff += 7;
			}
			return dt.AddDays(-1 * diff).Date;
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
}