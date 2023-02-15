SELECT dbo.PurchaseHead.ID, dbo.PurchaseHead.MemberCode, dbo.PurchaseHead.CouponCode,
dbo.PurchaseHead.ReceiptNumber, dbo.PurchaseItem.ItemName, 
dbo.PurchaseItem.Price, dbo.PurchaseItem.Qty, 
dbo.PurchaseItem.TotalPrice
FROM     dbo.PurchaseHead LEFT JOIN
dbo.PurchaseItem ON dbo.PurchaseHead.ID = dbo.PurchaseItem.HeadID