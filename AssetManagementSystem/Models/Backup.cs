using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Models
{
    public class Backup
    {
        public string FileLocation { get; set; }
        public string Interval { get; set; }
        public DateTime LastBackup { get; set; }
    }
}
