CREATE PROCEDURE [TimeTracker].[UpdateOvertime]
	@organizationId INT,
	@overtimeHours INT,
	@overtimePeriod VARCHAR(10),
	@overtimeMultiplier DECIMAL(9,4)

AS
	SET NOCOUNT ON;
SET XACT_ABORT ON
BEGIN
UPDATE [TimeTracker].[Setting]
	SET
		[OvertimeHours] = @overtimeHours,
		[OvertimePeriod] = @overtimePeriod,
		[OvertimeMultiplier] = @overtimeMultiplier
	WHERE [OrganizationId] = @organizationId ;
END