CREATE PROCEDURE [TimeTracker].[UpdateSettings]
	@OrganizationId INT,
    @StartOfWeek INT,
	@OvertimeHours INT,
	@OvertimePeriod VARCHAR(10),
	@OvertimeMultiplier DECIMAL(9,4)

AS
	SET NOCOUNT ON;
SET XACT_ABORT ON
IF EXISTS ( SELECT [OrganizationId]
			FROM [TimeTracker].[Setting] WITH (NOLOCK) 
			WHERE [OrganizationId] = @OrganizationId)
BEGIN
UPDATE [TimeTracker].[Setting]
	SET
		[StartOfWeek] = @StartOfWeek,
		[OvertimeHours] = @OvertimeHours,
		[OvertimePeriod] = @OvertimePeriod,
		[OvertimeMultiplier] = @OvertimeMultiplier
	WHERE [OrganizationId] = @OrganizationId ;
END
 
ELSE  
BEGIN
	INSERT INTO [TimeTracker].[Setting] ([OrganizationId], [StartOfWeek], [OvertimeHours], [OvertimePeriod], [OvertimeMultiplier])	  
	VALUES (@OrganizationId, @StartOfWeek, @OvertimeHours, @OvertimePeriod, @OvertimeMultiplier);
END