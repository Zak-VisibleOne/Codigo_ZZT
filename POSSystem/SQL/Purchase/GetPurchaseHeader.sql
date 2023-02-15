SELECT [ID]
	  ,CONVERT(char(10),isnull(PurchaseDate,''),126)  as PurchaseDate
      ,[InvoiceNo]
      ,[SupID]
      ,[SalesPersonID]
	  ,isnull([TotalAmount],0) as TotalAmount
      ,isnull([PaidAmount],0) as PaidAmount
      ,isnull([Discount],0) as Discount
	  ,isnull([Balance],0) as Balance
FROM [dbo].[PurchaseHeader]
WHERE ID = {0} 