CREATE PROCEDURE [Auth].[UpdateMember]
	@UserId INT,
	@OrgId INT,
	@EmployeeId NVARCHAR(100),
	@EmployeeRoleId INT,
	@IsInvited BIT,
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR (100)
AS
BEGIN
	IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrgId AND [EmployeeId] = @EmployeeId AND [UserId] != @UserId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrgId AND [IsActive] = 1 AND [EmployeeId] = @EmployeeId AND [InvitationId] != @UserId
		)
	BEGIN
		IF @IsInvited = 0
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[OrganizationUser]
			SET [OrganizationRoleId] = @EmployeeRoleId
			WHERE [UserId] = @UserId AND [OrganizationId] = @OrgId;
		END
		ELSE
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[Invitation]
			SET [OrganizationRoleId] = @EmployeeRoleId,
				[FirstName] = @FirstName,
				[LastName] = @LastName
			WHERE [InvitationId] = @UserId AND [OrganizationId] = @OrgId;
		END
		SELECT 1;
	END
	ELSE
	BEGIN
		IF @IsInvited = 0
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[OrganizationUser]
			SET [EmployeeId] = @EmployeeId,
				[OrganizationRoleId] = @EmployeeRoleId
			WHERE [UserId] = @UserId AND [OrganizationId] = @OrgId;
		END
		ELSE
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[Invitation]
			SET [EmployeeId] = @EmployeeId,
				[OrganizationRoleId] = @EmployeeRoleId,
				[FirstName] = @FirstName,
				[LastName] = @LastName
			WHERE [InvitationId] = @UserId AND [OrganizationId] = @OrgId;
		END
		SELECT 2;
	END
END