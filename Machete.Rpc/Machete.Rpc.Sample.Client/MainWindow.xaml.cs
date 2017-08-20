using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Machete.Rpc.Netty;
using Machete.Rpc.Proxy;
using Machete.Rpc.Sample.Service;

namespace Machete.Rpc.Sample.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IChatService chatService = InterfaceProxy.Resolve<IChatService>();
            string result = chatService.Hi("张三");
            MessageBox.Show(result);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IChatService chatService = InterfaceProxy.Resolve<IChatService>();
            string result = chatService.Hi("李四", "世界和平");
            MessageBox.Show(result);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            IChatService chatService = InterfaceProxy.Resolve<IChatService>();
            string result = chatService.Hello(1);
            string result1 = chatService.Hello(1.2);
            MessageBox.Show(result);
            MessageBox.Show(result1);
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            NettyContainer.Client.Close();
            Environment.Exit(0);
        }
    }
}
