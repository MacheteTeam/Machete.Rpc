/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/17 22:53:10 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Machete.Rpc.Socket
{
    public class SyncTcpServer
    {
        /// <summary>  
        /// 接收到数据事件  
        /// </summary>  
        public event RequestHandler Handle;

        public static TcpListener TcpListener;

        /// <summary>
        /// 监听请求
        /// </summary>
        /// <param name="port"></param>
        public void Listen(int port)
        {
            //[1]TcpListener对Socket进行了封装，这各类会自己创建Socket对象  
            TcpListener = new TcpListener(IPAddress.Any, port);
            //[2]开始进行监听  
            TcpListener.Start();
            while (true)
            {
                //[3]等待客户端连接过来  
                TcpClient client = TcpListener.AcceptTcpClient();
                ClientContainer.TcpClients.Add(client);
                //创建消息服务线程对象  
                SocketThread handleThread = new SocketThread(client);
                handleThread.Handle += Handle;
                //把RpcNetThread类的ClientService方法委托给线程  
                Thread newthread = new Thread(handleThread.Execute);
                //启动消息服务线程  
                newthread.Start();
            }
        }
    }
}
