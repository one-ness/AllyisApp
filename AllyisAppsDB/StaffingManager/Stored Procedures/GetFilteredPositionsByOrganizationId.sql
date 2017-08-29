CREATE PROCEDURE [StaffingManager].[GetFilteredPositionsByOrganizationId] 
   (@organizationId INT,
	@Status int = NULL,
	@Type int = NULL,
	@Tags char(1) = NULL)
AS
	DECLARE @sSQL NVARCHAR(2000), @Where NVARCHAR(1000) = ''
	SET @sSQL = 
		'SELECT [PositionId],
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
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryCode]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
		WHERE [Position].[OrganizationId] = @organizationId'
	IF @Status is not null
		SET @Where = @Where + 'AND PositionStatusId = @_Status '
	IF @Type is not null
		SET @Where = @Where + 'AND EmploymentTypeId = @_Type '
	IF @Tags IS NOT NULL
		SET @Where = @Where + 'AND Tags = @_Tags '
	EXEC sp_executesql @sSQL,
	N'@_Status int, @_Type int, @_Tags char(1)',
	@_Status = @Status, @_Type = @Type, @_Tags = @Tags
GO 