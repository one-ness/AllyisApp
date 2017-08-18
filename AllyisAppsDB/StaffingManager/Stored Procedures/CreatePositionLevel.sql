CREATE PROCEDURE [StaffingManager].[CreatePositionLevel]
	@organizationId		INT,
	@positionLevelName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [StaffingManager].[PositionLevel] 
		([OrganizationId],
		[PositionLevelName])
	VALUES 	 
		(@organizationId,
		@positionlevelName)
END
