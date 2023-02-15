select h.id,c.CustName,h.InvoiceNo,CONVERT(char(10),isnull(SaleDate,''),103)  as SaleDate,
c.Address1,c.Phone1,t.TspName,isnull(h.Balance,0) as Balance 
from SaleHeader h left join Customer c on 
h.CustID = c.CustId left join Township t on c.TspId = t.TspId
where h.ID =  {0} 