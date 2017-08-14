CREATE PROCEDURE [StaffingManager].[GetPositionsByorganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionId],
		[OrganizationId],
		[CustomerId],
		[Position].[AddressId] AS 'Address',  
		[StartDate], 
		[PositionStatus],
		[PositionTitle], 
		[BillingRateFrequency],
		[BillingRateAmount],
		[DurationMonths],
		[EmploymentType],
		[PositionCount],
		[RequiredSkills],
		[JobResponsibilities],
		[DesiredSkills],
		[PositionLevel],
		[HiringManager],
		[TeamName],
		[Address].[City],
		[Address].[postalCode]
	FROM [StaffingManager].[Position]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Position].[OrganizationId] = @organizationId
	ORDER BY [StaffingManager].[Position].[PositionCreatedUtc] DESC
END