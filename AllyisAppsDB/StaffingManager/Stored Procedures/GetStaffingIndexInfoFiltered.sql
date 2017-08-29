CREATE PROCEDURE [StaffingManager].[GetStaffingIndexInfoFiltered]
	@organizationId INT,
	@Status INT,
	@Type INT,
	@tags [Lookup].[TagTable] READONLY
AS
BEGIN
	DECLARE @sSQL NVARCHAR(2000), @Where NVARCHAR(1000) = ''
	SET @sSQL = 
		'SELECT DISTINCT [PositionId],
			[OrganizationId],
			[CustomerId],
			[Position].[AddressId],
			[StartDate], 
			[PositionStatusId],
			[PositionTitle], 
			[BillingRateFrequency],
			[BillingRateAmount],
			[DurationMonths],
			[EmploymentTypeId],
			[PositionCount],
			[RequiredSkills],
			[JobResponsibilities],
			[DesiredSkills],
			[PositionLevelId],
			[HiringManager],
			[TeamName],
			[Address].[Address1],
			[Address].[Address2],
			[Address].[City],
			[State].[StateName],
			[Country].[CountryName],
			[Address].[PostalCode]
		FROM [StaffingManager].[Position]
		LEFT JOIN [StaffingManager].[PositionTag]	WITH (NOLOCK) ON [PositionTag].[PositionId] = [Position].[PositionId]
			 JOIN [Lookup].[Tag]					WITH (NOLOCK) ON [PositionTag].[TagId] = [Tag].[TagId]
		LEFT JOIN [Lookup].[Address]				WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
		LEFT JOIN [Lookup].[Country]				WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryCode]
		LEFT JOIN [Lookup].[State]					WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
		WHERE [Position].[OrganizationId] = @organizationId '
	IF @Status is not null
		SET @Where = @Where + 'AND [PositionStatusId] = @_Status '
	IF @Type is not null
		SET @Where = @Where + 'AND [EmploymentTypeId] = @_Type '
	IF @Tags IS NOT NULL
		SET @Where = @Where + 'AND [Tag].[TagId] IN (SELECT [TagId] FROM @_Tags) '

	EXEC sp_executesql @sSQL,
	N'@_Status int, @_Type int, @_Tags [Lookup].[TagTable]',
	@_Status = @Status, @_Type = @Type, @_Tags = @Tags

	-- Select all tags from the positions
	SELECT
		[Tag].[TagId],
		[Tag].[TagName],
		[Position].[PositionId]
	FROM [StaffingManager].[Position]
		JOIN [StaffingManager].[PositionTag] ON [PositionTag].[PositionId] = [Position].[PositionId]
		JOIN [Lookup].[Tag] ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [Position].[OrganizationId] = @organizationId
	
	-- Select all Employment Types from the org
		SELECT [EmploymentTypeId],
		[OrganizationId],
		[EmploymentTypeName]
	FROM [StaffingManager].[EmploymentType]
	WHERE [EmploymentType].[OrganizationId] = @organizationId

	-- Select all Position Levels from the org
	SELECT [PositionLevelId],
		[OrganizationId],
		[PositionLevelName]
	FROM [StaffingManager].[PositionLevel]
	WHERE [PositionLevel].[OrganizationId] = @organizationId

	-- Select all Position Status' from the org
		SELECT [PositionStatusId],
		[OrganizationId],
		[PositionStatusName]
	FROM [StaffingManager].[PositionStatus]
	WHERE [PositionStatus].[OrganizationId] = @organizationId
END