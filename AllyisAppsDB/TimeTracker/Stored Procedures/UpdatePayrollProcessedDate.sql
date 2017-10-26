CREATE PROCEDURE [TimeTracker].[UpdatePayrollProcessedDate]
	@organizationId INT,
	@payrollProcessedDate DATE
AS
	UPDATE [TimeTracker].[Setting]
	SET [PayrollProcessedDate] = @payrollProcessedDate
	WHERE [OrganizationId] = @organizationId;
