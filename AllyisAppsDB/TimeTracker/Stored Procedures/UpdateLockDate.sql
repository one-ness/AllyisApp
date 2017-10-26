CREATE PROCEDURE [TimeTracker].[UpdateLockDate]
	@organizationId INT,
	@lockDate DATE
AS
	UPDATE [TimeTracker].[Setting]
	SET [LockDate] = @lockDate
	WHERE [OrganizationId] = @organizationId;
