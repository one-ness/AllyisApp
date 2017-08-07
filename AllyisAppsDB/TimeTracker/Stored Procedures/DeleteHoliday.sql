CREATE PROCEDURE [Hrm].[DeleteHoliday]
	@holidayName NVarChar(120),
	@date DATE,
	@organizationId INT
AS
	SET NOCOUNT ON;

	DELETE FROM [Hrm].[Holiday] WHERE [HolidayName] = @holidayName AND [Date] = @date AND [OrganizationId] = @organizationId;

	BEGIN
		DELETE FROM [TimeTracker].[TimeEntry]
		WHERE [Date] = @date
		AND [Duration] = 8
		AND [PayClassId] = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday')
		AND [ProjectId] IN (SELECT [ProjectId]
							FROM [Pjm].[Project] WITH (NOLOCK) JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
							WHERE [Customer].[OrganizationId] = @organizationId);
	END