CREATE PROCEDURE [Auth].[CreateUserInvitation]
	@Email NVARCHAR(384),
	@FirstName NVARCHAR(40),
	@LastName NVARCHAR(40),
	@DateOfBirth NVARCHAR(40),
	@OrganizationId INT,
	@AccessCode VARCHAR(50),
	@OrgRole INT,
	@ProjectId INT,
	@retId INT OUTPUT,
	@EmployeeId NVARCHAR(16),
	@EmployeeTypeId INT
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
		[OrgRoleId], 
		[EmployeeId],
		[EmployeeTypeId])
	VALUES 
		(@Email, 
		@FirstName, 
		@LastName, 
		@DateOfBirth, 
		@OrganizationId, 
		@AccessCode, 
		1, 
		@OrgRole, 
		@ProjectId,
		@EmployeeId,
		@EmployeeTypeId);

	SET @retId = SCOPE_IDENTITY();

	SELECT SCOPE_IDENTITY();
END