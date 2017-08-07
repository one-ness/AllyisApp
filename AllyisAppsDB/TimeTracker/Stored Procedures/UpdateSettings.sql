CREATE PROCEDURE [TimeTracker].[UpdateSettings]
	@organizationId INT,
    @startOfWeek INT,
	@overtimeHours INT,
	@overtimePeriod VARCHAR(10),
	@overtimeMultiplier DECIMAL(9,4)

AS
	SET NOCOUNT ON;
SET XACT_ABORT ON
IF EXISTS ( SELECT [OrganizationId]
			FROM [TimeTracker].[Setting] WITH (NOLOCK) 
			WHERE [OrganizationId] = @organizationId)
BEGIN
UPDATE [TimeTracker].[Setting]
	SET
		[StartOfWeek] = @startOfWeek,
		[OvertimeHours] = @overtimeHours,
		[OvertimePeriod] = @overtimePeriod,
		[OvertimeMultiplier] = @overtimeMultiplier
	WHERE [OrganizationId] = @organizationId ;
END
 
ELSE  
BEGIN
	INSERT INTO [TimeTracker].[Setting] ([OrganizationId], [StartOfWeek], [OvertimeHours], [OvertimePeriod], [OvertimeMultiplier])	  
	VALUES (@organizationId, @startOfWeek, @overtimeHours, @overtimePeriod, @overtimeMultiplier);
END