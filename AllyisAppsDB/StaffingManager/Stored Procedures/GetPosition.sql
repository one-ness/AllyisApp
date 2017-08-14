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
		[PositionStatusId],
		[PositionTitle], 
		[BillingRateFrequency],
		[BillingRateAmount],
		[DurationMonths],
		[EmploymentType],
		[PositionCount],
		[RequiredSkills],
		[JobResponsibilities],
		[DesiredSkills],
		[PositionLevelId],
		[HiringManager],
		[TeamName]
	FROM [StaffingManager].[Position]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Position].[AddressId]
	WHERE [Position].[PositionId] = @positionId
	ORDER BY [StaffingManager].[Position].[PositionCreatedUtc] DESC
END