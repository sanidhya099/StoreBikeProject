using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicingSystem.ViewModels
{
    public class ServiceItemModel
    {
        public decimal Hours { get; set; }
        public int StandardJobID { get; set; }
        public string Description { get; set; }
    }
}
