CREATE PROCEDURE [TimeTracker].[GetHolidays]
	@OrganizationId INT = 0
AS
	SET NOCOUNT ON;
	IF(SELECT COUNT(*) FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId ORDER BY [Date];
	ELSE
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [TimeTracker].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = 0 ORDER BY [Date];
