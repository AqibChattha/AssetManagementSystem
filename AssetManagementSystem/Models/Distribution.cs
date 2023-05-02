using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Models
{
    public class Distribution
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string Responsibility { get; set; }
        public string Place { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
