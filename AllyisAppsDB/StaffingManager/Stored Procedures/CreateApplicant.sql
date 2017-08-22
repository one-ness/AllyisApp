CREATE PROCEDURE [StaffingManager].[CreateApplicant]
	@email NVARCHAR (100),
	@firstName NVARCHAR (32),
	@lastName NVARCHAR (32),
	@phoneNumber NVARCHAR (16),
	@notes NVARCHAR (MAX),
	@address1 nvarchar(64),
	@address2 nvarchar(64),
	@city nvarchar(32),
	@stateId smallint,
	@postalCode nvarchar(16),
	@countryCode varchar(8)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		EXEC [Lookup].[CreateAddress]
			@address1,
			@address2,
			@city,
			@stateId,
			@postalCode,
			@countryCode

		INSERT INTO [StaffingManager].[Applicant]
			([AddressId],
			[FirstName],
			[LastName],
			[Email],
			[PhoneNumber],
			[Notes])
		VALUES
			(IDENT_CURRENT('[Lookup].[Address]'),
			@firstName,
			@lastName,
			@email,
			@phoneNumber,
			@notes);

		SELECT IDENT_CURRENT('[StaffingManager].[Applicant]') AS [ApplicantId];
	COMMIT TRANSACTION
END
