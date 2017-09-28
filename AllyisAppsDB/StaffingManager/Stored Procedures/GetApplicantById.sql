CREATE PROCEDURE [StaffingManager].[GetApplicantById]
	@applicantId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[Applicant] WHERE [ApplicantId] = @applicantId
END
