CREATE PROCEDURE [Pjm].[DeleteProject]
	@ProjectId INT,
	@DeactivateDate DATE
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the project's name
	DECLARE @ProjectName NVARCHAR(384);

	SELECT 
		@ProjectName = [ProjectName] 
	FROM [Pjm].[Project] WITH (NOLOCK)
	WHERE [ProjectId] = @ProjectId

	IF @ProjectName IS NOT NULL
	BEGIN --Project found
		UPDATE [Pjm].[Project]
		SET [IsActive] = 0, [EndUtc] = @DeactivateDate
		WHERE [ProjectId] = @ProjectId
	 
		UPDATE [Pjm].[ProjectUser] SET [IsActive] = 0
		WHERE [ProjectUser].[ProjectId] IN (SELECT [ProjectId] FROM [Pjm].[Project] WHERE [IsActive] = 0);
	END
	SELECT @ProjectName
END