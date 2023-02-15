SELECT [ID]
      ,[StockCode]
      ,[StockDescription]
      ,[ImageUrl]
      ,[CategoryID]
      ,[SalePrice]
      ,[PurchasePrice]
      ,[Remark]
  FROM [dbo].[Stock] WHERE ID = {0} 
