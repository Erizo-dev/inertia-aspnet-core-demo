using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PingCrm.Host.Dto
{
    public class LinkDto
    {
        public string Url { get; set; } = null!;
        public string Label { get; set; } = null!;
        public bool Active { get; set; } = true;
    }
}