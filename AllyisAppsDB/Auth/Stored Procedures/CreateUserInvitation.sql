CREATE PROCEDURE [Auth].[CreateUserInvitation]
	@email NVARCHAR(384),
	@firstName NVARCHAR(40),
	@lastName NVARCHAR(40),
	@dateOfBirth NVARCHAR(40),
	@organizationId INT,
	@accessCode VARCHAR(50),
	@organizationRole INT,
	@retId INT OUTPUT,
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

	SET @retId = SCOPE_IDENTITY();

	SELECT SCOPE_IDENTITY();
END