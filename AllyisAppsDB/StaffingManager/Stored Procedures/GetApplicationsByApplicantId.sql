CREATE PROCEDURE [StaffingManager].[GetApplicationsByApplicantId]
	@applicantId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[Application] WHERE [ApplicantId] = @applicantId
END
