using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Machete.Rpc.Config;

namespace Machete.Rpc.Sample.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            RpcHub hub = new RpcHub();
            ClientContainer.Client = hub.Connect(ClientConfig.RpcHost, ClientConfig.RpcPort);
        }
    }
}
