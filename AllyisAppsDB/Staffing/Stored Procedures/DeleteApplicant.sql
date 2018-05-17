CREATE PROCEDURE [Staffing].[DeleteApplicant]
	@applicantId INT
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM [Lookup].[Address]
	WHERE [AddressId] IN
		(SELECT [AddressId]
		FROM [Staffing].[Applicant]
		WHERE [ApplicantId] = @applicantId)

	DELETE FROM [Staffing].[Applicant] WHERE [ApplicantId] = @applicantId
END
