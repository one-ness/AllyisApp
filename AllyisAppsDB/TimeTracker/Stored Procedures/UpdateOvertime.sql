CREATE PROCEDURE [TimeTracker].[UpdateOvertime]
	@organizationId INT,
	@overtimeHours INT,
	@overtimePeriod VARCHAR(10)

AS
	UPDATE [TimeTracker].[Setting]
	SET [OvertimeHours] = @overtimeHours,
		[OvertimePeriod] = @overtimePeriod
	WHERE [OrganizationId] = @organizationId
