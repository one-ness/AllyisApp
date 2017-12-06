CREATE PROCEDURE [TimeTracker].[UpdateStartOfWeek]
	@organizationId INT,
	@startOfWeek INT
AS
	UPDATE [TimeTracker].[Setting]
	SET [StartOfWeek] = @startOfWeek
	WHERE [OrganizationId] = @organizationId
