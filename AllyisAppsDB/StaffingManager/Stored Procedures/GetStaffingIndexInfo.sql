CREATE PROCEDURE [StaffingManager].[GetStaffingIndexInfo]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [PositionId],
		[OrganizationId],
		[CustomerId],
		[AddressId],
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
		[TeamName]
	FROM [StaffingManager].[Position]
	WHERE [Position].[OrganizationId] = @organizationId
	ORDER BY [StaffingManager].[Position].[PositionCreatedUtc] DESC

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

	
	-- Select all Position Status' from the org
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