CREATE PROCEDURE [TimeTracker].[UpdateOvertimeChangedFlag]
	@organizationId INT,
	@flag BIT
AS
	UPDATE [TimeTracker].[Setting]
	SET [OtSettingRecentlyChanged] = @flag
	WHERE [OrganizationId] = @organizationId
