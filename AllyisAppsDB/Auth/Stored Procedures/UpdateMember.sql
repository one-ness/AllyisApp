CREATE PROCEDURE [Auth].[UpdateMember]
	@UserId INT,
	@OrgId INT,
	@EmployeeId NVARCHAR(100),
	@EmployeeTypeId INT,
	@EmployeeRoleId INT
AS
	IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrgId AND [EmployeeId] = @EmployeeId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrgId AND [IsActive] = 1 AND [EmployeeId] = @EmployeeId
		)
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