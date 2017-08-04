CREATE FUNCTION [Billing].[GetNumberSubscriptionUsers](@subscriptoinID Int)
RETURNS INT
AS
BEGIN
	Return (SELECT COUNT(*) FROM [Billing].[SubscriptionUser] [s] Where [s].SubscriptionId = @subscriptoinID); 
END