//------------------------------------------------------------------------------
// <copyright file="Serializer.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace AllyisApps.Lib
{
	/// <summary>
	/// Serialization utilities.
	/// </summary>
	public static class Serializer
	{
		/// <summary>
		/// Serialize the given object.
		/// </summary>
		/// <param name="data">The data to serialize.</param>
		/// <returns>The data serialized as a string.</returns>
		public static string SerilalizeToJson(object data)
		{
			return JsonConvert.SerializeObject(data);
		}

		/// <summary>
		/// Deserialize the given object.
		/// </summary>
		/// <typeparam name="T">The type parameter.</typeparam>
		/// <param name="data">The data to deserialize.</param>
		/// <returns>The deserialized data.</returns>
		public static T DeserializeFromJson<T>(string data)
		{
			return JsonConvert.DeserializeObject<T>(data);
		}
	}
}
