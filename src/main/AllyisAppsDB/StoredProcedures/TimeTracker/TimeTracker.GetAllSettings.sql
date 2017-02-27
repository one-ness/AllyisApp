CREATE PROCEDURE [TimeTracker].[GetAllSettings]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	SELECT [OrganizationId], [StartOfWeek], [OvertimeHours], [OvertimePeriod], [OvertimeMultiplier], [LockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId;

	IF(SELECT COUNT(*) FROM [TimeTracker].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassID], [Name], [OrganizationId] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassID], [Name], [OrganizationId] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

	IF(SELECT COUNT(*) FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId ORDER BY [Date];
	ELSE
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = 0 ORDER BY [Date];