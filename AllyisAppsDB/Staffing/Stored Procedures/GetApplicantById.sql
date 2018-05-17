CREATE PROCEDURE [Staffing].[GetApplicantById]
	@applicantId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Staffing].[Applicant] WHERE [ApplicantId] = @applicantId
END
