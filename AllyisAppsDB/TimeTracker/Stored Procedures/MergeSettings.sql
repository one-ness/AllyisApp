CREATE PROCEDURE [TimeTracker].[MergeSettings]
	@organizationId INT,
	@startOfWeek INT,
	@overtimeHours INT,
	@overtimePeriod VARCHAR(10)
AS

SET NOCOUNT ON;

WITH [S] AS (
	SELECT [StartOfWeek] = @startOfWeek,
		[OvertimeHours] = @overtimeHours,
		[OvertimePeriod] = @overtimePeriod,
		[OrganizationId] = @organizationId
)
MERGE [TimeTracker].[Setting] WITH (HOLDLOCK) AS [T]
USING [S]
ON [T].[OrganizationId] = [S].[OrganizationId]
WHEN MATCHED THEN
	UPDATE SET
		[T].[StartOfWeek] = [S].[StartOfWeek],
		[T].[OvertimeHours] = [S].[OvertimeHours],
		[T].[OvertimePeriod] = [S].[OvertimePeriod]
WHEN NOT MATCHED THEN
	INSERT (
		[OrganizationId],
		[StartOfWeek],
		[OvertimeHours],
		[OvertimePeriod])
	VALUES (
		[S].[OrganizationId],
		[S].[StartOfWeek],
		[S].[OvertimeHours],
		[S].[OvertimePeriod]);
