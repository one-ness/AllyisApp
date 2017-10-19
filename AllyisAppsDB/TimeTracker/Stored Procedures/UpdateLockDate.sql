CREATE PROCEDURE [TimeTracker].[UpdateLockDate]
	@organizationId INT,
	@lockDate DATETIME2
AS
	UPDATE [TimeTracker].[Setting]
	SET [LockDate] = @lockDate
	WHERE [OrganizationId] = @organizationId;
