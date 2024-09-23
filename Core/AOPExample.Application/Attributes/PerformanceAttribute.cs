using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Application.Attributes
{
    public class PerformanceAttribute : AttributeBase
    {
        public int Interval { get; set; }
    }
}
