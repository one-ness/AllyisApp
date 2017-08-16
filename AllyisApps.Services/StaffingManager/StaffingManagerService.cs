using AllyisApps.DBModel.StaffingManager;
using AllyisApps.Services.StaffingManager;
using System;
using System.Collections.Generic;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all staffing manager related operations.
	/// </summary>
	public partial class AppService : BaseService
	{

		//////////////////////////////////////////
		// SERVICE LAYER to DB LAYER CONVERTERS //
		//////////////////////////////////////////

		/// <summary>
		/// Converts a Position service layer object to a PositionDBEntity object.
		/// </summary>
		/// <param name="position"> The Position service layer obejct. </param>
		/// <returns> Returns the PositionDBEntity for the DB layer.  </returns>
		public static PositionDBEntity ServiceObjectToDBEntity(Position position)
		{
			if (position == null) throw new ArgumentNullException("position", "Cannot accept a null position object");

			return new PositionDBEntity
			{
				OrganizationId = position.OrganizationId,
				CustomerId = position.CustomerId,
				AddressId = position.AddressId,
				PositionStatusId = position.PositionStatusId,
				PositionTitle = position.PositionTitle,
				DurationMonths = position.DurationMonths,
				EmploymentTypeId = position.EmploymentTypeId,
				PositionCount = position.PositionCount,
				RequiredSkills = position.RequiredSkills,
				PositionLevelId = position.PositionLevelId,
				Address = position.Address,
				City = position.City,
				State = position.State,
				Country = position.Country,
				PostalCode = position.PostalCode,
				PositionId = (int)position.PositionId,
				PositionCreatedUtc = (DateTime)position.PositionCreatedUtc,
				PositionModifiedUtc = (DateTime)position.PositionModifiedUtc,
				StartDate = (DateTime)position.StartDate,
				BillingRateFrequency = (int)position.BillingRateFrequency,
				BillingRateAmount = (int)position.BillingRateAmount,
				JobResponsibilities = position.JobResponsibilities,
				DesiredSkills = position.DesiredSkills,
				Tags = ServiceObjectToDBEntity(position.Tags),
				HiringManager = position.HiringManager,
				TeamName = position.TeamName
			};
		}


		/// <summary>
		/// Converts a list of Tag service layer objects to a TagDBEntity object list.
		/// </summary>
		/// <param name="tags"> The List of tag service layer objects to be converted. </param>
		/// <returns> Returns a list of TagDBEntity objects for the DB layer.  </returns>
		public static List<TagDBEntity> ServiceObjectToDBEntity(List<Tag> tags)
		{
			if (tags == null) throw new ArgumentNullException(nameof(tags), "Cannot accept null list of tags to be converted.");

			var taglist = tags
					.ConvertAll(x => new TagDBEntity { TagId = (int)x.TagId, PositionId = (int)x.PositionId, TagName = x.TagName });
			return taglist;
		}

		/// <summary>
		/// Converts a Tag service layer object to a TagDBEntity object.
		/// </summary>
		/// <param name="tag"> Thetag service layer object to be converted. </param>
		/// <returns> Returns a TagDBEntity object for the DB layer.  </returns>
		public static TagDBEntity ServiceObjectToDBEntity(Tag tag)
		{
			if (tag == null) throw new ArgumentNullException(nameof(tag), "Cannot accept a null tag to be converted.");

			return new TagDBEntity { TagId = (int)tag.TagId, PositionId = (int)tag.PositionId, TagName = tag.TagName };
		}


		//////////////////////////////////////////
		// DB LAYER to SERVICE LAYER CONVERTERS //
		//////////////////////////////////////////

		/// <summary>
		/// Converts a DB Layer PositionDBEntity object to a service layer Position Object.
		/// </summary>
		/// <param name="position"> the PositionDBEntity to be converted to service layer object</param>
		/// <returns>Returns a service layer Position Obejct</returns>
		public static Position DBEntityToServiceObject(PositionDBEntity position)
		{
			if (position == null) throw new ArgumentNullException(nameof(position), "Cannot accept a null position object.");
			
			return new Position {
				OrganizationId = position.OrganizationId,
				CustomerId = position.CustomerId,
				AddressId = position.AddressId,
				PositionStatusId = position.PositionStatusId,
				PositionTitle = position.PositionTitle,
				DurationMonths = position.DurationMonths,
				EmploymentTypeId = position.EmploymentTypeId,
				PositionCount = position.PositionCount,
				RequiredSkills = position.RequiredSkills,
				PositionLevelId = position.PositionLevelId,
				Address = position.Address,
				City = position.City,
				State = position.State,
				Country = position.Country,
				PostalCode = position.PostalCode,
				PositionId = position.PositionId,
				PositionCreatedUtc = position.PositionCreatedUtc,
				PositionModifiedUtc = position.PositionModifiedUtc,
				StartDate = position.StartDate,
				BillingRateFrequency = position.BillingRateFrequency,
				BillingRateAmount = position.BillingRateAmount,
				JobResponsibilities = position.JobResponsibilities,
				DesiredSkills = position.DesiredSkills,
				Tags = DBEntityToServiceObject(position.Tags),
				HiringManager = position.HiringManager,
				TeamName = position.TeamName
				};
		}

		/// <summary>
		/// Converts a list of TagDBEntity DB layer objects to a list of Tag service layer object.
		/// </summary>
		/// <param name="tags"> The List of tag DB layer objects to be converted. </param>
		/// <returns> Returns a list of Tag service layer objects.  </returns>
		public static List<Tag> DBEntityToServiceObject(List<TagDBEntity> tags)
		{
			var taglist = tags
					.ConvertAll(x => new Tag {TagId = x.TagId, PositionId = x.PositionId, TagName = x.TagName });
			return taglist;
		}

		/// <summary>
		/// Converts a TagDBEntity DB layer object to a Tag service layer object.
		/// </summary>
		/// <param name="tag"> The tag DB layer object to be converted. </param>
		/// <returns> Returns a Tag service layer object.  </returns>
		public static Tag DBEntityToServiceObject(TagDBEntity tag)
		{
			if (tag == null) throw new ArgumentNullException(nameof(tag), "Cannot accept a null tag to be converted");

			return new Tag {TagId = tag.TagId, PositionId = tag.PositionId, TagName = tag.TagName };
		}
	}
}
