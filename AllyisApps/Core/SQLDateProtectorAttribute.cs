//------------------------------------------------------------------------------
// <copyright file="SQLDateProtectorAttribute.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.Utilities
{
	/// <summary>
	/// Validator to guard against SQL date errors.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class SQLDateProtectorAttribute : ValidationAttribute
	{
		private readonly DateTime minDate = new DateTime(1753, 1, 1, 0, 0, 0);
		private readonly DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);

		/// <summary>
		/// Initializes a new instance of the <see cref="SQLDateProtectorAttribute" /> class.
		/// </summary>
		public SQLDateProtectorAttribute()
		{
			this.ErrorMessage = Resources.Errors.DateOutOfValidSQLRange; // a default value
		}

		/// <summary>
		/// Determines whether the provided date falls within the defined values.
		/// </summary>
		/// <param name="value">The input date.</param>
		/// <param name="validationContext">The context where the validation is occuring.</param>
		/// <returns>Description of the successful validation, or an error message.</returns>
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (!((DateTime?)value).HasValue)
			{
				return ValidationResult.Success;
			}
			else if ((DateTime)value <= this.maxDate && (DateTime)value >= this.minDate)
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult(ErrorMessage);
			}
		}
	}
}
