CREATE PROCEDURE [Auth].[UpdateMember]
	@UserId INT,
	@OrgId INT,
	@EmployeeId NVARCHAR(100),
	@EmployeeTypeId INT,
	@EmployeeRoleId INT,
	@IsInvited BIT
AS
	IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrgId AND [EmployeeId] = @EmployeeId AND [UserId] != @UserId
		) /*OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrgId AND [IsActive] = 1 AND [EmployeeId] = @EmployeeId AND [UserId] = @UserId
		)*/
		BEGIN
		SELECT 0;
		END
	ELSE
		BEGIN
		SET NOCOUNT ON;
		UPDATE [Auth].[OrganizationUser]
		SET [EmployeeId] = @EmployeeId,
			[EmployeeTypeId] = @EmployeeTypeId,
			[OrgRoleId] = @EmployeeRoleId
		WHERE [UserId] = @UserId AND [OrganizationId] = @OrgId;
		SELECT 1;
		END