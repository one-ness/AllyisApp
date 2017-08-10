CREATE PROCEDURE [StaffingManager].[UpdatePosition]
	@PositionId INT,
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
		UPDATE [StaffingManager].[Position] 
		SET [OrganizationId] = @OrganizationId, 
			[CustomerId] = @CustomerId, 
			[AddressId] = @AddressId,
			[StartDate] = @StartDate, 
			[PositionStatus] = @PositionStatus, 
			[PositionTitle] = @PositionTitle,
			[BillingRateFrequency] = @BillingRateFrequency,
			[BillingRateAmount] = @BillingRateAmount,
			[DurationMonths] = @DurationMonths,
			[EmploymentType] = @EmploymentType,
			[PositionCount] = @PositionCount,
			[RequiredSkills] = @RequiredSkills,
			[JobResponsibilities] = @JobResponsibilities,
			[DesiredSkills] = @DesiredSkills,
			[PositionLevel] = @PositionLevel,
			[HiringManager] = @HiringManager,
			[TeamName] = @TeamName,
			[PositionModifiedUtc] = SYSUTCDATETIME()
	  WHERE [PositionId] = @PositionId

END
