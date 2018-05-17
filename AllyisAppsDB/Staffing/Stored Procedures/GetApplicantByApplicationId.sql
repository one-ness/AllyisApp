CREATE PROCEDURE [Staffing].[GetApplicantByApplicationId]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @applicantId INT
	SELECT @applicantId = [ApplicantId] FROM [Staffing].[Application] WHERE [ApplicationId] = @applicationId
	
	SELECT * FROM [Staffing].[Applicant] WHERE [ApplicantId] = @applicantId
END
