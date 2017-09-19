using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rs232.Services
{
    public interface IRs232Service
    {
        List<string> GetPortNames();
    }
}
