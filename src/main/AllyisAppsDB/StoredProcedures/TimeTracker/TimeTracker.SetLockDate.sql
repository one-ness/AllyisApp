CREATE PROCEDURE [TimeTracker].[SetLockDate]
	@OrganizationId INT,
	@UserId INT,
	@Date DATETIME2(0)
AS
	SET NOCOUNT ON;
	UPDATE [Auth].[OrganizationUser] SET [TTLockDate] = @Date WHERE [OrganizationId] = @OrganizationId AND [UserId] = @UserId;