CREATE PROCEDURE [TimeTracker].[GetStartOfWeek]
	@OrganizationId INT
	
AS
	SET NOCOUNT ON;
DECLARE @Default AS INT
SET @Default = 1
IF EXISTS (SELECT [StartOfWeek] FROM [TimeTracker].[Setting] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId)
	BEGIN
		SELECT
			[StartOfWeek]
		FROM [TimeTracker].[Setting]
		WITH (NOLOCK) 
		WHERE [OrganizationId] = @OrganizationId
	END
ELSE
	BEGIN
		SELECT @Default
	END