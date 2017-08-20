using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Rpc.Transport
{
    public interface ITransportServer
    {
        Task Listen(int port);
    }
}
