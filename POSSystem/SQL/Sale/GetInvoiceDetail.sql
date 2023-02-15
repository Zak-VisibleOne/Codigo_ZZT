select d.ID,d.SrNo,s.StockDescription,d.Qty,d.SalePrice,d.Amount
from SaleDetail d inner join Stock s on d.StockID = s.ID
where d.RefHeaderID = {0} 
