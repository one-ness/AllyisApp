//------------------------------------------------------------------------------
// <copyright file="BootstrapAlert.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace AllyisApps.Core.Alert
{
	/// <summary>
	/// A wrapper for a notification dat to be displayed in the view.
	/// Add notifications with
	/// this.Notifications.Add(new BootstrapAlert("text", BootstrapAlert.Variety.Success));
	/// in a controller that subclasses AllyisApps.Core.BaseController.
	/// </summary>
	public class BootstrapAlert
	{
		private static readonly Variety DefaultVariety = Variety.Primary;

		/// <summary>
		/// Initializes a new instance of the <see cref="BootstrapAlert" /> class.
		/// </summary>
		/// <param name="text">The notification Text.</param>
		public BootstrapAlert(string text)
			: this(text, DefaultVariety)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BootstrapAlert" /> class.
		/// </summary>
		/// <param name="text">The notification Text.</param>
		/// <param name="variety">The type of notification.</param>
		public BootstrapAlert(string text, Variety variety)
		{
			this.Text = text;
			this.Type = variety;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BootstrapAlert" /> class.
		/// </summary>
		/// <param name="text">The notification Text.</param>
		/// <param name="varietyName">The name of the type of notification.</param>
		public BootstrapAlert(string text, string varietyName)
		{
			this.Text = text;
			this.Type = Variety.GetByName(varietyName);
		}

		/// <summary>
		/// Gets the text to display.
		/// </summary>
		public string Text { get; internal set; }

		/// <summary>
		/// Gets the type of the alert.
		/// </summary>
		public Variety Type { get; internal set; }
	}

	/// <summary>
	/// An enhanced Enum for the kind of alert.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "This is a smart enum.")]
	public sealed class Variety
	{
		/// <summary>
		/// Enum value for the "bg-primary text-primary" Bootstrap alert.
		/// </summary>
		public static readonly Variety Primary = new Variety("bg-primary text-primary");

		/// <summary>
		/// Enum value for the "bg-success text-success" Bootstrap alert.
		/// </summary>
		public static readonly Variety Success = new Variety("bg-success text-success");

		/// <summary>
		/// Enum value for the "bg-info text-info" Bootstrap alert.
		/// </summary>
		public static readonly Variety Info = new Variety("bg-info text-info");

		/// <summary>
		/// Enum value for the "bg-warning text-warning" Bootstrap alert.
		/// </summary>
		public static readonly Variety Warning = new Variety("bg-warning text-warning");

		/// <summary>
		/// Enum value for the "bg-danger text-danger" Bootstrap alert.
		/// </summary>
		public static readonly Variety Danger = new Variety("bg-danger text-danger");

		/// <summary>
		/// Initializes a new instance of the <see cref="Variety"/> class and prevents it from being created outside this class.
		/// </summary>
		/// <param name="classes">The string of CSS classes to include in the display markup.</param>
		private Variety(string classes)
		{
			this.Classes = classes;
		}

		/// <summary>
		/// Gets the CSS classes to apply to style the result.
		/// </summary>
		public string Classes { get; internal set; }

		/// <summary>
		/// Gets a notification display variety by name. Used in creating notifications via ajax.
		/// </summary>
		/// <param name="name">The name of the variety to get.</param>
		/// <returns>The Variety object with specified by name, else returns Variety.Primary.</returns>
		public static Variety GetByName(string name)
		{
			Variety result = Variety.Primary;
			switch (name.ToLower())
			{
				case "primary":
					result = Variety.Primary;
					break;

				case "success":
					result = Variety.Success;
					break;

				case "info":
					result = Variety.Info;
					break;

				case "warning":
					result = Variety.Warning;
					break;

				case "Danger":
					result = Variety.Danger;
					break;
			}

			return result;
		}
	}
}