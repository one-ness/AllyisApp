CREATE PROCEDURE [TimeTracker].[GetSettings]
	@organizationId INT
AS
	SET NOCOUNT ON;
	SELECT	[OrganizationId],
			[StartOfWeek],
			[OvertimeHours],
			[OvertimePeriod],
			[PayrollProcessedDate],
			[LockDate],
			[PayPeriod],
			[OtSettingRecentlyChanged]
	FROM [TimeTracker].[Setting] WITH (NOLOCK) 
	WHERE [OrganizationId] = @organizationId;
