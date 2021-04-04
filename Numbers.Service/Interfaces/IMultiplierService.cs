using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Numbers.Service.Interfaces
{
    public interface IMultiplierService
    {
        Task<int> MultiplyNumber(int multiplicand);
    }
}
