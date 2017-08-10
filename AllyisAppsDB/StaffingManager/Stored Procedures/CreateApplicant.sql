CREATE PROCEDURE [StaffingManager].[CreateApplicant]
	@email NVARCHAR (100),
	@firstName NVARCHAR (32),
	@lastName NVARCHAR (32),
	@address NVARCHAR (64),
	@city NVARCHAR(32),
	@state NVARCHAR(32),
	@country NVARCHAR(32),
	@postalCode NVARCHAR(16),
	@phoneNumber NVARCHAR (16),
	@notes NVARCHAR (MAX)
AS
BEGIN
	SET NOCOUNT ON;

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

	INSERT INTO [StaffingManager].[Applicant]
		([AddressId],
		[FirstName],
		[LastName],
		[Email],
		[PhoneNumber],
		[Notes])
	VALUES
		(SCOPE_IDENTITY(),
		@firstName,
		@lastName,
		@email,
		@phoneNumber,
		@notes);

	SELECT
		IDENT_CURRENT('[Lookup].[Address]') AS [AddressId],
		IDENT_CURRENT('[StaffingManager].[Applicant]') AS [ApplicantId];
END
