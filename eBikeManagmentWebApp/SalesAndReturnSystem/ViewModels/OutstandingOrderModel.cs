using SalesAndReturnSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAndReturnSystem.ViewModels
{
    public class OutstandingOrderModel
    {
        public int JobID { get; set; }
        public CustomerVehicle Vehicle { get; set; }
    }
}
