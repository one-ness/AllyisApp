CREATE PROCEDURE [TimeTracker].[UpdateOvertime]
	@organizationId INT,
	@overtimeHours INT,
	@overtimePeriod VARCHAR(10)

AS
	SET NOCOUNT ON;
SET XACT_ABORT ON
BEGIN
	UPDATE [TimeTracker].[Setting]
	SET [OvertimeHours] = @overtimeHours,
		[OvertimePeriod] = @overtimePeriod
	WHERE [OrganizationId] = @organizationId
END