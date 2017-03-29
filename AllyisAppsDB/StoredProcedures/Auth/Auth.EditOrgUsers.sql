CREATE PROCEDURE [Auth].[EditOrgUsers]
	@OrganizationId	INT,
	@UserIDs [Auth].[UserTable] READONLY,
	@OrgRole INT
AS
BEGIN TRANSACTION
	IF @OrgRole = -1 -- Removing users from org
	BEGIN
		-- Delete project user records for these users in this organization
		DELETE FROM [Crm].[ProjectUser] 
		WHERE [UserId] IN (
				SELECT [userId] FROM @UserIDs
			) AND
			[ProjectId] IN (
				SELECT [ProjectId] FROM [Crm].[Project] WITH (NOLOCK)
				WHERE [CustomerId] IN (
					SELECT [CustomerId] FROM [Crm].[Customer] WITH (NOLOCK)
					WHERE [OrganizationId] = @OrganizationId
				)
			);

		-- Delete subscription user records for these users in this organization
		DELETE FROM [Billing].[SubscriptionUser]
		WHERE [UserId] IN (
				SELECT [userId] FROM @UserIDs
			) AND
			[SubscriptionId] IN (
				SELECT [SubscriptionId] FROM [Billing].[Subscription] WITH (NOLOCK)
				WHERE [OrganizationId] = @OrganizationId
			);

		-- Delete organization user records
		DELETE FROM [Auth].[OrganizationUser]
		WHERE [UserId] IN (
				SELECT [userId] FROM @UserIDs
			) AND [OrganizationId] = @OrganizationId

		SELECT @@ROWCOUNT
	END
	IF @OrgRole > 0 -- Assigning org role
	BEGIN
		UPDATE [Auth].[OrganizationUser]
		SET [OrgRoleId] = @OrgRole
		WHERE [UserId] IN (
				SELECT [userId] FROM @UserIDs
			) AND [OrganizationId] = @OrganizationId

		SELECT @@ROWCOUNT
	END
COMMIT