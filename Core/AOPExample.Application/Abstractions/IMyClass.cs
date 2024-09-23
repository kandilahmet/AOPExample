using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Application.Abstractions
{
    public interface IMyClass
    {
        public Task<int> MultiplyAsync(int num1,int num2);
        public int Add(int num1, int num2);
    }
}
