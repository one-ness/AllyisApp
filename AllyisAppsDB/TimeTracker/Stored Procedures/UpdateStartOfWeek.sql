CREATE PROCEDURE [TimeTracker].[UpdateStartOfWeek]
	@organizationId INT,
	@startOfWeek INT
AS
	SET NOCOUNT ON;
UPDATE [TimeTracker].[Setting]
	SET
		[StartOfWeek] = @startOfWeek
	WHERE [OrganizationId] = @organizationId ;