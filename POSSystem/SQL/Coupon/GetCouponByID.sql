SELECT [ID]
      ,[CouponCode]
      ,[CouponName]
      ,[QRCodeImageUrl]
	  ,isnull(AvailableQty,0) as AvailableQty
	  ,CONVERT(char(10),isnull(StartDate,''),126)  as StartDate
	  ,CONVERT(char(10),isnull(EndDate,''),126)  as EndDate
      ,[DiscountAmount]
	   ,isnull([DiscountAmount],0) as [DiscountAmount]
FROM [dbo].[Coupon]
WHERE ID = {0} 