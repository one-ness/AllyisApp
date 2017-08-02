﻿CREATE PROCEDURE [TimeTracker].[GetTimeEntriesThatUseAPayClass]
	@PayClassId INT
AS
	SET NOCOUNT ON;
SELECT DISTINCT [TimeEntryId], [ProjectId], [PayClassId], [Duration], [Description], [IsLockSaved] 
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK)
WHERE [PayClassId] = @PayClassId