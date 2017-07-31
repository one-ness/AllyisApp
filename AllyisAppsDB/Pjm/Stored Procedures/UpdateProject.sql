CREATE PROCEDURE [Pjm].[UpdateProject]
	@ProjectId INT,
	@Name NVARCHAR(MAX),
	@PriceType NVARCHAR(20),
    @StartingDate DATE,
    @EndingDate DATE,
	@ProjectOrgId NVARCHAR(16)
AS
	SET NOCOUNT ON;
	UPDATE [Pjm].[Project]
	SET 
		[Name] = @Name,
		[Type] = @PriceType,
		[StartUtc] = @StartingDate,
		[EndUtc] = @EndingDate,
		[ProjectOrgId] = @ProjectOrgId

	WHERE [ProjectId] = @ProjectId