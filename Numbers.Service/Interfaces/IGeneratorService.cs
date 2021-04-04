using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Numbers.Service.Interfaces
{
    public interface IGeneratorService
    {
        Task<int> GenerateNumber();
    }
}
