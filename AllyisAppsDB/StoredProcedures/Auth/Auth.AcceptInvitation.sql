CREATE PROCEDURE [Auth].[AcceptInvitation]
	@InvitationId INT,
	@CallingUserId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Retrieve the invitation information
	DECLARE @OrganizationId INT;
	DECLARE @OrgRole INT;
	DECLARE @Email NVARCHAR(384);
	DECLARE @ProjectId INT;
	DECLARE @EmployeeId NVARCHAR(16);
	DECLARE @EmployeeTypeId INT;

	SELECT
		@OrganizationId = [OrganizationId],
		@OrgRole = [OrgRole],
		@Email = [Email],
		@ProjectId = [ProjectId],
		@EmployeeId = [EmployeeId],
		@EmployeeTypeId = [EmployeeType]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [Invitation].[InvitationId] = @InvitationId AND [Invitation].[IsActive] = 1

	IF @OrganizationId IS NOT NULL
	BEGIN -- Invitation found

		-- Retrive invited user
		DECLARE @UserId INT;
		SET @UserId = (
			SELECT [UserId]
			FROM [Auth].[User] WITH (NOLOCK)
			WHERE [User].[Email] = @Email
		)

		IF @UserId IS NOT NULL AND @UserId = @CallingUserId
		BEGIN -- Invited user found and matches calling user id
			BEGIN TRANSACTION

			-- Add user to organization
			IF EXISTS (
				SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
				WHERE [OrganizationUser].[UserId] = @UserId AND [OrganizationUser].[OrganizationId] = @OrganizationId
			)
			BEGIN -- User already in organization
				UPDATE [Auth].[OrganizationUser]
				SET [OrgRoleId] = @OrgRole,
					[EmployeeId] = @EmployeeId,
					[EmployeeTypeId] = @EmployeeTypeId
				WHERE [UserId] = @UserId AND 
					[OrganizationId] = @OrganizationId;
			END
			ELSE
			BEGIN -- User not in organization
				INSERT INTO [Auth].[OrganizationUser]  (
					[UserId], 
					[OrganizationId], 
					[OrgRoleId], 
					[EmployeeId],
					[EmployeeTypeId]
				)
				VALUES (
					@UserId, 
					@OrganizationId,
					@OrgRole, 
					@EmployeeId,
					@EmployeeTypeId
				);
			END

			---- Add user to project, if invited
			--IF @ProjectId IS NOT NULL
			--BEGIN
			--	IF EXISTS (
			--		SELECT * FROM [Crm].[ProjectUser] WITH (NOLOCK)
			--		WHERE [ProjectUser].[UserId] = @UserId AND [ProjectUser].[ProjectId] = @ProjectId
			--	)
			--	BEGIN -- User already assigned to project at some point
			--		UPDATE [Crm].[ProjectUser] SET [IsActive] = 1
			--		WHERE [ProjectId] = @ProjectId AND [UserId] = @UserId;
			--	END
			--	ELSE
			--	BEGIN -- User never assigned to project
			--		INSERT INTO [Crm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
			--		VALUES(@ProjectId, @UserId, 1);
			--	END
			--END

			---- Get invitation sub roles
			---- There may be multiple, so we will place them in a table and loop through it
			--DECLARE @SubRoles TABLE (
			--	InviteId INT,
			--	SubscriptionId INT,
			--	ProductRoleId INT,
			--	NumberOfUsers INT,
			--	SubscriptionsUsed INT
			--);
			--INSERT INTO @SubRoles (InviteId, SubscriptionId, ProductRoleId, NumberOfUsers, SubscriptionsUsed)
			--SELECT 	[Invitation].[InvitationId],
			--		[InvitationSubRole].[SubscriptionId],
			--		[ProductRoleId],
			--		[NumberOfUsers],
			--		(SELECT COUNT([UserId])
			--			FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
			--			WHERE [SubscriptionUser].[SubscriptionId] = [Subscription].[SubscriptionId]
			--		) AS [SubscriptionsUsed]
			--FROM [Auth].[InvitationSubRole] WITH (NOLOCK)
			--JOIN [Auth].[Invitation] WITH (NOLOCK) ON [Invitation].[InvitationId] = [InvitationSubRole].[InvitationId]
			--JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [InvitationSubRole].[SubscriptionId]
			--WHERE [InvitationSubRole].[InvitationId] = @InvitationId
			--AND [Invitation].[IsActive] = 1;

			DECLARE @Inv INT;
			DECLARE @Sub INT;
			DECLARE @ProdRole INT;
			DECLARE @NumUsers INT;
			DECLARE @UserCount INT;
			WHILE EXISTS(SELECT * FROM @SubRoles)
			BEGIN
				SELECT TOP 1
					@Inv = InviteId,
					@Sub = SubscriptionId,
					@ProdRole = ProductRoleId,
					@NumUsers = NumberOfUsers,
					@UserCount = SubscriptionsUsed
				FROM @SubRoles
				IF @Sub IS NOT NULL AND @UserCount < @NumUsers
				BEGIN --Only add a subscriptoin role if the subscription still exists and it is not full
					IF EXISTS (
						SELECT * FROM [Billing].[SubscriptionUser]
						WHERE [SubscriptionUser].[UserId] = @UserId AND [SubscriptionUser].[SubscriptionId] = @Sub
					)
					BEGIN --User already part of subscription
						UPDATE [Billing].[SubscriptionUser] 
						SET [ProductRoleId] = @ProdRole
						WHERE [SubscriptionId] = @Sub AND [UserId] = @UserId;
					END
					ELSE
					BEGIN --User not part of subscription
						INSERT INTO [Billing].[SubscriptionUser] ([SubscriptionId], [UserId], [ProductRoleId]) 
						VALUES(@Sub, @UserId, @ProdRole);
					END
				END
				DELETE @SubRoles
				WHERE InviteId = @Inv AND SubscriptionId = @Sub
			END

			-- Remove invitation and its sub roles
			DELETE FROM [Auth].[InvitationSubRole]
			WHERE [InvitationId] = @InvitationId

			DELETE FROM [Auth].[Invitation]
			WHERE [InvitationId] = @InvitationId
			
			-- On success, return name of organization and role
			SELECT [Organization].[Name]
			FROM [Auth].[Organization]
			WHERE [Organization].[OrganizationId] = @OrganizationId

			SELECT [OrgRole].[Name]
			FROM [Auth].[OrgRole]
			WHERE [OrgRole].[OrgRoleId] = @OrgRole

			COMMIT
		END
	END
END