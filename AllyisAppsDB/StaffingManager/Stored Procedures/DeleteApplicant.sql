CREATE PROCEDURE [StaffingManager].[DeleteApplicant]
	@applicantId INT
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM [Lookup].[Address]
	WHERE [AddressId] IN
		(SELECT [AddressId]
		FROM [StaffingManager].[Applicant]
		WHERE [ApplicantId] = @applicantId)

	DELETE FROM [StaffingManager].[Applicant] WHERE [ApplicantId] = @applicantId
END
