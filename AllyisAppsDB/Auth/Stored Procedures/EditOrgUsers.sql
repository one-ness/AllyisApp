CREATE PROCEDURE [Auth].[EditOrgUsers]
	@OrganizationId	INT,
	@UserIds [Auth].[UserTable] READONLY,
	@OrganizationRole INT
AS
BEGIN TRANSACTION
	IF @OrganizationRole = -1 -- Removing users from org
	BEGIN
		-- Delete project user records for these users in this organization
		DELETE FROM [Pjm].[ProjectUser] 
		WHERE [UserId] IN (
				SELECT [userId] FROM @UserIds
			) AND
			[ProjectId] IN (
				SELECT [ProjectId] FROM [Pjm].[Project] WITH (NOLOCK)
				WHERE [CustomerId] IN (
					SELECT [CustomerId] FROM [Crm].[Customer] WITH (NOLOCK)
					WHERE [OrganizationId] = @OrganizationId
				)
			);

		-- Delete subscription user records for these users in this organization
		DELETE FROM [Billing].[SubscriptionUser]
		WHERE [UserId] IN (
				SELECT [userId] FROM @UserIds
			) AND
			[SubscriptionId] IN (
				SELECT [SubscriptionId] FROM [Billing].[Subscription] WITH (NOLOCK)
				WHERE [OrganizationId] = @OrganizationId
			);

		-- Delete organization user records
		DELETE FROM [Auth].[OrganizationUser]
		WHERE [UserId] IN (
				SELECT [userId] FROM @UserIds
			) AND [OrganizationId] = @OrganizationId

		SELECT @@ROWCOUNT
	END
	IF @OrganizationRole > 0 -- Assigning org role
	BEGIN
		UPDATE [Auth].[OrganizationUser]
		SET [OrganizationRoleId] = @OrganizationRole
		WHERE [UserId] IN (
				SELECT [userId] FROM @UserIds
			) AND [OrganizationId] = @OrganizationId

		SELECT @@ROWCOUNT
	END
COMMIT