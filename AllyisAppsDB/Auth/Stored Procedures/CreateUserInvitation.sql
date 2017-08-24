CREATE PROCEDURE [Auth].[CreateUserInvitation]
	@email NVARCHAR(384),
	@firstName NVARCHAR(32),
	@lastName NVARCHAR(32),
	@dateOfBirth date,
	@organizationId INT,
	@accessCode VARCHAR(50),
	@organizationRole INT,
	@employeeId NVARCHAR(16)
AS

BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Auth].[Invitation] 
		([Email], 
		[FirstName], 
		[LastName], 
		[DateOfBirth], 
		[OrganizationId], 
		[AccessCode], 
		[IsActive], 
		[OrganizationRoleId], 
		[EmployeeId])
	VALUES 
		(@email, 
		@firstName, 
		@lastName, 
		@dateOfBirth, 
		@organizationId, 
		@accessCode, 
		1, 
		@organizationRole, 
		@employeeId);
	SELECT SCOPE_IDENTITY();
END