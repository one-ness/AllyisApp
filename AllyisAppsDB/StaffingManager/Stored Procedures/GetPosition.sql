CREATE PROCEDURE [StaffingManager].[GetPosition]
	@positionId INT
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
		[Address].[City],
		[State].[StateName],
		[Country].[CountryName],
		[Address].[PostalCode]
	FROM [StaffingManager].[Position]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Position].[PositionId] = @positionId
	ORDER BY [StaffingManager].[Position].[PositionCreatedUtc] DESC
END