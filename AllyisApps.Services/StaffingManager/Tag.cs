using System;

namespace AllyisApps.Services.StaffingManager
{
	/// <summary>
	/// Represents the Address table in the database.
	/// </summary>
	public class Tag
	{
		private int? tagId;
		private int? positionId;
		private string tagName;

		/// <summary>
		/// Tag constructor for use in passing DBEntity object information to the view
		/// </summary>
		/// <param name="tagId"></param>
		/// <param name="positionId"></param>
		/// <param name="tagName"></param>
		public Tag (int tagId, int positionId, string tagName)
		{
			TagId = tagId;
			PositionId = positionId;
			TagName = tagName;
		}

		/// <summary>
		/// Gets or sets the Tags Id
		/// </summary>
		public int? TagId
		{
			get { return tagId; }
			set { tagId = value; }
		}

		/// <summary>
		/// Gets or sets Position Id
		/// </summary>
		public int? PositionId
		{
			get { return positionId; }
			set { positionId = value; }
		}

		/// <summary>
		/// Gets or sets the Tag's Name
		/// </summary>
		public string TagName
		{
			get { return tagName; }
			set
			{
				if (value.Length > 32 || value.Length == 0) throw new ArgumentOutOfRangeException("TagName", value, "Tag Name must be between 1 and 32 characters in length");
				tagName = value;
			}
		}

	}
}
