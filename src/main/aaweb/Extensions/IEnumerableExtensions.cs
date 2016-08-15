//------------------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.Extensions.IEnumerableExtensions
{
	/// <summary>
	/// IEnumerable extension class.
	/// </summary>
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Filters out duplicates based on an element.
		/// </summary>
		/// <typeparam name="TSource">The IEnumerable type source.</typeparam>
		/// <typeparam name="TKey">The type element used to filter.</typeparam>
		/// <param name="source">The IEnumerable list to filter through.</param>
		/// <param name="keySelector">The element to filter.</param>
		/// <returns>An IEnumerable without items with the duplicate value.</returns>
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			var knownKeys = new HashSet<TKey>();
			var filteredList = source.Where(element => knownKeys.Add(keySelector(element)));
			return filteredList;
		}
	}
}