CREATE PROCEDURE [Expense].[GetUsersByOrganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [UserId]
      ,[OrganizationId]
      ,[EmployeeId]
      ,[OrganizationRoleId]
      ,[OrganizationUserCreatedUtc]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE OrganizationId = @organizationId
END