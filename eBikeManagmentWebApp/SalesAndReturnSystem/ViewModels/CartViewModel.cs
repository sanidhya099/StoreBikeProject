using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAndReturnSystem.ViewModels
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public int EmployeeId { get; set; }
        public string PaymentType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public List<SaleDetailView> Items { get; set; }


    }
}
