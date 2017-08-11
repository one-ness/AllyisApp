CREATE PROCEDURE [StaffingManager].[UpdatePosition]
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
	@address NVARCHAR (64),
	@city NVARCHAR(32),
	@state NVARCHAR(32),
	@country NVARCHAR(32),
	@postalCode NVARCHAR(16)

AS
BEGIN
	UPDATE [StaffingManager].[Position] 
	SET [OrganizationId] = @organizationId, 
		[CustomerId] = @customerId, 
		[AddressId] = @addressId,
		[StartDate] = @startDate, 
		[PositionStatus] = @positionStatus, 
		[PositionTitle] = @positionTitle,
		[BillingRateFrequency] = @billingRateFrequency,
		[BillingRateAmount] = @billingRateAmount,
		[DurationMonths] = @durationMonths,
		[EmploymentType] = @employmentType,
		[PositionCount] = @positionCount,
		[RequiredSkills] = @requiredSkills,
		[JobResponsibilities] = @jobResponsibilities,
		[DesiredSkills] = @desiredSkills,
		[PositionLevel] = @positionLevel,
		[HiringManager] = @hiringManager,
		[TeamName] = @teamName,
		[PositionModifiedUtc] = SYSUTCDATETIME()
	WHERE [PositionId] = @positionId

	  
	SET NOCOUNT ON
	UPDATE [Lookup].[Address] SET 
		[Address1] = @address,
		[City] = @city,
		[StateId] = @state,
		[CountryId] = @country,
		[PostalCode] = @postalCode
	WHERE [AddressId] = @addressId

END
