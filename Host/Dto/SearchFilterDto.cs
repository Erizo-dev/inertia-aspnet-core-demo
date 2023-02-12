using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PingCrm.Host.Dto
{
    public class SearchFilterDto
    {
        public string? Search { get; set; }
        public string? Trashed { get; set; }
        public string? Role { get; set; }
    }
}