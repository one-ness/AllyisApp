CREATE PROCEDURE [Staffing].[CreatePositionStatus]
	@organizationId		INT,
	@positionStatusName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Staffing].[PositionStatus] 
		([OrganizationId],
		[PositionStatusName])
	VALUES 	 
		(@organizationId,
		@positionStatusName)
END
