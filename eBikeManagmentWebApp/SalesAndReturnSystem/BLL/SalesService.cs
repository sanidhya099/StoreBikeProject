using SalesAndReturnSystem.DAL;
using SalesAndReturnSystem.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesAndReturnSystem.BLL
{
    public class SalesService
    {
        private readonly SalesAndReturnContext Repository;
        public SalesService(SalesAndReturnContext WFS_2590Context)
        {
            Repository = WFS_2590Context;
        }

        public void DeleteSaleDetail(SaleDetail detailpart)
        {
            Repository.SaleDetails.Remove(detailpart);
            Repository.SaveChanges();
        }

        public List<SaleDetail> GetAllSalesDetails()
        {
            return Repository.SaleDetails.ToList();
        }

        public List<Category>? GetCategories()
        {
            return Repository.Categories.ToList();
        }

        public Coupon? GetCoupon(string? couponCode)
        {
            if (!string.IsNullOrEmpty(couponCode))
            {
                var coupon = Repository.Coupons.Where(c => c.CouponIDValue == couponCode);
                if (coupon.Count() > 0)
                {
                    return coupon.First();
                }
                else return null;
            }
            else return null;
        }

        public Coupon GetCouponByID(int? appliedCouponId)
        {
            return Repository.Coupons.Where(c => c.CouponID == appliedCouponId).First();
        }

        public int? GetCouponID(int saleInvoiceID)
        {
            return Repository.Sales.Where(s => s.SaleID == saleInvoiceID).First().CouponID;
        }

        public List<SaleDetail> GetInvoiceItems(int saleInvoiceID)
        {
            return Repository.SaleDetails.Where(saleDetail => saleDetail.SaleID == saleInvoiceID).ToList();
        }

        public int GetLastSaleID()
        {
            return Repository.Sales.OrderBy(s => s.SaleID).Last().SaleID;
        }

        public Part GetPart(int partID)
        {
            return Repository.Parts.Where(part => part.PartID == partID).First();
        }

        public List<Part>? GetParts()
        {
            return Repository.Parts.ToList();
        }

        public Sale? GetSale(int saleInvoiceID)
        {
            return Repository.Sales.Where(s => s.SaleID == saleInvoiceID).First();
        }

        public SaleDetail GetSaleDetailByReturnItem(int returnItemID, int saleInvoiceID)
        {
            return Repository.SaleDetails.Where(sd => sd.PartID == returnItemID && sd.SaleID == saleInvoiceID).First<SaleDetail>();
        }

        public IEnumerable<SaleDetail> GetSaleDetails(int saleInvoiceID)
        {
            return Repository.SaleDetails.Where(sd => sd.SaleID == saleInvoiceID);
        }

        public void InsertNewSale(Sale newSale)
        {
            Repository.Add(newSale);
            Repository.SaveChanges();
        }

        public int InsertRefund(SaleRefund refund)
        {
            Repository.Add(refund);
            Repository.SaveChanges();
            return Repository.SaleRefunds.Max(r => r.SaleRefundID);
        }

        public void InsertRefundDetail(SaleRefundDetail refundDetail)
        {
            Repository.Add(refundDetail);
            Repository.SaveChanges();
        }

        public void UpdateSalesDetails(List<SaleDetail> salesDetailsDbContext)
        {
            foreach (var item in salesDetailsDbContext)
            {
                Repository.SaleDetails.Update(item);
            }
        }
    }
}
