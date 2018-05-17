CREATE PROCEDURE [Staffing].[UpdatePosition]
	@positionId INT,
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
	@teamName NVARCHAR (140),
	@address1 NVARCHAR (64),
	@address2 NVARCHAR (64),
	@city NVARCHAR(32),
	@stateId NVARCHAR(32),
	@countryCode NVARCHAR(32),
	@postalCode NVARCHAR(16),
	@tags [Lookup].[TagTable] READONLY

AS
BEGIN
	UPDATE [Staffing].[Position] 
	SET [OrganizationId] = @organizationId, 
		[CustomerId] = @customerId, 
		[AddressId] = @addressId,
		[StartDate] = @startDate, 
		[PositionStatusId] = @positionStatus, 
		[PositionTitle] = @positionTitle,
		[BillingRateFrequency] = @billingRateFrequency,
		[BillingRateAmount] = @billingRateAmount,
		[DurationMonths] = @durationMonths,
		[EmploymentTypeId] = @employmentType,
		[PositionCount] = @positionCount,
		[RequiredSkills] = @requiredSkills,
		[JobResponsibilities] = @jobResponsibilities,
		[DesiredSkills] = @desiredSkills,
		[PositionLevelId] = @positionLevel,
		[HiringManager] = @hiringManager,
		[TeamName] = @teamName,
		[PositionModifiedUtc] = SYSUTCDATETIME()
	WHERE [PositionId] = @positionId

	SET NOCOUNT ON
	EXEC [Lookup].[UpdateAddress]
		@addressId,
		@address1,
		@address2,
		@city,
		@stateId,
		@postalCode,
		@countryCode

END
