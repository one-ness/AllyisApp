CREATE PROCEDURE [Crm].[CreateProjectFromCustomerIdOnly]
	@CustomerId INT,
	@Name NVARCHAR(MAX),
	@PriceType NVARCHAR(20),
	@ProjectOrgId NVARCHAR(16),
	@StartingDate DATETIME2(0),
	@EndingDate DATETIME2(0),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	INSERT INTO [Crm].[Project] ([CustomerId], [Name], [Type], [ProjectOrgId], [StartUTC], [EndUTC])
	VALUES (@CustomerId, 
			@Name,
			@PriceType,
			@ProjectOrgId,
			@StartingDate,
			@EndingDate)
	SET @retId = SCOPE_IDENTITY()
	COMMIT TRANSACTION
	SELECT SCOPE_IDENTITY();
END
