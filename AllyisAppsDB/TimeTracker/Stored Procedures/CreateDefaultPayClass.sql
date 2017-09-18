CREATE PROCEDURE [Hrm].[CreateDefaultPayClass]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Regular',           @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Paid Time Off',     @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Unpaid Time Off',   @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Holiday',           @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Bereavement Leave', @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Jury Duty',         @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Overtime',          @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Other Leave',       @organizationId);
END
