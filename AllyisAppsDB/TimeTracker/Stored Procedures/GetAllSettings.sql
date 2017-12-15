CREATE PROCEDURE [TimeTracker].[GetAllSettings]
	@organizationId INT
AS
	SET NOCOUNT ON;
	SELECT [OrganizationId], [StartOfWeek], [OvertimeHours], [OvertimePeriod], [Setting].LockDate, [Setting].PayrollProcessedDate
	FROM [TimeTracker].[Setting] WITH (NOLOCK) 
	WHERE [OrganizationId] = @organizationId;

	IF(SELECT COUNT(*) FROM [Hrm].[PayClass] WHERE [OrganizationId] = @organizationId) > 0
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId;
	ELSE
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

	IF(SELECT COUNT(*) FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId) > 0
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId ORDER BY [Date];
	ELSE
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = 0 ORDER BY [Date];