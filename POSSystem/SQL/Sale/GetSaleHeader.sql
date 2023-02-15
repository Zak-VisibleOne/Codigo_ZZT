SELECT [ID]
      ,CONVERT(char(10),isnull(SaleDate,''),126)  as SaleDate
      ,[InvoiceNo]
      ,[CustID]
      ,[SalesPersonID]
      ,isnull([TotalAmount],0) as TotalAmount
      ,isnull([PaidAmount],0) as PaidAmount
      ,isnull([Discount],0) as Discount
	  ,isnull([Balance],0) as Balance
  FROM [dbo].[SaleHeader]
WHERE ID = {0} 