using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Rpc.Netty
{
    public class NettyContainer
    {
        public static DotNettyClient Client { set; get; } = new DotNettyClient();
    }
}
