using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Models
{
    public class PreviousComments
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public DateTime commentTimeStamp { get; set; }
        public string comments { get; set; }
    }
}
