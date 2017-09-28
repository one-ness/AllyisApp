CREATE PROCEDURE [StaffingManager].[GetApplicantsByOrgId]
	@orgId INT
AS
BEGIN
	SELECT * FROM [StaffingManager].[Applicant] WHERE [OrganizationId] = @orgId
END
