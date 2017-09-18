CREATE PROCEDURE [StaffingManager].[CreatePosition]
	@organizationId INT,
	@customerId INT,
	@addressId INT,  
	@startDate DATETIME2(0), 
	@positionStatus INT,
	@positionTitle NVARCHAR(140), 
	@billingRateFrequency INT,
	@billingRateAmount INT,
	@durationMonths INT,
	@employmentType INT,
	@positionCount INT,
	@requiredSkills NVARCHAR (MAX),
	@jobResponsibilities NVARCHAR (MAX),
	@desiredSkills NVARCHAR (MAX),
	@positionLevel NVARCHAR (140),
	@hiringManager NVARCHAR (140),
	@teamName NVARCHAR (140)
AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [StaffingManager].[Position] 
		([OrganizationId], 
		[CustomerId], 
		[AddressId],
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
		[TeamName])
	VALUES 	
		(@organizationId, 
		@customerId, 
		@addressId,
		@startDate, 
		@positionStatus, 
		@positionTitle,
		@billingRateFrequency,
		@billingRateAmount,
		@durationMonths,
		@employmentType,
		@positionCount,
		@requiredSkills,
		@jobResponsibilities,
		@desiredSkills,
		@positionLevel,
		@hiringManager,
		@teamName)
		
	SELECT
		IDENT_CURRENT('[StaffingManager].[Position]') AS [PositionId];
END
