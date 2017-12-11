CREATE PROCEDURE [TimeTracker].[GetTimeEntriesThatUseAPayClass]
	@payClassId INT
AS
	SET NOCOUNT ON;
SELECT DISTINCT
	*
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK)
WHERE [PayClassId] = @payClassId