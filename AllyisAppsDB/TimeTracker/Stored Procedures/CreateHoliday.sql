CREATE PROCEDURE [TimeTracker].[CreateHoliday]
	@HolidayName NVARCHAR(50),
	@Date DATETIME2(0),
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [TimeTracker].[Holiday] ([HolidayName], [Date], [OrganizationId]) VALUES (@HolidayName, @Date, @OrganizationId);
	
	declare 
		@payClassIdForHoliday int;
		
	IF (SELECT COUNT([PayClassID]) FROM [PayClass] WITH (NOLOCK) WHERE [Name] = 'Holiday') > 0
		SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassID] FROM [PayClass] WITH (NOLOCK) WHERE [Name] = 'Holiday');
	ELSE
		BEGIN
			EXEC [TimeTracker].[CreatePayClass] @Name = 'Holiday', @OrganizationId = @OrganizationId
			SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassID] FROM [PayClass] WITH (NOLOCK) WHERE [Name] = 'Holiday');
		END
		
	EXEC [TimeTracker].[CreateBulkTimeEntry]
		@Date = @Date,
		@Duration = 8, 
		@Description = @HolidayName,
		@PayClassId = @payClassIdForHoliday,
		@OrganizationId = @OrganizationId,
		@Overwrite = 0;
SELECT SCOPE_IDENTITY();