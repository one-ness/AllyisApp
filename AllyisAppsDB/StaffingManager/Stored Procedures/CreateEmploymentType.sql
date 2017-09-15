CREATE PROCEDURE [StaffingManager].[CreateEmploymentType]
	@organizationId		INT,
	@employmentTypeName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [StaffingManager].[EmploymentType] 
		([OrganizationId],
		[EmploymentTypeName])
	VALUES 	 
		(@organizationId,
		@employmentTypeName)
END
