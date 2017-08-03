CREATE PROCEDURE [Hrm].[CreateHoliday]
	@HolidayName NVARCHAR(50),
	@Date DATE,
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[Holiday] ([HolidayName], [Date], [OrganizationId]) VALUES (@HolidayName, @Date, @OrganizationId);
	
	declare 
		@payClassIdForHoliday int;
		
	IF (SELECT COUNT([PayClassId]) FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday') > 0
		SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday');
	ELSE
		BEGIN
			EXEC [Hrm].[CreatePayClass] @PayClassName = 'Holiday', @OrganizationId = @OrganizationId
			SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday');
		END
		
	EXEC [TimeTracker].[CreateBulkTimeEntry]
		@Date = @Date,
		@Duration = 8, 
		@Description = @HolidayName,
		@PayClassId = @payClassIdForHoliday,
		@OrganizationId = @OrganizationId,
		@Overwrite = 0;
SELECT SCOPE_IDENTITY();
