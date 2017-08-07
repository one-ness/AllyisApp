CREATE PROCEDURE [Auth].[UpdateMember]
	@userId INT,
	@orgId INT,
	@employeeId NVARCHAR(100),
	@employeeRoleId INT,
	@isInvited BIT,
	@firstName NVARCHAR(100),
	@lastName NVARCHAR (100)
AS
BEGIN
	IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @orgId AND [EmployeeId] = @employeeId AND [UserId] != @userId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @orgId AND [IsActive] = 1 AND [EmployeeId] = @employeeId AND [InvitationId] != @userId
		)
	BEGIN
		IF @isInvited = 0
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[OrganizationUser]
			SET [OrganizationRoleId] = @employeeRoleId
			WHERE [UserId] = @userId AND [OrganizationId] = @orgId;
		END
		ELSE
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[Invitation]
			SET [OrganizationRoleId] = @employeeRoleId,
				[FirstName] = @firstName,
				[LastName] = @lastName
			WHERE [InvitationId] = @userId AND [OrganizationId] = @orgId;
		END
		SELECT 1;
	END
	ELSE
	BEGIN
		IF @isInvited = 0
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[OrganizationUser]
			SET [EmployeeId] = @employeeId,
				[OrganizationRoleId] = @employeeRoleId
			WHERE [UserId] = @userId AND [OrganizationId] = @orgId;
		END
		ELSE
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[Invitation]
			SET [EmployeeId] = @employeeId,
				[OrganizationRoleId] = @employeeRoleId,
				[FirstName] = @firstName,
				[LastName] = @lastName
			WHERE [InvitationId] = @userId AND [OrganizationId] = @orgId;
		END
		SELECT 2;
	END
END