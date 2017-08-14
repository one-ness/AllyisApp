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
	@teamName NVARCHAR (140),
	@address NVARCHAR (64),
	@city NVARCHAR(32),
	@state NVARCHAR(32),
	@country NVARCHAR(32),
	@postalCode NVARCHAR(16)
AS
BEGIN

	INSERT INTO [Lookup].[Address]
		([Address1],
		[City],
		[StateId],
		[CountryId],
		[PostalCode])
	VALUES
		(@address,
		@city,
		(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
		(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
		@postalCode);

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
		IDENT_CURRENT('[Lookup].[Address]') AS [AddressId],
		IDENT_CURRENT('[StaffingManager].[Position]') AS [PositionId];
END
