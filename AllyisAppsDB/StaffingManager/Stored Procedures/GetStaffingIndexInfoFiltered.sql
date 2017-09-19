CREATE PROCEDURE [StaffingManager].[GetStaffingIndexInfoFiltered]
	@organizationId INT,
	@status			[StaffingManager].[StatusesTable] READONLY,
	@type			[StaffingManager].[TypesTable] READONLY,
	@tags			[Lookup].[TagTable] READONLY
AS
BEGIN
	DECLARE @sSQL NVARCHAR(MAX), @Where NVARCHAR(MAX) = ''
	SET @sSQL =
		'SELECT DISTINCT [Position].[PositionId],
			[Position].[OrganizationId],
			[Position].[CustomerId],
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
			[PositionLevelId],
			[HiringManager],
			[TeamName],
			[Address].[Address1],
			[Address].[Address2],
			[Address].[City],
			[State].[StateName],
			[Country].[CountryName],
			[Address].[PostalCode],
			[Customer].[CustomerId],
			[Customer].[CustomerName],
			[Customer].[AddressId],
			[Customer].[ContactEmail],
			[Customer].[ContactPhoneNumber],
			[Customer].[FaxNumber],
			[Customer].[Website],
			[Customer].[EIN],
			[Customer].[CustomerCreatedUtc],
			[Customer].[OrganizationId],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive]
		FROM [StaffingManager].[Position]
		LEFT JOIN [StaffingManager].[PositionTag]	 WITH (NOLOCK) ON [PositionTag].[PositionId] = [Position].[PositionId]
			 JOIN [Lookup].[Tag]					 WITH (NOLOCK) ON [PositionTag].[TagId] = [Tag].[TagId]
		LEFT JOIN [Lookup].[Address]				 WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
		LEFT JOIN [Lookup].[Country]				 WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
		LEFT JOIN [Lookup].[State]					 WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
		LEFT JOIN [CRM].[Customer]					 WITH (NOLOCK) ON [Customer].[CustomerId] = [Position].[CustomerId]
		LEFT JOIN [StaffingManager].[EmploymentType] WITH (NOLOCK) ON [EmploymentType].[EmploymentTypeId] = [Position].[EmploymentTypeId]
		LEFT JOIN [StaffingManager].[PositionStatus] WITH (NOLOCK) ON [PositionStatus].[PositionStatusId] = [Position].[PositionStatusId]
		WHERE [Position].[OrganizationId] = @_organizationId '
	IF (SELECT count(*) from @status) > 0
		SET @Where = CONCAT(@Where, 'AND [PositionStatus].[PositionStatusName] IN ( SELECT [StatusName] FROM @_Status) ')
	IF(SELECT count(*) from @type) > 0
		SET @Where = CONCAT(@Where, 'AND [EmploymentType].[EmploymentTypeName] IN (SELECT [TypeName] FROM @_Type) ')
	IF (SELECT count(*) from @tags) > 0
		SET @Where = CONCAT(@Where, 'AND [Tag].[TagName] IN (SELECT [TagName] FROM @_Tags) ')
		SET @sSQL = CONCAT(@sSQL, @where)
	EXEC sp_executesql @sSQL,
		N'@_organizationId INT, @_Status [StaffingManager].[StatusesTable] READONLY, @_Type [StaffingManager].[TypesTable] READONLY, @_Tags [Lookup].[TagTable] READONLY',
	@_organizationId = @organizationId, @_Status = @status, @_Type = @type, @_Tags = @tags

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
	
	-- Select all Positions Customers from the org
	SELECT [CustomerId],
		   [CustomerName],
		   [AddressId],
		   [ContactEmail],
		   [ContactPhoneNumber],
		   [FaxNumber],
		   [Website],
		   [EIN],
		   [CustomerCreatedUtc],
		   [OrganizationId],
		   [CustomerOrgId],
		   [IsActive]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK)
	WHERE [Customer].[OrganizationId] = @organizationId
END