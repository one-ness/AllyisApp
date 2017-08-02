CREATE PROCEDURE [TimeTracker].[GetAllSettings]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	SELECT [OrganizationId], [StartOfWeek], [OvertimeHours], [OvertimePeriod], [OvertimeMultiplier], [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId;

	IF(SELECT COUNT(*) FROM [Hrm].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassId], [Name], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassId], [Name], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

	IF(SELECT COUNT(*) FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId ORDER BY [Date];
	ELSE
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = 0 ORDER BY [Date];