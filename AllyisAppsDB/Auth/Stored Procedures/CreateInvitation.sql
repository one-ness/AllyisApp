CREATE PROCEDURE [Auth].[CreateInvitation]
	@email NVARCHAR(384),
	@firstName NVARCHAR(40),
	@lastName NVARCHAR(40),
	@organizationId INT,
	@organizationRole INT,
	@employeeId NVARCHAR(16),
	@prodJson NVARCHAR(384)
AS

BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
		INNER JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
		WHERE [Email] = @email AND [OrganizationId] = @organizationId
	)
	BEGIN
		SELECT -1 --Indicates the user is already in the organization
	END
	ELSE
	BEGIN
		-- Check for existing employee id
		IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @organizationId AND [EmployeeId] = @employeeId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @organizationId AND [EmployeeId] = @employeeId AND [InvitationStatus] = 1
		)
		BEGIN
			SELECT -2 -- Indicates employee id already taken
		END
		ELSE
		BEGIN
			INSERT INTO [Auth].[Invitation] 
				([Email], 
				[FirstName], 
				[LastName], 
				[OrganizationId],
				[OrganizationRoleId],
				[EmployeeId],
				[ProductRolesJson]
				)
			VALUES 
				(@email, 
				@firstName, 
				@lastName, 
				@organizationId,
				@organizationRole,
				@employeeId,
				@prodJson
				);

			-- Return invitation id
			SELECT SCOPE_IDENTITY()
		END
	END
END