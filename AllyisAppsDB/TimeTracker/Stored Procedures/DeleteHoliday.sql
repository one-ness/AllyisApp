﻿CREATE PROCEDURE [Hrm].[DeleteHoliday]
	@HolidayName NVarChar(120),
	@Date DATE,
	@OrganizationId INT
AS
	SET NOCOUNT ON;

	DELETE FROM [Hrm].[Holiday] WHERE [HolidayName] = @HolidayName AND [Date] = @Date AND [OrganizationId] = @OrganizationId;

	BEGIN
		DELETE FROM [TimeTracker].[TimeEntry]
		WHERE [Date] = @Date
		AND [Duration] = 8
		AND [PayClassId] = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday')
		AND [ProjectId] IN (SELECT [ProjectId]
							FROM [Pjm].[Project] WITH (NOLOCK) JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
							WHERE [Customer].[OrganizationId] = @OrganizationId);
	END