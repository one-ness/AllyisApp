CREATE PROCEDURE [Staffing].[GetPositionsByorganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionId],
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
	FROM [Staffing].[Position]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].CountryCode = [Address].CountryCode
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Position].[OrganizationId] = @organizationId
	ORDER BY [Staffing].[Position].[PositionCreatedUtc] DESC

	-- Select all tags from the position
	SELECT
		[Tag].[TagId],
		[Tag].[TagName],
		[Position].[PositionId]
	FROM [Staffing].[Position]
		JOIN [Staffing].[PositionTag] ON [PositionTag].[PositionId] = [Position].[PositionId]
		JOIN [Lookup].[Tag] ON [PositionTag].[TagId] = [Tag].[TagId]
	WHERE [Position].[OrganizationId] = @organizationId
END