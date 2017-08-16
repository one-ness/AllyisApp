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


		public static Position DBEntityToServiceObject(PositionDBEntity position)
		{
			if (position == null) throw new ArgumentNullException("position", "Cannot accept a null position object");
			
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

		public static List<Tag> DBEntityToServiceObject(List<TagDBEntity> tags)
		{
			var taglist = tags
					.ConvertAll(x => new Tag(x.TagId, x.PositionId, x.TagName));
			return taglist;
		}

		public static Tag DBEntityToServiceObject(TagDBEntity tag)
		{
			if (tag == null) throw new ArgumentNullException("tag", "Cannot accept a null tag to be converted");

			return new Tag(tag.TagId, tag.PositionId, tag.TagName);
		}
	}
}
