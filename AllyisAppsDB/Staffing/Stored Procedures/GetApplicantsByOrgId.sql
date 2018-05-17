CREATE PROCEDURE [Staffing].[GetApplicantsByOrgId]
	@orgId INT
AS
BEGIN
	SELECT * FROM [Staffing].[Applicant] WHERE [OrganizationId] = @orgId
END
