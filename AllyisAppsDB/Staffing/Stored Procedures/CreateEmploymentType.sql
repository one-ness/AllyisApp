CREATE PROCEDURE [Staffing].[CreateEmploymentType]
	@organizationId		INT,
	@employmentTypeName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Staffing].[EmploymentType] 
		([OrganizationId],
		[EmploymentTypeName])
	VALUES 	 
		(@organizationId,
		@employmentTypeName)
END
