using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoMaker3000.Models
{
    public class Config
    {
        public Dictionary<int, double> ValuePerMusician { get; set; }
        public Dictionary<int, double> ExtraSalary { get; set; }
        //public double KilometerPrice { get; set; }
        public double GroupSavings { get; set; }
        public double ManagerSalary { get; set; }
    }
}
