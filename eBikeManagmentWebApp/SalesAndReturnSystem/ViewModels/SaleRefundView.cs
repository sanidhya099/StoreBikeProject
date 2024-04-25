using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAndReturnSystem.ViewModels
{
    public class SaleRefundsViewModel
    {

        public int SaleId { get; set; }
        public int EmployeeId { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
        public int DiscountPercent { get; set; }
    }
}
