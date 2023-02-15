DECLARE @@SupId int;
DECLARE @@sqlstate varchar(max);
SET @@SupId = {0};
SET @@sqlstate = 'SELECT dbo.PurchaseHeader.ID, dbo.PurchaseHeader.PurchaseDate, dbo.PurchaseHeader.InvoiceNo, 
dbo.PurchaseHeader.SupID, dbo.Supplier.SupName, 
isnull(dbo.PurchaseHeader.TotalAmount,'''') as TotalAmount, dbo.PurchaseHeader.CrtDate
FROM   dbo.PurchaseHeader LEFT JOIN  dbo.Supplier 
       ON dbo.Supplier.SupId = dbo.PurchaseHeader.SupId WHERE 1 = 1';

IF (@@SupId <> 0) BEGIN
	SET @@sqlstate = @@sqlstate + ' and dbo.PurchaseHeader.SupId ='+ @@SupId
END
execute(@@sqlstate);