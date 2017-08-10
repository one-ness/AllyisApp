CREATE PROCEDURE [StaffingManager].[CreatePosition]
	@OrganizationId INT,
	@CustomerId INT,
    @AddressId INT,  
    @StartDate DATETIME2(0), 
    @PositionStatus INT,
	@PositionTitle NVARCHAR(140), 
    @BillingRateFrequency INT,
	@BillingRateAmount INT,
	@DurationMonths INT,
	@EmploymentType INT,
	@PositionCount INT,
	@RequiredSkills NVARCHAR (MAX),
	@JobResponsibilities NVARCHAR (MAX),
	@DesiredSkills NVARCHAR (MAX),
	@PositionLevel NVARCHAR (140),
	@HiringManager NVARCHAR (140),
	@TeamName NVARCHAR (140)
AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [StaffingManager].[Position] 
		([OrganizationId], 
		[CustomerId], 
		[AddressId],
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
		[TeamName])
	VALUES 	
		(@OrganizationId, 
		@CustomerId, 
		@AddressId,
		@StartDate, 
		@PositionStatus, 
		@PositionTitle,
		@BillingRateFrequency,
		@BillingRateAmount,
		@DurationMonths,
		@EmploymentType,
		@PositionCount,
		@RequiredSkills,
		@JobResponsibilities,
		@DesiredSkills,
		@PositionLevel,
		@HiringManager,
		@TeamName)

	SELECT SCOPE_IDENTITY();
END
