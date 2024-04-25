using ServicingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicingSystem.ViewModels
{
    public class OutstandingOrderModel
    {
        public int JobID { get; set; }
        public CustomerVehicle Vehicle { get; set; }
    }
}
