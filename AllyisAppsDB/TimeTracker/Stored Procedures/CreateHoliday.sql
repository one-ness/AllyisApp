CREATE PROCEDURE [Hrm].[CreateHoliday]
	@holidayName NVARCHAR(50),
	@date DATE,
	@organizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[Holiday] ([HolidayName], [Date], [OrganizationId]) VALUES (@holidayName, @date, @organizationId);
	
	declare 
		@payClassIdForHoliday int;
		
	IF (SELECT COUNT([PayClassId]) FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday') > 0
		SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday');
	ELSE
		BEGIN
			EXEC [Hrm].[CreatePayClass] @payClassName = 'Holiday', @organizationId = @organizationId
			SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday');
		END
		
	EXEC [TimeTracker].[CreateBulkTimeEntry]
		@date = @date,
		@duration = 8, 
		@description = @holidayName,
		@payClassId = @payClassIdForHoliday,
		@organizationId = @organizationId,
		@overwrite = 0;
SELECT SCOPE_IDENTITY();
