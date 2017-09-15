CREATE PROCEDURE [StaffingManager].[GetApplicantByApplicationId]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @applicantId INT
	SELECT @applicantId = [ApplicantId] FROM [StaffingManager].[Application] WHERE [ApplicationId] = @applicationId
	
	SELECT * FROM [StaffingManager].[Applicant] WHERE [ApplicantId] = @applicantId
END
