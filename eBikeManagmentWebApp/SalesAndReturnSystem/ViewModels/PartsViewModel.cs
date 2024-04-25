using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAndReturnSystem.ViewModels
{
    public class PartsViewModel
    {
        public int PartID { get; set; }
        public string Description { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
    }
}
