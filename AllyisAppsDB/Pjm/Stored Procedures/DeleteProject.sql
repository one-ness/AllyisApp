CREATE PROCEDURE [Pjm].[DeleteProject]
	@projectId INT,
	@deactivateDate DATE
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the project's name
	DECLARE @projectName NVARCHAR(384);

	SELECT 
		@projectName = [ProjectName] 
	FROM [Pjm].[Project] WITH (NOLOCK)
	WHERE [ProjectId] = @projectId

	IF @projectName IS NOT NULL
	BEGIN --Project found
		UPDATE [Pjm].[Project]
		SET [IsActive] = 0, [EndUtc] = @deactivateDate
		WHERE [ProjectId] = @projectId
	 
		UPDATE [Pjm].[ProjectUser] SET [IsActive] = 0
		WHERE [ProjectUser].[ProjectId] = @projectId;
	END
	SELECT @projectName
END