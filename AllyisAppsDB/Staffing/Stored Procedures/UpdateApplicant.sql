CREATE PROCEDURE [Staffing].[UpdateApplicant]
	@applicantId INT,
	@addressId INT,
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
	BEGIN TRANSACTION
		SET NOCOUNT ON
		UPDATE [Lookup].[Address] SET 
			[Address1] = @address,
			[City] = @city,
			[StateId] = @state,
			[CountryCode] = @country,
			[PostalCode] = @postalCode
		WHERE [AddressId] = @addressId

		SET NOCOUNT OFF
		UPDATE [Staffing].[Applicant] SET
			[FirstName] = @firstName,
			[LastName] = @lastName,
			[Email] = @email,
			[PhoneNumber] = @phoneNumber,
			[Notes] = @notes
		WHERE [ApplicantId] = @applicantId
	COMMIT TRANSACTION
END
