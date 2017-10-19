CREATE PROCEDURE [TimeTracker].[UpdatePayrollProcessedDate]
	@organizationId INT,
	@payrollProcessedDate DATETIME2
AS
	UPDATE [TimeTracker].[Setting]
	SET [PayrollProcessedDate] = @payrollProcessedDate
	WHERE [OrganizationId] = @organizationId;
