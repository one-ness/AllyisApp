CREATE PROCEDURE [Staffing].[SetupPosition]
	@organizationId INT,
	@customerId INT,
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
	@stateId SMALLINT,
	@postalCode NVARCHAR(16),
	@countryCode VARCHAR(8),
	@tags [Lookup].[TagTable] READONLY
AS
BEGIN TRANSACTION

	EXEC [Lookup].[CreateAddress]
		@address1,
		@address2,
		@city,
		@stateId,
		@postalCode,
		@countryCode
		
		DECLARE @addressId INT
		SET @addressId = IDENT_CURRENT('[Lookup].[Address]')

	EXEC [Staffing].[CreatePosition]
		@organizationId,
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
		@teamName
	
		DECLARE @positionId INT
		SET @positionId = IDENT_CURRENT('[Staffing].[Position]')

	EXEC [Staffing].[CreatePositionTags]
		@tags,
		@positionId

	SELECT @positionId
COMMIT TRANSACTION
