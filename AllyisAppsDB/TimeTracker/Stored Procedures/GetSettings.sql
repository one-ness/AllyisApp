CREATE PROCEDURE [TimeTracker].[GetSettings]
	@organizationId INT
AS
	SET NOCOUNT ON;
	SELECT	[OrganizationId],
			[StartOfWeek],
			[OvertimeHours],
			[OvertimePeriod],
			[OvertimeMultiplier],
			[IsLockDateUsed],
			[LockDatePeriod],
			[LockDateQuantity],
			[PayrollProcessedDate],
			[LockDate],
			[PayPeriod]
	FROM [TimeTracker].[Setting] WITH (NOLOCK) 
	WHERE [OrganizationId] = @organizationId;
