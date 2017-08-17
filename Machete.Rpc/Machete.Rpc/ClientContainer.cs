/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 20:37:41 
* clrversion :	4.0.30319.42000
******************************************************/

using System.Collections.Generic;
using System.Net.Sockets;

namespace Machete.Rpc
{
    public class ClientContainer
    {
        public static TcpClient Client;


        public static List<TcpClient> TcpClients = new List<TcpClient>();
    }
}
