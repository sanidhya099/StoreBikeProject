using ServicingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicingSystem.ViewModels
{
    public class JobDetailModel
    {
        public decimal JobHours { get; set; }
        public decimal ShopRate { get; set; }
        public StandardJob? Service { get; set; }
        public string? Comments { get; set; }
    }
}
