//------------------------------------------------------------------------------
// <copyright file="BasePoco.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.Serialization;

[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1636:FileHeaderCopyrightTextMustMatch", Justification = "This is a Microsoft generated file")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1641:FileHeaderCompanyNameTextMustMatch", Justification = "This is a Microsoft generated file")]

namespace AllyisApps.DBModel
{
	/// <summary>
	/// Base class with facility for tracking property changes.
	/// </summary>
	/// <remarks>
	/// 1. Uses reflection to get the property name. Possible performance issues!
	/// 2. Based on code found here: http://www.codeproject.com/Articles/41791/Almost-automatic-INotifyPropertyChanged-automatic.
	/// </remarks>	
	[DataContract]
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "This is not mis-spelled")]
	public class BasePoco : INotifyPropertyChanged
	{
		/// <summary>
		/// Gets or sets the flag to enable or disable tracking of property changes.
		/// </summary>
		private bool trackChanges = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="BasePoco"/> class.
		/// </summary>
		public BasePoco()
		{
			this.PropertyChanges = new Dictionary<string, List<object>>();
		}

		/// <summary>
		/// Gets or sets the required event handler for the INotifyPropertyChanged interface.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Gets the property changes collection.
		/// Dictionary(property name, List of object values).
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Necessary to store historical property data without code re-design")]
		public Dictionary<string, List<object>> PropertyChanges { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this object is dirty (modified).
		/// </summary>
		public bool IsDirty
		{
			get
			{
				return this.PropertyChanges.Count > 0;
			}
		}

		/// <summary>
		/// Enable tracking.
		/// </summary>
		public void StartTracking()
		{
			this.trackChanges = true;
		}

		/// <summary>
		/// Disable tracking.
		/// </summary>
		public void StopTracking()
		{
			this.trackChanges = false;
		}

		/// <summary>
		/// Clears all the property changes, should be called after the property changes have been logged.
		/// </summary>
		public void ClearPropertyChanges()
		{
			this.PropertyChanges.Clear();
		}

		/// <summary>
		/// Change the property if required and throw event.
		/// Note: This must be called by all property setters in the derived classes.
		/// </summary>
		/// <param name="field">A reference to the field being changed.</param>
		/// <param name="property">An expression representing the base object and field being changed.</param>
		/// <param name="value">The new value of the field.</param>
		/// <typeparam name="T">The derived class type.</typeparam>
		/// <typeparam name="TF">The type of field being modified.</typeparam>		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Required.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "F", Justification = "Typeparam.")]
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Must do it this way to record historical data of properties without code re-design")]
		protected void ApplyPropertyChange<T, TF>(ref TF field, Expression<Func<T, object>> property, TF value)
		{
			// only do this if the value changes.
			if (!object.Equals(field, value))
			{
				// get property name
				if (property == null)
				{
					throw new InvalidOperationException("you must specify a property");
				}

				string propertyName = GetPropertyName(property);

				// set the new value
				TF oldValue = field;
				field = value;

				// is tracking enabled?
				if (this.trackChanges)
				{
					// yes, store the values.
					if (this.PropertyChanges.ContainsKey(propertyName))
					{
						this.PropertyChanges[propertyName].Add(value);
					}
					else
					{
						this.PropertyChanges.Add(propertyName, new List<object>());
						this.PropertyChanges[propertyName].Add(oldValue);
						this.PropertyChanges[propertyName].Add(value);
					}

					// notify change
					if (this.PropertyChanged != null)
					{
						this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
					}
				}
			}
		}

		/// <summary>
		/// Get the name of the given property in the given object.
		/// </summary>		
		/// <param name="property">An expression representing the source object and property modified.</param>
		/// <typeparam name="T">The type of the class containing the property.</typeparam>
		/// <returns>The name of the property being changed.</returns>
		private static string GetPropertyName<T>(Expression<Func<T, object>> property)
		{
			MemberExpression propertyExpression = null;

			if (property.NodeType == ExpressionType.Convert)
			{
				UnaryExpression body = property.Body as UnaryExpression;
				if (body != null)
				{
					propertyExpression = body.Operand as MemberExpression;
				}
			}
			else if (property.NodeType == ExpressionType.MemberAccess)
			{
				propertyExpression = property.Body as MemberExpression;
			}
			else if (property.NodeType == ExpressionType.Lambda)
			{
				UnaryExpression exp = property.Body as UnaryExpression;
				if (exp != null)
				{
					propertyExpression = exp.Operand as MemberExpression;
				}
				else
				{
					propertyExpression = property.Body as MemberExpression;
				}
			}

			if (propertyExpression == null)
			{
				throw new ArgumentException("Not a member access expression.", "property");
			}

			return propertyExpression.Member.Name;
		}
	}
}
