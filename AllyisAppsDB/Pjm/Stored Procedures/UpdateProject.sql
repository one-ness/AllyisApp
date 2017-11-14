CREATE PROCEDURE [Pjm].[UpdateProject]
	@projectId INT,
	@projectName NVARCHAR(MAX),
	@isHourly BIT,
    @startingDate DATE,
    @endingDate DATE,
	@projectCode NVARCHAR(16)
AS
	SET NOCOUNT ON;
	UPDATE [Pjm].[Project]
	SET 
		[ProjectName] = @projectName,
		[IsHourly] = @isHourly,
		[StartUtc] = @startingDate,
		[EndUtc] = @endingDate,
		[ProjectCode] = @projectCode

	WHERE [ProjectId] = @projectId