using ContactBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Services.Interfaces
{
   public interface ITokenGeneratorService
    {
        Task<string> GenerateToken(User user);
    }
}
