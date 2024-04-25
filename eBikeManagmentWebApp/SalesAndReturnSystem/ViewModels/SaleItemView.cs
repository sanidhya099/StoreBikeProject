using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAndReturnSystem.ViewModels
{
    public class SaleItemView
    {
        public int EmployeeId { get; set; }
        public int CouponId { get; set; }
        public int DiscountPercent { get; set; }
        public string PaymentType { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
    }
}
