CREATE PROCEDURE [StaffingManager].[GetPositionByPositionId]
	@PositionId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Position].[PositionId],
		[Position].[OrganizationId],
		[Position].[AddressId],
		[Position].[StartDate],
		[Position].[PositionStatus],
		[Position].[PositionTitle],
		[Position].[BillingRateFrequency],
		[Position].[BillingRateAmount],
		[Position].[DurationMonths],
		[Position].[EmploymentType],
		[Position].[PositionCount],
		[Position].[RequiredSkills],
		[Position].[JobResponsibilities],
		[Position].[DesiredSkills],
		[Position].[PositionLevel],
		[Position].[HiringManager],
		[Position].[TeamName]
	FROM [StaffingManager].[Position]
	--TODO also get Tags from PositionTags 
END