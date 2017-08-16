using AllyisApps.DBModel.StaffingManager;
using AllyisApps.Services.StaffingManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all staffing manager related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		///////////////////////
		// METHOD CONVERTERS //
		///////////////////////

		#region CreateMethods
		/// <summary>
		/// Creates a new position.
		/// </summary>
		/// <param name="position">The account object to be created. </param>
		/// <returns>The id of the created position. </returns>
		public int CreatePosition(Position position) => DBHelper.CreatePosition(ServiceObjectToDBEntity(position));

		/// <summary>
		/// Adds an Tag to the DB if there is not already another tag with the same name.
		/// </summary>
		/// <param name="name">The name of the tag to be added to the db. </param>
		/// <param name="positionId">The name of the tag to be added to the db. </param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use. </returns>
		public int CreateTag(string name, int positionId) => DBHelper.CreateTag(name, positionId);

		/// <summary>
		/// Adds a PositionTag to the DB when there is already another tag with the same name.
		/// </summary>
		/// <param name="tagId">The name of the tag to be added to the db. </param>
		/// <param name="positionId">The name of the tag to be added to the db. </param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use. </returns>
		public void AssignTag(int tagId, int positionId) => DBHelper.AssignTag(tagId, positionId);

		#endregion

		#region GetMethods
		/// <summary>
		/// Get Position method that pulls a position from the DB.
		/// </summary>
		/// <param name="positionId">ID of position to be pulled.</param>
		/// <returns>returns the service layer position object.</returns>
		public Position GetPosition(int positionId) => DBEntityToServiceObject(DBHelper.GetPositionById(positionId));

		/// <summary>
		/// Get Positions by Org ID method to pull a list of Service Layer positions objects based on their org ID.
		/// </summary>
		/// <param name="organizationId">the tagert organizations ID number. </param>
		/// <returns>A list of service layer Position Objects. </returns>
		public List<Position> GetPositionByOrganizationId(int organizationId) => DBHelper.GetPositionsByOrganizationId(organizationId).Select(DBEntityToServiceObject).ToList();

		/// <summary>
		/// Get Tags by a position Id method; pulls a list of all of the positions tags as service layer Tag Objects.
		/// </summary>
		/// <param name="positionId"> the position whose tags are to be pulled. </param>
		/// <returns>A list of the positions tags as service layer Tag objects</returns>
		public List<Tag> GetTagsByPositionId(int positionId) => DBHelper.GetTagsByPositionId(positionId).Select(DBEntityToServiceObject).ToList();

		/// <summary>
		/// Gets a list of ALL current tags.
		/// </summary>
		/// <returns>returns a list of all current tags. </returns>
		public List<Tag> GetTags() => DBHelper.GetTags().Select(DBEntityToServiceObject).ToList();

		#endregion

		#region UpdateMethods

		/// <summary>
		/// Updates the position with the given id.
		/// </summary>
		/// <param name="position">The service layer Position object to be passed to the DB and updated. </param>
		/// <returns>Returns the number of rows updated.</returns>
		public int UpdatePosition(Position position) => DBHelper.UpdatePosition(ServiceObjectToDBEntity(position));

		#endregion

		#region DeleteMethods

		/// <summary>
		/// Deletes a tag from the database.
		/// </summary>
		/// <param name="tagId">the id of the Tag to be removed from the db. </param>
		public void DeleteTag(int tagId) => DBHelper.DeleteTag(tagId);

		/// <summary>
		/// Deletes a position tag from the database; Doesnt delete the tag, just removes it from the position.
		/// </summary>
		/// <param name="tagId">tag id of the tag to be removed. </param>
		/// <param name="positionId">the position id that no longer has the tag. </param>
		public void DeletePositionTag(int tagId, int positionId) => DBHelper.DeletePositionTag(tagId, positionId);

		/// <summary>
		/// Removes a Position from the Database.
		/// </summary>
		/// <param name="positionId">ID of the Position to be removed from the DB. </param>
		public void DeletePosition(int positionId) => DBHelper.DeletePosition(positionId);

		#endregion

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
		/// <param name="position"> the PositionDBEntity to be converted to service layer object. </param>
		/// <returns>Returns a service layer Position Obejct. </returns>
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
