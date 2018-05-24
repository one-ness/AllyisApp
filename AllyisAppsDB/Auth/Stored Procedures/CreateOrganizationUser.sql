CREATE PROCEDURE [Auth].[CreateOrganizationUser]
	@organizationId INT,
	@userId INT,
	@roleId INT,
	@employeeId NVARCHAR(128),
	@employeeTypeId INT,
	@approvalLimit decimal = 0
AS
BEGIN

	INSERT INTO [Auth].[OrganizationUser]
		([UserId],
		[OrganizationId],
		[OrganizationRoleId],
		[EmployeeTypeId],
		[EmployeeId],
		[MaxAmount]
		)
	VALUES
		(@userId,
		@organizationId,
		@roleId,
		@employeeTypeId,
		@employeeId,
		@approvalLimit);
END
