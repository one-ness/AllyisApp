CREATE PROCEDURE [Auth].[InviteUser]
	@UserId INT,
	@Email NVARCHAR(384),
	@FirstName NVARCHAR(40),
	@LastName NVARCHAR(40),
	@OrganizationId INT,
	@AccessCode VARCHAR(50),
	@OrgRole INT,
	@ProjectId INT,
	@retId INT OUTPUT,
	@EmployeeId NVARCHAR(16),
	@SubscriptionId INT,
	@SubRoleId INT
AS

BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
		INNER JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
		WHERE [Email] = @Email AND [OrganizationId] = @OrganizationId
	)
	BEGIN
		SELECT -1 --Indicates the user is already in the organization
	END
	ELSE
	BEGIN
		-- Check for existing employee id
		IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrganizationId AND [EmployeeId] = @EmployeeId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1 AND [EmployeeId] = @EmployeeId
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
				[OrgRole], 
				[ProjectId],
				[EmployeeId],
				[DateOfBirth])
			VALUES 
				(@Email, 
				@FirstName, 
				@LastName, 
				@OrganizationId, 
				@AccessCode, 
				1, 
				@OrgRole, 
				@ProjectId,
				@EmployeeId,
				'1755-01-01');

			SET @retId = SCOPE_IDENTITY();

			-- Add invitation sub roles if included
			IF @SubscriptionId IS NOT NULL
			BEGIN
				-- Check subscription user count
				IF (SELECT [Subscription].[NumberOfUsers]
					FROM [Billing].[Subscription] WITH (NOLOCK)
					WHERE [Subscription].[SubscriptionId] = @SubscriptionId
				) > (SELECT COUNT(*)
					FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
					WHERE [SubscriptionUser].[SubscriptionId] = @SubscriptionId
				)
				BEGIN
					INSERT INTO [Auth].[InvitationSubRole]
						([InvitationId], [SubscriptionId], [ProductRoleId])
					VALUES
						(@retId, @SubscriptionId, @SubRoleId);
				END
			END

			-- Return invitation id
			SELECT @retId;

			-- Return first and last names of inviting user
			SELECT [FirstName] FROM [Auth].[User] WITH (NOLOCK) WHERE [UserId] = @UserId
			SELECT [LastName] FROM [Auth].[User] WITH (NOLOCK) WHERE [UserId] = @UserId
		END
	END
END