CREATE PROCEDURE [StaffingManager].[CreatePositionStatus]
	@organizationId		INT,
	@positionStatusName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [StaffingManager].[PositionStatus] 
		([OrganizationId],
		[PositionStatusName])
	VALUES 	 
		(@organizationId,
		@positionStatusName)
END
