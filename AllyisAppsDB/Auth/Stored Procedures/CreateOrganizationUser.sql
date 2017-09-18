CREATE PROCEDURE [Auth].[CreateOrganizationUser]
	@userId INT,
	@organizationId INT,
	@roleId INT,
	@employeeId NVARCHAR(128) = NULL
AS
BEGIN
	INSERT INTO [Auth].[OrganizationUser]
		([UserId],
		[OrganizationId],
		[OrganizationRoleId],
		[EmployeeId])
	VALUES
		(@userId,
		@organizationId,
		@roleId,
		@employeeId);
END
