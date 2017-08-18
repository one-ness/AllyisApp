CREATE PROCEDURE [StaffingManager].[SetupPosition]
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
	@postalCode NVARCHAR(16),
	@tags [Lookup].[TagTable] READONLY
AS
BEGIN TRANSACTION
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
		@teamName,
		@address,
		@city,
		@state,
		@country,
		@postalCode
	
	EXEC [StaffingManager].[CreatePositionTags]


COMMIT TRANSACTION
