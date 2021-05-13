using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blinky.Core.Services
{
    public interface IShortTokenGenerator : IDisposable
    {
        string GenerateShortToken(int tokenLength);

    }
}
