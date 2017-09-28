CREATE PROCEDURE [StaffingManager].[GetPosition]
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Position].[PositionId],
		[Position].[OrganizationId],
		[CustomerId],
		[Position].[AddressId],  
		[StartDate], 
		[Position].[PositionStatusId],
		[PositionTitle], 
		[BillingRateFrequency],
		[BillingRateAmount],
		[DurationMonths],
		[Position].[EmploymentTypeId],
		[PositionCount],
		[RequiredSkills],
		[JobResponsibilities],
		[DesiredSkills],
		[Position].[PositionLevelId],
		[HiringManager],
		[TeamName],
		[Address].[Address1],
		[Address].[City],
		[State].[StateName],
		[Country].[CountryName],
		[Address].[PostalCode],
		[PositionCreatedUtc],
		[PositionModifiedUtc],
		[EmploymentType].[EmploymentTypeName],
		[PositionStatus].[PositionStatusName],
		[PositionLevel].[PositionLevelName]
	FROM [StaffingManager].[Position]
	LEFT JOIN [Lookup].[Address]		WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	LEFT JOIN [Lookup].[Country]		WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]			WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	LEFT JOIN [StaffingManager].[PositionTag]	WITH (NOLOCK) ON [PositionTag].[PositionId] = [Position].[PositionId]
	LEFT JOIN [StaffingManager].[EmploymentType] WITH (NOLOCK) ON [EmploymentType].[EmploymentTypeId] = [Position].[EmploymentTypeId]
	LEFT JOIN [StaffingManager].[PositionStatus] WITH (NOLOCK) ON [PositionStatus].[PositionStatusId] = [Position].[PositionStatusId]
	LEFT JOIN [StaffingManager].[PositionLevel] WITH (NOLOCK) ON [PositionLevel].[PositionLevelId] = [Position].[PositionLevelId]
	WHERE [Position].[PositionId] = @positionId

	-- Select all tags from the position
	SELECT
		[Tag].[TagId],
		[Tag].[TagName]
	FROM [StaffingManager].[Position]
		JOIN [StaffingManager].[PositionTag] ON [PositionTag].[PositionId] = [Position].[PositionId]
		JOIN [Lookup].[Tag] ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [Position].[PositionId] = @positionId
END