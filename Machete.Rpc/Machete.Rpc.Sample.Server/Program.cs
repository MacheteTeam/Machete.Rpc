using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Rpc.Sample.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["rpc.server.port"].ToString());
            RpcHub hub = new RpcHub();
            hub.Start(port);

            Console.WriteLine("服务已经启动 端口：" + port);

            Console.ReadKey();
        }
    }
}
