using SalesAndReturnSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAndReturnSystem.ViewModels
{
    public class ReturnData
    {
        public string Item { get; set; }
        public int OrgQty { get; set; }
        public decimal Price { get; set; }
        public int RtnQty { get; set; }
        public string Ref { get; set; }
        public int Qty { get; set; }
        public string Reason { get; set; }
        public Part Part { get; set; }
    }
}
