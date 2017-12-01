CREATE PROCEDURE [TimeTracker].[UpdateUserOrgEmployeeType]
	@userId INT,
	@employeeType int
AS
	UPDATE [Auth].[OrganizationUser] SET [EmployeeTypeId] = @employeeType WHERE [UserId] = @userId
