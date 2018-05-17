CREATE PROCEDURE [Staffing].[CreateStaffingSettings]
	@organizationId			 INT,
	@subscriptionId			 INT,
	@defaultPositionName     NVARCHAR,
	@defaultApplicationName  NVARCHAR

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Staffing].[PositionStatus] 
		([OrganizationId],
		 [PositionStatusName])
	VALUES 	 
		(@organizationId,
		 @defaultPositionName)
		
		DECLARE @positionStatusId INT
		SET @positionStatusId = IDENT_CURRENT('[Staffing].[PositionStatus]')


	INSERT INTO [Staffing].[ApplicationStatus] 
		([OrganizationId],
		 [ApplicationStatusName])
	VALUES 	 
		(@organizationId,
		 @defaultPositionName)
		
		DECLARE @applicationStatusId INT
		SET @applicationStatusId = IDENT_CURRENT('[Staffing].[ApplicationStatus]')


	INSERT INTO [Staffing].[StaffingSettings] 
		([SubscriptionId],
		[DefaultPositionStatusId],
		[DefaultApplicationStatusId])
	VALUES 	 
		(@subscriptionId,
		 @positionStatusId,
		 @applicationStatusId)
END
