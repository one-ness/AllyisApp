CREATE PROCEDURE [Staffing].[GetApplicationsByApplicantId]
	@applicantId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Staffing].[Application] WHERE [ApplicantId] = @applicantId
END
