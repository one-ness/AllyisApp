CREATE PROCEDURE [StaffingManager].[SetupPosition]
	@organizationId INT,
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
	@customerName NVARCHAR(32),
    @address NVARCHAR(100),
	@contactEmail NVARCHAR(384), 
    @contactPhoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@website NVARCHAR(50),
	@eIN NVARCHAR(50),
	@customerOrgId NVARCHAR(16),
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

	Exec [Crm].[CreateCustomer]
		@customerName,
		@addressId,
		@contactEmail, 
		@contactPhoneNumber,
		@faxNumber,
		@website,
		@eIN,
		@organizationId,
		@customerOrgId
		
		DECLARE @customerId INT
		SET @customerId = IDENT_CURRENT('[Crm].[Customer]')

	EXEC [StaffingManager].[CreatePosition]
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
		SET @positionId = IDENT_CURRENT('[StaffingManager].[Position]')

	EXEC [StaffingManager].[CreatePositionTags]
		@tags,
		@positionId

	SELECT @positionId
COMMIT TRANSACTION
