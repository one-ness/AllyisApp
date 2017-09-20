CREATE PROCEDURE [Auth].[UpdateUserMaxAmount]
	@userId int,
	@orgId int,
	@maxAmount decimal
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [Auth].[OrganizationUser]
	SET [MaxAmount] = @maxAmount
	WHERE [UserId] = @userId AND [OrganizationId] = @orgId;
END
