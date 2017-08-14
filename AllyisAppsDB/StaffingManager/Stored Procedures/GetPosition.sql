CREATE PROCEDURE [StaffingManager].[GetPosition]
	@positionId INT
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
		[TeamName]
	FROM [StaffingManager].[Position]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	WHERE [Position].[PositionId] = @PositionId
	ORDER BY [StaffingManager].[Position].[PositionCreatedUtc] DESC
END