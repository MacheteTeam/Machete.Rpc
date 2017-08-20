using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machete.Rpc.Netty;
using Machete.Rpc.Proxy;

namespace Machete.Rpc.Transport
{
    public interface ITransportClient
    {
        TransportMessage SendMessage(string message);
    }
}
