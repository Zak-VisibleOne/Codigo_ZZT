DECLARE @@CustID int;
DECLARE @@sqlstate varchar(max);
SET @@CustID = {0};
SET @@sqlstate = 'SELECT dbo.SaleHeader.ID, dbo.SaleHeader.SaleDate, dbo.SaleHeader.InvoiceNo, dbo.SaleHeader.CustID, dbo.Customer.CustName, 
isnull(dbo.SaleHeader.TotalAmount,'''') as TotalAmount, dbo.SaleHeader.CrtDate
FROM   dbo.SaleHeader LEFT JOIN  dbo.Customer 
       ON dbo.Customer.CustId = dbo.SaleHeader.CustID WHERE 1 = 1';

IF (@@CustID <> 0) BEGIN
	SET @@sqlstate = @@sqlstate + ' and dbo.SaleHeader.CustID ='+ @@CustID
END
execute(@@sqlstate);