CREATE PROCEDURE [StaffingManager].[CreateStaffingSettings]
	@organizationId			 INT,
	@subscriptionId			 INT,
	@defaultPositionName     NVARCHAR,
	@defaultApplicationName  NVARCHAR

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [StaffingManager].[PositionStatus] 
		([OrganizationId],
		 [PositionStatusName])
	VALUES 	 
		(@organizationId,
		 @defaultPositionName)
		
		DECLARE @positionStatusId INT
		SET @positionStatusId = IDENT_CURRENT('[StaffingManager].[PositionStatus]')


	INSERT INTO [StaffingManager].[ApplicationStatus] 
		([OrganizationId],
		 [ApplicationStatusName])
	VALUES 	 
		(@organizationId,
		 @defaultPositionName)
		
		DECLARE @applicationStatusId INT
		SET @applicationStatusId = IDENT_CURRENT('[StaffingManager].[ApplicationStatus]')


	INSERT INTO [StaffingManager].[StaffingSettings] 
		([SubscriptionId],
		[DefaultPositionStatusId],
		[DefaultApplicationStatusId])
	VALUES 	 
		(@subscriptionId,
		 @positionStatusId,
		 @applicationStatusId)
END
