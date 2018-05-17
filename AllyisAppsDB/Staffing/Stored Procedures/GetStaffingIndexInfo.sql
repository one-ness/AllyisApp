CREATE PROCEDURE [Staffing].[GetStaffingIndexInfo]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Position].[PositionId],
		[Position].[OrganizationId],
		[Position].[CustomerId],
		[Position].[AddressId],
		[PositionCreatedUtc],
		[PositionModifiedUtc],
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
	FROM [Staffing].[Position]
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].CountryCode = [Address].CountryCode
	LEFT JOIN [Lookup].[State]   WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Position].[OrganizationId] = @organizationId
	ORDER BY [Staffing].[Position].[StartDate] ASC

	-- Select all tags from the positions
	SELECT
		[Tag].[TagId],
		[Tag].[TagName],
		[Position].[PositionId]
	FROM [Staffing].[Position]
		JOIN [Staffing].[PositionTag] ON [PositionTag].[PositionId] = [Position].[PositionId]
		JOIN [Lookup].[Tag] ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [Position].[OrganizationId] = @organizationId
	
	-- Select all Employment Types from the org
		SELECT [EmploymentTypeId],
		[OrganizationId],
		[EmploymentTypeName]
	FROM [Staffing].[EmploymentType]
	WHERE [EmploymentType].[OrganizationId] = @organizationId

	-- Select all Position Levels from the org
	SELECT [PositionLevelId],
		[OrganizationId],
		[PositionLevelName]
	FROM [Staffing].[PositionLevel]
	WHERE [PositionLevel].[OrganizationId] = @organizationId

	-- Select all Position Status' from the org
		SELECT [PositionStatusId],
		[OrganizationId],
		[PositionStatusName]
	FROM [Staffing].[PositionStatus]
	WHERE [PositionStatus].[OrganizationId] = @organizationId
	
	-- Select all application Status' from the org
		SELECT [ApplicationStatusId],
		[OrganizationId],
		[ApplicationStatusName]
	FROM [Staffing].[ApplicationStatus]
	WHERE [ApplicationStatus].[OrganizationId] = @organizationId

	-- Select all Customers for each position
	SELECT 
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
		[Customer].[CustomerCode],
		[Customer].[IsActive]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK)
	WHERE [Customer].[OrganizationId] = @organizationId
END