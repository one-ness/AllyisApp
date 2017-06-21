CREATE PROCEDURE [TimeTracker].[UpdateStartOfWeek]
	@OrganizationId INT,
	@StartOfWeek INT
AS
	SET NOCOUNT ON;
UPDATE [TimeTracker].[Setting]
	SET
		[StartOfWeek] = @StartOfWeek
	WHERE [OrganizationId] = @OrganizationId ;