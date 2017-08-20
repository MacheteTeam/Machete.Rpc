using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Machete.Rpc.Config;
using Machete.Rpc.Netty;

namespace Machete.Rpc.Sample.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            NettyContainer.Client.Connect(ClientConfig.RpcHost, ClientConfig.RpcPort);

            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException; //UI异常捕获
            //RpcHub hub = new RpcHub();
            //ClientContainer.Client = hub.Connect(ClientConfig.RpcHost, ClientConfig.RpcPort);
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.InnerException?.Message ?? e.Exception.Message);
            e.Handled = true;
        }
    }
}
