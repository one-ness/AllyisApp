CREATE PROCEDURE [Hrm].[CreateDefaultPayClass]
	@organizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId]) VALUES ('Regular',           @organizationId);
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId]) VALUES ('Paid Time Off',     @organizationId);
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId]) VALUES ('Unpaid Time Off',   @organizationId);
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId]) VALUES ('Holiday',           @organizationId);
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId]) VALUES ('Bereavement Leave', @organizationId);
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId]) VALUES ('Jury Duty',         @organizationId);
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId]) VALUES ('Overtime',          @organizationId);
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId]) VALUES ('Other Leave',       @organizationId);
