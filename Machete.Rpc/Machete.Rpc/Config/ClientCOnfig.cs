/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:30:44 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Configuration;

namespace Machete.Rpc.Config
{
    public class ClientConfig
    {
        public static int RpcPort = Convert.ToInt32(ConfigurationManager.AppSettings["rpc.server.port"]);

        public static string RpcHost = ConfigurationManager.AppSettings["rpc.server.host"].ToString();
    }
}
