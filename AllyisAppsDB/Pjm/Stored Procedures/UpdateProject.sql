CREATE PROCEDURE [Pjm].[UpdateProject]
	@ProjectId INT,
	@ProjectName NVARCHAR(MAX),
	@IsHourly BIT,
    @StartingDate DATE,
    @EndingDate DATE,
	@ProjectOrgId NVARCHAR(16)
AS
	SET NOCOUNT ON;
	UPDATE [Pjm].[Project]
	SET 
		[ProjectName] = @ProjectName,
		[IsHourly] = @IsHourly,
		[StartUtc] = @StartingDate,
		[EndUtc] = @EndingDate,
		[ProjectOrgId] = @ProjectOrgId

	WHERE [ProjectId] = @ProjectId