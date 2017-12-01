CREATE PROCEDURE [Pjm].[CreateProject]
	@customerId INT,
	@projectName NVARCHAR(MAX),
	@isDefault INT = 0,
	@isHourly BIT,
	@projectCode NVARCHAR(16),
	@startingDate DATETIME2(0),
	@endingDate DATETIME2(0),
    @retId INT OUTPUT
AS
BEGIN

	DECLARE @count INT 
	SET @count = (SELECT COUNT(*) 
	FROM [Pjm].[Project] AS [P]
	JOIN [Crm].[Customer] AS [C] ON [C].[CustomerId] = [P].[CustomerId] 
	WHERE [C].[CustomerId] = @customerId)

	SET NOCOUNT ON;
	BEGIN TRANSACTION

	INSERT INTO [Pjm].[Project] ([CustomerId], [ProjectName],[IsDefault] ,[IsHourly], [ProjectCode], [StartUtc], [EndUtc])
	VALUES	(@customerId, @projectName, @isDefault, @isHourly, @projectCode, @startingDate, @endingDate);

	SET @retId = SCOPE_IDENTITY()
	
	COMMIT TRANSACTION
	SELECT SCOPE_IDENTITY();
END