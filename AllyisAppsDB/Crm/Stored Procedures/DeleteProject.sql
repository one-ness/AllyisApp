CREATE PROCEDURE [Crm].[DeleteProject]
	@ProjectId INT,
	@DeactivateDate DATE
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the project's name
	DECLARE @ProjectName NVARCHAR(384);

	SELECT 
		@ProjectName = [Name] 
	FROM [Crm].[Project] WITH (NOLOCK)
	WHERE [ProjectId] = @ProjectId

	IF @ProjectName IS NOT NULL
	BEGIN --Project found
		UPDATE [Crm].[Project]
		SET [IsActive] = 0, [EndUTC] = @DeactivateDate
		WHERE [ProjectId] = @ProjectId
	 
		UPDATE [Crm].[ProjectUser] SET [IsActive] = 0
		WHERE [ProjectUser].[ProjectId] IN (SELECT [ProjectId] FROM [Crm].[Project] WHERE [IsActive] = 0);
	END
	SELECT @ProjectName
END