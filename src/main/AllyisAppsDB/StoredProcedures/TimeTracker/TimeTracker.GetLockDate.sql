CREATE PROCEDURE [TimeTracker].[GetLockDate]
	@OrganizationId INT,
	@UserId INT
AS
	SET NOCOUNT ON;
	SELECT [TTLockDate] FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId AND [UserId] = @UserId;
