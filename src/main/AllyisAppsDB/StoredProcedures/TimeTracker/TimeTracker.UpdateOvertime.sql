CREATE PROCEDURE [TimeTracker].[UpdateOvertime]
	@OrganizationId INT,
	@OvertimeHours INT,
	@OvertimePeriod VARCHAR(10),
	@OvertimeMultiplier DECIMAL(9,4)

AS
	SET NOCOUNT ON;
SET XACT_ABORT ON
BEGIN
UPDATE [TimeTracker].[Setting]
	SET
		[OvertimeHours] = @OvertimeHours,
		[OvertimePeriod] = @OvertimePeriod,
		[OvertimeMultiplier] = @OvertimeMultiplier
	WHERE [OrganizationId] = @OrganizationId ;
END