CREATE PROCEDURE [TimeTracker].[UpdatePayPeriod]
	@payPeriodJson VARCHAR(max),
	@organizationId INT
AS
	UPDATE [TimeTracker].[Setting]
	SET [PayPeriod] = @payPeriodJson
	WHERE [OrganizationId] = @organizationId;
