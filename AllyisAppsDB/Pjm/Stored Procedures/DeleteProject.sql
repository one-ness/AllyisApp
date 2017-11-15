CREATE PROCEDURE [Pjm].[DeleteProject]
	@projectId INT,
	@deactivateDate DATE
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the project's name
	DECLARE @projectName NVARCHAR(384);
	DECLARE @startDate DATE;

	SELECT 
		@projectName = [ProjectName],
		@startDate = [Project].[StartUtc]
	FROM [Pjm].[Project] WITH (NOLOCK)
	WHERE [ProjectId] = @projectId

	IF (@startDate IS NOT NULL AND @startDate > @deactivateDate)
	BEGIN
		SET @startDate = null
	END

	IF @projectName IS NOT NULL 
	BEGIN --Project found
		UPDATE [Pjm].[Project]
		SET [IsActive] = 0, [EndUtc] = @deactivateDate, [StartUtc] = @startDate
		WHERE [ProjectId] = @projectId
	 
		UPDATE [Pjm].[ProjectUser] SET [IsActive] = 0
		WHERE [ProjectUser].[ProjectId] = @projectId;
	END
	SELECT @projectName
END