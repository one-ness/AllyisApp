//------------------------------------------------------------------------------
// <copyright file="MinAgeAttribute.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.Utilities
{
	/// <summary>
	/// Validator for a minimum age.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class MinAgeAttribute : ValidationAttribute
	{
		private readonly int age;

		/// <summary>
		/// Initializes a new instance of the <see cref="MinAgeAttribute" /> class.
		/// </summary>
		/// <param name="age">The defined minimum user age.</param>
		public MinAgeAttribute(int age)
		{
			this.age = age;
		}

		/// <summary>
		/// Determines whether the age provided is valid.
		/// </summary>
		/// <param name="value">The age provided.</param>
		/// <param name="validationContext">Context that validation occured in.</param>
		/// <returns>ValidationResult describing whether the age is valid, or an error message.</returns>
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null)
			{
				return new ValidationResult(Resources.Errors.ArugmentNullMessage);
			}
			else if ((DateTime)value <= DateTime.Now.AddYears(-this.age))
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult(Resources.Errors.AgeTooLow);
			}
		}
	}
}
