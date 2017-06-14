﻿CREATE PROCEDURE [TimeTracker].[GetTimeEntriesByUserOverDateRange]
	@UserId [Auth].[UserTable] READONLY,
	@OrganizationId INT,
	@StartingDate DATE,
	@EndingDate DATE
AS
	SET NOCOUNT ON;
SELECT DISTINCT [TimeEntryId] 
	,[User].[UserId] AS [UserId]
	,[User].[FirstName] AS [FirstName]
	,[User].[LastName] AS [LastName]
	,[User].[Email]
	,[OrganizationUser].[EmployeeId]
	,[TimeEntry].[ProjectId]
	,[TimeEntry].[PayClassId]
	,[PayClass].[Name] AS [PayClassName]
	,[Date]
	,[Duration]
	,[Description]
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
JOIN [TimeTracker].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassID] = [TimeEntry].[PayClassId]
JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @OrganizationId
WHERE [User].[UserId] IN (SELECT [UserId] FROM @UserId)
	AND [Date] >= @StartingDate
	AND [Date] <= @EndingDate
	AND [PayClass].[OrganizationId] = @OrganizationId
ORDER BY [Date] ASC