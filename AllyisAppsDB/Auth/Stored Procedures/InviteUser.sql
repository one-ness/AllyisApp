CREATE PROCEDURE [Auth].[InviteUser]
	@userId INT,
	@email NVARCHAR(384),
	@firstName NVARCHAR(40),
	@lastName NVARCHAR(40),
	@organizationId INT,
	@accessCode VARCHAR(50),
	@organizationRole INT,
	@retId INT OUTPUT,
	@employeeId NVARCHAR(16),
	@subscriptionId INT,
	@subRoleId INT
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
			WHERE [OrganizationId] = @organizationId AND [IsActive] = 1 AND [EmployeeId] = @employeeId
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
				[AccessCode], 
				[IsActive], 
				[OrganizationRoleId],
				[EmployeeId],
				[DateOfBirth])
			VALUES 
				(@email, 
				@firstName, 
				@lastName, 
				@organizationId, 
				@accessCode, 
				1, 
				@organizationRole, 
				@employeeId,
				'1755-01-01');

			-- Return invitation id
			SELECT SCOPE_IDENTITY()

			-- Return first and last names of inviting user
			SELECT [FirstName] FROM [Auth].[User] WITH (NOLOCK) WHERE [UserId] = @userId
			SELECT [LastName] FROM [Auth].[User] WITH (NOLOCK) WHERE [UserId] = @userId
		END
	END
END