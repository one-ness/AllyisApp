CREATE PROCEDURE [StaffingManager].[GetApplicantById]
	@applicant INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[Applicant] WHERE [ApplicantId] = @applicant
END
