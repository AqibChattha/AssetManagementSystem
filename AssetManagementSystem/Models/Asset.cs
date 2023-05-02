using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Specifications { get; set; }
        public DateTime ProcurementDate { get; set; }
        public string Colour { get; set; }
        public byte[] Image { get; set; }
        public decimal Price { get; set; }
        public string ConditionCategory { get; set; }
        public int Quantity { get; set; }
        public string MinuteSheetNumber { get; set; }
        public byte[] MinuteSheetDocument { get; set; }
        public string Comments { get; set; }

        private Distribution _distribution = new Distribution();
        public Distribution Distribution
        {
            get { return _distribution; }
            set { _distribution = value; }
        }
    }
}
