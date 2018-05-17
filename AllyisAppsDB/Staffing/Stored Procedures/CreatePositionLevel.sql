CREATE PROCEDURE [Staffing].[CreatePositionLevel]
	@organizationId		INT,
	@positionLevelName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Staffing].[PositionLevel] 
		([OrganizationId],
		[PositionLevelName])
	VALUES
		(@organizationId,
		@positionLevelName)
END
